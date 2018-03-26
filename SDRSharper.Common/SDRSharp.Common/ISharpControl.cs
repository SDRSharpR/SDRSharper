// Refs: PanView and Radio
using SDRSharp.PanView;
using SDRSharp.Radio;
using System.ComponentModel;

namespace SDRSharp.Common
{
	public interface ISharpControl
	{
		DetectorType DetectorType
		{
			get;
			set;
		}

		WindowType FilterType
		{
			get;
			set;
		}

		int AudioGain
		{
			get;
			set;
		}

		long CenterFrequency
		{
			get;
			set;
		}

		int CWShift
		{
			get;
			set;
		}

		bool FilterAudio
		{
			get;
			set;
		}

		int FilterBandwidth
		{
			get;
			set;
		}

		int FilterOrder
		{
			get;
			set;
		}

		bool FmStereo
		{
			get;
			set;
		}

		long Frequency
		{
			get;
			set;
		}

		long FrequencyShift
		{
			get;
			set;
		}

		bool FrequencyShiftEnabled
		{
			get;
			set;
		}

		bool MarkPeaks
		{
			get;
			set;
		}

		bool SnapToGrid
		{
			get;
			set;
		}

		bool SquelchEnabled
		{
			get;
			set;
		}

		int SquelchThreshold
		{
			get;
			set;
		}

		bool IsSquelchOpen
		{
			get;
		}

		bool SwapIq
		{
			get;
			set;
		}

		bool UseAgc
		{
			get;
			set;
		}

		bool AgcHang
		{
			get;
			set;
		}

		int AgcThreshold
		{
			get;
			set;
		}

		int AgcDecay
		{
			get;
			set;
		}

		int AgcSlope
		{
			get;
			set;
		}

		int FFTResolution
		{
			get;
		}

		int FFTSkips
		{
			set;
		}

		bool IsPlaying
		{
			get;
		}

		int SAttack
		{
			get;
			set;
		}

		int SDecay
		{
			get;
			set;
		}

		int WAttack
		{
			get;
			set;
		}

		int WDecay
		{
			get;
			set;
		}

		bool UseTimeMarkers
		{
			get;
			set;
		}

		string RdsProgramService
		{
			get;
		}

		string RdsRadioText
		{
			get;
		}

		int RFBandwidth
		{
			get;
		}

		void StartRadio();

		void StopRadio();

		void RegisterStreamHook(object streamHook, ProcessorType processorType);

		void UnregisterStreamHook(object streamHook);		
		
		event PropertyChangedEventHandler PropertyChanged;

		void GetSpectrumSnapshot(byte[] destArray);

        /*
		event CustomPaintEventHandler WaterfallCustomPaint;

		event CustomPaintEventHandler SpectrumAnalyzerCustomPaint;

		void SetFrequency(long frequency, bool onlyMoveCenterFrequency);

		[Obsolete("Use GetSpectrumSnapshot(float[], float, float) instead")]
		void GetSpectrumSnapshot(byte[] destArray);

		void GetSpectrumSnapshot(float[] destArray, float scale = 1f, float offset = 0f);

		void RegisterFrontControl(UserControl control, PluginPosition preferredPosition);

		void Perform();
        */
	}
}
