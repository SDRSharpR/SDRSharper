using SDRSharp.Common;
using SDRSharp.Radio;
using System.Windows.Forms;

namespace SDRSharp.DNR
{
	public class DNRPlugin : ISharpPlugin
	{
		private const string _displayName = "Digital Noise Reduction";

		private ISharpControl _control;

		private AudioProcessor _audioProcessor;

		private IFProcessor _ifProcessor;

		private ProcessorPanel _guiControl;

		public string DisplayName => "Digital Noise Reduction";

		public bool HasGui => true;

		public UserControl GuiControl => this._guiControl;

		public void Initialize(ISharpControl control)
		{
			this._control = control;
			this._audioProcessor = new AudioProcessor();
			this._control.RegisterStreamHook(this._audioProcessor, ProcessorType.FilteredAudioOutput);
			this._ifProcessor = new IFProcessor();
			this._control.RegisterStreamHook(this._ifProcessor, ProcessorType.DecimatedAndFilteredIQ);
			this._ifProcessor.NoiseThreshold = Utils.GetIntSetting("DNRIThreshold", -30);
			this._ifProcessor.Enabled = Utils.GetBooleanSetting("DNRIEnabled");
			this._audioProcessor.NoiseThreshold = Utils.GetIntSetting("DNRAThreshold", -70);
			this._audioProcessor.Enabled = Utils.GetBooleanSetting("DNRAEnabled");
			this._guiControl = new ProcessorPanel(this._ifProcessor, this._audioProcessor);
		}

		public void Close()
		{
			Utils.SaveSetting("DNRIThreshold", this._ifProcessor.NoiseThreshold);
			Utils.SaveSetting("DNRIEnabled", this._ifProcessor.Enabled);
			Utils.SaveSetting("DNRAThreshold", this._audioProcessor.NoiseThreshold);
			Utils.SaveSetting("DNRAEnabled", this._audioProcessor.Enabled);
		}
	}
}
