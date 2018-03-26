namespace SDRSharp.Radio
{
	public sealed class SsbDetector
	{
		public enum Mode
		{
			LSB,
			USB
		}

		private Mode _mode;

		private Oscillator _bfo = default(Oscillator);

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

		public SsbDetector(Mode mode)
		{
			this._mode = mode;
		}

		public unsafe void Demodulate(Complex* iq, float* audio, int length)
		{
			if (this._mode == Mode.LSB)
			{
				for (int i = 0; i < length; i++)
				{
					this._bfo.Tick();
					audio[i] = iq[i].Real * this._bfo.OutI - iq[i].Imag * this._bfo.OutR;
				}
			}
			else
			{
				for (int j = 0; j < length; j++)
				{
					this._bfo.Tick();
					audio[j] = iq[j].Real * this._bfo.OutI + iq[j].Imag * this._bfo.OutR;
				}
			}
		}
	}
}
