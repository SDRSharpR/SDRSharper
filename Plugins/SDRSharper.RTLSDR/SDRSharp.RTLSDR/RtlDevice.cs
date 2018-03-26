using SDRSharp.Radio;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SDRSharp.RTLSDR
{
	public sealed class RtlDevice : IDisposable
	{
		private const uint DefaultFrequency = 105500000u;

		private const int DefaultSamplerate = 2048000;

		private readonly uint _index;

		private IntPtr _dev;

		private readonly string _name;

		private readonly int[] _supportedGains;

		private bool _useTunerAGC = true;

		private bool _useRtlAGC;

		private int _tunerGain;

		private uint _centerFrequency = 105500000u;

		private uint _sampleRate = 2048000u;

		private int _frequencyCorrection;

		private SamplingMode _samplingMode;

		private bool _useOffsetTuning;

		private readonly bool _supportsOffsetTuning;

		private GCHandle _gcHandle;

		private UnsafeBuffer _iqBuffer;

		private unsafe Complex* _iqPtr;

		private Thread _worker;

		private readonly SamplesAvailableEventArgs _eventArgs = new SamplesAvailableEventArgs();

		private unsafe static readonly RtlSdrReadAsyncDelegate _rtlCallback = RtlDevice.RtlSdrSamplesAvailable; //EDITHERE

		private static readonly uint _readLength = (uint)Utils.GetIntSetting("RTLBufferLength", 16384);

		public uint Index => this._index;

		public string Name => this._name;

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
					NativeMethods.rtlsdr_set_sample_rate(this._dev, this._sampleRate);
				}
			}
		}

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
					NativeMethods.rtlsdr_set_center_freq(this._dev, this._centerFrequency);
				}
			}
		}

		public bool UseRtlAGC
		{
			get
			{
				return this._useRtlAGC;
			}
			set
			{
				this._useRtlAGC = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.rtlsdr_set_agc_mode(this._dev, this._useRtlAGC ? 1 : 0);
				}
			}
		}

		public bool UseTunerAGC
		{
			get
			{
				return this._useTunerAGC;
			}
			set
			{
				this._useTunerAGC = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.rtlsdr_set_tuner_gain_mode(this._dev, (!this._useTunerAGC) ? 1 : 0);
				}
			}
		}

		public SamplingMode SamplingMode
		{
			get
			{
				return this._samplingMode;
			}
			set
			{
				this._samplingMode = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.rtlsdr_set_direct_sampling(this._dev, (int)this._samplingMode);
				}
			}
		}

		public bool SupportsOffsetTuning => this._supportsOffsetTuning;

		public bool UseOffsetTuning
		{
			get
			{
				return this._useOffsetTuning;
			}
			set
			{
				this._useOffsetTuning = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.rtlsdr_set_offset_tuning(this._dev, this._useOffsetTuning ? 1 : 0);
				}
			}
		}

		public int[] SupportedGains => this._supportedGains;

		public int Gain
		{
			get
			{
				return this._tunerGain;
			}
			set
			{
				this._tunerGain = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.rtlsdr_set_tuner_gain(this._dev, this._tunerGain);
				}
			}
		}

		public int FrequencyCorrection
		{
			get
			{
				return this._frequencyCorrection;
			}
			set
			{
				this._frequencyCorrection = value;
				if (this._dev != IntPtr.Zero)
				{
					NativeMethods.rtlsdr_set_freq_correction(this._dev, this._frequencyCorrection);
				}
			}
		}

		public RtlSdrTunerType TunerType
		{
			get
			{
				if (!(this._dev == IntPtr.Zero))
				{
					return NativeMethods.rtlsdr_get_tuner_type(this._dev);
				}
				return RtlSdrTunerType.Unknown;
			}
		}

		public bool IsStreaming => this._worker != null;

		public event SamplesAvailableDelegate SamplesAvailable;

		public RtlDevice(uint index)
		{
			this._index = index;
			if (NativeMethods.rtlsdr_open(out this._dev, this._index) != 0)
			{
				throw new ApplicationException("Cannot open RTL device. Is the device locked somewhere?");
			}
			int num = (!(this._dev == IntPtr.Zero)) ? NativeMethods.rtlsdr_get_tuner_gains(this._dev, null) : 0;
			if (num < 0)
			{
				num = 0;
			}
			this._supportsOffsetTuning = (NativeMethods.rtlsdr_set_offset_tuning(this._dev, 0) != -2);
			this._supportedGains = new int[num];
			if (num >= 0)
			{
				NativeMethods.rtlsdr_get_tuner_gains(this._dev, this._supportedGains);
			}
			this._name = NativeMethods.rtlsdr_get_device_name(this._index);
			this._gcHandle = GCHandle.Alloc(this);
		}

		~RtlDevice()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.Stop();
			NativeMethods.rtlsdr_close(this._dev);
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
			if (NativeMethods.rtlsdr_set_sample_rate(this._dev, this._sampleRate) != 0)
			{
				throw new ApplicationException("Cannot access RTL device");
			}
			if (NativeMethods.rtlsdr_set_center_freq(this._dev, this._centerFrequency) != 0)
			{
				throw new ApplicationException("Cannot access RTL device");
			}
			if (NativeMethods.rtlsdr_set_tuner_gain_mode(this._dev, (!this._useTunerAGC) ? 1 : 0) != 0)
			{
				throw new ApplicationException("Cannot access RTL device");
			}
			if (!this._useTunerAGC && NativeMethods.rtlsdr_set_tuner_gain(this._dev, this._tunerGain) != 0)
			{
				throw new ApplicationException("Cannot access RTL device");
			}
			if (NativeMethods.rtlsdr_reset_buffer(this._dev) != 0)
			{
				throw new ApplicationException("Cannot access RTL device");
			}
			this._worker = new Thread(this.StreamProc);
			this._worker.Priority = ThreadPriority.Highest;
			this._worker.Start();
		}

		public void Stop()
		{
			if (this._worker != null)
			{
				NativeMethods.rtlsdr_cancel_async(this._dev);
				if (this._worker.ThreadState == ThreadState.Running)
				{
					this._worker.Join();
				}
				this._worker = null;
			}
		}

		private unsafe void StreamProc()
		{
			NativeMethods.rtlsdr_read_async(this._dev, RtlDevice._rtlCallback, (IntPtr)this._gcHandle, 0u, RtlDevice._readLength);
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

		private unsafe static void RtlSdrSamplesAvailable(byte* buf, uint len, IntPtr ctx)
		{
			GCHandle gCHandle = GCHandle.FromIntPtr(ctx);
			if (gCHandle.IsAllocated)
			{
				RtlDevice rtlDevice = (RtlDevice)gCHandle.Target;
				int num = (int)len / 2;
				if (rtlDevice._iqBuffer == null || rtlDevice._iqBuffer.Length != num)
				{
					rtlDevice._iqBuffer = UnsafeBuffer.Create(num, sizeof(Complex));
					rtlDevice._iqPtr = (Complex*)(void*)rtlDevice._iqBuffer;
				}
				Complex* ptr = rtlDevice._iqPtr;
				for (int i = 0; i < num; i++)
				{
					Complex* intPtr = ptr;
					byte* intPtr2 = buf;
					buf = intPtr2 + 1;
					intPtr->Imag = (float)(*intPtr2 - 128) * 0.0078125f;
					Complex* intPtr3 = ptr;
					byte* intPtr4 = buf;
					buf = intPtr4 + 1;
					intPtr3->Real = (float)(*intPtr4 - 128) * 0.0078125f;
					ptr++;
				}
				rtlDevice.ComplexSamplesAvailable(rtlDevice._iqPtr, rtlDevice._iqBuffer.Length);
			}
		}
	}
}
