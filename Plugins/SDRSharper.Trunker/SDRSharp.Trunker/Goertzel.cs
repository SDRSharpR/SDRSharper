using System;

namespace SDRSharp.Trunker
{
	public class Goertzel
	{
		public unsafe static float GetMag(double sampleRate, int frequency, float* buffer, int length)
		{
			int num = (int)(0.5 + (double)(length * frequency) / sampleRate);
			double num2 = 6.2831853071795862 * (double)num / (double)length;
			float num3 = (float)(2.0 * Math.Cos(num2));
			float num5;
			float num4;
			float num6 = num5 = (num4 = 0f);
			for (int i = 0; i < length; i++)
			{
				num6 = num3 * num5 - num4 + buffer[i];
				num4 = num5;
				num5 = num6;
			}
			float num7 = (float)length / 2f;
			float num8 = (float)((double)num5 - (double)num4 * Math.Cos(num2)) / num7;
			float num9 = (float)((double)num4 * Math.Sin(num2)) / num7;
			return (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
		}
	}
}
