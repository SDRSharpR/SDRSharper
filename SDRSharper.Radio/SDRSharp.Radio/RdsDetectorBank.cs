namespace SDRSharp.Radio
{
	public class RdsDetectorBank
	{
		private readonly RdsDumpGroups _dumpGroups;

		private readonly SyndromeDetector _detector;

		public string RadioText => this._dumpGroups.RadioText;

		public string ProgramService => this._dumpGroups.ProgramService;

		public ushort PICode => this._dumpGroups.PICode;

		public RdsDetectorBank()
		{
			this._dumpGroups = new RdsDumpGroups();
			this._detector = new SyndromeDetector(this._dumpGroups);
		}

		public void Process(bool b)
		{
			this._detector.Clock(b);
		}

		public void Reset()
		{
			this._dumpGroups.Reset();
		}
	}
}
