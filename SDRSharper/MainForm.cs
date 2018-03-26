//Remember to double-check reference version - VS2017 doesn't do a good job of it when changing builds
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using SDRSharp.CollapsiblePanel;
using SDRSharp.Common;
using SDRSharp.Controls;
using SDRSharp.PanView;
using SDRSharp.Radio;
using SDRSharp.Radio.PortAudio;

namespace SDRSharp
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm, ISharpControl, INotifyPropertyChanged
    {
        #region [Misc]
        public static Assembly UnmanagedDLLAssemblyVer = Assembly.GetExecutingAssembly();

        public event PropertyChangedEventHandler PropertyChanged;

        public DetectorType DetectorType
        {
            get
            {
                return this._vfo.DetectorType;
            }
            set
            {
                switch (value)
                {
                    case DetectorType.NFM:
                        this.nfmRadioButton.Checked = true;
                        return;
                    case DetectorType.WFM:
                        this.wfmRadioButton.Checked = true;
                        return;
                    case DetectorType.AM:
                        this.amRadioButton.Checked = true;
                        return;
                    case DetectorType.DSB:
                        this.dsbRadioButton.Checked = true;
                        return;
                    case DetectorType.LSB:
                        this.lsbRadioButton.Checked = true;
                        return;
                    case DetectorType.USB:
                        this.usbRadioButton.Checked = true;
                        return;
                    case DetectorType.CW:
                        this.cwRadioButton.Checked = true;
                        return;
                    case DetectorType.RAW:
                        this.rawRadioButton.Checked = true;
                        return;
                    case DetectorType.SAM:
                        this.samRadioButton.Checked = true;
                        return;
                    default:
                        return;
                }
            }
        }

        public WindowType FilterType
        {
            get
            {
                return this.filterTypeComboBox.SelectedIndex + WindowType.Hamming;
            }
            set
            {
                this.filterTypeComboBox.SelectedIndex = value - WindowType.Hamming;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this._streamControl.IsPlaying;
            }
        }

        public long Frequency
        {
            get
            {
                return (long)this.frequencyNumericUpDown.Value;
            }
            set
            {
                this.frequencyNumericUpDown.Value = value;
            }
        }

        public long CenterFrequency
        {
            get
            {
                return this.centerFreqNumericUpDown.Value;
            }
            set
            {
                if (this._frontendController != null)
                {
                    this.centerFreqNumericUpDown.Value = value;
                    this._fftSkipCnt = -2;
                    return;
                }
                if (this.SourceIsWaveFile && value >= this.centerFreqNumericUpDown.Value - (decimal)this._streamControl.SampleRate && value <= this.centerFreqNumericUpDown.Value + (decimal)this._streamControl.SampleRate)
                {
                    return;
                }
                throw new ApplicationException("Cannot set the center frequency when no front end is connected");
            }
        }

        public long FrequencyShift
        {
            get
            {
                return this.frequencyShiftNumericUpDown.Value;
            }
            set
            {
                this.frequencyShiftNumericUpDown.Value = value;
            }
        }

        public bool FrequencyShiftEnabled
        {
            get
            {
                return this.frequencyShiftCheckBox.Checked;
            }
            set
            {
                this.frequencyShiftCheckBox.Checked = value;
            }
        }

        public int FilterBandwidth
        {
            get
            {
                return (int)this.filterBandwidthNumericUpDown.Value;
            }
            set
            {
                this.filterBandwidthNumericUpDown.Value = (long)value;
            }
        }

        public int FilterOrder
        {
            get
            {
                return (int)this.filterOrderNumericUpDown.Value;
            }
            set
            {
                this.filterOrderNumericUpDown.Value = (long)value;
            }
        }

        public bool SquelchEnabled
        {
            get
            {
                return this.useSquelchCheckBox.Checked;
            }
            set
            {
                this.useSquelchCheckBox.Checked = value;
            }
        }

        public int SquelchThreshold
        {
            get
            {
                return (int)this.squelchNumericUpDown.Value;
            }
            set
            {
                this.squelchNumericUpDown.Value = (long)value;
            }
        }

        public int CWShift
        {
            get
            {
                return (int)this.cwShiftNumericUpDown.Value;
            }
            set
            {
                this.cwShiftNumericUpDown.Value = (long)value;
            }
        }

        public bool SnapToGrid
        {
            get
            {
                return this.snapFrequencyCheckBox.Checked;
            }
            set
            {
                this.snapFrequencyCheckBox.Checked = value;
            }
        }

        public bool SwapIq
        {
            get
            {
                return this.swapIQCheckBox.Checked;
            }
            set
            {
                this.swapIQCheckBox.Checked = value;
            }
        }

        public bool FmStereo
        {
            get
            {
                return this.fmStereoCheckBox.Checked;
            }
            set
            {
                this.fmStereoCheckBox.Checked = value;
            }
        }

        public bool MarkPeaks
        {
            get
            {
                return this.markPeaksCheckBox.Checked;
            }
            set
            {
                this.markPeaksCheckBox.Checked = value;
            }
        }

        public int AudioGain
        {
            get
            {
                return this.audioGainTrackBar.Value;
            }
            set
            {
                this.audioGainTrackBar.Value = value;
            }
        }

        public bool FilterAudio
        {
            get
            {
                return this.filterAudioCheckBox.Checked;
            }
            set
            {
                this.filterAudioCheckBox.Checked = value;
            }
        }

        public bool UseAgc
        {
            get
            {
                return this.agcCheckBox.Checked;
            }
            set
            {
                this.agcCheckBox.Checked = value;
            }
        }

        public bool AgcHang
        {
            get
            {
                return this.agcUseHangCheckBox.Checked;
            }
            set
            {
                this.agcUseHangCheckBox.Checked = value;
            }
        }

        public int AgcThreshold
        {
            get
            {
                return (int)this.agcThresholdNumericUpDown.Value;
            }
            set
            {
                this.agcThresholdNumericUpDown.Value = (long)value;
            }
        }

        public int AgcDecay
        {
            get
            {
                return this.agcDecayNumericUpDown.Value;
            }
            set
            {
                this.agcDecayNumericUpDown.Value = value;
            }
        }

        public int AgcSlope
        {
            get
            {
                return (int)this.agcSlopeNumericUpDown.Value;
            }
            set
            {
                this.agcSlopeNumericUpDown.Value = (long)value;
            }
        }

        public int SAttack
        {
            get
            {
                return this.sAttackTrackBar.Value;
            }
            set
            {
                this.sAttackTrackBar.Value = value;
            }
        }

        public int SDecay
        {
            get
            {
                return this.sDecayTrackBar.Value;
            }
            set
            {
                this.sDecayTrackBar.Value = value;
            }
        }

        public int WAttack
        {
            get
            {
                return this.wAttackTrackBar.Value;
            }
            set
            {
                this.wAttackTrackBar.Value = value;
            }
        }

        public int WDecay
        {
            get
            {
                return this.wDecayTrackBar.Value;
            }
            set
            {
                this.wDecayTrackBar.Value = value;
            }
        }

        public bool UseTimeMarkers
        {
            get
            {
                return this.useTimestampsCheckBox.Checked;
            }
            set
            {
                this.useTimestampsCheckBox.Checked = value;
            }
        }

        public string RdsProgramService
        {
            get
            {
                return this._vfo.RdsStationName;
            }
        }

        public string RdsRadioText
        {
            get
            {
                return this._vfo.RdsStationText;
            }
        }

        public int RFBandwidth
        {
            get
            {
                return (int)this._vfo.SampleRate;
            }
        }

        public bool SourceIsWaveFile
        {
            get
            {
                return this.iqSourceComboBox.SelectedIndex == this.iqSourceComboBox.Items.Count - 2;
            }
        }

        public bool IsSquelchOpen
        {
            get
            {
                return true;
            }
        }

        public int FFTResolution
        {
            get
            {
                return this._fftBins;
            }
        }

        public int FFTSkips
        {
            set
            {
                this._fftSkips = value;
            }
        }
        #endregion

        #region [MachineType]
        public static MachineType GetDllMachineType(string dllPath)
        {
            //see http://www.microsoft.com/whdc/system/platform/firmware/PECOFF.mspx
            //offset to PE header is always at 0x3C
            //PE header starts with "PE\0\0" =  0x50 0x45 0x00 0x00
            //followed by 2-byte machine type field (see document above for enum)
            FileStream fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            fs.Seek(0x3c, SeekOrigin.Begin);
            Int32 peOffset = br.ReadInt32();
            fs.Seek(peOffset, SeekOrigin.Begin);
            UInt32 peHead = br.ReadUInt32();

            if (peHead != 0x00004550) // "PE\0\0", little-endian
                throw new Exception("Can't find PE header");

            MachineType machineType = (MachineType)br.ReadUInt16();

            br.Close();
            fs.Close();
            return machineType;
        }

        public enum MachineType : ushort
        {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
        }

        // returns true if the dll is 64-bit, false if 32-bit, and null if unknown
        public static bool? UnmanagedDllIs64Bit(string dllPath)
        {
            switch (GetDllMachineType(dllPath))
            {
                case MachineType.IMAGE_FILE_MACHINE_AMD64:
                case MachineType.IMAGE_FILE_MACHINE_IA64:
                    return true;
                case MachineType.IMAGE_FILE_MACHINE_I386:
                    return false;
                default:
                    return null;
            }
        }

        public static string getArch()
        {
            MachineType dlltype = GetDllMachineType(Application.ExecutablePath);

            if (dlltype.Equals(MachineType.IMAGE_FILE_MACHINE_I386))
            {
                Console.WriteLine("Dll architecture: x86/32bit");
                arch = "x86";
            }
            else if (dlltype.Equals(MachineType.IMAGE_FILE_MACHINE_AMD64))
            {
                Console.WriteLine("Dll architecture: x64/64bit");
                arch = "x64";
            }

            return arch;
        }
        #endregion

        #region [Main/Init]
        public unsafe MainForm()
        {
            this._stopwatch.Start();
            base.SuspendLayout();
            this._iqPtr = (Complex*)this._iqBuffer;
            this._afqPtr = (Complex*)this._afqBuffer;
            this._ifqPtr = (Complex*)this._ifqBuffer;
            this._fftPtr = (Complex*)this._fftBuffer;
            this._aftPtr = (Complex*)this._aftBuffer;
            this._iftPtr = (Complex*)this._iftBuffer;
            this._fftWindowPtr = (float*)this._fftWindow;
            this._bftWindowPtr = (float*)this._bftWindow;
            this._rfSpectrumPtr = (float*)this._rfSpectrum;
            this._rfAveragePtr = (float*)this._rfAverage;
            this._afSpectrumPtr = (float*)this._afSpectrum;
            this._iftSpectrumPtr = (float*)this._iftSpectrum;
            this._scaledRfSpectrumPtr = (byte*)this._scaledRfSpectrum;
            this._scaledAfSpectrumPtr = (byte*)this._scaledAfSpectrum;
            this._scaledIFTSpectrumPtr = (byte*)this._scaledIFTSpectrum;
            GradientDialog.GradientChanged += this.GradientDialog_GradientChanged;
            MainForm.loadSettings();
            Utils.Log("Settings loaded", false);
            this._streamControl = new StreamControl();
            this._iqBalancer = new IQBalancer();
            this._fractResampler = new FractResampler();
            this._audioDecimator = new FloatDecimator(1);
            this._vfoHookManager = new VfoHookManager();
            this._vfo = new Vfo(this._vfoHookManager);
            this._vfoHookManager.Vfo = this._vfo;
            this.InitializeComponent();
            Utils.Log("Component initialized", false);
            this.InitialiseSharpPlugins();
            Utils.Log("Plugins initialized", false);
            this.InitializeGUI();
            Utils.Log("GUI initialized", false);
            this.barMeter.DrawBackground(0, 1000, 200);
            this.scrollPanel.AutoScroll = false;
            Console.WriteLine("MainForm constructor " + this._stopwatch.ElapsedMilliseconds + " ms");
            base.ResumeLayout();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine("MainForm_Load " + this._stopwatch.ElapsedMilliseconds + " ms");
            int index = Utils.GetIntSetting("iqSource", this.iqSourceComboBox.Items.Count - 1);
            this.iqSourceComboBox.SelectedIndex = ((index < this.iqSourceComboBox.Items.Count) ? index : (this.iqSourceComboBox.Items.Count - 1));
            Utils.Log("IQ source selected.", false);
            this._formLoaded = true;
            base.Size = this._lastSize;
            base.Location = this._lastLocation;
            this.panSplitContainer.SplitterDistance = Utils.GetIntSetting("splitterPosition", this.panSplitContainer.SplitterDistance);
            this.panSplitContainer2.SplitterDistance = Utils.GetIntSetting("splitter2Position", this.panSplitContainer2.SplitterDistance);
            this.panSplitContainer3.Panel2MinSize = 150;
            this.panSplitContainer3.SplitterDistance = Utils.GetIntSetting("splitter3Position", this.panSplitContainer3.SplitterDistance);
            this.panSplitContainer4.SplitterDistance = Utils.GetIntSetting("splitter4Position", this.panSplitContainer4.SplitterDistance);
            this.panSplitContainer5.SplitterDistance = Utils.GetIntSetting("splitter5Position", this.panSplitContainer5.SplitterDistance);
            Utils.Log("MainForm loaded", false);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this._stopwatch.Stop();
            Console.WriteLine("MainForm_Shown " + this._stopwatch.ElapsedMilliseconds + " ms");
            if (this.iqSourceComboBox.SelectedItem.ToString().IndexOf("extio", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                this._frontendController.ShowSettingGUI(this);
            }
            Utils.Log("MainForm shown", false);
        }

        private unsafe void InitializeGUI()
        {
            this._initializing = true;
            this.playStopButton.SetColor(Color.Silver);
            this._modeStates[DetectorType.AM] = Utils.GetIntArraySetting("stateAM", MainForm._defaultAMState);
            this._modeStates[DetectorType.SAM] = Utils.GetIntArraySetting("stateSAM", MainForm._defaultDSBState);
            this._modeStates[DetectorType.DSB] = Utils.GetIntArraySetting("stateDSB", MainForm._defaultDSBState);
            this._modeStates[DetectorType.LSB] = Utils.GetIntArraySetting("stateLSB", MainForm._defaultSSBState);
            this._modeStates[DetectorType.USB] = Utils.GetIntArraySetting("stateUSB", MainForm._defaultSSBState);
            this._modeStates[DetectorType.CW] = Utils.GetIntArraySetting("stateCW", MainForm._defaultCWState);
            this._modeStates[DetectorType.WFM] = Utils.GetIntArraySetting("stateWFM", MainForm._defaultWFMState);
            this._modeStates[DetectorType.NFM] = Utils.GetIntArraySetting("stateNFM", MainForm._defaultNFMState);
            this._modeStates[DetectorType.RAW] = Utils.GetIntArraySetting("stateRAW", MainForm._defaultRAWState);
            this._inputDevices = AudioDevice.GetDevices(DeviceDirection.Input);
            this._outputDevices = AudioDevice.GetDevices(DeviceDirection.Output);
            if (this._inputDevices == null)
            {
                MessageBox.Show("Error setting audiodevices, portaudio.dll missing?");
                Utils.Log("PortAudio.dll missing.", false);
                throw new ApplicationException();
            }
            int defaultIndex = 0;
            int savedIndex = -1;
            string savedDeviceName = Utils.GetStringSetting("inputDevice", string.Empty);
            for (int i = 0; i < this._inputDevices.Count; i++)
            {
                Utils.Log("    Audio input device " + this._inputDevices[i].ToString() + " found.", false);
                int len = Math.Min(24, this._inputDevices[i].ToString().Length);
                this.inputDeviceComboBox.Items.Add(this._inputDevices[i].ToString().Substring(0, len));
                if (this._inputDevices[i].IsDefault)
                {
                    defaultIndex = i;
                }
                if (this.inputDeviceComboBox.Items[this.inputDeviceComboBox.Items.Count - 1] == savedDeviceName)
                {
                    savedIndex = i;
                }
            }
            if (this.inputDeviceComboBox.Items.Count > 0)
            {
                this.inputDeviceComboBox.SelectedIndex = ((savedIndex >= 0) ? savedIndex : defaultIndex);
            }
            defaultIndex = 0;
            savedIndex = -1;
            savedDeviceName = Utils.GetStringSetting("outputDevice", string.Empty);
            for (int j = 0; j < this._outputDevices.Count; j++)
            {
                Utils.Log("    Audio output device " + this._outputDevices[j].ToString() + " found.", false);
                int len2 = Math.Min(24, this._outputDevices[j].ToString().Length);
                this.outputDeviceComboBox.Items.Add(this._outputDevices[j].ToString().Substring(0, len2));
                if (this._outputDevices[j].IsDefault)
                {
                    defaultIndex = j;
                }
                if (this.outputDeviceComboBox.Items[this.outputDeviceComboBox.Items.Count - 1] == savedDeviceName)
                {
                    savedIndex = j;
                }
            }
            if (this.outputDeviceComboBox.Items.Count > 0)
            {
                this.outputDeviceComboBox.SelectedIndex = ((savedIndex >= 0) ? savedIndex : defaultIndex);
            }
            this._streamControl.ProcessBufferPtr += this.ProcessBuffer;
            if (Utils.Settings.Count == 0)
            {
                Environment.Exit(1);
            }
            this.cwShiftNumericUpDown.Value = (long)Utils.GetIntSetting("cwShift", 600);
            this.cwShiftNumericUpDown_ValueChanged(null, null);
            this.agcCheckBox.Checked = true;
            this.agcCheckBox_CheckedChanged(null, null);
            this.agcThresholdNumericUpDown.Value = (long)Utils.GetIntSetting("agcThreshold", -100);
            this.agcThresholdNumericUpDown_ValueChanged(null, null);
            this.agcDecayNumericUpDown.Value = Utils.GetIntSetting("agcDecay", 100);
            this.agcDecayNumericUpDown_ValueChanged(null, null);
            this.agcSlopeNumericUpDown.Value = (long)Utils.GetIntSetting("agcSlope", 0);
            this.agcSlopeNumericUpDown_ValueChanged(null, null);
            this.agcUseHangCheckBox.Checked = Utils.GetBooleanSetting("agcHang");
            this.agcUseHangCheckBox_CheckedChanged(null, null);
            this.centerFreqNumericUpDown.Value = 0L;
            this.centerFreqNumericUpDown_ValueChanged(null, null);
            this.snapFrequencyCheckBox.Checked = Utils.GetBooleanSetting("snapToGrid");
            this.swapIQCheckBox.Checked = Utils.GetBooleanSetting("swapIQ");
            this.swapIQCheckBox_CheckedChanged(null, null);
            this.correctIQCheckBox.Checked = Utils.GetBooleanSetting("correctIQ");
            this.autoCorrectIQCheckBox_CheckStateChanged(null, null);
            this.markPeaksCheckBox.Checked = Utils.GetBooleanSetting("markPeaks");
            this.markPeaksCheckBox_CheckedChanged(null, null);
            this.fmStereoCheckBox.Checked = Utils.GetBooleanSetting("fmStereo");
            this.fmStereoCheckBox_CheckedChanged(null, null);
            this.audioGainTrackBar.Value = Utils.GetIntSetting("audioGain", 30);
            this.audioGainTrackBar_ValueChanged(null, null);
            this.fftZoomCombo.SelectedIndex = Utils.GetIntSetting("zoomIndex", 0);
            this.latencyNumericUpDown.Value = (long)Utils.GetIntSetting("latency", 100);
            base.WindowState = (FormWindowState)Utils.GetIntSetting("windowState", 0);
            int left = 0;
            foreach (Screen scr in Screen.AllScreens)
            {
                left = Math.Min(left, scr.Bounds.Left);
            }
            int[] locationArray = Utils.GetIntArraySetting("windowPosition", null);
            this._lastLocation = base.Location;
            if (locationArray == null)
            {
                return;
            }
            this._lastLocation.X = Math.Max(0, locationArray[0]);
            this._lastLocation.Y = Math.Max(0, locationArray[1]);
            int[] sizeArray = Utils.GetIntArraySetting("windowSize", null);
            this._lastSize = base.Size;
            if (sizeArray == null)
            {
                return;
            }
            this._lastSize.Width = Math.Min(Screen.PrimaryScreen.Bounds.Width, sizeArray[0]);
            this._lastSize.Height = Math.Min(Screen.PrimaryScreen.Bounds.Height, sizeArray[1]);
            this.waterfall.FilterOffset = (this.spectrumAnalyzer.FilterOffset = 0);
            this.ifWaterfall.FilterOffset = (this.ifAnalyzer.FilterOffset = 0);
            this.afWaterfall.FilterOffset = (this.afAnalyzer.FilterOffset = 0);
            this.audiogram.FilterOffset = (this.afWaterfall.FilterOffset = (this.afAnalyzer.FilterOffset = 0));
            this.spectrumAnalyzer.DisplayFrequency = 0L;
            this.spectrumAnalyzer.CenterFrequency = 0L;
            this.ifAnalyzer.DisplayFrequency = 0L;
            this.ifAnalyzer.CenterFrequency = 0L;
            this.afAnalyzer.Frequency = 1L;
            this.afWaterfall.Frequency = 1L;
            this.frequencyShiftNumericUpDown.Value = Utils.GetLongSetting("frequencyShift", 0L);
            this.frequencyShiftNumericUpDown_ValueChanged(null, null);
            this.frequencyShiftCheckBox.Checked = Utils.GetBooleanSetting("frequencyShiftEnabled");
            this.frequencyShiftCheckBox_CheckStateChanged(null, null);
            Utils.Log("VFO initialized.", false);
            this._fftTimer = new System.Windows.Forms.Timer(this.components);
            this._fftTimer.Tick += this.fftTimer_Tick;
            this._fftTimer.Interval = 20;
            this._performTimer = new System.Windows.Forms.Timer(this.components);
            this._performTimer.Tick += this.performTimer_Tick;
            this._performTimer.Interval = 40;
            this._performTimer.Enabled = true;
            this.fftResolutionComboBox.SelectedIndex = Utils.GetIntSetting("fftResolution", 3);
            this.fftWindowComboBox.SelectedIndex = Utils.GetIntSetting("fftWindowType", 3);
            this.fftSpeedTrackBar.Value = Math.Min(this.fftSpeedTrackBar.Maximum - 2, Utils.GetIntSetting("fftSpeed", 4));
            this.fftSpeedTrackBar_ValueChanged(null, null);
            this.tbAgSpeed.Value = Math.Min(this.tbAgSpeed.Maximum - 2, Utils.GetIntSetting("agSpeed", 4));
            this.tbAgSpeed_ValueChanged(null, null);
            this.spectrumAnalyzer.Attack = Utils.GetDoubleSetting("spectrumAnalyzerAttack", 0.9);
            this.ifAnalyzer.Attack = 0.0;
            this.afAnalyzer.Attack = 0.0;
            this.sAttackTrackBar.Value = (int)(this.spectrumAnalyzer.Attack * (double)this.sAttackTrackBar.Maximum);
            this.spectrumAnalyzer.Decay = Utils.GetDoubleSetting("spectrumAnalyzerDecay", 0.3);
            this.ifAnalyzer.Decay = 0.0;
            this.afAnalyzer.Decay = 0.0;
            this.sDecayTrackBar.Value = (int)(this.spectrumAnalyzer.Decay * (double)this.sDecayTrackBar.Maximum);
            this.waterfall.Attack = Utils.GetDoubleSetting("waterfallAttack", 0.9);
            this.wAttackTrackBar.Value = (int)(this.waterfall.Attack * (double)this.wAttackTrackBar.Maximum);
            this.waterfall.Decay = Utils.GetDoubleSetting("waterfallDecay", 0.5);
            this.wDecayTrackBar.Value = (int)(this.waterfall.Decay * (double)this.wDecayTrackBar.Maximum);
            this.remDcSlider.Value = Utils.GetIntSetting("removeDC", 100);
            this.ifWaterfall.Attack = this.waterfall.Attack;
            this.ifWaterfall.Decay = this.waterfall.Decay;
            this.afWaterfall.Attack = this.waterfall.Attack;
            this.afWaterfall.Decay = this.waterfall.Decay;
            this.audiogram.Attack = this.waterfall.Attack;
            this.audiogram.Decay = this.waterfall.Decay;
            int t = Utils.GetIntSetting("useTimeMarkers", 0);
            this.useTimestampsCheckBox.Checked = (t > 0);
            this.useTimestampsCheckBox.Text = ((t != 2) ? "Time" : "UTC");
            this.useTimestampCheckBox_CheckedChanged(null, null);
            this.GetColorSettings();
            this.spectrumAnalyzer.ContextMnu = this.MnuSA;
            this.waterfall.ContextMnu = this.MnuSA;
            this.ifAnalyzer.ContextMnu = this.MnuSA;
            this.ifWaterfall.ContextMnu = this.MnuSA;
            this.afAnalyzer.ContextMnu = this.MnuSA;
            this.afWaterfall.ContextMnu = this.MnuSA;
            this.audiogram.ContextMenuStrip = this.MnuSA;
            this.scope.ContextMenuStrip = this.MnuSA;
            this.wideScope.ContextMenuStrip = this.MnuSA;
            this.cmbDbm.SelectedIndex = Utils.GetIntSetting("dBm", 0);
            this.dbmOffsetUpDown.Value = (long)Utils.GetIntSetting("dBmOffset", 0);
            this.chkWF.Checked = Utils.GetBooleanSetting("showWaterfall");
            this.chkIF.Checked = Utils.GetBooleanSetting("showBaseband");
            this.cmbAudio.SelectedIndex = Utils.GetIntSetting("showAudio", 0);
            int maxPwr = -50;
            int minPwr = -120;
            this.tbSpanSA.Value = Math.Max(this.tbSpanSA.Minimum, Math.Min(this.tbSpanSA.Maximum, Utils.GetIntSetting("spanSA", this.tbSpanSA.Minimum + this.tbSpanSA.Maximum - (maxPwr - minPwr))));
            this.tbFloorSA.Value = Math.Max(this.tbFloorSA.Minimum, Math.Min(this.tbFloorSA.Maximum, Utils.GetIntSetting("floorSA", this.tbFloorSA.Minimum + this.tbFloorSA.Maximum - minPwr)));
            maxPwr = -50;
            minPwr = -140;
            this.tbContrastWv.Value = Math.Max(this.tbContrastWv.Minimum, Math.Min(this.tbContrastWv.Maximum, Utils.GetIntSetting("contrastWV", this.tbContrastWv.Minimum + this.tbContrastWv.Maximum - (maxPwr - minPwr))));
            this.tbIntensityWv.Value = Math.Max(this.tbIntensityWv.Minimum, Math.Min(this.tbIntensityWv.Maximum, Utils.GetIntSetting("intensityWV", this.tbIntensityWv.Minimum + this.tbIntensityWv.Maximum - minPwr)));
            this.tbContrastAg.Value = Math.Max(this.tbContrastAg.Minimum, Math.Min(this.tbContrastAg.Maximum, Utils.GetIntSetting("contrastAG", this.tbContrastAg.Minimum + this.tbContrastAg.Maximum - (maxPwr - minPwr))));
            this.tbIntensityAg.Value = Math.Max(this.tbIntensityAg.Minimum, Math.Min(this.tbIntensityAg.Maximum, Utils.GetIntSetting("intensityAG", this.tbIntensityAg.Minimum + this.tbIntensityAg.Maximum - minPwr)));
            this.cmbVer.SelectedIndex = Utils.GetIntSetting("verticalVoltsPerDiv", 2);
            this.cmbHor.SelectedIndex = Utils.GetIntSetting("horizontalVoltsPerDiv", 2);
            this.cmbTim.SelectedIndex = Utils.GetIntSetting("horizontalTimePerDiv", 3);
            this.chkXY.Checked = Utils.GetBooleanSetting("XYmode");
            this.chkXY_CheckedChanged(null, null);
            this.tbTrigL.Value = Utils.GetIntSetting("triggerLevel", 0);
            this.wideScope.TrigLevel = 0.1f;
            this.scope.Hshift = (float)Utils.GetIntSetting("horizontalShift", 0) / 100f;
            this.scope.Vshift = (float)Utils.GetIntSetting("verticalShift", 0) / 100f;
            this.chkHrunDC.Checked = Utils.GetBooleanSetting("horizontalDC");
            this.chkVrunDC.Checked = Utils.GetBooleanSetting("verticalDC");
            this.cmbVchannel.SelectedIndex = Utils.GetIntSetting("verticalChannel", 0);
            this.cmbHchannel.SelectedIndex = Utils.GetIntSetting("horizontalChannel", 2);
            this.tbRFgain_ValueChanged(null, null);
            int centerStep = Utils.GetIntSetting("centerStep", -1);
            if (centerStep >= 0)
            {
                this.cmbCenterStep.SelectedIndex = centerStep;
            }
            this.chkLock.Checked = Utils.GetBooleanSetting("chkLock");
            this.chkAver.Checked = Utils.GetBooleanSetting("chkAver");
            this.chk1.Checked = false;
            this.chkFastConv.Checked = true;
            this.tbAverage.Value = Utils.GetIntSetting("tbAverage", 400);
            this.tbGain.Value = Utils.GetIntSetting("tbGain", 20);
            this.tbAverage_ValueChanged(null, null);
            this.tbGain_ValueChanged(null, null);
            this.chkIndepSideband.Checked = Utils.GetBooleanSetting("IndepSideband");
            this.chkAutoSize.Checked = Utils.GetBooleanSetting("AutoResize");
            this.chkBaseBand.Checked = false;
            this.fastFftCheckBox.Checked = Utils.GetBooleanSetting("FastFFT");
            this.scrollPanel.Visible = this.chkScrollPanel.Checked;
            this.chkNLimiter.Checked = false;
            this.tbNLTreshold.Value = Utils.GetIntSetting("NlTreshold", -100);
            this.tbNLRatio.Value = Utils.GetIntSetting("NlRatio", 50);
            this.tbvCarrierAvg.Value = Utils.GetIntSetting("carrierAvg", 0);
            this.tbvAudioRel.Value = Utils.GetIntSetting("audioRel", 0);
            this.tbvAudioAvg.Value = Utils.GetIntSetting("audioAvg", 0);
            this.tbvPeakDelay.Value = Utils.GetIntSetting("peakDelay", 0);
            this.tbvPeakRel.Value = Utils.GetIntSetting("peakRel", 0);
            this.btnShowLog.Checked = Utils.GetBooleanSetting("showLog");
            this.fftAverageUpDown.Value = (long)Utils.GetIntSetting("FFTaverage", 0);
            this.gBexpand_CheckedChanged(null, null);
            this.gBexpandScope_CheckedChanged(null, null);
            this._maxStations = Utils.GetIntSetting("StationlistMax", 10);
            this.butVfoA.Tag = Utils.GetStringSetting("vfoA", "");
            this.butVfoB.Tag = Utils.GetStringSetting("vfoB", "");
            this.butVfoC.Tag = Utils.GetStringSetting("vfoC", "");
            this.audioButton.NoKeyDown = true;
            Utils.Log("GUI initialized.", false);
            this.LoadFrequencyList();
            Utils.Log("Frequency list loaded.", false);
            NameValueCollection frontendPlugins = (NameValueCollection)ConfigurationManager.GetSection("frontendPlugins");
            foreach (object obj in frontendPlugins.Keys)
            {
                string key = (string)obj;
                try
                {
                    Utils.Log("    Frontend plugin " + key + " found.", false);
                    if (!(key == "SDR-IQ"))
                    {
                        string fullyQualifiedTypeName = frontendPlugins[key];
                        string[] patterns = fullyQualifiedTypeName.Split(new char[]
                        {
                            ','
                        });
                        string typeName = patterns[0];
                        string assemblyName = patterns[1];
                        ObjectHandle objectHandle = Activator.CreateInstance(assemblyName, typeName);
                        IFrontendController controller = (IFrontendController)objectHandle.Unwrap();
                        this._frontendControllers.Add(key, controller);
                        this.iqSourceComboBox.Items.Add(key);
                    }
                }
                catch (Exception ex)
                {
                    Utils.Log("Error loading '" + frontendPlugins[key] + "' - " + ex.Message, false);
                    MessageBox.Show("Error loading '" + frontendPlugins[key] + "' - " + ex.Message);
                }
            }

            string[] extIOs = Directory.GetFiles(".", "ExtIO_*.dll");

            bool? SDRSharpMain = UnmanagedDllIs64Bit(Application.ExecutablePath);
            if (SDRSharpMain.HasValue)
            {
                if ((bool)SDRSharpMain)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "x32";
            }

            foreach (string extIO in extIOs)
            {
                if (extIO.Contains(arch) == true)
                {
                    try
                    {
                        ExtIOController controller2 = new ExtIOController(extIO);
                        Utils.Log("    ExtIO controller " + Path.GetFileName(extIO) + " found.", false);
                        string displayName = Path.GetFileName(extIO).Replace(".dll", "");
                        this._frontendControllers.Add(displayName, controller2);
                        this.iqSourceComboBox.Items.Add(displayName);
                    }
                    catch (Exception ex2)
                    {
                        Utils.Log("Error loading '" + Path.GetFileName(extIO) + "'\r\n" + ex2.Message, false);
                        MessageBox.Show("Error loading '" + Path.GetFileName(extIO) + "'\r\n" + ex2.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
            ExtIO.SampleRateChanged += this.ExtIO_SampleRateChanged;
            ExtIO.LOFreqChanged += this.ExtIO_LOFreqChanged;
            this.iqSourceComboBox.Items.Add("Recording (*.wav)");
            this.iqSourceComboBox.Items.Add("Other (Soundcard)");
            Utils.Log("Plugins initialized.", false);
            this._waveFile = Utils.GetStringSetting("waveFile", string.Empty);
            this.Text = MainForm._baseTitle;
            if (this.iqSourceComboBox.SelectedIndex != this.iqSourceComboBox.Items.Count - 2)
            {
                this.centerFreqNumericUpDown.Value = Utils.GetLongSetting("centerFrequency", this.centerFreqNumericUpDown.Value);
                this.centerFreqNumericUpDown_ValueChanged(null, null);
                long vfo = Utils.GetLongSetting("vfo", this.centerFreqNumericUpDown.Value);
                if (vfo >= this.frequencyNumericUpDown.Minimum && vfo <= this.frequencyNumericUpDown.Maximum)
                {
                    this.frequencyNumericUpDown.Value = vfo;
                }
                else
                {
                    this.frequencyNumericUpDown.Value = this.centerFreqNumericUpDown.Value + this._frequencyShift;
                }
            }
            Utils.Log("IQ source selected.", false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.TuneThreadProc));
            string vfoSel = Utils.GetStringSetting("vfoSel", "");
            if (vfoSel == "A")
            {
                this.butVfoA.Checked = true;
            }
            if (vfoSel == "B")
            {
                this.butVfoB.Checked = true;
            }
            if (vfoSel == "C")
            {
                this.butVfoC.Checked = true;
            }
            this.filterAudioCheckBox.Checked = false;
            this._vfo.FilterAudio = false;

            this._initializing = false;

        }
        #endregion

        #region [Misc 2]
        public void GetColorSettings()
        {
            string name = "backgrounds";
            int[] defaultValue = new int[5];
            this._backgrounds = Utils.GetIntArraySetting(name, defaultValue);
            string name2 = "traceColors";
            int[] defaultValue2 = new int[5];
            this._traceColors = Utils.GetIntArraySetting(name2, defaultValue2);
            string name3 = "spectrumFill";
            int[] defaultValue3 = new int[5];
            this._spectrumFill = Utils.GetIntArraySetting(name3, defaultValue3);
            for (int i = 0; i <= this._saBlends.GetUpperBound(0); i++)
            {
                this._saBlends[i] = Utils.GetGradientBlend(255, "saBlends" + i.ToString());
                this._wfBlends[i] = Utils.GetGradientBlend(255, "wfBlends" + i.ToString());
                this._agBlends[i] = Utils.GetGradientBlend(255, "agBlends" + i.ToString());
            }
            this._saBlendIndex = Utils.GetIntSetting("saGradient", 1);
            this._wfBlendIndex = Utils.GetIntSetting("wfGradient", 1);
            this._agBlendIndex = Utils.GetIntSetting("agGradient", 1);
            this.spectrumAnalyzer.SpectrumFill = this._spectrumFill[this._saBlendIndex - 1];
            this.spectrumAnalyzer.SpectrumColor = Color.FromArgb(this._traceColors[this._saBlendIndex - 1]);
            this.spectrumAnalyzer.BackgroundColor = Color.FromArgb(this._backgrounds[this._saBlendIndex - 1]);
            this.spectrumAnalyzer.GradientColorBlend = this._saBlends[this._saBlendIndex - 1];
            this.ifAnalyzer.GradientColorBlend = this._saBlends[this._saBlendIndex - 1];
            this.afAnalyzer.GradientColorBlend = this._saBlends[this._saBlendIndex - 1];
            this.scope.SpectrumFill = this.spectrumAnalyzer.SpectrumFill;
            this.scope.TraceColor = this.spectrumAnalyzer.SpectrumColor;
            this.scope.BackgoundColor = this.spectrumAnalyzer.BackgroundColor;
            this.scope.GradientColorBlend = this._saBlends[this._saBlendIndex - 1];
            this.wideScope.GradientColorBlend = this._saBlends[this._saBlendIndex - 1];
            this.waterfall.BackgroundColor = this.spectrumAnalyzer.BackgroundColor;
            this.ifWaterfall.BackgroundColor = this.waterfall.BackgroundColor;
            this.afWaterfall.BackgroundColor = this.waterfall.BackgroundColor;
            this.audiogram.BackgroundColor = this.waterfall.BackgroundColor;
            this.waterfall.GradientColorBlend = this._wfBlends[this._wfBlendIndex - 1];
            this.ifWaterfall.GradientColorBlend = this.waterfall.GradientColorBlend;
            this.afWaterfall.GradientColorBlend = this.waterfall.GradientColorBlend;
            this.audiogram.GradientColorBlend = this._agBlends[this._agBlendIndex - 1];
        }

        public void PutColorSettings()
        {
            Utils.SaveSetting("saGradient", this._saBlendIndex.ToString());
            Utils.SaveSetting("wfGradient", this._wfBlendIndex.ToString());
            Utils.SaveSetting("agGradient", this._agBlendIndex.ToString());
            for (int i = 0; i <= this._saBlends.GetUpperBound(0); i++)
            {
                Utils.SaveSetting("saBlends" + i.ToString(), MainForm.GradientToString(this._saBlends[i].Colors));
                Utils.SaveSetting("wfBlends" + i.ToString(), MainForm.GradientToString(this._wfBlends[i].Colors));
                Utils.SaveSetting("agBlends" + i.ToString(), MainForm.GradientToString(this._agBlends[i].Colors));
            }
            Utils.SaveSetting("spectrumFill", Utils.IntArrayToString(this._spectrumFill));
            Utils.SaveSetting("traceColors", Utils.IntArrayToString(this._traceColors));
            Utils.SaveSetting("backgrounds", Utils.IntArrayToString(this._backgrounds));
        }

        private void ExtIO_LOFreqChanged(int frequency)
        {
            base.BeginInvoke(new Action(delegate
            {
                if (this._streamControl.SampleRate == 0.0)
                {
                    return;
                }
                if (Math.Abs(this.centerFreqNumericUpDown.Value - (long)frequency) < 10L)
                {
                    return;
                }
                this._extioChangingFrequency = true;
                this.centerFreqNumericUpDown.Value = (long)frequency;
                this._extioChangingFrequency = false;
            }));
        }

        private void ExtIO_SampleRateChanged(int newSamplerate)
        {
            base.BeginInvoke(new Action(delegate
            {
                if (this._streamControl.IsPlaying)
                {
                    this._extioChangingSamplerate = true;
                    try
                    {
                        this._streamControl.Stop();
                        this.Open();
                        this._streamControl.Play();
                    }
                    finally
                    {
                        this._extioChangingSamplerate = false;
                    }
                }
            }));
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            this._terminated = true;
            GradientDialog.CloseGradient();
            this.StopRadio();
            if (this._frontendController != null)
            {
                this._frontendController.Close();
                this._frontendController = null;
            }
            foreach (string item in this.iqSourceComboBox.Items)
            {
                string name = item.ToString();
                if (name.IndexOf("extio", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    this._frontendController = this._frontendControllers[name];
                    this._frontendController.Close();
                }
            }
            foreach (ISharpPlugin plugin in this._sharpPlugins.Values)
            {
                plugin.Close();
            }
            Utils.SaveSetting("spectrumAnalyzerAttack", this.spectrumAnalyzer.Attack);
            Utils.SaveSetting("spectrumAnalyzerDecay", this.spectrumAnalyzer.Decay);
            Utils.SaveSetting("waterfallAttack", this.waterfall.Attack);
            Utils.SaveSetting("waterfallDecay", this.waterfall.Decay);
            Utils.SaveSetting("removeDC", this.remDcSlider.Value);
            Utils.SaveSetting("useTimeMarkers", (!this.useTimestampsCheckBox.Checked) ? 0 : ((this.useTimestampsCheckBox.Text == "Time") ? 1 : 2));
            Utils.SaveSetting("fftSpeed", this.fftSpeedTrackBar.Value);
            Utils.SaveSetting("agSpeed", this.tbAgSpeed.Value);
            Utils.SaveSetting("fftWindowType", this.fftWindowComboBox.SelectedIndex);
            Utils.SaveSetting("fftResolution", this.fftResolutionComboBox.SelectedIndex);
            Utils.SaveSetting("cwShift", this.cwShiftNumericUpDown.Value);
            Utils.SaveSetting("detectorType", (int)this.DetectorType);
            Utils.SaveSetting("useAGC", this.agcCheckBox.Checked);
            Utils.SaveSetting("agcThreshold", (int)this.agcThresholdNumericUpDown.Value);
            Utils.SaveSetting("agcDecay", this.agcDecayNumericUpDown.Value);
            Utils.SaveSetting("agcSlope", (int)this.agcSlopeNumericUpDown.Value);
            Utils.SaveSetting("agcHang", this.agcUseHangCheckBox.Checked);
            Utils.SaveSetting("snapToGrid", this.snapFrequencyCheckBox.Checked);
            Utils.SaveSetting("frequencyShift", this.frequencyShiftNumericUpDown.Value);
            Utils.SaveSetting("frequencyShiftEnabled", this.frequencyShiftCheckBox.Checked);
            Utils.SaveSetting("swapIQ", this.swapIQCheckBox.Checked);
            Utils.SaveSetting("correctIQ", this.correctIQCheckBox.Checked);
            Utils.SaveSetting("IQgain", this._iqBalancer.Gain);
            Utils.SaveSetting("IQphase", this._iqBalancer.Phase);
            Utils.SaveSetting("markPeaks", this.markPeaksCheckBox.Checked);
            Utils.SaveSetting("fmStereo", this.fmStereoCheckBox.Checked);
            Utils.SaveSetting("latency", (int)this.latencyNumericUpDown.Value);
            if (this.labSampling.Text.Substring(0, 5) == "Input")
            {
                Utils.SaveSetting("sampleRate", this.sampleRateComboBox.Text.Replace(" s/sec", ""));
            }
            else
            {
                Utils.SaveSetting("minOutputSampleRate", this.sampleRateComboBox.Text.Replace(" s/sec", ""));
            }
            Utils.SaveSetting("audioGain", this.audioGainTrackBar.Value);
            Utils.SaveSetting("zoomIndex", this.fftZoomCombo.SelectedIndex);
            Utils.SaveSetting("windowState", (int)base.WindowState);
            Utils.SaveSetting("windowPosition", Utils.IntArrayToString(new int[]
            {
                this._lastLocation.X,
                this._lastLocation.Y
            }));
            Utils.SaveSetting("windowSize", Utils.IntArrayToString(new int[]
            {
                this._lastSize.Width,
                this._lastSize.Height
            }));
            Utils.SaveSetting("collapsiblePanelStates", Utils.IntArrayToString(this.GetCollapsiblePanelStates()));
            if (base.WindowState != FormWindowState.Minimized)
            {
                Utils.SaveSetting("splitterPosition", this.panSplitContainer.SplitterDistance);
            }
            if (base.WindowState != FormWindowState.Minimized)
            {
                Utils.SaveSetting("splitter2Position", this.panSplitContainer2.SplitterDistance);
            }
            if (base.WindowState != FormWindowState.Minimized)
            {
                Utils.SaveSetting("splitter3Position", this.panSplitContainer3.SplitterDistance);
            }
            if (base.WindowState != FormWindowState.Minimized)
            {
                Utils.SaveSetting("splitter4Position", this.panSplitContainer4.SplitterDistance);
            }
            if (base.WindowState != FormWindowState.Minimized)
            {
                Utils.SaveSetting("splitter5Position", this.panSplitContainer5.SplitterDistance);
            }
            Utils.SaveSetting("iqSource", this.iqSourceComboBox.SelectedIndex);
            Utils.SaveSetting("waveFile", this._waveFile ?? "");
            Utils.SaveSetting("vfo", (long)this.frequencyNumericUpDown.Value);
            Utils.SaveSetting("inputDevice", this.inputDeviceComboBox.SelectedItem);
            Utils.SaveSetting("outputDevice", this.outputDeviceComboBox.SelectedItem);
            Utils.SaveSetting("floorSA", this.tbFloorSA.Value);
            Utils.SaveSetting("spanSA", this.tbSpanSA.Value);
            Utils.SaveSetting("contrastWV", this.tbContrastWv.Value);
            Utils.SaveSetting("intensityWV", this.tbIntensityWv.Value);
            Utils.SaveSetting("contrastAG", this.tbContrastAg.Value);
            Utils.SaveSetting("intensityAG", this.tbIntensityAg.Value);
            Utils.SaveSetting("dBm", this.cmbDbm.SelectedIndex);
            Utils.SaveSetting("dBmOffset", this.dbmOffsetUpDown.Value);
            Utils.SaveSetting("horizontalShift", (int)(this.scope.Hshift * 100f));
            Utils.SaveSetting("verticalShift", (int)(this.scope.Vshift * 100f));
            Utils.SaveSetting("triggerLevel", this.tbTrigL.Value);
            Utils.SaveSetting("horizontalDC", this.chkHrunDC.Checked);
            Utils.SaveSetting("verticalDC", this.chkVrunDC.Checked);
            Utils.SaveSetting("XYmode", this.chkXY.Checked);
            Utils.SaveSetting("horizontalTimePerDiv", this.cmbTim.SelectedIndex);
            Utils.SaveSetting("horizontalVoltsPerDiv", this.cmbHor.SelectedIndex);
            Utils.SaveSetting("verticalVoltsPerDiv", this.cmbVer.SelectedIndex);
            Utils.SaveSetting("horizontalChannel", this.cmbHchannel.SelectedIndex);
            Utils.SaveSetting("verticalChannel", this.cmbVchannel.SelectedIndex);
            Utils.SaveSetting("showWaterfall", this.chkWF.Checked);
            Utils.SaveSetting("showBaseband", this.chkIF.Checked);
            Utils.SaveSetting("showAudio", this.cmbAudio.SelectedIndex);
            Utils.SaveSetting("centerStep", this.cmbCenterStep.SelectedIndex);
            Utils.SaveSetting("chkLock", this.chkLock.Checked);
            Utils.SaveSetting("chkAver", this.chkAver.Checked);
            Utils.SaveSetting("tbAverage", this.tbAverage.Value);
            Utils.SaveSetting("tbGain", this.tbGain.Value);
            string tag = this.makeVfoTag();
            if (this.butVfoA.Checked)
            {
                this.butVfoA.Tag = tag;
            }
            else if (this.butVfoB.Checked)
            {
                this.butVfoB.Tag = tag;
            }
            else if (this.butVfoC.Checked)
            {
                this.butVfoC.Tag = tag;
            }
            Utils.SaveSetting("vfoA", this.butVfoA.Tag);
            Utils.SaveSetting("vfoB", this.butVfoB.Tag);
            Utils.SaveSetting("vfoC", this.butVfoC.Tag);
            if (this.butVfoA.Checked)
            {
                Utils.SaveSetting("vfoSel", "A");
            }
            if (this.butVfoB.Checked)
            {
                Utils.SaveSetting("vfoSel", "B");
            }
            if (this.butVfoC.Checked)
            {
                Utils.SaveSetting("vfoSel", "C");
            }
            Utils.SaveSetting("IndepSideband", this.chkIndepSideband.Checked);
            Utils.SaveSetting("AutoResize", this.chkAutoSize.Checked);
            Utils.SaveSetting("FastFFT", this.fastFftCheckBox.Checked);
            Utils.SaveSetting("NlTreshold", this.tbNLTreshold.Value);
            Utils.SaveSetting("NlRatio", this.tbNLRatio.Value);
            Utils.SaveSetting("carrierAvg", this.tbvCarrierAvg.Value);
            Utils.SaveSetting("audioRel", this.tbvAudioRel.Value);
            Utils.SaveSetting("audioAvg", this.tbvAudioAvg.Value);
            Utils.SaveSetting("peakDelay", this.tbvPeakDelay.Value);
            Utils.SaveSetting("peakRel", this.tbvPeakRel.Value);
            Utils.SaveSetting("showLog", this.btnShowLog.Checked);
            Utils.SaveSetting("FFTaverage", this.fftAverageUpDown.Value);
            if (this.butVfoA.Checked || this.butVfoB.Checked || this.butVfoC.Checked)
            {
                this._modeStates[this._vfo.DetectorType] = this.GetModeState();
                Utils.SaveSetting("stateAM", Utils.IntArrayToString(this._modeStates[DetectorType.AM]));
                Utils.SaveSetting("stateSAM", Utils.IntArrayToString(this._modeStates[DetectorType.SAM]));
                Utils.SaveSetting("stateDSB", Utils.IntArrayToString(this._modeStates[DetectorType.DSB]));
                Utils.SaveSetting("stateLSB", Utils.IntArrayToString(this._modeStates[DetectorType.LSB]));
                Utils.SaveSetting("stateUSB", Utils.IntArrayToString(this._modeStates[DetectorType.USB]));
                Utils.SaveSetting("stateCW", Utils.IntArrayToString(this._modeStates[DetectorType.CW]));
                Utils.SaveSetting("stateWFM", Utils.IntArrayToString(this._modeStates[DetectorType.WFM]));
                Utils.SaveSetting("stateNFM", Utils.IntArrayToString(this._modeStates[DetectorType.NFM]));
                Utils.SaveSetting("stateRAW", Utils.IntArrayToString(this._modeStates[DetectorType.RAW]));
            }
            this.saveSettings();
        }

        private static void loadSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<MainForm.Setting>));
            try
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\Settings.xml", FileMode.Open);
                List<MainForm.Setting> list = (List<MainForm.Setting>)serializer.Deserialize(fs);
                fs.Close();
                Utils.Settings.Clear();
                foreach (MainForm.Setting kv in list)
                {
                    Utils.Settings.Add(kv.Key, kv.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Init File 'Settings.xml' could not be loaded.\n\n" + ex.Message);
            }
        }

        private static string rv(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private void saveSettings()
        {
            List<MainForm.Setting> list = new List<MainForm.Setting>(Utils.Settings.Count);
            foreach (string key in Utils.Settings.Keys)
            {
                list.Add(new MainForm.Setting(key, Utils.Settings[key]));
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<MainForm.Setting>));
            FileStream fs = new FileStream(Application.StartupPath + "\\Settings.xml", FileMode.Create);
            serializer.Serialize(fs, list);
            fs.Close();
        }

        private void LoadFrequencyList()
        {
            int oldFreq = -1;
            string stations = "";
            this._frequencyList.Clear();
            bool fmScan = false;
            bool start = false;
            string id = "";
            int count = 0;
            try
            {
                StreamReader sr = new StreamReader(Application.StartupPath + "\\eibi.csv", Encoding.Default);
                string input;
                while ((input = sr.ReadLine()) != null)
                {
                    if (input.StartsWith("FMSCAN"))
                    {
                        fmScan = true;
                    }
                    if (fmScan)
                    {
                        if (input.StartsWith("===="))
                        {
                            start = true;
                        }
                        if (start)
                        {
                            this.ReadFMscan(input, ref oldFreq, ref id, ref count, ref stations);
                        }
                    }
                    else
                    {
                        string[] fields = input.Split(new char[]
                        {
                            ';'
                        });
                        double frequency = Utils.ValD(fields[0], 0.0);
                        if (frequency > 1610.0)
                        {
                        }
                        int intFreq = (int)frequency;
                        if (intFreq != oldFreq)
                        {
                            if (stations.Length > 0)
                            {
                                this._frequencyList.Add(oldFreq, stations);
                            }
                            oldFreq = intFreq;
                            stations = "";
                        }
                        int i = fields[4].LastIndexOf('(');
                        if (i > 0)
                        {
                            fields[4] = fields[4].Substring(0, i - 1);
                        }
                        fields[9] = fields[9].Replace("km", "");
                        fields[10] = fields[10].Replace("n", "");
                        fields[10] = fields[10].Replace("d", "");
                        fields[6] = fields[6].Replace("dB", "");
                        stations = stations + ((stations.Length == 0) ? "" : ";") + string.Join(",", fields);
                    }
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load station list file 'eibi.csv'.\n\n" + ex.Message);
            }
        }

        private bool ReadFMscan(string input, ref int oldFreq, ref string id, ref int count, ref string stations)
        {
            string[] fields = input.Split(new char[]
            {
                ','
            });
            double frequency = Utils.ValD(fields[0], 0.0);
            if (frequency > 1610.0)
            {
            }
            int intFreq = (int)frequency;
            if (intFreq != oldFreq)
            {
                if (stations.Length > 0)
                {
                    this._frequencyList.Add(oldFreq, stations);
                }
                oldFreq = intFreq;
                stations = "";
                count = 0;
            }
            string[] outp = new string[11];
            outp[4] = fields[3].Replace(';', ':');
            outp[7] = fields[5].Replace(';', ':');
            if (outp[4] + outp[7] == id)
            {
                return false;
            }
            id = outp[4] + outp[7];
            if (count > this._maxStations)
            {
                return false;
            }
            count++;
            outp[0] = fields[0];
            outp[5] = fields[1];
            outp[3] = outp[5];
            outp[9] = fields[11];
            outp[10] = fields[9];
            outp[1] = fields[15];
            outp[2] = fields[16];
            double power = Utils.ValD(outp[10], 0.001);
            int distance = Utils.Val(outp[9], 1);
            double loss = 299792.0 / frequency / (12.566370614359172 * (double)distance * 1000.0);
            double dbm = 10.0 * Math.Log10(power * 1000000.0 * loss * loss) - 70.0;
            outp[6] = ((int)dbm + 107).ToString();
            stations = stations + ((stations.Length == 0) ? "" : ";") + string.Join(",", outp);
            return true;
        }

        private void SaveStationList()
        {
            if (this._frequencyList.Count == 0)
            {
                return;
            }
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\eibi.csv", false, Encoding.Default);
            foreach (KeyValuePair<int, string> kvp in this._frequencyList)
            {
                string[] stations = kvp.Value.Split(new char[]
                {
                    ';'
                });
                for (int i = 0; i <= stations.GetUpperBound(0); i++)
                {
                    string station = stations[i].Replace(',', ';');
                    sw.WriteLine(station);
                }
            }
            sw.Close();
        }

        private int[] GetModeState()
        {
            int[] result = new int[9];
            result[0] = (int)this.cmbBandWidth.Tag;
            result[1] = this.FilterOrder;
            result[2] = (int)this.FilterType;
            result[3] = this.SquelchThreshold;
            result[4] = (this.SquelchEnabled ? 1 : 0);
            result[5] = (this.SnapToGrid ? 1 : 0);
            result[6] = this.stepSizeComboBox.SelectedIndex;
            return result;
        }

        private void SetModeState(int[] state)
        {
            this.FilterBandwidth = state[0];
            this.FilterOrder = state[1];
            this.FilterType = (WindowType)state[2];
            this.SquelchThreshold = state[3];
            this.SquelchEnabled = (state[4] != 0);
            this.SnapToGrid = (state[5] == 1);
            this.stepSizeComboBox.SelectedIndex = Math.Min(this.stepSizeComboBox.Items.Count - 1, state[6]);
            this.filterBandwidthNumericUpDown_ValueChanged(null, null);
            this.stepSizeComboBox_SelectedIndexChanged(null, null);
            this.useSquelchCheckBox_CheckedChanged(null, null);
        }

        private unsafe int ProcessBuffer(Complex* iqBuffer, float* audioBuffer, int iqLen, int audioLen)
        {
            this._iqBalancer.Process(iqBuffer, iqLen);
            if (this._xBuf == null || this._xBuf.Length != iqLen)
            {
                if (this._xBuf != null)
                {
                    this._xBuf.Dispose();
                }
                this._xBuf = UnsafeBuffer.Create(iqLen, 4);
                this._xPtr = (float*)this._xBuf;
            }
            if (this._yBuf == null || this._yBuf.Length != iqLen)
            {
                if (this._yBuf != null)
                {
                    this._yBuf.Dispose();
                }
                this._yBuf = UnsafeBuffer.Create(iqLen, 4);
                this._yPtr = (float*)this._yBuf;
            }
            if (this._audioBuf == null || this._audioBuf.Length != audioLen)
            {
                if (this._audioBuf != null)
                {
                    this._audioBuf.Dispose();
                }
                this._audioBuf = UnsafeBuffer.Create(audioLen, sizeof(Complex));
                this._audioPtr = (Complex*)this._audioBuf;
            }
            if (this._envBuf == null || this._envBuf.Length != iqLen)
            {
                if (this._envBuf != null)
                {
                    this._envBuf.Dispose();
                }
                this._envBuf = UnsafeBuffer.Create(iqLen, 4);
                this._envPtr = (float*)this._envBuf;
            }
            if (this._vfoHookManager != null)
            {
                this._vfoHookManager.ProcessRawIQ(iqBuffer, iqLen);
            }
            if (this._fftSkips < -2)
            {
                this._fftSkips++;
            }
            if (this.spectrumAnalyzer.DataType == DataType.RF)
            {
                if (!this._extioChangingSamplerate && this._fftStream.Length < this._maxIQSamples)
                {
                    this._fftStream.Write(iqBuffer, iqLen);
                    if (this._fftBufferIsWaiting)
                    {
                        this._fftBufferIsWaiting = false;
                        this._fftEvent.Set();
                    }
                    if (this._speedError)
                    {
                        this.labSpeed.BackColor = this.BackColor;
                    }
                    this._speedError = false;
                }
                else if (base.WindowState != FormWindowState.Minimized && this._isActive && this._fftStream.Length > 0)
                {
                    Console.WriteLine("_fftStream reached max of " + this._maxIQSamples.ToString() + ", fftSkips=" + this._fftSkips.ToString());
                    this._fftSkips = Math.Min(this._fftSkips + 1, 40);
                    if (this._fftSkips > 0)
                    {
                        if (!this._speedError)
                        {
                            this.labSpeed.BackColor = Color.Red;
                        }
                        this._speedError = true;
                    }
                }
            }

            if (this.wideScope.Visible)
            {
                this._envPtr = (float*)this._envBuf;
            }
            else
            {
                this._envPtr = null;
            }

            int basebandLen = this._vfo.ProcessBuffer(iqBuffer, audioBuffer, iqLen, this._xPtr, this._yPtr, this._envPtr);
            if (this.amRadioButton.Checked && this.chkNLimiter.Checked)
            {
                this._nLimiter.process(audioBuffer, audioLen);
            }
            int audioSize = this._streamControl.AudioStreamSize;
            if (this._streamControl.SoundCardRatio > 1.0 && audioSize > 0)
            {
                int min = (int)((double)audioLen / this._streamControl.SoundCardRatio);
                if (audioSize < min && this._AsioCorrection > 0f)
                {
                    this._AsioCorrection = 0f;
                    Console.WriteLine("AudioStreamSize < " + min.ToString() + ", correction=0");
                }
                else if (audioSize > 2 * min && this._AsioCorrection == 0f)
                {
                    this._AsioCorrection = 0.001f;
                    Console.WriteLine("AudioStreamSize > 2 * " + min.ToString() + ", correction=-" + this._AsioCorrection.ToString());
                }
                audioLen = this._fractResampler.Resample(audioLen, this._streamControl.SoundCardRatio + (double)this._AsioCorrection, audioBuffer, audioBuffer);
            }
            if (this.spectrumAnalyzer.DataType == DataType.IF)
            {
                if (!this._extioChangingSamplerate && this._fftStream.Length < this._maxIQSamples)
                {
                    this._fftStream.Write(iqBuffer, basebandLen);
                    if (this._fftBufferIsWaiting)
                    {
                        this._fftBufferIsWaiting = false;
                        this._fftEvent.Set();
                    }
                }
            }
            else if (this.spectrumAnalyzer.DataType == DataType.AF && !this._extioChangingSamplerate && this._fftStream.Length < this._maxIQSamples)
            {
                this.audioToComplex(audioBuffer, this._audioPtr, audioLen);
                this._fftStream.Write(this._audioPtr, audioLen / 2);
                if (this._fftBufferIsWaiting)
                {
                    this._fftBufferIsWaiting = false;
                    this._fftEvent.Set();
                }
            }
            if (!this._extioChangingSamplerate && this._iftStream.Length < this._maxIFSamples)
            {
                this._iftStream.Write(iqBuffer, basebandLen);
            }
            if (!this._extioChangingSamplerate && this._aftStream.Length < this._maxAFSamples)
            {
                this.audioToComplex(audioBuffer, this._audioPtr, audioLen);
                this._aftStream.Write(this._audioPtr, audioLen / 2);
            }
            DemodType demodX = DemodType.Empty;
            DemodType demodY = DemodType.Empty;
            if (this.scopeCollapsiblePanel.PanelState == PanelStateOptions.Expanded || this.wideScope.Visible)
            {
                demodX = this.scope.Hchannel;
                if (this._vfo.DemodX != demodX)
                {
                    this._vfo.DemodX = demodX;
                }
                demodY = this.scope.Vchannel;
                if (this._vfo.DemodY != demodY)
                {
                    this._vfo.DemodY = demodY;
                }
            }
            this.chkLock.Enabled = (demodX == DemodType.PM || demodY == DemodType.PM);
            if (this.chkLock.Enabled && this.chkLock.Checked)
            {
                if (this.frequencyNumericUpDown.InvokeRequired)
                {
                    this.frequencyNumericUpDown.BeginInvoke(new MethodInvoker(delegate
                    {
                        this.DoVfoCorrection();
                    }));
                }
                else
                {
                    this.DoVfoCorrection();
                }
            }
            if (demodX == DemodType.Audio)
            {
                int i = 0;
                for (int j = 0; j < audioLen; j += 2)
                {
                    this._xPtr[(int)(IntPtr)(i++) * 4] = audioBuffer[j];
                }
            }
            if (demodY == DemodType.Audio)
            {
                int k = 0;
                for (int l = 0; l < audioLen; l += 2)
                {
                    this._yPtr[(int)(IntPtr)(k++) * 4] = audioBuffer[l];
                }
            }
            int timeOut = 1;
            if (this.scopeCollapsiblePanel.PanelState == PanelStateOptions.Expanded)
            {
                int len = iqLen >> ((demodY == DemodType.Envelope) ? this._vfo.EnvelopeDecimationStageCount : this._vfo.BaseBandDecimationStageCount);
                if (this.scope.InvokeRequired)
                {
                    this.scope.BeginInvoke(new MethodInvoker(delegate
                    {
                        this.scope.Render(this._xPtr, this._yPtr, len, this._streamControl.BufferSizeInMs, timeOut);
                    }));
                }
                else
                {
                    this.scope.Render(this._xPtr, this._yPtr, len, this._streamControl.BufferSizeInMs, timeOut);
                }
            }
            if (this._envPtr != null)
            {
                int elen = iqLen >> this._vfo.EnvelopeDecimationStageCount;
                if (this.wideScope.InvokeRequired)
                {
                    this.wideScope.BeginInvoke(new MethodInvoker(delegate
                    {
                        this.wideScope.Render(this._xPtr, this._envPtr, elen, this._streamControl.BufferSizeInMs, timeOut);
                    }));
                }
                else
                {
                    this.wideScope.Render(this._xPtr, this._envPtr, elen, this._streamControl.BufferSizeInMs, timeOut);
                }
            }
            if (this._streamControl.IsPlaying && this.AdvancedCollapsiblePanel.PanelState == PanelStateOptions.Expanded && this.barMeter.Draw(this._streamControl.AudioStreamSize, 10))
            {
                this.labCPU.Text = this._streamControl.AudioStreamSize.ToString();
            }
            if (!this.audioButton.Checked)
            {
                for (int m = 0; m < audioLen; m++)
                {
                    audioBuffer[m] = 0f;
                }
            }
            return audioLen;
        }

        private void logMinMax(int size)
        {
            if (this._upwards && size < this._prevSize)
            {
                this._upwards = false;
                this._maxSize = this._prevSize;
                Console.WriteLine("AudioStream MAX=" + this._maxSize.ToString());
            }
            else if (size > this._prevSize)
            {
                this._upwards = true;
                this._minSize = this._prevSize;
                Console.WriteLine("AudioStream MIN=" + this._minSize.ToString());
            }
            this._prevSize = size;
        }

        private unsafe void audioToComplex(float* audioPtr, Complex* cpxPtr, int len)
        {
            int i = 0;
            for (int j = 0; j < len; j += 2)
            {
                if ((double)Math.Abs(audioPtr[j]) < 1E-20)
                {
                    cpxPtr[i].Real = 0f;
                }
                else
                {
                    cpxPtr[i].Real = audioPtr[j];
                }
                cpxPtr[i++].Imag = 0f;
            }
        }

        private void DoVfoCorrection()
        {
            if (this._terminated)
            {
                return;
            }
            long newFreq = (long)(this.frequencyNumericUpDown.Value - (decimal)this._vfo.FreqErr * 100m);
            if (newFreq >= this.frequencyNumericUpDown.Minimum && newFreq <= this.frequencyNumericUpDown.Maximum)
            {
                this.frequencyNumericUpDown.Value = newFreq;
            }
        }

        private void ProcessFFT(object parameter)
        {
            DataType dataType = DataType.none;
            int prevInterval = 0;
            this._actualFftBins = 0;
            this._actualIftBins = 0;
            this._actualAftBins = 0;
            while (this._streamControl.IsPlaying || this._extioChangingSamplerate)
            {
                long elap = this._processTmr.Elaps(100);
                if (elap > 0L)
                {
                    this.labProcessTmr.Text = elap.ToString();
                }
                if (this._fftTimer.Interval * this._waterfallTimout != prevInterval)
                {
                    prevInterval = this._fftTimer.Interval * this._waterfallTimout;
                }
                if (Utils.FastFFT)
                {
                    Application.DoEvents();
                }
                int sampleRate = (int)this._streamControl.SampleRate;
                int ifSampleRate = sampleRate >> this._vfo.BaseBandDecimationStageCount;
                int afSampleRate = sampleRate >> this._vfo.DecimationStageCount;
                if (this._streamControl.SoundCardRatio > 1.0)
                {
                    afSampleRate = (int)((double)afSampleRate / this._streamControl.SoundCardRatio);
                }
                if (dataType == DataType.IF)
                {
                    sampleRate = ifSampleRate;
                }
                else if (dataType == DataType.AF)
                {
                    sampleRate = afSampleRate;
                }
                if (this.spectrumAnalyzer.DataType != dataType)
                {
                    dataType = this.spectrumAnalyzer.DataType;
                    this._fftStream.Flush();
                    this._actualFftBins = 0;
                    Console.WriteLine("ProcessFFT DataType changed, samplerate=" + sampleRate.ToString());
                    Console.WriteLine("ProcessIFT Samplerate=" + ifSampleRate.ToString());
                    Console.WriteLine("ProcessAFT Samplerate=" + afSampleRate.ToString());
                }
                this.readAndRunFFT(sampleRate);
                if (this.ifAnalyzer.Visible)
                {
                    this.readAndRunIFT(ifSampleRate);
                }
                if (this.afAnalyzer.Visible || this.audiogram.Visible)
                {
                    this.readAndRunAFT(afSampleRate);
                }
                if (Utils.FastFFT)
                {
                    this.renderFFT(sampleRate);
                    this.renderIFT(ifSampleRate);
                    this.renderAFT(afSampleRate);
                    this.getWaterfallSpeed();
                }
            }
            this._fftStream.Flush();
            this._iftStream.Flush();
            this._aftStream.Flush();
            this._actualFftBins = 0;
            this._actualIftBins = 0;
            this._actualAftBins = 0;
            Console.WriteLine("ProcessFFT stopped");
        }

        private unsafe void readAndRunFFT(int sampleRate)
        {
            if (this._actualFftBins != this._fftBins)
            {
                for (int i = 0; i < this._fftBins; i++)
                {
                    this._iqPtr[i].Real = 0f;
                    this._iqPtr[i].Imag = 0f;
                }
                this._actualFftBins = this._fftBins;
            }
            float time = Utils.FastFFT ? 0.0005f : 0.001f;
            int samplesToRead = (int)((float)(sampleRate * this._fftTimer.Interval) * time);
            int samplesToUse = Math.Min(samplesToRead, this._actualFftBins);
            int excessSamples = samplesToRead - samplesToUse;
            this._maxIQSamples = (int)((double)(sampleRate * this._streamControl.BufferSizeInMs) * 0.0015);
            if (samplesToUse < this._actualFftBins)
            {
                Utils.Memcpy((void*)this._iqPtr, (void*)(this._iqPtr + samplesToUse), (this._actualFftBins - samplesToUse) * sizeof(Complex));
            }
            int total = 0;
            while (this._streamControl.IsPlaying && total < samplesToUse)
            {
                total += this._fftStream.Read(this._iqPtr, this._actualFftBins - samplesToUse + total, samplesToUse - total);
            }
            if (excessSamples > 0)
            {
                this._fftStream.Advance(excessSamples);
            }
            if (!this._rfSpectrumAvailable && (!Utils.FastFFT || --this._fftSkipCnt < 0))
            {
                this._fftSkipCnt = this._fftSkips;
                DataType dataType = this.spectrumAnalyzer.DataType;
                float compensation = 8f - this._fftGain + (float)this.dbmOffsetUpDown.Value;
                if (dataType == DataType.IF)
                {
                    compensation = 49f - this._bftGain + (float)this.dbmOffsetUpDown.Value;
                    if (this.wfmRadioButton.Checked)
                    {
                        compensation -= 35f;
                    }
                }
                else if (dataType == DataType.AF)
                {
                    compensation = 65f - this._bftGain;
                }
                Utils.Memcpy((void*)this._fftPtr, (void*)this._iqPtr, this._actualFftBins * sizeof(Complex));
                if (dataType == DataType.RF)
                {
                    Fourier.ApplyFFTWindow(this._fftPtr, this._fftWindowPtr, this._actualFftBins);
                }
                else
                {
                    Fourier.ApplyFFTWindow(this._fftPtr, this._bftWindowPtr, this._actualFftBins);
                }
                Fourier.ForwardTransform(this._fftPtr, this._actualFftBins, true);
                Fourier.SpectrumPower(this._fftPtr, this._rfSpectrumPtr, this._actualFftBins, compensation, false);
                if (this.fftAverageUpDown.Value > 0L)
                {
                    int avg = (int)this.fftAverageUpDown.Value + 1;
                    float level = this.spectrumAnalyzer.MinPower + (this.spectrumAnalyzer.MaxPower - this.spectrumAnalyzer.MinPower) / 5f;
                    for (int j = 0; j < this._actualFftBins; j++)
                    {
                        if (this._rfAveragePtr[j] > level)
                        {
                            this._rfAveragePtr[j] = this._rfSpectrumPtr[j];
                        }
                        else
                        {
                            this._rfAveragePtr[j] = ((float)(avg - 1) * this._rfAveragePtr[j] + this._rfSpectrumPtr[j]) / (float)avg;
                        }
                    }
                    Utils.Memcpy((void*)this._rfSpectrumPtr, (void*)this._rfAveragePtr, this._actualFftBins * 4);
                }
                this._rfSpectrumSamples = this._actualFftBins;
                this._rfSpectrumAvailable = true;
            }
            if (this._fftStream.Length <= this._maxIQSamples)
            {
                this._fftBufferIsWaiting = true;
                this._fftEvent.WaitOne();
            }
        }

        private unsafe void readAndRunIFT(int ifSampleRate)
        {
            if (this._actualIftBins != this._bftBins)
            {
                for (int i = this._actualIftBins; i < this._bftBins; i++)
                {
                    this._ifqPtr[i].Real = 0f;
                    this._ifqPtr[i].Imag = 0f;
                }
                this._actualIftBins = this._bftBins;
            }
            float time = Utils.FastFFT ? 0.0005f : 0.001f;
            int samplesToRead = (int)((float)(ifSampleRate * this._fftTimer.Interval) * time);
            int samplesToUse = Math.Min(samplesToRead, this._actualIftBins);
            int excessSamples = samplesToRead - samplesToUse;
            this._maxIFSamples = (int)((double)(ifSampleRate * this._streamControl.BufferSizeInMs) * 0.0015);
            if (samplesToUse < this._actualIftBins)
            {
                Utils.Memcpy((void*)this._ifqPtr, (void*)(this._ifqPtr + samplesToUse), (this._actualIftBins - samplesToUse) * sizeof(Complex));
            }
            int total = 0;
            while (this._streamControl.IsPlaying && total < samplesToUse)
            {
                total += this._iftStream.Read(this._ifqPtr, this._actualIftBins - samplesToUse + total, samplesToUse - total);
            }
            if (excessSamples > 0)
            {
                this._iftStream.Advance(excessSamples);
            }
            if (!this._ifSpectrumAvailable && (!Utils.FastFFT || this._fftSkipCnt == this._fftSkips))
            {
                float compensation = 51f - this._bftGain + (float)this.dbmOffsetUpDown.Value;
                if (this.wfmRadioButton.Checked)
                {
                    compensation -= 35f;
                }
                else if (this.rawRadioButton.Checked)
                {
                    compensation -= 48f;
                }
                Utils.Memcpy((void*)this._iftPtr, (void*)this._ifqPtr, this._actualIftBins * sizeof(Complex));
                Fourier.ApplyFFTWindow(this._iftPtr, this._bftWindowPtr, this._actualIftBins);
                Fourier.ForwardTransform(this._iftPtr, this._actualIftBins, true);
                Fourier.SpectrumPower(this._iftPtr, this._iftSpectrumPtr, this._actualIftBins, compensation, false);
                this._ifSpectrumSamples = this._actualIftBins;
                this._ifSpectrumAvailable = true;
            }
        }

        private unsafe void readAndRunAFT(int afSampleRate)
        {
            if (this._actualAftBins != this._bftBins)
            {
                for (int i = this._actualAftBins; i < this._bftBins; i++)
                {
                    this._afqPtr[i].Real = 0f;
                    this._afqPtr[i].Imag = 0f;
                }
                this._actualAftBins = this._bftBins;
            }
            float time = Utils.FastFFT ? 0.0005f : 0.001f;
            int samplesToRead = (int)((float)(afSampleRate * this._fftTimer.Interval) * time);
            int samplesToUse = Math.Min(samplesToRead, this._actualAftBins);
            int excessSamples = samplesToRead - samplesToUse;
            this._maxAFSamples = (int)((double)(afSampleRate * this._streamControl.BufferSizeInMs) * 0.0015);
            if (samplesToUse < this._actualAftBins)
            {
                Utils.Memcpy((void*)this._afqPtr, (void*)(this._afqPtr + samplesToUse), (this._actualAftBins - samplesToUse) * sizeof(Complex));
            }
            int total = 0;
            while (this._streamControl.IsPlaying && total < samplesToUse)
            {
                total += this._aftStream.Read(this._afqPtr, this._actualAftBins - samplesToUse + total, samplesToUse - total);
            }
            if (excessSamples > 0)
            {
                this._aftStream.Advance(excessSamples);
            }
            if (!this._afSpectrumAvailable && (!Utils.FastFFT || this._fftSkipCnt == this._fftSkips))
            {
                float compensation = 55f - this._bftGain;
                if (this.wfmRadioButton.Checked)
                {
                    compensation += 10f;
                }
                if (this.rawRadioButton.Checked)
                {
                    compensation -= 48f;
                }
                Utils.Memcpy((void*)this._aftPtr, (void*)this._afqPtr, this._actualAftBins * sizeof(Complex));
                Fourier.ApplyFFTWindow(this._aftPtr, this._bftWindowPtr, this._actualAftBins);
                Fourier.ForwardTransform(this._aftPtr, this._actualAftBins, true);
                Fourier.SpectrumPower(this._aftPtr, this._afSpectrumPtr, this._actualAftBins, compensation, true);
                this._afSpectrumSamples = this._actualAftBins;
                this._afSpectrumAvailable = true;
            }
        }

        private unsafe void renderFFT(int sampleRate)
        {
            bool render = this._streamControl.IsPlaying;
            if (!this.spectrumAnalyzer.Visible)
            {
                render = false;
            }
            if (render)
            {
                DataType dataType = this.chkBaseBand.Checked ? DataType.IF : DataType.RF;
                if (Utils.Chk1 && dataType == DataType.IF)
                {
                    dataType = DataType.AF;
                }
                if (this.spectrumAnalyzer.DataType != dataType || (dataType != DataType.AF && this.spectrumAnalyzer.SpectrumWidth != sampleRate) || (dataType == DataType.AF && this.spectrumAnalyzer.SpectrumWidth != sampleRate / 2))
                {
                    for (int i = 1; i < this.fftZoomCombo.Items.Count; i++)
                    {
                        int spectrum = this.getFrequency(this.fftZoomCombo.Items[i]);
                        this.fftZoomCombo.setEnabled(i, spectrum <= sampleRate);
                    }
                    this.spectrumAnalyzer.DataType = dataType;
                    if (dataType == DataType.RF)
                    {
                        this.spectrumAnalyzer.SpectrumWidth = sampleRate;
                        this.spectrumAnalyzer.CenterFrequency = this.centerFreqNumericUpDown.Value + this._frequencyShift;
                        this.spectrumAnalyzer.Frequency = (long)this.frequencyNumericUpDown.Value;
                        this.spectrumAnalyzer.UseSnap = this.snapFrequencyCheckBox.Checked;
                    }
                    else if (dataType == DataType.IF)
                    {
                        this.spectrumAnalyzer.SpectrumWidth = sampleRate;
                        this.spectrumAnalyzer.Frequency = (long)this.frequencyNumericUpDown.Value;
                        if (this.spectrumAnalyzer.SpectrumWidth < this.spectrumAnalyzer.FilterBandwidth)
                        {
                            this.filterBandwidthNumericUpDown.Value = (long)(this.spectrumAnalyzer.SpectrumWidth / 1000 * 1000);
                        }
                        this.spectrumAnalyzer.UseSnap = false;
                    }
                    else
                    {
                        this.spectrumAnalyzer.SpectrumWidth = sampleRate / 2;
                        this.spectrumAnalyzer.Frequency = 0L;
                        if (this.spectrumAnalyzer.SpectrumWidth < this.spectrumAnalyzer.FilterBandwidth / 2)
                        {
                            this.filterBandwidthNumericUpDown.Value = (long)(this.spectrumAnalyzer.SpectrumWidth / 500 * 1000);
                        }
                        this.spectrumAnalyzer.UseSnap = false;
                    }
                    this.spectrumAnalyzer.MaxPower = this.spectrumAnalyzer.MaxPower;
                    this.waterfall.DataType = dataType;
                    this.waterfall.UseSnap = this.spectrumAnalyzer.UseSnap;
                    this.waterfall.CenterFrequency = this.spectrumAnalyzer.CenterFrequency;
                    this.waterfall.Frequency = this.spectrumAnalyzer.Frequency;
                    this.waterfall.SpectrumWidth = this.spectrumAnalyzer.SpectrumWidth;
                    this.labSpectrum.Text = ((float)this.spectrumAnalyzer.SpectrumWidth / 1000f).ToString() + " kHz";
                    this.fftZoomCombo_SelectedIndexChanged(null, null);
                    this._floorValueNeeded = 15;
                }
                int offset = 0;
                int samples = this._rfSpectrumSamples;
                if (dataType == DataType.AF)
                {
                    samples >>= 1;
                    offset = samples;
                }
                if (--this._rfTimout <= 0)
                {
                    this._rfTimout = this._spectrumTimeout;
                    this.spectrumAnalyzer.Render(this._rfSpectrumPtr + offset, samples);
                }
                if (this.waterfall.Visible)
                {
                    if (this.waterfall.WaveStart != this._streamControl.WaveStart)
                    {
                        this.waterfall.WaveStart = this._streamControl.WaveStart;
                    }
                    this.waterfall.RenderAndDraw(this._rfSpectrumPtr + offset, samples, this._waterfallTimout, this._fftTimer.Interval);
                }
                else
                {
                    int j = 0;
                    j++;
                }
            }
            this._rfSpectrumAvailable = false;
            if (this._fftBufferIsWaiting)
            {
                this._fftBufferIsWaiting = false;
                this._fftEvent.Set();
            }
        }

        private unsafe void renderIFT(int ifSamplerate)
        {
            bool render = this._streamControl.IsPlaying;
            if (base.WindowState == FormWindowState.Minimized)
            {
                render = false;
            }
            if (render && this.ifAnalyzer.Visible)
            {
                int samplerate = (int)((double)ifSamplerate / this._streamControl.SoundCardRatio);
                if (this.ifAnalyzer.SpectrumWidth != samplerate)
                {
                    this.ifAnalyzer.DataType = DataType.IF;
                    this.ifAnalyzer.SpectrumWidth = samplerate;
                    this.ifAnalyzer.Frequency = (long)this.frequencyNumericUpDown.Value;
                    this.ifAnalyzer.Zoom = (float)(this.chkAutoSize.Checked ? -1 : 1);
                    this.ifWaterfall.DataType = this.ifAnalyzer.DataType;
                    this.ifWaterfall.SpectrumWidth = this.ifAnalyzer.SpectrumWidth;
                    this.ifWaterfall.Frequency = this.ifAnalyzer.Frequency;
                    this.ifWaterfall.Zoom = this.ifAnalyzer.Zoom;
                    if (this._vfo.DetectorType != DetectorType.WFM && this.ifAnalyzer.SpectrumWidth < this.ifAnalyzer.FilterBandwidth)
                    {
                        this.filterBandwidthNumericUpDown.Value = (long)(this.ifAnalyzer.SpectrumWidth / 1000 * 1000);
                    }
                }
                int samples = (int)((double)this._ifSpectrumSamples / this._streamControl.SoundCardRatio);
                int offset = (this._ifSpectrumSamples - (int)((double)this._ifSpectrumSamples / this._streamControl.SoundCardRatio)) / 2;
                if (--this._ifTimeout <= 0)
                {
                    this._ifTimeout = this._spectrumTimeout;
                    this.ifAnalyzer.Render(this._iftSpectrumPtr + offset, samples);
                }
                if (this.ifWaterfall.Visible)
                {
                    this.ifWaterfall.RenderAndDraw(this._iftSpectrumPtr + offset, samples, this._waterfallTimout, this._fftTimer.Interval);
                }
            }
            this._ifSpectrumAvailable = false;
        }

        private unsafe void renderAFT(int afSamplerate)
        {
            bool render = this._streamControl.IsPlaying && this._afSpectrumSamples > 0;
            if (base.WindowState == FormWindowState.Minimized)
            {
                render = false;
            }
            if (render)
            {
                int halfTheSamples = this._afSpectrumSamples / 2;
                float* halfSpectrumPtr = this._afSpectrumPtr + halfTheSamples;
                if (this.afAnalyzer.Visible)
                {
                    if (this.afAnalyzer.SpectrumWidth != afSamplerate / 2)
                    {
                        this.afAnalyzer.DataType = DataType.AF;
                        this.afAnalyzer.SpectrumWidth = afSamplerate / 2;
                        this.afAnalyzer.CenterFrequency = (long)(afSamplerate / 4);
                        this.afAnalyzer.Frequency = 0L;
                        this.afAnalyzer.Zoom = (float)(this.chkAutoSize.Checked ? -1 : 1);
                        this.afWaterfall.DataType = this.afAnalyzer.DataType;
                        this.afWaterfall.SpectrumWidth = this.afAnalyzer.SpectrumWidth;
                        this.afWaterfall.CenterFrequency = this.afAnalyzer.CenterFrequency;
                        this.afWaterfall.Frequency = 0L;
                        this.afWaterfall.Zoom = this.afAnalyzer.Zoom;
                        if (this._vfo.DetectorType != DetectorType.WFM && this.ifAnalyzer.SpectrumWidth < this.afAnalyzer.FilterBandwidth)
                        {
                            this.filterBandwidthNumericUpDown.Value = (long)(this.ifAnalyzer.SpectrumWidth / 1000 * 1000);
                        }
                    }
                    if (--this._afTimeout <= 0)
                    {
                        this._afTimeout = this._spectrumTimeout;
                        this.afAnalyzer.Render(halfSpectrumPtr, halfTheSamples);
                    }
                    if (this.afWaterfall.Visible)
                    {
                        this.afWaterfall.RenderAndDraw(halfSpectrumPtr, halfTheSamples, this._waterfallTimout, this._fftTimer.Interval);
                    }
                }
                if (this.audiogram.Visible)
                {
                    int bw = this.FilterBandwidth;
                    if (this._vfo.DetectorType == DetectorType.CW)
                    {
                        bw = this._vfo.CWToneShift * 4;
                    }
                    else if (this.waterfall.BandType != BandType.Center)
                    {
                        bw *= 2;
                    }
                    double f = 1.0;
                    if (this.chkAutoSize.Checked)
                    {
                        f = Math.Min(1.0, (double)((float)bw / (float)afSamplerate));
                    }
                    int spectrumWidth = (int)((double)(afSamplerate / 2) * f);
                    if (this.audiogram.SpectrumWidth != spectrumWidth)
                    {
                        this.audiogram.SpectrumWidth = spectrumWidth;
                        this.audiogram.CenterFrequency = (long)(spectrumWidth / 2);
                        this.audiogram.Frequency = 0L;
                        if (spectrumWidth < 1000)
                        {
                            this.btnShowLog.Checked = false;
                        }
                    }
                    if (this.audiogram.WaveStart != this._streamControl.WaveStart)
                    {
                        this.audiogram.WaveStart = this._streamControl.WaveStart;
                    }
                    this.audiogram.RenderAndDraw(halfSpectrumPtr, (int)((double)halfTheSamples * f), this._audiogramTimout, this._fftTimer.Interval);
                }
            }
            this._afSpectrumAvailable = false;
        }

        private void performTimer_Tick(object sender, EventArgs e)
        {
            int avgCount = 30;
            long elap = this._performTmr.Elaps(100);
            if (elap > 0L)
            {
                this.labPerformTmr.Text = elap.ToString();
            }
            if (this._floorValueNeeded >= -avgCount)
            {
                this._floorValueNeeded--;
            }
            if (this._floorValueNeeded <= 0 && this._floorValueNeeded >= -avgCount && this.spectrumAnalyzer.DataType == DataType.RF)
            {
                this._fftMinimum = this.getFFTminimum(this.spectrumAnalyzer.DataType) + (this._fftGain - 84f) / 4f;
                if (this._floorValueNeeded == 0 || this._floorValueNeeded == -avgCount)
                {
                    if (this._floorValueNeeded == 0)
                    {
                        this._fftAverageMin = this._fftMinimum;
                    }
                    else
                    {
                        this._fftMinimum = this._fftAverageMin;
                    }
                    if (this.fftAverageUpDown.Value > 0L)
                    {
                        this._fftMinimum -= 5f;
                    }
                    Console.WriteLine("_fftMinumum=" + this._fftMinimum.ToString() + ", fftGain=" + this._fftGain.ToString());
                    this.tbFloorSA.Value = Math.Min(this.tbFloorSA.Maximum, Math.Max(this.tbFloorSA.Minimum, this.tbFloorSA.Minimum + this.tbFloorSA.Maximum - (int)this._fftMinimum));
                    if (this._floorValueNeeded == -avgCount)
                    {
                        this.startSpeedCalc();
                    }
                }
                else if (this._floorValueNeeded > -avgCount)
                {
                    this._fftAverageMin = 0.95f * this._fftAverageMin + 0.05f * this._fftMinimum;
                }
            }
            if (this._gainValueNeeded >= 0)
            {
                this._gainValueNeeded--;
            }
            if (this._gainValueNeeded == 1)
            {
                this.setRfGainFromSmeter();
            }
            else if (this._gainValueNeeded == 0)
            {
                this.wideScope.Reset();
            }
            this.spectrumAnalyzer.Perform();
            if (this.ifAnalyzer.Visible)
            {
                this.ifAnalyzer.Perform();
            }
            if (this.afAnalyzer.Visible)
            {
                this.afAnalyzer.Perform();
            }
            if (this.waterfall.Visible)
            {
                this.waterfall.Perform();
            }
            else
            {
                int i = 1;
                i++;
            }
            this.ifWaterfall.Perform();
            this.afWaterfall.Perform();
            this.audiogram.Perform();
            if (this.scopeCollapsiblePanel.PanelState == PanelStateOptions.Expanded)
            {
                this.scope.Perform(false);
            }
            if (this.wideScope.Visible)
            {
                this.wideScope.Perform(false);
            }
            if (this.SourceIsWaveFile)
            {
                int msec10 = (int)DateTime.Now.Subtract(this._streamControl.WaveStart).TotalMilliseconds / 10;
                if (this._streamControl.IsPlaying && this._duration > 0)
                {
                    this.playBar.Draw(msec10);
                }
            }
            if ((double)this._streamControl.ClipLevel > 0.99)
            {
                this.audioButton.ForeColor = Color.CornflowerBlue;
                return;
            }
            this.audioButton.ForeColor = Color.Lime;
        }

        private void setRfGainFromSmeter()
        {
            double gain = -68.0 - 0.95 * (double)this.spectrumAnalyzer.SignalDbm + (double)this.dbmOffsetUpDown.Value;
            if ((this.amRadioButton.Checked || this.samRadioButton.Checked || this.rawRadioButton.Checked) && this.spectrumAnalyzer.SignalDbm > -130)
            {
                this.tbRFgain.Value = (int)Math.Min((double)this.tbRFgain.Maximum, gain);
            }
        }

        private void setBftBins()
        {
            int sampleRate = (int)this._streamControl.SampleRate >> this._streamControl.DecimationStageCount;
            int bftBins;
            if (sampleRate < 16001)
            {
                bftBins = 1024;
            }
            else if (sampleRate < 32001)
            {
                bftBins = 2048;
            }
            else if (sampleRate < 64001)
            {
                bftBins = 4096;
            }
            else
            {
                bftBins = 8192;
            }
            if (this.btnShowLog.Checked)
            {
                bftBins *= 2;
            }
            for (int i = 0; i < this.bftResolutionComboBox.Items.Count - 1; i++)
            {
                if (this.getFrequency(this.bftResolutionComboBox.Items[i]) == bftBins)
                {
                    this.bftResolutionComboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private unsafe float getFFTminimum(DataType dataType)
        {
            int skip = this._actualFftBins / 10;
            if (dataType == DataType.IF)
            {
                skip = (int)((float)(this.spectrumAnalyzer.SpectrumWidth - this.FilterBandwidth) * (float)this._actualFftBins / (float)this.spectrumAnalyzer.SpectrumWidth / 2f);
            }
            float fftMin;
            float fftMax;
            Fourier.AverageMinimum(this._rfSpectrumPtr, this._actualFftBins, skip, out fftMin, out fftMax);
            return fftMin;
        }

        private void fftTimer_Tick(object sender, EventArgs e)
        {
            if (!this._streamControl.IsBuffering)
            {
                return;
            }
            if (Utils.FastFFT)
            {
                this.labFftTmr.Text = this._fftSkips.ToString();
            }
            this._fftTimer.Enabled = false;
            if (Utils.FastFFT)
            {
                this.ProcessFFT(null);
                return;
            }
            int sampleRate = (int)this._streamControl.SampleRate;
            int ifSampleRate = sampleRate >> this._vfo.BaseBandDecimationStageCount;
            int afSampleRate = sampleRate >> this._vfo.DecimationStageCount;
            if (this._streamControl.SoundCardRatio > 1.0)
            {
                afSampleRate = (int)((double)afSampleRate / this._streamControl.SoundCardRatio);
            }
            DataType dataType = this.spectrumAnalyzer.DataType;
            if (dataType == DataType.IF)
            {
                sampleRate = ifSampleRate;
            }
            else if (dataType == DataType.AF)
            {
                sampleRate = afSampleRate;
            }
            this.renderFFT(sampleRate);
            this.renderIFT(ifSampleRate);
            this.renderAFT(afSampleRate);
            this.getWaterfallSpeed();
            this._fftTimer.Enabled = true;
        }

        private void iqTimer_Tick(object sender, EventArgs e)
        {
            if (this.Text.Length == 0)
            {
                return;
            }
            string last = this.Text.Substring(this.Text.Length - 1);
            if (last == ")")
            {
                Utils.Log("iqTimer start.", false);
                this.playStopButton.SetColor(Color.Lime);
                this.Text = MainForm._baseTitle + " [" + File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd-MMM-yyyy") + "]";
            }
            else if (this.correctIQCheckBox.Checked)
            {
                this.Text = MainForm._baseTitle + string.Format(" - IQ Imbalance: Gain = {0:F3} Phase = {1:F3}°", this._iqBalancer.Gain, Math.Asin((double)this._iqBalancer.Phase) * 180.0 / 3.1415926535897931);
            }
            else if (this.SourceIsWaveFile)
            {
                string curFile = (this._streamControl.WaveFile == null) ? this._waveFile : this._streamControl.WaveFile.FileName;
                int duration = (this._streamControl.WaveFile == null) ? 0 : this._streamControl.WaveFile.Duration;
                if (!this.Text.Contains(curFile))
                {
                    if (curFile == this._waveFile)
                    {
                        this._duration = 0;
                    }
                    if (duration > 0)
                    {
                        this.playBar.DrawBackground(this._duration * 100, (this._duration + duration) * 100);
                    }
                    this._duration += duration;
                }
                this.Text = MainForm._baseTitle + " - " + curFile + string.Format(" [{0:00}m{1:00}] ", duration / 60, duration % 60);
            }
            else if (last != "]")
            {
                this.Text = MainForm._baseTitle;
            }
            if (this._vfo.SignalIsStereo)
            {
                this.Text += " ((( stereo )))";
            }
            if (this.frequencyNumericUpDown.Value > 30000000m)
            {
                this.spectrumAnalyzer.StatusText = string.Empty;
            }
            if (this._vfo.DetectorType == DetectorType.WFM)
            {
                if (!string.IsNullOrEmpty(this._vfo.RdsStationName.Trim()))
                {
                    this.spectrumAnalyzer.StatusText = this._vfo.RdsStationName;
                }
                if (!string.IsNullOrEmpty(this._vfo.RdsStationText))
                {
                    SpectrumAnalyzer spectrumAnalyzer = this.spectrumAnalyzer;
                    spectrumAnalyzer.StatusText = spectrumAnalyzer.StatusText + " [ " + this._vfo.RdsStationText + " ]";
                }
            }
        }

        private unsafe void BuildFFTWindow()
        {
            float[] window = FilterBuilder.MakeWindow(this._fftWindowType, this._fftBins);
            fixed (float* windowPtr = window)
            {
                Utils.Memcpy(this._fftWindow, (void*)windowPtr, this._fftBins * 4);
            }
        }

        private unsafe void BuildBFTWindow()
        {
            float[] window = FilterBuilder.MakeWindow(this._fftWindowType, this._bftBins);
            fixed (float* windowPtr = window)
            {
                Utils.Memcpy(this._bftWindow, (void*)windowPtr, this._bftBins * 4);
            }
        }

        private void iqSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.StopRadio();
            this.spectrumAnalyzer.CenterFixed = this.SourceIsWaveFile;
            this.waterfall.CenterFixed = this.SourceIsWaveFile;
            this.afAnalyzer.CenterFixed = true;
            this.playBar.Visible = this.SourceIsWaveFile;
            if (!this.SourceIsWaveFile)
            {
                this.panSplitContainer.Height = this.scrollPanel.Top + this.scrollPanel.Height - this.panSplitContainer.Top;
            }
            else
            {
                this.panSplitContainer.Height = this.scrollPanel.Top + this.scrollPanel.Height - this.panSplitContainer.Top - this.playBar.Height;
            }
            string oldsampling = this.labSampling.Text;
            this.labSampling.Text = "Min-outp sampling";
            this.configureSourceButton.Text = "Config";
            if (this.iqSourceComboBox.SelectedIndex == this.iqSourceComboBox.Items.Count - 1)
            {
                if (this._frontendController != null)
                {
                    this._frontendController.Close();
                    this._frontendController = null;
                }
                this.inputDeviceComboBox.Enabled = true;
                this.outputDeviceComboBox.Enabled = true;
                this.sampleRateComboBox.Enabled = true;
                this.labSampling.Text = "Input sampling";
                this.storeSampling(oldsampling, this.labSampling.Text);
                this.centerFreqNumericUpDown.Enabled = true;
                this.centerFreqNumericUpDown.Value = 0L;
                this.centerFreqNumericUpDown_ValueChanged(null, null);
                this.frequencyNumericUpDown.Value = this._frequencyShift;
                this.frequencyNumericUpDown_ValueChanged(null, null);
                this.configureSourceButton.Visible = false;
                return;
            }
            this.configureSourceButton.Visible = true;
            if (this.iqSourceComboBox.SelectedIndex == this.iqSourceComboBox.Items.Count - 2)
            {
                if (this._frontendController != null)
                {
                    this._frontendController.Close();
                    this._frontendController = null;
                }
                this.configureSourceButton.Text = "Select";
                this.inputDeviceComboBox.Enabled = false;
                this.outputDeviceComboBox.Enabled = true;
                this.storeSampling(oldsampling, this.labSampling.Text);
                this.latencyNumericUpDown.Enabled = true;
                this.centerFreqNumericUpDown.Enabled = false;
                if (this._formLoaded)
                {
                    this.SelectWaveFile();
                }
                return;
            }
            this.frequencyShiftCheckBox.Enabled = true;
            this.frequencyShiftNumericUpDown.Enabled = this.frequencyShiftCheckBox.Checked;
            string frontendName = this.iqSourceComboBox.SelectedItem;
            try
            {
                if (this._frontendController != null)
                {
                    this._frontendController.Close();
                }
                this._frontendController = this._frontendControllers[frontendName];
                this._frontendController.Open();
                this.inputDeviceComboBox.Enabled = this._frontendController.IsSoundCardBased;
                if (this._frontendController.IsSoundCardBased)
                {
                    this.labSampling.Text = "Input Sampling";
                    Regex regex = new Regex(this._frontendController.SoundCardHint, RegexOptions.IgnoreCase);
                    if (regex.ToString().Length > 0)
                    {
                        for (int i = 0; i < this.inputDeviceComboBox.Items.Count; i++)
                        {
                            string item = this.inputDeviceComboBox.Items[i].ToString();
                            if (regex.IsMatch(item))
                            {
                                this.inputDeviceComboBox.SelectedIndex = i;
                                break;
                            }
                        }
                        if (this._frontendController.Samplerate > 0.0)
                        {
                            this.sampleRateComboBox.Text = this._frontendController.Samplerate.ToString();
                        }
                    }
                }
                this.storeSampling(oldsampling, this.labSampling.Text);
                this._vfo.Frequency = 0;
                this.centerFreqNumericUpDown.Enabled = true;
                if (this.centerFreqNumericUpDown.Value == 0L)
                {
                    this.centerFreqNumericUpDown.Value = this._frontendController.Frequency;
                    this.centerFreqNumericUpDown_ValueChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                if (this._frontendController != null)
                {
                    if (this._frontendController is ExtIOController)
                    {
                        this._frontendController.Stop();
                    }
                    this._frontendController.Close();
                }
                this._frontendController = null;
                MessageBox.Show(frontendName + " is either not connected or its driver is not working properly.\n" + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.iqSourceComboBox.SelectedIndex = this.iqSourceComboBox.Items.Count - 1;
            }
        }

        private void storeSampling(string oldSampling, string newSampling)
        {
            if (oldSampling == newSampling)
            {
                return;
            }
            if (newSampling.Substring(0, 5) == "Input")
            {
                if (this._streamControl.MinOutputSampleRate > 18000)
                {
                    Utils.SaveSetting("minOutputSampleRate", this._streamControl.MinOutputSampleRate.ToString());
                }
                this.sampleRateComboBox.Text = Utils.GetStringSetting("sampleRate", "32000") + " s/sec";
            }
            else
            {
                if (this.sampleRateComboBox.SelectedIndex >= 0)
                {
                    Utils.SaveSetting("sampleRate", this.sampleRateComboBox.Text.Replace(" s/sec", ""));
                }
                this.sampleRateComboBox.Text = Utils.GetStringSetting("minOutputSampleRate", "18000") + " s/sec";
            }
            for (int i = 0; i < this.sampleRateComboBox.Items.Count; i++)
            {
                if (this.sampleRateComboBox.Items[i] == this.sampleRateComboBox.Text)
                {
                    this.sampleRateComboBox.SelectedIndex = i;
                }
            }
        }

        private void SelectWaveFile()
        {
            if (this.openDlg.ShowDialog() == DialogResult.OK)
            {
                this.StopRadio();
                this._waveFile = this.openDlg.FileName;
                this.playStopButton_Click(null, null);
            }
        }

        private void Open()
        {
            AudioDevice inputDevice = null;
            if (this.inputDeviceComboBox.Items.Count > 0)
            {
                inputDevice = this._inputDevices[this.inputDeviceComboBox.SelectedIndex];
            }
            if (this.outputDeviceComboBox.Items.Count == 0)
            {
                throw new ApplicationException("No audio output device found.");
            }
            AudioDevice outputDevice = this._outputDevices[this.outputDeviceComboBox.SelectedIndex];
            long oldCenterFrequency = this.centerFreqNumericUpDown.Value;
            double sampleRate = 0.0;
            Match match = Regex.Match(this.sampleRateComboBox.Text, "([0-9\\.]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                sampleRate = Utils.ValD(match.Groups[1].Value, 0.0);
            }
            if (this.SourceIsWaveFile)
            {
                this._streamControl.MinOutputSampleRate = (int)sampleRate;
            }
            else if (this._frontendController == null || this._frontendController.IsSoundCardBased)
            {
                this._streamControl.MinOutputSampleRate = Utils.GetIntSetting("minOutputSampleRate", 18000);
            }
            else
            {
                this._streamControl.MinOutputSampleRate = (int)sampleRate;
            }
            if (this.SourceIsWaveFile)
            {
                if (!File.Exists(this._waveFile))
                {
                    throw new ApplicationException("File '" + this._waveFile + "'not found.");
                }
                this._duration = this._streamControl.OpenFile(this._waveFile, outputDevice.Index, (int)this.latencyNumericUpDown.Value);
                this.playBar.DrawBackground(0, this._duration * 100);
                this.waterfall.RecordStart = this.getRecordStartFromFile(this._waveFile);
                this.audiogram.RecordStart = this.waterfall.RecordStart;
                this.waterfall.ScanLineMsec = 999;
                this.audiogram.ScanLineMsec = 999;
                match = Regex.Match(Path.GetFileName(this._waveFile), "([0-9]+)kHz", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    int center = int.Parse(match.Groups[1].Value) * 1000;
                    this.centerFreqNumericUpDown.Value = (long)center;
                }
                else
                {
                    this.centerFreqNumericUpDown.Value = 0L;
                }
                this.centerFreqNumericUpDown_ValueChanged(null, null);
                if (-this.frequencyShiftNumericUpDown.Value > this.centerFreqNumericUpDown.Value)
                {
                    this.frequencyShiftCheckBox.Checked = false;
                }
            }
            else if (this._frontendController == null || this._frontendController.IsSoundCardBased)
            {
                if (inputDevice == null)
                {
                    throw new ApplicationException("No audio input device found.");
                }
                this._streamControl.OpenSoundDevice(inputDevice.Index, outputDevice.Index, sampleRate, (int)this.latencyNumericUpDown.Value);
            }
            else
            {
                this._streamControl.OpenPlugin(this._frontendController, outputDevice.Index, (int)this.latencyNumericUpDown.Value);
            }
            this._vfo.SampleRate = this._streamControl.SampleRate;
            this._vfo.DecimationStageCount = this._streamControl.DecimationStageCount;
            int spectrumWidth = (int)this._streamControl.SampleRate;
            this.labSpectrum.Text = ((float)spectrumWidth / 1000f).ToString() + " kHz";
            int i = this.cmbCenterStep.Items.Count - 1;
            while (i >= 0 && this.getFrequency(this.cmbCenterStep.Items[i].ToString()) > spectrumWidth)
            {
                i--;
            }
            this.cmbCenterStep.SelectedIndex = i;
            this.frequencyNumericUpDown.Maximum = this.centerFreqNumericUpDown.Value + (long)((int)(this._streamControl.SampleRate / 2.0)) + this._frequencyShift;
            this.frequencyNumericUpDown.Minimum = this.centerFreqNumericUpDown.Value - (long)((int)(this._streamControl.SampleRate / 2.0)) + this._frequencyShift;
            if (this.centerFreqNumericUpDown.Value != oldCenterFrequency)
            {
                this.frequencyNumericUpDown.Value = this.centerFreqNumericUpDown.Value + this._frequencyShift;
                this.fftZoomTrackBar.Value = 0;
                this.fftZoomCombo.SelectedIndex = 0;
                this.fftZoomTrackBar_ValueChanged(null, null);
            }
            this.frequencyNumericUpDown_ValueChanged(null, null);
            this.BuildFFTWindow();
            this.BuildBFTWindow();
            this._fftSkips = -2;
        }

        private DateTime getRecordStartFromFile(string waveFile)
        {
            Path.GetFileName(waveFile);
            string[] fields = waveFile.Split(new char[]
            {
                '_'
            });
            DateTime recordStart = DateTime.MinValue;
            if (fields.Length >= 3)
            {
                string date = fields[1];
                string time = fields[2];
                if (time.Substring(time.Length - 1, 1).ToUpper() == "Z")
                {
                    time = time.Substring(0, time.Length - 1);
                }
                if (!DateTime.TryParseExact(date + time, "yyyyMMddHHmmss", null, DateTimeStyles.None, out recordStart))
                {
                    recordStart = DateTime.MinValue;
                }
            }
            if (recordStart == DateTime.MinValue)
            {
                recordStart = File.GetCreationTime(waveFile);
            }
            if (this.useTimestampsCheckBox.Text != "UTC")
            {
                recordStart = recordStart.ToLocalTime();
            }
            return recordStart;
        }

        private void audioGainTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this._streamControl.AudioGain = (float)this.audioGainTrackBar.Value;
            this.NotifyPropertyChanged("AudioGain");
        }

        private void filterAudioCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            this._vfo.FilterAudio = this.filterAudioCheckBox.Checked;
            this.NotifyPropertyChanged("FilterAudio");
        }

        private void playStopButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._streamControl.IsPlaying)
                {
                    this.toolTip.Active = true;
                    this.StopRadio();
                }
                else
                {
                    this.toolTip.Active = false;
                    this.StartRadio();
                }
            }
            catch (Exception ex)
            {
                this.StopRadio();
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void frequencyNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this._frequencyChangeBusy)
            {
                return;
            }
            long freq = (long)this.frequencyNumericUpDown.Value;
            this._frequencyChangeBusy = true;
            if (!this.centerFreqNumericUpDown.Enabled)
            {
                if (freq < this.frequencyNumericUpDown.Minimum)
                {
                    freq = (long)this.frequencyNumericUpDown.Minimum;
                }
                if (freq > this.frequencyNumericUpDown.Maximum)
                {
                    freq = (long)this.frequencyNumericUpDown.Maximum;
                }
                this.frequencyNumericUpDown.Value = freq;
            }
            else if ((float)freq < (float)this.waterfall.DisplayFrequency - (float)this.waterfall.SpectrumWidth / this.waterfall.Zoom / this._frequencyBounds || (float)freq > (float)this.waterfall.DisplayFrequency + (float)this.waterfall.SpectrumWidth / this.waterfall.Zoom / this._frequencyBounds)
            {
                long oldVfo = this.spectrumAnalyzer.Frequency - this.spectrumAnalyzer.CenterFrequency;
                this.centerFreqNumericUpDown.Value = freq - oldVfo - this._frequencyShift;
                if (this.centerFreqNumericUpDown.Value < 0L)
                {
                    this.centerFreqNumericUpDown.Value = 0L;
                }
            }
            this._frequencyBounds = 2.2f;
            this._frequencyChangeBusy = false;
            int oldFrequency = (int)(this.spectrumAnalyzer.Frequency + 500L) / 1000;
            this._vfo.Frequency = (int)(this.frequencyNumericUpDown.Value - this.centerFreqNumericUpDown.Value - this._frequencyShift);
            this.spectrumAnalyzer.Frequency = (long)this.frequencyNumericUpDown.Value;
            this.waterfall.Frequency = this.spectrumAnalyzer.Frequency;
            this.ifAnalyzer.Frequency = this.spectrumAnalyzer.Frequency;
            this.ifWaterfall.Frequency = this.spectrumAnalyzer.Frequency;
            if (this._vfo.DetectorType == DetectorType.WFM)
            {
                this._vfo.RdsReset();
            }
            this.NotifyPropertyChanged("Frequency");
            if (this._streamControl.IsPlaying && this.spectrumAnalyzer.DataType == DataType.RF)
            {
                long frequency = (long)(this.frequencyNumericUpDown.Value + 500m) / 1000L;
                if (frequency == (long)oldFrequency)
                {
                    return;
                }
                if (frequency > 3000000L)
                {
                    return;
                }
                string station = "";
                string stations;
                if (!this._frequencyList.TryGetValue((int)frequency, out stations))
                {
                    stations = "";
                }
                if (stations.Length > 0)
                {
                    int i = stations.IndexOf(';');
                    if (i < 0)
                    {
                        i = stations.Length;
                    }
                    if (i > 0)
                    {
                        station = stations.Substring(0, i);
                        string[] fields = station.Split(new char[]
                        {
                            ','
                        });
                        if (fields.GetUpperBound(0) >= 9)
                        {
                            i = fields[7].IndexOf('(');
                            string site = (i < 2) ? fields[7] : fields[7].Substring(0, i - 1);
                            int dbm = Utils.Val(fields[6], 0) - 107;
                            station = string.Concat(new string[]
                            {
                                fields[3],
                                "/",
                                fields[5],
                                " ",
                                fields[4],
                                " - ",
                                site,
                                ", ",
                                fields[9],
                                "km ",
                                fields[10],
                                "kW ",
                                Utils.Signal(dbm, this.cmbDbm.SelectedIndex, true)
                            });
                        }
                    }
                }
                this.spectrumAnalyzer.StatusText = station;
                this.spectrumAnalyzer.StationList = stations;
            }
        }

        private void spectrumAnalyzer_StationDatachanged(object sender, EventArgs e)
        {
            string stations = this.spectrumAnalyzer.StationList;
            if (stations == null)
            {
                return;
            }
            int frequency = (int)(this.spectrumAnalyzer.Frequency + 500L) / 1000;
            if (this._frequencyList.ContainsKey(frequency))
            {
                this._frequencyList[frequency] = stations;
            }
        }

        private void centerFreqNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this.centerFreqNumericUpDown.Value < 0L)
            {
                this.centerFreqNumericUpDown.Value = 0L;
            }
            long newCenterFreq = this.centerFreqNumericUpDown.Value;
            if (Math.Abs(newCenterFreq - this.spectrumAnalyzer.CenterFrequency) > (long)this.waterfall.SpectrumWidth && this._streamControl.IsPlaying)
            {
                this._floorValueNeeded = 10;
            }
            this.waterfall.CenterFrequency = newCenterFreq + this._frequencyShift;
            this.spectrumAnalyzer.CenterFrequency = newCenterFreq + this._frequencyShift;
            long newFreq = newCenterFreq + (long)this._vfo.Frequency + this._frequencyShift;
            this.frequencyNumericUpDown.Maximum = 9223372036854775807m;
            this.frequencyNumericUpDown.Minimum = -9223372036854775808m;
            this.frequencyNumericUpDown.Value = newFreq;
            if (this._vfo.SampleRate > 0.0)
            {
                this.frequencyNumericUpDown.Maximum = newCenterFreq + (long)((int)(this._vfo.SampleRate / 2.0)) + this._frequencyShift;
                this.frequencyNumericUpDown.Minimum = newCenterFreq - (long)((int)(this._vfo.SampleRate / 2.0)) + this._frequencyShift;
            }
            if (this.snapFrequencyCheckBox.Checked)
            {
                if (this.waterfall.StepSize > 0)
                {
                    this.frequencyNumericUpDown.Maximum = (long)this.frequencyNumericUpDown.Maximum / (long)this.waterfall.StepSize * (long)this.waterfall.StepSize;
                }
                this.frequencyNumericUpDown.Minimum = 2L * this.spectrumAnalyzer.CenterFrequency - this.frequencyNumericUpDown.Maximum;
            }
            if (this._frontendController != null && !this.SourceIsWaveFile && !this._extioChangingFrequency)
            {
                lock (this)
                {
                    this._frequencyToSet = newCenterFreq;
                }
            }
            if (this._vfo.DetectorType == DetectorType.WFM)
            {
                this._vfo.RdsReset();
            }
            this.NotifyPropertyChanged("CenterFrequency");
        }

        private void TuneThreadProc(object state)
        {
            Console.WriteLine("TuneThread started");
            Utils.Log("TuneThread started", true);
            while (!this._terminated)
            {
                long copyOfFrequencyToSet;
                lock (this)
                {
                    copyOfFrequencyToSet = this._frequencyToSet;
                }
                if (this._frontendController != null && this._frequencySet != copyOfFrequencyToSet)
                {
                    this._frequencySet = copyOfFrequencyToSet;
                    this._frontendController.Frequency = copyOfFrequencyToSet;
                }
                Thread.Sleep(1);
            }
            Console.WriteLine("TuneThread stopped");
        }

        private void panview_NotchChanged(object sender, NotchEventArgs e)
        {
            gButton chk = (gButton)this.audioCollapsiblePanel.Controls["chkNotch" + e.Notch.ToString()];
            string i = ((UserControl)sender).Name.Substring(0, 4);
            if (i != "spec")
            {
                this.spectrumAnalyzer.SetNotch(e.Notch, e.Offset, e.Width, e.Active);
            }
            if (i != "base")
            {
                this.ifAnalyzer.SetNotch(e.Notch, e.Offset, e.Width, e.Active);
            }
            if (!e.Active)
            {
                this._vfo.SetNotch(e.Notch, 0, 0);
            }
            else
            {
                this._vfo.SetNotch(e.Notch, e.Offset, e.Width);
            }
            if (e.Active && e.Width > 0)
            {
                chk.Checked = true;
            }
            if (!e.Active && e.Width < 0)
            {
                chk.Checked = false;
            }
        }

        private void panview_FrequencyChanged(object sender, FrequencyEventArgs e)
        {
            if (e.Frequency >= this.frequencyNumericUpDown.Minimum && e.Frequency <= this.frequencyNumericUpDown.Maximum)
            {
                if (this.scopeCollapsiblePanel.PanelState == PanelStateOptions.Expanded && !this.agcCheckBox.Checked)
                {
                    this.agcCheckBox.Checked = true;
                }
                this._frequencyBounds = 2f;
                this.frequencyNumericUpDown.Value = e.Frequency;
                return;
            }
            e.Cancel = true;
        }

        private void panview_DisplayFrequencyChanged(object sender, FrequencyEventArgs e)
        {
            if (this.centerFreqNumericUpDown.Enabled)
            {
                return;
            }
            this.spectrumAnalyzer.DisplayFrequency = e.Frequency;
            this.waterfall.DisplayFrequency = e.Frequency;
            this.ifAnalyzer.Frequency = this.spectrumAnalyzer.Frequency;
            this.ifWaterfall.Frequency = this.spectrumAnalyzer.Frequency;
        }

        private void panview_CenterFrequencyChanged(object sender, FrequencyEventArgs e)
        {
            if (this.SourceIsWaveFile)
            {
                e.Cancel = true;
                return;
            }
            long f = e.Frequency - this._frequencyShift;
            if (f < 0L)
            {
                f = 0L;
                e.Cancel = true;
            }
            if (Math.Abs(f - this.centerFreqNumericUpDown.Value) >= (long)(this.waterfall.SpectrumWidth - 1) && this._streamControl.IsPlaying)
            {
                this._floorValueNeeded = 5;
            }
            if (this.scopeCollapsiblePanel.PanelState == PanelStateOptions.Expanded && !this.agcCheckBox.Checked)
            {
                this.agcCheckBox.Checked = true;
            }
            this.centerFreqNumericUpDown.Value = f;
        }

        private void panview_BandwidthChanged(object sender, BandwidthEventArgs e)
        {
            if ((long)e.Bandwidth < this.filterBandwidthNumericUpDown.Minimum)
            {
                e.Bandwidth = (int)this.filterBandwidthNumericUpDown.Minimum;
            }
            else if ((long)e.Bandwidth > this.filterBandwidthNumericUpDown.Maximum)
            {
                e.Bandwidth = (int)this.filterBandwidthNumericUpDown.Maximum;
            }
            int maxRate = (int)this._streamControl.SampleRate >> this._vfo.BaseBandDecimationStageCount;
            if (this._streamControl.SoundCardRatio > 1.0)
            {
                maxRate = (int)((double)maxRate / this._streamControl.SoundCardRatio);
            }
            e.Bandwidth = Math.Min(e.Bandwidth, maxRate);
            this._bandwidthChangeBusy = true;
            this.filterBandwidthNumericUpDown.Value = (long)e.Bandwidth;
            this._bandwidthChangeBusy = false;
            this._vfo.FrequencyOffset = e.Offset;
            this.spectrumAnalyzer.FilterOffset = (this.waterfall.FilterOffset = e.Offset);
            this.ifAnalyzer.FilterOffset = (this.ifWaterfall.FilterOffset = e.Offset);
            this.afAnalyzer.FilterOffset = (this.afWaterfall.FilterOffset = e.Offset);
        }

        private void panview_AutoZoomed(object sender, EventArgs e)
        {
            if (this.ifAnalyzer.Visible)
            {
                this.ifAnalyzer.Zoom = (float)(this.chkAutoSize.Checked ? -1 : 1);
            }
            if (this.ifWaterfall.Visible)
            {
                this.ifWaterfall.Zoom = (float)(this.chkAutoSize.Checked ? -1 : 1);
            }
            if (this.afAnalyzer.Visible)
            {
                this.afAnalyzer.Zoom = (float)(this.chkAutoSize.Checked ? -1 : 1);
            }
            if (this.afWaterfall.Visible)
            {
                this.afWaterfall.Zoom = (float)(this.chkAutoSize.Checked ? -1 : 1);
            }
        }

        private void filterBandwidthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this._vfo.DetectorType != DetectorType.WFM)
            {
                int baseBandDecimationStageCount = this._vfo.BaseBandDecimationStageCount;
            }
            this._vfo.Bandwidth = (int)this.filterBandwidthNumericUpDown.Value;
            this.spectrumAnalyzer.FilterBandwidth = this._vfo.Bandwidth;
            this.waterfall.FilterBandwidth = this._vfo.Bandwidth;
            this.ifAnalyzer.FilterBandwidth = this._vfo.Bandwidth;
            this.ifWaterfall.FilterBandwidth = this._vfo.Bandwidth;
            this.audiogram.FilterBandwidth = this._vfo.Bandwidth;
            this.afAnalyzer.FilterBandwidth = this._vfo.Bandwidth;
            this.afWaterfall.FilterBandwidth = this._vfo.Bandwidth;
            int bw = this._vfo.Bandwidth;
            if (this._vfo.DetectorType == DetectorType.AM || this._vfo.DetectorType == DetectorType.SAM || this._vfo.DetectorType == DetectorType.DSB)
            {
                bw /= 2;
            }
            int i = this.cmbBandWidth.Items.Count - 1;
            while (i > 0 && this.getFrequency(this.cmbBandWidth.Items[i]) != bw)
            {
                i--;
            }
            if (i == 0)
            {
                this.cmbBandWidth.Items[0] = bw.ToString();
            }
            this.cmbBandWidth.SelectedIndex = i;
            this.NotifyPropertyChanged("FilterBandwidth");
        }

        private void filterOrderNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._vfo.FilterOrder = (int)this.filterOrderNumericUpDown.Value;
            this.NotifyPropertyChanged("FilterOrder");
        }

        private void filterTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._vfo.WindowType = this.filterTypeComboBox.SelectedIndex + WindowType.Hamming;
            this.NotifyPropertyChanged("FilterType");
        }

        private void autoCorrectIQCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            this._iqBalancer.AutoBalanceIQ = this.correctIQCheckBox.Checked;
            this.NotifyPropertyChanged("CorrectIq");
        }

        private void frequencyShiftCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            this.frequencyShiftNumericUpDown.Enabled = this.frequencyShiftCheckBox.Checked;
            this.frequencyNumericUpDown.Minimum = -9223372036854775808m;
            this.frequencyNumericUpDown.Maximum = 9223372036854775807m;
            if (this.frequencyShiftCheckBox.Checked)
            {
                this._frequencyShift = this.frequencyShiftNumericUpDown.Value;
                this.frequencyNumericUpDown.Value += this._frequencyShift;
            }
            else
            {
                long shift = this._frequencyShift;
                this._frequencyShift = 0L;
                this.frequencyNumericUpDown.Value -= shift;
            }
            this.centerFreqNumericUpDown_ValueChanged(null, null);
            this.NotifyPropertyChanged("FrequencyShiftEnabled");
        }

        private void frequencyShiftNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._frequencyShift = this.frequencyShiftNumericUpDown.Value;
            this.centerFreqNumericUpDown_ValueChanged(null, null);
            this.NotifyPropertyChanged("FrequencyShift");
        }

        private void modeRadioButton_CheckStateChanged(object sender, EventArgs e)
        {
            this.filterOrderNumericUpDown.Enabled = !this.wfmRadioButton.Checked;
            this.agcDecayNumericUpDown.Enabled = (!this.wfmRadioButton.Checked && !this.nfmRadioButton.Checked);
            this.agcSlopeNumericUpDown.Enabled = (!this.wfmRadioButton.Checked && !this.nfmRadioButton.Checked);
            this.agcThresholdNumericUpDown.Enabled = (!this.wfmRadioButton.Checked && !this.nfmRadioButton.Checked);
            this.agcUseHangCheckBox.Enabled = (!this.wfmRadioButton.Checked && !this.nfmRadioButton.Checked);
            this.agcCheckBox.Enabled = (!this.wfmRadioButton.Checked && !this.nfmRadioButton.Checked);
            this.useSquelchCheckBox.Enabled = (this.nfmRadioButton.Checked || this.amRadioButton.Checked);
            this.squelchNumericUpDown.Enabled = (this.useSquelchCheckBox.Enabled && this.useSquelchCheckBox.Checked);
            this.cwShiftNumericUpDown.Enabled = (this.cwRadioButton.Checked || this.rawRadioButton.Checked);
            if (this._vfo.DetectorType == DetectorType.RAW && !this.rawRadioButton.Checked)
            {
                this.audioGainTrackBar.Value = Utils.GetIntSetting("audioGain", 30);
            }
            bool snap = this.snapFrequencyCheckBox.Checked;
            this.snapFrequencyCheckBox.Checked = false;
            if (this._formLoaded || !this.wfmRadioButton.Checked)
            {
                this.filterAudioCheckBox.Checked = true;
            }
            bool check = sender != null && ((gButton)sender).Checked;
            if (!this._initializing && !check)
            {
                this._modeStates[this._vfo.DetectorType] = this.GetModeState();
            }
            if (this.amRadioButton.Checked)
            {
                this._vfo.DetectorType = DetectorType.AM;
            }
            else if (this.samRadioButton.Checked)
            {
                this.filterBandwidthNumericUpDown.Value = 6000L;
                this._vfo.DetectorType = DetectorType.SAM;
            }
            else if (this.dsbRadioButton.Checked)
            {
                this._vfo.DetectorType = DetectorType.DSB;
            }
            else if (this.lsbRadioButton.Checked)
            {
                this._vfo.DetectorType = DetectorType.LSB;
            }
            else if (this.usbRadioButton.Checked)
            {
                this._vfo.DetectorType = DetectorType.USB;
            }
            else if (this.nfmRadioButton.Checked)
            {
                this._vfo.DetectorType = DetectorType.NFM;
            }
            else if (this.cwRadioButton.Checked)
            {
                this._vfo.DetectorType = DetectorType.CW;
            }
            else if (this.wfmRadioButton.Checked)
            {
                this._vfo.DetectorType = DetectorType.WFM;
                for (int i = 1; i < this.cmbBandWidth.Items.Count; i++)
                {
                    this.cmbBandWidth.setEnabled(i, true);
                }
            }
            else
            {
                if (!this.rawRadioButton.Checked)
                {
                    return;
                }
                this._vfo.DetectorType = DetectorType.RAW;
                this.waterfall.FilterOffset = 0;
                if (this._formLoaded)
                {
                    this.audioGainTrackBar.Value = 0;
                }
            }
            if (this._vfo.DetectorType == DetectorType.LSB)
            {
                this.waterfall.BandType = BandType.Lower;
                this.waterfall.FilterOffset = 400;
            }
            else if (this._vfo.DetectorType == DetectorType.USB)
            {
                this.waterfall.BandType = BandType.Upper;
                this.waterfall.FilterOffset = 400;
            }
            else
            {
                this.waterfall.BandType = BandType.Center;
                this.waterfall.FilterOffset = 0;
            }
            this.spectrumAnalyzer.BandType = this.waterfall.BandType;
            this.ifAnalyzer.BandType = (this.ifWaterfall.BandType = this.waterfall.BandType);
            this.afAnalyzer.BandType = (this.afWaterfall.BandType = this.waterfall.BandType);
            if (check)
            {
                this.SetModeState(this._modeStates[this._vfo.DetectorType]);
                this.fmStereoCheckBox.Enabled = (this.wfmRadioButton.Checked || this.samRadioButton.Checked);
                this.fmStereoCheckBox.Text = (this.samRadioButton.Checked ? " Pseud" : "Ster.");
            }
            this.spectrumAnalyzer.FilterOffset = this.waterfall.FilterOffset;
            this.ifAnalyzer.FilterOffset = (this.ifWaterfall.FilterOffset = this.waterfall.FilterOffset);
            this.afAnalyzer.FilterOffset = (this.afWaterfall.FilterOffset = this.waterfall.FilterOffset);
            this.spectrumAnalyzer.DetectorType = this._vfo.DetectorType;
            this.ifAnalyzer.DetectorType = this._vfo.DetectorType;
            this.afAnalyzer.DetectorType = this._vfo.DetectorType;
            this.afWaterfall.DetectorType = this._vfo.DetectorType;
            this.afAnalyzer.BandType = this.waterfall.BandType;
            this.afAnalyzer.Zoom = (float)(this.chkAutoSize.Checked ? -1 : 1);
            this.scope.ShowLines = (this._vfo.DetectorType == DetectorType.AM || this._vfo.DetectorType == DetectorType.SAM);
            this.snapFrequencyCheckBox.CheckedChanged -= this.stepSizeComboBox_SelectedIndexChanged;
            this.snapFrequencyCheckBox.Checked = snap;
            this.snapFrequencyCheckBox.CheckedChanged += this.stepSizeComboBox_SelectedIndexChanged;
            this.NotifyPropertyChanged("DetectorType");
        }

        private void fmStereoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._vfo.Stereo = this.fmStereoCheckBox.Checked;
            this.NotifyPropertyChanged("FmStereo");
        }

        private void cwShiftNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._vfo.CWToneShift = (int)this.cwShiftNumericUpDown.Value;
            this.waterfall.FilterOffset = this._vfo.CWToneShift - this._vfo.Bandwidth / 2;
            this.ifAnalyzer.FilterOffset = this.waterfall.FilterOffset;
            this.ifWaterfall.FilterOffset = this.waterfall.FilterOffset;
            this.afAnalyzer.FilterOffset = this.waterfall.FilterOffset;
            this.afWaterfall.FilterOffset = this.waterfall.FilterOffset;
            this.spectrumAnalyzer.FilterOffset = this.waterfall.FilterOffset;
            this.NotifyPropertyChanged("CWShift");
        }

        private void squelchNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._vfo.SquelchThreshold = (int)this.squelchNumericUpDown.Value;
            this.NotifyPropertyChanged("SquelchThreshold");
        }

        private void useSquelchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.squelchNumericUpDown.Enabled = this.useSquelchCheckBox.Checked;
            if (this.useSquelchCheckBox.Checked)
            {
                this._vfo.SquelchThreshold = (int)this.squelchNumericUpDown.Value;
            }
            else
            {
                this._vfo.SquelchThreshold = 0;
            }
            this.NotifyPropertyChanged("SquelchEnabled");
        }

        private void stepSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.UseSnap = this.snapFrequencyCheckBox.Checked;
            this.waterfall.UseSnap = this.snapFrequencyCheckBox.Checked;
            int stepSize = this.getFrequency(this.stepSizeComboBox.Text);
            if (stepSize > 0)
            {
                this.frequencyNumericUpDown.Increment = stepSize;
                this.spectrumAnalyzer.StepSize = stepSize;
                this.waterfall.StepSize = stepSize;
                this.ifAnalyzer.StepSize = stepSize;
                this.ifWaterfall.StepSize = stepSize;
                this.afAnalyzer.StepSize = stepSize;
                this.ifWaterfall.StepSize = stepSize;
                this.audiogram.StepSize = stepSize;
                if (this.snapFrequencyCheckBox.Checked && !this.SourceIsWaveFile)
                {
                    this.frequencyNumericUpDown.Maximum = decimal.MaxValue;
                    this.frequencyNumericUpDown.Minimum = decimal.MinValue;
                    this.centerFreqNumericUpDown.Value = (this.centerFreqNumericUpDown.Value + (long)(stepSize / 2)) / (long)stepSize * (long)stepSize;
                    this.frequencyNumericUpDown.Value = ((long)this.frequencyNumericUpDown.Value + (long)(stepSize / 2)) / (long)stepSize * (long)stepSize;
                    this.frequencyNumericUpDown.Maximum = this.centerFreqNumericUpDown.Value + this._frequencyShift + (long)((int)(this._vfo.SampleRate / 2.0));
                    this.frequencyNumericUpDown.Minimum = this.centerFreqNumericUpDown.Value + this._frequencyShift - (long)((int)(this._vfo.SampleRate / 2.0));
                    this.frequencyNumericUpDown.Maximum = (long)this.frequencyNumericUpDown.Maximum / (long)this.waterfall.StepSize * (long)this.waterfall.StepSize;
                    this.frequencyNumericUpDown.Minimum = 2L * this.spectrumAnalyzer.CenterFrequency - this.frequencyNumericUpDown.Maximum;
                }
            }
            if (sender == this.snapFrequencyCheckBox)
            {
                this.NotifyPropertyChanged("SnapToGrid");
            }
            this.NotifyPropertyChanged("StepSize");
        }

        private void cmbCenterStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.UseSnap = this.snapFrequencyCheckBox.Checked;
            this.waterfall.UseSnap = this.snapFrequencyCheckBox.Checked;
            int stepSize = this.getFrequency(this.cmbCenterStep.Text);
            if (stepSize > 0)
            {
                this.centerFreqNumericUpDown.Increment = stepSize;
                this.spectrumAnalyzer.CenterStep = stepSize;
                this.ifAnalyzer.CenterStep = stepSize;
            }
            this.NotifyPropertyChanged("CenterStep");
        }

        private void frontendGuiButton_Click(object sender, EventArgs e)
        {
            if (this.SourceIsWaveFile)
            {
                this.SelectWaveFile();
                return;
            }
            if (this._frontendController != null)
            {
                this._frontendController.ShowSettingGUI(this);
            }
        }

        private void agcCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._vfo.UseAGC = this.agcCheckBox.Checked;
            this.agcThresholdNumericUpDown.Enabled = this.agcCheckBox.Checked;
            this.agcDecayNumericUpDown.Enabled = this.agcCheckBox.Checked;
            this.agcSlopeNumericUpDown.Enabled = this.agcCheckBox.Checked;
            this.agcUseHangCheckBox.Enabled = this.agcCheckBox.Checked;
            if (this.agcCheckBox.Checked)
            {
                this.tbRFgain.Value = 0;
            }
            else
            {
                this.setRfGainFromSmeter();
            }
            this.wideScope.Reset();
            if (!this.agcCheckBox.Checked)
            {
                this.scope.Reset();
            }
            this.NotifyPropertyChanged("UseAgc");
        }

        private void agcUseHangCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._vfo.AgcHang = this.agcUseHangCheckBox.Checked;
            this.NotifyPropertyChanged("UseHang");
        }

        private void agcDecayNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._vfo.AgcDecay = (float)this.agcDecayNumericUpDown.Value;
            this.agcDecayLabel.Text = this.agcDecayNumericUpDown.Value.ToString();
            this.NotifyPropertyChanged("AgcDecay");
        }

        private void agcThresholdNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._vfo.AgcThreshold = (float)((int)this.agcThresholdNumericUpDown.Value);
            this.NotifyPropertyChanged("AgcThreshold");
        }

        private void agcSlopeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._vfo.AgcSlope = (float)((int)this.agcSlopeNumericUpDown.Value);
            this.NotifyPropertyChanged("AgcSlope");
        }

        private void swapIQCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._streamControl.SwapIQ = this.swapIQCheckBox.Checked;
            this.NotifyPropertyChanged("SwapIq");
        }

        private void chkFFT_CheckedChanged(object sender, EventArgs e)
        {
            this.panSplitContainer.Panel1Collapsed = false;
            this.panSplitContainer.Panel2Collapsed = (!this.chkWF.Checked && !this.chkIF.Checked && this.cmbAudio.SelectedIndex == 0);
            this.panSplitContainer2.Panel1Collapsed = !this.chkWF.Checked;
            this.panSplitContainer2.Panel2Collapsed = (!this.chkIF.Checked && this.cmbAudio.SelectedIndex == 0);
            this.panSplitContainer3.Panel1Collapsed = !this.chkIF.Checked;
            this.panSplitContainer3.Panel2Collapsed = (this.cmbAudio.SelectedIndex == 0);
            this.panSplitContainer5.Panel1Collapsed = (this.cmbAudio.SelectedIndex != 1 && this.cmbAudio.SelectedIndex != 2);
            this.panSplitContainer5.Panel2Collapsed = (this.cmbAudio.SelectedIndex != 1 && this.cmbAudio.SelectedIndex != 3);
            this.panSplitContainer4.Panel2Collapsed = (this.cmbAudio.SelectedIndex != 1 && this.cmbAudio.SelectedIndex != 0);
            this.ifAnalyzer.Visible = this.chkIF.Checked;
            this.ifWaterfall.Visible = (this.chkIF.Checked && (this.cmbAudio.SelectedIndex == 1 || this.cmbAudio.SelectedIndex == 0));
            this.afAnalyzer.Visible = (this.cmbAudio.SelectedIndex == 1);
            this.afWaterfall.Visible = (this.cmbAudio.SelectedIndex == 1);
            this.panelAG.Visible = (this.cmbAudio.SelectedIndex == 2);
            this.audiogram.Visible = (this.cmbAudio.SelectedIndex == 2);
            this.wideScope.Visible = (this.cmbAudio.SelectedIndex == 3);
            if (!this.audiogram.Visible)
            {
                this.tbAgSpeed.Value = Math.Min(this.tbAgSpeed.Maximum - 2, this.tbAgSpeed.Value);
            }
            if (!this.waterfall.Visible)
            {
                this.fftSpeedTrackBar.Value = Math.Min(this.fftSpeedTrackBar.Maximum - 2, this.fftSpeedTrackBar.Value);
            }
            this.mnuShowWaterfall.Checked = this.chkWF.Checked;
            this.mnuShowBaseband.Checked = this.chkIF.Checked;
            this.mnuShowAudio.Checked = (this.cmbAudio.SelectedIndex == 1);
            this.mnuShowAudiogram.Checked = (this.cmbAudio.SelectedIndex == 2);
            this.mnuShowEnvelope.Checked = (this.cmbAudio.SelectedIndex == 3);
        }

        private void cmbAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkFFT_CheckedChanged(null, null);
        }

        private void fftResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._fftBins = int.Parse(this.fftResolutionComboBox.SelectedItem.ToString());
            this._fftGain = (float)(Math.Log((double)this._fftBins, 2.0) * 6.0);
            this.BuildFFTWindow();
            this._fftSkips = -2;
            this.NotifyPropertyChanged("FFTResolution");
        }

        private void bftResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._bftBins = int.Parse(this.bftResolutionComboBox.SelectedItem.ToString());
            this._bftGain = (float)(Math.Log((double)this._bftBins, 2.0) * 6.0);
            this.BuildBFTWindow();
        }

        private void fftWindowComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._fftWindowType = (WindowType)this.fftWindowComboBox.SelectedIndex;
            this.BuildFFTWindow();
            this.BuildBFTWindow();
        }

        private void gradientButtonSA_Click(object sender, EventArgs e)
        {
            new Thread(delegate ()
            {
                GradientDialog.ShowGradient(this._saBlendIndex, this._saBlends, this._backgrounds, this._traceColors, this._spectrumFill, "SPECTRUM/SCOPE");
            }).Start();
        }

        private void gradientButtonWF_Click(object sender, EventArgs e)
        {
            new Thread(delegate ()
            {
                GradientDialog.ShowGradient(this._wfBlendIndex, this._wfBlends, this._backgrounds, this._traceColors, this._spectrumFill, "WATERFALL");
            }).Start();
        }

        private void gradientButtonAG_Click(object sender, EventArgs e)
        {
            new Thread(delegate ()
            {
                GradientDialog.ShowGradient(this._agBlendIndex, this._agBlends, this._backgrounds, this._traceColors, this._spectrumFill, "AUDIOGRAM");
            }).Start();
        }

        private void GradientDialog_GradientChanged(object sender, GradientEventArgs e)
        {
            if (this._colorInvokeBusy || (e.Index > 0 && (e.Blend == null || e.Blend.Positions.Length == 0)))
            {
                return;
            }
            base.BeginInvoke(new MethodInvoker(delegate
            {
                this._colorInvokeBusy = true;
                this.InvokedGradientChange(e);
            }));
        }

        private void InvokedGradientChange(GradientEventArgs e)
        {
            int i = e.Index - 1;
            string p = e.Parent.Substring(0, 1);
            if (e.Parent == "undo")
            {
                this.GetColorSettings();
            }
            else if (e.Parent == "save")
            {
                this.PutColorSettings();
            }
            else if (e.Index == -1)
            {
                if (p == "S")
                {
                    this.gradientButtonSA_Click(this, null);
                }
                else if (p == "W")
                {
                    this.gradientButtonWF_Click(this, null);
                }
                else if (p == "A")
                {
                    this.gradientButtonAG_Click(this, null);
                }
            }
            else if (p == "S")
            {
                this._saBlendIndex = e.Index;
                this._spectrumFill[i] = e.Fill;
                this._backgrounds[i] = e.Back;
                this._traceColors[i] = e.Trace;
                this._saBlends[i] = e.Blend;
                this.spectrumAnalyzer.SpectrumFill = this._spectrumFill[i];
                this.spectrumAnalyzer.SpectrumColor = Color.FromArgb(this._traceColors[i]);
                this.scope.SpectrumFill = this.spectrumAnalyzer.SpectrumFill;
                this.scope.TraceColor = this.spectrumAnalyzer.SpectrumColor;
                Color color = Color.FromArgb(this._backgrounds[i]);
                if (this.spectrumAnalyzer.BackgroundColor != color)
                {
                    this.spectrumAnalyzer.BackgroundColor = color;
                    this.ifAnalyzer.BackgroundColor = color;
                    this.afAnalyzer.BackgroundColor = color;
                    this.scope.BackgoundColor = color;
                    this.wideScope.BackgoundColor = color;
                    this.waterfall.BackgroundColor = this.spectrumAnalyzer.BackgroundColor;
                    this.ifWaterfall.BackgroundColor = this.spectrumAnalyzer.BackgroundColor;
                    this.afWaterfall.BackgroundColor = this.spectrumAnalyzer.BackgroundColor;
                    this.audiogram.BackgroundColor = this.spectrumAnalyzer.BackgroundColor;
                }
                this.spectrumAnalyzer.GradientColorBlend = e.Blend;
                this.ifAnalyzer.GradientColorBlend = e.Blend;
                this.afAnalyzer.GradientColorBlend = e.Blend;
                this.scope.GradientColorBlend = e.Blend;
                this.wideScope.GradientColorBlend = e.Blend;
            }
            else if (p == "W")
            {
                this._wfBlendIndex = e.Index;
                this._wfBlends[i] = e.Blend;
                this.waterfall.GradientColorBlend = e.Blend;
                this.ifWaterfall.GradientColorBlend = e.Blend;
                this.afWaterfall.GradientColorBlend = e.Blend;
            }
            else if (p == "A")
            {
                this._agBlendIndex = e.Index;
                this._agBlends[i] = e.Blend;
                this.audiogram.GradientColorBlend = e.Blend;
            }
            this._colorInvokeBusy = false;
        }

        private static string GradientToString(Color[] colors)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < colors.Length; i++)
            {
                sb.AppendFormat(",{0:X2}{1:X2}{2:X2}", colors[i].R, colors[i].G, colors[i].B);
            }
            return sb.ToString().Substring(1);
        }

        private void sAttackTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.Attack = (double)this.sAttackTrackBar.Value / (double)this.sAttackTrackBar.Maximum;
            this.ifAnalyzer.Attack = 1.0;
            this.afAnalyzer.Attack = 1.0;
            this.NotifyPropertyChanged("SAttack");
        }

        private void sDecayTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.Decay = (double)this.sDecayTrackBar.Value / (double)this.sDecayTrackBar.Maximum;
            this.ifAnalyzer.Decay = 1.0;
            this.afAnalyzer.Decay = 1.0;
            this.NotifyPropertyChanged("SDecay");
        }

        private void wAttackTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this.waterfall.Attack = (double)this.wAttackTrackBar.Value / (double)this.wAttackTrackBar.Maximum;
            this.ifWaterfall.Attack = this.waterfall.Attack;
            this.afWaterfall.Attack = this.waterfall.Attack;
            this.audiogram.Attack = this.waterfall.Attack;
            this.NotifyPropertyChanged("WAttack");
        }

        private void wDecayTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this.waterfall.Decay = (double)this.wDecayTrackBar.Value / (double)this.wDecayTrackBar.Maximum;
            this.ifWaterfall.Decay = this.waterfall.Decay;
            this.afWaterfall.Decay = this.waterfall.Decay;
            this.audiogram.Decay = this.waterfall.Decay;
            this.NotifyPropertyChanged("WDecay");
        }

        private void markPeaksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.MarkPeaks = this.markPeaksCheckBox.Checked;
            this.ifAnalyzer.MarkPeaks = this.markPeaksCheckBox.Checked;
            this.afAnalyzer.MarkPeaks = this.markPeaksCheckBox.Checked;
            this.NotifyPropertyChanged("MarkPeaks");
        }

        private void useTimestampCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.SourceIsWaveFile)
            {
                this.waterfall.RecordStart = this.getRecordStartFromFile(this._waveFile);
                this.audiogram.RecordStart = this.waterfall.RecordStart;
            }
            if (!this._initializing && !this.useTimestampsCheckBox.Checked)
            {
                if (this.useTimestampsCheckBox.Text == "UTC")
                {
                    this.useTimestampsCheckBox.Text = "Time";
                }
                else
                {
                    this.useTimestampsCheckBox.Text = "UTC";
                    this.useTimestampsCheckBox.Checked = true;
                }
            }
            this.waterfall.UseTimestamps = ((!this.useTimestampsCheckBox.Checked) ? 0 : ((this.useTimestampsCheckBox.Text == "Time") ? 1 : 2));
            this.audiogram.UseTimestamps = this.waterfall.UseTimestamps;
            this.NotifyPropertyChanged("UseTimeMarkers");
        }

        private void fftSpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this.setFftInterval();
            this.startSpeedCalc();
            this.showWaterfallSpeed();
        }

        private void fftZoomCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._streamControl.IsPlaying)
            {
                return;
            }
            double scale = 1.0;
            if (this.fftZoomCombo.SelectedIndex > 0 && this.fftZoomCombo.SelectedIndex < this.fftZoomCombo.Items.Count)
            {
                double spectrum = (double)this.getFrequency(this.fftZoomCombo.Text);
                if (spectrum > (double)this.spectrumAnalyzer.SpectrumWidth)
                {
                    this.fftZoomCombo.SelectedIndex = 0;
                }
                else
                {
                    scale = (double)this.spectrumAnalyzer.SpectrumWidth / spectrum;
                }
            }
            this.spectrumAnalyzer.Zoom = (float)scale;
            this.waterfall.Zoom = (float)scale;
            int i = this.cmbCenterStep.Items.Count - 1;
            while (i >= 0 && (double)this.getFrequency(this.cmbCenterStep.Items[i].ToString()) > (double)this.spectrumAnalyzer.SpectrumWidth / scale)
            {
                i--;
            }
            this.cmbCenterStep.SelectedIndex = i;
            this.labSpectrum.Text = Convert.ToInt32((float)this.spectrumAnalyzer.SpectrumWidth / 1000f).ToString() + " kHz";
        }

        private void fftZoomTrackBar_ValueChanged(object sender, EventArgs e)
        {
            this.fftZoomCombo_SelectedIndexChanged(sender, e);
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (base.WindowState != FormWindowState.Minimized && this._formLoaded)
            {
                this._lastLocation = base.Location;
            }
            this._fftSkips = -20;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized || !this._formLoaded)
            {
                return;
            }
            if (base.WindowState != FormWindowState.Minimized)
            {
                this._lastSize = base.Size;
            }
            int width = base.Width;
            int left = this.playStopButton.Left;
            int left2 = this.playStopButton.Left;
            this.chkLock.Left = this.frequencyNumericUpDown.Left + this.frequencyNumericUpDown.Width + 10;
            this.labBandWidth.Left = (this.chkLock.Left + this.chkLock.Width + this.labZoom.Left - (this.labBandWidth.Width + this.cmbBandWidth.Width + 5)) / 2;
            this.cmbBandWidth.Left = this.labBandWidth.Left + this.labBandWidth.Width + 5;
            this.fftZoomCombo.Location = this.fftZoomTrackBar.Location;
            this.pnlScroll.Top = 0;
            this.pnlScroll.Height = this.scrollPanel.Height;
            this.controlPanel.Top = 2;
            this.SetThumb(this.gThumb.Top);
            this._fftSkips = -20;
        }

        private void panSplitContainer3_Panel2_Resize(object sender, EventArgs e)
        {
            this.wideScope.Tdiv = this.scope.Tdiv;
        }

        private void panSplitContainer5_Panel1_Resize(object sender, EventArgs e)
        {
        }

        private void panSplitContainer5_Panel2_Resize(object sender, EventArgs e)
        {
            this.wideScope.Location = this.afWaterfall.Location;
            this.wideScope.Size = this.panSplitContainer5.Panel2.Size;
            if ((double)this.wideScope.Width < 0.3 * (double)this.spectrumAnalyzer.Width)
            {
                this.wideScope.Tdiv = this.scope.Tdiv / 4f;
                return;
            }
            if ((double)this.wideScope.Width < 0.6 * (double)this.spectrumAnalyzer.Width)
            {
                this.wideScope.Tdiv = this.scope.Tdiv / 2f;
                return;
            }
            this.wideScope.Tdiv = this.scope.Tdiv;
        }

        private int[] GetCollapsiblePanelStates()
        {
            List<int> states = new List<int>();
            for (SDRSharp.CollapsiblePanel.CollapsiblePanel currentPanel = this.radioCollapsiblePanel; currentPanel != null; currentPanel = currentPanel.NextPanel)
            {
                states.Add((int)currentPanel.PanelState);
            }
            return states.ToArray();
        }

        private void chkAver_CheckedChanged(object sender, EventArgs e)
        {
            Utils.ChkAver = this.chkAver.Checked;
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {
            Utils.Chk1 = this.chk1.Checked;
            this.startSpeedCalc();
            this._fftSkips = -1;
        }

        private void chkFastConv_CheckedChanged(object sender, EventArgs e)
        {
            Utils.FastConvolve = this.chkFastConv.Checked;
        }

        private void cmbDbm_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.ShowDbm = this.cmbDbm.SelectedIndex;
            this.waterfall.ShowDbm = this.cmbDbm.SelectedIndex;
        }

        private void dbmOffsetUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._floorValueNeeded = 5;
        }

        private void tbSpanSA_Changed(object sender, EventArgs e)
        {
            int floorValue = this.tbFloorSA.Minimum + this.tbFloorSA.Maximum - this.tbFloorSA.Value;
            int spanValue = this.tbSpanSA.Minimum + this.tbSpanSA.Maximum - this.tbSpanSA.Value;
            this.spectrumAnalyzer.MinPower = (float)floorValue;
            this.spectrumAnalyzer.MaxPower = (float)Math.Min(20, floorValue + spanValue);
            this.ifAnalyzer.MinPower = this.spectrumAnalyzer.MinPower - 10f;
            this.ifAnalyzer.MaxPower = this.spectrumAnalyzer.MaxPower;
            this.afAnalyzer.MinPower = -130f;
            this.afAnalyzer.MaxPower = -130f + this.ifAnalyzer.MaxPower - this.ifAnalyzer.MinPower;
            this._fftSkips = -1;
        }

        private void tbFloorSA_Changed(object sender, EventArgs e)
        {
            int floorValue = this.tbFloorSA.Minimum + this.tbFloorSA.Maximum - this.tbFloorSA.Value;
            int spanValue = this.tbSpanSA.Minimum + this.tbSpanSA.Maximum - this.tbSpanSA.Value;
            if (floorValue + spanValue > 20)
            {
                Console.WriteLine("Floor+Span > 25");
            }
            this.spectrumAnalyzer.MinPower = (float)floorValue;
            this.spectrumAnalyzer.MaxPower = (float)Math.Min(20, floorValue + spanValue);
            this.ifAnalyzer.MinPower = this.spectrumAnalyzer.MinPower - 10f;
            this.ifAnalyzer.MaxPower = this.spectrumAnalyzer.MaxPower;
            this.afAnalyzer.MinPower = -130f;
            this.afAnalyzer.MaxPower = -130f + this.ifAnalyzer.MaxPower - this.ifAnalyzer.MinPower;
            this.tbIntensityWv.Value = Math.Min(this.tbIntensityAg.Maximum, Math.Max(this.tbIntensityAg.Minimum, this.tbFloorSA.Value));
        }

        private void tbContrastWv_Changed(object sender, EventArgs e)
        {
            int intensityValue = this.tbIntensityWv.Minimum + this.tbIntensityWv.Maximum - this.tbIntensityWv.Value;
            int contrastValue = this.tbContrastWv.Minimum + this.tbContrastWv.Maximum - this.tbContrastWv.Value;
            if (intensityValue + contrastValue > 20)
            {
                Console.WriteLine("Intens+Contr > 25");
            }
            this.waterfall.MinPower = (float)intensityValue;
            this.waterfall.MaxPower = (float)Math.Min(20, intensityValue + contrastValue);
            this.ifWaterfall.MinPower = this.waterfall.MinPower;
            this.ifWaterfall.MaxPower = this.waterfall.MaxPower;
            this.afWaterfall.MinPower = -130f;
            this.afWaterfall.MaxPower = -130f + this.ifWaterfall.MaxPower - this.ifWaterfall.MinPower;
            this.labCon.Text = Utils.Signal((int)this.waterfall.MaxPower, this.cmbDbm.SelectedIndex, false);
            this._fftSkips = -1;
        }

        private void tbIntensityWv_Changed(object sender, EventArgs e)
        {
            int intensityValue = this.tbIntensityWv.Minimum + this.tbIntensityWv.Maximum - this.tbIntensityWv.Value;
            int contrastValue = this.tbContrastWv.Minimum + this.tbContrastWv.Maximum - this.tbContrastWv.Value;
            if (intensityValue + contrastValue > 20)
            {
                Console.WriteLine("Intens+Contr > 25");
            }
            this.waterfall.MinPower = (float)intensityValue;
            this.waterfall.MaxPower = (float)Math.Min(20, intensityValue + contrastValue);
            this.ifWaterfall.MinPower = this.waterfall.MinPower;
            this.ifWaterfall.MaxPower = this.waterfall.MaxPower;
            this.afWaterfall.MinPower = -130f;
            this.afWaterfall.MaxPower = -130f + this.ifWaterfall.MaxPower - this.ifWaterfall.MinPower;
            this.labInt.Text = Utils.Signal((int)this.waterfall.MinPower, this.cmbDbm.SelectedIndex, false);
            this._fftSkips = -1;
        }

        private void cmbBandWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._bandwidthChangeBusy)
            {
                return;
            }
            bool doubleBW = this._vfo.DetectorType == DetectorType.AM || this._vfo.DetectorType == DetectorType.SAM || this._vfo.DetectorType == DetectorType.DSB;
            int bw = this.getFrequency(this.cmbBandWidth.Text);
            if (doubleBW)
            {
                bw *= 2;
            }
            if (bw > 0)
            {
                this.filterBandwidthNumericUpDown.Value = (long)bw;
            }
            if (this.cmbBandWidth.SelectedIndex > 0)
            {
                bw = this.getFrequency(this.cmbBandWidth.SelectedItem);
                if (doubleBW)
                {
                    bw *= 2;
                }
                if (bw > 0)
                {
                    this.cmbBandWidth.Tag = bw;
                }
            }
            if (this.waterfall.BandType == BandType.Center && this.cmbBandWidth.SelectedIndex > 0)
            {
                this.spectrumAnalyzer.FilterOffset = (this.waterfall.FilterOffset = 0);
                this.ifAnalyzer.FilterOffset = (this.ifWaterfall.FilterOffset = 0);
                this.afAnalyzer.FilterOffset = (this.afWaterfall.FilterOffset = 0);
                this.audiogram.FilterOffset = 0;
                this._vfo.FrequencyOffset = 0;
            }
            this.panview_AutoZoomed(null, null);
        }

        private void cmbTim_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.scope.Tdiv = this.StrToVal(this.cmbTim.SelectedItem.ToString());
            this.wideScope.Tdiv = this.scope.Tdiv;
            this.panSplitContainer3_Panel2_Resize(null, null);
        }

        private void cmbVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.scope.Vdiv = this.StrToVal(this.cmbVer.SelectedItem.ToString());
            this.wideScope.Vdiv = this.scope.Vdiv;
        }

        private void cmbHor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.scope.Hdiv = this.StrToVal(this.cmbHor.SelectedItem.ToString());
            this.wideScope.Hdiv = this.scope.Hdiv;
        }

        private float StrToVal(string str)
        {
            int i = str.IndexOf(' ');
            if (i < 0)
            {
                i = str.Length;
            }
            float val = (float)Utils.ValD(str.Substring(0, i), 0.0);
            if (str.IndexOf('m') >= 0)
            {
                val /= 1000f;
            }
            if (str.IndexOf('u') >= 0)
            {
                val /= 1000000f;
            }
            if (str.IndexOf('k') >= 0)
            {
                val *= 1000f;
            }
            if (str.IndexOf('K') >= 0)
            {
                val *= 1000f;
            }
            return val;
        }

        private void chkVinvert_CheckedChanged(object sender, EventArgs e)
        {
            this.scope.Vinvert = this.chkVinvert.Checked;
            this.wideScope.Vinvert = this.chkVinvert.Checked;
        }

        private void chkHinvert_CheckedChanged(object sender, EventArgs e)
        {
            this.scope.Hinvert = this.chkHinvert.Checked;
            this.wideScope.Hinvert = this.chkHinvert.Checked;
        }

        private void chkXY_CheckedChanged(object sender, EventArgs e)
        {
            this.scope.XYmode = this.chkXY.Checked;
            this.wideScope.XYmode = false;
            this.cmbHor.Enabled = this.scope.XYmode;
            this.cmbHchannel.Enabled = this.scope.XYmode;
            this.chkHrunDC.Enabled = this.scope.XYmode;
            this.chkHinvert.Enabled = this.scope.XYmode;
            this.tbTrigL.Enabled = !this.scope.XYmode;
            if (this.scope.Vchannel == DemodType.AM)
            {
                this.chkVrunDC.Checked = false;
            }
        }

        private void chkHrunDC_CheckedChanged(object sender, EventArgs e)
        {
            this.scope.HblockDC = !this.chkHrunDC.Checked;
            this.wideScope.HblockDC = !this.chkHrunDC.Checked;
        }

        private void chkVrunDC_CheckedChanged(object sender, EventArgs e)
        {
            this.scope.VblockDC = !this.chkVrunDC.Checked;
            this.wideScope.VblockDC = !this.chkVrunDC.Checked;
        }

        private void cmbVchannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.wideScope.Vchannel = DemodType.Envelope;
            this.scope.Vchannel = this.cmbVchannel.SelectedIndex + DemodType.AM;
            if (this.scope.Vchannel == DemodType.AM)
            {
                this.chkVrunDC.Checked = true;
            }
        }

        private void cmbHchannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.scope.Hchannel = this.cmbHchannel.SelectedIndex + DemodType.AM;
            if (this.scope.Vchannel == DemodType.AM)
            {
                this.chkVrunDC.Checked = true;
            }
        }

        private void scope_XYPositionChanged(object sender, PositionEventArgs e)
        {
            if (e.Trig)
            {
                this.tbTrigL.Value = (int)(e.Ypos * 100f);
            }
        }

        private void tbTrigL_ValueChanged(object sender, EventArgs e)
        {
            this.scope.TrigLevel = (float)this.tbTrigL.Value / 100f;
        }

        private void tbAverage_ValueChanged(object sender, EventArgs e)
        {
            this.labTbAverage.Text = this.tbAverage.Value.ToString();
            Utils.PhaseAverage = this.tbAverage.Value;
        }

        private void tbGain_ValueChanged(object sender, EventArgs e)
        {
            this.labTbGain.Text = this.tbGain.Value.ToString();
            Utils.PhaseGain = this.tbGain.Value;
        }

        private int getFrequency(string text)
        {
            Match match = Regex.Match(text, "([0-9\\.]+) Mhz", RegexOptions.None);
            if (match.Success)
            {
                return (int)(double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture) * 1000000.0);
            }
            match = Regex.Match(text, "([0-9\\.]+) kHz", RegexOptions.None);
            if (match.Success)
            {
                return (int)(double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture) * 1000.0);
            }
            match = Regex.Match(text, "([0-9]+) Hz", RegexOptions.None);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return Utils.Val(text, 0);
        }
        #endregion

        #region [Plugins Load]
        private void InitialiseSharpPlugins()
        {
            this._sharpControlProxy = new SharpControlProxy(this);
            NameValueCollection sharpPlugins = (NameValueCollection)ConfigurationManager.GetSection("sharpPlugins");
            if (sharpPlugins == null)
            {
                MessageBox.Show("Configuration section 'sharpPlugins' was not found. Please check 'SDRSharp.exe.config'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (object obj in sharpPlugins.Keys)
            {
                string key = (string)obj;
                try
                {
                    string fullyQualifiedTypeName = sharpPlugins[key];
                    string[] patterns = fullyQualifiedTypeName.Split(new char[]
                    {
                        ','
                    });
                    string typeName = patterns[0];
                    string assemblyName = patterns[1];
                    ObjectHandle objectHandle = Activator.CreateInstance(assemblyName, typeName);
                    ISharpPlugin plugin = (ISharpPlugin)objectHandle.Unwrap();                    
                    this._sharpPlugins.Add(key, plugin);
                    plugin.Initialize(this._sharpControlProxy);
                    if (plugin.HasGui)
                    {
                        this.CreatePluginCollapsiblePanel(plugin);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading '" + sharpPlugins[key] + "' - " + ex.Message);
                }
            }
            int[] collapsiblePanelStates = Utils.GetIntArraySetting("collapsiblePanelStates", null);
            if (collapsiblePanelStates != null)
            {
                SDRSharp.CollapsiblePanel.CollapsiblePanel currentPanel = this.radioCollapsiblePanel;
                int i = 0;
                while (i < collapsiblePanelStates.Length && currentPanel != null)
                {
                    currentPanel.PanelState = (PanelStateOptions)collapsiblePanelStates[i];
                    currentPanel = currentPanel.NextPanel;
                    i++;
                }
            }
            this.radioCollapsiblePanel.PanelState = PanelStateOptions.Collapsed;
            if (collapsiblePanelStates[0] == 1)
            {
                this.radioCollapsiblePanel.PanelState = PanelStateOptions.Expanded;
            }
        }

        private void CreatePluginCollapsiblePanel(ISharpPlugin plugin)
        {
            UserControl panelContents = plugin.GuiControl;
            if (panelContents != null)
            {
                panelContents.Padding = new Padding(0, 20, 0, 0);
                SDRSharp.CollapsiblePanel.CollapsiblePanel newPanel = new SDRSharp.CollapsiblePanel.CollapsiblePanel();
                newPanel.PanelTitle = plugin.DisplayName + " (Plugin)";
                newPanel.ForeColor = Color.Black;
                newPanel.PanelState = PanelStateOptions.Collapsed;
                newPanel.Controls.Add(panelContents);
                if (this.displayCollapsiblePanel.NextPanel == null)
                {
                    this.displayCollapsiblePanel.NextPanel = newPanel;
                }
                else
                {
                    newPanel.NextPanel = this.displayCollapsiblePanel.NextPanel;
                    this.displayCollapsiblePanel.NextPanel = newPanel;
                }
                newPanel.Width = this.displayCollapsiblePanel.Width;
                newPanel.ExpandedHeight = panelContents.Height;
                newPanel.StateChanged += this.collapsiblePanel_StateChanged;
                panelContents.Width = newPanel.Width;
                this.controlPanel.Controls.Add(newPanel);
            }
        }
        #endregion

        #region [Misc 3]
        public void StartRadio()
        {
            if (this.playStopButton.Text != "Stop")
            {
                this.playStopButton.Text = "Stop";
                this.playStopButton.SetColor(Color.Red);
            }
            if (this._frontendController != null)
            {
                this._frontendController.HideSettingGUI();
            }
            this.Open();
            if (this.frequencyNumericUpDown.Value > 0m && this.frequencyNumericUpDown.Value <= 30000000m)
            {
                if (this.wfmRadioButton.Checked)
                {
                    this.amRadioButton.Checked = true;
                }
            }
            else if (this.frequencyNumericUpDown.Value > 97000000m && this.frequencyNumericUpDown.Value < 106000000m && !this.wfmRadioButton.Checked)
            {
                this.wfmRadioButton.Checked = true;
                if (this.scopeCollapsiblePanel.PanelState == PanelStateOptions.Expanded)
                {
                    this.scopeCollapsiblePanel.PanelState = PanelStateOptions.Collapsed;
                }
            }
            this._streamControl.Play();
            if (this._frontendController != null)
            {
                this._frontendController.HideSettingGUI();
            }
            this.setBftBins();
            this.setFftInterval();
            this._fftStream.Flush();
            this._iftStream.Flush();
            this._aftStream.Flush();
            if (Utils.FastFFT)
            {
                Console.WriteLine("ProcessFft started from Gui Thread");
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessFFT));
                Console.WriteLine("ProcessFft started from ThreadPool");
            }
            this._fftTimer.Enabled = true;
            this.inputDeviceComboBox.Enabled = false;
            this.outputDeviceComboBox.Enabled = false;
            this.latencyNumericUpDown.Enabled = false;
            if (this._frontendController != null)
            {
                this._frontendController.Frequency = this.centerFreqNumericUpDown.Value;
            }
            this.spectrumAnalyzer.StatusText = "";
            this.ifAnalyzer.StatusText = "";
            this.afAnalyzer.StatusText = "";
            this.wideScope.StatusText = "";
            this._floorValueNeeded = 15;
            int samplerate = (int)this._streamControl.SampleRate >> this._vfo.BaseBandDecimationStageCount;
            if (this._vfo.DetectorType == DetectorType.AM || this._vfo.DetectorType == DetectorType.SAM || this._vfo.DetectorType == DetectorType.DSB)
            {
                samplerate >>= 1;
            }
            if (this._streamControl.SoundCardRatio > 1.0)
            {
                samplerate = (int)((double)samplerate / this._streamControl.SoundCardRatio);
            }
            for (int i = 1; i < this.cmbBandWidth.Items.Count; i++)
            {
                int width = this.getFrequency(this.cmbBandWidth.Items[i]);
                this.cmbBandWidth.setEnabled(i, this._vfo.DetectorType == DetectorType.WFM || width <= samplerate);
            }
            if (this._streamControl.InputType == InputType.SoundCard)
            {
                this.remDcSlider.Enabled = true;
                this._iqBalancer.RemoveDC = (((float)this.remDcSlider.Value == 0f) ? 0f : (1f / (float)this.remDcSlider.Value));
                this._iqBalancer.Reset((float)Utils.GetDoubleSetting("IQgain", 1.0), (float)Utils.GetDoubleSetting("IQphase", 0.0));
            }
            else if (this._streamControl.InputType == InputType.Plugin && this.iqSourceComboBox.SelectedItem.ToString().Contains("RTL"))
            {
                this.remDcSlider.Enabled = true;
                this._iqBalancer.RemoveDC = (((float)this.remDcSlider.Value == 0f) ? 0f : (1f / (float)this.remDcSlider.Value));
                this._iqBalancer.Reset((float)Utils.GetDoubleSetting("IQgain", 1.0), (float)Utils.GetDoubleSetting("IQphase", 0.0));
            }
            else
            {
                this.remDcSlider.Enabled = false;
                this._iqBalancer.RemoveDC = 0f;
                this._iqBalancer.Reset(1f, 0f);
            }
            this.NotifyPropertyChanged("StartRadio");
        }

        public unsafe void StopRadio()
        {
            if (this.playStopButton.Text != "Play")
            {
                this.playStopButton.Text = "Play";
                this.playStopButton.SetColor(Color.Lime);
            }
            this._streamControl.Stop();
            this._vfo.SampleRate = 2048000.0;
            this._xBuf = null;
            this._yBuf = null;
            this._fftStream.Write(this._iqPtr, this._fftBins + 1);
            this._iftStream.Write(this._ifqPtr, this._bftBins + 1);
            this._aftStream.Write(this._afqPtr, this._bftBins + 1);
            if (!this.SourceIsWaveFile)
            {
                this.inputDeviceComboBox.Enabled = (this._frontendController == null || this._frontendController.IsSoundCardBased);
            }
            this.latencyNumericUpDown.Enabled = true;
            this.outputDeviceComboBox.Enabled = true;
            this._fftEvent.Set();
            this.spectrumAnalyzer.StatusText = "RF spectrum";
            this.ifAnalyzer.StatusText = "IF baseband [right click to (re)set notch]";
            this.afAnalyzer.StatusText = "AF audio spectrum";
            this.wideScope.StatusText = "AM audio envelope";
            this.NotifyPropertyChanged("StopRadio");
        }

        public unsafe void GetSpectrumSnapshot(byte[] destArray)
        {
            Fourier.ScaleFFT(this._rfSpectrumPtr, this._scaledRfSpectrumPtr, this._rfSpectrumSamples, -130f, 0f);
            fixed (byte* destPtr = destArray)
            {
                Fourier.SmoothCopy(this._scaledRfSpectrumPtr, destPtr, this._rfSpectrumSamples, destArray.Length, 1f, 0);
            }
        }

        public void RegisterStreamHook(object streamHook, ProcessorType processorType)
        {
            if (!this._streamControl.IsPlaying)
            {
                this._vfoHookManager.RegisterStreamHook(streamHook, processorType);
            }
        }

        public void UnregisterStreamHook(object streamHook)
        {
            if (!this._streamControl.IsPlaying)
            {
                this._vfoHookManager.UnregisterStreamHook(streamHook);
            }
        }

        private void NotifyPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.RButton | Keys.MButton | Keys.Space | Keys.Control:
                    if (this.audioGainTrackBar.Value < this.audioGainTrackBar.Maximum)
                    {
                        this.audioGainTrackBar.Value++;
                        return true;
                    }
                    return true;
                case Keys.Back | Keys.Space | Keys.Control:
                    if (this.audioGainTrackBar.Value > this.audioGainTrackBar.Minimum)
                    {
                        this.audioGainTrackBar.Value--;
                        return true;
                    }
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this._keyPreview)
            {
                return;
            }
            Keys keyCode = e.KeyCode;
            if (keyCode > Keys.Space)
            {
                if (keyCode <= Keys.Z)
                {
                    if (keyCode == Keys.Home)
                    {
                        this._floorValueNeeded = 5;
                        goto IL_5E3;
                    }
                    switch (keyCode)
                    {
                        case Keys.D0:
                        case Keys.D1:
                        case Keys.D2:
                        case Keys.D3:
                        case Keys.D4:
                        case Keys.D5:
                        case Keys.D6:
                        case Keys.D7:
                        case Keys.D8:
                        case Keys.D9:
                            break;
                        case Keys.RButton | Keys.Back | Keys.ShiftKey | Keys.Space:
                        case Keys.LButton | Keys.RButton | Keys.Back | Keys.ShiftKey | Keys.Space:
                        case Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space:
                        case Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space:
                        case Keys.RButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space:
                        case Keys.LButton | Keys.RButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space:
                        case (Keys)64:
                        case Keys.E:
                        case Keys.F:
                        case Keys.H:
                        case Keys.I:
                        case Keys.J:
                        case Keys.P:
                        case Keys.Q:
                        case Keys.R:
                        case Keys.S:
                        case Keys.T:
                        case Keys.V:
                        case Keys.X:
                        case Keys.Y:
                            goto IL_5E3;
                        case Keys.A:
                            this.amRadioButton.Checked = true;
                            goto IL_5E3;
                        case Keys.B:
                            this._numEntry = "B";
                            goto IL_5E3;
                        case Keys.C:
                            this._numEntry = "C";
                            goto IL_5E3;
                        case Keys.D:
                            this.dsbRadioButton.Checked = true;
                            goto IL_5E3;
                        case Keys.G:
                            this._numEntry = "G";
                            goto IL_5E3;
                        case Keys.K:
                        case Keys.M:
                            goto IL_2F6;
                        case Keys.L:
                            this.lsbRadioButton.Checked = true;
                            goto IL_5E3;
                        case Keys.N:
                            this.nfmRadioButton.Checked = true;
                            goto IL_5E3;
                        case Keys.O:
                            if (e.Modifiers == Keys.Control)
                            {
                                this.frontendGuiButton_Click(null, null);
                                goto IL_5E3;
                            }
                            goto IL_5E3;
                        case Keys.U:
                            this.usbRadioButton.Checked = true;
                            goto IL_5E3;
                        case Keys.W:
                            this.wfmRadioButton.Checked = true;
                            goto IL_5E3;
                        case Keys.Z:
                            this._numEntry = "Z";
                            goto IL_5E3;
                        default:
                            goto IL_5E3;
                    }
                }
                else
                {
                    switch (keyCode)
                    {
                        case Keys.F1:
                            this.butVfoA.Checked = true;
                            goto IL_5E3;
                        case Keys.F2:
                            this.butVfoB.Checked = true;
                            goto IL_5E3;
                        case Keys.F3:
                            this.butVfoC.Checked = true;
                            goto IL_5E3;
                        case Keys.F4:
                            goto IL_5E3;
                        case Keys.F5:
                            this.playStopButton.Checked = true;
                            goto IL_5E3;
                        default:
                            if (keyCode != Keys.OemPeriod)
                            {
                                goto IL_5E3;
                            }
                            break;
                    }
                }
                this._numEntry += ((e.KeyCode == Keys.OemPeriod) ? "." : ((char)e.KeyValue).ToString());
                goto IL_5E3;
            }
            if (keyCode <= Keys.Return)
            {
                if (keyCode != Keys.Back)
                {
                    if (keyCode != Keys.Return)
                    {
                        goto IL_5E3;
                    }
                }
                else
                {
                    if (this._numEntry.Length > 0)
                    {
                        this._numEntry = this._numEntry.Substring(0, this._numEntry.Length - 1);
                        goto IL_5E3;
                    }
                    goto IL_5E3;
                }
            }
            else
            {
                if (keyCode == Keys.Escape)
                {
                    this._numEntry = "";
                    goto IL_5E3;
                }
                if (keyCode != Keys.Space)
                {
                    goto IL_5E3;
                }
                this.audioButton.Checked = !this.audioButton.Checked;
                goto IL_5E3;
            }
        IL_2F6:
            float val = 0f;
            if (this._numEntry == null)
            {
                return;
            }
            if (this._numEntry.Length == 0)
            {
                return;
            }
            string prefix = this._numEntry.Substring(0, 1);
            if (prefix.CompareTo("9") > 0)
            {
                this._numEntry = this._numEntry.Substring(1);
            }
            if (float.TryParse(this._numEntry, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
            {
                int freq = (int)((double)(val * 1000f) + 0.1);
                if (e.KeyCode == Keys.M)
                {
                    freq *= 1000;
                }
                if (prefix == "B")
                {
                    if ((long)freq <= this.filterBandwidthNumericUpDown.Maximum && (long)freq >= this.filterBandwidthNumericUpDown.Minimum)
                    {
                        this.filterBandwidthNumericUpDown.Value = (long)freq;
                    }
                }
                else if (prefix == "G")
                {
                    float gn = (float)(this.audioGainTrackBar.Maximum - this.audioGainTrackBar.Minimum) * val / 100f;
                    this.audioGainTrackBar.Value = this.audioGainTrackBar.Minimum + (int)gn;
                }
                else if (prefix == "C")
                {
                    int i = this.FrequencyIndex(this.cmbCenterStep, freq.ToString());
                    if (i >= 0)
                    {
                        this.cmbCenterStep.SelectedIndex = i;
                    }
                }
                else if (prefix == "Z")
                {
                    int j = this.FrequencyIndex(this.stepSizeComboBox, freq.ToString());
                    if (j >= 0)
                    {
                        this.stepSizeComboBox.SelectedIndex = j;
                    }
                }
                else if (!this.centerFreqNumericUpDown.Enabled || ((double)freq >= (double)this.waterfall.DisplayFrequency - (double)((float)this.waterfall.SpectrumWidth / this.waterfall.Zoom) / 2.2 && (double)freq <= (double)this.waterfall.DisplayFrequency + (double)((float)this.waterfall.SpectrumWidth / this.waterfall.Zoom) / 2.2))
                {
                    if (freq <= this.frequencyNumericUpDown.Maximum && freq >= this.frequencyNumericUpDown.Minimum)
                    {
                        this.frequencyNumericUpDown.Value = freq;
                    }
                }
                else if ((long)freq <= this.centerFreqNumericUpDown.Maximum && (long)freq >= this.centerFreqNumericUpDown.Minimum)
                {
                    int stepSize = this.getFrequency(this.stepSizeComboBox.Text);
                    int offset = (int)((float)this.waterfall.SpectrumWidth / this.waterfall.Zoom / 20f);
                    offset = offset / stepSize * stepSize;
                    this.centerFreqNumericUpDown.Value = (long)(freq - offset);
                    this.frequencyNumericUpDown.Value = freq;
                    this.agcCheckBox.Checked = true;
                }
            }
            this._numEntry = "";
        IL_5E3:
            if (this._numEntry.Length == 0)
            {
                this.frequencyNumericUpDown.ShowValue((int)this.waterfall.Frequency);
                return;
            }
            if (this._numEntry.Substring(0, 1) != "B")
            {
                float val2 = 0f;
                if (float.TryParse(this._numEntry, NumberStyles.Any, CultureInfo.InvariantCulture, out val2))
                {
                    this.frequencyNumericUpDown.ShowValue((int)((double)(val2 * 1000f) + 0.1));
                }
            }
        }

        private void butVfo_CheckedChanged(object sender, EventArgs e)
        {
            gButton butVfo = (gButton)sender;
            if (!butVfo.Checked)
            {
                butVfo.Tag = this.makeVfoTag();
            }
            else
            {
                string tag = (string)butVfo.Tag;
                if (tag == null || tag.Length == 0)
                {
                    butVfo.Tag = this.makeVfoTag();
                }
                else
                {
                    string[] fields = tag.Split(new char[]
                    {
                        ','
                    });
                    if (fields.GetUpperBound(0) >= 3)
                    {
                        int freq;
                        int.TryParse(fields[1], out freq);
                        int value;
                        if (freq < this.centerFreqNumericUpDown.Value + this._frequencyShift - (decimal)this._streamControl.SampleRate / 2m || freq > this.centerFreqNumericUpDown.Value + this._frequencyShift + (decimal)this._streamControl.SampleRate / 2m)
                        {
                            if (this.SourceIsWaveFile)
                            {
                                return;
                            }
                            int.TryParse(fields[0], out value);
                            this.centerFreqNumericUpDown.Value = (long)value;
                        }
                        this.frequencyNumericUpDown.Value = freq;
                        int.TryParse(fields[2], out value);
                        this.DetectorType = (DetectorType)value;
                        int.TryParse(fields[3], out value);
                        this.filterBandwidthNumericUpDown.Value = (long)value;
                        this.agcCheckBox.Checked = true;
                        this.frequencyNumericUpDown_ValueChanged(null, null);
                        this.panview_AutoZoomed(null, null);
                    }
                }
            }
            this.mnuVfoA.Checked = this.butVfoA.Checked;
            this.mnuVfoB.Checked = this.butVfoB.Checked;
            this.mnuVfoC.Checked = this.butVfoC.Checked;
        }

        private string makeVfoTag()
        {
            return string.Concat(new string[]
            {
                this.CenterFrequency.ToString(),
                ", ",
                this.Frequency.ToString(),
                ", ",
                ((int)this.DetectorType).ToString(),
                ", ",
                this.FilterBandwidth.ToString()
            });
        }

        private void chkIndepSideband_Changed(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.IndepSideband = this.chkIndepSideband.Checked;
            this.waterfall.IndepSideband = this.chkIndepSideband.Checked;
            this.ifAnalyzer.IndepSideband = this.chkIndepSideband.Checked;
            this.ifWaterfall.IndepSideband = this.chkIndepSideband.Checked;
        }

        private int FrequencyIndex(gCombo cmb, string text)
        {
            int freq = this.getFrequency(text);
            int i = cmb.Items.Count - 1;
            while (i >= 0 && this.getFrequency(cmb.Items[i]) != freq)
            {
                i--;
            }
            return i;
        }

        private void playStopButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.IsInputKey = true;
            }
        }

        private void chkScrollPanel_CheckedChanged(object sender, EventArgs e)
        {
            this.scrollPanel.Visible = this.chkScrollPanel.Checked; //HERE
            if (this.scrollPanel.Visible)
            {
                this.panSplitContainer.Left = this.scrollPanel.Width + 1;
                this.panSplitContainer.Width = this.tbSpanSA.Left - this.scrollPanel.Width - 12;
            }
            else
            {
                this.panSplitContainer.Left = this.scrollPanel.Left;
                this.panSplitContainer.Width = this.tbSpanSA.Left - 20;
            }
            this._fftSkips = -2;
        }

        private unsafe void calcFFTs()
        {
            int size = 16384;
            this._stopwatch.Reset();
            this._stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                Fourier.ForwardTransformOrgOrg(this._fftPtr, size);
            }
            this._stopwatch.Stop();
            Console.WriteLine("dummy " + this._stopwatch.ElapsedMilliseconds.ToString());
            this._stopwatch.Reset();
            this._stopwatch.Start();
            for (int j = 0; j < 100; j++)
            {
                Fourier.ForwardTransformOrgOrg(this._fftPtr, size);
            }
            this._stopwatch.Stop();
            Console.WriteLine("Org(float) " + this._stopwatch.ElapsedMilliseconds.ToString());
            this._stopwatch.Reset();
            this._stopwatch.Start();
            for (int k = 0; k < 100; k++)
            {
                Fourier.ForwardTransformOrg(this._fftPtr, size);
            }
            this._stopwatch.Stop();
            Console.WriteLine("Org(double) " + this._stopwatch.ElapsedMilliseconds.ToString());
            this._stopwatch.Reset();
            this._stopwatch.Start();
            for (int l = 0; l < 100; l++)
            {
                Fourier.ForwardTransformLut(this._fftPtr, size);
            }
            this._stopwatch.Stop();
            Console.WriteLine("Lut " + this._stopwatch.ElapsedMilliseconds.ToString());
            this._stopwatch.Reset();
            this._stopwatch.Start();
            for (int m = 0; m < 100; m++)
            {
                Fourier.ForwardTransformRot(this._fftPtr, size);
            }
            this._stopwatch.Stop();
            Console.WriteLine("Rot " + this._stopwatch.ElapsedMilliseconds.ToString());
        }

        private void chkNotch_CheckedChanged(object sender, EventArgs e)
        {
            if (!Utils.FastConvolve)
            {
                MessageBox.Show("Fast convolve not enabled, notching not supplied\n Stop Radio, click 'Fast Convolve' and Start Radio");
            }
            gButton but = (gButton)sender;
            int num = Utils.Val(but.Name.Substring(but.Name.Length - 1, 1), 0);
            this.spectrumAnalyzer.SetNotch(num, 0, 0, but.Checked);
            this.ifAnalyzer.SetNotch(num, 0, 0, but.Checked);
            if (but.Checked && !this.chkIF.Checked)
            {
                this.chkIF.Checked = true;
                MessageBox.Show("Use below 'IF Baseband spectrum' and drag mouse to move/resize notch.");
            }
        }

        private void sampleRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._formLoaded)
            {
                return;
            }
            if (!this._streamControl.IsPlaying)
            {
                return;
            }
            this.StopRadio();
            this._vfo.AgcThreshold -= 1f;
            this._vfo.AgcThreshold += 1f;
            this.StartRadio();
            this.modeRadioButton_CheckStateChanged(null, null);
        }

        private void chkBaseBand_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkNLimiter_CheckedChanged(object sender, EventArgs e)
        {
            this.tbNLRatio.Enabled = this.chkNLimiter.Checked;
            this.tbNLTreshold.Enabled = this.chkNLimiter.Checked;
        }

        private void tbNLTreshold_ValueChanged(object sender, EventArgs e)
        {
            this._nLimiter.Treshold = this.tbNLTreshold.Value;
            this.labNLTreshold.Text = this.tbNLTreshold.Value.ToString();
        }

        private void tbNLRatio_ValueChanged(object sender, EventArgs e)
        {
            this._nLimiter.Ratio = (double)this.tbNLRatio.Value;
            this.labNLRatio.Text = Math.Pow(10.0, (double)(-1f + (float)this.tbNLRatio.Value / 50f)).ToString("0.0");
        }

        private void tbRFgain_ValueChanged(object sender, EventArgs e)
        {
            this._vfo.RFgain = (double)this.tbRFgain.Value;
            Console.WriteLine("RFgain=" + this.tbRFgain.Value.ToString());
            this.wideScope.Reset();
        }

        private void gThumb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            this._oldTop = this.gThumb.Top;
            this._oldCur = Control.MousePosition.Y;
        }

        private void gThumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._oldCur == 0 || e.Button != MouseButtons.Left)
            {
                return;
            }
            this.SetThumb(this._oldTop + Control.MousePosition.Y - this._oldCur);
        }

        private void gThumb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this._oldCur = 0;
            }
        }

        private void pnlScroll_MouseDown(object sender, MouseEventArgs e)
        {
            this.SetThumb(this.pnlScroll.Top + e.Y);
        }

        private void controlPanel_Resize(object sender, EventArgs e)
        {
            this.SetThumb(this.gThumb.Top);
        }

        private void SetThumb(int newPos)
        {
            int h = (int)((float)this.scrollPanel.Height / (float)this.controlPanel.Height * (float)this.pnlScroll.Height);
            if (h > this.pnlScroll.Height)
            {
                h = this.pnlScroll.Height;
            }
            if (this.gThumb.Height != h)
            {
                this.gThumb.Height = h;
            }
            int p = Math.Max(0, Math.Min(newPos, this.pnlScroll.Height - this.gThumb.Height));
            if (p != this.gThumb.Top)
            {
                this.gThumb.Top = p;
            }
            p = -(int)((float)p / (float)this.pnlScroll.Height * (float)this.controlPanel.Height);
            if (this.controlPanel.Top != p)
            {
                this.controlPanel.Top = p;
            }
            base.Invalidate();
        }

        private void tbvCarrierAvg_ValueChanged(object sender, EventArgs e)
        {
            this.scope.CarrierAvg = this.tbvCarrierAvg.Value;
        }

        private void tbvAudioRel_ValueChanged(object sender, EventArgs e)
        {
            this.scope.AudioRel = this.tbvAudioRel.Value;
        }

        private void tbvAudioAvg_ValueChanged(object sender, EventArgs e)
        {
            this.scope.AudioAvg = this.tbvAudioAvg.Value;
        }

        private void tbvPeakDelay_ValueChanged(object sender, EventArgs e)
        {
            this.scope.PeakDelay = this.tbvPeakDelay.Value;
        }

        private void tbvPeakRel_ValueChanged(object sender, EventArgs e)
        {
            this.scope.PeakRel = this.tbvPeakRel.Value;
        }

        private void gBexpand_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.gBexpand.Checked)
            {
                this.scopeCollapsiblePanel.ExpandedHeight = this.scope.Top + this.scope.Height + 5;
            }
            else if (!this.gBexpandScope.Checked)
            {
                this.scopeCollapsiblePanel.ExpandedHeight = this.chkXY.Top + this.chkXY.Height + 10;
            }
            else
            {
                this.scopeCollapsiblePanel.ExpandedHeight = this.chkAver.Top + this.chkAver.Height + 8;
            }
            if (this.scopeCollapsiblePanel.PanelState == PanelStateOptions.Expanded)
            {
                this.scopeCollapsiblePanel.Height = this.scopeCollapsiblePanel.ExpandedHeight;
            }
        }

        private void gBexpandScope_CheckedChanged(object sender, EventArgs e)
        {
            this.gBexpand_CheckedChanged(sender, e);
        }

        private void gBsetScale_CheckedChanged(object sender, EventArgs e)
        {
            this._floorValueNeeded = 5;
        }

        private void gUpDown_Enter(object sender, EventArgs e)
        {
            this._keyPreview = false;
        }

        private void gpUpDown_Leave(object sender, EventArgs e)
        {
            this._keyPreview = true;
        }

        private void tbContrasAG_ValueChanged(object sender, EventArgs e)
        {
            int intensityValue = this.tbIntensityAg.Minimum + this.tbIntensityAg.Maximum - this.tbIntensityAg.Value;
            int contrastValue = this.tbContrastAg.Minimum + this.tbContrastAg.Maximum - this.tbContrastAg.Value;
            if (intensityValue + contrastValue > 25)
            {
                return;
            }
            this.audiogram.MinPower = (float)intensityValue;
            this.audiogram.MaxPower = (float)(intensityValue + contrastValue);
        }

        private void tbIntensityAG_ValueChanged(object sender, EventArgs e)
        {
            int intensityValue = this.tbIntensityAg.Minimum + this.tbIntensityAg.Maximum - this.tbIntensityAg.Value;
            int contrastValue = this.tbContrastAg.Minimum + this.tbContrastAg.Maximum - this.tbContrastAg.Value;
            if (intensityValue + contrastValue > 40)
            {
                return;
            }
            this.audiogram.MinPower = (float)intensityValue;
            this.audiogram.MaxPower = (float)(intensityValue + contrastValue);
        }

        private void tbAgSpeed_ValueChanged(object sender, EventArgs e)
        {
            this.setFftInterval();
            this.startSpeedCalc();
        }

        private unsafe void fftAverageUpDown_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this._actualFftBins; i++)
            {
                this._rfAveragePtr[i] = this._rfSpectrumPtr[i];
            }
        }

        private void logFactorUpDown_ValueChanged(object sender, EventArgs e)
        {
        }

        private void btnShowLog_CheckedChanged(object sender, EventArgs e)
        {
            this.audiogram.ShowLog = this.btnShowLog.Checked;
            this.setBftBins();
        }

        private void setFftInterval()
        {
            int wfSpeed = this.fftSpeedTrackBar.Value;
            int agSpeed = this.tbAgSpeed.Value;
            int oldInterval = this._fftTimer.Interval;
            if (wfSpeed == this.fftSpeedTrackBar.Maximum || agSpeed == this.tbAgSpeed.Maximum)
            {
                this._fftTimer.Interval = 10;
            }
            else if (wfSpeed == this.fftSpeedTrackBar.Maximum - 1 || agSpeed == this.tbAgSpeed.Maximum - 1)
            {
                this._fftTimer.Interval = 20;
            }
            else
            {
                this._fftTimer.Interval = 40;
            }
            int incr = 0;
            if (this._fftTimer.Interval == 20)
            {
                incr = 1;
            }
            if (this._fftTimer.Interval == 40)
            {
                incr = 2;
            }
            wfSpeed += incr;
            agSpeed += incr;
            if (Utils.FastFFT)
            {
                this._spectrumTimeout = 120 / this._fftTimer.Interval;
            }
            else
            {
                this._spectrumTimeout = 40 / this._fftTimer.Interval;
            }
            int val = this.fftSpeedTrackBar.Maximum + this.fftSpeedTrackBar.Minimum - wfSpeed;
            this._waterfallTimout = (int)Math.Pow(2.0, (double)val);
            val = this.tbAgSpeed.Maximum + this.tbAgSpeed.Minimum - agSpeed;
            this._audiogramTimout = (int)Math.Pow(2.0, (double)val);
            if (Utils.FastFFT && this._waterfallTimout * this._fftTimer.Interval < 40)
            {
                this.waterfall.UseTimestamps = 0;
            }
            else
            {
                this.waterfall.UseTimestamps = ((!this.useTimestampsCheckBox.Checked) ? 0 : ((this.useTimestampsCheckBox.Text == "Time") ? 1 : 2));
            }
            if (this._fftTimer.Interval != oldInterval)
            {
                this._delta100 = this._delta100 * (long)this._fftTimer.Interval / (long)oldInterval;
            }
            this._fftSkips = -1;
        }

        private void waterfall_Resize(object sender, EventArgs e)
        {
            this.showWaterfallSpeed();
        }

        private void startSpeedCalc()
        {
            this._stopwatch.Reset();
            this._count100 = 100;
            this._stopwatch.Start();
        }

        private void getWaterfallSpeed()
        {
            if (this._count100 <= 0)
            {
                return;
            }
            if (--this._count100 > 0)
            {
                return;
            }
            this._stopwatch.Stop();
            this._delta100 = this._stopwatch.ElapsedMilliseconds;
            this._stopwatch.Reset();
            this.showWaterfallSpeed();
        }

        private void showWaterfallSpeed()
        {
            if (this._delta100 == 0L)
            {
                return;
            }
            this.waterfall.ScanLineMsec = (int)((long)this._waterfallTimout * this._delta100 / 100L);
            this.ifWaterfall.ScanLineMsec = (int)((long)this._waterfallTimout * this._delta100 / 100L);
            this.audiogram.ScanLineMsec = (int)((long)((this._audiogramTimout == 1) ? 1 : (this._audiogramTimout / 2)) * this._delta100 / 100L);
            float secs;
            if (this.waterfall.Horizontal)
            {
                secs = (float)((long)this._waterfallTimout * this._delta100 * (long)this.waterfall.Width) / 100000f;
            }
            else
            {
                secs = (float)((long)this._waterfallTimout * this._delta100 * (long)this.waterfall.Height) / 100000f;
            }
            if (secs < 10f)
            {
                this.labSpeed.Text = string.Format("{0:0.0} s", secs);
                return;
            }
            if (secs < 100f)
            {
                this.labSpeed.Text = string.Format("{0:#0} s", secs);
                return;
            }
            if (secs < 600f)
            {
                this.labSpeed.Text = string.Format("{0:#0}m{1:00}", (int)(secs / 60f), (int)(secs % 60f));
                return;
            }
            if (secs < 3600f)
            {
                this.labSpeed.Text = string.Format("{0:#0} m", (int)(secs / 60f));
                return;
            }
            this.labSpeed.Text = string.Format("{0:#0}h{1:00}", (int)(secs / 3600f), (int)(secs % 3600f / 60f));
        }

        private void collapsiblePanel_StateChanged(object sender, EventArgs e)
        {
            SDRSharp.CollapsiblePanel.CollapsiblePanel pnl = (SDRSharp.CollapsiblePanel.CollapsiblePanel)sender;
            if (pnl.PanelTitle.Contains("Recording"))
            {
                this.spectrumAnalyzer.CenterSnap = (pnl.PanelState == PanelStateOptions.Expanded);
                this.waterfall.CenterSnap = (pnl.PanelState == PanelStateOptions.Expanded);
                if (pnl.PanelState == PanelStateOptions.Expanded)
                {
                    int step = Math.Min(1000, this.spectrumAnalyzer.StepSize);
                    long f = this.centerFreqNumericUpDown.Value;
                    this.centerFreqNumericUpDown.Value = (f + (long)(Math.Sign(f) * step / 2)) / (long)step * (long)step;
                }
            }
        }

        private void audioDeviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = ((gCombo)sender).Name;
            string item = ((gCombo)sender).SelectedItem.ToString().ToUpper();
            if (name.Contains("output"))
            {
                this._streamControl.UseASIO = (item.Contains("ASIO") || item.Contains("CABL") || item.Contains("VIRT"));
            }
            if (!this._formLoaded)
            {
                return;
            }
            if (!item.ToUpper().Contains("[WDS]"))
            {
                return;
            }
            for (int i = 0; i < this.outputDeviceComboBox.Items.Count; i++)
            {
                string alt = this.outputDeviceComboBox.Items[i].ToString();
                if (alt.ToUpper().Contains("[MME]"))
                {
                    MessageBox.Show("Please use [MME] device, this might give lower latencies.");
                    return;
                }
            }
        }

        private void mnuSetNotch_Click(object sender, EventArgs e)
        {
            if (!this.chkNotch0.Checked)
            {
                this.chkNotch0.Checked = true;
                return;
            }
            if (!this.chkNotch1.Checked)
            {
                this.chkNotch1.Checked = true;
                return;
            }
            if (!this.chkNotch2.Checked)
            {
                this.chkNotch2.Checked = true;
                return;
            }
            if (!this.chkNotch3.Checked)
            {
                this.chkNotch3.Checked = true;
            }
        }

        private void MnuClearNotch_Click(object sender, EventArgs e)
        {
            if (this.chkNotch3.Checked)
            {
                this.chkNotch3.Checked = false;
                return;
            }
            if (this.chkNotch2.Checked)
            {
                this.chkNotch2.Checked = false;
                return;
            }
            if (this.chkNotch1.Checked)
            {
                this.chkNotch1.Checked = false;
                return;
            }
            if (this.chkNotch0.Checked)
            {
                this.chkNotch0.Checked = false;
            }
        }

        private void mnuVfo_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mnu = (ToolStripMenuItem)sender;
            string a;
            if ((a = mnu.Text.Substring(4)) != null)
            {
                if (a == "A")
                {
                    this.butVfoA.Checked = true;
                    return;
                }
                if (a == "B")
                {
                    this.butVfoB.Checked = true;
                    return;
                }
                if (!(a == "C"))
                {
                    return;
                }
                this.butVfoC.Checked = true;
            }
        }

        private void mnuAutoScale_Click(object sender, EventArgs e)
        {
            this._floorValueNeeded = 5;
        }

        private void mnuSetColors_Click(object sender, EventArgs e)
        {
            string parent = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.Name;
            if (parent.Contains("lyzer"))
            {
                this.gradientButtonSA_Click(this, null);
                return;
            }
            if (parent.Contains("fall"))
            {
                this.gradientButtonWF_Click(this, null);
                return;
            }
            if (parent.Contains("scope"))
            {
                this.gradientButtonSA_Click(this, null);
                return;
            }
            this.gradientButtonAG_Click(this, null);
        }

        private void mnuShowWaterfall_Click(object sender, EventArgs e)
        {
            this.chkWF.Checked = !this.chkWF.Checked;
        }

        private void mnuShowBaseband_Click(object sender, EventArgs e)
        {
            this.chkIF.Checked = !this.chkIF.Checked;
        }

        private void mnuShowAudio_Click(object sender, EventArgs e)
        {
            this.cmbAudio.SelectedIndex = (this.mnuShowAudio.Checked ? 0 : 1);
        }

        private void mnuShowAudiogram_Click(object sender, EventArgs e)
        {
            this.cmbAudio.SelectedIndex = (this.mnuShowAudiogram.Checked ? 0 : 2);
        }

        private void mnuShowEnvelope_Click(object sender, EventArgs e)
        {
            this.cmbAudio.SelectedIndex = (this.mnuShowEnvelope.Checked ? 0 : 3);
        }

        private void mnuStationList_Click(object sender, EventArgs e)
        {
            this.spectrumAnalyzer.ShowStationList();
        }

        private void MnuSA_Opening(object sender, CancelEventArgs e)
        {
            string parent = ((ContextMenuStrip)sender).SourceControl.Name;
            this.mnuStationList.Visible = parent.Contains("pectrum");
            this.mnuAutoScale.Visible = this.mnuStationList.Visible;
        }

        private void chkAutoSize_CheckedChanged(object sender, EventArgs e)
        {
            this.panview_AutoZoomed(null, null);
        }

        private void playBar_ValueChanged(object sender, EventArgs e)
        {
            if (this._streamControl.WaveFile == null)
            {
                return;
            }
            long position = (long)((float)this._streamControl.WaveFile.Size * this.playBar.Fraction);
            this._streamControl.WaveFile.Position = position;
            DateTime start = DateTime.Now.AddSeconds((double)(-1f * ((float)this._duration - (float)this._streamControl.WaveFile.Duration * (1f - this.playBar.Fraction))));
            this._streamControl.WaveStart = start;
        }

        private void waterfall_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void panSplitContainer4_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.panSplitContainer5.SplitterDistance = e.SplitY;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            this._isActive = true;
            this._fftSkips = -2;
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            this._isActive = false;
        }

        private void fastFftCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this._streamControl.IsPlaying)
            {
                this.StopRadio();
            }
            Utils.FastFFT = this.fastFftCheckBox.Checked;
        }

        private void audioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.audioButton.Text = (this.audioButton.Checked ? "Mute" : "Audio");
        }

        private void remDcSlider_ValueChanged(object sender, EventArgs e)
        {
            if (this.remDcSlider.Enabled)
            {
                this._iqBalancer.RemoveDC = ((this.remDcSlider.Value == 0) ? 0f : (1f / (float)this.remDcSlider.Value));
            }
        }

        private void gButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadAboutBox1 _About = new RadAboutBox1();
            _About.Show();

            /*
            string helpFile = "help.txt";
            if (File.Exists(helpFile))
            {
                Process.Start(helpFile);
                return;
            }
            MessageBox.Show(helpFile + " not found.");
            */
        }
        #endregion

        #region [Declarations]
        private const int FREQ = 0;
        private const int TIME = 1;
        private const int DAYS = 2;
        private const int CTRY = 3;
        private const int NAME = 4;
        private const int LANG = 5;
        private const int DBUV = 6;
        private const int SITE = 7;
        private const int EMP = 8;
        private const int DIST = 9;
        private const int POWR = 10;
        private const int MaxFFTBins = 4194304;
        private const int MaxBFTBins = 32768;
        static DateTime now = DateTime.Now;
        public static string arch = getArch();

#if DEBUG
        private static string _baseTitle = "SDRSharper Revised "; // + now.ToLocalTime().ToShortTimeString() + " " + now.ToLocalTime().ToShortDateString();
#else
    private static string _baseTitle = "SDRSharper Revised "; // + now.ToLocalTime().ToShortTimeString() + " " + now.ToLocalTime().ToShortDateString();
#endif

        private static readonly int[] _defaultAMState = new int[]
        {
            9000,
            450,
            3,
            50,
            0,
            1,
            4
        };

        private static readonly int[] _defaultDSBState = new int[]
        {
            6000,
            500,
            3,
            50,
            0,
            1,
            2
        };

        private static readonly int[] _defaultSSBState = new int[]
        {
            2400,
            500,
            3,
            50,
            0,
            1,
            2
        };

        private static readonly int[] _defaultCWState = new int[]
        {
            300,
            800,
            3,
            50,
            0,
            1,
            2
        };

        private static readonly int[] _defaultNFMState = new int[]
        {
            8000,
            300,
            3,
            50,
            1,
            1,
            11
        };

        private static readonly int[] _defaultWFMState = new int[]
        {
            180000,
            100,
            3,
            50,
            0,
            1,
            14
        };

        private static readonly int[] _defaultRAWState = new int[]
        {
            10000,
            450,
            3,
            50,
            0,
            1,
            4
        };

        private WindowType _fftWindowType;
        private IFrontendController _frontendController;
        private readonly Dictionary<string, IFrontendController> _frontendControllers = new Dictionary<string, IFrontendController>();
        private readonly IQBalancer _iqBalancer;
        private readonly Vfo _vfo;
        private readonly VfoHookManager _vfoHookManager;
        private readonly StreamControl _streamControl;
        private FractResampler _fractResampler;
        private FloatDecimator _audioDecimator;
        private readonly Limiter _nLimiter = new Limiter();
        private readonly ComplexFifoStream _fftStream = new ComplexFifoStream(BlockMode.BlockingRead);
        private readonly ComplexFifoStream _iftStream = new ComplexFifoStream(BlockMode.BlockingRead);
        private readonly ComplexFifoStream _aftStream = new ComplexFifoStream(BlockMode.BlockingRead);
        private readonly FloatFifoStream _spectrumStream = new FloatFifoStream(BlockMode.None);
        private readonly UnsafeBuffer _fftWindow = UnsafeBuffer.Create(4194304, 4);
        private unsafe readonly float* _fftWindowPtr;
        private readonly UnsafeBuffer _bftWindow = UnsafeBuffer.Create(2097152, 4);
        private unsafe readonly float* _bftWindowPtr;
        private unsafe readonly UnsafeBuffer _iqBuffer = UnsafeBuffer.Create(4194304, sizeof(Complex));
        private unsafe readonly Complex* _iqPtr;
        private unsafe readonly UnsafeBuffer _fftBuffer = UnsafeBuffer.Create(4194304, sizeof(Complex));
        private unsafe readonly Complex* _fftPtr;
        private readonly UnsafeBuffer _rfSpectrum = UnsafeBuffer.Create(4194304, 4);
        private unsafe readonly float* _rfSpectrumPtr;
        private readonly UnsafeBuffer _rfAverage = UnsafeBuffer.Create(4194304, 4);
        private unsafe readonly float* _rfAveragePtr;
        private readonly UnsafeBuffer _scaledRfSpectrum = UnsafeBuffer.Create(4194304, 1);
        private unsafe readonly byte* _scaledRfSpectrumPtr;
        private unsafe readonly UnsafeBuffer _afqBuffer = UnsafeBuffer.Create(4194304, sizeof(Complex));
        private unsafe readonly Complex* _afqPtr;
        private unsafe readonly UnsafeBuffer _aftBuffer = UnsafeBuffer.Create(4194304, sizeof(Complex));
        private unsafe readonly Complex* _aftPtr;
        private readonly UnsafeBuffer _afSpectrum = UnsafeBuffer.Create(4194304, 4);
        private unsafe readonly float* _afSpectrumPtr;
        private readonly UnsafeBuffer _scaledAfSpectrum = UnsafeBuffer.Create(4194304, 1);
        private unsafe readonly byte* _scaledAfSpectrumPtr;
        private unsafe readonly UnsafeBuffer _ifqBuffer = UnsafeBuffer.Create(4194304, sizeof(Complex));
        private unsafe readonly Complex* _ifqPtr;
        private unsafe readonly UnsafeBuffer _iftBuffer = UnsafeBuffer.Create(4194304, sizeof(Complex));
        private unsafe readonly Complex* _iftPtr;
        private readonly UnsafeBuffer _iftSpectrum = UnsafeBuffer.Create(4194304, 4);
        private unsafe readonly float* _iftSpectrumPtr;
        private readonly UnsafeBuffer _scaledIFTSpectrum = UnsafeBuffer.Create(4194304, 1);
        private unsafe readonly byte* _scaledIFTSpectrumPtr;
        private readonly SharpEvent _fftEvent = new SharpEvent(false);
        private UnsafeBuffer _xBuf;
        private unsafe float* _xPtr;
        private UnsafeBuffer _yBuf;
        private unsafe float* _yPtr;
        private UnsafeBuffer _audioBuf;
        private unsafe Complex* _audioPtr;
        private UnsafeBuffer _envBuf;
        private unsafe float* _envPtr;
        private System.Windows.Forms.Timer _fftTimer;
        private System.Windows.Forms.Timer _performTimer;
        private long _frequencyToSet;
        private long _frequencySet;
        private long _frequencyShift;
        private int _maxIQSamples;
        private int _maxAFSamples;
        private int _maxIFSamples;
        private int _fftBins;
        private int _bftBins;
        private int _actualFftBins;
        private int _actualIftBins;
        private int _actualAftBins;
        private int _rfSpectrumSamples;
        private int _afSpectrumSamples;
        private int _ifSpectrumSamples;
        private bool _rfSpectrumAvailable;
        private bool _afSpectrumAvailable;
        private bool _ifSpectrumAvailable;
        private bool _fftBufferIsWaiting;
        private bool _extioChangingFrequency;
        private bool _extioChangingSamplerate;
        private bool _terminated;
        private string _waveFile;
        private int _duration;
        private Point _lastLocation;
        private Size _lastSize;
        private bool _initializing;
        private bool _formLoaded;
        private int _gainValueNeeded = -1;
        private int _floorValueNeeded = -99;
        private float _fftMinimum;
        private float _fftAverageMin;
        private string _numEntry = "";
        private bool _keyPreview = true;
        private int _spectrumTimeout = 3;
        private int _rfTimout;
        private int _ifTimeout;
        private int _afTimeout;
        private int _waterfallTimout = 10;
        private int _audiogramTimout = 10;
        private bool _frequencyChangeBusy;
        private bool _bandwidthChangeBusy;
        private float _frequencyBounds = 2.2f;
        private Stopwatch _stopwatch = new Stopwatch();
        private int _count100;
        private long _delta100;
        private bool _speedError;
        private int _fftSkips;
        private int _fftSkipCnt;
        private bool _isActive;
        private Dictionary<int, string> _frequencyList = new Dictionary<int, string>();
        private int _maxStations = 10;
        private ColorBlend[] _saBlends = new ColorBlend[5];
        private ColorBlend[] _wfBlends = new ColorBlend[5];
        private ColorBlend[] _agBlends = new ColorBlend[5];
        private int[] _backgrounds = new int[10];
        private int[] _traceColors = new int[10];
        private int[] _spectrumFill = new int[10];
        private int _wfBlendIndex = 1;
        private int _saBlendIndex = 1;
        private int _agBlendIndex = 1;
        private int _oldCur;
        private int _oldTop;
        private List<AudioDevice> _inputDevices = new List<AudioDevice>();
        private List<AudioDevice> _outputDevices = new List<AudioDevice>();
        private readonly Dictionary<string, ISharpPlugin> _sharpPlugins = new Dictionary<string, ISharpPlugin>();
        private readonly Dictionary<DetectorType, int[]> _modeStates = new Dictionary<DetectorType, int[]>();
        private SharpControlProxy _sharpControlProxy;
        private float _fftGain;
        private float _bftGain;
        private float _AsioCorrection;
        private int _prevSize;
        private int _minSize;
        private int _maxSize = 10000;
        private bool _upwards;
        private bool _colorInvokeBusy;
        private Counter _fftTmr = new Counter();
        private Counter _performTmr = new Counter();
        private Counter _processTmr = new Counter();

        public struct Setting
        {
            public Setting(string key, string value)
            {
                this.Key = key;
                this.Value = value;
            }
            [XmlAttribute]
            public string Key;
            [XmlAttribute]
            public string Value;
        }
        #endregion
    }
}
