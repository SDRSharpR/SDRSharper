namespace SDRSharp.Radio
{
	public sealed class FloatDecimator
	{
		private readonly int _stageCount;

		private readonly int _threadCount;

		private readonly int _cicCount;

		private readonly UnsafeBuffer _cicDecimatorsBuffer;

		private unsafe readonly CicDecimator* _cicDecimators;

		private readonly FirFilter[] _firFilters;

		private static readonly double _minimumCICSampleRate = Utils.GetDoubleSetting("minimumCICSampleRate", 1500000.0);

		public int StageCount => this._stageCount;

		public FloatDecimator(int stageCount)
			: this(stageCount, 0.0, DecimationFilterType.Audio, 1)
		{
		}

		public FloatDecimator(int stageCount, double samplerate, DecimationFilterType filterType)
			: this(stageCount, samplerate, filterType, 1)
		{
		}

		public unsafe FloatDecimator(int stageCount, double samplerate, DecimationFilterType filterType, int threadCount)
		{
			this._stageCount = stageCount;
			this._threadCount = threadCount;
			this._cicCount = 0;
			int num = 0;
			switch (filterType)
			{
			case DecimationFilterType.Fast:
				this._cicCount = stageCount;
				break;
			case DecimationFilterType.Audio:
				num = stageCount;
				break;
			case DecimationFilterType.Baseband:
				while (this._cicCount < stageCount && samplerate >= FloatDecimator._minimumCICSampleRate)
				{
					this._cicCount++;
					samplerate /= 2.0;
				}
				num = stageCount - this._cicCount;
				break;
			}
			this._cicDecimatorsBuffer = UnsafeBuffer.Create(this._threadCount * this._cicCount, sizeof(CicDecimator));
			this._cicDecimators = (CicDecimator*)(void*)this._cicDecimatorsBuffer;
			for (int i = 0; i < this._threadCount; i++)
			{
				for (int j = 0; j < this._cicCount; j++)
				{
					this._cicDecimators[i * this._cicCount + j] = default(CicDecimator);
				}
			}
			this._firFilters = new FirFilter[num];
			for (int k = 0; k < num; k++)
			{
				this._firFilters[k] = new FirFilter(DecimationKernels.Kernel51, 2);
			}
		}

		public unsafe void Process(float* buffer, int length)
		{
			this.DecimateStage1(buffer, length);
			length >>= this._cicCount;
			this.DecimateStage2(buffer, length);
		}

		public unsafe void ProcessInterleaved(float* buffer, int length)
		{
			this.DecimateStage1Interleaved(buffer, length);
			length >>= this._cicCount;
			this.DecimateStage2Interleaved(buffer, length);
		}

		private unsafe void DecimateStage1(float* buffer, int sampleCount)
		{
			for (int i = 0; i < this._cicCount; i++)
			{
				int num = 0;
				int length = sampleCount;
				this._cicDecimators[num * this._cicCount + i].Process(buffer, length);
				sampleCount /= 2;
			}
		}

		private unsafe void DecimateStage2(float* buffer, int length)
		{
			for (int i = 0; i < this._firFilters.Length; i++)
			{
				this._firFilters[i].Process(buffer, length);
				length /= 2;
			}
		}

		private unsafe void DecimateStage1Interleaved(float* buffer, int sampleCount)
		{
			for (int i = 0; i < this._cicCount; i++)
			{
				int num = 0;
				int length = sampleCount;
				this._cicDecimators[num * this._cicCount + i].ProcessInterleaved(buffer, length);
				sampleCount /= 2;
			}
		}

		private unsafe void DecimateStage2Interleaved(float* buffer, int length)
		{
			for (int i = 0; i < this._firFilters.Length; i++)
			{
				this._firFilters[i].ProcessInterleaved(buffer, length);
				length /= 2;
			}
		}
	}
}
