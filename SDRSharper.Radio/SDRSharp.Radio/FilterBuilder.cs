using System;

namespace SDRSharp.Radio
{
	public static class FilterBuilder
	{
		public const int DefaultFilterOrder = 500;

		public static float[] MakeWindow(WindowType windowType, int length)
		{
			float[] array = new float[length];
			length--;
			for (int i = 0; i <= length; i++)
			{
				array[i] = 1f;
				switch (windowType)
				{
				case WindowType.Hamming:
				{
					float num = 0.54f;
					float num2 = 0.46f;
					float num3 = 0f;
					float num4 = 0f;
					array[i] *= num - num2 * (float)Math.Cos(6.2831853071795862 * (double)i / (double)length) + num3 * (float)Math.Cos(12.566370614359172 * (double)i / (double)length) - num4 * (float)Math.Cos(18.849555921538759 * (double)i / (double)length);
					break;
				}
				case WindowType.Blackman:
				{
					float num = 0.42f;
					float num2 = 0.5f;
					float num3 = 0.08f;
					float num4 = 0f;
					array[i] *= num - num2 * (float)Math.Cos(6.2831853071795862 * (double)i / (double)length) + num3 * (float)Math.Cos(12.566370614359172 * (double)i / (double)length) - num4 * (float)Math.Cos(18.849555921538759 * (double)i / (double)length);
					break;
				}
				case WindowType.BlackmanHarris4:
				{
					float num = 0.35875f;
					float num2 = 0.48829f;
					float num3 = 0.14128f;
					float num4 = 0.01168f;
					array[i] *= num - num2 * (float)Math.Cos(6.2831853071795862 * (double)i / (double)length) + num3 * (float)Math.Cos(12.566370614359172 * (double)i / (double)length) - num4 * (float)Math.Cos(18.849555921538759 * (double)i / (double)length);
					break;
				}
				case WindowType.BlackmanHarris7:
				{
					float num = 0.2710514f;
					float num2 = 0.433297932f;
					float num3 = 0.218123f;
					float num4 = 0.06592545f;
					float num6 = 0.0108117424f;
					float num7 = 0.000776584842f;
					float num8 = 1.38872174E-05f;
					array[i] *= num - num2 * (float)Math.Cos(6.2831853071795862 * (double)i / (double)length) + num3 * (float)Math.Cos(12.566370614359172 * (double)i / (double)length) - num4 * (float)Math.Cos(18.849555921538759 * (double)i / (double)length) + num6 * (float)Math.Cos(25.132741228718345 * (double)i / (double)length) - num7 * (float)Math.Cos(31.415926535897931 * (double)i / (double)length) + num8 * (float)Math.Cos(37.699111843077517 * (double)i / (double)length);
					break;
				}
				case WindowType.HannPoisson:
				{
					float value = (float)i - (float)length / 2f;
					float num5 = 0.005f;
					array[i] *= 0.5f * (float)((1.0 + Math.Cos(6.2831853071795862 * (double)value / (double)length)) * Math.Exp(-2.0 * (double)num5 * (double)Math.Abs(value) / (double)length));
					break;
				}
				case WindowType.Youssef:
				{
					float num = 0.35875f;
					float num2 = 0.48829f;
					float num3 = 0.14128f;
					float num4 = 0.01168f;
					float value = (float)i - (float)length / 2f;
					float num5 = 0.005f;
					array[i] *= num - num2 * (float)Math.Cos(6.2831853071795862 * (double)i / (double)length) + num3 * (float)Math.Cos(12.566370614359172 * (double)i / (double)length) - num4 * (float)Math.Cos(18.849555921538759 * (double)i / (double)length);
					array[i] *= (float)Math.Exp(-2.0 * (double)num5 * (double)Math.Abs(value) / (double)length);
					break;
				}
				}
			}
			return array;
		}

		public static float[] MakeSinc(double sampleRate, double frequency, int length)
		{
			if (length % 2 == 0)
			{
				throw new ArgumentException("Length should be odd", "length");
			}
			double num = 6.2831853071795862 * frequency / sampleRate;
			float[] array = new float[length];
			for (int i = 0; i < length; i++)
			{
				int num2 = i - length / 2;
				if (num2 == 0)
				{
					array[i] = (float)num;
				}
				else
				{
					array[i] = (float)(Math.Sin(num * (double)num2) / (double)num2);
				}
			}
			return array;
		}

		public static float[] MakeSin(double sampleRate, double frequency, int length)
		{
			if (length % 2 == 0)
			{
				throw new ArgumentException("Length should be odd", "length");
			}
			double num = 6.2831853071795862 * frequency / sampleRate;
			float[] array = new float[length];
			int num2 = length / 2;
			for (int i = 0; i <= num2; i++)
			{
				array[num2 - i] = 0f - (array[num2 + i] = (float)Math.Sin(num * (double)i));
			}
			return array;
		}

		public static float[] MakeLowPassKernel(double sampleRate, int filterOrder, double cutoffFrequency, WindowType windowType)
		{
			filterOrder |= 1;
			float[] array = FilterBuilder.MakeSinc(sampleRate, cutoffFrequency, filterOrder);
			float[] window = FilterBuilder.MakeWindow(windowType, filterOrder);
			FilterBuilder.ApplyWindow(array, window);
			FilterBuilder.Normalize(array);
			return array;
		}

		public static float[] MakeHighPassKernel(double sampleRate, int filterOrder, double cutoffFrequency, WindowType windowType)
		{
			return FilterBuilder.InvertSpectrum(FilterBuilder.MakeLowPassKernel(sampleRate, filterOrder, cutoffFrequency, windowType));
		}

		public static float[] MakeStopBandKernel(double sampleRate, int filterOrder, int cutoff1, int cutoff2, WindowType windowType)
		{
			return FilterBuilder.InvertSpectrum(FilterBuilder.MakeBandPassKernel(sampleRate, filterOrder, (double)cutoff1, (double)cutoff2, windowType));
		}

		public static float[] MakeBandPassKernel(double sampleRate, int filterOrder, double cutoff1, double cutoff2, WindowType windowType)
		{
			double num = (cutoff2 - cutoff1) / 2.0;
			double num2 = cutoff2 - num;
			double num3 = 6.2831853071795862 * num2 / sampleRate;
			float[] array = FilterBuilder.MakeLowPassKernel(sampleRate, filterOrder, num, windowType);
			for (int i = 0; i < array.Length; i++)
			{
				int num4 = i - filterOrder / 2;
				array[i] *= (float)(2.0 * Math.Cos(num3 * (double)num4));
			}
			return array;
		}

		public static Complex[] MakeKernelFromFFT(Complex[] buf, int len, WindowType window, int order)
		{
			float[] array = FilterBuilder.MakeWindow(window, order + 1);
			Complex[] array2 = new Complex[order + 1];
			for (int i = 0; i < order + 1; i++)
			{
				int num = i - order / 2;
				if (num < 0)
				{
					num += len;
				}
				Complex.Mul(ref array2[i], buf[num], array[i]);
			}
			return array2;
		}

		public static void Normalize(float[] h)
		{
			float num = 0f;
			for (int i = 0; i < h.Length; i++)
			{
				num += h[i];
			}
			for (int j = 0; j < h.Length; j++)
			{
				h[j] /= num;
			}
		}

		public static void ApplyWindow(float[] coefficients, float[] window)
		{
			for (int i = 0; i < coefficients.Length; i++)
			{
				coefficients[i] *= window[i];
			}
		}

		private static float[] InvertSpectrum(float[] h)
		{
			for (int i = 0; i < h.Length; i++)
			{
				h[i] = 0f - h[i];
			}
			h[(h.Length - 1) / 2] += 1f;
			return h;
		}
	}
}
