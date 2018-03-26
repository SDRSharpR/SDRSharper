using System.Diagnostics;

namespace SDRSharp.Radio
{
	public class Counter : Stopwatch
	{
		private int _count;

		public long Elaps(int init)
		{
			if (--this._count > 0)
			{
				return 0L;
			}
			if (init != 0)
			{
				base.Stop();
			}
			long elapsedMilliseconds = base.ElapsedMilliseconds;
			this._count = init;
			base.Reset();
			base.Start();
			return elapsedMilliseconds / init;
		}
	}
}
