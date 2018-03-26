using SDRSharp.Common;
using System.Windows.Forms;

namespace SDRSharp.WavRecorder
{
	public class WavRecorderPlugin : ISharpPlugin
	{
		private const string DefaultDisplayName = "Recording";

		private ISharpControl _control;

		private RecordingPanel _guiControl;

		public bool HasGui => true;

		public UserControl GuiControl => this._guiControl;

		public string DisplayName => "Recording";

		public void Initialize(ISharpControl control)
		{
			this._control = control;
			this._guiControl = new RecordingPanel(this._control);
		}

		public void Close()
		{
			if (this._guiControl != null)
			{
				this._guiControl.AbortRecording();
			}
		}
	}
}
