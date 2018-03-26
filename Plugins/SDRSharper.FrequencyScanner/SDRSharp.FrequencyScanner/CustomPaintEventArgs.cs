using System;
using System.Drawing;

namespace SDRSharp.FrequencyScanner
{
	public class CustomPaintEventArgs : EventArgs
	{
		public Graphics Graphics
		{
			get;
			private set;
		}

		public bool Cancel
		{
			get;
			set;
		}

		public CustomPaintEventArgs(Graphics graphics)
		{
			this.Graphics = graphics;
		}
	}
}
