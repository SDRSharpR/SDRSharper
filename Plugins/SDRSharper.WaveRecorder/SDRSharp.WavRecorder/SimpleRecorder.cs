using SDRSharp.Radio;
using System;
using System.Threading;

namespace SDRSharp.WavRecorder
{
	public class SimpleRecorder : IDisposable
	{
		private const int DefaultAudioGain = 30;

		private static readonly int _bufferCount = 8;

		private readonly float _audioGain = (float)Math.Pow(3.0, 10.0);

		private readonly SharpEvent _bufferEvent = new SharpEvent(false);

		private readonly UnsafeBuffer[] _circularBuffers = new UnsafeBuffer[SimpleRecorder._bufferCount];

		private unsafe readonly Complex*[] _complexCircularBufferPtrs = new Complex*[SimpleRecorder._bufferCount];

		private unsafe readonly float*[] _floatCircularBufferPtrs = new float*[SimpleRecorder._bufferCount];

		private int _circularBufferTail;

		private int _circularBufferHead;

		private int _circularBufferLength;

		private volatile int _circularBufferUsedCount;

		private long _skippedBuffersCount;

		private bool _diskWriterRunning;

		private string _fileName;

		private double _sampleRate;

		private WavSampleFormat _wavSampleFormat;

		private SimpleWavWriter _wavWriter;

		private Thread _diskWriter;

		private readonly RecordingMode _recordingMode;

		private readonly RecordingIQObserver _iQObserver;

		private readonly RecordingAudioProcessor _audioProcessor;

		public bool IsRecording => this._diskWriterRunning;

		public bool IsStreamFull
		{
			get
			{
				if (this._wavWriter != null)
				{
					return this._wavWriter.IsStreamFull;
				}
				return false;
			}
		}

		public long BytesWritten
		{
			get
			{
				if (this._wavWriter != null)
				{
					return this._wavWriter.Length;
				}
				return 0L;
			}
		}

		public long SkippedBuffers
		{
			get
			{
				if (this._wavWriter != null)
				{
					return this._skippedBuffersCount;
				}
				return 0L;
			}
		}

		public RecordingMode Mode => this._recordingMode;

		public WavSampleFormat Format
		{
			get
			{
				return this._wavSampleFormat;
			}
			set
			{
				if (this._diskWriterRunning)
				{
					throw new ArgumentException("Format cannot be set while recording");
				}
				this._wavSampleFormat = value;
			}
		}

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				if (this._diskWriterRunning)
				{
					throw new ArgumentException("SampleRate cannot be set while recording");
				}
				this._sampleRate = value;
			}
		}

		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				if (this._diskWriterRunning)
				{
					throw new ArgumentException("FileName cannot be set while recording");
				}
				this._fileName = value;
			}
		}

		public unsafe SimpleRecorder(RecordingIQObserver iQObserver)
		{
			this._iQObserver = iQObserver;
			this._recordingMode = RecordingMode.Baseband;
		}

		public unsafe SimpleRecorder(RecordingAudioProcessor audioProcessor)
		{
			this._audioProcessor = audioProcessor;
			this._recordingMode = RecordingMode.Audio;
		}

		~SimpleRecorder()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.FreeBuffers();
		}

		public unsafe void IQSamplesIn(Complex* buffer, int length)
		{
			if (this._circularBufferLength != length)
			{
				this.FreeBuffers();
				this.CreateBuffers(length);
				this._circularBufferTail = 0;
				this._circularBufferHead = 0;
			}
			if (this._circularBufferUsedCount == SimpleRecorder._bufferCount)
			{
				this._skippedBuffersCount += 1L;
			}
			else
			{
				Utils.Memcpy(this._complexCircularBufferPtrs[this._circularBufferHead], buffer, length * sizeof(Complex));
				this._circularBufferHead++;
				this._circularBufferHead &= SimpleRecorder._bufferCount - 1;
				this._circularBufferUsedCount++;
				this._bufferEvent.Set();
			}
		}

		public unsafe void AudioSamplesIn(float* audio, int length)
		{
			int num = length / 2;
			if (this._circularBufferLength != num)
			{
				this.FreeBuffers();
				this.CreateBuffers(num);
				this._circularBufferTail = 0;
				this._circularBufferHead = 0;
			}
			if (this._circularBufferUsedCount == SimpleRecorder._bufferCount)
			{
				this._skippedBuffersCount += 1L;
			}
			else
			{
				Utils.Memcpy(this._floatCircularBufferPtrs[this._circularBufferHead], audio, length * 4);
				this._circularBufferHead++;
				this._circularBufferHead &= SimpleRecorder._bufferCount - 1;
				this._circularBufferUsedCount++;
				this._bufferEvent.Set();
			}
		}

		public unsafe void ScaleAudio(float* audio, int length)
		{
			for (int i = 0; i < length; i++)
			{
				audio[i] *= this._audioGain;
			}
		}

		private unsafe void DiskWriterThread()
		{
			if (this._recordingMode == RecordingMode.Baseband)
			{
				Console.WriteLine("DiskWriterThread for baseband started");
				this._iQObserver.IQReady += this.IQSamplesIn;
				this._iQObserver.Enabled = true;
			}
			else
			{
				Console.WriteLine("DiskWriterThread for audio started");
				this._audioProcessor.AudioReady += this.AudioSamplesIn;
				this._audioProcessor.Enabled = true;
			}
			while (this._diskWriterRunning && !this._wavWriter.IsStreamFull)
			{
				if (this._circularBufferTail == this._circularBufferHead)
				{
					this._bufferEvent.WaitOne();
				}
				if (this._diskWriterRunning && this._circularBufferTail != this._circularBufferHead)
				{
					if (this._recordingMode == RecordingMode.Audio)
					{
						this.ScaleAudio(this._floatCircularBufferPtrs[this._circularBufferTail], this._circularBuffers[this._circularBufferTail].Length * 2);
					}
					this._wavWriter.Write(this._floatCircularBufferPtrs[this._circularBufferTail], this._circularBuffers[this._circularBufferTail].Length);
					this._circularBufferUsedCount--;
					this._circularBufferTail++;
					this._circularBufferTail &= SimpleRecorder._bufferCount - 1;
				}
			}
			while (this._circularBufferTail != this._circularBufferHead)
			{
				if (this._floatCircularBufferPtrs[this._circularBufferTail] != null)
				{
					this._wavWriter.Write(this._floatCircularBufferPtrs[this._circularBufferTail], this._circularBuffers[this._circularBufferTail].Length);
				}
				this._circularBufferTail++;
				this._circularBufferTail &= SimpleRecorder._bufferCount - 1;
			}
			if (this._recordingMode == RecordingMode.Baseband)
			{
				this._iQObserver.Enabled = false;
				this._iQObserver.IQReady -= this.IQSamplesIn;
			}
			else
			{
				this._audioProcessor.Enabled = false;
				this._audioProcessor.AudioReady -= this.AudioSamplesIn;
			}
			this._diskWriterRunning = false;
			Console.WriteLine("DiskWriterThread stopped");
		}

		private void Flush()
		{
			if (this._wavWriter != null)
			{
				this._wavWriter.Close();
			}
		}

		private unsafe void CreateBuffers(int size)
		{
			for (int i = 0; i < SimpleRecorder._bufferCount; i++)
			{
				this._circularBuffers[i] = UnsafeBuffer.Create(size, sizeof(Complex));
				this._complexCircularBufferPtrs[i] = (Complex*)(void*)this._circularBuffers[i];
				this._floatCircularBufferPtrs[i] = (float*)(void*)this._circularBuffers[i];
			}
			this._circularBufferLength = size;
		}

		private unsafe void FreeBuffers() //EDITHERE
        {
			this._circularBufferLength = 0;
			for (int i = 0; i < SimpleRecorder._bufferCount; i++)
			{
				if (this._circularBuffers[i] != null)
				{
					this._circularBuffers[i].Dispose();
					this._circularBuffers[i] = null;
					this._complexCircularBufferPtrs[i] = null;
					this._floatCircularBufferPtrs[i] = null;
				}
			}
		}

		public void StartRecording()
		{
			if (this._diskWriter == null)
			{
				this._circularBufferHead = 0;
				this._circularBufferTail = 0;
				this._skippedBuffersCount = 0L;
				this._bufferEvent.Reset();
				this._wavWriter = new SimpleWavWriter(this._fileName, this._wavSampleFormat, (uint)this._sampleRate);
				this._wavWriter.Open();
				this._diskWriter = new Thread(this.DiskWriterThread);
				this._diskWriterRunning = true;
				this._diskWriter.Start();
			}
		}

		public void StopRecording()
		{
			this._diskWriterRunning = false;
			if (this._diskWriter != null)
			{
				this._bufferEvent.Set();
				this._diskWriter.Join();
			}
			this.Flush();
			this.FreeBuffers();
			this._diskWriter = null;
			this._wavWriter = null;
		}
	}
}
