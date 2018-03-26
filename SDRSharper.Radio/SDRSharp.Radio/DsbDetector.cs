namespace SDRSharp.Radio
{
	public sealed class DsbDetector
	{
		public unsafe void Demodulate(Complex* iq, float* audio, int length)
		{
			for (int i = 0; i < length; i++)
			{
				audio[i] = iq[i].Real;
			}
		}
	}
}
