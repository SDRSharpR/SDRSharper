using System;

namespace SDRSharp.Radio
{
	public class FractResampler
	{
		private const int SINC_PERIOD_PTS = 10000;

		private const int SINC_PERIODS = 28;

		private const int SINC_LENGTH = 280001;

		private const float MAX_SOUNDCARDVAL = 32767f;

		private UnsafeBuffer _sinc;

		private UnsafeBuffer _workL;

		private UnsafeBuffer _workR;

		private unsafe double* _pSinc;

		private unsafe double* _pWorkL;

		private unsafe double* _pWorkR;

		private double _fTime;

		public unsafe FractResampler()
		{
			if (this._pSinc == null)
			{
				this._sinc = UnsafeBuffer.Create(280001, 8);
				this._pSinc = (double*)(void*)this._sinc;
			}
			for (int i = 0; i < 280001; i++)
			{
				double num = 0.35875 - 0.48829 * Math.Cos(6.2831853071795862 * (double)i / 280000.0) + 0.14128 * Math.Cos(12.566370614359172 * (double)i / 280000.0) - 0.01168 * Math.Cos(18.849555921538759 * (double)i / 280000.0);
				double num2 = 3.1415926535897931 * (double)(i - 140000) / 10000.0;
				if (i == 140000)
				{
					this._pSinc[i] = 1.0;
				}
				else
				{
					this._pSinc[i] = (double)(float)(num * Math.Sin(num2) / num2);
				}
			}
			this._fTime = 0.0;
		}

		unsafe ~FractResampler()
		{
			this._pSinc = (this._pWorkL = (this._pWorkR = null));
		}

		public unsafe int Resample(int inLength, double rate, float* pInBuf, float* pOutBuf)
		{
			if (this._workL == null || this._workL.Length != inLength / 2 + 28)
			{
				if (this._workL != null)
				{
					this._workL.Dispose();
				}
				this._workL = UnsafeBuffer.Create(inLength / 2 + 28, 8);
				this._pWorkL = (double*)(void*)this._workL;
			}
			if (this._workR == null || this._workR.Length != inLength / 2 + 28)
			{
				if (this._workR != null)
				{
					this._workR.Dispose();
				}
				this._workR = UnsafeBuffer.Create(inLength / 2 + 28, 8);
				this._pWorkR = (double*)(void*)this._workR;
			}
			int num = (int)this._fTime;
			int num2 = 0;
			int num3 = 0;
			int i;
			for (i = 28; i < 28 + inLength / 2; i++)
			{
				this._pWorkL[i] = (double)pInBuf[num3++];
				this._pWorkR[i] = (double)pInBuf[num3++];
			}
			while (num < inLength / 2)
			{
				double num6 = 0.0;
				double num7 = 0.0;
				for (i = 1; i <= 28; i++)
				{
					num3 = num + i;
					int num8 = (int)(((double)num3 - this._fTime) * 10000.0);
					num6 += this._pWorkL[num3] * this._pSinc[num8];
					num7 += this._pWorkR[num3] * this._pSinc[num8];
				}
				pOutBuf[num2++] = (float)num6;
				pOutBuf[num2++] = (float)num7;
				this._fTime += rate;
				num = (int)this._fTime;
			}
			this._fTime -= (double)inLength / 2.0;
			num3 = inLength / 2;
			i = 0;
			while (i < 28)
			{
				this._pWorkL[i] = this._pWorkL[num3];
				this._pWorkR[i] = this._pWorkR[num3];
				i++;
				num3++;
			}
			return num2;
		}
	}
}
