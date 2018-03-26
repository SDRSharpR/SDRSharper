using System;
using System.Runtime.InteropServices;

namespace SDRSharp.Radio
{
	[StructLayout(LayoutKind.Sequential, Pack = 16, Size = 80)]
	public struct Oscillator
	{
		private double _sinOfAnglePerSample;

		private double _cosOfAnglePerSample;

		private double _vectR;

		private double _vectI;

		private double _outR;

		private double _outI;

		private double _sampleRate;

		private double _frequency;

		private double _anglePerSample;

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

		public double StateReal
		{
			get
			{
				return this._vectR;
			}
			set
			{
				this._vectR = value;
			}
		}

		public double StateImag
		{
			get
			{
				return this._vectI;
			}
			set
			{
				this._vectI = value;
			}
		}

		public double StateSin
		{
			get
			{
				return this._sinOfAnglePerSample;
			}
			set
			{
				this._sinOfAnglePerSample = value;
			}
		}

		public double StateCos
		{
			get
			{
				return this._cosOfAnglePerSample;
			}
			set
			{
				this._cosOfAnglePerSample = value;
			}
		}

		public Complex Out => new Complex((float)this._outR, (float)this._outI);

		public float OutR => (float)this._outR;

		public float OutI => (float)this._outI;

		private void Configure()
		{
			if (this._vectI == 0.0 && this._vectR == 0.0)
			{
				this._vectR = 1.0;
			}
			if (this._sampleRate != 0.0)
			{
				this._anglePerSample = 6.2831853071795862 * this._frequency / this._sampleRate;
				this._sinOfAnglePerSample = Math.Sin(this._anglePerSample);
				this._cosOfAnglePerSample = Math.Cos(this._anglePerSample);
			}
		}

		public void Tick()
		{
			this._outR = this._vectR * this._cosOfAnglePerSample - this._vectI * this._sinOfAnglePerSample;
			this._outI = this._vectI * this._cosOfAnglePerSample + this._vectR * this._sinOfAnglePerSample;
			double num = 1.95 - (this._vectR * this._vectR + this._vectI * this._vectI);
			this._vectR = num * this._outR;
			this._vectI = num * this._outI;
		}

		public unsafe void Mix(float* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				this.Tick();
				buffer[i] *= (float)this._outR;
			}
		}

		public unsafe void Mix(Complex* buffer, int length)
		{
			this.Mix(buffer, length, 0, 1);
		}

		public unsafe void Mix(Complex* buffer, int length, int startIndex, int stepSize)
		{
			for (int i = startIndex; i < length; i += stepSize)
			{
				this.Tick();
				Complex c = default(Complex);
				c.Real = (float)this._outR;
				c.Imag = (float)this._outI;
				Complex.Mul(ref buffer[i], c);
			}
		}

		public static implicit operator Complex(Oscillator osc)
		{
			return osc.Out;
		}
	}
}
