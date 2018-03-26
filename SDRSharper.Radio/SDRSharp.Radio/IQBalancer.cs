using System;
using System.Runtime.InteropServices;

namespace SDRSharp.Radio
{
	[StructLayout(LayoutKind.Sequential, Pack = 16)]
	public class IQBalancer
	{
		private const int FFTBins = 1024;

		private const float DcTimeConst = 0.0001f;

		private const float BaseIncrement = 0.001f;

		private const float PowerThreshold = 20f;

		private float _dcTimeConst;

		private int _maxAutomaticPasses = Utils.GetIntSetting("automaticIQBalancePasses", 10);

		private bool _autoBalanceIQ;

		private bool _removeDC;

		private float _gain = 1f;

		private float _phase;

		private float _averagePower;

		private float _powerRange;

		private unsafe Complex* _iqPtr;

		private unsafe readonly DcRemover* _dcRemoverI;

		private readonly UnsafeBuffer _dcRemoverIBuffer;

		private unsafe readonly DcRemover* _dcRemoverQ;

		private readonly UnsafeBuffer _dcRemoverQBuffer;

		private readonly bool _isMultithreaded;

		private unsafe readonly float* _windowPtr;

		private readonly UnsafeBuffer _windowBuffer;

		private readonly Random _rng = new Random();

		private readonly SharpEvent _event = new SharpEvent(false);

		public float Phase => (float)Math.Asin((double)this._phase);

		public float Gain => this._gain;

		public int MaxAutomaticPasses
		{
			get
			{
				return this._maxAutomaticPasses;
			}
			set
			{
				this._maxAutomaticPasses = value;
			}
		}

		public bool AutoBalanceIQ
		{
			get
			{
				return this._autoBalanceIQ;
			}
			set
			{
				this._autoBalanceIQ = value;
			}
		}

		public unsafe float RemoveDC
		{
			get
			{
				return this._dcTimeConst;
			}
			set
			{
				this._dcTimeConst = value;
				this._dcRemoverI->Init(this._dcTimeConst);
				this._dcRemoverQ->Init(this._dcTimeConst);
				this._removeDC = (this._dcTimeConst != 0f);
			}
		}

		public unsafe IQBalancer()
		{
			this._dcTimeConst = 0.0001f;
			this._dcRemoverIBuffer = UnsafeBuffer.Create(sizeof(DcRemover));
			this._dcRemoverI = (DcRemover*)(void*)this._dcRemoverIBuffer;
			this._dcRemoverI->Init(this._dcTimeConst);
			this._dcRemoverQBuffer = UnsafeBuffer.Create(sizeof(DcRemover));
			this._dcRemoverQ = (DcRemover*)(void*)this._dcRemoverQBuffer;
			this._dcRemoverQ->Init(this._dcTimeConst);
			float[] buffer = FilterBuilder.MakeWindow(WindowType.Hamming, 1024);
			this._windowBuffer = UnsafeBuffer.Create(buffer);
			this._windowPtr = (float*)(void*)this._windowBuffer;
			this._isMultithreaded = (Utils.ProcessorCount > 1);
		}

		public unsafe void Reset(float gain, float phase)
		{
			this._dcRemoverI->Reset();
			this._dcRemoverQ->Reset();
			this._gain = gain;
			this._phase = phase;
		}

		public unsafe void Process(Complex* iq, int length)
		{
			if (this._removeDC || this._autoBalanceIQ)
			{
				this.remDC(iq, length);
			}
			if (this._autoBalanceIQ && length >= 1024)
			{
				this._iqPtr = iq;
				this.EstimateImbalance();
			}
			if (this._gain == 1f && this._phase == 0f)
			{
				return;
			}
			IQBalancer.Adjust(iq, length, this._phase, this._gain);
		}

		private unsafe void remDC(Complex* iq, int length)
		{
			float* buffer = (float*)((byte*)iq + 4);
			if (this._isMultithreaded)
			{
				DSPThreadPool.QueueUserWorkItem(delegate
				{
					this._dcRemoverI->ProcessInterleaved((float*)iq, length);
					this._event.Set();
				});
			}
			else
			{
				this._dcRemoverI->ProcessInterleaved((float*)iq, length);
			}
			this._dcRemoverQ->ProcessInterleaved(buffer, length);
			if (this._isMultithreaded)
			{
				this._event.WaitOne();
			}
		}

		private void EstimateImbalance()
		{
			this.EstimatePower();
			if (!(this._powerRange < 20f))
			{
				float num = this.Utility(this._phase, this._gain);
				for (int i = 0; i < this._maxAutomaticPasses; i++)
				{
					float num2 = 0.001f * this.GetRandomDirection();
					float num3 = 0.001f * this.GetRandomDirection();
					float phase = this._phase + num2;
					float gain = this._gain + num3;
					float num4 = this.Utility(phase, gain);
					if (num4 > num)
					{
						num = num4;
						this._gain = gain;
						this._phase = phase;
					}
				}
			}
		}

		private float GetRandomDirection()
		{
			if (!(this._rng.NextDouble() > 0.5))
			{
				return -1f;
			}
			return 1f;
		}

		private unsafe float Utility(float phase, float gain)
		{
			byte* ptr = stackalloc byte[(int)(uint)(1024 * sizeof(Complex) + 16)];
			Complex* ptr2 = (Complex*)((long)ptr + 15 & -16);
			byte* ptr3 = stackalloc byte[4112];
			float* ptr4 = (float*)((long)ptr3 + 15 & -16);
			Utils.Memcpy(ptr2, this._iqPtr, 1024 * sizeof(Complex));
			IQBalancer.Adjust(ptr2, 1024, phase, gain);
			Fourier.ApplyFFTWindow(ptr2, this._windowPtr, 1024);
			Fourier.ForwardTransform(ptr2, 1024, true);
			Fourier.SpectrumPower(ptr2, ptr4, 1024, 0f, false);
			float num = 0f;
			for (int i = 0; i < 512; i++)
			{
				int num2 = 512 - i;
				if ((float)num2 > 25.6f && (float)num2 < 486.4f)
				{
					int num3 = 1022 - i;
					if (ptr4[i] - this._averagePower > 20f || ptr4[num3] - this._averagePower > 20f)
					{
						float num4 = ptr4[i] - ptr4[num3];
						num += num4 * num4;
					}
				}
			}
			return num;
		}

		private unsafe void EstimatePower()
		{
			byte* ptr = stackalloc byte[(int)(uint)(1024 * sizeof(Complex) + 16)];
			Complex* ptr2 = (Complex*)((long)ptr + 15 & -16);
			byte* ptr3 = stackalloc byte[4112];
			float* ptr4 = (float*)((long)ptr3 + 15 & -16);
			Utils.Memcpy(ptr2, this._iqPtr, 1024 * sizeof(Complex));
			Fourier.ApplyFFTWindow(ptr2, this._windowPtr, 1024);
			Fourier.ForwardTransform(ptr2, 1024, true);
			Fourier.SpectrumPower(ptr2, ptr4, 1024, 0f, false);
			float num = float.NegativeInfinity;
			float num2 = 0f;
			int num3 = 0;
			for (int i = 0; i < 512; i++)
			{
				int num4 = 512 - i;
				if ((float)num4 > 25.6f && (float)num4 < 486.4f)
				{
					int num5 = 1022 - i;
					if (ptr4[i] > num)
					{
						num = ptr4[i];
					}
					if (ptr4[num5] > num)
					{
						num = ptr4[num5];
					}
					num2 += ptr4[i] + ptr4[num5];
					num3 += 2;
				}
			}
			num2 /= (float)num3;
			this._powerRange = num - num2;
			this._averagePower = num2;
		}

		private unsafe static void Adjust(Complex* buffer, int length, float phase, float gain)
		{
			for (int i = 0; i < length; i++)
			{
				buffer[i].Real = buffer[i].Real * gain + phase * buffer[i].Imag;
			}
		}
	}
}
