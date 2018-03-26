using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Mouses
	{
		public static Cursor Passband = Cursors.Hand;

		public static Cursor Spectrum = Cursors.Cross;

		public static Cursor ChangeBW = Cursors.VSplit;

		public static Cursor RangeUp = Cursors.PanEast;

		public static Cursor RangeDown = Cursors.PanWest;

		public static Cursor Scale = Cursors.HSplit;

		public static Cursor Static = Cursors.Cross;

		public static Cursor StationList = Cursors.Default;
	}
}
