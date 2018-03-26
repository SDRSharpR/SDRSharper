using SDRSharp.Radio;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace SDRSharp.RTLTCP
{
	public class RtlTcpIO : IFrontendController, IDisposable
	{
		private const double DEFAULT_SAMPLE_RATE = 2048000.0;

		private const long DEFAULT_FREQUENCY = 100000000L;

		private const string DEFAULT_HOSTNAME = "192.168.0.11";

		private const int DEFAULT_PORT = 1234;

		private const byte CMD_SET_FREQ = 1;

		private const byte CMD_SET_SAMPLE_RATE = 2;

		private const byte CMD_SET_GAIN_MODE = 3;

		private const byte CMD_SET_GAIN = 4;

		private const byte CMD_SET_FREQ_COR = 5;

		public const uint GAIN_MODE_AUTO = 0u;

		public const uint GAIN_MODE_MANUAL = 1u;

		private const int BUFFER_SIZE = 16384;

		private const int MAX_TUNE_RATE = 20;

		private RTLTcpSettings _gui;

		private volatile SamplesAvailableDelegate _callback;

		private Thread _sampleThread;

		private long _freq;

		private double _sr;

		private Socket _s;

		private string _host;

		private int _port;

		private uint _gainMode;

		private int _gainVal;

		private int _fCor;

		private UnsafeBuffer _b;

		private bool _tunePlease;

		private System.Timers.Timer _retuneTimer = new System.Timers.Timer(50.0);

		public string hostName
		{
			get
			{
				return this._host;
			}
			set
			{
				this._host = value;
			}
		}

		public int port
		{
			get
			{
				return this._port;
			}
			set
			{
				this._port = value;
			}
		}

		public bool IsSoundCardBased => false;

		public string SoundCardHint => string.Empty;

		public double Samplerate
		{
			get
			{
				return this._sr;
			}
			set
			{
				this._sr = value;
				this.sendCommand(2, (uint)this._sr);
			}
		}

		public long Frequency
		{
			get
			{
				return this._freq;
			}
			set
			{
				this._freq = value;
				lock (this._retuneTimer)
				{
					this._tunePlease = true;
				}
			}
		}

		public int Gain
		{
			get
			{
				return this._gainVal;
			}
			set
			{
				this._gainVal = value;
				this.sendCommand(4, this._gainVal);
			}
		}

		public uint GainMode
		{
			get
			{
				return this._gainMode;
			}
			set
			{
				this._gainMode = value;
				this.sendCommand(3, this._gainMode);
			}
		}

		public int FreqCorrection
		{
			get
			{
				return this._fCor;
			}
			set
			{
				this._fCor = value;
				this.sendCommand(5, this._fCor);
			}
		}

		private bool sendCommand(byte cmd, byte[] val)
		{
			if (this._s == null)
			{
				return false;
			}
			if (val.Length < 4)
			{
				return false;
			}
			byte[] buffer = new byte[5]
			{
				cmd,
				val[3],
				val[2],
				val[1],
				val[0]
			};
			try
			{
				this._s.Send(buffer);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		private bool sendCommand(byte cmd, uint val)
		{
			byte[] bytes = BitConverter.GetBytes(val);
			return this.sendCommand(cmd, bytes);
		}

		private bool sendCommand(byte cmd, int val)
		{
			byte[] bytes = BitConverter.GetBytes(val);
			return this.sendCommand(cmd, bytes);
		}

		public RtlTcpIO()
		{
			this._freq = 100000000L;
			this._sr = 2048000.0;
			this._gainVal = 0;
			this._gainMode = 0u;
			this._host = "192.168.0.11";
			this._port = 1234;
			this._retuneTimer.Elapsed += this.retuneNow;
			this._retuneTimer.Start();
		}

		~RtlTcpIO()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this._gui != null)
			{
				this._gui.Dispose();
				this._gui = null;
			}
			GC.SuppressFinalize(this);
		}

		public void Open()
		{
		}

		public void Close()
		{
			if (this._s != null)
			{
				this._s.Close();
				this._s = null;
			}
		}

		public unsafe void Start(SamplesAvailableDelegate callback)
		{
			lock (this)
			{
				this._callback = callback;
			}
			this._s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this._s.Connect(this._host, this._port);
			this._sampleThread = new Thread((ThreadStart)delegate
			{
				this.receiveSamples();
			});
			this._sampleThread.Start();
			this.sendCommand(3, this._gainMode);
			this.sendCommand(2, (uint)this._sr);
			this.sendCommand(1, (uint)this._freq);
			this.sendCommand(5, this._fCor);
		}

		public unsafe void Stop()
		{
			lock (this)
			{
				this._callback = null;
			}
			this.Close();
			if (this._sampleThread != null)
			{
				this._sampleThread.Join();
			}
		}

		public void ShowSettingGUI(IWin32Window parent)
		{
			if (this._gui == null || this._gui.IsDisposed)
			{
				this._gui = new RTLTcpSettings(this);
			}
			this._gui.Show();
		}

		public void HideSettingGUI()
		{
			if (this._gui != null && !this._gui.IsDisposed)
			{
				this._gui.Hide();
			}
		}

		private unsafe void receiveSamples()
		{
			byte[] buffer = new byte[17408];
			int num = 0;
			ulong num2 = 0uL;
			DateTime now = DateTime.Now;
			while (this._callback != null && this._s != null && this._s.Connected)
			{
				try
				{
					int num3 = this._s.Receive(buffer, num, 16384, SocketFlags.None);
					num2 = (ulong)((long)num2 + (long)num3);
					int num4 = num + num3;
					num = num4 % 2;
					this.beamUpThemSamples(buffer, num4 - num);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					this.Close();
					break;
				}
			}
			DateTime now2 = DateTime.Now;
			TimeSpan timeSpan = now2 - now;
			double num5 = (double)num2 / timeSpan.TotalSeconds;
			double num6 = num5 / 2.0;
			Console.WriteLine($"Received {num2} bytes over {timeSpan.TotalSeconds} seconds, which is {num5} bps or {num6} sps");
		}

		private unsafe void beamUpThemSamples(byte[] buffer, int len)
		{
			int num = len / 2;
			if (this._b == null || this._b.Length < num)
			{
				this._b = UnsafeBuffer.Create(num, sizeof(Complex));
			}
			Complex* ptr = (Complex*)(void*)this._b;
			for (int i = 0; i < num; i++)
			{
				ptr[i].Real = (float)(buffer[i * 2 + 1] - 128) * 0.0078125f;
				ptr[i].Imag = (float)(buffer[i * 2] - 128) * 0.0078125f;
			}
			lock (this)
			{
				if (this._callback != null)
				{
					this._callback(this, ptr, num);
				}
			}
		}

		private void retuneNow(object source, ElapsedEventArgs e)
		{
			lock (this._retuneTimer)
			{
				if (this._tunePlease)
				{
					this.sendCommand(1, (uint)this._freq);
					this._tunePlease = false;
				}
			}
		}
	}
}
