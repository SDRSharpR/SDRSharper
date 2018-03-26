using SDRSharp.Radio;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SDRSharp.SDRIQ
{
	public class SdrIqDevice
	{
		public const uint DefaultFrequency = 15000000u;

		private const int DefaultSamplerate = 158730;

		private unsafe static readonly float* _lut16;

		private static readonly UnsafeBuffer _lut16Buffer;

		private IntPtr _dev;

		private readonly uint _index;

		private GCHandle _gcHandle;

		private UnsafeBuffer _iqBuffer;

		private unsafe Complex* _iqPtr;

		private uint _centerFrequency = 15000000u;

		private uint _sampleRate = 158730u;

		private sbyte _rfGain;

		private sbyte _ifGain;

		private Thread _worker;

		private readonly SamplesAvailableEventArgs _eventArgs = new SamplesAvailableEventArgs();

		private static readonly SdrIqReadAsyncDelegate _sdriqCallback;

		private static readonly int _readBlockCount;

		private static readonly uint _outFifoBlockCount;

		public bool IsStreaming => this._worker != null;

		public uint Frequency
		{
			get
			{
				return this._centerFrequency;
			}
			set
			{
				this._centerFrequency = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.sdriq_set_center_frequency(this._dev, this._centerFrequency);
				}
			}
		}

		public uint Samplerate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.sdriq_set_out_samplerate(this._dev, this._sampleRate);
				}
			}
		}

		public sbyte RfGain
		{
			get
			{
				return this._rfGain;
			}
			set
			{
				this._rfGain = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.sdriq_set_rf_gain(this._dev, value);
				}
			}
		}

		public sbyte IfGain
		{
			get
			{
				return this._ifGain;
			}
			set
			{
				this._ifGain = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.sdriq_set_if_gain(this._dev, value);
				}
			}
		}

		public uint Index => this._index;

		public event SamplesAvailableDelegate SamplesAvailable;

		unsafe static SdrIqDevice()
		{
			SdrIqDevice._lut16Buffer = UnsafeBuffer.Create(65536, 4);
			SdrIqDevice._sdriqCallback = SdrIqDevice.SdrIqSamplesAvailable;
			SdrIqDevice._readBlockCount = Utils.GetIntSetting("SDRIQReadBlockCount", 1);
			SdrIqDevice._outFifoBlockCount = (uint)Utils.GetIntSetting("SDRIQOutFifoBlockCount", 0);
			SdrIqDevice._lut16 = (float*)(void*)SdrIqDevice._lut16Buffer;
			for (int i = 0; i < 65536; i++)
			{
				SdrIqDevice._lut16[i] = (float)(i - 32768) * 3.051851E-05f;
			}
		}

		public SdrIqDevice(uint index)
		{
			this._index = index;
			if (NativeMethods.sdriq_open(this._index, SdrIqDevice._outFifoBlockCount, out this._dev) != 0)
			{
				throw new ApplicationException("Cannot open SDR-IQ");
			}
			this._gcHandle = GCHandle.Alloc(this);
		}

		~SdrIqDevice()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.Stop();
			NativeMethods.sdriq_close(this._dev);
			if (this._gcHandle.IsAllocated)
			{
				this._gcHandle.Free();
			}
			this._dev = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		public void Start()
		{
			if (this._worker != null)
			{
				throw new ApplicationException("Already running");
			}
			if (NativeMethods.sdriq_set_out_samplerate(this._dev, this._sampleRate) != 0)
			{
				throw new ApplicationException("Cannot access SDR-IQ");
			}
			if (NativeMethods.sdriq_set_center_frequency(this._dev, this._centerFrequency) != 0)
			{
				throw new ApplicationException("Cannot access SDR-IQ");
			}
			this._worker = new Thread(this.StreamProc);
			this._worker.Priority = ThreadPriority.Highest;
			this._worker.Start();
		}

		public void Stop()
		{
			if (this._worker != null)
			{
				NativeMethods.sdriq_async_cancel(this._dev);
				if (this._worker.ThreadState == ThreadState.Running)
				{
					this._worker.Join();
				}
				this._worker = null;
			}
		}

		private unsafe void StreamProc()
		{
			NativeMethods.sdriq_async_read(this._dev, (IntPtr)this._gcHandle, SdrIqDevice._sdriqCallback, SdrIqDevice._readBlockCount);
		}

		private unsafe static void SdrIqSamplesAvailable(short* buf, uint len, IntPtr ctx)
		{
			GCHandle gCHandle = GCHandle.FromIntPtr(ctx);
			if (gCHandle.IsAllocated)
			{
				SdrIqDevice sdrIqDevice = (SdrIqDevice)gCHandle.Target;
				int num = (int)len / 2;
				if (sdrIqDevice._iqBuffer == null || sdrIqDevice._iqBuffer.Length != num)
				{
					sdrIqDevice._iqBuffer = UnsafeBuffer.Create(num, sizeof(Complex));
					sdrIqDevice._iqPtr = (Complex*)(void*)sdrIqDevice._iqBuffer;
				}
				Complex* ptr = sdrIqDevice._iqPtr;
				for (int i = 0; i < num; i++)
				{
					Complex* intPtr = ptr;
					float* lut = SdrIqDevice._lut16;
					short* intPtr2 = buf;
					buf = intPtr2 + 1;
					intPtr->Imag = lut[*intPtr2 + 32768];
					Complex* intPtr3 = ptr;
					float* lut2 = SdrIqDevice._lut16;
					short* intPtr4 = buf;
					buf = intPtr4 + 1;
					intPtr3->Real = lut2[*intPtr4 + 32768];
					ptr++;
				}
				sdrIqDevice.ComplexSamplesAvailable(sdrIqDevice._iqPtr, sdrIqDevice._iqBuffer.Length);
			}
		}

		private unsafe void ComplexSamplesAvailable(Complex* buffer, int length)
		{
			if (this.SamplesAvailable != null)
			{
				this._eventArgs.Buffer = buffer;
				this._eventArgs.Length = length;
				this.SamplesAvailable(this, this._eventArgs);
			}
		}
	}
}
