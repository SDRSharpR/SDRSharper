using SDRSharp.Radio;

namespace SDRSharp.WavRecorder
{
	public class RecordingIQObserver : IIQProcessor, IBaseProcessor
	{
		public unsafe delegate void IQReadyDelegate(Complex* buffer, int length);

		private volatile bool _enabled;

		private double _sampleRate;

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

		public event IQReadyDelegate IQReady;

		public unsafe void Process(Complex* buffer, int length)
		{
			IQReadyDelegate iQReady = this.IQReady;
			if (iQReady != null)
			{
				this.IQReady(buffer, length);
			}
		}
	}
}
