using SDRSharp.Common;
using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace SDRSharp.Trunker
{
	[Category("SDRSharp")]
	[Description("Trunker Control Panel")]
	[DesignTimeVisible(true)]
	public class TrunkerPanel : UserControl
	{
		private const int MUTE = 15;

		private readonly SettingsPersisterTrunker _settingsPersister;

		private List<TrunkerSettings> _trunkerConfig;

		private TrunkerLogger _trunkLoggerData;

		private ISharpControl _controlInterface;

		private TrunkerPlugin _plugin;

		private FileSystemWatcher _fileWatcher;

		private readonly object _fftResolutionLockObject = new object();

		private string _trunkingFileName = "sdrsharptrunking.log";

		private byte[] _fftSpectrum;

		private int _cachedAudioGain = 15;

		private System.Timers.Timer _retuneDelay;

		private int _beepDelay;

		private bool unitrunkerConfigured;

		private bool loggingConfigured;

		private bool loggingEnabled;

		private bool loggingSystemMessageDisplayed;

		private bool unitrunkerConfigureMessageDisplayed;

		private bool initializing = true;

		private string _version;

		private bool unitrunkerIsParked;

		private bool _sdrsharp_rev_1133;

		private IContainer components;

		private NumericUpDown trunkingControlChannelFreq;

		private Label lblTrunkingLogFile;

		private Label lbltrunkingControlChannelFreq;

		private CheckBox trunkingTuneToControlChannel;

		private CheckBox trunkingEnabledCheckbox;

		private CheckBox stickyTuneTrunkingCheckbox;

		private TextBox uniTrunkerHomeDirectory;

		private Button trunkingUnitrunkerBrowseButton;

		private Panel mainPanel;

		private System.Windows.Forms.Timer trunkingTimer;

		private TextBox singleLogFilePath;

		private Button trunkingLogFileButton;

		private Label label1;

		private CheckBox singleLogFileEnabled;

		private Label versionAndStatus;

		private Label label2;

		private Button spawnLogDialog;

		private ComboBox retuneDelayMethod;

		private Label label3;

		private NumericUpDown retuneMinSignalLevel;

		private Label retuneMinSignalLabel;

		private Panel panelForOptionsAfterRetune;

		private CheckBox muteEdacsBeep;

		private CheckBox muteWhileParkedCheckbox;

		private Label currentDbLabel;

		private Label currentDbValue;

		public TrunkerPlugin PlugIn
		{
			get
			{
				return this._plugin;
			}
			set
			{
				this._plugin = value;
			}
		}

		private bool isRev1133orBetter
		{
			get
			{
				if (this._controlInterface.GetType().GetProperty("SourceIsTunable") != null)
				{
					return true;
				}
				return false;
			}
		}

		private bool isRev1134orBetter
		{
			get
			{
				if (this._controlInterface.GetType().GetProperty("FFTRange") != null && this._controlInterface.GetType().GetProperty("FFTOffset") != null)
				{
					return true;
				}
				return false;
			}
		}

		private bool isSourceTunable => (bool)this._controlInterface.GetType().GetProperty("SourceIsTunable").GetValue(this._controlInterface, null);

		private float getFFTRange => (float)this._controlInterface.GetType().GetProperty("FFTRange").GetValue(this._controlInterface, null);

		private float getFFTOffset => (float)this._controlInterface.GetType().GetProperty("FFTOffset").GetValue(this._controlInterface, null);

		private bool shouldWeRetune
		{
			get
			{
				if (this.stickyTuneTrunkingCheckbox.Checked)
				{
					if (!this.unitrunkerIsParked)
					{
						return false;
					}
					if (this._trunkerConfig[0].delayRetuneMethod == 0 && this._controlInterface.SquelchEnabled)
					{
						return !this._controlInterface.IsSquelchOpen;
					}
					if (this._trunkerConfig[0].delayRetuneMethod == 1)
					{
						Monitor.Enter(this._fftResolutionLockObject);
						float peakSignal = this.getPeakSignal();
						Monitor.Exit(this._fftResolutionLockObject);
						this.currentDbValue.Text = peakSignal.ToString() + "dB";
						return peakSignal < (float)this.retuneMinSignalLevel.Value;
					}
				}
				else if (this.currentDbValue != null)
				{
					this.currentDbValue.Text = "Off";
				}
				return false;
			}
		}

		public TrunkerPanel(ISharpControl control, TrunkerPlugin _parentplug)
		{
			this.InitializeComponent();
			this._version = Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ZefieMod";
			this._plugin = _parentplug;
			this._controlInterface = control;
			this._cachedAudioGain = this._controlInterface.AudioGain;
			this._controlInterface.PropertyChanged += this._controlInterface_PropertyChanged;
			this._fftSpectrum = new byte[this._controlInterface.FFTResolution];
			this._settingsPersister = new SettingsPersisterTrunker();
			this._trunkerConfig = this._settingsPersister.readConfig();
			this._trunkLoggerData = new TrunkerLogger();
			this.setVersion();
			this._sdrsharp_rev_1133 = this.isRev1133orBetter;
			this.ProcessSavedSettings();
			this.initializing = false;
		}

		private void setVersion()
		{
			this.versionAndStatus.Text = "v" + this._version;
		}

		private void ProcessSavedSettings()
		{
			try
			{
				this.uniTrunkerHomeDirectory.Text = this._trunkerConfig[0].unitrunkerPath;
				this.singleLogFilePath.Text = this._trunkerConfig[0].logPath;
				this.muteEdacsBeep.Checked = this._trunkerConfig[0].isMuteEDACSEnabled;
				this.muteWhileParkedCheckbox.Checked = this._trunkerConfig[0].isMuteControlEnabled;
				this.trunkingControlChannelFreq.Value = (long)this._trunkerConfig[0].controlFreq;
				this.retuneDelayMethod.SelectedIndex = this._trunkerConfig[0].delayRetuneMethod;
				this.retuneMinSignalLevel.Value = this._trunkerConfig[0].delayRetuneMinimumSignal;
				this.stickyTuneTrunkingCheckbox.Checked = this._trunkerConfig[0].isDelayRetuneEnabled;
				if (this.singleLogFilePath.Text != null)
				{
					this.loggingConfigured = true;
					this.singleLogFileEnabled.Checked = this._trunkerConfig[0].isLoggingEnabled;
				}
				if (this.uniTrunkerHomeDirectory.Text != null)
				{
					this.unitrunkerConfigured = true;
					this.trunkingEnabledCheckbox.Checked = this._trunkerConfig[0].isEnabled;
				}
			}
			catch (Exception ex)
			{
				this.DoNothing(ex);
			}
		}

		private bool checkUnitrunkerPath(string path)
		{
			if (Directory.Exists(path))
			{
				if (File.Exists(path + "\\Unitrunker.xml"))
				{
					if (File.Exists(path + "\\Remote.dll"))
					{
						this.unitrunkerConfigured = true;
						return true;
					}
					MessageBox.Show("Warning: Remote.dll not found in UniTrunker configuration directory.  Copy Remote.dll from SDRSharp directory to directory that contains UniTrunker.xml (typically c:\\users\\[username]\\AppData\\Roaming\\UniTrunker).");
				}
				else
				{
					MessageBox.Show("Warning: UniTrunker.xml configuration file not found, ensure that the directory selected is the 'AppData' directory for unitrunker");
				}
			}
			return false;
		}

		private void WriteConfig()
		{
			if (!this.initializing)
			{
				try
				{
					this._trunkerConfig[0].settingsExist();
				}
				catch (Exception ex)
				{
					this.DoNothing(ex);
					this._trunkerConfig.Insert(0, new TrunkerSettings());
				}
				this._trunkerConfig[0].isEnabled = this.trunkingEnabledCheckbox.Checked;
				this._trunkerConfig[0].unitrunkerPath = this.uniTrunkerHomeDirectory.Text;
				this._trunkerConfig[0].logPath = this.singleLogFilePath.Text;
				this._trunkerConfig[0].isDelayRetuneEnabled = this.stickyTuneTrunkingCheckbox.Checked;
				this._trunkerConfig[0].isMuteEDACSEnabled = this.muteEdacsBeep.Checked;
				this._trunkerConfig[0].isMuteControlEnabled = this.muteWhileParkedCheckbox.Checked;
				this._trunkerConfig[0].isLoggingEnabled = this.singleLogFileEnabled.Checked;
				this._trunkerConfig[0].controlFreq = this.trunkingControlChannelFreq.Value;
				this._trunkerConfig[0].delayRetuneMethod = this.retuneDelayMethod.SelectedIndex;
				this._trunkerConfig[0].delayRetuneMinimumSignal = this.retuneMinSignalLevel.Value;
				this._settingsPersister.writeConfig(this._trunkerConfig);
			}
		}

		private void _controlInterface_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("AudioGain"))
			{
				this._cachedAudioGain = this._controlInterface.AudioGain;
			}
			else if (e.PropertyName.Equals("CenterFrequency"))
			{
				this.delayRetune();
			}
		}

		private bool isRetuningDelayed()
		{
			return this._retuneDelay != null;
		}

		private bool HasMethod(object objectToCheck, string methodName)
		{
			Type type = objectToCheck.GetType();
			return type.GetMethod(methodName) != null;
		}

		private float getPeakSignal()
		{
			lock (this._fftResolutionLockObject)
			{
				if (this._fftSpectrum.Length != this._controlInterface.FFTResolution)
				{
					this._fftSpectrum = new byte[this._controlInterface.FFTResolution];
				}
				this._controlInterface.GetSpectrumSnapshot(this._fftSpectrum);
				int filterBandwidth = this._controlInterface.FilterBandwidth;
				int rFBandwidth = this._controlInterface.RFBandwidth;
				long frequency = this._controlInterface.Frequency;
				long num = frequency - filterBandwidth / 2;
				long num2 = frequency + filterBandwidth / 2;
				float num3 = -120000f;
				float num4 = 0f;
				float num5 = (float)(rFBandwidth / this._fftSpectrum.Length);
				for (long num6 = num; num6 <= num2; num6 += (long)num5)
				{
					num4 = (float)(int)this._fftSpectrum[this.convertFrequencyToBin(num6)];
					if (num4 > num3)
					{
						num3 = num4;
					}
				}
				return (float)Math.Round((double)(0f - 0.509803951f * (255f - num3)), 1);
			}
		}

		private int convertFrequencyToBin(long frequency)
		{
			int result = 0;
			long centerFrequency = this._controlInterface.CenterFrequency;
			int rFBandwidth = this._controlInterface.RFBandwidth;
			long num = centerFrequency - rFBandwidth / 2;
			long num2 = centerFrequency + rFBandwidth / 2;
			if (frequency > num && frequency < num2)
			{
				float num3 = (float)(rFBandwidth / this._controlInterface.FFTResolution);
				result = (int)((float)(frequency - num) / num3);
			}
			return result;
		}

		private void doTrunkTuneTimerLoop()
		{
			if (this.stickyTuneTrunkingCheckbox.Checked && this.isRetuningDelayed() && this.shouldWeRetune)
			{
				return;
			}
			if (this.trunkingTuneToControlChannel.Checked && this.trunkingControlChannelFreq.Value > 0m)
			{
				if (this._sdrsharp_rev_1133)
				{
					if (this.isSourceTunable)
					{
						this._controlInterface.CenterFrequency = (long)this.trunkingControlChannelFreq.Value - 100000;
					}
				}
				else
				{
					try
					{
						this._controlInterface.CenterFrequency = (long)this.trunkingControlChannelFreq.Value - 100000;
					}
					catch (Exception ex)
					{
						this.DoNothing(ex);
					}
				}
				this._controlInterface.Frequency = (long)this.trunkingControlChannelFreq.Value;
				this._controlInterface.AudioGain = this._cachedAudioGain;
				this.delayRetune();
			}
		}

		private void WriteLog(string txtToWrite, string filePath)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(filePath))
				{
					streamWriter.Write(txtToWrite.ToString());
				}
			}
			catch (Exception ex)
			{
				this.DoNothing(ex);
			}
		}

		private void writeSingleLog()
		{
			if (this.singleLogFilePath.Text != null && this.loggingEnabled)
			{
				string text = LogParser.ParseLogStyle(this._trunkLoggerData, null, null, null, false);
				if (text != null)
				{
					this.versionAndStatus.Text = text;
					this.WriteLog(text, this.singleLogFilePath.Text);
				}
			}
		}

		private void PrepareLog()
		{
			this._trunkLoggerData = null;
			this._trunkLoggerData = new TrunkerLogger();
		}

		private void getLogData()
		{
			this.PrepareLog();
			try
			{
				using (StreamReader streamReader = new StreamReader(this.uniTrunkerHomeDirectory.Text + "\\" + this._trunkingFileName))
				{
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						try
						{
							string[] array = text.Split('\t');
							if (!"frequency".Equals(array[2]))
							{
								decimal currentFrequency = Convert.ToDecimal(array[2]);
								int num = array.Count();
								this._trunkLoggerData.currentFrequency = currentFrequency;
								if (num >= 1)
								{
									this._trunkLoggerData.currentAction = array[0];
								}
								if (num >= 2)
								{
									this._trunkLoggerData.currentReceiver = array[1];
								}
								if (num >= 4)
								{
									this._trunkLoggerData.currentTrunkgroup = array[3];
								}
								if (num >= 5)
								{
									this._trunkLoggerData.currentTrunklabel = array[4];
								}
								if (num >= 6)
								{
									this._trunkLoggerData.currentSourcegroup = array[5];
								}
								if (num >= 7)
								{
									this._trunkLoggerData.currentSourcelabel = array[6];
								}
								if ("Park".Equals(this._trunkLoggerData.currentAction))
								{
									this.unitrunkerIsParked = true;
								}
								else
								{
									this.unitrunkerIsParked = false;
								}
							}
						}
						catch (Exception ex)
						{
							this.DoNothing(ex);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				this.DoNothing(ex2);
			}
		}

		private void tuneToTrunkingFile()
		{
			if (!"Listen".Equals(this._trunkLoggerData.currentAction) && !"Park".Equals(this._trunkLoggerData.currentAction))
			{
				return;
			}
			if (this.singleLogFileEnabled.Checked)
			{
				this.writeSingleLog();
			}
			if (this._sdrsharp_rev_1133)
			{
				if (this.isSourceTunable)
				{
					this._controlInterface.CenterFrequency = (long)this._trunkLoggerData.currentFrequency - 100000;
				}
			}
			else
			{
				try
				{
					this._controlInterface.CenterFrequency = (long)this._trunkLoggerData.currentFrequency - 100000;
				}
				catch (Exception ex)
				{
					this.DoNothing(ex);
				}
			}
			this._controlInterface.Frequency = (long)this._trunkLoggerData.currentFrequency;
			this.delayRetune();
			if (this.muteWhileParkedCheckbox.Checked)
			{
				if ("Listen".Equals(this._trunkLoggerData.currentAction))
				{
					this._controlInterface.AudioGain = this._cachedAudioGain;
				}
				else if ("Park".Equals(this._trunkLoggerData.currentAction))
				{
					this._controlInterface.AudioGain = 15;
				}
			}
		}

		private void DoNothing(Exception ex)
		{
		}

		private void initTrunkingFileWatcher()
		{
			if (this._fileWatcher == null)
			{
				this._fileWatcher = new FileSystemWatcher();
				this._fileWatcher.Path = this.uniTrunkerHomeDirectory.Text;
				this._fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
				this._fileWatcher.Filter = this._trunkingFileName;
				this._fileWatcher.Changed += this.OnChanged;
			}
			this._fileWatcher.EnableRaisingEvents = true;
		}

		private void OnChanged(object source, FileSystemEventArgs e)
		{
			this.getLogData();
			if (this.trunkingEnabledCheckbox.Checked)
			{
				if (!(this.trunkingControlChannelFreq.Value == (decimal)this._controlInterface.Frequency) && this.stickyTuneTrunkingCheckbox.Checked && !this.shouldWeRetune)
				{
					return;
				}
				this.tuneToTrunkingFile();
			}
		}

		private void trunkingEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.unitrunkerConfigured)
			{
				if (this.trunkingEnabledCheckbox.Checked)
				{
					this.trunkingUnitrunkerBrowseButton.Enabled = false;
					this._plugin.Bypass = false;
					if (!string.IsNullOrEmpty(this.uniTrunkerHomeDirectory.Text))
					{
						this.initTrunkingFileWatcher();
						try
						{
							this.getLogData();
							if (this.trunkingEnabledCheckbox.Checked && (this.trunkingControlChannelFreq.Value == (decimal)this._controlInterface.Frequency || !this.stickyTuneTrunkingCheckbox.Checked || this.shouldWeRetune))
							{
								this.tuneToTrunkingFile();
							}
							else
							{
								this.writeSingleLog();
							}
						}
						catch (Exception ex)
						{
							this.DoNothing(ex);
						}
					}
				}
				else
				{
					if (this._fileWatcher != null)
					{
						this._fileWatcher.EnableRaisingEvents = false;
						this._fileWatcher.Dispose();
						this._fileWatcher = null;
					}
					if (this._retuneDelay != null)
					{
						this._retuneDelay.Stop();
						this._retuneDelay.Dispose();
						this._retuneDelay = null;
					}
					this._plugin.Bypass = true;
					this.unitrunkerIsParked = false;
					this.trunkingUnitrunkerBrowseButton.Enabled = true;
					this.setVersion();
				}
				this.trunkingTimer.Enabled = this.trunkingEnabledCheckbox.Checked;
				this.WriteConfig();
			}
			else if (!this.unitrunkerConfigureMessageDisplayed)
			{
				MessageBox.Show("Please set the Unitrunker directory below, and verify Remote.dll is in the Unitrunker directory.", "Please set Unitrunker Location", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.unitrunkerConfigureMessageDisplayed = true;
				this.trunkingEnabledCheckbox.Checked = false;
			}
			else
			{
				this.unitrunkerConfigureMessageDisplayed = false;
			}
		}

		private void uniTrunkerHomeDirectory_TextChanged(object sender, EventArgs e)
		{
			if (this.checkUnitrunkerPath(this.uniTrunkerHomeDirectory.Text))
			{
				this.initTrunkingFileWatcher();
			}
		}

		private void trunkingLogFileButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.FileName = "single.log";
			saveFileDialog.DefaultExt = "log";
			saveFileDialog.Filter = "Log File|*.log|Comma Seperated Value File|*.csv|Standard Text File|*.txt";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.singleLogFilePath.Text = saveFileDialog.FileName;
				this.loggingConfigured = true;
				this.WriteConfig();
			}
		}

		private void trunkingUnitrunkerBrowseButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog;
			do
			{
				folderBrowserDialog = new FolderBrowserDialog();
				if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
			}
			while (!this.checkUnitrunkerPath(folderBrowserDialog.SelectedPath));
			this.uniTrunkerHomeDirectory.Text = folderBrowserDialog.SelectedPath;
			this.WriteConfig();
		}

		private void trunkingTimer_Tick(object sender, EventArgs e)
		{
			this.doTrunkTuneTimerLoop();
		}

		private void trunkingTuneToControlChannel_CheckedChanged(object sender, EventArgs e)
		{
			this.WriteConfig();
		}

		private void delayRetune()
		{
			if (this._retuneDelay == null)
			{
				this._retuneDelay = new System.Timers.Timer(1250.0);
				this._retuneDelay.Elapsed += this._retuneDelay_Elapsed;
				this._retuneDelay.Start();
			}
		}

		private void _retuneDelay_Elapsed(object sender, ElapsedEventArgs e)
		{
			lock (this._retuneDelay)
			{
				if (this.shouldWeRetune && this.trunkingEnabledCheckbox.Checked)
				{
					this._retuneDelay.Stop();
					this._retuneDelay.Dispose();
					this._retuneDelay = null;
					this.getLogData();
					this.tuneToTrunkingFile();
				}
			}
		}

		public unsafe void MuteBeeps(float* audioBuffer, int length)
		{
			string text = Environment.GetEnvironmentVariable("TEMP");
			if (text.Length <= 0)
			{
				text = "C:\\temp";
			}
			StreamWriter streamWriter = new StreamWriter(text + "\\beeps6.csv", true);
			if (this.muteEdacsBeep.Checked)
			{
				int num = 1200;
				int num2 = 0;
				while (num2 < length - num)
				{
					if (this._beepDelay-- < 0)
					{
						this._controlInterface.AudioGain = 30;
					}
					UnsafeBuffer unsafeBuffer = UnsafeBuffer.Create(num, 4);
					float* ptr = (float*)(void*)unsafeBuffer;
					for (int i = 0; i < num; i++)
					{
						if (num2 >= length)
						{
							break;
						}
						ptr[i] = audioBuffer[num2 * 2];
						num2++;
					}
					int value = 48000;
					float mag = Goertzel.GetMag((double)Convert.ToInt32(value), 4800, ptr, num);
					float mag2 = Goertzel.GetMag((double)Convert.ToInt32(value), 3000, ptr, num);
					streamWriter.WriteLine($"{mag},{mag2},{num2}");
					if ((double)mag > 0.01)
					{
						this._controlInterface.AudioGain = 15;
						this._beepDelay = 2;
					}
				}
			}
			streamWriter.Flush();
			streamWriter.Close();
		}

		private void stickyTuneTrunkingCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this.WriteConfig();
			if (!this.stickyTuneTrunkingCheckbox.Checked && this.trunkingEnabledCheckbox.Checked)
			{
				this.tuneToTrunkingFile();
			}
		}

		private void singleLogFileEnabled_CheckedChanged(object sender, EventArgs e)
		{
			if (this.loggingConfigured && this.singleLogFilePath.Text != null)
			{
				if (this.singleLogFileEnabled.Checked)
				{
					this.trunkingLogFileButton.Enabled = false;
					this.loggingEnabled = true;
					if (this.trunkingEnabledCheckbox.Checked)
					{
						if (this._trunkLoggerData == null)
						{
							this.getLogData();
						}
						this.writeSingleLog();
					}
				}
				else
				{
					this.trunkingLogFileButton.Enabled = true;
					this.loggingEnabled = false;
					this.versionAndStatus.Text = "v" + this._version;
				}
				this.WriteConfig();
			}
			else if (!this.loggingSystemMessageDisplayed)
			{
				MessageBox.Show("Please set the log file location.", "Where do I write the log?", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.loggingSystemMessageDisplayed = true;
				this.singleLogFileEnabled.Checked = false;
			}
			else
			{
				this.loggingSystemMessageDisplayed = false;
			}
		}

		private void trunkingControlChannelFreq_ValueChanged(object sender, EventArgs e)
		{
			this.WriteConfig();
		}

		private void muteWhileParkedCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this.WriteConfig();
		}

		private void muteEdacsBeep_CheckedChanged(object sender, EventArgs e)
		{
			this.WriteConfig();
		}

		private void logFormatStyle_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.WriteConfig();
		}

		private void spawnLogDialog_Click(object sender, EventArgs e)
		{
			LogOptions logOptions = new LogOptions();
			logOptions.FormClosed += this.logStyleFormClosed;
			logOptions.ShowDialog();
		}

		private void logStyleFormClosed(object sender, FormClosedEventArgs e)
		{
			this._trunkerConfig = this._settingsPersister.readConfig();
			if (this.singleLogFileEnabled.Checked)
			{
				this.writeSingleLog();
			}
		}

		private void retuneDelayMethod_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.retuneDelayMethod.DroppedDown)
			{
				if (this.retuneDelayMethod.SelectedIndex == this._trunkerConfig[0].delayRetuneMethod && !this.initializing)
				{
					return;
				}
				this.resizeForDelayMethod();
				this.WriteConfig();
			}
		}

		private void resizeForDelayMethod()
		{
			int num = 40;
			if (this.retuneDelayMethod.SelectedIndex == 0)
			{
				this.retuneMinSignalLabel.Visible = false;
				this.retuneMinSignalLevel.Visible = false;
				this.currentDbLabel.Visible = false;
				this.currentDbValue.Visible = false;
				this.mainPanel.Height = this.mainPanel.Height - num;
				this.panelForOptionsAfterRetune.Top = this.panelForOptionsAfterRetune.Top - num;
			}
			if (this.retuneDelayMethod.SelectedIndex == 1)
			{
				if (!this.initializing)
				{
					this.mainPanel.Height = this.mainPanel.Height + num;
					this.panelForOptionsAfterRetune.Top = this.panelForOptionsAfterRetune.Top + num;
				}
				this.retuneMinSignalLabel.Visible = true;
				this.retuneMinSignalLevel.Visible = true;
				this.currentDbLabel.Visible = true;
				this.currentDbValue.Visible = true;
			}
		}

		private void retuneMinSignalLevel_ValueChanged(object sender, EventArgs e)
		{
			this.WriteConfig();
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
			this.trunkingControlChannelFreq = new NumericUpDown();
			this.lblTrunkingLogFile = new Label();
			this.lbltrunkingControlChannelFreq = new Label();
			this.trunkingTuneToControlChannel = new CheckBox();
			this.trunkingEnabledCheckbox = new CheckBox();
			this.stickyTuneTrunkingCheckbox = new CheckBox();
			this.uniTrunkerHomeDirectory = new TextBox();
			this.trunkingUnitrunkerBrowseButton = new Button();
			this.mainPanel = new Panel();
			this.spawnLogDialog = new Button();
			this.label2 = new Label();
			this.versionAndStatus = new Label();
			this.singleLogFileEnabled = new CheckBox();
			this.singleLogFilePath = new TextBox();
			this.trunkingLogFileButton = new Button();
			this.label1 = new Label();
			this.trunkingTimer = new System.Windows.Forms.Timer(this.components);
			this.retuneDelayMethod = new ComboBox();
			this.label3 = new Label();
			this.retuneMinSignalLevel = new NumericUpDown();
			this.retuneMinSignalLabel = new Label();
			this.panelForOptionsAfterRetune = new Panel();
			this.muteEdacsBeep = new CheckBox();
			this.muteWhileParkedCheckbox = new CheckBox();
			this.currentDbLabel = new Label();
			this.currentDbValue = new Label();
			((ISupportInitialize)this.trunkingControlChannelFreq).BeginInit();
			this.mainPanel.SuspendLayout();
			((ISupportInitialize)this.retuneMinSignalLevel).BeginInit();
			this.panelForOptionsAfterRetune.SuspendLayout();
			base.SuspendLayout();
			this.trunkingControlChannelFreq.Location = new Point(88, 365);
			this.trunkingControlChannelFreq.Maximum = new decimal(new int[4]
			{
				-1486618624,
				232830643,
				0,
				0
			});
			this.trunkingControlChannelFreq.Name = "trunkingControlChannelFreq";
			this.trunkingControlChannelFreq.Size = new Size(104, 20);
			this.trunkingControlChannelFreq.TabIndex = 39;
			this.trunkingControlChannelFreq.TextAlign = HorizontalAlignment.Right;
			this.trunkingControlChannelFreq.Visible = false;
			this.trunkingControlChannelFreq.ValueChanged += this.trunkingControlChannelFreq_ValueChanged;
			this.lblTrunkingLogFile.AutoSize = true;
			this.lblTrunkingLogFile.Location = new Point(2, 53);
			this.lblTrunkingLogFile.Name = "lblTrunkingLogFile";
			this.lblTrunkingLogFile.Size = new Size(135, 13);
			this.lblTrunkingLogFile.TabIndex = 41;
			this.lblTrunkingLogFile.Text = "UniTrunker Install Directory";
			this.lbltrunkingControlChannelFreq.AutoSize = true;
			this.lbltrunkingControlChannelFreq.Location = new Point(29, 367);
			this.lbltrunkingControlChannelFreq.Name = "lbltrunkingControlChannelFreq";
			this.lbltrunkingControlChannelFreq.Size = new Size(57, 13);
			this.lbltrunkingControlChannelFreq.TabIndex = 47;
			this.lbltrunkingControlChannelFreq.Text = "Frequency";
			this.lbltrunkingControlChannelFreq.Visible = false;
			this.trunkingTuneToControlChannel.AutoSize = true;
			this.trunkingTuneToControlChannel.Location = new Point(5, 341);
			this.trunkingTuneToControlChannel.Name = "trunkingTuneToControlChannel";
			this.trunkingTuneToControlChannel.Size = new Size(141, 17);
			this.trunkingTuneToControlChannel.TabIndex = 46;
			this.trunkingTuneToControlChannel.Text = "Tune to Control Channel";
			this.trunkingTuneToControlChannel.UseVisualStyleBackColor = true;
			this.trunkingTuneToControlChannel.Visible = false;
			this.trunkingTuneToControlChannel.CheckedChanged += this.trunkingTuneToControlChannel_CheckedChanged;
			this.trunkingEnabledCheckbox.AutoSize = true;
			this.trunkingEnabledCheckbox.Location = new Point(4, 14);
			this.trunkingEnabledCheckbox.Name = "trunkingEnabledCheckbox";
			this.trunkingEnabledCheckbox.Size = new Size(65, 17);
			this.trunkingEnabledCheckbox.TabIndex = 38;
			this.trunkingEnabledCheckbox.Text = "Enabled";
			this.trunkingEnabledCheckbox.UseVisualStyleBackColor = true;
			this.trunkingEnabledCheckbox.CheckedChanged += this.trunkingEnabledCheckbox_CheckedChanged;
			this.stickyTuneTrunkingCheckbox.AutoSize = true;
			this.stickyTuneTrunkingCheckbox.Location = new Point(4, 177);
			this.stickyTuneTrunkingCheckbox.Name = "stickyTuneTrunkingCheckbox";
			this.stickyTuneTrunkingCheckbox.Size = new Size(194, 17);
			this.stickyTuneTrunkingCheckbox.TabIndex = 43;
			this.stickyTuneTrunkingCheckbox.Text = "Delay Re-Tune Until Call Completes";
			this.stickyTuneTrunkingCheckbox.UseVisualStyleBackColor = true;
			this.stickyTuneTrunkingCheckbox.CheckedChanged += this.stickyTuneTrunkingCheckbox_CheckedChanged;
			this.uniTrunkerHomeDirectory.Enabled = false;
			this.uniTrunkerHomeDirectory.Location = new Point(3, 70);
			this.uniTrunkerHomeDirectory.Name = "uniTrunkerHomeDirectory";
			this.uniTrunkerHomeDirectory.Size = new Size(168, 20);
			this.uniTrunkerHomeDirectory.TabIndex = 40;
			this.uniTrunkerHomeDirectory.TextChanged += this.uniTrunkerHomeDirectory_TextChanged;
			this.trunkingUnitrunkerBrowseButton.Location = new Point(171, 69);
			this.trunkingUnitrunkerBrowseButton.Name = "trunkingUnitrunkerBrowseButton";
			this.trunkingUnitrunkerBrowseButton.Size = new Size(35, 23);
			this.trunkingUnitrunkerBrowseButton.TabIndex = 42;
			this.trunkingUnitrunkerBrowseButton.Text = "Set";
			this.trunkingUnitrunkerBrowseButton.UseVisualStyleBackColor = true;
			this.trunkingUnitrunkerBrowseButton.Click += this.trunkingUnitrunkerBrowseButton_Click;
			this.mainPanel.Controls.Add(this.currentDbLabel);
			this.mainPanel.Controls.Add(this.panelForOptionsAfterRetune);
			this.mainPanel.Controls.Add(this.currentDbValue);
			this.mainPanel.Controls.Add(this.retuneMinSignalLabel);
			this.mainPanel.Controls.Add(this.retuneMinSignalLevel);
			this.mainPanel.Controls.Add(this.retuneDelayMethod);
			this.mainPanel.Controls.Add(this.spawnLogDialog);
			this.mainPanel.Controls.Add(this.label2);
			this.mainPanel.Controls.Add(this.versionAndStatus);
			this.mainPanel.Controls.Add(this.singleLogFileEnabled);
			this.mainPanel.Controls.Add(this.singleLogFilePath);
			this.mainPanel.Controls.Add(this.trunkingLogFileButton);
			this.mainPanel.Controls.Add(this.label1);
			this.mainPanel.Controls.Add(this.trunkingControlChannelFreq);
			this.mainPanel.Controls.Add(this.uniTrunkerHomeDirectory);
			this.mainPanel.Controls.Add(this.trunkingUnitrunkerBrowseButton);
			this.mainPanel.Controls.Add(this.stickyTuneTrunkingCheckbox);
			this.mainPanel.Controls.Add(this.label3);
			this.mainPanel.Controls.Add(this.lblTrunkingLogFile);
			this.mainPanel.Controls.Add(this.trunkingEnabledCheckbox);
			this.mainPanel.Controls.Add(this.lbltrunkingControlChannelFreq);
			this.mainPanel.Controls.Add(this.trunkingTuneToControlChannel);
			this.mainPanel.Location = new Point(7, 14);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new Size(214, 315);
			this.mainPanel.TabIndex = 49;
			this.spawnLogDialog.Location = new Point(107, 141);
			this.spawnLogDialog.Name = "spawnLogDialog";
			this.spawnLogDialog.Size = new Size(99, 22);
			this.spawnLogDialog.TabIndex = 57;
			this.spawnLogDialog.Text = "Show Options";
			this.spawnLogDialog.UseVisualStyleBackColor = true;
			this.spawnLogDialog.Click += this.spawnLogDialog_Click;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(2, 145);
			this.label2.Name = "label2";
			this.label2.Size = new Size(60, 13);
			this.label2.TabIndex = 56;
			this.label2.Text = "Log Format";
			this.versionAndStatus.Location = new Point(71, 15);
			this.versionAndStatus.Name = "versionAndStatus";
			this.versionAndStatus.RightToLeft = RightToLeft.No;
			this.versionAndStatus.Size = new Size(135, 30);
			this.versionAndStatus.TabIndex = 54;
			this.versionAndStatus.TextAlign = ContentAlignment.TopRight;
			this.singleLogFileEnabled.AutoSize = true;
			this.singleLogFileEnabled.CheckAlign = ContentAlignment.MiddleRight;
			this.singleLogFileEnabled.Location = new Point(147, 97);
			this.singleLogFileEnabled.Name = "singleLogFileEnabled";
			this.singleLogFileEnabled.Size = new Size(59, 17);
			this.singleLogFileEnabled.TabIndex = 53;
			this.singleLogFileEnabled.Text = "Enable";
			this.singleLogFileEnabled.UseVisualStyleBackColor = true;
			this.singleLogFileEnabled.CheckedChanged += this.singleLogFileEnabled_CheckedChanged;
			this.singleLogFilePath.Enabled = false;
			this.singleLogFilePath.Location = new Point(3, 115);
			this.singleLogFilePath.Name = "singleLogFilePath";
			this.singleLogFilePath.Size = new Size(168, 20);
			this.singleLogFilePath.TabIndex = 50;
			this.trunkingLogFileButton.Location = new Point(171, 114);
			this.trunkingLogFileButton.Name = "trunkingLogFileButton";
			this.trunkingLogFileButton.Size = new Size(35, 22);
			this.trunkingLogFileButton.TabIndex = 52;
			this.trunkingLogFileButton.Text = "Set";
			this.trunkingLogFileButton.UseVisualStyleBackColor = true;
			this.trunkingLogFileButton.Click += this.trunkingLogFileButton_Click;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(2, 98);
			this.label1.Name = "label1";
			this.label1.Size = new Size(111, 13);
			this.label1.TabIndex = 51;
			this.label1.Text = "Single Log File Output";
			this.trunkingTimer.Tick += this.trunkingTimer_Tick;
			this.retuneDelayMethod.DropDownStyle = ComboBoxStyle.DropDownList;
			this.retuneDelayMethod.FormattingEnabled = true;
			this.retuneDelayMethod.Items.AddRange(new object[2]
			{
				"Squelch",
				"Signal"
			});
			this.retuneDelayMethod.Location = new Point(143, 194);
			this.retuneDelayMethod.Name = "retuneDelayMethod";
			this.retuneDelayMethod.Size = new Size(62, 21);
			this.retuneDelayMethod.TabIndex = 65;
			this.retuneDelayMethod.SelectedIndexChanged += this.retuneDelayMethod_SelectedIndexChanged;
			this.label3.Location = new Point(3, 199);
			this.label3.Name = "label3";
			this.label3.Size = new Size(134, 13);
			this.label3.TabIndex = 41;
			this.label3.Text = "Delay detection method:";
			this.label3.TextAlign = ContentAlignment.TopRight;
			this.retuneMinSignalLevel.DecimalPlaces = 1;
			this.retuneMinSignalLevel.Increment = new decimal(new int[4]
			{
				1,
				0,
				0,
				65536
			});
			this.retuneMinSignalLevel.Location = new Point(143, 221);
			NumericUpDown numericUpDown = this.retuneMinSignalLevel;
			int[] bits = new int[4];
			numericUpDown.Maximum = new decimal(bits);
			this.retuneMinSignalLevel.Minimum = new decimal(new int[4]
			{
				130,
				0,
				0,
				-2147483648
			});
			this.retuneMinSignalLevel.Name = "retuneMinSignalLevel";
			this.retuneMinSignalLevel.Size = new Size(62, 20);
			this.retuneMinSignalLevel.TabIndex = 66;
			this.retuneMinSignalLevel.TextAlign = HorizontalAlignment.Right;
			this.retuneMinSignalLevel.Visible = false;
			this.retuneMinSignalLevel.ValueChanged += this.retuneMinSignalLevel_ValueChanged;
			this.retuneMinSignalLabel.Location = new Point(2, 223);
			this.retuneMinSignalLabel.Name = "retuneMinSignalLabel";
			this.retuneMinSignalLabel.Size = new Size(135, 13);
			this.retuneMinSignalLabel.TabIndex = 67;
			this.retuneMinSignalLabel.Text = "Minimum Signal (dB):";
			this.retuneMinSignalLabel.TextAlign = ContentAlignment.TopRight;
			this.retuneMinSignalLabel.Visible = false;
			this.panelForOptionsAfterRetune.Controls.Add(this.muteEdacsBeep);
			this.panelForOptionsAfterRetune.Controls.Add(this.muteWhileParkedCheckbox);
			this.panelForOptionsAfterRetune.Location = new Point(0, 262);
			this.panelForOptionsAfterRetune.Name = "panelForOptionsAfterRetune";
			this.panelForOptionsAfterRetune.Size = new Size(214, 49);
			this.panelForOptionsAfterRetune.TabIndex = 68;
			this.muteEdacsBeep.AutoSize = true;
			this.muteEdacsBeep.Enabled = false;
			this.muteEdacsBeep.Location = new Point(4, 28);
			this.muteEdacsBeep.Name = "muteEdacsBeep";
			this.muteEdacsBeep.Size = new Size(190, 17);
			this.muteEdacsBeep.TabIndex = 51;
			this.muteEdacsBeep.Text = "Mute EDACS Tones (Not yet fixed)";
			this.muteEdacsBeep.UseVisualStyleBackColor = true;
			this.muteWhileParkedCheckbox.AutoSize = true;
			this.muteWhileParkedCheckbox.Enabled = false;
			this.muteWhileParkedCheckbox.Location = new Point(4, 4);
			this.muteWhileParkedCheckbox.Name = "muteWhileParkedCheckbox";
			this.muteWhileParkedCheckbox.Size = new Size(185, 17);
			this.muteWhileParkedCheckbox.TabIndex = 50;
			this.muteWhileParkedCheckbox.Text = "Mute While Parked (Not yet fixed)";
			this.muteWhileParkedCheckbox.UseVisualStyleBackColor = true;
			this.currentDbLabel.Location = new Point(3, 246);
			this.currentDbLabel.Name = "currentDbLabel";
			this.currentDbLabel.Size = new Size(134, 13);
			this.currentDbLabel.TabIndex = 69;
			this.currentDbLabel.Text = "Current dB Level:";
			this.currentDbLabel.TextAlign = ContentAlignment.TopRight;
			this.currentDbLabel.Visible = false;
			this.currentDbValue.Location = new Point(143, 246);
			this.currentDbValue.Name = "currentDbValue";
			this.currentDbValue.Size = new Size(62, 13);
			this.currentDbValue.TabIndex = 67;
			this.currentDbValue.Text = "Off";
			this.currentDbValue.TextAlign = ContentAlignment.TopRight;
			this.currentDbValue.Visible = false;
			base.AccessibleDescription = "";
			base.AccessibleName = "Trunker";
			base.AccessibleRole = AccessibleRole.Pane;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.mainPanel);
			base.Name = "TrunkerPanel";
			base.Size = new Size(226, 332);
			base.Tag = "Trunker";
			((ISupportInitialize)this.trunkingControlChannelFreq).EndInit();
			this.mainPanel.ResumeLayout(false);
			this.mainPanel.PerformLayout();
			((ISupportInitialize)this.retuneMinSignalLevel).EndInit();
			this.panelForOptionsAfterRetune.ResumeLayout(false);
			this.panelForOptionsAfterRetune.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
