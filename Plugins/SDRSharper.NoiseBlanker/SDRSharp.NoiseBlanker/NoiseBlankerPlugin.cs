using SDRSharp.Common;
using SDRSharp.Radio;
using System.Windows.Forms;

namespace SDRSharp.NoiseBlanker
{
	public class NoiseBlankerPlugin : ISharpPlugin
	{
		private const string _displayName = "Noise Blanker";

		private ISharpControl _control;

		private NoiseBlankerProcessor _processor;

		private ProcessorPanel _guiControl;

		public string DisplayName => "Noise Blanker";

		public bool HasGui => true;

		public UserControl GuiControl => this._guiControl;

		public void Initialize(ISharpControl control)
		{
			this._control = control;
			this._processor = new NoiseBlankerProcessor();
			this._processor.Enabled = Utils.GetBooleanSetting("NBEnabled");
			this._processor.NoiseThreshold = Utils.GetIntSetting("NBThreshold", 80);
			this._processor.PulseWidth = Utils.GetDoubleSetting("NBPulseWidth", 10.0);
			this._guiControl = new ProcessorPanel(this._processor);
			this._control.RegisterStreamHook(this._processor, ProcessorType.RawIQ);
		}

		public void Close()
		{
			Utils.SaveSetting("NBEnabled", this._processor.Enabled);
			Utils.SaveSetting("NBThreshold", this._processor.NoiseThreshold);
			Utils.SaveSetting("NBPulseWidth", this._processor.PulseWidth);
		}
	}
}
