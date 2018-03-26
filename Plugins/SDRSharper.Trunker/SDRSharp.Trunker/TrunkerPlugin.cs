using SDRSharp.Common;
using System.Windows.Forms;

namespace SDRSharp.Trunker
{
	public class TrunkerPlugin : ISharpPlugin
	{
		private const string _displayName = "Trunker";

		private ISharpControl _controlInterface;

		private TrunkerPanel _trunkerPanel;

		private double _sampleRate;

		private bool _bypass = true;

		public bool Bypass
		{
			get
			{
				return this._bypass;
			}
			set
			{
				this._bypass = value;
			}
		}

		public bool HasGui => true;

		public UserControl GuiControl => this._trunkerPanel;

		public string DisplayName => "Trunker";

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				this._sampleRate = value;
			}
		}

		public void Initialize(ISharpControl control)
		{
			this._controlInterface = control;
			this._trunkerPanel = new TrunkerPanel(this._controlInterface, this);
			this._controlInterface.RegisterStreamHook((object)this);
		}

		public unsafe void Process(float* audioBuffer, int length)
		{
			if (!this._bypass)
			{
				this._trunkerPanel.MuteBeeps(audioBuffer, length);
			}
		}

		public void Close()
		{
		}
	}
}
