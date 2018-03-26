using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SDRSharp.FrequencyScanner
{
	public class SettingsPersister
	{
		private const string FilenameScannerSettings = "scanner_entryes.xml";

		private const string FilenameFrequencyManager = "frequencies.xml";

		private readonly string _settingsFolder;

		public SettingsPersister()
		{
			this._settingsFolder = Path.GetDirectoryName(Application.ExecutablePath);
		}

		public MemoryEntryNewSkipAndRangeFrequency ReadStored()
		{
			MemoryEntryNewSkipAndRangeFrequency memoryEntryNewSkipAndRangeFrequency = this.ReadObject<MemoryEntryNewSkipAndRangeFrequency>("scanner_entryes.xml");
			if (memoryEntryNewSkipAndRangeFrequency != null)
			{
				return memoryEntryNewSkipAndRangeFrequency;
			}
			return new MemoryEntryNewSkipAndRangeFrequency();
		}

		public void PersistStored(MemoryEntryNewSkipAndRangeFrequency entries)
		{
			this.WriteObject(entries, "scanner_entryes.xml");
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
				using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
				}
			}
			return default(T);
		}

		private void WriteObject<T>(T obj, string fileName)
		{
			using (FileStream stream = new FileStream(Path.Combine(this._settingsFolder, fileName), FileMode.Create))
			{
				new XmlSerializer(obj.GetType()).Serialize(stream, obj);
			}
		}
	}
}
