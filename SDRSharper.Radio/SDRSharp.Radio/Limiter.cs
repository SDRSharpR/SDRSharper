using System;

namespace SDRSharp.Radio
{
	public class Limiter
	{
		private double _treshold;

		private double _ratio;

		public int Treshold
		{
			set
			{
				this._treshold = Math.Pow(10.0, (double)value / 20.0);
			}
		}

		public double Ratio
		{
			set
			{
				this._ratio = Math.Pow(10.0, -1.0 + value / 50.0);
			}
		}

		public unsafe void process(float* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				float num = Math.Abs(buffer[i]);
				double num2 = this._treshold * this._ratio;
				float num3 = (!((double)num > this._treshold)) ? num : ((float)(this._treshold + num2 * (1.0 - Math.Exp((0.0 - ((double)num - this._treshold)) / num2))));
				if (buffer[i] > 0f)
				{
					buffer[i] = num3;
				}
				else
				{
					buffer[i] = 0f - num3;
				}
			}
		}
	}
}
