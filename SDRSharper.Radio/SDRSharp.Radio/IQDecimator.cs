namespace SDRSharp.Radio
{
	public sealed class IQDecimator
	{
		private readonly bool _isMultithreaded;

		private readonly FloatDecimator _rDecimator;

		private readonly FloatDecimator _iDecimator;

		private readonly SharpEvent _event = new SharpEvent(false);

		public int StageCount => this._rDecimator.StageCount;

		public IQDecimator(int stageCount, double samplerate, bool useFastFilters, bool isMultithreaded)
		{
			this._isMultithreaded = (Utils.ProcessorCount > 1);
			int threadCount = (!this._isMultithreaded) ? 1 : (Utils.ProcessorCount / 2);
			DecimationFilterType filterType = (!useFastFilters) ? DecimationFilterType.Baseband : DecimationFilterType.Fast;
			this._rDecimator = new FloatDecimator(stageCount, samplerate, filterType, threadCount);
			this._iDecimator = new FloatDecimator(stageCount, samplerate, filterType, threadCount);
		}

		public IQDecimator(int stageCount, double samplerate, bool useFastFilters)
			: this(stageCount, samplerate, useFastFilters, false)
		{
		}

		public IQDecimator(int stageCount, double samplerate)
			: this(stageCount, samplerate, false)
		{
		}

		public unsafe void Process(Complex* buffer, int length)
		{
			float* buffer2 = (float*)((byte*)buffer + 4);
			if (this._isMultithreaded)
			{
				DSPThreadPool.QueueUserWorkItem(delegate
				{
					this._rDecimator.ProcessInterleaved((float*)buffer, length);
					this._event.Set();
				});
			}
			else
			{
				this._rDecimator.ProcessInterleaved((float*)buffer, length);
			}
			this._iDecimator.ProcessInterleaved(buffer2, length);
			if (this._isMultithreaded)
			{
				this._event.WaitOne();
			}
		}
	}
}
