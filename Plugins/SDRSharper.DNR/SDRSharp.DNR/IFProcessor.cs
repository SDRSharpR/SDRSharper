using SDRSharp.Radio;

namespace SDRSharp.DNR
{
	public class IFProcessor : INoiseProcessor, IIQProcessor, IBaseProcessor
	{
		private double _sampleRate;

		private bool _enabled;

		private NoiseFilter _filter;

		private bool _needNewFilter;

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
				this._needNewFilter = true;
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

		public unsafe void Process(Complex* buffer, int length)
		{
			if (this._needNewFilter)
			{
				this._filter = new NoiseFilter(4096);
				this._needNewFilter = false;
			}
			this._filter.NoiseThreshold = (float)this.NoiseThreshold;
			this._filter.Process(buffer, length);
		}
	}
}
