using SDRSharp.Radio;
using System;

namespace SDRSharp.PanView
{
	public class LinLog
	{
		private double _exp;

		private double _frac = 0.5;

		private float _fMin;

		private float _fMax;

		private UnsafeBuffer _tmpBuf;

		private unsafe byte* _tmpPtr;

		private UnsafeBuffer _frBuf;

		private unsafe long* _frPtr;

		public double LogFactor
		{
			get
			{
				return this._frac;
			}
			set
			{
				this._fMax = -1f;
				this._frac = value;
				this._exp = Math.Log(2.0) / Math.Log(1.0 / this._frac);
			}
		}

		public double GetLog(float ldMin, float ldMax, float ldval)
		{
			if (ldMin < ldMax)
			{
				if ((double)(ldMax - ldMin) < 1E-99)
				{
					return 0.0;
				}
				if (ldval < ldMin)
				{
					return 0.0;
				}
				if (ldval > ldMax)
				{
					return 1.0;
				}
			}
			else
			{
				if ((double)(ldMin - ldMax) < 1E-99)
				{
					return 0.0;
				}
				if (ldval < ldMax)
				{
					return 0.0;
				}
				if (ldval > ldMin)
				{
					return 1.0;
				}
			}
			if (this._frac == 0.5)
			{
				return (double)((ldval - ldMin) / (ldMax - ldMin));
			}
			double num = (double)((ldval - ldMin) / (ldMax - ldMin));
			return 0.2 * num + Math.Pow((double)((ldval - ldMin) / (ldMax - ldMin)), this._exp) / 1.2;
		}

		public unsafe void MakeLog(byte[] srcPtr, int length, long fMin, long fMax)
		{
			if (this._tmpBuf == null || this._tmpBuf.Length != length)
			{
				if (this._tmpBuf != null)
				{
					this._tmpBuf.Dispose();
				}
				this._tmpBuf = UnsafeBuffer.Create(length, 1);
				this._tmpPtr = (byte*)(void*)this._tmpBuf;
			}
			if (this._frBuf == null || this._frBuf.Length != length)
			{
				if (this._frBuf != null)
				{
					this._frBuf.Dispose();
				}
				this._frBuf = UnsafeBuffer.Create(length, 8);
				this._frPtr = (long*)(void*)this._frBuf;
				this._fMax = -1f;
			}
			if (this._fMin != (float)fMin || this._fMax != (float)fMax)
			{
				this._fMin = (float)fMin;
				this._fMax = (float)fMax;
				double num = Math.Log10((double)fMin);
				double num2 = Math.Log10((double)fMax);
				double num3 = (num2 - num) / (double)length;
				for (int i = 0; i < length; i++)
				{
					this._frPtr[i] = Convert.ToInt32(Math.Pow(10.0, num + (double)i * num3));
				}
			}
			long num4 = 0L;
			long num5 = 0L;
			int num6 = 0;
			int num7 = 0;
			for (int j = 0; j < length; j++)
			{
				num4 = ((num5 <= 0) ? ((j == 0) ? (*this._frPtr) : Convert.ToInt32(Math.Sqrt((double)(this._frPtr[j] * this._frPtr[j - 1])))) : num5);
				num5 = ((j == length - 1) ? this._frPtr[length - 1] : Convert.ToInt32(Math.Sqrt((double)(this._frPtr[j] * this._frPtr[j + 1]))));
				num6 = ((num7 <= 0) ? Math.Min((int)(num4 * length / fMax), length - 1) : num7);
				num7 = Math.Min((int)(num5 * length / fMax), length - 1);
				if (num7 > num6)
				{
					num6++;
				}
				this._tmpPtr[j] = 0;
				for (int k = num6; k <= num7; k++)
				{
					this._tmpPtr[j] = Math.Max(this._tmpPtr[j], srcPtr[k]);
				}
			}
			for (int l = 0; l < length; l++)
			{
				srcPtr[l] = this._tmpPtr[l];
			}
		}
	}
}
