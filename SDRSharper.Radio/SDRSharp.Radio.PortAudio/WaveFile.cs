using System;
using System.IO;
using System.Text;

namespace SDRSharp.Radio.PortAudio
{
	public sealed class WaveFile : IDisposable
	{
		private string _fileName = "";

		private int _duration;

		private int _size;

		private readonly Stream _stream;

		private bool _isPCM;

		private long _dataPos;

		private short _formatTag;

		private int _sampleRate;

		private int _avgBytesPerSec;

		private short _blockAlign;

		private short _bitsPerSample;

		private UnsafeBuffer _tempBuffer;

		private byte[] _temp;

		private unsafe byte* _tempPtr;

		public short FormatTag => this._formatTag;

		public int SampleRate => this._sampleRate;

		public int AvgBytesPerSec => this._avgBytesPerSec;

		public short BlockAlign => this._blockAlign;

		public short BitsPerSample => this._bitsPerSample;

		public string FileName => this._fileName;

		public int Duration => this._duration;

		public int Size => this._size;

		public long Position
		{
			get
			{
				return this._stream.Position - this._dataPos;
			}
			set
			{
				long num = value / this._blockAlign * this._blockAlign;
				this._stream.Seek(num + this._dataPos, SeekOrigin.Begin);
			}
		}

		public WaveFile(string fileName)
		{
			this._stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this._fileName = fileName;
			this.ReadHeader();
		}

		~WaveFile()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.Close();
			this._fileName = "";
			if (this._tempBuffer != null)
			{
				this._tempBuffer.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		public void Close()
		{
			if (this._stream != null)
			{
				this._stream.Close();
			}
		}

		private static string ReadChunk(BinaryReader reader)
		{
			byte[] array = new byte[4];
			reader.Read(array, 0, array.Length);
			return Encoding.ASCII.GetString(array);
		}

		private void ReadHeader()
		{
			BinaryReader binaryReader = new BinaryReader(this._stream);
			if (WaveFile.ReadChunk(binaryReader) != "RIFF")
			{
				throw new Exception("Invalid file format");
			}
			int num = binaryReader.ReadInt32();
			if (WaveFile.ReadChunk(binaryReader) != "WAVE")
			{
				throw new Exception("Invalid file format");
			}
			if (WaveFile.ReadChunk(binaryReader) != "fmt ")
			{
				throw new Exception("Invalid file format");
			}
			num = binaryReader.ReadInt32();
			if (num < 16)
			{
				throw new Exception("Invalid file format");
			}
			this._formatTag = binaryReader.ReadInt16();
			this._isPCM = (this._formatTag == 1);
			short num2 = binaryReader.ReadInt16();
			if (num2 != 2)
			{
				throw new Exception("Invalid file format");
			}
			this._sampleRate = binaryReader.ReadInt32();
			this._avgBytesPerSec = binaryReader.ReadInt32();
			this._blockAlign = binaryReader.ReadInt16();
			this._bitsPerSample = binaryReader.ReadInt16();
			for (num -= 16; num > 0; num--)
			{
				binaryReader.ReadByte();
			}
			string text = "";
			while (this._stream.Position < this._stream.Length)
			{
				text = WaveFile.ReadChunk(binaryReader);
				if (text == "data")
				{
					break;
				}
				num = binaryReader.ReadInt32();
				while (this._stream.Position < this._stream.Length && num > 0)
				{
					binaryReader.ReadByte();
					num--;
				}
			}
			if (this._stream.Position >= this._stream.Length)
			{
				throw new Exception("Invalid file format");
			}
			this._size = binaryReader.ReadInt32();
			this._dataPos = this._stream.Position;
			int num3 = num2 * this._sampleRate * this._bitsPerSample / 8;
			this._duration = this._size / num3;
		}

		public unsafe int Read(Complex* iqBuffer, int length, ref int iqPos)
		{
			if (this._temp == null || this._temp.Length != this._blockAlign * length)
			{
				this._temp = new byte[this._blockAlign * length];
				this._tempBuffer = UnsafeBuffer.Create(this._temp);
				this._tempPtr = (byte*)(void*)this._tempBuffer;
			}
			int num = this._stream.Read(this._temp, 0, this._tempBuffer.Length) / this._blockAlign;
			this.FillIQ(iqBuffer, num);
			return num;
		}

		private unsafe void FillIQ(Complex* iqPtr, int length)
		{
			if (this._isPCM)
			{
				if (this._blockAlign == 6)
				{
					Int24* ptr = (Int24*)this._tempPtr;
					for (int i = 0; i < length; i++)
					{
						Complex* intPtr = iqPtr;
						Int24* intPtr2 = ptr;
						ptr = intPtr2 + 1;
						intPtr->Real = (float)(*intPtr2) * 1.1920929E-07f;
						Complex* intPtr3 = iqPtr;
						Int24* intPtr4 = ptr;
						ptr = intPtr4 + 1;
						intPtr3->Imag = (float)(*intPtr4) * 1.1920929E-07f;
						iqPtr++;
					}
				}
				else if (this._blockAlign == 4)
				{
					short* ptr2 = (short*)this._tempPtr;
					for (int j = 0; j < length; j++)
					{
						Complex* intPtr5 = iqPtr;
						short* intPtr6 = ptr2;
						ptr2 = intPtr6 + 1;
						intPtr5->Real = (float)(*intPtr6) * 3.051851E-05f;
						Complex* intPtr7 = iqPtr;
						short* intPtr8 = ptr2;
						ptr2 = intPtr8 + 1;
						intPtr7->Imag = (float)(*intPtr8) * 3.051851E-05f;
						iqPtr++;
					}
				}
				else if (this._blockAlign == 2)
				{
					byte* ptr3 = this._tempPtr;
					for (int k = 0; k < length; k++)
					{
						Complex* intPtr9 = iqPtr;
						byte* intPtr10 = ptr3;
						ptr3 = intPtr10 + 1;
						intPtr9->Real = (float)(*intPtr10 - 128) * 0.0078125f;
						Complex* intPtr11 = iqPtr;
						byte* intPtr12 = ptr3;
						ptr3 = intPtr12 + 1;
						intPtr11->Imag = (float)(*intPtr12 - 128) * 0.0078125f;
						iqPtr++;
					}
				}
			}
			else
			{
				float* ptr4 = (float*)this._tempPtr;
				for (int l = 0; l < length; l++)
				{
					Complex* intPtr13 = iqPtr;
					float* intPtr14 = ptr4;
					ptr4 = intPtr14 + 1;
					intPtr13->Real = *intPtr14;
					Complex* intPtr15 = iqPtr;
					float* intPtr16 = ptr4;
					ptr4 = intPtr16 + 1;
					intPtr15->Imag = *intPtr16;
					iqPtr++;
				}
			}
		}
	}
}
