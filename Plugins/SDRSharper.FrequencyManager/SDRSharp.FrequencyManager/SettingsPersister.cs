using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SDRSharp.FrequencyManager
{
	public class SettingsPersister
	{
		private const string FreqMgrFilename = "frequencies.xml";

		private readonly string _settingsFolder;

		public SettingsPersister()
		{
			this._settingsFolder = Path.GetDirectoryName(Application.ExecutablePath);
		}

		public List<MemoryEntry> ReadStoredFrequencies()
		{
			List<MemoryEntry> list = this.ReadObject<List<MemoryEntry>>("frequencies.xml");
			if (list != null)
			{
				list.Sort((MemoryEntry e1, MemoryEntry e2) => e1.Frequency.CompareTo(e2.Frequency));
				return list;
			}
			return new List<MemoryEntry>();
		}

		public void PersistStoredFrequencies(List<MemoryEntry> entries)
		{
			this.WriteObject(entries, "frequencies.xml");
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
