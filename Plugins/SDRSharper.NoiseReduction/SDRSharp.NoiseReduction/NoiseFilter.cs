using SDRSharp.Radio;

namespace SDRSharp.NoiseReduction
{
	public class NoiseFilter : FftProcessor
	{
		private const int WindowSize = 32;

		private const int FftSize = 4096;

		private const float OverlapRatio = 0.2f;

		private float _noiseThreshold;

		private readonly UnsafeBuffer _gainBuffer;

		private unsafe readonly float* _gainPtr;

		private readonly UnsafeBuffer _smoothedGainBuffer;

		private unsafe readonly float* _smoothedGainPtr;

		private readonly UnsafeBuffer _powerBuffer;

		private unsafe readonly float* _powerPtr;

		public float NoiseThreshold
		{
			get
			{
				return this._noiseThreshold;
			}
			set
			{
				this._noiseThreshold = value;
			}
		}

		public unsafe NoiseFilter(int fftSize = 4096)
			: base(fftSize, 0.2f)
		{
			this._gainBuffer = UnsafeBuffer.Create(fftSize, 4);
			this._gainPtr = (float*)(void*)this._gainBuffer;
			this._smoothedGainBuffer = UnsafeBuffer.Create(fftSize, 4);
			this._smoothedGainPtr = (float*)(void*)this._smoothedGainBuffer;
			this._powerBuffer = UnsafeBuffer.Create(fftSize, 4);
			this._powerPtr = (float*)(void*)this._powerBuffer;
		}

		protected unsafe override void ProcessFft(Complex* buffer, int length)
		{
			Fourier.SpectrumPower(buffer, this._powerPtr, length, 0f, false);
			for (int i = 0; i < length; i++)
			{
				this._gainPtr[i] = ((this._powerPtr[i] > this._noiseThreshold) ? 1f : 0f);
			}
			for (int j = 0; j < length; j++)
			{
				float num = 0f;
				for (int k = -16; k < 16; k++)
				{
					int num2 = j + k;
					if (num2 >= length)
					{
						num2 -= length;
					}
					if (num2 < 0)
					{
						num2 += length;
					}
					num += this._gainPtr[num2];
				}
				float num3 = num / 32f;
				this._smoothedGainPtr[j] = num3;
			}
			for (int l = 0; l < length; l++)
			{
				Complex.Mul(ref buffer[l], this._smoothedGainPtr[l]);
			}
		}
	}
}
