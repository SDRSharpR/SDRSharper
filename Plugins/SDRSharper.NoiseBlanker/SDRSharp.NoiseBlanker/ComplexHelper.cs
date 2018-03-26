using SDRSharp.Radio;
using System;

namespace SDRSharp.NoiseBlanker
{
	internal static class ComplexHelper
	{
		public static float FastMagnitude(this Complex c)
		{
			float val = Math.Abs(c.Real);
			float val2 = Math.Abs(c.Imag);
			return Math.Max(val, val2);
		}
	}
}
