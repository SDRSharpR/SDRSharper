using System;

namespace SDRSharp.Radio
{
	public static class Fourier
	{
		private const int MaxLutBits = 16;

		private const int MaxLutBins = 65536;

		private const int LutSize = 32768;

		private const double TwoPi = 6.2831853071795862;

		private static UnsafeBuffer _lrBuffer;

		private unsafe static float* _lr;

		private static UnsafeBuffer _liBuffer;

		private unsafe static float* _li;

		unsafe static Fourier()
		{
			Fourier._lrBuffer = UnsafeBuffer.Create(32768, 4);
			Fourier._liBuffer = UnsafeBuffer.Create(32768, 4);
			Fourier._lr = (float*)(void*)Fourier._lrBuffer;
			Fourier._li = (float*)(void*)Fourier._liBuffer;
			for (int i = 0; i < 32768; i++)
			{
				Fourier._lr[i] = (float)Math.Cos(9.5873799242852573E-05 * (double)i);
				Fourier._li[i] = (float)(0.0 - Math.Sin(9.5873799242852573E-05 * (double)i));
			}
		}

		public unsafe static void SpectrumPower(Complex* buffer, float* power, int length, float offset = 0f, bool half = false)
		{
			for (int i = 0; i < length; i++)
			{
				float num = buffer[i].Real * buffer[i].Real + buffer[i].Imag * buffer[i].Imag;
				float num2 = power[i] = (float)(10.0 * Math.Log10(1E-60 + (double)num)) + offset;
			}
		}

		public unsafe static void AverageMinimum(float* power, int length, int skip, out float avgMin, out float avgMax)
		{
			avgMin = 0f;
			avgMax = -200f;
			if (skip < length / 10)
			{
				skip = length / 10;
			}
			int num = (length - 2 * skip) / 8;
			if (length >= 1000000)
			{
				num *= 2;
			}
			for (int i = skip; i < length - skip; i++)
			{
				if (power[i] < avgMin)
				{
					avgMin = ((float)(num - 1) * avgMin + power[i]) / (float)num;
				}
			}
		}

		public unsafe static void ScaleFFT(float* src, byte* dest, int length, float minPower, float maxPower)
		{
			float num = 255f / (maxPower - minPower);
			for (int i = 0; i < length; i++)
			{
				float num2 = src[i];
				if (num2 < minPower)
				{
					num2 = minPower;
				}
				else if (num2 > maxPower)
				{
					num2 = maxPower;
				}
				dest[i] = (byte)((num2 - minPower) * num);
			}
		}

		public unsafe static void SmoothCopy(byte[] source, byte[] destination, int sourceLength, float scale, int offset)
		{
			fixed (byte* srcPtr = source)
			{
				fixed (byte* dstPtr = destination)
				{
					Fourier.SmoothCopy(srcPtr, dstPtr, sourceLength, destination.Length, scale, offset);
				}
			}
		}

		public unsafe static void SmoothCopy(byte* srcPtr, byte* dstPtr, int sourceLength, int destinationLength, float scale, int offset)
		{
			float num = (float)sourceLength / scale / (float)destinationLength;
			if (num > 1f)
			{
				int num2 = (int)Math.Ceiling((double)num);
				for (int i = 0; i < destinationLength; i++)
				{
					int num3 = (int)((float)i * num - (float)num2 * 0.5f);
					byte b = 0;
					for (int j = 0; j < num2; j++)
					{
						int num4 = num3 + j + offset;
						if (num4 >= 0 && num4 < sourceLength && b < srcPtr[num4])
						{
							b = srcPtr[num4];
						}
					}
					dstPtr[i] = b;
				}
				*dstPtr = dstPtr[2];
			}
			else
			{
				for (int k = 0; k < destinationLength; k++)
				{
					int num5 = (int)(num * (float)k + (float)offset);
					if (num5 >= 0 && num5 < sourceLength)
					{
						dstPtr[k] = srcPtr[num5];
					}
				}
			}
		}

		public unsafe static void SubCopy(float* srcPtr, float* dstPtr, int srcLen, int dstLen, float factor)
		{
			if ((double)factor > 1.1)
			{
				for (int i = 0; i < dstLen; i++)
				{
					int num = (int)((float)i * factor);
					if (num >= 0 && num < srcLen)
					{
						dstPtr[i] = srcPtr[num];
					}
				}
			}
			else if ((double)factor < 0.9)
			{
				for (int j = 0; j < dstLen; j++)
				{
					int num2 = (int)(factor * (float)j);
					if (num2 >= 0 && num2 < srcLen)
					{
						dstPtr[j] = srcPtr[num2];
					}
				}
			}
			else
			{
				Utils.Memcpy(dstPtr, srcPtr, dstLen * 4);
			}
		}

		public unsafe static void ApplyFFTWindow(Complex* buffer, float* window, int length)
		{
			for (int i = 0; i < length; i++)
			{
				buffer[i].Real *= window[i];
				buffer[i].Imag *= window[i];
			}
		}

		public unsafe static void ForwardTransform(Complex* buffer, int length, bool swap)
		{
			Fourier.ForwardTransformOrg(buffer, length);
			if (swap)
			{
				Fourier.SwapLR(buffer, length / 2);
				Fourier.SwapLR(buffer + length / 2, length / 2);
			}
		}

		public unsafe static void ForwardTransformOrg(Complex* buffer, int length)
		{
			int num = length - 1;
			int num2 = length / 2;
			int num3 = 0;
			for (int num4 = length; num4 > 1; num4 >>= 1)
			{
				num3++;
			}
			int num5 = num2;
			for (int num4 = 1; num4 < num; num4++)
			{
				if (num4 < num5)
				{
					float real = buffer[num5].Real;
					float imag = buffer[num5].Imag;
					buffer[num5].Real = buffer[num4].Real;
					buffer[num5].Imag = buffer[num4].Imag;
					buffer[num4].Real = real;
					buffer[num4].Imag = imag;
				}
				int num6;
				for (num6 = num2; num6 <= num5; num6 /= 2)
				{
					num5 -= num6;
				}
				num5 += num6;
			}
			for (int i = 1; i <= num3; i++)
			{
				int num7 = 1 << i;
				int num8 = num7 / 2;
				double num9 = 1.0;
				double num10 = 0.0;
				double num11 = Math.Cos(3.1415926535897931 / (double)num8);
				double num12 = 0.0 - Math.Sin(3.1415926535897931 / (double)num8);
				for (num5 = 1; num5 <= num8; num5++)
				{
					int num13 = num5 - 1;
					float num14 = (float)num9;
					float num15 = (float)num10;
					for (int num4 = num13; num4 <= num; num4 += num7)
					{
						int num16 = num4 + num8;
						float real = buffer[num16].Real * num14 - buffer[num16].Imag * num15;
						float imag = buffer[num16].Real * num15 + buffer[num16].Imag * num14;
						buffer[num16].Real = buffer[num4].Real - real;
						buffer[num16].Imag = buffer[num4].Imag - imag;
						buffer[num4].Real += real;
						buffer[num4].Imag += imag;
					}
					double num17 = num9;
					num9 = num17 * num11 - num10 * num12;
					num10 = num17 * num12 + num10 * num11;
				}
			}
		}

		public unsafe static void ForwardTransformOrgOrg(Complex* buffer, int length)
		{
			int num = length - 1;
			int num2 = length / 2;
			int num3 = 0;
			for (int num4 = length; num4 > 1; num4 >>= 1)
			{
				num3++;
			}
			int num5 = num2;
			for (int num4 = 1; num4 < num; num4++)
			{
				if (num4 < num5)
				{
					float real = buffer[num5].Real;
					float imag = buffer[num5].Imag;
					buffer[num5].Real = buffer[num4].Real;
					buffer[num5].Imag = buffer[num4].Imag;
					buffer[num4].Real = real;
					buffer[num4].Imag = imag;
				}
				int num6;
				for (num6 = num2; num6 <= num5; num6 /= 2)
				{
					num5 -= num6;
				}
				num5 += num6;
			}
			for (int i = 1; i <= num3; i++)
			{
				int num7 = 1 << i;
				int num8 = num7 / 2;
				float num9 = 1f;
				float num10 = 0f;
				float num11 = (float)Math.Cos(3.1415926535897931 / (double)num8);
				float num12 = (float)(0.0 - Math.Sin(3.1415926535897931 / (double)num8));
				for (num5 = 1; num5 <= num8; num5++)
				{
					int num13 = num5 - 1;
					float real;
					for (int num4 = num13; num4 <= num; num4 += num7)
					{
						int num14 = num4 + num8;
						real = buffer[num14].Real * num9 - buffer[num14].Imag * num10;
						float imag = buffer[num14].Real * num10 + buffer[num14].Imag * num9;
						buffer[num14].Real = buffer[num4].Real - real;
						buffer[num14].Imag = buffer[num4].Imag - imag;
						buffer[num4].Real += real;
						buffer[num4].Imag += imag;
					}
					real = num9;
					num9 = real * num11 - num10 * num12;
					num10 = real * num12 + num10 * num11;
				}
			}
		}

		public unsafe static void ForwardTransformLut(Complex* buffer, int length)
		{
			int num = length - 1;
			int num2 = length / 2;
			Complex complex = default(Complex);
			complex.Real = 0f;
			complex.Imag = 0f;
			Complex c = default(Complex);
			c.Real = 0f;
			c.Imag = 0f;
			int num3 = 0;
			for (int num4 = length; num4 > 1; num4 >>= 1)
			{
				num3++;
			}
			int num5 = num2;
			for (int num4 = 1; num4 < num; num4++)
			{
				if (num4 < num5)
				{
					complex = buffer[num5];
					buffer[num5] = buffer[num4];
					buffer[num4] = complex;
				}
				int num6;
				for (num6 = num2; num6 <= num5; num6 /= 2)
				{
					num5 -= num6;
				}
				num5 += num6;
			}
			for (int i = 1; i <= num3; i++)
			{
				int num7 = 1 << i;
				int num8 = num7 / 2;
				int num9 = 16 - i;
				for (num5 = 1; num5 <= num8; num5++)
				{
					int num10 = num5 - 1;
					int num11 = num10 << num9;
					c.Real = Fourier._lr[num11];
					c.Imag = Fourier._li[num11];
					for (int num4 = num10; num4 <= num; num4 += num7)
					{
						num11 = num4 + num8;
						Complex.Mul(ref complex, c, buffer[num11]);
						Complex.Sub(ref buffer[num11], buffer[num4], complex);
						Complex.Add(ref buffer[num4], complex);
					}
				}
			}
		}

		public unsafe static void ForwardTransformRot(Complex* buffer, int length)
		{
			int num = length - 1;
			int num2 = length / 2;
			Complex c = default(Complex);
			c.Real = 0f;
			c.Imag = 0f;
			Complex complex = default(Complex);
			complex.Real = 0f;
			complex.Imag = 0f;
			int num3 = 0;
			for (int num4 = length; num4 > 1; num4 >>= 1)
			{
				num3++;
			}
			int num5 = num2;
			for (int num4 = 1; num4 < num; num4++)
			{
				if (num4 < num5)
				{
					complex = buffer[num5];
					buffer[num5] = buffer[num4];
					buffer[num4] = complex;
				}
				int num6;
				for (num6 = num2; num6 <= num5; num6 /= 2)
				{
					num5 -= num6;
				}
				num5 += num6;
			}
			for (int i = 1; i <= num3; i++)
			{
				int num7 = 1 << i;
				int num8 = num7 / 2;
				double num9 = 3.1415926535897931 / (double)num8;
				for (num5 = 1; num5 <= num8; num5++)
				{
					int num10 = num5 - 1;
					Complex.FromAngle(ref c, num9 * (double)num10);
					Complex.Conjugate(ref c);
					for (int num4 = num10; num4 <= num; num4 += num7)
					{
						int num11 = num4 + num8;
						Complex.Mul(ref complex, c, buffer[num11]);
						Complex.Sub(ref buffer[num11], buffer[num4], complex);
						Complex.Add(ref buffer[num4], complex);
					}
				}
			}
		}

		public unsafe static void BackwardTransform(Complex* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				buffer[i].Imag = 0f - buffer[i].Imag;
			}
			Fourier.ForwardTransform(buffer, length, false);
			float num = 1f / (float)length;
			for (int j = 0; j < length; j++)
			{
				buffer[j].Real *= num;
				buffer[j].Imag = (0f - buffer[j].Imag) * num;
			}
		}

		public unsafe static void SwapLR(Complex* buf, int len)
		{
			for (int i = 0; i < len / 2; i++)
			{
				Complex complex = buf[i];
				buf[i] = buf[len - i - 1];
				buf[len - i - 1] = complex;
			}
		}
	}
}
