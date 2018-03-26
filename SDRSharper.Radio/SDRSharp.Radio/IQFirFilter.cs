using System;

namespace SDRSharp.Radio
{
	public sealed class IQFirFilter
	{
		private readonly bool _isMultiThteaded;

		private readonly FirFilter _rFilter;

		private readonly FirFilter _iFilter;

		private readonly SharpEvent _event;

		public IQFirFilter(float[] coefficients)
			: this(coefficients, false, 1)
		{
		}

		public IQFirFilter(float[] coefficients, bool isMultiThteaded, int decimationFactor)
		{
			this._rFilter = new FirFilter(coefficients, decimationFactor);
			this._iFilter = new FirFilter(coefficients, decimationFactor);
			this._isMultiThteaded = isMultiThteaded;
			if (this._isMultiThteaded)
			{
				this._event = new SharpEvent(false);
			}
		}

		~IQFirFilter()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this._rFilter.Dispose();
			this._iFilter.Dispose();
			GC.SuppressFinalize(this);
		}

		public unsafe void Process(Complex* iq, int length)
		{
			if (this._isMultiThteaded)
			{
				DSPThreadPool.QueueUserWorkItem(delegate
				{
					this._rFilter.ProcessInterleaved((float*)iq, length);
					this._event.Set();
				});
			}
			else
			{
				this._rFilter.ProcessInterleaved((float*)iq, length);
			}
			this._iFilter.ProcessInterleaved((float*)((byte*)iq + 4), length);
			if (this._isMultiThteaded)
			{
				this._event.WaitOne();
			}
		}

		public void SetCoefficients(float[] coefficients)
		{
			this._rFilter.SetCoefficients(coefficients);
			this._iFilter.SetCoefficients(coefficients);
		}
	}
}
