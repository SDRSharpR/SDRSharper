using System.Runtime.InteropServices;

namespace SDRSharp.Radio
{
	[StructLayout(LayoutKind.Sequential, Pack = 16, Size = 96)]
	public struct CicDecimator
	{
		private float _xOdd;

		private float _xEven;

		public float XOdd
		{
			get
			{
				return this._xOdd;
			}
			set
			{
				this._xOdd = value;
			}
		}

		public float XEven
		{
			get
			{
				return this._xEven;
			}
			set
			{
				this._xEven = value;
			}
		}

		public unsafe void Process(float* buffer, int length)
		{
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float num3 = buffer[num];
				float num4 = buffer[num + 1];
				buffer[num2] = (float)(0.125 * ((double)(num4 + this._xEven) + 3.0 * (double)(this._xOdd + num3)));
				this._xOdd = num4;
				this._xEven = num3;
				num += 2;
				num2++;
			}
		}

		public unsafe void ProcessInterleaved(float* buffer, int length)
		{
			length *= 2;
			int num = 0;
			int num2 = 0;
			while (num < length)
			{
				float num3 = buffer[num];
				float num4 = buffer[num + 2];
				buffer[num2] = 0.125f * (num4 + this._xEven + 3f * (this._xOdd + num3));
				this._xOdd = num4;
				this._xEven = num3;
				num += 4;
				num2 += 2;
			}
		}
	}
}
