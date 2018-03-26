namespace SDRSharp.PanView
{
	public sealed class PeakDetector
	{
		private const byte Threshold = 15;

		public static void GetPeaks(byte[] buffer, bool[] peaks, int windowSize)
		{
			for (int i = 0; i < buffer.Length; i++)
			{
				bool flag = true;
				byte b = byte.MaxValue;
				byte b2 = 0;
				for (int j = 0; j < windowSize; j++)
				{
					int num = i + j - windowSize / 2;
					if (num != i && num >= 0 && num < buffer.Length)
					{
						if (buffer[num] > buffer[i])
						{
							flag = false;
							break;
						}
						if (buffer[num] == buffer[i] && i < num)
						{
							flag = false;
							break;
						}
						if (buffer[num] > b2)
						{
							b2 = buffer[num];
						}
						if (buffer[num] < b)
						{
							b = buffer[num];
						}
					}
				}
				peaks[i] = (flag && b2 - b >= 15);
			}
		}
	}
}
