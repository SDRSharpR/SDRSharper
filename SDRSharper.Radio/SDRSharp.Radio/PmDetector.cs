namespace SDRSharp.Radio
{
	public sealed class PmDetector
	{
		private const int PllDefaultFrequency = 57000;

		private float _angState;

		private float _meanAng;

		private float _freqErr;

		private float _avgMin;

		private float _avgMax;

		private float _avgMod;

		private float _old;

		private double _sampleRate;

		public float FreqErr => this._freqErr;

		public double SampleRate
		{
			set
			{
				this._sampleRate = value;
			}
		}

		public unsafe void Demodulate(Complex* iq, float* audio, int length)
		{
			float num = 3.14159274f;
			float num2 = 2f * num;
			int num3 = Utils.PhaseGain;
			if (this._sampleRate < 8000.0)
			{
				num3 /= 8;
			}
			else if (this._sampleRate < 24000.0)
			{
				num3 /= 4;
			}
			else if (this._sampleRate < 96000.0)
			{
				num3 /= 2;
			}
			for (int i = 0; i < length; i++)
			{
				float num4 = iq[i].Modulus();
				this._avgMod = (this._avgMod * 999f + num4) / 1000f;
				float num5 = iq[i].Argument();
				float num6 = num5 - this._angState;
				this._angState = num5;
				if (num6 >= num)
				{
					num6 -= num2;
				}
				else if (num6 <= 0f - num)
				{
					num6 += num2;
				}
				if (num6 < 0f)
				{
					this._avgMin = (this._avgMin * 99f + num6) / 100f;
					if (Utils.ChkAver && num6 < this._avgMin * 2f)
					{
						num6 = this._avgMin * 2f;
					}
				}
				else
				{
					this._avgMax = (this._avgMax * 99f + num6) / 100f;
					if (Utils.ChkAver && num6 > this._avgMax * 2f)
					{
						num6 = this._avgMax * 2f;
					}
				}
				if (num4 < this._avgMod / 2f)
				{
					num6 = this._old;
				}
				this._old = num6;
				this._meanAng = (this._meanAng * (float)(Utils.PhaseAverage - 1) + num6) / (float)Utils.PhaseAverage;
				float num7 = this._freqErr = this._meanAng * (float)num3;
				audio[i] = this._freqErr * 5E-06f;
			}
		}

		public unsafe void unwrap(float* ptr, int len)
		{
			float num = 6.28318548f;
			float num2 = 0f;
			for (int i = 1; i < len - 1; i++)
			{
				float num3 = ptr[i] - ptr[i - 1];
				if ((double)num3 > 3.1415926535897931)
				{
					num2 -= num;
				}
				else if ((double)num3 <= 3.1415926535897931)
				{
					num2 += num;
				}
				ptr[i] += num2;
			}
		}
	}
}
