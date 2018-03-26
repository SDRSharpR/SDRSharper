using System;

namespace SDRSharp.FrequencyManager
{
	public class MemoryInfoEventArgs : EventArgs
	{
		private readonly MemoryEntry _memoryEntry;

		public MemoryEntry MemoryEntry => this._memoryEntry;

		public MemoryInfoEventArgs(MemoryEntry entry)
		{
			this._memoryEntry = entry;
		}
	}
}
