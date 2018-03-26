using System;
using System.Windows.Forms;

namespace SDRSharp.Radio
{
	public class ExtIOController : IFrontendController
	{
		private readonly string _filename;

		public bool IsOpen => ExtIO.IsHardwareOpen;

		public string Filename => this._filename;

		public bool IsSoundCardBased => ExtIO.HWType == ExtIO.HWTypes.Soundcard;

		public string SoundCardHint => string.Empty;

		public double Samplerate => (double)ExtIO.GetHWSR();

		public long Frequency
		{
			get
			{
				return Math.Max(0, ExtIO.GetHWLO());
			}
			set
			{
				if (value == -1)
				{
					ExtIO.CloseLibrary();
				}
				else
				{
					ExtIO.SetHWLO((int)value);
				}
			}
		}

		public ExtIOController(string filename)
		{
			this._filename = filename;
		}

		~ExtIOController()
		{
			ExtIO.CloseLibrary();
		}

		public void Open()
		{
			ExtIO.UseLibrary(this._filename);
			ExtIO.OpenHW(true);
		}

		public unsafe void Start(SamplesAvailableDelegate callback)
		{
			ExtIO.SamplesAvailable = callback;
			ExtIO.StartHW(Math.Max(0, ExtIO.GetHWLO()));
		}

		public unsafe void Stop()
		{
			ExtIO.StopHW();
			ExtIO.SamplesAvailable = null;
		}

		public void Close()
		{
			ExtIO.HideGUI();
			ExtIO.CloseHW();
		}

		public void ShowSettingGUI(IWin32Window parent)
		{
			ExtIO.ShowGUI();
		}

		public void HideSettingGUI()
		{
			ExtIO.HideGUI();
		}
	}
}
