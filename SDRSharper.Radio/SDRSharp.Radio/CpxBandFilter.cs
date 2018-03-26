using System;

namespace SDRSharp.Radio
{
	public class CpxBandFilter
	{
		private int _width;

		private int _frequency;

		private CpxFirFilter _cpxFilter;

		private int _order = 1000;

		private int _fftLen = 1024;

		private float[] _coeffs;

		private Complex[] _kernel;

		private Complex[] _fftBuf;

		public int Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
			}
		}

		public int Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				this._frequency = value;
			}
		}

		public CpxBandFilter()
		{
			this._cpxFilter = new CpxFirFilter();
			this._coeffs = new float[this._order];
			this._fftBuf = new Complex[this._fftLen];
			this._kernel = new Complex[this._order];
		}

		public unsafe void MakeCoefficients(double sampleRate, int frequency, int width, WindowType window, bool stopBand = true)
		{
			int order = this._order;
			if (sampleRate < 30000.0 || width > 600)
			{
				order = 400;
			}
			if (sampleRate < 3000.0 || width > 6000)
			{
				order = 200;
			}
			this._width = width;
			this._frequency = frequency;
			int num = this._frequency - this._width / 2;
			int num2 = this._frequency + this._width / 2;
			int num3 = (int)Math.Max((double)(-this._fftLen / 2), (double)(num * this._fftLen) / sampleRate);
			int num4 = (int)Math.Min((double)(this._fftLen / 2), (double)(num2 * this._fftLen) / sampleRate);
			num3 = -num3;
			if (num3 < 0)
			{
				num3 += this._fftLen;
			}
			num4 = -num4;
			if (num4 < 0)
			{
				num4 += this._fftLen;
			}
			Complex[] fftBuf = this._fftBuf;
			fixed (Complex* buffer = fftBuf)
			{
				int num5 = (!stopBand) ? 1 : 0;
				int num6 = stopBand ? 1 : 0;
				for (int i = 0; i < this._fftLen; this._fftBuf[i].Imag = 0f, i++)
				{
					if (num3 < this._fftLen / 2 && num4 >= this._fftLen / 2)
					{
						this._fftBuf[i].Real = (float)((i < num3 || i > num4) ? num5 : num6);
						continue;
					}
					ref Complex val = ref this._fftBuf[i];
					if (num3 <= num4 && i >= num3 && i <= num4)
					{
						goto IL_0171;
					}
					if (num4 < num3 && i >= num4 && i < num3)
					{
						goto IL_0171;
					}
					int num7 = num6;
					goto IL_0173;
					IL_0171:
					num7 = num5;
					goto IL_0173;
					IL_0173:
					val.Real = (float)num7;
				}
				Fourier.BackwardTransform(buffer, this._fftLen);
			}
			this._kernel = FilterBuilder.MakeKernelFromFFT(this._fftBuf, this._fftLen, window, order);
			this._cpxFilter.SetCoefficients(this._kernel);
		}

		public unsafe void Process(Complex* iqPtr, int length)
		{
			this._cpxFilter.Process(iqPtr, length);
		}
	}
}
