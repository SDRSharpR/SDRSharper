using System;
using System.Threading;

namespace SDRSharp.Radio
{
	public sealed class DownConverter
	{
		private readonly int _phaseCount;

		private readonly UnsafeBuffer _oscillatorsBuffer;

		private unsafe readonly Oscillator* _oscillators;

		private readonly SharpEvent _event = new SharpEvent(false);

		private readonly bool _isMultithreaded;

		private double _sampleRate;

		private double _frequency;

		private int _completedCount;

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				if (this._sampleRate != value)
				{
					this._sampleRate = value;
					this.Configure();
				}
			}
		}

		public double Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				if (this._frequency != value)
				{
					this._frequency = value;
					this.Configure();
				}
			}
		}

		public unsafe DownConverter(int phaseCount)
		{
			this._phaseCount = phaseCount;
			this._oscillatorsBuffer = UnsafeBuffer.Create(sizeof(Oscillator) * phaseCount);
			this._oscillators = (Oscillator*)(void*)this._oscillatorsBuffer;
			this._isMultithreaded = (Utils.ProcessorCount > 1);
		}

		public DownConverter()
			: this(Utils.ProcessorCount)
		{
		}

		private unsafe void Configure()
		{
			if (this._sampleRate != 0.0)
			{
				double frequency = this._frequency * (double)this._phaseCount;
				for (int i = 0; i < this._phaseCount; i++)
				{
					this._oscillators[i].SampleRate = this._sampleRate;
					this._oscillators[i].Frequency = frequency;
				}
				double num = 6.2831853071795862 * this._frequency / this._sampleRate;
				double num2 = Math.Sin(num);
				double num3 = Math.Cos(num);
				double num4 = this._oscillators->StateReal;
				double num5 = this._oscillators->StateImag;
				for (int j = 1; j < this._phaseCount; j++)
				{
					double num6 = num4 * num3 - num5 * num2;
					double num7 = num5 * num3 + num4 * num2;
					double num8 = 1.95 - (num4 * num4 + num5 * num5);
					num4 = num8 * num6;
					num5 = num8 * num7;
					this._oscillators[j].StateReal = num4;
					this._oscillators[j].StateImag = num5;
				}
			}
		}

		public unsafe void Process(Complex* buffer, int length)
		{
			if (this._isMultithreaded)
			{
				this._completedCount = 0;
				for (int i = 1; i < this._phaseCount; i++)
				{
					DSPThreadPool.QueueUserWorkItem(delegate(object parameter)
					{
						int num2 = (int)parameter;
						this._oscillators[num2].Mix(buffer, length, num2, this._phaseCount);
						Interlocked.Increment(ref this._completedCount);
						this._event.Set();
					}, i);
				}
				this._oscillators->Mix(buffer, length, 0, this._phaseCount);
				if (this._phaseCount > 1)
				{
					Interlocked.Increment(ref this._completedCount);
					while (this._completedCount < this._phaseCount)
					{
						this._event.WaitOne();
					}
				}
			}
			else
			{
				for (int j = 1; j < this._phaseCount; j++)
				{
					int num = j;
					this._oscillators[num].Mix(buffer, length, num, this._phaseCount);
				}
				this._oscillators->Mix(buffer, length, 0, this._phaseCount);
			}
		}
	}
}
