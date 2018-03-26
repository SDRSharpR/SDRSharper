using SDRSharp.Radio;
using System.Drawing;

namespace SDRSharp.FrequencyScanner
{
	public class MemoryEntryFrequencyRange
	{
		private long _startFrequency;

		private long _endFrequency;

		private string _rangeName;

		private DetectorType _detectorType;

		private int _stepSize;

		private int _filterBandwidth;

		private Point _lastLocation;

		private Size _lastSize;

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

		public Size LastSize
		{
			get
			{
				return this._lastSize;
			}
			set
			{
				this._lastSize = value;
			}
		}

		public Point LastLocation
		{
			get
			{
				return this._lastLocation;
			}
			set
			{
				this._lastLocation = value;
			}
		}

		public long StartFrequency
		{
			get
			{
				return this._startFrequency;
			}
			set
			{
				this._startFrequency = value;
			}
		}

		public long EndFrequency
		{
			get
			{
				return this._endFrequency;
			}
			set
			{
				this._endFrequency = value;
			}
		}

		public string RangeName
		{
			get
			{
				return this._rangeName;
			}
			set
			{
				this._rangeName = value;
			}
		}

		public DetectorType RangeDetectorType
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

		public MemoryEntryFrequencyRange()
		{
		}

		public MemoryEntryFrequencyRange(MemoryEntryFrequencyRange memoryEntryFrequencyRange)
		{
			this._startFrequency = memoryEntryFrequencyRange._startFrequency;
			this._endFrequency = memoryEntryFrequencyRange._endFrequency;
			this._rangeName = memoryEntryFrequencyRange._rangeName;
			this._detectorType = memoryEntryFrequencyRange._detectorType;
			this._lastLocation = memoryEntryFrequencyRange._lastLocation;
			this._lastSize = memoryEntryFrequencyRange._lastSize;
			this._stepSize = memoryEntryFrequencyRange._stepSize;
			this._filterBandwidth = memoryEntryFrequencyRange._filterBandwidth;
		}
	}
}
