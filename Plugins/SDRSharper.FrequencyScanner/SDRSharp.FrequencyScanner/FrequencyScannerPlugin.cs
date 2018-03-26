using SDRSharp.Common;
using System.Windows.Forms;

namespace SDRSharp.FrequencyScanner
{
	public class FrequencyScannerPlugin : ISharpPlugin
	{
		private const string _displayName = "Frequency Scanner";

		private ISharpControl _controlInterface;

		private FrequencyScannerPanel _frequencyScannerPanel;

		public UserControl Gui => this._frequencyScannerPanel;

		public string DisplayName => "Frequency Scanner";

		public void Close()
		{
			if (this._frequencyScannerPanel != null)
			{
				this._frequencyScannerPanel.ScanStop();
				this._frequencyScannerPanel.SaveSettings();
			}
		}

		public void Initialize(ISharpControl control)
		{
			this._controlInterface = control;
			this._frequencyScannerPanel = new FrequencyScannerPanel(this._controlInterface);
		}
	}
}
