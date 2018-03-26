namespace SDRSharp.Radio
{
	public sealed class CwDetector
	{
		private Oscillator _bfo = default(Oscillator);

		private Complex _tmp = default(Complex);

		public double SampleRate
		{
			get
			{
				return this._bfo.SampleRate;
			}
			set
			{
				this._bfo.SampleRate = value;
			}
		}

		public int BfoFrequency
		{
			get
			{
				return (int)this._bfo.Frequency;
			}
			set
			{
				this._bfo.Frequency = (double)value;
			}
		}

		public unsafe void Demodulate(Complex* iq, float* audio, int length)
		{
			for (int i = 0; i < length; i++)
			{
				this._bfo.Tick();
				Complex.Mul(ref this._tmp, iq[i], this._bfo);
				audio[i] = this._tmp.Real;
			}
		}
	}
}
