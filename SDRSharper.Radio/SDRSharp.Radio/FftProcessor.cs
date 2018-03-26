using System;

namespace SDRSharp.Radio
{
	public abstract class FftProcessor
	{
		private readonly int _fftSize;

		private readonly int _halfSize;

		private readonly int _overlapSize;

		private readonly float _blendFactor;

		private int _fftBufferPos;

		private int _sampleBufferHead;

		private int _sampleBufferTail;

		private UnsafeBuffer _fftBuffer;

		private unsafe Complex* _fftBufferPtr;

		private UnsafeBuffer _overlapBuffer;

		private unsafe Complex* _overlapBufferPtr;

		private UnsafeBuffer _outOverlapBuffer;

		private unsafe Complex* _outOverlapPtr;

		private UnsafeBuffer _sampleBuffer;

		private unsafe Complex* _sampleBufferPtr;

		private Complex _tmp = default(Complex);

		public unsafe FftProcessor(int fftSize, float overlapRatio = 0f)
		{
			this._fftSize = fftSize;
			this._halfSize = fftSize / 2;
			this._overlapSize = (int)Math.Ceiling((double)((float)this._fftSize * overlapRatio));
			this._fftBufferPos = this._halfSize;
			this._blendFactor = 1f / (float)this._overlapSize;
			this._fftBuffer = UnsafeBuffer.Create(fftSize, sizeof(Complex));
			this._fftBufferPtr = (Complex*)(void*)this._fftBuffer;
			this._outOverlapBuffer = UnsafeBuffer.Create(this._overlapSize, sizeof(Complex));
			this._outOverlapPtr = (Complex*)(void*)this._outOverlapBuffer;
			this._overlapBuffer = UnsafeBuffer.Create(fftSize / 2, sizeof(Complex));
			this._overlapBufferPtr = (Complex*)(void*)this._overlapBuffer;
			this._sampleBuffer = UnsafeBuffer.Create(fftSize, sizeof(Complex));
			this._sampleBufferPtr = (Complex*)(void*)this._sampleBuffer;
			this._sampleBufferHead = this._halfSize;
		}

		public unsafe void Process(Complex* buffer, int length)
		{
			int i = 0;
			int j = 0;
			for (; i < length; i++)
			{
				this._fftBufferPtr[this._fftBufferPos++] = buffer[i];
				if (this._fftBufferPos == this._fftSize)
				{
					int num = this._halfSize;
					int num2 = 0;
					while (num < this._fftSize)
					{
						this._overlapBufferPtr[num2] = this._fftBufferPtr[num];
						num++;
						num2++;
					}
					for (; j < length; j++)
					{
						if (this._sampleBufferHead == this._sampleBufferTail)
						{
							break;
						}
						buffer[j] = this._sampleBufferPtr[this._sampleBufferTail];
						this._sampleBufferTail = (this._sampleBufferTail + 1 & this._fftSize - 1);
					}
					Fourier.ForwardTransform(this._fftBufferPtr, this._fftSize, false);
					this.ProcessFft(this._fftBufferPtr, this._fftSize);
					Fourier.BackwardTransform(this._fftBufferPtr, this._fftSize);
					int num3 = 0;
					int num4 = this._halfSize - this._overlapSize;
					while (num3 < this._halfSize)
					{
						if (num3 < this._overlapSize)
						{
							float num5 = (float)num3 * this._blendFactor;
							Complex.Mul(ref this._sampleBufferPtr[this._sampleBufferHead], this._fftBufferPtr[num4], num5);
							Complex.Mul(ref this._tmp, this._outOverlapPtr[num3], 1f - num5);
							Complex.Add(ref this._sampleBufferPtr[this._sampleBufferHead], this._tmp);
						}
						else
						{
							this._sampleBufferPtr[this._sampleBufferHead] = this._fftBufferPtr[num4];
						}
						this._sampleBufferHead = (this._sampleBufferHead + 1 & this._fftSize - 1);
						num3++;
						num4++;
					}
					int num6 = 0;
					int num7 = this._fftSize - this._overlapSize;
					while (num6 < this._overlapSize)
					{
						this._outOverlapPtr[num6] = this._fftBufferPtr[num7];
						num6++;
						num7++;
					}
					for (int k = 0; k < this._halfSize; k++)
					{
						this._fftBufferPtr[k] = this._overlapBufferPtr[k];
					}
					this._fftBufferPos = this._halfSize;
				}
			}
			for (; j < length; j++)
			{
				if (this._sampleBufferHead == this._sampleBufferTail)
				{
					break;
				}
				buffer[j] = this._sampleBufferPtr[this._sampleBufferTail];
				this._sampleBufferTail = (this._sampleBufferTail + 1 & this._fftSize - 1);
			}
		}

		public unsafe void Process(float* buffer, int length, int step = 1)
		{
			int i = 0;
			int j = 0;
			for (; i < length; i += step)
			{
				this._fftBufferPtr[this._fftBufferPos].Real = buffer[i];
				this._fftBufferPtr[this._fftBufferPos++].Imag = 0f;
				if (this._fftBufferPos == this._fftSize)
				{
					int num = this._halfSize;
					int num2 = 0;
					while (num < this._fftSize)
					{
						this._overlapBufferPtr[num2].Real = this._fftBufferPtr[num].Real;
						this._overlapBufferPtr[num2].Imag = 0f;
						num++;
						num2++;
					}
					for (; j < length; j += step)
					{
						if (this._sampleBufferHead == this._sampleBufferTail)
						{
							break;
						}
						buffer[j] = this._sampleBufferPtr[this._sampleBufferTail].Real;
						this._sampleBufferTail = (this._sampleBufferTail + 1 & this._fftSize - 1);
					}
					Fourier.ForwardTransform(this._fftBufferPtr, this._fftSize, false);
					this.ProcessFft(this._fftBufferPtr, this._fftSize);
					Fourier.BackwardTransform(this._fftBufferPtr, this._fftSize);
					int num3 = 0;
					int num4 = this._halfSize - this._overlapSize;
					while (num3 < this._halfSize)
					{
						if (num3 < this._overlapSize)
						{
							float num5 = (float)num3 * this._blendFactor;
							Complex.Mul(ref this._sampleBufferPtr[this._sampleBufferHead], this._fftBufferPtr[num4], num5);
							Complex.Mul(ref this._tmp, this._outOverlapPtr[num3], 1f - num5);
							Complex.Add(ref this._sampleBufferPtr[this._sampleBufferHead], this._tmp);
						}
						else
						{
							this._sampleBufferPtr[this._sampleBufferHead].Real = this._fftBufferPtr[num4].Real;
							this._sampleBufferPtr[this._sampleBufferHead].Imag = 0f;
						}
						this._sampleBufferHead = (this._sampleBufferHead + 1 & this._fftSize - 1);
						num3++;
						num4++;
					}
					int num6 = 0;
					int num7 = this._fftSize - this._overlapSize;
					while (num6 < this._overlapSize)
					{
						this._outOverlapPtr[num6].Real = this._fftBufferPtr[num7].Real;
						this._outOverlapPtr[num6].Imag = 0f;
						num6++;
						num7++;
					}
					for (int k = 0; k < this._halfSize; k++)
					{
						this._fftBufferPtr[k] = this._overlapBufferPtr[k];
					}
					this._fftBufferPos = this._halfSize;
				}
			}
			for (; j < length; j += step)
			{
				if (this._sampleBufferHead == this._sampleBufferTail)
				{
					break;
				}
				buffer[j] = this._sampleBufferPtr[this._sampleBufferTail].Real;
				this._sampleBufferTail = (this._sampleBufferTail + 1 & this._fftSize - 1);
			}
		}

		protected unsafe abstract void ProcessFft(Complex* buffer, int length);
	}
}
