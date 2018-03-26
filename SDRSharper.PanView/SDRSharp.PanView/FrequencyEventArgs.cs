using System;

namespace SDRSharp.PanView
{
	public class FrequencyEventArgs : EventArgs
	{
		public long Frequency
		{
			get;
			set;
		}

		public bool Cancel
		{
			get;
			set;
		}

		public FrequencyEventArgs(long frequency)
		{
			this.Frequency = frequency;
		}
	}
}
