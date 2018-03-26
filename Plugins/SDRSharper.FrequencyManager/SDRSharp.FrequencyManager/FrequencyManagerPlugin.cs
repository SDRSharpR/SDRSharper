using SDRSharp.Common;
using System.Windows.Forms;

namespace SDRSharp.FrequencyManager
{
	public class FrequencyManagerPlugin : ISharpPlugin
	{
		private const string _displayName = "Frequency Mgr";

		private ISharpControl _controlInterface;

		private FrequencyManagerPanel _frequencyManagerPanel;

		public bool HasGui => true;

		public UserControl GuiControl => this._frequencyManagerPanel;

		public string DisplayName => "Frequency Mgr";

		public void Close()
		{
		}

		public void Initialize(ISharpControl control)
		{
			this._controlInterface = control;
			this._frequencyManagerPanel = new FrequencyManagerPanel(this._controlInterface);
		}
	}
}
