using SDRSharp.Radio;
using System;

namespace SDRSharp.RTLSDR
{
	public sealed class SamplesAvailableEventArgs : EventArgs
	{
		public int Length
		{
			get;
			set;
		}

		public unsafe Complex* Buffer
		{
			get;
			set;
		}
	}
}
