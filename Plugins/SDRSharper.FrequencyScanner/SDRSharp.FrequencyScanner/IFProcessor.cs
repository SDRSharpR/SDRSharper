using SDRSharp.Radio;

namespace SDRSharp.FrequencyScanner
{
	public class IFProcessor : IIQProcessor, IBaseProcessor
	{
		public unsafe delegate void IQReadyDelegate(Complex* buffer, int length);

		private double _sampleRate;

		private bool _enabled;

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
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

		public event IQReadyDelegate IQReady;

		public unsafe void Process(Complex* buffer, int length)
		{
			if (this.IQReady != null)
			{
				this.IQReady(buffer, length);
			}
		}
	}
}
