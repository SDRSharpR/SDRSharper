using System;

namespace SDRSharp.Radio
{
	public sealed class FirFilter : IDisposable, IFilter
	{
		private const double Epsilon = 1E-06;

		private const int CircularBufferSize = 2;

		private unsafe float* _coeffPtr;

		private UnsafeBuffer _coeffBuffer;

		private unsafe float* _queuePtr;

		private UnsafeBuffer _queueBuffer;

		private int _queueSize;

		private int _offset;

		private bool _isSymmetric;

		private bool _isHalfBand;

		private int _decimationFactor;

		public int Length => this._queueSize;

		public FirFilter()
			: this(new float[0])
		{
		}

		public FirFilter(float[] coefficients)
			: this(coefficients, 1)
		{
		}

		public FirFilter(float[] coefficients, int decimationFactor)
		{
			this.SetCoefficients(coefficients);
			if (this._decimationFactor != 0 && this._decimationFactor != decimationFactor)
			{
				throw new ArgumentException("This decimation factor cannot be used with a half band filter", "decimationFactor");
			}
			if (decimationFactor <= 0)
			{
				throw new ArgumentException("The decimation factor must be greater than zero", "decimationFactor");
			}
			this._decimationFactor = decimationFactor;
		}

		~FirFilter()
		{
			this.Dispose();
		}

		public unsafe void Dispose()
		{
			this._coeffBuffer = null;
			this._queueBuffer = null;
			this._coeffPtr = null;
			this._queuePtr = null;
			GC.SuppressFinalize(this);
		}

		public unsafe void SetCoefficients(float[] coefficients)
		{
			if (coefficients != null)
			{
				if (this._coeffBuffer == null || coefficients.Length != this._queueSize)
				{
					this._queueSize = coefficients.Length;
					this._offset = this._queueSize;
					this._coeffBuffer = UnsafeBuffer.Create(this._queueSize, 4);
					this._coeffPtr = (float*)(void*)this._coeffBuffer;
					this._queueBuffer = UnsafeBuffer.Create(this._queueSize * 2, 4);
					this._queuePtr = (float*)(void*)this._queueBuffer;
				}
				for (int i = 0; i < this._queueSize; i++)
				{
					this._coeffPtr[i] = coefficients[i];
				}
				if (this._queueSize % 2 != 0)
				{
					this._isSymmetric = true;
					this._isHalfBand = true;
					int num = this._queueSize / 2;
					for (int j = 0; j < num; j++)
					{
						int num2 = this._queueSize - 1 - j;
						if ((double)Math.Abs(this._coeffPtr[j] - this._coeffPtr[num2]) > 1E-06)
						{
							this._isSymmetric = false;
							this._isHalfBand = false;
							break;
						}
						if (j % 2 != 0)
						{
							this._isHalfBand = (this._coeffPtr[j] == 0f && this._coeffPtr[num2] == 0f);
						}
					}
					if (this._isHalfBand)
					{
						this._decimationFactor = 2;
					}
				}
			}
		}

		private unsafe void ProcessSymmetricKernel(float* buffer, int length)
		{
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float* ptr = this._queuePtr + this._offset;
				int num3 = 0;
				int num4 = num + this._decimationFactor - 1;
				while (num3 < this._decimationFactor)
				{
					ptr[num3] = buffer[num4];
					num3++;
					num4--;
				}
				float num5 = 0f;
				int num6 = this._queueSize / 2;
				int num7 = num6;
				float* ptr2 = this._coeffPtr;
				float* ptr3 = ptr;
				float* ptr4 = ptr + this._queueSize - 1;
				if (num7 >= 4)
				{
					do
					{
						num5 += *ptr2 * (*ptr3 + *ptr4) + ptr2[1] * (ptr3[1] + *(float*)((byte*)ptr4 + -4)) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8)) + ptr2[3] * (ptr3[3] + *(float*)((byte*)ptr4 + -12));
						ptr2 += 4;
						ptr3 += 4;
						ptr4 -= 4;
					}
					while ((num7 -= 4) >= 4);
				}
				while (num7-- > 0)
				{
					float num9 = num5;
					float* intPtr = ptr2;
					ptr2 = intPtr + 1;
					float num10 = *intPtr;
					float* intPtr2 = ptr3;
					ptr3 = intPtr2 + 1;
					float num11 = *intPtr2;
					float* intPtr3 = ptr4;
					ptr4 = intPtr3 - 1;
					num5 = num9 + num10 * (num11 + *intPtr3);
				}
				num5 += ptr[num6] * this._coeffPtr[num6];
				if ((this._offset -= this._decimationFactor) < 0)
				{
					int num12 = this._offset + this._decimationFactor;
					this._offset += this._queueSize;
					Utils.Memcpy(this._queuePtr + this._offset + this._decimationFactor, this._queuePtr + num12, (this._queueSize - this._decimationFactor) * 4);
				}
				buffer[num2] = num5;
				num += this._decimationFactor;
				num2++;
			}
		}

		private unsafe void ProcessSymmetricKernelInterleaved(float* buffer, int length)
		{
			length <<= 1;
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float* ptr = this._queuePtr + this._offset;
				int num3 = 0;
				int num4 = num + 2 * (this._decimationFactor - 1);
				while (num3 < this._decimationFactor)
				{
					ptr[num3] = buffer[num4];
					num3++;
					num4 -= 2;
				}
				float num5 = 0f;
				int num6 = this._queueSize / 2;
				int num7 = num6;
				float* ptr2 = this._coeffPtr;
				float* ptr3 = ptr;
				float* ptr4 = ptr + this._queueSize - 1;
				if (num7 >= 16)
				{
					do
					{
						num5 += *ptr2 * (*ptr3 + *ptr4) + ptr2[1] * (ptr3[1] + *(float*)((byte*)ptr4 + -4)) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8)) + ptr2[3] * (ptr3[3] + *(float*)((byte*)ptr4 + -12)) + ptr2[4] * (ptr3[4] + *(float*)((byte*)ptr4 + -16)) + ptr2[5] * (ptr3[5] + *(float*)((byte*)ptr4 + -20)) + ptr2[6] * (ptr3[6] + *(float*)((byte*)ptr4 + -24)) + ptr2[7] * (ptr3[7] + *(float*)((byte*)ptr4 + -28)) + ptr2[8] * (ptr3[8] + *(float*)((byte*)ptr4 + -32)) + ptr2[9] * (ptr3[9] + *(float*)((byte*)ptr4 + -36)) + ptr2[10] * (ptr3[10] + *(float*)((byte*)ptr4 + -40)) + ptr2[11] * (ptr3[11] + *(float*)((byte*)ptr4 + -44)) + ptr2[12] * (ptr3[12] + *(float*)((byte*)ptr4 + -48)) + ptr2[13] * (ptr3[13] + *(float*)((byte*)ptr4 + -52)) + ptr2[14] * (ptr3[14] + *(float*)((byte*)ptr4 + -56)) + ptr2[15] * (ptr3[15] + *(float*)((byte*)ptr4 + -60));
						ptr2 += 16;
						ptr3 += 16;
						ptr4 -= 16;
					}
					while ((num7 -= 16) >= 16);
				}
				if (num7 >= 8)
				{
					num5 += *ptr2 * (*ptr3 + *ptr4) + ptr2[1] * (ptr3[1] + *(float*)((byte*)ptr4 + -4)) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8)) + ptr2[3] * (ptr3[3] + *(float*)((byte*)ptr4 + -12)) + ptr2[4] * (ptr3[4] + *(float*)((byte*)ptr4 + -16)) + ptr2[5] * (ptr3[5] + *(float*)((byte*)ptr4 + -20)) + ptr2[6] * (ptr3[6] + *(float*)((byte*)ptr4 + -24)) + ptr2[7] * (ptr3[7] + *(float*)((byte*)ptr4 + -28));
					ptr2 += 8;
					ptr3 += 8;
					ptr4 -= 8;
					num7 -= 8;
				}
				if (num7 >= 4)
				{
					num5 += *ptr2 * (*ptr3 + *ptr4) + ptr2[1] * (ptr3[1] + *(float*)((byte*)ptr4 + -4)) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8)) + ptr2[3] * (ptr3[3] + *(float*)((byte*)ptr4 + -12));
					ptr2 += 4;
					ptr3 += 4;
					ptr4 -= 4;
					num7 -= 4;
				}
				if (num7 >= 2)
				{
					num5 += *ptr2 * (*ptr3 + *ptr4) + ptr2[1] * (ptr3[1] + *(float*)((byte*)ptr4 + -4));
					ptr2 += 2;
					ptr3 += 2;
					ptr4 -= 2;
					num7 -= 2;
				}
				if (num7 >= 1)
				{
					num5 += *ptr2 * (*ptr3 + *ptr4);
				}
				num5 += ptr[num6] * this._coeffPtr[num6];
				if ((this._offset -= this._decimationFactor) < 0)
				{
					this._offset = this._queueSize;
					Utils.Memcpy(this._queuePtr + this._offset + this._decimationFactor, ptr, (this._queueSize - this._decimationFactor) * 4);
				}
				buffer[num2] = num5;
				num += this._decimationFactor * 2;
				num2 += 2;
			}
		}

		private unsafe void ProcessHalfBandKernel(float* buffer, int length)
		{
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float* ptr = this._queuePtr + this._offset;
				*ptr = buffer[num + 1];
				ptr[1] = buffer[num];
				float num3 = 0f;
				int num4 = this._queueSize / 2;
				int num5 = num4;
				float* ptr2 = this._coeffPtr;
				float* ptr3 = ptr;
				float* ptr4 = ptr + this._queueSize - 1;
				if (num5 >= 8)
				{
					do
					{
						num3 += *ptr2 * (*ptr3 + *ptr4) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8)) + ptr2[4] * (ptr3[4] + *(float*)((byte*)ptr4 + -16)) + ptr2[6] * (ptr3[6] + *(float*)((byte*)ptr4 + -24));
						ptr2 += 8;
						ptr3 += 8;
						ptr4 -= 8;
					}
					while ((num5 -= 8) >= 8);
				}
				if (num5 >= 4)
				{
					num3 += *ptr2 * (*ptr3 + *ptr4) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8));
					ptr2 += 4;
					ptr3 += 4;
					ptr4 -= 4;
					num5 -= 4;
				}
				while (num5-- > 0)
				{
					float num7 = num3;
					float* intPtr = ptr2;
					ptr2 = intPtr + 1;
					float num8 = *intPtr;
					float* intPtr2 = ptr3;
					ptr3 = intPtr2 + 1;
					float num9 = *intPtr2;
					float* intPtr3 = ptr4;
					ptr4 = intPtr3 - 1;
					num3 = num7 + num8 * (num9 + *intPtr3);
				}
				num3 += ptr[num4] * this._coeffPtr[num4];
				if ((this._offset -= this._decimationFactor) < 0)
				{
					int num10 = this._offset + this._decimationFactor;
					this._offset += this._queueSize;
					Utils.Memcpy(this._queuePtr + this._offset + this._decimationFactor, this._queuePtr + num10, (this._queueSize - this._decimationFactor) * 4);
				}
				buffer[num2] = num3;
				num += 2;
				num2++;
			}
		}

		private unsafe void ProcessHalfBandInterleaved(float* buffer, int length)
		{
			length <<= 1;
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float* ptr = this._queuePtr + this._offset;
				*ptr = buffer[num + 2];
				ptr[1] = buffer[num];
				float num3 = 0f;
				int num4 = this._queueSize / 2;
				int num5 = num4;
				float* ptr2 = this._coeffPtr;
				float* ptr3 = ptr;
				float* ptr4 = ptr + this._queueSize - 1;
				if (num5 >= 8)
				{
					do
					{
						num3 += *ptr2 * (*ptr3 + *ptr4) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8)) + ptr2[4] * (ptr3[4] + *(float*)((byte*)ptr4 + -16)) + ptr2[6] * (ptr3[6] + *(float*)((byte*)ptr4 + -24));
						ptr2 += 8;
						ptr3 += 8;
						ptr4 -= 8;
					}
					while ((num5 -= 8) >= 8);
				}
				if (num5 >= 4)
				{
					num3 += *ptr2 * (*ptr3 + *ptr4) + ptr2[2] * (ptr3[2] + *(float*)((byte*)ptr4 + -8));
					ptr2 += 4;
					ptr3 += 4;
					ptr4 -= 4;
					num5 -= 4;
				}
				while (num5-- > 0)
				{
					float num7 = num3;
					float* intPtr = ptr2;
					ptr2 = intPtr + 1;
					float num8 = *intPtr;
					float* intPtr2 = ptr3;
					ptr3 = intPtr2 + 1;
					float num9 = *intPtr2;
					float* intPtr3 = ptr4;
					ptr4 = intPtr3 - 1;
					num3 = num7 + num8 * (num9 + *intPtr3);
				}
				num3 += ptr[num4] * this._coeffPtr[num4];
				if ((this._offset -= this._decimationFactor) < 0)
				{
					int num10 = this._offset + this._decimationFactor;
					this._offset += this._queueSize;
					Utils.Memcpy(this._queuePtr + this._offset + this._decimationFactor, this._queuePtr + num10, (this._queueSize - this._decimationFactor) * 4);
				}
				buffer[num2] = num3;
				num += 4;
				num2 += 2;
			}
		}

		private unsafe void ProcessStandard(float* buffer, int length)
		{
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float* ptr = this._queuePtr + this._offset;
				int num3 = 0;
				int num4 = num + this._decimationFactor - 1;
				while (num3 < this._decimationFactor)
				{
					ptr[num3] = buffer[num4];
					num3++;
					num4--;
				}
				float num5 = 0f;
				int num6 = this._queueSize;
				float* ptr2 = ptr;
				float* ptr3 = this._coeffPtr;
				if (num6 >= 4)
				{
					do
					{
						num5 += *ptr2 * *ptr3 + ptr2[1] * ptr3[1] + ptr2[2] * ptr3[2] + ptr2[3] * ptr3[3];
						ptr2 += 4;
						ptr3 += 4;
					}
					while ((num6 -= 4) >= 4);
				}
				while (num6-- > 0)
				{
					float num8 = num5;
					float* intPtr = ptr2;
					ptr2 = intPtr + 1;
					float num9 = *intPtr;
					float* intPtr2 = ptr3;
					ptr3 = intPtr2 + 1;
					num5 = num8 + num9 * *intPtr2;
				}
				if ((this._offset -= this._decimationFactor) < 0)
				{
					int num10 = this._offset + this._decimationFactor;
					this._offset += this._queueSize;
					Utils.Memcpy(this._queuePtr + this._offset + this._decimationFactor, this._queuePtr + num10, (this._queueSize - this._decimationFactor) * 4);
				}
				buffer[num2] = num5;
				num += this._decimationFactor;
				num2++;
			}
		}

		private unsafe void ProcessStandardInterleaved(float* buffer, int length)
		{
			length <<= 1;
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float* ptr = this._queuePtr + this._offset;
				int num3 = 0;
				int num4 = num + 2 * (this._decimationFactor - 1);
				while (num3 < this._decimationFactor)
				{
					ptr[num3] = buffer[num4];
					num3++;
					num4 -= 2;
				}
				float num5 = 0f;
				int num6 = this._queueSize;
				float* ptr2 = ptr;
				float* ptr3 = this._coeffPtr;
				if (num6 >= 4)
				{
					do
					{
						num5 += *ptr2 * *ptr3 + ptr2[1] * ptr3[1] + ptr2[2] * ptr3[2] + ptr2[3] * ptr3[3];
						ptr2 += 4;
						ptr3 += 4;
					}
					while ((num6 -= 4) >= 4);
				}
				while (num6-- > 0)
				{
					float num8 = num5;
					float* intPtr = ptr2;
					ptr2 = intPtr + 1;
					float num9 = *intPtr;
					float* intPtr2 = ptr3;
					ptr3 = intPtr2 + 1;
					num5 = num8 + num9 * *intPtr2;
				}
				if ((this._offset -= this._decimationFactor) < 0)
				{
					int num10 = this._offset + this._decimationFactor;
					this._offset += this._queueSize;
					Utils.Memcpy(this._queuePtr + this._offset + this._decimationFactor, this._queuePtr + num10, (this._queueSize - this._decimationFactor) * 4);
				}
				buffer[num2] = num5;
				num += this._decimationFactor * 2;
				num2 += 2;
			}
		}

		public unsafe void Process(float* buffer, int length)
		{
			if (this._isHalfBand)
			{
				this.ProcessHalfBandKernel(buffer, length);
			}
			else if (this._isSymmetric)
			{
				this.ProcessSymmetricKernel(buffer, length);
			}
			else
			{
				this.ProcessStandard(buffer, length);
			}
		}

		public unsafe void ProcessInterleaved(float* buffer, int length)
		{
			if (this._isHalfBand)
			{
				this.ProcessHalfBandInterleaved(buffer, length);
			}
			else if (this._isSymmetric)
			{
				this.ProcessSymmetricKernelInterleaved(buffer, length);
			}
			else
			{
				this.ProcessStandardInterleaved(buffer, length);
			}
		}
	}
}
