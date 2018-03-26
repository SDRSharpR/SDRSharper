using System.Windows.Forms;

namespace SDRSharp.Radio
{
	public interface IFrontendController
	{
		bool IsSoundCardBased
		{
			get;
		}

		string SoundCardHint
		{
			get;
		}

		double Samplerate
		{
			get;
		}

		long Frequency
		{
			get;
			set;
		}

		void Open();

		void Start(SamplesAvailableDelegate callback);

		void Stop();

		void Close();

		void ShowSettingGUI(IWin32Window parent);

		void HideSettingGUI();
	}
}
