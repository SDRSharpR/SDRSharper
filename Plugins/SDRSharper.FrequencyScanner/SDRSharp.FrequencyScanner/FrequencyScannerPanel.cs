using SDRSharp.Common;
using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace SDRSharp.FrequencyScanner
{
	public class FrequencyScannerPanel : UserControl
	{
		private enum ScanState
		{
			SetScreen,
			PauseToSet,
			ScanScreen,
			ScanStop
		}

		private class CenterFrequencyEntry
		{
			public long CenterFrequency
			{
				get;
				set;
			}

			public int StartFrequencyIndex
			{
				get;
				set;
			}

			public int EndFrequencyIndex
			{
				get;
				set;
			}
		}

		private class IControlParameters
		{
			private bool _audioIsMuted;

			private bool _audioNeedUpdate;

			private long _centerFrequency;

			private bool _centerNeedUpdate;

			private long _frequency;

			private bool _frequencyNeedUpdate;

			private DetectorType _detectorType;

			private bool _detectorNeedUpdate;

			private int _filterBandwidth;

			private bool _filterNeedUpdate;

			private int _rfBandwidth;

			private bool _isChanged;

			public bool AudioIsMuted
			{
				get
				{
					return this._audioIsMuted;
				}
				set
				{
					this._audioIsMuted = value;
					this._audioNeedUpdate = true;
					this._isChanged = true;
				}
			}

			public bool AudioNeedUpdate => this._audioNeedUpdate;

			public long CenterFrequency
			{
				get
				{
					return this._centerFrequency;
				}
				set
				{
					this._centerFrequency = value;
					this._centerNeedUpdate = true;
					this._isChanged = true;
				}
			}

			public bool CenterNeedUpdate => this._centerNeedUpdate;

			public long Frequency
			{
				get
				{
					return this._frequency;
				}
				set
				{
					this._frequency = value;
					this._frequencyNeedUpdate = true;
					this._isChanged = true;
				}
			}

			public bool FrequencyNeedUpdate => this._frequencyNeedUpdate;

			public DetectorType DetectorType
			{
				get
				{
					return this._detectorType;
				}
				set
				{
					this._detectorType = value;
					this._detectorNeedUpdate = true;
					this._isChanged = true;
				}
			}

			public bool DetectorNeedUpdate => this._detectorNeedUpdate;

			public int FilterBandwidth
			{
				get
				{
					return this._filterBandwidth;
				}
				set
				{
					this._filterBandwidth = value;
					this._filterNeedUpdate = true;
					this._isChanged = true;
				}
			}

			public bool FilterNeedUpdate => this._filterNeedUpdate;

			public int RfBandwidth
			{
				get
				{
					return this._rfBandwidth;
				}
				set
				{
					this._rfBandwidth = value;
				}
			}

			public bool IsChanged => this._isChanged;

			public void ResetFlags()
			{
				this._audioNeedUpdate = false;
				this._centerNeedUpdate = false;
				this._detectorNeedUpdate = false;
				this._filterNeedUpdate = false;
				this._frequencyNeedUpdate = false;
				this._isChanged = false;
			}
		}

		private enum Buttons
		{
			None,
			SNRUp,
			SNRDown,
			HystUp,
			HystDown
		}

		private ISharpControl _controlInterface;

		private IFProcessor _ifProcessor;

		private ChannelAnalyzer _channelAnalyzer;

		private readonly SortableBindingList<MemoryEntryNewFrequency> _displayedEntries = new SortableBindingList<MemoryEntryNewFrequency>();

		private readonly List<MemoryEntryNewFrequency> _entriesNewFrequency;

		private List<MemoryEntry> _entriesInManager;

		private List<MemoryEntryFrequencyRange> _enteriesFrequencyRange;

		private MemoryEntryNewSkipAndRangeFrequency _newSkipAndRange;

		private List<long> _skipFrequencyList;

		private readonly SettingsPersister _settingsPersister;

		private List<long> _newActiveFrequencies;

		private List<ChannelAnalizerMemoryEntry> _channelList;

		private int _currentStartIndex;

		private int _currentEndIndex;

		private int _currentScreenIndex;

		private bool _isPlayed;

		private bool _isActivePlaedFrequency;

		private bool _scanMemory;

		private bool _scanWidthStoreNew;

		private bool _scanExceptMemory;

		private ScanState _state;

		private float _interval;

		private float _pauseAfter;

		private bool _changeFrequencyInScanner;

		private bool _timeToClean;

		private System.Timers.Timer _autoSkipTimer;

		private System.Timers.Timer _autoLockTimer;

		private const int FFTBins = 32768;

		private int _fftBins;

		private UnsafeBuffer _fftBuffer;

		private unsafe Complex* _fftPtr;

		private UnsafeBuffer _fftWindow;

		private unsafe float* _fftWindowPtr;

		private UnsafeBuffer _unScaledFFTSpectrum;

		private unsafe float* _unScaledFFTSpectrumPtr;

		private UnsafeBuffer _scaledFFTSpectrum;

		private unsafe float* _scaledFFTSpectrumPtr;

		private readonly SharpEvent _bufferEvent = new SharpEvent(false);

		private readonly SharpEvent _scannerEvent = new SharpEvent(false);

		private float _timeConstant;

		private int _squelchCount;

		private int _count;

		private bool _scanProcessIsWork;

		private List<CenterFrequencyEntry> _centerFrequncyList;

		private float _usableSpectrum;

		private bool _requestToStopScanner;

		private bool _directionReverse;

		private List<int> _activeFrequencies;

		private IControlParameters _controlParameters = new IControlParameters();

		private int _writePos;

		private Thread _scannerThread;

		private bool _scannerRunning;

		private bool _needUpdateParametrs;

		private int _scanLevel;

		private int _hysteresis;

		private bool _pauseScan;

		private bool _nextButtonPress;

		private bool _direction;

		private int _detect;

		private int _pauseToNextScreen;

		private int _debugCounter;

		private int _debugTime;

		private double _bufferLengthInMs;

		private TuningStyle _tuningStyleTemp;

		private float _tuningLimitTemp;

		private bool _hotTrackNeeded;

		private bool _selectFrequency;

		private int _trackingX;

		private int _trackingY;

		private int _startSelectX;

		private int _endSelectX;

		private int _oldX;

		private int _trackingIndex;

		private long _trackingFrequency;

		private int _oldIndex;

		private long _oldFrequency;

		private int _startSelectIndex;

		private int _endSelectIndex;

		public bool AutoSkipEnabled;

		public int AutoSkipInterval;

		public bool AutoLockEnabled;

		public int AutoLockInterval;

		public bool AutoClearEnabled;

		public float AutoClearActivityLevel;

		public int AutoClearInterval;

		public bool ChannelDetectMetod;

		public bool UseMute;

		public int UnMuteDelay;

		public float UsableSpectrum;

		public bool MaxLevelSelect;

		public bool ShowDebugInfo;

		public int MaxButtonsAlpha;

		public int MinButtonsAlpha;

		public int ScannerPluginPosition;

		private Rectangle _reverseRectangle;

		private Rectangle _forwardRectangle;

		private Rectangle _pauseRectangle;

		private Rectangle _lockRectangle;

		private Rectangle _unlockRectangle;

		private Rectangle _snrPlusRectangle;

		private Rectangle _snrMinusRectangle;

		private Rectangle _histPlusRectangle;

		private Rectangle _histMinusRectangle;

		private int _bright;

		private string _infoText;

		private int _filterWidth;

		private float _iavg;

		private Buttons _buttons;

		private bool _frequencyIsChanged;

		private bool _centerFrequencyIsChanged;

		private IContainer components;

		private ToolStrip mainToolStrip;

		private ToolStripButton btnNewEntry;

		private ToolStripButton btnDelete;

		private DataGridView frequencyDataGridView;

		private ComboBox scanModeComboBox;

		private BindingSource memoryEntryBindingSource;

		private Button ScanButton;

		private Button FrequencyRangeEditButton;

		private System.Windows.Forms.Timer newFrequencyDisplayUpdateTimer;

		private Button configureButton;

		private System.Windows.Forms.Timer autoCleanTimer;

		private Label timeConstantLabel;

		private System.Windows.Forms.Timer testDisplayTimer;

		private DataGridViewTextBoxColumn Activity;

		private DataGridViewTextBoxColumn frequencyDataGridViewTextBoxColumn;

		private ListBox frequencyRangeListBox;

		private TableLayoutPanel tableLayoutPanel1;

		private System.Windows.Forms.Timer iControlsUpdateTimer;

		private System.Windows.Forms.Timer refreshTimer;

		private Label label1;

		private Label label2;

		private NumericUpDown detectNumericUpDown;

		private NumericUpDown waitNumericUpDown;

		private System.Windows.Forms.Timer buttonsRepeaterTimer;

		public unsafe FrequencyScannerPanel(ISharpControl control)
		{
			this.InitializeComponent();
			this._controlInterface = control;
			this._controlInterface.PropertyChanged += this.PropertyChangedHandler;
			this._ifProcessor = new IFProcessor();
			this._controlInterface.RegisterStreamHook(this._ifProcessor, ProcessorType.RawIQ);
			this._ifProcessor.Enabled = false;
			this._ifProcessor.IQReady += this.FastBufferAvailable;
			this._fftBins = 32768;
			this.InitFFTBuffers();
			this.BuildFFTWindow();
			this._settingsPersister = new SettingsPersister();
			this._newSkipAndRange = this._settingsPersister.ReadStored();
			this._entriesInManager = this._settingsPersister.ReadStoredFrequencies();
			this._entriesNewFrequency = this._newSkipAndRange.NewFrequency;
			this._enteriesFrequencyRange = this._newSkipAndRange.FrequencyRange;
			this._skipFrequencyList = this._newSkipAndRange.SkipFrequencyArray;
			this.memoryEntryBindingSource.DataSource = this._displayedEntries;
			this.ProcessRangeName();
			int[] intArraySetting = Utils.GetIntArraySetting("FrequencyRangeIndexes", null);
			if (intArraySetting != null && intArraySetting.Length != 0)
			{
				for (int i = 0; i < intArraySetting.Length; i++)
				{
					if (intArraySetting[i] < this._enteriesFrequencyRange.Count)
					{
						this.frequencyRangeListBox.SetSelected(intArraySetting[i], true);
					}
				}
			}
			int num = Utils.GetIntSetting("ScanModeIndex", 0);
			if (num >= this.scanModeComboBox.Items.Count)
			{
				num = 0;
			}
			this.scanModeComboBox.SelectedIndex = num;
			this.scanModeComboBox_SelectedIndexChanged(null, null);
			this.InitDisplayEntry();
			this.ScanButton.Enabled = false;
			this.AutoSkipEnabled = Utils.GetBooleanSetting("AutoSkipEnabled");
			this.AutoSkipInterval = Utils.GetIntSetting("AutoSkipIntervalInSec", 30);
			this.AutoLockEnabled = Utils.GetBooleanSetting("AutoLockEnabled");
			this.AutoLockInterval = Utils.GetIntSetting("AutoLockIntervalInSec", 60);
			this.AutoClearEnabled = Utils.GetBooleanSetting("AutoClearEnabled");
			this.AutoClearActivityLevel = (float)Utils.GetDoubleSetting("AutoClearActivityLevel", 1.0);
			this.AutoClearInterval = Utils.GetIntSetting("AutoClearIntervalInSec", 10);
			this.ChannelDetectMetod = Utils.GetBooleanSetting("ChannelDetectOnCenterFrequency");
			this.UseMute = Utils.GetBooleanSetting("ScannerUseAudioMute");
			this.UnMuteDelay = Utils.GetIntSetting("ScannerUnMuteDelay", 5);
			this._detect = Utils.GetIntSetting("ScannerDetect", 100);
			this.detectNumericUpDown.Value = this._detect;
			this.MaxLevelSelect = Utils.GetBooleanSetting("ScannerMaxSelect");
			this._pauseToNextScreen = Utils.GetIntSetting("PauseToNextScreenInMs", 2000);
			this.waitNumericUpDown.Value = (decimal)((float)this._pauseToNextScreen / 1000f);
			this._hysteresis = Utils.GetIntSetting("ScannerHysteresis", 10);
			this._scanLevel = Utils.GetIntSetting("ScanLevel", 30);
			this.MaxButtonsAlpha = Utils.GetIntSetting("ScannerButtonsMaxAlpha", 150);
			this.MinButtonsAlpha = Utils.GetIntSetting("ScannerButtonsMinAlpha", 40);
			this.ScannerPluginPosition = Utils.GetIntSetting("ScannerPluginPosition", 1);
			this._autoLockTimer = new System.Timers.Timer();
			this._autoLockTimer.AutoReset = true;
			this._autoLockTimer.Elapsed += this.AutoLock_Tick;
			this._autoSkipTimer = new System.Timers.Timer();
			this._autoSkipTimer.AutoReset = true;
			this._autoSkipTimer.Elapsed += this.AutoSkip_Tick;
			this._state = ScanState.ScanStop;
			this._channelAnalyzer = new ChannelAnalyzer();
			this._controlInterface.RegisterFrontControl((UserControl)this._channelAnalyzer, this.ScannerPluginPosition);
			this._channelAnalyzer.Visible = false;
			this._channelAnalyzer.CustomPaint += this._channelAnalyzer_CustomPaint;
			this._channelAnalyzer.MouseMove += this._channelAnalyzer_MouseMove;
			this._channelAnalyzer.MouseDown += this._channelAnalyzer_MouseDown;
			this._channelAnalyzer.MouseUp += this._channelAnalyzer_MouseUp;
			this._channelAnalyzer.MouseEnter += this._channelAnalyzer_MouseEnter;
			this._channelAnalyzer.MouseLeave += this._channelAnalyzer_MouseLeave;
			this._channelAnalyzer.MouseWheel += this._channelAnalyzer_MouseWheel;
		}

		public void SaveSettings()
		{
			Utils.SaveSetting("ScanModeIndex", this.scanModeComboBox.SelectedIndex);
			int[] array = new int[this.frequencyRangeListBox.SelectedIndices.Count];
			this.frequencyRangeListBox.SelectedIndices.CopyTo(array, 0);
			Utils.SaveSetting("FrequencyRangeIndexes", Utils.IntArrayToString(array));
			Utils.SaveSetting("AutoSkipEnabled", this.AutoSkipEnabled);
			Utils.SaveSetting("AutoSkipIntervalInSec", this.AutoSkipInterval);
			Utils.SaveSetting("AutoLockEnabled", this.AutoLockEnabled);
			Utils.SaveSetting("AutoLockIntervalInSec", this.AutoLockInterval);
			Utils.SaveSetting("AutoClearEnabled", this.AutoClearEnabled);
			Utils.SaveSetting("AutoClearActivityLevel", this.AutoClearActivityLevel);
			Utils.SaveSetting("AutoClearIntervalInSec", this.AutoClearInterval);
			Utils.SaveSetting("ChannelDetectOnCenterFrequency", this.ChannelDetectMetod);
			Utils.SaveSetting("ScannerUseAudioMute", this.UseMute);
			Utils.SaveSetting("ScannerUnMuteDelay", this.UnMuteDelay);
			Utils.SaveSetting("ScannerDetect", this._detect);
			Utils.SaveSetting("ScanLevel", this._scanLevel);
			Utils.SaveSetting("PauseToNextScreenInMs", this._pauseToNextScreen);
			Utils.SaveSetting("ScannerHysteresis", this._hysteresis);
			Utils.SaveSetting("ScannerMaxSelect", this.MaxLevelSelect);
			Utils.SaveSetting("ScannerButtonsMaxAlpha", this.MaxButtonsAlpha);
			Utils.SaveSetting("ScannerButtonsMinAlpha", this.MinButtonsAlpha);
			Utils.SaveSetting("ScannerPluginPosition", this.ScannerPluginPosition);
		}

		private void StoreEntry()
		{
			if (this.memoryEntryBindingSource != null)
			{
				this._entriesNewFrequency.Clear();
				foreach (MemoryEntryNewFrequency item in this.memoryEntryBindingSource)
				{
					this._entriesNewFrequency.Add(item);
				}
				this._entriesNewFrequency.Sort((MemoryEntryNewFrequency e1, MemoryEntryNewFrequency e2) => e1.Frequency.CompareTo(e2.Frequency));
				this.InitDisplayEntry();
			}
			if (this._channelList != null)
			{
				for (int i = 0; i < this._channelList.Count; i++)
				{
					bool flag = this._channelList[i].Skipped;
					int num = 0;
					while (num < this._skipFrequencyList.Count)
					{
						if (this._channelList[i].Frequency != this._skipFrequencyList[num])
						{
							num++;
							continue;
						}
						if (!this._channelList[i].Skipped)
						{
							this._skipFrequencyList.RemoveAt(num);
						}
						flag = false;
						break;
					}
					if (flag)
					{
						this._skipFrequencyList.Add(this._channelList[i].Frequency);
					}
				}
			}
			this._newSkipAndRange.NewFrequency = this._entriesNewFrequency;
			this._newSkipAndRange.SkipFrequencyArray = this._skipFrequencyList;
			this._newSkipAndRange.FrequencyRange = this._enteriesFrequencyRange;
			this._settingsPersister.PersistStored(this._newSkipAndRange);
		}

		private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			switch (propertyName)
			{
			default:
				if (propertyName == "CenterFrequency")
				{
					this._centerFrequencyIsChanged = true;
				}
				break;
			case "StartRadio":
				this.ScanButton.Enabled = true;
				break;
			case "StopRadio":
				if (this._state != ScanState.ScanStop)
				{
					this.ScanStop();
				}
				this.ScanButton.Enabled = false;
				break;
			case "Frequency":
				if (!this._changeFrequencyInScanner && this._state != ScanState.ScanStop)
				{
					this._channelAnalyzer.Frequency = this._controlInterface.Frequency;
				}
				this._frequencyIsChanged = true;
				break;
			}
		}

		private void configureButton_Click(object sender, EventArgs e)
		{
			new DialogConfigure(this).ShowDialog();
		}

		private void detectNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			this._detect = (int)this.detectNumericUpDown.Value;
		}

		private void waitNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			this._pauseToNextScreen = (int)(this.waitNumericUpDown.Value * 1000m);
		}

		private void scanModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.scanModeComboBox.SelectedIndex)
			{
			case 0:
				this._scanWidthStoreNew = true;
				this._scanMemory = false;
				this._scanExceptMemory = false;
				break;
			case 1:
				this._scanWidthStoreNew = false;
				this._scanMemory = false;
				this._scanExceptMemory = false;
				break;
			case 2:
				this._scanWidthStoreNew = false;
				this._scanMemory = true;
				this._scanExceptMemory = false;
				break;
			case 3:
				this._scanWidthStoreNew = true;
				this._scanMemory = false;
				this._scanExceptMemory = true;
				break;
			}
		}

		private static string GetFrequencyDisplay(long frequency)
		{
			long num = Math.Abs(frequency);
			if (num == 0L)
			{
				return "DC";
			}
			if (num > 1500000000)
			{
				return $"{(double)frequency / 1000000000.0:#,0.000 000} GHz";
			}
			if (num > 30000000)
			{
				return $"{(double)frequency / 1000000.0:0,0.000###} MHz";
			}
			if (num > 1000)
			{
				return $"{(double)frequency / 1000.0:#,#.###} kHz";
			}
			return frequency.ToString();
		}

		private void InitDisplayEntry()
		{
			this.memoryEntryBindingSource.Clear();
			this._displayedEntries.Clear();
			if (this._entriesNewFrequency != null)
			{
				foreach (MemoryEntryNewFrequency item in this._entriesNewFrequency)
				{
					this._displayedEntries.Add(item);
				}
			}
		}

		private void AddNewFrequencyEntry(long frequency)
		{
			MemoryEntryNewFrequency memoryEntryNewFrequency = new MemoryEntryNewFrequency();
			memoryEntryNewFrequency.Frequency = frequency;
			memoryEntryNewFrequency.Activity = this._timeConstant * 0.001f;
			memoryEntryNewFrequency.CenterFrequency = this._controlInterface.CenterFrequency;
			this._displayedEntries.Add(memoryEntryNewFrequency);
		}

		private void frequencyDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (this.frequencyDataGridView.Columns[e.ColumnIndex].DataPropertyName == "Frequency" && e.Value != null)
			{
				long frequency = (long)e.Value;
				e.Value = FrequencyScannerPanel.GetFrequencyDisplay(frequency);
				e.FormattingApplied = true;
			}
		}

		private void frequencyDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			this.Navigate();
		}

		private void frequencyDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			this.btnDelete.Enabled = (this.frequencyDataGridView.SelectedRows.Count > 0);
		}

		private void frequencyDataGridView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.Navigate();
				e.Handled = true;
			}
		}

		private void btnClearEntry_Click(object sender, EventArgs e)
		{
			this._entriesNewFrequency.Clear();
			this._newSkipAndRange.NewFrequency = this._entriesNewFrequency;
			this._displayedEntries.Clear();
			if (this._state == ScanState.ScanStop)
			{
				this._settingsPersister.PersistStored(this._newSkipAndRange);
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			int num = this.frequencyDataGridView.SelectedRows.Count;
			while (num > 0 && this.memoryEntryBindingSource.Count > 0)
			{
				MemoryEntryNewFrequency item = (MemoryEntryNewFrequency)this.memoryEntryBindingSource.Current;
				this._displayedEntries.Remove(item);
				num--;
			}
		}

		public void Navigate()
		{
			if (this._controlInterface.IsPlaying)
			{
				int num = (this.frequencyDataGridView.SelectedCells.Count > 0) ? this.frequencyDataGridView.SelectedCells[0].RowIndex : (-1);
				if (num != -1)
				{
					try
					{
						MemoryEntryNewFrequency memoryEntryNewFrequency = (MemoryEntryNewFrequency)this.memoryEntryBindingSource.List[num];
						this._changeFrequencyInScanner = true;
						this._controlInterface.CenterFrequency = memoryEntryNewFrequency.CenterFrequency;
						this._controlInterface.Frequency = memoryEntryNewFrequency.Frequency;
						this._changeFrequencyInScanner = false;
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
			}
		}

		private void UpdateNewFrequencyDisplay()
		{
			if (this._newActiveFrequencies.Count > 0)
			{
				do
				{
					bool flag = true;
					if (this.memoryEntryBindingSource.Count > 0)
					{
						foreach (MemoryEntryNewFrequency item in this.memoryEntryBindingSource)
						{
							if (item.Frequency == this._newActiveFrequencies[0])
							{
								item.Activity += this._timeConstant * 0.001f;
								this.memoryEntryBindingSource.ResetItem(this.memoryEntryBindingSource.IndexOf(item));
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						this.AddNewFrequencyEntry(this._newActiveFrequencies[0]);
					}
					this._newActiveFrequencies.RemoveAt(0);
				}
				while (this._newActiveFrequencies.Count > 0);
			}
			if (this.memoryEntryBindingSource.Count > 0)
			{
				for (int i = 0; i < this._channelList.Count; i++)
				{
					if (this._channelList[i].Skipped)
					{
						foreach (MemoryEntryNewFrequency item2 in this.memoryEntryBindingSource)
						{
							if (item2.Frequency == this._channelList[i].Frequency)
							{
								this.memoryEntryBindingSource.Remove(item2);
								break;
							}
						}
						if (this.memoryEntryBindingSource.Count == 0)
						{
							break;
						}
					}
				}
				if (this._timeToClean && this.memoryEntryBindingSource.Count > 0)
				{
					int position = this.memoryEntryBindingSource.Position;
					this._timeToClean = false;
					int num = 0;
					while (num < this.memoryEntryBindingSource.Count)
					{
						if (((MemoryEntryNewFrequency)this.memoryEntryBindingSource[num]).Activity < this.AutoClearActivityLevel)
						{
							this.memoryEntryBindingSource.RemoveAt(num);
						}
						else
						{
							num++;
						}
					}
					if (position < this.memoryEntryBindingSource.Count)
					{
						this.memoryEntryBindingSource.Position = position;
					}
					else if (this.memoryEntryBindingSource.Count > 0)
					{
						this.memoryEntryBindingSource.Position = this.memoryEntryBindingSource.Count - 1;
					}
				}
				this.autoCleanTimer.Interval = this.AutoClearInterval * 1000;
				this.autoCleanTimer.Enabled = this.AutoClearEnabled;
			}
		}

		private void ScanButton_Click(object sender, EventArgs e)
		{
			if (this.ScanButton.Text == "Scan" && this._controlInterface.IsPlaying)
			{
				this.ScanStart();
			}
			else
			{
				this.ScanStop();
			}
		}

		private void ScanStart()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (this.CheckConditions())
			{
				this._tuningStyleTemp = this._controlInterface.get_TuningStyle();
				this._controlInterface.set_TuningStyle(0);
				this._tuningLimitTemp = this._controlInterface.get_TuningLimit();
				this._controlInterface.set_TuningLimit(0.5f);
				this._entriesInManager = this._settingsPersister.ReadStoredFrequencies();
				this.frequencyRangeListBox.Enabled = false;
				this.FrequencyRangeEditButton.Enabled = false;
				this.scanModeComboBox.Enabled = false;
				this.ScanButton.Text = "Stop scan";
				this._state = ScanState.SetScreen;
				this._pauseScan = false;
				this._usableSpectrum = (float)this._controlInterface.get_RFDisplayBandwidth() / (float)this._controlInterface.RFBandwidth;
				this.CreateChannelList();
				this._currentScreenIndex = -1;
				this._pauseAfter = 0f;
				this._newActiveFrequencies = new List<long>();
				this._activeFrequencies = new List<int>();
				if (this._scanWidthStoreNew)
				{
					this.newFrequencyDisplayUpdateTimer.Enabled = true;
				}
				this._channelAnalyzer.Zoom = 0;
				this._channelAnalyzer.ZoomPosition = 0;
				this._channelAnalyzer.Visible = true;
				this._autoSkipTimer.Interval = (double)(this.AutoSkipInterval * 1000);
				this._autoSkipTimer.Enabled = this.AutoSkipEnabled;
				this._autoLockTimer.Interval = (double)(this.AutoLockInterval * 1000);
				this._autoLockTimer.Enabled = this.AutoLockEnabled;
				this.autoCleanTimer.Interval = this.AutoClearInterval * 1000;
				this.autoCleanTimer.Enabled = this.AutoClearEnabled;
				if (this.UseMute)
				{
					this._controlInterface.set_AudioIsMuted(true);
				}
				this._scanProcessIsWork = false;
				this._scannerThread = new Thread(this.ScanProcess);
				this._scannerThread.Name = "Scanner Process";
				this._bufferEvent.Reset();
				this._scannerRunning = true;
				this._scannerThread.Start();
				this.iControlsUpdateTimer.Enabled = true;
				this._ifProcessor.Enabled = true;
			}
		}

		public void ScanStop()
		{
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			if (!(this.ScanButton.Text == "Scan"))
			{
				this._state = ScanState.ScanStop;
				this._ifProcessor.Enabled = false;
				this._autoSkipTimer.Enabled = false;
				this._autoLockTimer.Enabled = false;
				this.autoCleanTimer.Enabled = false;
				this.newFrequencyDisplayUpdateTimer.Enabled = false;
				this._scannerRunning = false;
				this._bufferEvent.Set();
				this._scannerThread.Join();
				if (this._scannerThread != null)
				{
					this._scannerThread = null;
				}
				this.iControlsUpdateTimer.Enabled = false;
				this.StoreEntry();
				this._channelAnalyzer.Visible = false;
				this.frequencyRangeListBox.Enabled = true;
				this.FrequencyRangeEditButton.Enabled = true;
				this.scanModeComboBox.Enabled = true;
				this._controlInterface.set_TuningStyle(this._tuningStyleTemp);
				this._controlInterface.set_TuningLimit(this._tuningLimitTemp);
				if (this.UseMute)
				{
					this._controlInterface.set_AudioIsMuted(false);
				}
				this.ScanButton.Text = "Scan";
			}
		}

		private void PanViewClose(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			if (this._scannerRunning)
			{
				this._requestToStopScanner = true;
			}
		}

		private void CreateChannelList()
		{
			this._channelList = new List<ChannelAnalizerMemoryEntry>();
			this._centerFrequncyList = new List<CenterFrequencyEntry>();
			int rFBandwidth = this._controlInterface.RFBandwidth;
			int num = (int)((float)this._controlInterface.RFBandwidth * this._usableSpectrum);
			int count = this.frequencyRangeListBox.SelectedIndices.Count;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				MemoryEntryFrequencyRange memoryEntryFrequencyRange = new MemoryEntryFrequencyRange();
				int num3 = this.frequencyRangeListBox.SelectedIndices[i];
				if (num3 < 1)
				{
					long num4 = this._controlInterface.CenterFrequency / this._controlInterface.get_StepSize() * this._controlInterface.get_StepSize();
					int stepSize = this._controlInterface.get_StepSize();
					int num5 = num / stepSize / 2 * 2;
					memoryEntryFrequencyRange.StartFrequency = num4 - num5 / 2 * stepSize;
					memoryEntryFrequencyRange.EndFrequency = num4 + num5 / 2 * stepSize;
					memoryEntryFrequencyRange.FilterBandwidth = this._controlInterface.FilterBandwidth;
					memoryEntryFrequencyRange.StepSize = stepSize;
					memoryEntryFrequencyRange.RangeDetectorType = this._controlInterface.DetectorType;
				}
				else
				{
					memoryEntryFrequencyRange = this._enteriesFrequencyRange[num3 - 1];
				}
				long startFrequency = memoryEntryFrequencyRange.StartFrequency;
				long endFrequency = memoryEntryFrequencyRange.EndFrequency;
				int num6 = memoryEntryFrequencyRange.FilterBandwidth;
				int stepSize2 = memoryEntryFrequencyRange.StepSize;
				if (num6 > stepSize2)
				{
					num6 = stepSize2;
				}
				int num7 = num / stepSize2 / 2 * 2;
				long num8 = startFrequency;
				int num9 = 0;
				long centerFrequency = 0L;
				for (; num8 <= endFrequency; num8 += stepSize2)
				{
					if (num9 > num7 || num9 == 0)
					{
						centerFrequency = (((endFrequency - num8) / stepSize2 > num7) ? (num8 + num7 / 2 * stepSize2) : ((num8 + endFrequency) / 2));
						CenterFrequencyEntry centerFrequencyEntry = new CenterFrequencyEntry();
						centerFrequencyEntry.CenterFrequency = centerFrequency;
						centerFrequencyEntry.StartFrequencyIndex = num2;
						this._centerFrequncyList.Add(centerFrequencyEntry);
						num9 = 0;
					}
					int iFOffset = this._controlInterface.get_IFOffset();
					ChannelAnalizerMemoryEntry channelAnalizerMemoryEntry = new ChannelAnalizerMemoryEntry();
					channelAnalizerMemoryEntry.Frequency = num8;
					channelAnalizerMemoryEntry.CenterFrequency = centerFrequency;
					channelAnalizerMemoryEntry.FilterBandwidth = memoryEntryFrequencyRange.FilterBandwidth;
					channelAnalizerMemoryEntry.StepSize = memoryEntryFrequencyRange.StepSize;
					channelAnalizerMemoryEntry.DetectorType = memoryEntryFrequencyRange.RangeDetectorType;
					channelAnalizerMemoryEntry.StartBins = this.FrequencyToBins(num8 - num6 / 2 - iFOffset, centerFrequency, rFBandwidth);
					channelAnalizerMemoryEntry.EndBins = this.FrequencyToBins(num8 + num6 / 2 - iFOffset, centerFrequency, rFBandwidth);
					this._channelList.Add(channelAnalizerMemoryEntry);
					num2++;
					num9++;
				}
			}
			for (int j = 0; j < this._centerFrequncyList.Count - 1; j++)
			{
				this._centerFrequncyList[j].EndFrequencyIndex = this._centerFrequncyList[j + 1].StartFrequencyIndex - 1;
			}
			this._centerFrequncyList[this._centerFrequncyList.Count - 1].EndFrequencyIndex = this._channelList.Count - 1;
			for (int k = 0; k < this._channelList.Count; k++)
			{
				long frequency = this._channelList[k].Frequency;
				int filterBandwidth = this._channelList[k].FilterBandwidth;
				if (this._skipFrequencyList != null)
				{
					int num10 = 0;
					while (num10 < this._skipFrequencyList.Count)
					{
						if (this._skipFrequencyList[num10] != frequency)
						{
							num10++;
							continue;
						}
						this._channelList[k].Skipped = true;
						break;
					}
				}
				if (this._entriesInManager != null)
				{
					foreach (MemoryEntry item in this._entriesInManager)
					{
						if (item.Frequency == frequency)
						{
							this._channelList[k].IsStore = true;
							this._channelList[k].StoreName = item.GroupName + " " + item.Name;
							break;
						}
					}
				}
			}
			this._filterWidth = (this._channelList[0].EndBins - this._channelList[0].StartBins) * 2;
			this._filterWidth |= 1;
		}

		private bool CheckConditions()
		{
			if (this.frequencyRangeListBox.SelectedIndices.Count == 0)
			{
				this.frequencyRangeListBox.SelectedIndex = 0;
			}
			if (this.frequencyRangeListBox.SelectedIndices.Count > 1 && this.frequencyRangeListBox.SelectedIndices.IndexOf(0) != -1)
			{
				this.frequencyRangeListBox.SelectedIndices.Remove(this.frequencyRangeListBox.SelectedIndices.IndexOf(0));
			}
			if (!this._controlInterface.get_SourceIsTunable() && this.frequencyRangeListBox.SelectedIndex != 0)
			{
				if (CultureInfo.CurrentCulture.Name == "ru-RU")
				{
					MessageBox.Show("Невозможно управлять настройкой частоты источника сигнала. Доступно только сканирование в пределах экрана.", "Scan conditions Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					MessageBox.Show("Unable to control the frequency settings for the source. Available only scan within the screen.", "Scan conditions Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}
			int num = 0;
			while (num < this.frequencyRangeListBox.SelectedIndices.Count)
			{
				int num2 = this.frequencyRangeListBox.SelectedIndices[num];
				int num3 = 0;
				int num4 = 0;
				if (num2 == 0)
				{
					num3 = this._controlInterface.get_StepSize();
					num4 = this._controlInterface.FilterBandwidth;
				}
				else
				{
					num3 = this._enteriesFrequencyRange[num2 - 1].StepSize;
					num4 = this._enteriesFrequencyRange[num2 - 1].FilterBandwidth;
				}
				if (num3 != 0 && num4 != 0)
				{
					num++;
					continue;
				}
				if (CultureInfo.CurrentCulture.Name == "ru-RU")
				{
					MessageBox.Show("Параметр Step size или Filter bandwidth равны нулю! Для работы сканера параметр Step Size и Filter Bandwidth не равны нулю.", "Scan conditions Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					MessageBox.Show("Step size or Filter bandwidth equal to zero! For the scanner must be Step Size and Filter Bandwidth not equal to zero.", "Scan conditions Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}
			return true;
		}

		private void ScanProcess()
		{
			while (this._scannerRunning)
			{
				this._bufferEvent.WaitOne();
				if (!this._scannerRunning)
				{
					break;
				}
				this.GetControlParameters();
				this._directionReverse = this._direction;
				this._interval += this._timeConstant;
				switch (this._state)
				{
				case ScanState.SetScreen:
				{
					int currentScreenIndex = this._currentScreenIndex;
					this.SetCurrentScreen();
					if (this._currentScreenIndex != currentScreenIndex)
					{
						this._state = ScanState.PauseToSet;
						this._interval = 0f;
						goto case ScanState.PauseToSet;
					}
					this._state = ScanState.ScanScreen;
					this._interval = 0f;
					goto case ScanState.ScanScreen;
				}
				case ScanState.PauseToSet:
					if (!this._centerFrequencyIsChanged)
					{
						this._interval = 0f;
					}
					if (!(this._interval >= (float)this._detect))
					{
						break;
					}
					this._centerFrequencyIsChanged = false;
					this._state = ScanState.ScanScreen;
					this._interval = 0f;
					goto case ScanState.ScanScreen;
				case ScanState.ScanScreen:
				{
					this.ProcessFFT();
					int num = 1;
					if (!this._directionReverse)
					{
						num = 1;
						this._currentStartIndex = this._centerFrequncyList[this._currentScreenIndex].StartFrequencyIndex;
						this._currentEndIndex = this._centerFrequncyList[this._currentScreenIndex].EndFrequencyIndex;
					}
					else
					{
						num = -1;
						this._currentEndIndex = this._centerFrequncyList[this._currentScreenIndex].StartFrequencyIndex;
						this._currentStartIndex = this._centerFrequncyList[this._currentScreenIndex].EndFrequencyIndex;
					}
					for (int i = this._currentStartIndex; i != this._currentEndIndex + num; i += num)
					{
						this._channelList[i].Level = this.MaxLevelOnChannel(this._channelList[i].StartBins, this._channelList[i].EndBins);
					}
					bool flag = true;
					this._activeFrequencies.Clear();
					for (int j = this._currentStartIndex; j != this._currentEndIndex + num; j += num)
					{
						int num2;
						if (this._channelList[j].Skipped)
						{
							if (this._controlParameters.Frequency == this._channelList[j].Frequency && !this._pauseScan)
							{
								num2 = j + num;
								if (!this._directionReverse && num2 > this._currentEndIndex)
								{
									goto IL_0242;
								}
								if (this._directionReverse && num2 < this._currentEndIndex)
								{
									goto IL_0242;
								}
								goto IL_024a;
							}
						}
						else if ((!this._scanMemory || this._channelList[j].IsStore) && (!this._scanExceptMemory || !this._channelList[j].IsStore) && this._channelList[j].Level > (float)this._scanLevel)
						{
							flag = false;
							if (this._scanWidthStoreNew && !this._channelList[j].IsStore)
							{
								this._newActiveFrequencies.Add(this._channelList[j].Frequency);
							}
							this._activeFrequencies.Add(j);
						}
						continue;
						IL_0242:
						num2 = this._currentStartIndex;
						goto IL_024a;
						IL_024a:
						this._controlParameters.Frequency = this._channelList[num2].Frequency;
						this._isPlayed = false;
						if (this.UseMute)
						{
							this.AudioMuteProcess(false);
						}
					}
					List<int>.Enumerator enumerator;
					if (!this._isPlayed && this._activeFrequencies.Count > 0)
					{
						if (this._activeFrequencies.Count == 1 || !this.MaxLevelSelect)
						{
							this.ChangeFrequencyInScanner(this._channelList[this._activeFrequencies[0]], true);
						}
						else
						{
							float num3 = 0f;
							int index = 0;
							enumerator = this._activeFrequencies.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									int current = enumerator.Current;
									if (this._channelList[current].Level > num3)
									{
										num3 = this._channelList[current].Level;
										index = current;
									}
								}
							}
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
							this.ChangeFrequencyInScanner(this._channelList[index], true);
						}
						this._isPlayed = true;
					}
					if (this._nextButtonPress)
					{
						this._nextButtonPress = false;
						flag = true;
						this._isPlayed = false;
						if (this._activeFrequencies.Count > 0)
						{
							enumerator = this._activeFrequencies.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									int current2 = enumerator.Current;
									if (this._channelList[current2].Frequency > this._controlParameters.Frequency && !this._directionReverse)
									{
										goto IL_049d;
									}
									if (this._channelList[current2].Frequency < this._controlParameters.Frequency && this._directionReverse)
									{
										goto IL_049d;
									}
									continue;
									IL_049d:
									this.ChangeFrequencyInScanner(this._channelList[current2], true);
									this._isPlayed = true;
									break;
								}
							}
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						if (this.UseMute)
						{
							this.AudioMuteProcess(this._isPlayed);
						}
					}
					if (this._isPlayed)
					{
						flag = false;
						this._isActivePlaedFrequency = this.SquelchActiveFrequency(this._isActivePlaedFrequency);
						if (this._isActivePlaedFrequency)
						{
							this._pauseAfter = (float)this._pauseToNextScreen;
						}
						else
						{
							this._pauseAfter -= this._interval;
						}
						this._isPlayed = (this._pauseAfter > 0f || this._isActivePlaedFrequency);
						if (this.UseMute)
						{
							this.AudioMuteProcess(this._isActivePlaedFrequency);
						}
					}
					if (!this._isPlayed || this._pauseScan)
					{
						this.ResetTimers();
					}
					if (this._pauseScan)
					{
						this._isPlayed = true;
						flag = false;
						this._pauseAfter = 0f;
					}
					if (flag)
					{
						this._state = ScanState.SetScreen;
					}
					this._interval = 0f;
					break;
				}
				case ScanState.ScanStop:
					this._interval = 0f;
					break;
				}
				if (this._controlParameters.IsChanged)
				{
					this._needUpdateParametrs = true;
				}
				else
				{
					if (this._state == ScanState.ScanScreen || this._centerFrequncyList.Count == 1)
					{
						Thread.Sleep(20);
					}
					this._scanProcessIsWork = false;
				}
			}
		}

		private void GetControlParameters()
		{
			this._controlParameters.AudioIsMuted = this._controlInterface.get_AudioIsMuted();
			this._controlParameters.CenterFrequency = this._controlInterface.CenterFrequency;
			this._controlParameters.Frequency = this._controlInterface.Frequency;
			this._controlParameters.FilterBandwidth = this._controlInterface.FilterBandwidth;
			this._controlParameters.DetectorType = this._controlInterface.DetectorType;
			this._controlParameters.RfBandwidth = this._controlInterface.RFBandwidth;
			this._controlParameters.ResetFlags();
		}

		private unsafe float MaxLevelOnChannel(int startBins, int endBins)
		{
			float num = 0f;
			if (this.ChannelDetectMetod)
			{
				int num2 = (startBins + endBins) / 2;
				num = this._scaledFFTSpectrumPtr[num2];
			}
			else
			{
				for (int i = startBins; i <= endBins; i++)
				{
					int num3 = i;
					if (num3 < 0)
					{
						num3 = 0;
					}
					if (num3 >= this._fftBins)
					{
						num3 = this._fftBins - 1;
					}
					num = Math.Max(num, this._scaledFFTSpectrumPtr[num3]);
				}
			}
			return num;
		}

		private void ChangeFrequencyInScanner(ChannelAnalizerMemoryEntry entry, bool outToPanView)
		{
			this._controlParameters.DetectorType = entry.DetectorType;
			this._controlParameters.FilterBandwidth = entry.FilterBandwidth;
			this._controlParameters.Frequency = entry.Frequency;
			if (outToPanView)
			{
				this._channelAnalyzer.Frequency = entry.Frequency;
			}
		}

		private bool SquelchActiveFrequency(bool useHisteresis)
		{
			int startBins = this.FrequencyToBins(this._controlParameters.Frequency - this._controlParameters.FilterBandwidth / 4 - this._controlInterface.get_IFOffset(), this._controlParameters.CenterFrequency, this._controlParameters.RfBandwidth);
			int endBins = this.FrequencyToBins(this._controlParameters.Frequency + this._controlParameters.FilterBandwidth / 4 - this._controlInterface.get_IFOffset(), this._controlParameters.CenterFrequency, this._controlParameters.RfBandwidth);
			if (useHisteresis)
			{
				return this.MaxLevelOnChannel(startBins, endBins) > (float)this._hysteresis;
			}
			return this.MaxLevelOnChannel(startBins, endBins) > (float)this._scanLevel;
		}

		private void AudioMuteProcess(bool unmute)
		{
			bool flag = this._controlParameters.AudioIsMuted;
			if (unmute)
			{
				this._squelchCount++;
				if (this._squelchCount > this.UnMuteDelay)
				{
					flag = false;
					this._squelchCount = this.UnMuteDelay;
				}
			}
			else
			{
				flag = true;
				this._squelchCount = 0;
			}
			if (this._controlParameters.AudioIsMuted != flag)
			{
				this._controlParameters.AudioIsMuted = flag;
			}
		}

		private void SetCurrentScreen()
		{
			if (this._currentScreenIndex == 0)
			{
				this._debugTime = this._debugCounter;
				this._debugCounter = 0;
			}
			if (this._centerFrequncyList.Count == 1 && this._controlParameters.CenterFrequency == this._centerFrequncyList[0].CenterFrequency)
			{
				this._currentScreenIndex = 0;
			}
			else
			{
				if (!this._directionReverse)
				{
					this._currentScreenIndex++;
					if (this._currentScreenIndex >= this._centerFrequncyList.Count)
					{
						this._currentScreenIndex = 0;
					}
				}
				else
				{
					this._currentScreenIndex--;
					if (this._currentScreenIndex < 0)
					{
						this._currentScreenIndex = this._centerFrequncyList.Count - 1;
					}
				}
				this._controlParameters.CenterFrequency = this._centerFrequncyList[this._currentScreenIndex].CenterFrequency;
			}
		}

		private int FrequencyToBins(long frequency, long centerFrequency, int rfBandwidth)
		{
			long num = centerFrequency - rfBandwidth / 2;
			float num2 = (float)rfBandwidth / (float)this._fftBins;
			return (int)((float)(frequency - num) / num2);
		}

		private long BinsToFrequency(int bins)
		{
			float num = (float)this._controlParameters.RfBandwidth / (float)this._fftBins;
			return this._controlParameters.CenterFrequency - this._controlParameters.RfBandwidth / 2 + (int)((float)bins * num);
		}

		private List<string> GetRangeNameFromEntries()
		{
			List<string> list = new List<string>();
			list.Add("Screen");
			foreach (MemoryEntryFrequencyRange item in this._enteriesFrequencyRange)
			{
				list.Add(item.RangeName);
			}
			return list;
		}

		private void ProcessRangeName()
		{
			this.frequencyRangeListBox.Items.Clear();
			this.frequencyRangeListBox.Items.AddRange(this.GetRangeNameFromEntries().ToArray());
		}

		private void FrequencyRangeEditButton_Click(object sender, EventArgs e)
		{
			DialogEditRange dialogEditRange = new DialogEditRange();
			this._enteriesFrequencyRange = dialogEditRange.EditRange(this._enteriesFrequencyRange);
			if (dialogEditRange.ShowDialog() == DialogResult.OK)
			{
				this.StoreEntry();
				this.ProcessRangeName();
			}
		}

		private unsafe void BuildFFTWindow()
		{
			float[] array = FilterBuilder.MakeWindow(WindowType.BlackmanHarris4, this._fftBins);
			fixed (float* src = array)
			{
				Utils.Memcpy(this._fftWindow, src, this._fftBins * 4);
			}
		}

		private unsafe void InitFFTBuffers()
		{
			this._fftBuffer = UnsafeBuffer.Create(this._fftBins, sizeof(Complex));
			this._fftWindow = UnsafeBuffer.Create(this._fftBins, 4);
			this._unScaledFFTSpectrum = UnsafeBuffer.Create(this._fftBins, 4);
			this._scaledFFTSpectrum = UnsafeBuffer.Create(this._fftBins, 4);
			this._fftPtr = (Complex*)(void*)this._fftBuffer;
			this._fftWindowPtr = (float*)(void*)this._fftWindow;
			this._unScaledFFTSpectrumPtr = (float*)(void*)this._unScaledFFTSpectrum;
			this._scaledFFTSpectrumPtr = (float*)(void*)this._scaledFFTSpectrum;
		}

		private unsafe void FastBufferAvailable(Complex* buffer, int length)
		{
			if (this._scannerRunning && this._controlInterface.IsPlaying)
			{
				this._count++;
				this._debugCounter++;
				if (!this._scanProcessIsWork)
				{
					int num = Math.Min(length, this._fftBins - this._writePos);
					Utils.Memcpy(this._fftPtr + this._writePos, buffer, num * sizeof(Complex));
					this._writePos += num;
					if (this._writePos >= this._fftBins)
					{
						this._writePos = 0;
						this._bufferLengthInMs = (double)length / this._ifProcessor.SampleRate * 1000.0;
						this._timeConstant = (float)(this._bufferLengthInMs * (double)this._count);
						this._scanProcessIsWork = true;
						this._count = 0;
						this._bufferEvent.Set();
					}
				}
				else
				{
					this._writePos = 0;
				}
			}
		}

		private unsafe void ProcessFFT()
		{
			Fourier.ApplyFFTWindow(this._fftPtr, this._fftWindowPtr, this._fftBins);
			Fourier.ForwardTransform(this._fftPtr, this._fftBins, true);
			Fourier.SpectrumPower(this._fftPtr, this._unScaledFFTSpectrumPtr, this._fftBins, 0f);
			this.ScaleSpectrum(this._unScaledFFTSpectrumPtr, this._scaledFFTSpectrumPtr, this._fftBins);
		}

		private unsafe void ScaleSpectrum(float* source, float* dest, int length)
		{
			bool swapIq = this._controlInterface.SwapIq;
			float num = 1f / (float)this._filterWidth;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < length; i++)
			{
				num3 = (swapIq ? (length - i) : i);
				num2 = i;
				this._iavg += num * (source[num2] - this._iavg);
				dest[num3] = source[num2] - this._iavg;
			}
		}

		private void iControlsUpdateTimer_Tick(object sender, EventArgs e)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (this._controlParameters != null && this._needUpdateParametrs)
			{
				this._needUpdateParametrs = false;
				if (this._controlParameters.IsChanged)
				{
					this._changeFrequencyInScanner = true;
					if ((int)this._controlInterface.get_TuningStyle() != 0)
					{
						this._controlInterface.set_TuningStyle(0);
					}
					if (this._controlParameters.AudioNeedUpdate && this._controlInterface.get_AudioIsMuted() != this._controlParameters.AudioIsMuted)
					{
						this._controlInterface.set_AudioIsMuted(this._controlParameters.AudioIsMuted);
					}
					if (this._controlParameters.DetectorNeedUpdate && this._controlInterface.DetectorType != this._controlParameters.DetectorType)
					{
						this._controlInterface.DetectorType = this._controlParameters.DetectorType;
					}
					if (this._controlParameters.FilterNeedUpdate && this._controlInterface.FilterBandwidth != this._controlParameters.FilterBandwidth)
					{
						this._controlInterface.FilterBandwidth = this._controlParameters.FilterBandwidth;
					}
					if (this._controlParameters.CenterNeedUpdate && this._controlInterface.CenterFrequency != this._controlParameters.CenterFrequency)
					{
						this._controlInterface.CenterFrequency = this._controlParameters.CenterFrequency;
					}
					if (this._controlParameters.FrequencyNeedUpdate && this._controlInterface.Frequency != this._controlParameters.Frequency)
					{
						this._controlInterface.Frequency = this._controlParameters.Frequency;
					}
					this._changeFrequencyInScanner = false;
					this._controlParameters.ResetFlags();
				}
				this._scanProcessIsWork = false;
			}
		}

		private void testDisplayTimer_Tick(object sender, EventArgs e)
		{
			this.timeConstantLabel.Text = $"{this._timeConstant:F} ms";
			if (this._requestToStopScanner)
			{
				this._requestToStopScanner = false;
				this.ScanStop();
			}
			string str = "Scan";
			str = ((!this._directionReverse) ? (str + " >>>") : (str + " <<<"));
			if (this._isPlayed)
			{
				str = ((this._pauseAfter != (float)this._pauseToNextScreen) ? $"Wait {this._pauseAfter / 1000f:F1} s" : "Play");
			}
			if (this._pauseScan)
			{
				str = "Pause";
			}
			if (this._channelAnalyzer.Zoom != 0 && this._scannerRunning)
			{
				str = $"Zoom x{this._channelAnalyzer.Zoom + 1:N0} " + str;
			}
			if (this.ShowDebugInfo && this._scannerRunning)
			{
				int count = this._centerFrequncyList.Count;
				float num = (float)this._controlInterface.RFBandwidth * this._usableSpectrum;
				float num2 = (float)(this._bufferLengthInMs * (double)this._debugTime * 0.0010000000474974513);
				float num3 = num * (float)count / num2 * 1E-06f;
				str = $"Info: screens {count:N0} x {num * 1E-06f:F2} MHz, time {num2:F3} s, speed {num3:F1} MHz/s. " + str;
			}
			this._infoText = str;
		}

		private void ResetTimers()
		{
			this._autoSkipTimer.Enabled = false;
			this._autoSkipTimer.Interval = (double)(this.AutoSkipInterval * 1000);
			this._autoSkipTimer.Enabled = this.AutoSkipEnabled;
			this._autoLockTimer.Enabled = false;
			this._autoLockTimer.Interval = (double)(this.AutoLockInterval * 1000);
			this._autoLockTimer.Enabled = this.AutoLockEnabled;
		}

		private void newFrequencyDisplayUpdateTimer_Tick(object sender, EventArgs e)
		{
			this.UpdateNewFrequencyDisplay();
		}

		private void AutoSkip_Tick(object source, ElapsedEventArgs e)
		{
			this._nextButtonPress = true;
		}

		private void AutoLock_Tick(object source, ElapsedEventArgs e)
		{
			this.SetSkipSelectedChannel(true);
		}

		private void autoCleanTimer_Tick(object sender, EventArgs e)
		{
			this._timeToClean = true;
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			if (this._channelList != null)
			{
				this._channelAnalyzer.Perform(this._channelList);
			}
		}

		private void _channelAnalyzer_MouseUp(object sender, MouseEventArgs e)
		{
			if (this._channelList != null)
			{
				this.buttonsRepeaterTimer.Enabled = false;
				this._buttons = Buttons.None;
				if (this._selectFrequency)
				{
					this._startSelectX = this._oldX;
					this._endSelectX = this._trackingX;
					this._startSelectIndex = this._oldIndex;
					this._endSelectIndex = this._trackingIndex;
					if (this._startSelectIndex > this._endSelectIndex)
					{
						int startSelectX = this._startSelectX;
						this._startSelectX = this._endSelectX;
						this._endSelectX = startSelectX;
						startSelectX = this._startSelectIndex;
						this._startSelectIndex = this._endSelectIndex;
						this._endSelectIndex = startSelectX;
					}
					if (this._startSelectX == this._endSelectX)
					{
						this._channelList[this._startSelectIndex].Skipped = !this._channelList[this._startSelectIndex].Skipped;
						this._selectFrequency = false;
					}
				}
			}
		}

		private void _channelAnalyzer_MouseDown(object sender, MouseEventArgs e)
		{
			Point location = e.Location;
			if (this._channelList != null)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (this._reverseRectangle.Contains(location))
					{
						this._nextButtonPress = true;
						this._direction = true;
					}
					else if (this._forwardRectangle.Contains(location))
					{
						this._nextButtonPress = true;
						this._direction = false;
					}
					else if (this._pauseRectangle.Contains(location))
					{
						this._pauseScan = !this._pauseScan;
					}
					else if (this._lockRectangle.Contains(location))
					{
						this.SetSkipSelectedChannel(true);
					}
					else if (this._unlockRectangle.Contains(location))
					{
						this.SetSkipSelectedChannel(false);
					}
					else if (this._snrPlusRectangle.Contains(location))
					{
						this._scanLevel++;
						this._buttons = Buttons.SNRUp;
						this.buttonsRepeaterTimer.Enabled = true;
					}
					else if (this._snrMinusRectangle.Contains(location))
					{
						if (this._scanLevel > this._hysteresis)
						{
							this._scanLevel--;
						}
						this._buttons = Buttons.SNRDown;
						this.buttonsRepeaterTimer.Enabled = true;
					}
					else if (this._histPlusRectangle.Contains(location))
					{
						if (this._hysteresis < this._scanLevel)
						{
							this._hysteresis++;
						}
						this._buttons = Buttons.HystUp;
						this.buttonsRepeaterTimer.Enabled = true;
					}
					else if (this._histMinusRectangle.Contains(location))
					{
						if (this._hysteresis > 0)
						{
							this._hysteresis--;
						}
						this._buttons = Buttons.HystDown;
						this.buttonsRepeaterTimer.Enabled = true;
					}
					else if (this._selectFrequency && this._trackingX > this._startSelectX && this._trackingX < this._endSelectX)
					{
						this.SetSkipSelectedChannel(true);
					}
					else
					{
						this._oldX = this._trackingX;
						this._oldIndex = this._trackingIndex;
						this._oldFrequency = this._trackingFrequency;
						this._selectFrequency = true;
						this._startSelectX = 0;
						this._endSelectX = 0;
					}
				}
				else if (e.Button == MouseButtons.Right && this._selectFrequency && this._trackingX > this._startSelectX && this._trackingX < this._endSelectX)
				{
					this.SetSkipSelectedChannel(false);
				}
			}
		}

		private void _channelAnalyzer_MouseMove(object sender, MouseEventArgs e)
		{
			if (this._channelList != null)
			{
				this._trackingX = e.X;
				this._trackingY = e.Y;
				this._trackingIndex = this._channelAnalyzer.PointToChannel((float)this._trackingX);
				this._trackingFrequency = this._channelList[this._trackingIndex].Frequency;
			}
		}

		private void _channelAnalyzer_MouseWheel(object sender, MouseEventArgs e)
		{
			if (this._channelAnalyzer.Zoom > 0 && e.Delta < 0)
			{
				this._channelAnalyzer.Zoom--;
				if (this._channelAnalyzer.Zoom == 0)
				{
					this._channelAnalyzer.ZoomPosition = 0;
				}
			}
			if (this._channelAnalyzer.Zoom < 100 && e.Delta > 0)
			{
				if (this._channelAnalyzer.Zoom == 0)
				{
					this._channelAnalyzer.ZoomPosition = this._trackingX;
				}
				this._channelAnalyzer.Zoom++;
			}
		}

		private void _channelAnalyzer_MouseLeave(object sender, EventArgs e)
		{
			this._hotTrackNeeded = false;
		}

		private void _channelAnalyzer_MouseEnter(object sender, EventArgs e)
		{
			this._hotTrackNeeded = true;
		}

		public void SetSkipSelectedChannel(bool setUnSet)
		{
			if (this._selectFrequency)
			{
				if (this._startSelectX < this._endSelectX)
				{
					for (int i = this._startSelectIndex; i <= this._endSelectIndex; i++)
					{
						if (i < this._channelList.Count && i >= 0)
						{
							this._channelList[i].Skipped = setUnSet;
						}
					}
					this._selectFrequency = false;
				}
			}
			else
			{
				int num = this._channelAnalyzer.CurrentChannelIndex();
				if (num < this._channelList.Count && num >= 0)
				{
					this._channelList[num].Skipped = setUnSet;
				}
			}
		}

		private void _channelAnalyzer_CustomPaint(object sender, CustomPaintEventArgs e)
		{
			ChannelAnalyzer channelAnalyzer = (ChannelAnalyzer)sender;
			Graphics graphics = e.Graphics;
			if (this._hotTrackNeeded)
			{
				if (this._bright < this.MaxButtonsAlpha)
				{
					this._bright += 10;
				}
				else
				{
					this._bright = this.MaxButtonsAlpha;
				}
			}
			else if (this._bright > this.MinButtonsAlpha)
			{
				this._bright--;
			}
			else
			{
				this._bright = this.MinButtonsAlpha;
			}
			using (SolidBrush brush7 = new SolidBrush(Color.FromArgb(80, Color.DarkGray)))
			{
				using (Pen pen = new Pen(Color.Red))
				{
					using (Pen pen2 = new Pen(Color.Yellow))
					{
						using (Pen pen3 = new Pen(Color.Green))
						{
							using (Font font3 = new Font("Lucida Console", 9f))
							{
								using (Font font = new Font("Webdings", 32f))
								{
									using (Font font2 = new Font("Lucida Console", 8f))
									{
										using (new Font("Lucida Console", 32f))
										{
											using (SolidBrush brush5 = new SolidBrush(Color.FromArgb(150, Color.Black)))
											{
												using (SolidBrush brush6 = new SolidBrush(Color.FromArgb(255, Color.White)))
												{
													using (SolidBrush brush = new SolidBrush(Color.FromArgb(this._bright, Color.White)))
													{
														using (SolidBrush brush3 = new SolidBrush(Color.FromArgb(this._bright, Color.Red)))
														{
															using (SolidBrush brush4 = new SolidBrush(Color.FromArgb(this._bright, Color.Yellow)))
															{
																using (SolidBrush brush2 = new SolidBrush(Color.FromArgb(this._bright, Color.Black)))
																{
																	using (StringFormat format = new StringFormat(StringFormat.GenericTypographic))
																	{
																		using (new Pen(Color.Black))
																		{
																			Point p = default(Point);
																			SizeF sizeF = default(SizeF);
																			string empty = string.Empty;
																			empty = "7";
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			p.X = 37;
																			p.Y = 7;
																			this._reverseRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = "8";
																			p.X = p.X + (int)sizeF.Width + 10;
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._forwardRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = (this._pauseScan ? "4" : ";");
																			p.X = p.X + (int)sizeF.Width + 10;
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._pauseRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = "r";
																			p.X = p.X + (int)sizeF.Width + 10;
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._lockRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = "q";
																			p.X = p.X + (int)sizeF.Width + 10;
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._unlockRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = "5";
																			p.X = 37;
																			p.Y = 62;
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._snrPlusRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush3, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = "6";
																			p.Y = (int)((float)p.Y + sizeF.Height + 10f);
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._snrMinusRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush3, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = "5";
																			p.X = channelAnalyzer.Width - 35 - (int)sizeF.Width;
																			p.Y = 62;
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._histPlusRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush4, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = "6";
																			p.Y = (int)((float)p.Y + sizeF.Height + 10f);
																			sizeF = graphics.MeasureString(empty, font, channelAnalyzer.Width, format);
																			this._histMinusRectangle = new Rectangle(p.X, p.Y, (int)sizeF.Width, (int)sizeF.Height);
																			graphics.FillRectangle(brush4, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font, brush2, p, format);
																			empty = this._infoText;
																			sizeF = graphics.MeasureString(empty, font2, channelAnalyzer.Width, format);
																			p.Y = 7;
																			p.X = channelAnalyzer.Width - 2 - 5 - (int)sizeF.Width;
																			graphics.FillRectangle(brush5, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 4f);
																			graphics.DrawString(empty, font2, brush6, p, format);
																			int num = Math.Max(2, channelAnalyzer.Height - 2 - this._scanLevel * 2);
																			graphics.DrawLine(pen, 2, num, channelAnalyzer.Width - 2, num);
																			int num2 = Math.Max(2, channelAnalyzer.Height - 2 - this._hysteresis * 2);
																			graphics.DrawLine(pen2, 2, num2, channelAnalyzer.Width - 2, num2);
																			if (this._selectFrequency)
																			{
																				int num3 = this._oldX;
																				int num4 = this._trackingX;
																				if (this._startSelectX != 0 && this._endSelectX != 0)
																				{
																					num3 = this._startSelectX;
																					num4 = this._endSelectX;
																				}
																				if (num3 > num4)
																				{
																					int num5 = num3;
																					num3 = num4;
																					num4 = num5;
																				}
																				graphics.FillRectangle(brush7, num3, 2, num4 - num3, channelAnalyzer.Height - 4);
																			}
																			if (this._hotTrackNeeded && this._trackingX >= 2 && this._trackingX <= channelAnalyzer.Width - 2 && this._trackingY >= 2 && this._trackingY <= channelAnalyzer.Height - 2)
																			{
																				graphics.DrawLine(pen3, this._trackingX, 0, this._trackingX, channelAnalyzer.Height);
																				string text;
																				if (this._selectFrequency && this._oldFrequency != this._trackingFrequency)
																				{
																					text = $"{FrequencyScannerPanel.GetFrequencyDisplay(this._oldFrequency)}-{FrequencyScannerPanel.GetFrequencyDisplay(this._trackingFrequency)}";
																				}
																				else
																				{
																					text = $"{FrequencyScannerPanel.GetFrequencyDisplay(this._trackingFrequency)}";
																					if (this._channelList[this._trackingIndex].IsStore)
																					{
																						text = text + " " + this._channelList[this._trackingIndex].StoreName;
																					}
																				}
																				sizeF = graphics.MeasureString(text, font3, channelAnalyzer.Width, format);
																				p.X = this._trackingX + 5;
																				if ((float)p.X + sizeF.Width + 2f > (float)channelAnalyzer.Width)
																				{
																					p.X = (int)((float)(this._trackingX - 5) - sizeF.Width);
																				}
																				p.Y = 52;
																				graphics.FillRectangle(brush5, (float)(p.X - 1), (float)(p.Y - 2), sizeF.Width + 2f, sizeF.Height + 2f);
																				graphics.DrawString(text, font3, brush6, p, format);
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private void buttonsRepeaterTimer_Tick(object sender, EventArgs e)
		{
			switch (this._buttons)
			{
			case Buttons.None:
				break;
			case Buttons.SNRUp:
				if (this._scanLevel < 120)
				{
					this._scanLevel++;
				}
				break;
			case Buttons.SNRDown:
				if (this._scanLevel > this._hysteresis)
				{
					this._scanLevel--;
				}
				break;
			case Buttons.HystUp:
				if (this._hysteresis < this._scanLevel)
				{
					this._hysteresis++;
				}
				break;
			case Buttons.HystDown:
				if (this._hysteresis > 0)
				{
					this._hysteresis--;
				}
				break;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrequencyScannerPanel));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.mainToolStrip = new ToolStrip();
			this.btnNewEntry = new ToolStripButton();
			this.btnDelete = new ToolStripButton();
			this.frequencyDataGridView = new DataGridView();
			this.Activity = new DataGridViewTextBoxColumn();
			this.scanModeComboBox = new ComboBox();
			this.ScanButton = new Button();
			this.FrequencyRangeEditButton = new Button();
			this.newFrequencyDisplayUpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.configureButton = new Button();
			this.autoCleanTimer = new System.Windows.Forms.Timer(this.components);
			this.timeConstantLabel = new Label();
			this.testDisplayTimer = new System.Windows.Forms.Timer(this.components);
			this.frequencyRangeListBox = new ListBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.iControlsUpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.label1 = new Label();
			this.label2 = new Label();
			this.detectNumericUpDown = new NumericUpDown();
			this.waitNumericUpDown = new NumericUpDown();
			this.buttonsRepeaterTimer = new System.Windows.Forms.Timer(this.components);
			this.frequencyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			this.memoryEntryBindingSource = new BindingSource(this.components);
			this.mainToolStrip.SuspendLayout();
			((ISupportInitialize)this.frequencyDataGridView).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.detectNumericUpDown).BeginInit();
			((ISupportInitialize)this.waitNumericUpDown).BeginInit();
			((ISupportInitialize)this.memoryEntryBindingSource).BeginInit();
			base.SuspendLayout();
			this.mainToolStrip.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.mainToolStrip.AutoSize = false;
			this.mainToolStrip.Dock = DockStyle.None;
			this.mainToolStrip.GripStyle = ToolStripGripStyle.Hidden;
			this.mainToolStrip.Items.AddRange(new ToolStripItem[2]
			{
				this.btnNewEntry,
				this.btnDelete
			});
			this.mainToolStrip.Location = new Point(0, 204);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new Size(217, 23);
			this.mainToolStrip.TabIndex = 7;
			this.btnNewEntry.Image = (Image)componentResourceManager.GetObject("btnNewEntry.Image");
			this.btnNewEntry.ImageTransparentColor = Color.Magenta;
			this.btnNewEntry.Name = "btnNewEntry";
			this.btnNewEntry.Size = new Size(54, 20);
			this.btnNewEntry.Text = "Clear";
			this.btnNewEntry.ToolTipText = "Clear";
			this.btnNewEntry.Click += this.btnClearEntry_Click;
			this.btnDelete.Image = (Image)componentResourceManager.GetObject("btnDelete.Image");
			this.btnDelete.ImageTransparentColor = Color.Magenta;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new Size(60, 20);
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += this.btnDelete_Click;
			this.frequencyDataGridView.AllowUserToAddRows = false;
			this.frequencyDataGridView.AllowUserToDeleteRows = false;
			this.frequencyDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle.BackColor = Color.WhiteSmoke;
			this.frequencyDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.frequencyDataGridView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.frequencyDataGridView.AutoGenerateColumns = false;
			this.frequencyDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.frequencyDataGridView.Columns.AddRange(this.Activity, this.frequencyDataGridViewTextBoxColumn);
			this.frequencyDataGridView.DataSource = this.memoryEntryBindingSource;
			this.frequencyDataGridView.Location = new Point(0, 229);
			this.frequencyDataGridView.Margin = new Padding(2);
			this.frequencyDataGridView.Name = "frequencyDataGridView";
			this.frequencyDataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.frequencyDataGridView.RowHeadersVisible = false;
			this.frequencyDataGridView.RowTemplate.Height = 24;
			this.frequencyDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.frequencyDataGridView.ShowCellErrors = false;
			this.frequencyDataGridView.ShowCellToolTips = false;
			this.frequencyDataGridView.ShowEditingIcon = false;
			this.frequencyDataGridView.ShowRowErrors = false;
			this.frequencyDataGridView.Size = new Size(215, 186);
			this.frequencyDataGridView.TabIndex = 6;
			this.frequencyDataGridView.CellDoubleClick += this.frequencyDataGridView_CellDoubleClick;
			this.frequencyDataGridView.CellFormatting += this.frequencyDataGridView_CellFormatting;
			this.frequencyDataGridView.SelectionChanged += this.frequencyDataGridView_SelectionChanged;
			this.frequencyDataGridView.KeyDown += this.frequencyDataGridView_KeyDown;
			this.Activity.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.Activity.DataPropertyName = "Activity";
			dataGridViewCellStyle2.Format = "N2";
			dataGridViewCellStyle2.NullValue = null;
			this.Activity.DefaultCellStyle = dataGridViewCellStyle2;
			this.Activity.HeaderText = "Activity time s.";
			this.Activity.Name = "Activity";
			this.Activity.ReadOnly = true;
			this.scanModeComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.scanModeComboBox.AutoCompleteCustomSource.AddRange(new string[2]
			{
				"screen with playback",
				"screen without playback"
			});
			this.scanModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.scanModeComboBox.FormattingEnabled = true;
			this.scanModeComboBox.IntegralHeight = false;
			this.scanModeComboBox.Items.AddRange(new object[4]
			{
				"Scan all with save new",
				"Scan all without save new",
				"Scan only memorized except new",
				"Scan only new except memorized"
			});
			this.scanModeComboBox.Location = new Point(3, 4);
			this.scanModeComboBox.Margin = new Padding(2);
			this.scanModeComboBox.Name = "scanModeComboBox";
			this.scanModeComboBox.Size = new Size(211, 21);
			this.scanModeComboBox.TabIndex = 4;
			this.scanModeComboBox.SelectedIndexChanged += this.scanModeComboBox_SelectedIndexChanged;
			this.ScanButton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.ScanButton.Cursor = Cursors.Default;
			this.ScanButton.FlatAppearance.BorderSize = 0;
			this.ScanButton.ImageAlign = ContentAlignment.TopLeft;
			this.ScanButton.Location = new Point(89, 3);
			this.ScanButton.Name = "ScanButton";
			this.ScanButton.Size = new Size(125, 21);
			this.ScanButton.TabIndex = 22;
			this.ScanButton.Text = "Scan";
			this.ScanButton.UseCompatibleTextRendering = true;
			this.ScanButton.UseMnemonic = false;
			this.ScanButton.UseVisualStyleBackColor = false;
			this.ScanButton.Click += this.ScanButton_Click;
			this.FrequencyRangeEditButton.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.FrequencyRangeEditButton.Location = new Point(3, 119);
			this.FrequencyRangeEditButton.Name = "FrequencyRangeEditButton";
			this.FrequencyRangeEditButton.Size = new Size(211, 23);
			this.FrequencyRangeEditButton.TabIndex = 32;
			this.FrequencyRangeEditButton.Text = "Edit scan ranges";
			this.FrequencyRangeEditButton.UseVisualStyleBackColor = true;
			this.FrequencyRangeEditButton.Click += this.FrequencyRangeEditButton_Click;
			this.newFrequencyDisplayUpdateTimer.Interval = 500;
			this.newFrequencyDisplayUpdateTimer.Tick += this.newFrequencyDisplayUpdateTimer_Tick;
			this.configureButton.Cursor = Cursors.Default;
			this.configureButton.FlatAppearance.BorderSize = 0;
			this.configureButton.ImageAlign = ContentAlignment.TopLeft;
			this.configureButton.Location = new Point(3, 3);
			this.configureButton.Name = "configureButton";
			this.configureButton.Size = new Size(78, 21);
			this.configureButton.TabIndex = 22;
			this.configureButton.Text = "Configure";
			this.configureButton.UseCompatibleTextRendering = true;
			this.configureButton.UseMnemonic = false;
			this.configureButton.UseVisualStyleBackColor = false;
			this.configureButton.Click += this.configureButton_Click;
			this.autoCleanTimer.Interval = 100000;
			this.autoCleanTimer.Tick += this.autoCleanTimer_Tick;
			this.timeConstantLabel.AutoSize = true;
			this.timeConstantLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.timeConstantLabel.Location = new Point(167, 211);
			this.timeConstantLabel.Name = "timeConstantLabel";
			this.timeConstantLabel.Size = new Size(35, 13);
			this.timeConstantLabel.TabIndex = 36;
			this.timeConstantLabel.Text = "00 ms";
			this.timeConstantLabel.TextAlign = ContentAlignment.MiddleRight;
			this.testDisplayTimer.Enabled = true;
			this.testDisplayTimer.Tick += this.testDisplayTimer_Tick;
			this.frequencyRangeListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.frequencyRangeListBox.BackColor = SystemColors.Menu;
			this.frequencyRangeListBox.FormattingEnabled = true;
			this.frequencyRangeListBox.Location = new Point(3, 30);
			this.frequencyRangeListBox.Name = "frequencyRangeListBox";
			this.frequencyRangeListBox.SelectionMode = SelectionMode.MultiExtended;
			this.frequencyRangeListBox.Size = new Size(211, 82);
			this.frequencyRangeListBox.TabIndex = 37;
			this.frequencyRangeListBox.Tag = "";
			this.tableLayoutPanel1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.ScanButton, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.configureButton, 0, 0);
			this.tableLayoutPanel1.Location = new Point(0, 141);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
			this.tableLayoutPanel1.Size = new Size(217, 28);
			this.tableLayoutPanel1.TabIndex = 38;
			this.iControlsUpdateTimer.Interval = 1;
			this.iControlsUpdateTimer.Tick += this.iControlsUpdateTimer_Tick;
			this.refreshTimer.Enabled = true;
			this.refreshTimer.Interval = 50;
			this.refreshTimer.Tick += this.refreshTimer_Tick;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(5, 177);
			this.label1.Name = "label1";
			this.label1.Size = new Size(39, 13);
			this.label1.TabIndex = 39;
			this.label1.Text = "Detect";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(112, 177);
			this.label2.Name = "label2";
			this.label2.Size = new Size(29, 13);
			this.label2.TabIndex = 40;
			this.label2.Text = "Wait";
			this.detectNumericUpDown.Location = new Point(50, 175);
			this.detectNumericUpDown.Maximum = new decimal(new int[4]
			{
				1000,
				0,
				0,
				0
			});
			this.detectNumericUpDown.Name = "detectNumericUpDown";
			this.detectNumericUpDown.Size = new Size(48, 20);
			this.detectNumericUpDown.TabIndex = 41;
			this.detectNumericUpDown.ValueChanged += this.detectNumericUpDown_ValueChanged;
			this.waitNumericUpDown.DecimalPlaces = 1;
			this.waitNumericUpDown.Location = new Point(147, 175);
			this.waitNumericUpDown.Maximum = new decimal(new int[4]
			{
				10000,
				0,
				0,
				0
			});
			this.waitNumericUpDown.Name = "waitNumericUpDown";
			this.waitNumericUpDown.Size = new Size(55, 20);
			this.waitNumericUpDown.TabIndex = 42;
			this.waitNumericUpDown.ValueChanged += this.waitNumericUpDown_ValueChanged;
			this.buttonsRepeaterTimer.Tick += this.buttonsRepeaterTimer_Tick;
			this.frequencyDataGridViewTextBoxColumn.DataPropertyName = "Frequency";
			this.frequencyDataGridViewTextBoxColumn.HeaderText = "Frequency";
			this.frequencyDataGridViewTextBoxColumn.Name = "frequencyDataGridViewTextBoxColumn";
			this.frequencyDataGridViewTextBoxColumn.ReadOnly = true;
			this.memoryEntryBindingSource.DataSource = typeof(MemoryEntry);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.Controls.Add(this.waitNumericUpDown);
			base.Controls.Add(this.detectNumericUpDown);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.frequencyRangeListBox);
			base.Controls.Add(this.timeConstantLabel);
			base.Controls.Add(this.FrequencyRangeEditButton);
			base.Controls.Add(this.scanModeComboBox);
			base.Controls.Add(this.mainToolStrip);
			base.Controls.Add(this.frequencyDataGridView);
			base.Margin = new Padding(2);
			base.Name = "FrequencyScannerPanel";
			base.Size = new Size(217, 417);
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			((ISupportInitialize)this.frequencyDataGridView).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			((ISupportInitialize)this.detectNumericUpDown).EndInit();
			((ISupportInitialize)this.waitNumericUpDown).EndInit();
			((ISupportInitialize)this.memoryEntryBindingSource).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
