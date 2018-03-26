using SDRSharp.Radio;

namespace SDRSharp.FrequencyManager
{
	public class MemoryEntry
	{
		private long _frequency;

		private DetectorType _detectorType;

		private string _name;

		private long _shift;

		private long _centerFrequency;

		private string _groupName;

		private long _filterBandwidth;

		private bool _isFavourite;

		public bool IsFavourite
		{
			get
			{
				return this._isFavourite;
			}
			set
			{
				this._isFavourite = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string GroupName
		{
			get
			{
				return this._groupName;
			}
			set
			{
				this._groupName = value;
			}
		}

		public long Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				this._frequency = value;
			}
		}

		public DetectorType DetectorType
		{
			get
			{
				return this._detectorType;
			}
			set
			{
				this._detectorType = value;
			}
		}

		public long Shift
		{
			get
			{
				return this._shift;
			}
			set
			{
				this._shift = value;
			}
		}

		public long CenterFrequency
		{
			get
			{
				return this._centerFrequency;
			}
			set
			{
				this._centerFrequency = value;
			}
		}

		public long FilterBandwidth
		{
			get
			{
				return this._filterBandwidth;
			}
			set
			{
				this._filterBandwidth = value;
			}
		}

		public MemoryEntry()
		{
		}

		public MemoryEntry(MemoryEntry memoryEntry)
		{
			this._name = memoryEntry._name;
			this._groupName = memoryEntry._groupName;
			this._detectorType = memoryEntry._detectorType;
			this._frequency = memoryEntry._frequency;
			this._shift = memoryEntry._shift;
			this._centerFrequency = memoryEntry._centerFrequency;
			this._filterBandwidth = memoryEntry._filterBandwidth;
			this._isFavourite = memoryEntry._isFavourite;
		}
	}
}
