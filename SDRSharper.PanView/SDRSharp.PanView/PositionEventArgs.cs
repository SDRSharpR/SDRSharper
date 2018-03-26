using System;

namespace SDRSharp.PanView
{
	public class PositionEventArgs : EventArgs
	{
		public float Xpos;

		public float Ypos;

		public bool Trig;

		public PositionEventArgs(float x, float y, bool trig)
		{
			this.Xpos = x;
			this.Ypos = y;
			this.Trig = trig;
		}
	}
}
