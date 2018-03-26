using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SDRSharp.WavRecorder
{
	public class SimpleWavWriter
	{
		private const long MaxStreamLength = 2147483647L;

		private readonly string _filename;

		private readonly WavFormatHeader _format;

		private readonly WavSampleFormat _wavSampleFormat;

		private BinaryWriter _outputStream;

		private long _fileSizeOffs;

		private long _dataSizeOffs;

		private long _length;

		private byte[] _outputBuffer;

		private bool _isStreamFull;

		public WavSampleFormat SampleFormat => this._wavSampleFormat;

		public WavFormatHeader WaveFormat => this._format;

		public string FileName => this._filename;

		public long Length => this._length;

		public bool IsStreamFull => this._isStreamFull;

		public SimpleWavWriter(string filename, WavSampleFormat recordingFormat, uint sampleRate)
		{
			this._filename = filename;
			this._wavSampleFormat = recordingFormat;
			this._format = new WavFormatHeader(recordingFormat, 2, sampleRate);
		}

		public void Open()
		{
			if (this._outputStream == null)
			{
				this._outputStream = new BinaryWriter(File.Create(this._filename));
				this.WriteHeader();
				return;
			}
			throw new InvalidOperationException("Stream already open");
		}

		public void Close()
		{
			if (this._outputStream != null)
			{
				this.UpdateLength();
				this._outputStream.Flush();
				this._outputStream.Close();
				this._outputStream = null;
				this._outputBuffer = null;
				return;
			}
			throw new InvalidOperationException("Stream not open");
		}

		public unsafe void Write(float* data, int length)
		{
			if (this._outputStream != null)
			{
				switch (this._wavSampleFormat)
				{
				case WavSampleFormat.PCM8:
					this.WritePCM8(data, length);
					break;
				case WavSampleFormat.PCM16:
					this.WritePCM16(data, length);
					break;
				case WavSampleFormat.Float32:
					this.WriteFloat(data, length);
					break;
				}
				return;
			}
			throw new InvalidOperationException("Stream not open");
		}

		private unsafe void WritePCM8(float* data, int length)
		{
			if (this._outputBuffer == null || this._outputBuffer.Length != length * 2)
			{
				this._outputBuffer = null;
				this._outputBuffer = new byte[length * 2];
			}
			float* ptr = data;
			for (int i = 0; i < length; i++)
			{
				byte[] outputBuffer = this._outputBuffer;
				int num = i * 2;
				float* intPtr = ptr;
				ptr = intPtr + 1;
				outputBuffer[num] = (byte)(*intPtr * 128f + 128f);
				byte[] outputBuffer2 = this._outputBuffer;
				int num2 = i * 2 + 1;
				float* intPtr2 = ptr;
				ptr = intPtr2 + 1;
				outputBuffer2[num2] = (byte)(*intPtr2 * 128f + 128f);
			}
			this.WriteStream(this._outputBuffer);
		}

		private unsafe void WritePCM16(float* data, int length)
		{
			if (this._outputBuffer == null || this._outputBuffer.Length != length * 2 * 2)
			{
				this._outputBuffer = null;
				this._outputBuffer = new byte[length * 2 * 2];
			}
			float* ptr = data;
			for (int i = 0; i < length; i++)
			{
				float* intPtr = ptr;
				ptr = intPtr + 1;
				short num = (short)(*intPtr * 32767f);
				float* intPtr2 = ptr;
				ptr = intPtr2 + 1;
				short num2 = (short)(*intPtr2 * 32767f);
				this._outputBuffer[i * 4] = (byte)(num & 0xFF);
				this._outputBuffer[i * 4 + 1] = (byte)(num >> 8);
				this._outputBuffer[i * 4 + 2] = (byte)(num2 & 0xFF);
				this._outputBuffer[i * 4 + 3] = (byte)(num2 >> 8);
			}
			this.WriteStream(this._outputBuffer);
		}

		private unsafe void WriteFloat(float* data, int length)
		{
			if (this._outputBuffer == null || this._outputBuffer.Length != length * 4 * 2)
			{
				this._outputBuffer = null;
				this._outputBuffer = new byte[length * 4 * 2];
			}
			Marshal.Copy((IntPtr)(void*)data, this._outputBuffer, 0, this._outputBuffer.Length);
			this.WriteStream(this._outputBuffer);
		}

		private void WriteStream(byte[] data)
		{
			if (this._outputStream != null)
			{
				int num = (int)Math.Min(2147483647 - this._outputStream.BaseStream.Length, data.Length);
				this._outputStream.Write(data, 0, num);
				this._length += num;
				this.UpdateLength();
				this._isStreamFull = (this._outputStream.BaseStream.Length >= 2147483647);
			}
		}

		private void WriteHeader()
		{
			if (this._outputStream != null)
			{
				this.WriteTag("RIFF");
				this._fileSizeOffs = this._outputStream.BaseStream.Position;
				this._outputStream.Write(0u);
				this.WriteTag("WAVE");
				this.WriteTag("fmt ");
				this._outputStream.Write(16u);
				this._outputStream.Write(this._format.FormatTag);
				this._outputStream.Write(this._format.Channels);
				this._outputStream.Write(this._format.SamplesPerSec);
				this._outputStream.Write(this._format.AvgBytesPerSec);
				this._outputStream.Write(this._format.BlockAlign);
				this._outputStream.Write(this._format.BitsPerSample);
				this.WriteTag("data");
				this._dataSizeOffs = this._outputStream.BaseStream.Position;
				this._outputStream.Write(0u);
			}
		}

		private void UpdateLength()
		{
			if (this._outputStream != null)
			{
				this._outputStream.Seek((int)this._fileSizeOffs, SeekOrigin.Begin);
				this._outputStream.Write((uint)(this._outputStream.BaseStream.Length - 8));
				this._outputStream.Seek((int)this._dataSizeOffs, SeekOrigin.Begin);
				this._outputStream.Write((uint)this._length);
				this._outputStream.BaseStream.Seek(0L, SeekOrigin.End);
			}
		}

		private void WriteTag(string tag)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(tag);
			this._outputStream.Write(bytes, 0, bytes.Length);
		}
	}
}
