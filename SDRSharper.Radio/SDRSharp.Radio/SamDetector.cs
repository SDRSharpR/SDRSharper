using System;

namespace SDRSharp.Radio
{
	public sealed class SamDetector
	{
		private const float ZETA = 0.707f;

		private float TWOPI = 6.28318548f;

		private float _sampleRate = 48000f;

		private float _norm = 1f;

		private float _fDefault;

		private float _bw = 400f;

		private float _range = 500f;

		private float _lockTreshold;

		private float _lockTime;

		private float _phase;

		private float _dcReal;

		private float _dcImag;

		private float _freqN;

		private float _bwN;

		private float _fLowN;

		private float _fhighN;

		private float _alpha;

		private float _beta;

		private float _errorAvg;

		private float _lockAlpha;

		private float _lockOneMinAlpha;

		private CuteFir _cuteFir = new CuteFir();

		private UnsafeBuffer _cpxBuf;

		private unsafe Complex* _cpxPtr;

		public float SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
				this._norm = this.TWOPI / this._sampleRate;
				this._cuteFir.InitLPFilter(0, 1.0, 40.0, 4500f, 5500f, this._sampleRate);
				this._cuteFir.GenerateHBFilter(5000f);
			}
		}

		public float Frequency
		{
			get
			{
				return (0f - this._freqN) / this._norm;
			}
			set
			{
				this._fDefault = value;
				this._freqN = this._fDefault * this._norm;
			}
		}

		public float Range
		{
			get
			{
				return this._range;
			}
			set
			{
				this._range = value;
				this._fLowN = (this._fDefault - this._range) * this._norm;
				this._fhighN = (this._fDefault + this._range) * this._norm;
			}
		}

		public bool IsLocked => this._errorAvg < this._lockTreshold;

		public float LockTreshold
		{
			set
			{
				this._lockTreshold = value;
			}
		}

		public float LockTime
		{
			set
			{
				this._lockTime = value;
			}
		}

		public float BandWidth
		{
			get
			{
				return this._bw;
			}
			set
			{
				this._bw = value;
				this._bwN = this._bw * this._norm;
				this._alpha = 0.3f * this._bwN;
				this._beta = this._alpha * this._alpha * 0.25f;
				this._lockAlpha = (float)(1.0 - Math.Exp(-1.0 / (double)(this._sampleRate * this._lockTime)));
				this._lockOneMinAlpha = 1f - this._lockAlpha;
			}
		}

		public unsafe void Demodulate(Complex* iq, float* audio, int length, bool stereo = false, bool filter = false)
		{
			for (int i = 0; i < length; i++)
			{
				float num = (float)Math.Cos((double)this._phase);
				float num2 = (float)Math.Sin((double)this._phase);
				float num3 = num * iq[i].Real + num2 * iq[i].Imag;
				float num4 = (0f - num2) * iq[i].Real + num * iq[i].Imag;
				float num5 = (float)Math.Atan2((double)num4, (double)num3);
				this._freqN += this._beta * num5;
				if (this._freqN < this._fLowN)
				{
					this.Frequency = this._fLowN;
				}
				else if (this._freqN > this._fhighN)
				{
					this.Frequency = this._fhighN;
				}
				this._phase += this._freqN + this._alpha * num5;
				while (this._phase >= this.TWOPI)
				{
					this._phase -= this.TWOPI;
				}
				while (this._phase < 0f)
				{
					this._phase += this.TWOPI;
				}
				this._dcReal = 0.9999f * this._dcReal + 0.0001f * num3;
				this._errorAvg = this._lockOneMinAlpha * this._errorAvg + this._lockAlpha * num5 * num5;
				audio[i] = (num3 - this._dcReal) * 0.002f;
			}
		}

		public unsafe void Demodulate(Complex* iq, float* rPtr, float* lPtr, int length)
		{
			if (this._cpxBuf == null || this._cpxBuf.Length != length)
			{
				if (this._cpxBuf != null)
				{
					this._cpxBuf.Dispose();
				}
				this._cpxBuf = UnsafeBuffer.Create(length, sizeof(Complex));
				this._cpxPtr = (Complex*)(void*)this._cpxBuf;
			}
			for (int i = 0; i < length; i++)
			{
				float num = (float)Math.Cos((double)this._phase);
				float num2 = (float)Math.Sin((double)this._phase);
				float num3 = num * iq[i].Real + num2 * iq[i].Imag;
				float num4 = (0f - num2) * iq[i].Real + num * iq[i].Imag;
				float num5 = (float)Math.Atan2((double)num4, (double)num3);
				this._freqN += this._beta * num5;
				this._freqN = Math.Max(this._fLowN, Math.Min(this._fhighN, this._freqN));
				this._phase += this._freqN + this._alpha * num5;
				this._phase %= this.TWOPI;
				this._dcReal = 0.9999f * this._dcReal + 0.0001f * num3;
				this._dcImag = 0.9999f * this._dcImag + 0.0001f * num4;
				this._cpxPtr[i].Real = num3 - this._dcReal;
				this._cpxPtr[i].Imag = num4 - this._dcImag;
			}
			this._cuteFir.ProcessFilter(length, this._cpxPtr, this._cpxPtr);
			for (int j = 0; j < length; j++)
			{
				Complex complex = this._cpxPtr[j];
				rPtr[j] = (complex.Real - complex.Imag) * 0.002f;
				lPtr[j] = (complex.Real + complex.Imag) * 0.002f;
			}
		}
	}
}
