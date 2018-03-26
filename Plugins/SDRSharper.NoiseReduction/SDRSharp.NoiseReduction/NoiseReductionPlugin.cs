using SDRSharp.Common;
using SDRSharp.Radio;
using System.Windows.Forms;

namespace SDRSharp.NoiseReduction
{
	public class NoiseReductionPlugin : ISharpPlugin
	{
		private const string _displayName = "Noise Reduction";

		private ISharpControl _control;

		private AFProcessor _audioProcessor;

		private IFProcessor _ifProcessor;

		private NoiseBlankerProcessor _noiseProcessor;

		private NoiseReductionPanel _guiControl;

		public string DisplayName => "Noise Reduction";

		public bool HasGui => true;

		public UserControl GuiControl => this._guiControl;

		public void Initialize(ISharpControl control)
		{
			this._control = control;
			this._audioProcessor = new AFProcessor();
			this._control.RegisterStreamHook(this._audioProcessor, ProcessorType.FilteredAudioOutput);
			this._ifProcessor = new IFProcessor();
			this._control.RegisterStreamHook(this._ifProcessor, ProcessorType.DecimatedAndFilteredIQ);
			this._ifProcessor.NoiseThreshold = Utils.GetIntSetting("DNRIThreshold", -30);
			this._ifProcessor.Enabled = Utils.GetBooleanSetting("DNRIEnabled");
			this._audioProcessor.NoiseThreshold = Utils.GetIntSetting("DNRAThreshold", -70);
			this._audioProcessor.Enabled = Utils.GetBooleanSetting("DNRAEnabled");
			this._noiseProcessor = new NoiseBlankerProcessor();
			this._noiseProcessor.Enabled = Utils.GetBooleanSetting("NBEnabled");
			this._noiseProcessor.NoiseThreshold = Utils.GetIntSetting("NBThreshold", 80);
			this._noiseProcessor.PulseWidth = Utils.GetDoubleSetting("NBPulseWidth", 10.0);
			this._control.RegisterStreamHook(this._noiseProcessor, ProcessorType.RawIQ);
			this._guiControl = new NoiseReductionPanel(this._ifProcessor, this._audioProcessor, this._noiseProcessor);
		}

		public void Close()
		{
			Utils.SaveSetting("DNRIThreshold", this._ifProcessor.NoiseThreshold);
			Utils.SaveSetting("DNRIEnabled", this._ifProcessor.Enabled);
			Utils.SaveSetting("DNRAThreshold", this._audioProcessor.NoiseThreshold);
			Utils.SaveSetting("DNRAEnabled", this._audioProcessor.Enabled);
			Utils.SaveSetting("NBEnabled", this._noiseProcessor.Enabled);
			Utils.SaveSetting("NBThreshold", this._noiseProcessor.NoiseThreshold);
			Utils.SaveSetting("NBPulseWidth", this._noiseProcessor.PulseWidth);
		}
	}
}
