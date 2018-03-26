using System.Windows.Forms;

namespace SDRSharp.Common
{
	public interface ISharpPlugin
	{
		bool HasGui
		{
			get;
		}

		UserControl GuiControl
		{
			get;
		}

		string DisplayName
		{
			get;
		}

		void Initialize(ISharpControl control);

		void Close();
	}
}
