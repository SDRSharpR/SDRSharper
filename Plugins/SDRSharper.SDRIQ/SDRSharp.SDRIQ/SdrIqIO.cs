using SDRSharp.Radio;
using System;
using System.Windows.Forms;

namespace SDRSharp.SDRIQ
{
	public class SdrIqIO : IFrontendController, IDisposable
	{
		private SdrIqDevice _device;

		private readonly SDRIQControllerDialog _gui;

		private SDRSharp.Radio.SamplesAvailableDelegate _callback;

		private uint _frequency = 15000000u;

		public bool IsSoundCardBased => false;

		public string SoundCardHint => string.Empty;

		public double Samplerate => (this._device == null) ? 0.0 : ((double)this._device.Samplerate);

		public long Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				this._frequency = (uint)value;
				if (this._device != null)
				{
					this._device.Frequency = (uint)value;
				}
			}
		}

		public SdrIqDevice Device => this._device;

		public SdrIqIO()
		{
			this._gui = new SDRIQControllerDialog(this);
		}

		~SdrIqIO()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this._gui != null)
			{
				this._gui.Dispose();
			}
			try
			{
				NativeMethods.sdriq_destroy();
			}
			catch (DllNotFoundException)
			{
			}
			GC.SuppressFinalize(this);
		}

		public void Open()
		{
			NativeMethods.sdriq_destroy();
			NativeMethods.sdriq_initialise();
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
			NativeMethods.sdriq_destroy();
			if (activeDevices.Length != 0)
			{
				throw new ApplicationException(activeDevices.Length + " compatible devices have been found but are all busy");
			}
			throw new ApplicationException("No compatible devices found");
		}

		public void SelectDevice(uint index)
		{
			this.Close();
			this._device = new SdrIqDevice(index);
			this._device.SamplesAvailable += this.sdriqDevice_SamplesAvailable;
			this._device.Frequency = this._frequency;
			this._gui.ConfigureGUI();
			this._gui.ConfigureDevice();
		}

		public unsafe void Start(SDRSharp.Radio.SamplesAvailableDelegate callback)
		{
			if (this._device == null)
			{
				throw new ApplicationException("No device selected");
			}
			this._callback = callback;
			try
			{
				this._device.Start();
			}
			catch
			{
				this.Open();
				this._device.Start();
			}
		}

		public void Stop()
		{
			this._device.Stop();
		}

		public void Close()
		{
			if (this._device != null)
			{
				this._device.Stop();
				this._device.SamplesAvailable -= this.sdriqDevice_SamplesAvailable;
				this._device.Dispose();
				this._device = null;
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

		private unsafe void sdriqDevice_SamplesAvailable(object sender, SamplesAvailableEventArgs e)
		{
			this._callback(this, e.Buffer, e.Length);
		}
	}
}
