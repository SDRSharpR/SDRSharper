using SDRSharp.Common;
using SDRSharp.Radio;
using System.ComponentModel;
using System.Windows.Forms;

namespace SDRSharp.Equalizer
{
	public class EqualizerPlugin : ISharpPlugin
	{
		private const string _displayName = "Audio Equalizer";

		private ISharpControl _control;

		private EqualizerProcessor _audioProcessor;

		private EqualizerPanel _guiControl;

		public string DisplayName => "Audio Equalizer";

		public bool HasGui => true;

		public UserControl GuiControl => this._guiControl;

		public void Initialize(ISharpControl control)
		{
			this._control = control;
			this._control.PropertyChanged += this.PropertyChangedHandler;
			this._audioProcessor = new EqualizerProcessor();
			this._audioProcessor.Enabled = Utils.GetBooleanSetting("EqEnabled");
			this._audioProcessor.BassBoost = Utils.GetBooleanSetting("EqBassBoost");
			this._audioProcessor.LowCutoff = (float)Utils.GetIntSetting("EqLowCutoff", 200);
			this._audioProcessor.HighCutoff = (float)Utils.GetIntSetting("EqHighCutoff", 4000);
			this._audioProcessor.LowGain = (float)Utils.GetDoubleSetting("EqLowGain", 2.25);
			this._audioProcessor.MidGain = (float)Utils.GetDoubleSetting("EqMidGain", 1.125);
			this._audioProcessor.HighGain = (float)Utils.GetDoubleSetting("EqHighGain", 2.25);
			this._control.RegisterStreamHook(this._audioProcessor, ProcessorType.FilteredAudioOutput);
			this._guiControl = new EqualizerPanel(this._control, this._audioProcessor);
		}

		private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
			string propertyName;
			if ((propertyName = e.PropertyName) != null && !(propertyName == "StartRadio"))
			{
				bool flag = propertyName == "StopRadio";
			}
		}

		public void Close()
		{
			Utils.SaveSetting("EqEnabled", this._audioProcessor.Enabled);
			Utils.SaveSetting("EqBassBoost", this._audioProcessor.BassBoost);
			Utils.SaveSetting("EqLowCutoff", this._audioProcessor.LowCutoff);
			Utils.SaveSetting("EqHighCutoff", this._audioProcessor.HighCutoff);
			Utils.SaveSetting("EqLowGain", this._audioProcessor.LowGain);
			Utils.SaveSetting("EqMidGain", this._audioProcessor.MidGain);
			Utils.SaveSetting("EqHighGain", this._audioProcessor.HighGain);
		}
	}
}
