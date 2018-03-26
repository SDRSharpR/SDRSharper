using SDRSharp.Radio;
using System;
using System.Windows.Forms;

namespace SDRSharp.RTLSDR
{
	public class RtlSdrIO : IFrontendController, IDisposable
	{
		private readonly RtlSdrControllerDialog _gui;

		private RtlDevice _rtlDevice;

		private uint _frequency = 105500000u;

		private SDRSharp.Radio.SamplesAvailableDelegate _callback;

		public RtlDevice Device => this._rtlDevice;

		public bool IsSoundCardBased => false;

		public string SoundCardHint => string.Empty;

		public double Samplerate
		{
			get
			{
				if (this._rtlDevice != null)
				{
					return (double)this._rtlDevice.Samplerate;
				}
				return 0.0;
			}
		}

		public long Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				this._frequency = (uint)value;
				if (this._rtlDevice != null)
				{
					this._rtlDevice.Frequency = this._frequency;
				}
			}
		}

		public RtlSdrIO()
		{
			this._gui = new RtlSdrControllerDialog(this);
		}

		~RtlSdrIO()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this._gui != null)
			{
				this._gui.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		public void SelectDevice(uint index)
		{
			this.Close();
			this._rtlDevice = new RtlDevice(index);
			this._rtlDevice.SamplesAvailable += this.rtlDevice_SamplesAvailable;
			this._rtlDevice.Frequency = this._frequency;
			this._gui.ConfigureGUI();
			this._gui.ConfigureDevice();
		}

		public void Open()
		{
			DeviceDisplay[] activeDevices = DeviceDisplay.GetActiveDevices();
			DeviceDisplay[] array = activeDevices;
			foreach (DeviceDisplay deviceDisplay in array)
			{
				try
				{
					this.SelectDevice(deviceDisplay.Index);
					return;
				}
				catch (ApplicationException)
				{
				}
			}
			if (activeDevices.Length > 0)
			{
				throw new ApplicationException(activeDevices.Length + " compatible devices have been found but are all busy");
			}
			throw new ApplicationException("No compatible devices found");
		}

		public void Close()
		{
			if (this._rtlDevice != null)
			{
				this._rtlDevice.Stop();
				this._rtlDevice.SamplesAvailable -= this.rtlDevice_SamplesAvailable;
				this._rtlDevice.Dispose();
				this._rtlDevice = null;
			}
		}

		public unsafe void Start(SDRSharp.Radio.SamplesAvailableDelegate callback)
		{
			if (this._rtlDevice == null)
			{
				throw new ApplicationException("No device selected");
			}
			this._callback = callback;
			try
			{
				this._rtlDevice.Start();
			}
			catch
			{
				this.Open();
				this._rtlDevice.Start();
			}
		}

		public void Stop()
		{
			if (this._rtlDevice != null)
			{
				this._rtlDevice.Stop();
			}
		}

		public void ShowSettingGUI(IWin32Window parent)
		{
			this._gui.Show();
		}

		public void HideSettingGUI()
		{
			this._gui.Hide();
		}

		private unsafe void rtlDevice_SamplesAvailable(object sender, SamplesAvailableEventArgs e)
		{
			this._callback(this, e.Buffer, e.Length);
		}
	}
}
