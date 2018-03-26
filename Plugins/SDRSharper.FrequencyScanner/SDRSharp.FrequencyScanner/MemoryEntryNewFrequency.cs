namespace SDRSharp.FrequencyScanner
{
	public class MemoryEntryNewFrequency
	{
		private long _frequency;

		private float _activity;

		private long _centerFrequency;

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

		public float Activity
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

		public MemoryEntryNewFrequency()
		{
		}

		public MemoryEntryNewFrequency(MemoryEntryNewFrequency memoryEntryNewFrequency)
		{
			this._frequency = memoryEntryNewFrequency._frequency;
			this._activity = memoryEntryNewFrequency._activity;
			this._centerFrequency = memoryEntryNewFrequency._centerFrequency;
		}
	}
}
