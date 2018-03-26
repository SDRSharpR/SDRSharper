using System.Collections.Generic;
using System.Drawing;

namespace SDRSharp.FrequencyScanner
{
	public class MemoryEntryNewSkipAndRangeFrequency
	{
		private List<long> _skipFrequencyArray;

		private List<MemoryEntryFrequencyRange> _scanFrequencyRange;

		private List<MemoryEntryNewFrequency> _newFrequency;

		private Point _lastLocationScreen;

		private Size _lastSizeScreen;

		private Point _lastLocationMultiSelect;

		private Size _lastSizeMultiSelect;

		public List<long> SkipFrequencyArray
		{
			get
			{
				return this._skipFrequencyArray;
			}
			set
			{
				this._skipFrequencyArray = value;
			}
		}

		public List<MemoryEntryFrequencyRange> FrequencyRange
		{
			get
			{
				return this._scanFrequencyRange;
			}
			set
			{
				this._scanFrequencyRange = value;
			}
		}

		public List<MemoryEntryNewFrequency> NewFrequency
		{
			get
			{
				return this._newFrequency;
			}
			set
			{
				this._newFrequency = value;
			}
		}

		public Size LastSizeScreen
		{
			get
			{
				return this._lastSizeScreen;
			}
			set
			{
				this._lastSizeScreen = value;
			}
		}

		public Point LastLocationScreen
		{
			get
			{
				return this._lastLocationScreen;
			}
			set
			{
				this._lastLocationScreen = value;
			}
		}

		public Size LastSizeMultiSelect
		{
			get
			{
				return this._lastSizeMultiSelect;
			}
			set
			{
				this._lastSizeMultiSelect = value;
			}
		}

		public Point LastLocationMultiSelect
		{
			get
			{
				return this._lastLocationMultiSelect;
			}
			set
			{
				this._lastLocationMultiSelect = value;
			}
		}

		public MemoryEntryNewSkipAndRangeFrequency()
		{
			this._scanFrequencyRange = new List<MemoryEntryFrequencyRange>();
			this._newFrequency = new List<MemoryEntryNewFrequency>();
			this._skipFrequencyArray = new List<long>();
			this._lastLocationScreen = default(Point);
			this._lastSizeScreen = default(Size);
			this._lastLocationMultiSelect = default(Point);
			this._lastSizeMultiSelect = default(Size);
		}

		public MemoryEntryNewSkipAndRangeFrequency(MemoryEntryNewSkipAndRangeFrequency memoryEntry)
		{
			this._skipFrequencyArray = memoryEntry._skipFrequencyArray;
			this._scanFrequencyRange = memoryEntry._scanFrequencyRange;
			this._newFrequency = memoryEntry._newFrequency;
			this._lastLocationScreen = memoryEntry._lastLocationScreen;
			this._lastSizeScreen = memoryEntry._lastSizeScreen;
			this._lastLocationMultiSelect = memoryEntry._lastLocationMultiSelect;
			this._lastSizeMultiSelect = memoryEntry._lastSizeMultiSelect;
		}
	}
}
