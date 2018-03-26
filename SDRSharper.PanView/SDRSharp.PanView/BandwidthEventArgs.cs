using System;

namespace SDRSharp.PanView
{
	public class BandwidthEventArgs : EventArgs
	{
		public int Bandwidth
		{
			get;
			set;
		}

		public int Offset
		{
			get;
			set;
		}

		public bool Cancel
		{
			get;
			set;
		}

		public BandwidthEventArgs(int bandwidth, int offset)
		{
			this.Bandwidth = bandwidth;
			this.Offset = offset;
		}
	}
}
