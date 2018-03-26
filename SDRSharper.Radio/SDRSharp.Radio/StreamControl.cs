using SDRSharp.Radio.PortAudio;
using System;
using System.IO;
using System.Threading;

namespace SDRSharp.Radio
{
	public sealed class StreamControl : IDisposable
	{
		private const int WaveBufferSize = 65536;

		private const int MaxDecimationFactor = 1024;

		private static readonly int _processorCount = Utils.ProcessorCount;

		private int _minOutputSampleRate = Utils.GetIntSetting("minOutputSampleRate", 18000);

		private unsafe float* _dspOutPtr;

		private UnsafeBuffer _dspOutBuffer;

		private unsafe Complex* _iqInPtr;

		private UnsafeBuffer _iqInBuffer;

		private unsafe Complex* _dspInPtr;

		private UnsafeBuffer _dspInBuffer;

		private WavePlayer _wavePlayer;

		private WaveRecorder _waveRecorder;

		private WaveDuplex _waveDuplex;

		private WaveFile _waveFile;

		private string _fileName;

		private ComplexFifoStream _iqStream;

		private FloatFifoStream _audioStream;

		private Thread _waveReadThread;

		private Thread _dspThread;

		private DateTime _waveStart;

		private float _audioGain;

		private float _outputGain;

		private int _inputDevice;

		private double _inputSampleRate;

		private int _inputBufferSize;

		private int _bufferSizeInMs;

		private int _outputDevice;

		private double _outputSampleRate;

		private int _outputBufferSize;

		private double _soundCardRatio = 1.0;

		private int _decimationStageCount;

		private bool _swapIQ;

		private bool _useASIO;

		private InputType _inputType;

		private IFrontendController _frontend;

		private bool _isBuffering;

		private float _clipLevel;

		public DateTime WaveStart
		{
			get
			{
				return this._waveStart;
			}
			set
			{
				this._waveStart = value;
			}
		}

		public string FileName => this._fileName;

		public WaveFile WaveFile => this._waveFile;

		public int MinOutputSampleRate
		{
			get
			{
				return this._minOutputSampleRate;
			}
			set
			{
				this._minOutputSampleRate = value;
			}
		}

		public double SoundCardRatio => this._soundCardRatio;

		public int AudioStreamSize
		{
			get
			{
				if (this._audioStream == null)
				{
					return 0;
				}
				return this._audioStream.Length;
			}
		}

		public float AudioGain
		{
			get
			{
				return this._audioGain;
			}
			set
			{
				this._audioGain = value;
				this._outputGain = (float)Math.Pow((double)value / 10.0, 10.0);
				this._outputGain = (float)Math.Pow(10.0, (double)value / 10.0);
			}
		}

		public bool SwapIQ
		{
			get
			{
				return this._swapIQ;
			}
			set
			{
				this._swapIQ = value;
			}
		}

		public bool UseASIO
		{
			get
			{
				return this._useASIO;
			}
			set
			{
				this._useASIO = value;
			}
		}

		public double SampleRate => this._inputSampleRate;

		public bool IsPlaying => this._inputSampleRate != 0.0;

		public bool IsBuffering
		{
			get
			{
				return this._isBuffering;
			}
			set
			{
				this._isBuffering = false;
			}
		}

		public int BufferSize => this._inputBufferSize;

		public int BufferSizeInMs => this._bufferSizeInMs;

		public int DecimationStageCount => this._decimationStageCount;

		public InputType InputType => this._inputType;

		public float ClipLevel => this._clipLevel;

		public event ProcessBufferDelegate ProcessBufferPtr;

		public StreamControl()
		{
			this.AudioGain = 0f;
		}

		~StreamControl()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.Stop();
			GC.SuppressFinalize(this);
		}

		private unsafe void DuplexFiller(float* buffer, int frameCount)
		{
			if (this._dspInBuffer == null || this._dspInBuffer.Length != frameCount)
			{
				this._dspInBuffer = UnsafeBuffer.Create(frameCount, sizeof(Complex));
				this._dspInPtr = (Complex*)(void*)this._dspInBuffer;
			}
			if (this._dspOutBuffer == null || this._dspOutBuffer.Length != this._dspInBuffer.Length * 2)
			{
				this._dspOutBuffer = UnsafeBuffer.Create(this._dspInBuffer.Length * 2, 4);
				this._dspOutPtr = (float*)(void*)this._dspOutBuffer;
			}
			Utils.Memcpy(this._dspInPtr, buffer, frameCount * sizeof(Complex));
			this.ProcessBuffer();
			this.ScaleBuffer(this._dspOutPtr, this._dspOutBuffer.Length);
			Utils.Memcpy(buffer, this._dspOutPtr, this._dspOutBuffer.Length * 4);
		}

		private unsafe void PlayerFiller(float* buffer, int frameCount)
		{
			int num = frameCount * 2;
			int num2 = this._audioStream.Read(buffer, num);
			this.ScaleBuffer(buffer, num2);
			this._clipLevel = 0f;
			for (int i = 0; i < num2; i++)
			{
				this._clipLevel = Math.Max(this._clipLevel, Math.Abs(buffer[i]));
			}
			for (int j = num2; j < num; j++)
			{
				buffer[j] = 0f;
			}
		}

		private unsafe void RecorderFiller(float* buffer, int frameCount)
		{
			if (this._iqStream.Length <= this._inputBufferSize * 2)
			{
				if (this._iqInBuffer == null || this._iqInBuffer.Length != frameCount)
				{
					this._iqInBuffer = UnsafeBuffer.Create(frameCount, sizeof(Complex));
					this._iqInPtr = (Complex*)(void*)this._iqInBuffer;
				}
				Utils.Memcpy(this._iqInPtr, buffer, frameCount * sizeof(Complex));
				this._iqStream.Write(this._iqInPtr, frameCount);
			}
		}

		private unsafe void FrontendFiller(IFrontendController sender, Complex* samples, int len)
		{
			if (this._iqStream.Length < this._inputBufferSize * 4)
			{
				this._iqStream.Write(samples, len);
			}
		}

		private unsafe void WaveFileFiller()
		{
			Complex[] array = new Complex[65536];
			int num = 0;
			Complex[] array2 = array;
			fixed (Complex* ptr = array2)
			{
				Console.WriteLine("WaveFiller started");
				while (this.IsPlaying)
				{
					if (this._iqStream.Length < this._inputBufferSize * 4)
					{
						int num2 = this._waveFile.Read(ptr, array.Length, ref num);
						this._iqStream.Write(ptr, num2);
						if (num2 < array.Length)
						{
							string fileName = this._waveFile.FileName;
							int num3 = fileName.ToLower().LastIndexOf(".wav");
							if (int.TryParse(fileName.Substring(num3 - 2, 2), out int num4))
							{
								num3 -= 2;
							}
							string str = fileName.Substring(0, num3);
							int num5 = ++num4;
							string text = str + num5.ToString("00") + ".wav";
							if (!File.Exists(text))
							{
								text = this._fileName;
								this._waveStart = DateTime.Now;
							}
							this._waveFile.Close();
							this._waveFile.Dispose();
							this._waveFile = new WaveFile(text);
						}
					}
					else
					{
						Thread.Sleep(2);
					}
				}
				Console.WriteLine("WaveFiller stopped");
			}
		}

		private unsafe void ScaleBuffer(float* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				buffer[i] *= this._outputGain;
			}
		}

		private unsafe void DSPProc()
		{
			if (this._dspInBuffer == null || this._dspInBuffer.Length != this._inputBufferSize)
			{
				this._dspInBuffer = UnsafeBuffer.Create(this._inputBufferSize, sizeof(Complex));
				this._dspInPtr = (Complex*)(void*)this._dspInBuffer;
			}
			if (this._dspOutBuffer == null || this._dspOutBuffer.Length != this._outputBufferSize)
			{
				this._dspOutBuffer = UnsafeBuffer.Create(this._outputBufferSize, 4);
				this._dspOutPtr = (float*)(void*)this._dspOutBuffer;
			}
			Console.WriteLine("DSPPROC started");
			while (this.IsPlaying)
			{
				int num = 0;
				while (this.IsPlaying && num < this._dspInBuffer.Length)
				{
					int count = this._dspInBuffer.Length - num;
					num += this._iqStream.Read(this._dspInPtr, num, count);
				}
				int count2 = this.ProcessBuffer();
				this._audioStream.Write(this._dspOutPtr, count2);
			}
			this._isBuffering = false;
			Console.WriteLine("DSPPROC stopped");
		}

		private unsafe int ProcessBuffer()
		{
			if (this.ProcessBufferPtr == null)
			{
				return 0;
			}
			if (this._swapIQ)
			{
				this.swapIQBuffer();
			}
			this._isBuffering = true;
            var x = this.ProcessBufferPtr(this._dspInPtr, this._dspOutPtr, this._dspInBuffer.Length, this._dspOutBuffer.Length);
			return x;
		}

		private unsafe void swapIQBuffer()
		{
			for (int i = 0; i < this._dspInBuffer.Length; i++)
			{
				float real = this._dspInPtr[i].Real;
				this._dspInPtr[i].Real = this._dspInPtr[i].Imag;
				this._dspInPtr[i].Imag = real;
			}
		}

		public void Stop()
		{
			if (this._inputType == InputType.Plugin && this._frontend != null)
			{
				this._frontend.Stop();
				this._frontend = null;
			}
			if (this._wavePlayer != null)
			{
				this._wavePlayer.Dispose();
				this._wavePlayer = null;
			}
			if (this._waveRecorder != null)
			{
				this._waveRecorder.Dispose();
				this._waveRecorder = null;
			}
			if (this._waveDuplex != null)
			{
				this._waveDuplex.Dispose();
				this._waveDuplex = null;
			}
			this._inputSampleRate = 0.0;
			if (this._waveReadThread != null)
			{
				this._waveReadThread.Join();
				this._waveReadThread = null;
			}
			if (this._iqStream != null)
			{
				this._iqStream.Close();
			}
			if (this._audioStream != null)
			{
				this._audioStream.Close();
			}
			if (this._dspThread != null)
			{
				this._dspThread.Join();
				this._dspThread = null;
			}
			if (this._waveFile != null)
			{
				this._waveFile.Dispose();
				this._waveFile = null;
			}
			if (this._iqStream != null)
			{
				this._iqStream.Dispose();
				this._iqStream = null;
			}
			this._audioStream = null;
			this._dspOutBuffer = null;
			this._iqInBuffer = null;
		}

		public unsafe void Play()
		{
			if (this._wavePlayer == null && this._waveDuplex == null)
			{
				if (this._inputType != InputType.WaveFile)
				{
					this._waveStart = DateTime.Now.AddYears(1);
				}
				double sampleRate = this._outputSampleRate;
				int framesPerBuffer = this._outputBufferSize / 2;
				if (this._soundCardRatio > 1.0)
				{
					sampleRate = (double)this._minOutputSampleRate;
					framesPerBuffer = (int)((double)this._outputBufferSize / (2.0 * this._soundCardRatio));
				}
				switch (this._inputType)
				{
				case InputType.SoundCard:
					if (this._inputDevice == this._outputDevice)
					{
						this._waveDuplex = new WaveDuplex(this._inputDevice, this._inputSampleRate, this._inputBufferSize, this.DuplexFiller);
					}
					else
					{
						this._iqStream = new ComplexFifoStream(BlockMode.BlockingRead);
						ComplexFifoStream iqStream2 = this._iqStream;
						bool read2 = false;
						iqStream2.SetLog("_iqStream", 20000, read2, true);
						this._audioStream = new FloatFifoStream(BlockMode.BlockingWrite, this._outputBufferSize);
						Console.WriteLine("_audioStream created with size " + this._outputBufferSize.ToString());
						FloatFifoStream audioStream3 = this._audioStream;
						bool write3 = false;
						audioStream3.SetLog("_audioStream", this._outputBufferSize, true, write3);
						this._waveRecorder = new WaveRecorder(this._inputDevice, this._inputSampleRate, this._inputBufferSize, this.RecorderFiller);
						this._wavePlayer = new WavePlayer(this._outputDevice, sampleRate, framesPerBuffer, this.PlayerFiller);
						this._dspThread = new Thread(this.DSPProc);
						this._dspThread.Start();
					}
					break;
				case InputType.WaveFile:
				{
					this._iqStream = new ComplexFifoStream(BlockMode.BlockingRead);
					this._iqStream.SetLog("_iqStream", 20000, true, true);
					this._audioStream = new FloatFifoStream(BlockMode.BlockingWrite, this._outputBufferSize * 2);
					Console.WriteLine("_audioStream created with size " + (this._outputBufferSize * 2).ToString());
					FloatFifoStream audioStream2 = this._audioStream;
					bool write2 = false;
					audioStream2.SetLog("_audioStream", this._outputBufferSize, true, write2);
					this._wavePlayer = new WavePlayer(this._outputDevice, sampleRate, framesPerBuffer, this.PlayerFiller);
					this._waveReadThread = new Thread(this.WaveFileFiller);
					this._waveReadThread.Start();
					this._dspThread = new Thread(this.DSPProc);
					this._dspThread.Start();
					break;
				}
				case InputType.Plugin:
				{
					this._iqStream = new ComplexFifoStream(BlockMode.BlockingRead);
					ComplexFifoStream iqStream = this._iqStream;
					bool read = false;
					iqStream.SetLog("_iqStream", 20000, read, true);
					this._audioStream = new FloatFifoStream(BlockMode.BlockingWrite, this._outputBufferSize * 2);
					Console.WriteLine("_audioStream created with size " + (this._outputBufferSize * 2).ToString());
					FloatFifoStream audioStream = this._audioStream;
					bool write = false;
					audioStream.SetLog("_audioStream", 0, true, write);
					this._wavePlayer = new WavePlayer(this._outputDevice, sampleRate, framesPerBuffer, this.PlayerFiller);
					this._frontend.Start(this.FrontendFiller);
					this._dspThread = new Thread(this.DSPProc);
					this._dspThread.Start();
					break;
				}
				}
			}
		}

		public void OpenSoundDevice(int inputDevice, int outputDevice, double inputSampleRate, int latency)
		{
			this.Stop();
			this._inputType = InputType.SoundCard;
			this._inputDevice = inputDevice;
			this._outputDevice = outputDevice;
			this._inputSampleRate = inputSampleRate;
			this._bufferSizeInMs = latency;
			this._inputBufferSize = (int)((double)this._bufferSizeInMs * this._inputSampleRate / 1000.0);
			if (this._inputDevice == this._outputDevice)
			{
				this._decimationStageCount = 0;
				this._inputBufferSize = this._inputBufferSize / StreamControl._processorCount * StreamControl._processorCount;
				int num = (int)Math.Log((double)this._inputBufferSize, 2.0);
				this._inputBufferSize = (int)Math.Pow(2.0, (double)num);
				this._bufferSizeInMs = (int)Math.Round((double)this._inputBufferSize / this._inputSampleRate * 1000.0);
				this._outputSampleRate = this._inputSampleRate;
				this._outputBufferSize = this._inputBufferSize * 2;
				this._soundCardRatio = 1.0;
			}
			else
			{
				this._decimationStageCount = this.GetDecimationStageCount();
				int num2 = (int)Math.Pow(2.0, (double)this._decimationStageCount);
				this._inputBufferSize = this._inputBufferSize / num2 * num2;
				this._inputBufferSize = this._inputBufferSize / StreamControl._processorCount * StreamControl._processorCount;
				int num3 = (int)Math.Log((double)this._inputBufferSize, 2.0);
				this._inputBufferSize = (int)Math.Pow(2.0, (double)num3);
				this._bufferSizeInMs = (int)Math.Round((double)this._inputBufferSize / this._inputSampleRate * 1000.0);
				this._outputSampleRate = this._inputSampleRate / (double)num2;
				this._outputBufferSize = this._inputBufferSize / num2 * 2;
				if (this._outputSampleRate > 192000.0)
				{
					this._soundCardRatio = this._outputSampleRate / 192000.0;
				}
				else
				{
					this._soundCardRatio = (this._useASIO ? (this._outputSampleRate / (double)this._minOutputSampleRate) : 1.0);
				}
			}
			Console.WriteLine("_inputBuffersize (complex) set to " + (Utils.FastConvolve ? "power of 2 : " : ": ") + this._inputBufferSize.ToString());
			Console.WriteLine("_outputBuffersize (real) set to : " + this._outputBufferSize.ToString());
			Console.WriteLine("_soundCardRatio set to : " + this._soundCardRatio.ToString());
		}

		public int OpenFile(string filename, int outputDevice, int latency)
		{
			this.Stop();
			try
			{
				this._inputType = InputType.WaveFile;
				this._fileName = filename;
				this._waveFile = new WaveFile(filename);
				this._waveStart = DateTime.Now;
				this._outputDevice = outputDevice;
				this._bufferSizeInMs = latency;
				this._inputSampleRate = (double)this._waveFile.SampleRate;
				this._decimationStageCount = this.GetDecimationStageCount();
				this._inputBufferSize = (int)((double)this._bufferSizeInMs * this._inputSampleRate / 1000.0);
				int num = (int)Math.Pow(2.0, (double)this._decimationStageCount);
				this._inputBufferSize = this._inputBufferSize / num * num;
				this._inputBufferSize = this._inputBufferSize / StreamControl._processorCount * StreamControl._processorCount;
				int num2 = (int)Math.Log((double)this._inputBufferSize, 2.0);
				this._inputBufferSize = (int)Math.Pow(2.0, (double)num2);
				this._bufferSizeInMs = (int)Math.Round((double)this._inputBufferSize / this._inputSampleRate * 1000.0);
				this._outputSampleRate = this._inputSampleRate / (double)num;
				this._outputBufferSize = this._inputBufferSize / num * 2;
				if (this._outputSampleRate > 192000.0)
				{
					this._soundCardRatio = this._outputSampleRate / 192000.0;
				}
				else
				{
					this._soundCardRatio = (this._useASIO ? (this._outputSampleRate / (double)this._minOutputSampleRate) : 1.0);
				}
				Console.WriteLine("_inputBuffersize (complex) set to " + (Utils.FastConvolve ? "power of 2 : " : ": ") + this._inputBufferSize.ToString());
				Console.WriteLine("_outputBuffersize (real) set to : " + this._outputBufferSize.ToString());
				Console.WriteLine("_soundCardRatio set to : " + this._soundCardRatio.ToString());
				return this._waveFile.Duration;
			}
			catch
			{
				this.Stop();
				throw;
			}
		}

		public void OpenPlugin(IFrontendController frontend, int outputDevice, int latency)
		{
			this.Stop();
			try
			{
				this._inputType = InputType.Plugin;
				this._frontend = frontend;
				this._inputSampleRate = this._frontend.Samplerate;
				this._outputDevice = outputDevice;
				this._bufferSizeInMs = latency;
				this._inputBufferSize = (int)((double)this._bufferSizeInMs * this._inputSampleRate / 1000.0);
				this._decimationStageCount = this.GetDecimationStageCount();
				int num = (int)Math.Pow(2.0, (double)this._decimationStageCount);
				this._inputBufferSize = this._inputBufferSize / num * num;
				this._inputBufferSize = this._inputBufferSize / StreamControl._processorCount * StreamControl._processorCount;
				int num2 = (int)Math.Log((double)this._inputBufferSize, 2.0);
				this._inputBufferSize = (int)Math.Pow(2.0, (double)num2);
				this._bufferSizeInMs = (int)Math.Round((double)this._inputBufferSize / this._inputSampleRate * 1000.0);
				this._outputSampleRate = this._inputSampleRate / (double)num;
				this._outputBufferSize = this._inputBufferSize / num * 2;
				if (this._outputSampleRate > 192000.0)
				{
					this._soundCardRatio = this._outputSampleRate / 192000.0;
				}
				else
				{
					this._soundCardRatio = (this._useASIO ? (this._outputSampleRate / (double)this._minOutputSampleRate) : 1.0);
				}
				Console.WriteLine("_inputBuffersize (complex) set to " + (Utils.FastConvolve ? "power of 2 : " : ": ") + this._inputBufferSize.ToString());
				Console.WriteLine("_outputBuffersize (real) set to : " + this._outputBufferSize.ToString());
				Console.WriteLine("_soundCardRatio set to : " + this._soundCardRatio.ToString());
			}
			catch
			{
				this.Stop();
				throw;
			}
		}

		private int GetDecimationStageCount()
		{
			if (this._inputSampleRate <= (double)this._minOutputSampleRate)
			{
				return 0;
			}
			int num = 1024;
			while (this._inputSampleRate < (double)(this._minOutputSampleRate * num) && num > 0)
			{
				num /= 2;
			}
			return (int)Math.Log((double)num, 2.0);
		}
	}
}
