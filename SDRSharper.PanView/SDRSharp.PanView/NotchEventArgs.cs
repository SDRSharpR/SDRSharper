using System;

namespace SDRSharp.PanView
{
	public class NotchEventArgs : EventArgs
	{
		public int Notch
		{
			get;
			set;
		}

		public int Offset
		{
			get;
			set;
		}

		public int Width
		{
			get;
			set;
		}

		public bool Active
		{
			get;
			set;
		}

		public bool Cancel
		{
			get;
			set;
		}

		public NotchEventArgs(int notch, int offset, int width, bool active)
		{
			this.Notch = notch;
			this.Offset = offset;
			this.Width = width;
			this.Active = active;
		}
	}
}
