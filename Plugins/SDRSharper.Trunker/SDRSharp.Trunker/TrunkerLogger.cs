namespace SDRSharp.Trunker
{
	public class TrunkerLogger
	{
		private string _action;

		private string _receiver;

		private decimal _frequency;

		private string _trunkgroup;

		private string _trunklabel;

		private string _sourcegroup;

		private string _sourcelabel;

		public string currentAction
		{
			get
			{
				return this._action;
			}
			set
			{
				this._action = value;
			}
		}

		public string currentReceiver
		{
			get
			{
				return this._receiver;
			}
			set
			{
				this._receiver = value;
			}
		}

		public decimal currentFrequency
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

		public string currentTrunkgroup
		{
			get
			{
				return this._trunkgroup;
			}
			set
			{
				this._trunkgroup = value;
			}
		}

		public string currentTrunklabel
		{
			get
			{
				return this._trunklabel;
			}
			set
			{
				this._trunklabel = value;
			}
		}

		public string currentSourcegroup
		{
			get
			{
				return this._sourcegroup;
			}
			set
			{
				this._sourcegroup = value;
			}
		}

		public string currentSourcelabel
		{
			get
			{
				return this._sourcelabel;
			}
			set
			{
				this._sourcelabel = value;
			}
		}

		public TrunkerLogger()
		{
		}

		public TrunkerLogger(TrunkerLogger trunkerLogger)
		{
			this._action = trunkerLogger._action;
			this._receiver = trunkerLogger._receiver;
			this._frequency = trunkerLogger._frequency;
			this._trunkgroup = trunkerLogger._trunkgroup;
			this._trunklabel = trunkerLogger._trunklabel;
			this._sourcegroup = trunkerLogger._sourcegroup;
			this._sourcelabel = trunkerLogger._sourcelabel;
		}

		public void settingsExist()
		{
		}
	}
}
