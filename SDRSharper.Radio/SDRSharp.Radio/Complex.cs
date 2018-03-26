using System;

namespace SDRSharp.Radio
{
	public struct Complex
	{
		public float Real;

		public float Imag;

		public Complex(float real, float imaginary)
		{
			this.Real = real;
			this.Imag = imaginary;
		}

		public Complex(Complex c)
		{
			this.Real = c.Real;
			this.Imag = c.Imag;
		}

		public static void Add(ref Complex result, Complex c1, Complex c2)
		{
			result.Real = c1.Real + c2.Real;
			result.Imag = c1.Imag + c2.Imag;
		}

		public static void Add(ref Complex result, Complex c, float f)
		{
			result.Real = c.Real + f;
			result.Imag = c.Imag;
		}

		public static void Add(ref Complex result, Complex c)
		{
			result.Real += c.Real;
			result.Imag += c.Imag;
		}

		public static void Add(ref Complex result, float f)
		{
			result.Real += f;
		}

		public static void Sub(ref Complex result, Complex c1, Complex c2)
		{
			result.Real = c1.Real - c2.Real;
			result.Imag = c1.Imag - c2.Imag;
		}

		public static void Sub(ref Complex result, Complex c, float f)
		{
			result.Real = c.Real - f;
			result.Imag = c.Imag;
		}

		public static void Mul(ref Complex result, Complex c1, Complex c2)
		{
			result.Real = c1.Real * c2.Real - c1.Imag * c2.Imag;
			result.Imag = c1.Imag * c2.Real + c1.Real * c2.Imag;
		}

		public static void Mul(ref Complex result, Complex c)
		{
			float real = result.Real;
			float imag = result.Imag;
			result.Real = real * c.Real - imag * c.Imag;
			result.Imag = imag * c.Real + real * c.Imag;
		}

		public static void Mul(ref Complex result, Complex c, float f)
		{
			result.Real = c.Real * f;
			result.Imag = c.Imag * f;
		}

		public static void Mul(ref Complex result, float f)
		{
			result.Real *= f;
			result.Imag *= f;
		}

		public static void Div(ref Complex result, Complex c, float f)
		{
			result.Real = c.Real / f;
			result.Imag = c.Imag / f;
		}

		public static void Div(ref Complex result, float f)
		{
			result.Real /= f;
			result.Imag /= f;
		}

		public float Modulus()
		{
			return (float)Math.Sqrt((double)this.ModulusSquared());
		}

		public float ModulusSquared()
		{
			return this.Real * this.Real + this.Imag * this.Imag;
		}

		public float Argument()
		{
			return (float)Math.Atan2((double)this.Imag, (double)this.Real);
		}

		public float FastArgument()
		{
			return Trig.Atan2(this.Imag, this.Real);
		}

		public static void Conjugate(ref Complex result, Complex c)
		{
			result.Real = c.Real;
			result.Imag = 0f - c.Imag;
		}

		public static void Conjugate(ref Complex c)
		{
			c.Imag = 0f - c.Imag;
		}

		public static void FromAngle(ref Complex result, double angle)
		{
			result.Real = (float)Math.Cos(angle);
			result.Imag = (float)Math.Sin(angle);
		}

		public static bool operator ==(Complex leftHandSide, Complex rightHandSide)
		{
			if (leftHandSide.Real != rightHandSide.Real)
			{
				return false;
			}
			return leftHandSide.Imag == rightHandSide.Imag;
		}

		public static bool operator !=(Complex leftHandSide, Complex rightHandSide)
		{
			if (leftHandSide.Real != rightHandSide.Real)
			{
				return true;
			}
			return leftHandSide.Imag != rightHandSide.Imag;
		}

		public override int GetHashCode()
		{
			return this.Real.GetHashCode() * 397 ^ this.Imag.GetHashCode();
		}

		public bool Equals(Complex obj)
		{
			if (obj.Real == this.Real)
			{
				return obj.Imag == this.Imag;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Complex))
			{
				return false;
			}
			return this.Equals((Complex)obj);
		}
	}
}
