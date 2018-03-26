using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SDRSharp.Trunker
{
	public class SettingsPersisterTrunker
	{
		private const string SettingsFilename = "trunker.xml";

		private readonly string _settingsFolder;

		public SettingsPersisterTrunker()
		{
			this._settingsFolder = Path.GetDirectoryName(Application.ExecutablePath);
		}

		public List<TrunkerSettings> readConfig()
		{
			if (File.Exists("trunker.xml"))
			{
				List<TrunkerSettings> list = this.ReadObject<List<TrunkerSettings>>("trunker.xml");
				if (list != null)
				{
					return list;
				}
			}
			return new List<TrunkerSettings>();
		}

		public void writeConfig(List<TrunkerSettings> config)
		{
			this.WriteObject(config, "trunker.xml");
		}

		private T ReadObject<T>(string fileName)
		{
			string path = Path.Combine(this._settingsFolder, fileName);
			if (File.Exists(path))
			{
				using (FileStream stream = new FileStream(path, FileMode.Open))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					return (T)xmlSerializer.Deserialize(stream);
				}
			}
			return default(T);
		}

		private void WriteObject<T>(T obj, string fileName)
		{
			string path = Path.Combine(this._settingsFolder, fileName);
			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
				xmlSerializer.Serialize(stream, obj);
			}
		}
	}
}
