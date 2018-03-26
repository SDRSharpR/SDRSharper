using System;

namespace SDRSharp.Radio
{
	public sealed class CpxFirFilter : IDisposable, IComplexFir
	{
		private const double Epsilon = 1E-06;

		private const int CircularBufferSize = 4;

		private unsafe Complex* _coeffPtr;

		private UnsafeBuffer _coeffBuffer;

		private unsafe Complex* _queuePtr;

		private UnsafeBuffer _queueBuffer;

		private int _queueSize;

		private int _offset;

		private bool _isSymmetric;

		private bool _isSparse;

		private bool _isFast;

		private int _fftSize = 4096;

		private UnsafeBuffer _segment;

		private unsafe Complex* _segPtr;

		private UnsafeBuffer _kernel;

		private unsafe Complex* _kerPtr;

		private UnsafeBuffer _overlap;

		private unsafe Complex* _olaPtr;

		private UnsafeBuffer _temp;

		private unsafe Complex* _tmpPtr;

		private Complex _acc = default(Complex);

		private Complex _tmp = default(Complex);

		private Complex _dif = default(Complex);

		private int _fftLen;

		public int Length => this._queueSize;

		public CpxFirFilter()
		{
		}

		public CpxFirFilter(float[] coefficients)
		{
			this.SetCoefficients(coefficients);
		}

		public CpxFirFilter(Complex[] coefficients)
		{
			this.SetCoefficients(coefficients);
		}

		~CpxFirFilter()
		{
			this.Dispose();
		}

		public unsafe void Dispose()
		{
			this._coeffBuffer = null;
			this._queueBuffer = null;
			this._coeffPtr = null;
			this._queuePtr = null;
			this._segment = null;
			this._segPtr = null;
			this._kernel = null;
			this._kerPtr = null;
			this._overlap = null;
			this._olaPtr = null;
			this._tmpPtr = null;
			GC.SuppressFinalize(this);
		}

		public void SetCoefficients(float[] coefficients)
		{
			Complex[] array = new Complex[coefficients.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Real = coefficients[i];
				array[i].Imag = 0f;
			}
			this.SetCoefficients(array);
		}

		public unsafe void SetCoefficients(Complex[] coefficients)
		{
			this._isFast = false;
			if (coefficients != null)
			{
				if (this._coeffBuffer == null || coefficients.Length != this._queueSize)
				{
					this._queueSize = coefficients.Length;
					this._offset = this._queueSize * 3;
					this._coeffBuffer = UnsafeBuffer.Create(this._queueSize, sizeof(Complex));
					this._coeffPtr = (Complex*)(void*)this._coeffBuffer;
					this._queueBuffer = UnsafeBuffer.Create(this._queueSize * 4, sizeof(Complex));
					this._queuePtr = (Complex*)(void*)this._queueBuffer;
				}
				for (int i = 0; i < this._queueSize; i++)
				{
					this._coeffPtr[i] = coefficients[i];
				}
				this._isSymmetric = true;
				this._isSparse = true;
				if (this._queueSize % 2 != 0)
				{
					int num = this._queueSize / 2;
					for (int j = 0; j < num; j++)
					{
						int num2 = this._queueSize - 1 - j;
						Complex.Sub(ref this._dif, this._coeffPtr[j], this._coeffPtr[num2]);
						if ((double)this._dif.Modulus() > 1E-06)
						{
							this._isSymmetric = false;
							this._isSparse = false;
							break;
						}
						if (j % 2 != 0)
						{
							this._isSparse = (this._coeffPtr[j].Real == 0f && this._coeffPtr[j].Imag == 0f && this._coeffPtr[num2].Real == 0f && this._coeffPtr[num2].Imag == 0f);
						}
					}
				}
				if (this._segment == null || this._segment.Length != this._fftSize)
				{
					this._segment = UnsafeBuffer.Create(this._fftSize, sizeof(Complex));
					this._segPtr = (Complex*)(void*)this._segment;
					this._kernel = UnsafeBuffer.Create(this._fftSize, sizeof(Complex));
					this._kerPtr = (Complex*)(void*)this._kernel;
					this._overlap = UnsafeBuffer.Create(this._fftSize, sizeof(Complex));
					this._olaPtr = (Complex*)(void*)this._overlap;
					this._temp = UnsafeBuffer.Create(this._fftSize, sizeof(Complex));
					this._tmpPtr = (Complex*)(void*)this._temp;
				}
				if (this._queueSize < 64)
				{
					this._fftLen = 128;
					goto IL_033c;
				}
				if (this._queueSize < 128)
				{
					this._fftLen = 256;
					goto IL_033c;
				}
				if (this._queueSize < 256)
				{
					this._fftLen = 512;
					goto IL_033c;
				}
				if (this._queueSize < 512)
				{
					this._fftLen = 1024;
					goto IL_033c;
				}
				if (this._queueSize < 1024)
				{
					this._fftLen = 2048;
					goto IL_033c;
				}
				if (this._queueSize < 2048)
				{
					this._fftLen = 4096;
					goto IL_033c;
				}
				throw new Exception("kernel too long");
			}
			return;
			IL_033c:
			CpxFirFilter.FillFft(this._kerPtr, this._fftLen, this._coeffPtr, this._queueSize);
			Fourier.ForwardTransform(this._kerPtr, this._fftLen, false);
			this._isFast = true;
		}

		public unsafe void Process(Complex* buffer, int length)
		{
			if (this._isFast)
			{
				this.FastConvolve(buffer, length);
			}
			else if (this._isSparse)
			{
				this.ProcessSparseSymmetric(buffer, length);
			}
			else if (this._isSymmetric)
			{
				this.ProcessSymmetric(buffer, length);
			}
			else
			{
				this.ProcessStandard(buffer, length);
			}
		}

		private unsafe void ProcessStandard(Complex* buffer, int length)
		{
			if (this._queueBuffer != null)
			{
				for (int i = 0; i < length; i++)
				{
					Complex* ptr = this._queuePtr + this._offset;
					*ptr = buffer[i];
					this._acc.Real = 0f;
					this._acc.Imag = 0f;
					int num = this._queueSize;
					Complex* ptr2 = ptr;
					Complex* ptr3 = this._coeffPtr;
					if (num >= 4)
					{
						do
						{
							Complex.Mul(ref this._acc, *ptr2, *ptr3);
							Complex.Mul(ref this._tmp, ptr2[1], ptr3[1]);
							Complex.Add(ref this._acc, this._tmp);
							Complex.Mul(ref this._tmp, ptr2[2], ptr3[2]);
							Complex.Add(ref this._acc, this._tmp);
							Complex.Mul(ref this._tmp, ptr2[3], ptr3[3]);
							Complex.Add(ref this._acc, this._tmp);
							ptr2 += 4;
							ptr3 += 4;
						}
						while ((num -= 4) >= 4);
					}
					while (num-- > 0)
					{
						ref Complex tmp = ref this._tmp;
						Complex* intPtr = ptr2;
						ptr2 = intPtr + 1;
						Complex c = *intPtr;
						Complex* intPtr2 = ptr3;
						ptr3 = intPtr2 + 1;
						Complex.Mul(ref tmp, c, *intPtr2);
						Complex.Add(ref this._acc, this._tmp);
					}
					if (--this._offset < 0)
					{
						this._offset = this._queueSize * 3;
						Utils.Memcpy(this._queuePtr + this._offset + 1, this._queuePtr, (this._queueSize - 1) * sizeof(Complex));
					}
					buffer[i] = this._acc;
				}
			}
		}

		private unsafe void ProcessSymmetric(Complex* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				Complex* ptr = this._queuePtr + this._offset;
				*ptr = buffer[i];
				this._acc.Real = 0f;
				this._acc.Imag = 0f;
				int num = this._queueSize / 2;
				int num2 = num;
				Complex* ptr2 = this._coeffPtr;
				Complex* ptr3 = ptr;
				Complex* ptr4 = ptr + this._queueSize - 1;
				if (num2 >= 4)
				{
					do
					{
						Complex.Mul(ref this._acc, *ptr2, *ptr3);
						Complex.Mul(ref this._tmp, ptr2[1], ptr3[1]);
						Complex.Add(ref this._acc, this._tmp);
						Complex.Mul(ref this._tmp, ptr2[2], ptr3[2]);
						Complex.Add(ref this._acc, this._tmp);
						Complex.Mul(ref this._tmp, ptr2[3], ptr3[3]);
						Complex.Add(ref this._acc, this._tmp);
						ptr2 += 4;
						ptr3 += 4;
						ptr4 -= 4;
					}
					while ((num2 -= 4) >= 4);
				}
				while (num2-- > 0)
				{
					ref Complex tmp = ref this._tmp;
					Complex* intPtr = ptr3;
					ptr3 = intPtr + 1;
					Complex c = *intPtr;
					Complex* intPtr2 = ptr4;
					ptr4 = intPtr2 - 1;
					Complex.Add(ref tmp, c, *intPtr2);
					ref Complex tmp2 = ref this._tmp;
					Complex* intPtr3 = ptr2;
					ptr2 = intPtr3 + 1;
					Complex.Mul(ref tmp2, *intPtr3);
					Complex.Add(ref this._acc, this._tmp);
				}
				Complex.Mul(ref this._tmp, ptr[num], this._coeffPtr[num]);
				Complex.Add(ref this._acc, this._tmp);
				if (--this._offset < 0)
				{
					this._offset = this._queueSize * 3;
					Utils.Memcpy(this._queuePtr + this._offset + 1, this._queuePtr, (this._queueSize - 1) * sizeof(Complex));
				}
				buffer[i] = this._acc;
			}
		}

		private unsafe void ProcessSparseSymmetric(Complex* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				Complex* ptr = this._queuePtr + this._offset;
				*ptr = buffer[i];
				this._acc.Real = 0f;
				this._acc.Imag = 0f;
				int num = this._queueSize / 2;
				int num2 = num;
				Complex* ptr2 = this._coeffPtr;
				Complex* ptr3 = ptr;
				Complex* ptr4 = ptr + this._queueSize - 1;
				if (num2 >= 8)
				{
					do
					{
						Complex.Mul(ref this._acc, *ptr2, *ptr3);
						Complex.Mul(ref this._tmp, ptr2[2], ptr3[2]);
						Complex.Add(ref this._acc, this._tmp);
						Complex.Mul(ref this._tmp, ptr2[4], ptr3[4]);
						Complex.Add(ref this._acc, this._tmp);
						Complex.Mul(ref this._tmp, ptr2[6], ptr3[6]);
						Complex.Add(ref this._acc, this._tmp);
						ptr2 += 8;
						ptr3 += 8;
						ptr4 -= 8;
					}
					while ((num2 -= 8) >= 8);
				}
				if (num2 >= 4)
				{
					Complex.Add(ref this._tmp, *ptr3, *ptr4);
					Complex.Mul(ref this._tmp, *ptr2);
					Complex.Add(ref this._acc, this._tmp);
					Complex.Add(ref this._tmp, ptr3[2], ptr4[-2]);
					Complex.Mul(ref this._tmp, ptr2[2]);
					Complex.Add(ref this._acc, this._tmp);
					ptr2 += 4;
					ptr3 += 4;
					ptr4 -= 4;
					num2 -= 4;
				}
				while (num2-- > 0)
				{
					ref Complex tmp = ref this._tmp;
					Complex* intPtr = ptr3;
					ptr3 = intPtr + 1;
					Complex c = *intPtr;
					Complex* intPtr2 = ptr4;
					ptr4 = intPtr2 - 1;
					Complex.Add(ref tmp, c, *intPtr2);
					ref Complex tmp2 = ref this._tmp;
					Complex* intPtr3 = ptr2;
					ptr2 = intPtr3 + 1;
					Complex.Mul(ref tmp2, *intPtr3);
					Complex.Add(ref this._acc, this._tmp);
				}
				Complex.Mul(ref this._tmp, ptr[num], this._coeffPtr[num]);
				Complex.Add(ref this._acc, this._tmp);
				if (--this._offset < 0)
				{
					this._offset = this._queueSize * 3;
					Utils.Memcpy(this._queuePtr + this._offset + 1, this._queuePtr, (this._queueSize - 1) * sizeof(Complex));
				}
				buffer[i] = this._acc;
			}
		}

		private unsafe void FastConvolve(Complex* buffer, int bufLen)
		{
			int num = (int)Math.Log((double)bufLen, 2.0);
			if (1 << num != bufLen)
			{
				throw new Exception("Length is not a power of 2");
			}
			int num2 = this._fftLen / 2;
			if (bufLen < num2)
			{
				num2 = bufLen;
			}
			int num3 = bufLen / num2;
			for (int i = 0; i < num3; i++)
			{
				CpxFirFilter.FillFft(this._segPtr, this._fftLen, buffer, num2);
				Fourier.ForwardTransform(this._segPtr, this._fftLen, false);
				for (int j = 0; j < this._fftLen; j++)
				{
					Complex.Mul(ref this._segPtr[j], this._kerPtr[j]);
				}
				Fourier.BackwardTransform(this._segPtr, this._fftLen);
				for (int k = 0; k < this._fftLen; k++)
				{
					if (k < num2)
					{
						Complex.Add(ref buffer[k], this._olaPtr[k], this._segPtr[k]);
					}
					else if (k < this._fftLen - num2)
					{
						Complex.Add(ref this._olaPtr[k - num2], this._olaPtr[k], this._segPtr[k]);
					}
					else
					{
						this._olaPtr[k - num2] = this._segPtr[k];
					}
				}
				buffer += num2;
			}
		}

		public unsafe static void FillFft(Complex* buffer, int bufLen, Complex* data, int datLen)
		{
			for (int i = 0; i < bufLen; i++)
			{
				if (i < datLen)
				{
					buffer[i] = data[i];
				}
				else
				{
					buffer[i].Real = 0f;
					buffer[i].Imag = 0f;
				}
			}
		}
	}
}
