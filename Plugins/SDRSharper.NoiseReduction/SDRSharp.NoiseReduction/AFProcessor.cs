using SDRSharp.Radio;

namespace SDRSharp.NoiseReduction
{
	public class AFProcessor : INoiseProcessor, IRealProcessor, IBaseProcessor
	{
		private double _sampleRate;

		private bool _enabled;

		private NoiseFilter _filter1;

		private NoiseFilter _filter2;

		private bool _needNewFilters;

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
				this._needNewFilters = true;
			}
		}

		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		public int NoiseThreshold
		{
			get;
			set;
		}

		public unsafe void Process(float* buffer, int length)
		{
			if (this._needNewFilters)
			{
				this._filter1 = new NoiseFilter(4096);
				this._filter2 = new NoiseFilter(4096);
				this._needNewFilters = false;
			}
			this._filter1.NoiseThreshold = (float)this.NoiseThreshold;
			this._filter2.NoiseThreshold = (float)this.NoiseThreshold;
			this._filter1.Process(buffer, length, 2);
			this._filter2.Process(buffer + 1, length, 2);
		}
	}
}
