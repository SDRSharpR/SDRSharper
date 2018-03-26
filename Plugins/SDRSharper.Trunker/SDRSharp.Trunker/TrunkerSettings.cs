namespace SDRSharp.Trunker
{
	public class TrunkerSettings
	{
		private string _unitrunkerPath;

		private string _logPath;

		private bool _enabled;

		private bool _loggingEnabled;

		private bool _delayRetune;

		private bool _tuneControl;

		private bool _muteEDACS;

		private bool _muteControl;

		private bool _ignoreParked;

		private int _delayRetuneMethod;

		private decimal _controlFreq;

		private decimal _delayRetuneMinimumSignal;

		private string _logStyle;

		private string _parkedString;

		private string _unknownSrcString;

		public bool isEnabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		public bool ignoreParked
		{
			get
			{
				return this._ignoreParked;
			}
			set
			{
				this._ignoreParked = value;
			}
		}

		public string unitrunkerPath
		{
			get
			{
				return this._unitrunkerPath;
			}
			set
			{
				this._unitrunkerPath = value;
			}
		}

		public string parkedStr
		{
			get
			{
				return this._parkedString;
			}
			set
			{
				this._parkedString = value;
			}
		}

		public string unknownSrcStr
		{
			get
			{
				return this._unknownSrcString;
			}
			set
			{
				this._unknownSrcString = value;
			}
		}

		public string logStyle
		{
			get
			{
				return this._logStyle;
			}
			set
			{
				this._logStyle = value;
			}
		}

		public string logPath
		{
			get
			{
				return this._logPath;
			}
			set
			{
				this._logPath = value;
			}
		}

		public bool isLoggingEnabled
		{
			get
			{
				return this._loggingEnabled;
			}
			set
			{
				this._loggingEnabled = value;
			}
		}

		public bool isDelayRetuneEnabled
		{
			get
			{
				return this._delayRetune;
			}
			set
			{
				this._delayRetune = value;
			}
		}

		public bool isControlAutotuneEnabled
		{
			get
			{
				return this._tuneControl;
			}
			set
			{
				this._tuneControl = value;
			}
		}

		public bool isMuteEDACSEnabled
		{
			get
			{
				return this._muteEDACS;
			}
			set
			{
				this._muteEDACS = value;
			}
		}

		public bool isMuteControlEnabled
		{
			get
			{
				return this._muteControl;
			}
			set
			{
				this._muteControl = value;
			}
		}

		public decimal controlFreq
		{
			get
			{
				return this._controlFreq;
			}
			set
			{
				this._controlFreq = value;
			}
		}

		public int delayRetuneMethod
		{
			get
			{
				return this._delayRetuneMethod;
			}
			set
			{
				this._delayRetuneMethod = value;
			}
		}

		public decimal delayRetuneMinimumSignal
		{
			get
			{
				return this._delayRetuneMinimumSignal;
			}
			set
			{
				this._delayRetuneMinimumSignal = value;
			}
		}

		public TrunkerSettings()
		{
		}

		public TrunkerSettings(TrunkerSettings trunkerSettings)
		{
			this._enabled = trunkerSettings._enabled;
			this._ignoreParked = trunkerSettings._ignoreParked;
			this._parkedString = trunkerSettings._parkedString;
			this._unknownSrcString = trunkerSettings._unknownSrcString;
			this._unitrunkerPath = trunkerSettings._unitrunkerPath;
			this._logPath = trunkerSettings._logPath;
			this._loggingEnabled = trunkerSettings._loggingEnabled;
			this._delayRetune = trunkerSettings._delayRetune;
			this._delayRetuneMethod = trunkerSettings._delayRetuneMethod;
			this._delayRetuneMinimumSignal = trunkerSettings._delayRetuneMinimumSignal;
			this._tuneControl = trunkerSettings._tuneControl;
			this._muteEDACS = trunkerSettings._muteEDACS;
			this._muteControl = trunkerSettings._muteControl;
			this._controlFreq = trunkerSettings._controlFreq;
			this._logStyle = trunkerSettings._logStyle;
		}

		public void settingsExist()
		{
		}
	}
}
