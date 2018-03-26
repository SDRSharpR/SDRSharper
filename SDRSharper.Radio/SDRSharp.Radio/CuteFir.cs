using System;

namespace SDRSharp.Radio
{
	public class CuteFir
	{
		private const int MAX_NUMCOEF = 75;

		private float _sampleRate;

		private int _numTaps;

		private int _state;

		private float[] _coef = new float[150];

		private float[] _iCoef = new float[150];

		private float[] _qCoef = new float[150];

		private float[] _rZBuf = new float[75];

		private Complex[] _cZBuf = new Complex[75];

		public CuteFir()
		{
			this._numTaps = 1;
			this._state = 0;
		}

		public unsafe void ProcessFilter(int InLength, float* InBuf, float* OutBuf)
		{
			lock (this)
			{
				for (int i = 0; i < InLength; i++)
				{
					this._rZBuf[this._state] = InBuf[i];
					fixed (float* ptr = &this._coef[this._numTaps - this._state])
					{
						fixed (float* ptr3 = &this._rZBuf[0])
						{
							float* ptr2 = ptr;
							float* ptr4 = ptr3;
							float* intPtr = ptr2;
							ptr2 = intPtr + 1;
							float num = *intPtr;
							float* intPtr2 = ptr4;
							ptr4 = intPtr2 + 1;
							float num2 = num * *intPtr2;
							for (int j = 1; j < this._numTaps; j++)
							{
								float num3 = num2;
								float* intPtr3 = ptr2;
								ptr2 = intPtr3 + 1;
								float num4 = *intPtr3;
								float* intPtr4 = ptr4;
								ptr4 = intPtr4 + 1;
								num2 = num3 + num4 * *intPtr4;
							}
							if (--this._state < 0)
							{
								this._state += this._numTaps;
							}
							OutBuf[i] = num2;
						}
					}
				}
			}
		}

		public unsafe void ProcessFilter(int InLength, Complex* InBuf, Complex* OutBuf)
		{
			lock (this)
			{
				for (int i = 0; i < InLength; i++)
				{
					this._cZBuf[this._state] = InBuf[i];
					float[] iCoef = this._iCoef;
					fixed (float* ptr = iCoef)
					{
						float[] qCoef = this._qCoef;
						fixed (float* ptr3 = qCoef)
						{
							Complex[] cZBuf = this._cZBuf;
							fixed (Complex* ptr5 = cZBuf)
							{
								float* ptr2 = ptr + this._numTaps - this._state;
								float* ptr4 = ptr3 + this._numTaps - this._state;
								Complex* ptr6 = ptr5;
								float* intPtr = ptr2;
								ptr2 = intPtr + 1;
								Complex complex = default(Complex);
								complex.Real = *intPtr * ptr6->Real;
								float* intPtr2 = ptr4;
								ptr4 = intPtr2 + 1;
								float num = *intPtr2;
								Complex* intPtr3 = ptr6;
								ptr6 = intPtr3 + 1;
								complex.Imag = num * intPtr3->Imag;
								for (int j = 1; j < this._numTaps; j++)
								{
									float real = complex.Real;
									float* intPtr4 = ptr2;
									ptr2 = intPtr4 + 1;
									complex.Real = real + *intPtr4 * ptr6->Real;
									float imag = complex.Imag;
									float* intPtr5 = ptr4;
									ptr4 = intPtr5 + 1;
									float num2 = *intPtr5;
									Complex* intPtr6 = ptr6;
									ptr6 = intPtr6 + 1;
									complex.Imag = imag + num2 * intPtr6->Imag;
								}
								if (--this._state < 0)
								{
									this._state += this._numTaps;
								}
								OutBuf[i] = complex;
							}
						}
					}
				}
			}
		}

		public unsafe void ProcessFilter(int InLength, float* InBuf, Complex* OutBuf)
		{
			lock (this)
			{
				for (int i = 0; i < InLength; i++)
				{
					this._cZBuf[this._state].Real = InBuf[i];
					this._cZBuf[this._state].Imag = InBuf[i];
					float[] iCoef = this._iCoef;
					fixed (float* ptr = iCoef)
					{
						float[] qCoef = this._qCoef;
						fixed (float* ptr3 = qCoef)
						{
							Complex[] cZBuf = this._cZBuf;
							fixed (Complex* ptr5 = cZBuf)
							{
								float* ptr2 = ptr + this._numTaps - this._state;
								float* ptr4 = ptr3 + this._numTaps - this._state;
								Complex* ptr6 = ptr5;
								float* intPtr = ptr2;
								ptr2 = intPtr + 1;
								Complex complex = default(Complex);
								complex.Real = *intPtr * ptr6->Real;
								float* intPtr2 = ptr4;
								ptr4 = intPtr2 + 1;
								float num = *intPtr2;
								Complex* intPtr3 = ptr6;
								ptr6 = intPtr3 + 1;
								complex.Imag = num * intPtr3->Imag;
								for (int j = 1; j < this._numTaps; j++)
								{
									float real = complex.Real;
									float* intPtr4 = ptr2;
									ptr2 = intPtr4 + 1;
									complex.Real = real + *intPtr4 * ptr6->Real;
									float imag = complex.Imag;
									float* intPtr5 = ptr4;
									ptr4 = intPtr5 + 1;
									float num2 = *intPtr5;
									Complex* intPtr6 = ptr6;
									ptr6 = intPtr6 + 1;
									complex.Imag = imag + num2 * intPtr6->Imag;
								}
								if (--this._state < 0)
								{
									this._state += this._numTaps;
								}
								OutBuf[i] = complex;
							}
						}
					}
				}
			}
		}

		public unsafe void InitConstFir(int numTaps, double* pCoef, float samprate)
		{
			lock (this)
			{
				this._sampleRate = samprate;
				if (numTaps > 75)
				{
					this._numTaps = 75;
				}
				else
				{
					this._numTaps = numTaps;
				}
				for (int i = 0; i < this._numTaps; i++)
				{
					this._coef[i] = (float)pCoef[i];
					this._coef[this._numTaps + i] = (float)pCoef[i];
				}
				for (int j = 0; j < this._numTaps; j++)
				{
					this._rZBuf[j] = 0f;
					this._cZBuf[j].Real = 0f;
					this._cZBuf[j].Imag = 0f;
				}
				this._state = 0;
			}
		}

		public unsafe void InitConstFir(int numTaps, double* pICoef, double* pQCoef, float samprate)
		{
			lock (this)
			{
				this._sampleRate = samprate;
				if (numTaps > 75)
				{
					this._numTaps = 75;
				}
				else
				{
					this._numTaps = numTaps;
				}
				for (int i = 0; i < this._numTaps; i++)
				{
					this._iCoef[i] = (float)pICoef[i];
					this._iCoef[this._numTaps + i] = (float)pICoef[i];
					this._qCoef[i] = (float)pQCoef[i];
					this._qCoef[this._numTaps + i] = (float)pQCoef[i];
				}
				for (int j = 0; j < this._numTaps; j++)
				{
					this._rZBuf[j] = 0f;
					this._cZBuf[j].Real = 0f;
					this._cZBuf[j].Imag = 0f;
				}
				this._state = 0;
			}
		}

		public int InitLPFilter(int numTaps, double scale, double aStop, float fPass, float fStop, float samprate)
		{
			lock (this)
			{
				this._sampleRate = samprate;
				float num = fPass / samprate;
				float num2 = fStop / samprate;
				float num3 = (num2 + num) / 2f;
				double num4 = (!(aStop < 20.96)) ? ((!(aStop >= 50.0)) ? (0.5842 * Math.Pow(aStop - 20.96, 0.4) + 0.07886 * (aStop - 20.96)) : ((double)(float)(0.1102 * (aStop - 8.71)))) : 0.0;
				this._numTaps = (int)((aStop - 8.0) / (14.357078426905355 * (double)(num2 - num)) + 1.0);
				if (this._numTaps > 75)
				{
					this._numTaps = 75;
				}
				else if (this._numTaps < 3)
				{
					this._numTaps = 3;
				}
				if (numTaps != 0)
				{
					this._numTaps = numTaps;
				}
				double num5 = (double)(0.5f * (float)(this._numTaps - 1));
				double num6 = (double)(float)this.Izero(num4);
				for (int i = 0; i < this._numTaps; i++)
				{
					double num7 = (double)i - num5;
					double num8 = ((double)i != num5) ? (Math.Sin(6.2831853071795862 * num7 * (double)num3) / (3.1415926535897931 * num7)) : ((double)(2f * num3));
					num7 = ((double)i - (double)(this._numTaps - 1) / 2.0) / ((double)(this._numTaps - 1) / 2.0);
					this._coef[i] = (float)(scale * num8 * this.Izero(num4 * Math.Sqrt(1.0 - num7 * num7)) / num6);
				}
				for (int i = 0; i < this._numTaps; i++)
				{
					this._coef[i + this._numTaps] = this._coef[i];
				}
				for (int i = 0; i < this._numTaps * 2; i++)
				{
					this._iCoef[i] = this._coef[i];
					this._qCoef[i] = this._coef[i];
				}
				for (int j = 0; j < this._numTaps; j++)
				{
					this._rZBuf[j] = 0f;
					this._cZBuf[j].Real = 0f;
					this._cZBuf[j].Imag = 0f;
				}
				this._state = 0;
			}
			return this._numTaps;
		}

		public int InitHPFilter(int numTaps, float scale, float aStop, float fPass, float fStop, float samprate)
		{
			lock (this)
			{
				this._sampleRate = samprate;
				float num = fPass / samprate;
				float num2 = fStop / samprate;
				float num3 = (num2 + num) / 2f;
				double num4 = (!((double)aStop < 20.96)) ? ((!((double)aStop >= 50.0)) ? (0.5842 * Math.Pow((double)aStop - 20.96, 0.4) + 0.07886 * ((double)aStop - 20.96)) : (0.1102 * ((double)aStop - 8.71))) : 0.0;
				this._numTaps = (int)(((double)aStop - 8.0) / (14.357078426905355 * (double)(num - num2)) + 1.0);
				if (this._numTaps > 74)
				{
					this._numTaps = 74;
				}
				else if (this._numTaps < 3)
				{
					this._numTaps = 3;
				}
				this._numTaps |= 1;
				if (this._numTaps != 0)
				{
					this._numTaps = numTaps;
				}
				double num5 = this.Izero(num4);
				double num6 = 0.5 * (double)(this._numTaps - 1);
				for (int i = 0; i < this._numTaps; i++)
				{
					double num7 = (double)((float)i - (float)(this._numTaps - 1) / 2f);
					double num8 = ((double)(float)i != num6) ? (Math.Sin(3.1415926535897931 * num7) / (3.1415926535897931 * num7) - Math.Sin(6.2831853071795862 * num7 * (double)num3) / (3.1415926535897931 * num7)) : (1.0 - 2.0 * (double)num3);
					num7 = ((double)i - (double)(this._numTaps - 1) / 2.0) / ((double)(this._numTaps - 1) / 2.0);
					this._coef[i] = (float)((double)scale * num8 * this.Izero(num4 * Math.Sqrt(1.0 - num7 * num7)) / num5);
				}
				for (int i = 0; i < this._numTaps; i++)
				{
					this._coef[i + this._numTaps] = this._coef[i];
				}
				for (int i = 0; i < this._numTaps * 2; i++)
				{
					this._iCoef[i] = this._coef[i];
					this._qCoef[i] = this._coef[i];
				}
				for (int j = 0; j < this._numTaps; j++)
				{
					this._rZBuf[j] = 0f;
					this._cZBuf[j].Real = 0f;
					this._cZBuf[j].Imag = 0f;
				}
				this._state = 0;
			}
			return this._numTaps;
		}

		public void GenerateHBFilter(float freqOffset)
		{
			for (int i = 0; i < this._numTaps; i++)
			{
				this._iCoef[i] = (float)(2.0 * (double)this._coef[i] * Math.Cos(6.2831853071795862 * (double)freqOffset / (double)this._sampleRate * ((double)i - (double)(this._numTaps - 1) / 2.0)));
				this._qCoef[i] = (float)(2.0 * (double)this._coef[i] * Math.Sin(6.2831853071795862 * (double)freqOffset / (double)this._sampleRate * ((double)i - (double)(this._numTaps - 1) / 2.0)));
			}
			for (int j = 0; j < this._numTaps; j++)
			{
				this._iCoef[j + this._numTaps] = this._iCoef[j];
				this._qCoef[j + this._numTaps] = this._qCoef[j];
			}
		}

		private double Izero(double x)
		{
			double num = x / 2.0;
			double num2 = 1.0;
			double num3 = 1.0;
			double num4 = 1.0;
			double num5 = 1E-09;
			do
			{
				double num6 = num / num4;
				num6 *= num6;
				num3 *= num6;
				num2 += num3;
				num4 += 1.0;
			}
			while (num3 >= num5 * num2);
			return num2;
		}
	}
}
