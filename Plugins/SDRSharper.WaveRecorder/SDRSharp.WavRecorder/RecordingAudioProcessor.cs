using SDRSharp.Radio;

namespace SDRSharp.WavRecorder
{
	public class RecordingAudioProcessor : IRealProcessor, IBaseProcessor
	{
		public unsafe delegate void AudioReadyDelegate(float* audio, int length);

		private bool _bypass;

		private double _sampleRate;

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

		public event AudioReadyDelegate AudioReady;

		public unsafe void Process(float* audio, int length)
		{
			AudioReadyDelegate audioReady = this.AudioReady;
			audioReady?.Invoke(audio, length);
		}
	}
}
