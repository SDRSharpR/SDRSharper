using SDRSharp.Radio;

namespace SDRSharp.Equalizer
{
	public class SimpleEq
	{
		private const int BassFilterCutoff = 150;

		private float _lowGain = 1f;

		private float _midGain = 1f;

		private float _highGain = 1f;

		private float _lowCutoff = 400f;

		private float _highCutoff = 5000f;

		private double _sampleRate = 48000.0;

		private bool _bassBoost;

		private IirFilter _lowFilter;

		private IirFilter _highFilter;

		private IirFilter _bassFilter;

		public float LowGain
		{
			get
			{
				return this._lowGain;
			}
			set
			{
				this._lowGain = value;
			}
		}

		public float HighGain
		{
			get
			{
				return this._highGain;
			}
			set
			{
				this._highGain = value;
			}
		}

		public float MidGain
		{
			get
			{
				return this._midGain;
			}
			set
			{
				this._midGain = value;
			}
		}

		public bool BassBoost
		{
			get
			{
				return this._bassBoost;
			}
			set
			{
				this._bassBoost = value;
			}
		}

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
				this.initLow();
				this.initHigh();
				this.initBass();
			}
		}

		public float LowCutoff
		{
			get
			{
				return this._lowCutoff;
			}
			set
			{
				this._lowCutoff = value;
				this.initLow();
			}
		}

		public float HighCutoff
		{
			get
			{
				return this._highCutoff;
			}
			set
			{
				this._highCutoff = value;
				this.initHigh();
			}
		}

		public SimpleEq()
		{
			this._lowFilter = default(IirFilter);
			this._highFilter = default(IirFilter);
			this._bassFilter = default(IirFilter);
			this.initLow();
			this.initHigh();
			this.initBass();
		}

		private void initBass()
		{
			this._bassFilter.Init(IirFilterType.LowPass, 150.0, this._sampleRate, 4);
		}

		private void initLow()
		{
			this._lowFilter.Init(IirFilterType.LowPass, (double)this._lowCutoff, this._sampleRate, 1);
		}

		private void initHigh()
		{
			this._highFilter.Init(IirFilterType.HighPass, (double)this._highCutoff, this._sampleRate, 1);
		}

		public unsafe void ProcessInterleaved(float* buffer, int length)
		{
			float num = 0f;
			for (int i = 0; i < length; i += 2)
			{
				float num2 = this._lowFilter.Process(buffer[i]);
				float num3 = this._highFilter.Process(buffer[i]);
				float num4 = buffer[i] - (num2 + num3);
				if (this._bassBoost)
				{
					num = this._bassFilter.Process(buffer[i]);
				}
				num2 *= this._lowGain;
				num4 *= this._midGain;
				num3 *= this._highGain;
				buffer[i] = num2 + num4 + num3;
				if (this._bassBoost)
				{
					num *= 0.8f;
					buffer[i] += num;
				}
				buffer[i] *= 0.5f;
			}
		}
	}
}
