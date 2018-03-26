using SDRSharp.Radio;

namespace SDRSharp.FrequencyScanner
{
	public class ChannelAnalizerMemoryEntry
	{
		private long _frequency;

		private long _centerFrequency;

		private float _level;

		private int _activity;

		private bool _skipped;

		private bool _isStore;

		private string _storeName;

		private int _filterBandwidth;

		private DetectorType _detectorType;

		private int _stepSize;

		private int _startBins;

		private int _endBins;

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

		public string StoreName
		{
			get
			{
				return this._storeName;
			}
			set
			{
				this._storeName = value;
			}
		}

		public bool IsStore
		{
			get
			{
				return this._isStore;
			}
			set
			{
				this._isStore = value;
			}
		}

		public float Level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
			}
		}

		public bool Skipped
		{
			get
			{
				return this._skipped;
			}
			set
			{
				this._skipped = value;
			}
		}

		public int Activity
		{
			get
			{
				return this._activity;
			}
			set
			{
				this._activity = value;
			}
		}

		public int StartBins
		{
			get
			{
				return this._startBins;
			}
			set
			{
				this._startBins = value;
			}
		}

		public int EndBins
		{
			get
			{
				return this._endBins;
			}
			set
			{
				this._endBins = value;
			}
		}

		public int FilterBandwidth
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

		public int StepSize
		{
			get
			{
				return this._stepSize;
			}
			set
			{
				this._stepSize = value;
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

		public ChannelAnalizerMemoryEntry()
		{
		}

		public ChannelAnalizerMemoryEntry(ChannelAnalizerMemoryEntry ChannelAnalizerMemoryEntry)
		{
			this._frequency = ChannelAnalizerMemoryEntry._frequency;
			this._centerFrequency = ChannelAnalizerMemoryEntry._centerFrequency;
			this._level = ChannelAnalizerMemoryEntry._level;
			this._activity = ChannelAnalizerMemoryEntry._activity;
			this._skipped = ChannelAnalizerMemoryEntry._skipped;
			this._isStore = ChannelAnalizerMemoryEntry._isStore;
			this._storeName = ChannelAnalizerMemoryEntry._storeName;
			this._filterBandwidth = ChannelAnalizerMemoryEntry._filterBandwidth;
			this._detectorType = ChannelAnalizerMemoryEntry._detectorType;
			this._stepSize = ChannelAnalizerMemoryEntry._stepSize;
			this._startBins = ChannelAnalizerMemoryEntry._startBins;
			this._endBins = ChannelAnalizerMemoryEntry._endBins;
		}
	}
}
