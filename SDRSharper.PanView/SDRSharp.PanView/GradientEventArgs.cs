using System;
using System.Drawing.Drawing2D;

namespace SDRSharp.PanView
{
	public class GradientEventArgs : EventArgs
	{
		public string Parent;

		public int Index;

		public ColorBlend Blend;

		public int Back;

		public int Trace;

		public int Fill;

		public GradientEventArgs(string parent, int index, ColorBlend blend, int back, int trace, int fill)
		{
			this.Parent = parent;
			this.Index = index;
			this.Blend = blend;
			this.Back = back;
			this.Trace = trace;
			this.Fill = fill;
		}
	}
}
