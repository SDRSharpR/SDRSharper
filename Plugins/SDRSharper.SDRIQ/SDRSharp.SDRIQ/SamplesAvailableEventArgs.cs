using SDRSharp.Radio;
using System;

namespace SDRSharp.SDRIQ
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
