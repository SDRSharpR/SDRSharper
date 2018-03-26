using SDRSharp.Radio;
using System;

namespace SDRSharp.NoiseBlanker
{
	public class NoiseBlankerProcessor : IIQProcessor, IBaseProcessor
	{
		private const int SizeFactor = 2;

		private static readonly int AveragingWindow = 1000;

		private double _sampleRate;

		private bool _enabled;

		private bool _needConfigure;

		private int _averagingWindowLength;

		private int _blankingWindowLength;

		private int _index;

		private int _threshold;

		private double _pulseWidth;

		private float _ratio;

		private float _sum;

		private float _norm;

		private float _alpha;

		private UnsafeBuffer _delay;

		private unsafe Complex* _delayPtr;

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
				this._needConfigure = true;
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
			get
			{
				return this._threshold;
			}
			set
			{
				this._threshold = value;
				this._ratio = ((float)(100 - this._threshold) * 0.1f + 1f) * this._norm;
			}
		}

		public double PulseWidth
		{
			get
			{
				return this._pulseWidth;
			}
			set
			{
				this._pulseWidth = value;
				double num = Math.Min(Math.Max(this._pulseWidth * 1E-06 * this._sampleRate, 1.0), (double)this._averagingWindowLength);
				this._blankingWindowLength = (int)num;
				this._alpha = 1f / (float)this._blankingWindowLength;
			}
		}

		private unsafe void Configure()
		{
			this._index = 0;
			this._sum = 0f;
			this._averagingWindowLength = (int)((double)NoiseBlankerProcessor.AveragingWindow * 1E-06 * this._sampleRate);
			this._norm = 1f / (float)this._averagingWindowLength;
			this._ratio = ((float)(100 - this._threshold) * 0.1f + 1f) * this._norm;
			this._delay = UnsafeBuffer.Create((2 * this._averagingWindowLength + 1) * 2, sizeof(Complex));
			this._delayPtr = (Complex*)(void*)this._delay;
			double num = Math.Min(Math.Max(this._pulseWidth * 1E-06 * this._sampleRate, 1.0), (double)this._averagingWindowLength);
			this._blankingWindowLength = (int)num;
			this._alpha = 1f / (float)this._blankingWindowLength;
		}

		public unsafe void Process(Complex* buffer, int length)
		{
			if (this._needConfigure)
			{
				this.Configure();
				this._needConfigure = false;
			}
			for (int i = 0; i < length; i++)
			{
				Complex* ptr = this._delayPtr + this._index;
				*ptr = buffer[i];
				float num = ptr[this._averagingWindowLength].FastMagnitude();
				this._sum -= num;
				this._sum += (*ptr).FastMagnitude();
				if (num > this._ratio * this._sum)
				{
					Complex* ptr2 = ptr + this._averagingWindowLength;
					for (int j = 0; j < this._blankingWindowLength; j++)
					{
						Complex.Mul(ref ptr2[j], (float)j * this._alpha);
					}
				}
				buffer[i] = ptr[this._averagingWindowLength * 2];
				if (--this._index < 0)
				{
					this._index = 2 * this._averagingWindowLength + 1;
					Utils.Memcpy(this._delayPtr + this._index + 1, this._delayPtr, 2 * this._averagingWindowLength * sizeof(Complex));
				}
			}
		}
	}
}
