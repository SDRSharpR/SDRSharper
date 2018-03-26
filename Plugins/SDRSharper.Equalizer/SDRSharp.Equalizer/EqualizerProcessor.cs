using SDRSharp.Radio;

namespace SDRSharp.Equalizer
{
	public class EqualizerProcessor : IRealProcessor, IBaseProcessor
	{
		private bool _bypass;

		private SimpleEq _eq1 = new SimpleEq();

		private SimpleEq _eq2 = new SimpleEq();

		public double SampleRate
		{
			get
			{
				return this._eq1.SampleRate;
			}
			set
			{
				this._eq1.SampleRate = value;
				this._eq2.SampleRate = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return !this._bypass;
			}
			set
			{
				this._bypass = !value;
			}
		}

		public float LowGain
		{
			get
			{
				return this._eq1.LowGain;
			}
			set
			{
				this._eq1.LowGain = value;
				this._eq2.LowGain = value;
			}
		}

		public float MidGain
		{
			get
			{
				return this._eq1.MidGain;
			}
			set
			{
				this._eq1.MidGain = value;
				this._eq2.MidGain = value;
			}
		}

		public float HighGain
		{
			get
			{
				return this._eq1.HighGain;
			}
			set
			{
				this._eq1.HighGain = value;
				this._eq2.HighGain = value;
			}
		}

		public bool BassBoost
		{
			get
			{
				return this._eq1.BassBoost;
			}
			set
			{
				this._eq1.BassBoost = value;
				this._eq2.BassBoost = value;
			}
		}

		public float LowCutoff
		{
			get
			{
				return this._eq1.LowCutoff;
			}
			set
			{
				this._eq1.LowCutoff = value;
				this._eq2.LowCutoff = value;
			}
		}

		public float HighCutoff
		{
			get
			{
				return this._eq1.HighCutoff;
			}
			set
			{
				this._eq1.HighCutoff = value;
				this._eq2.HighCutoff = value;
			}
		}

		public unsafe void Process(float* audio, int length)
		{
			this._eq1.ProcessInterleaved(audio, length);
			this._eq2.ProcessInterleaved(audio + 1, length);
		}
	}
}
