using SDRSharp.PanView;

namespace SDRSharp
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm, global::SDRSharp.Common.ISharpControl, global::System.ComponentModel.INotifyPropertyChanged
    {
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openDlg = new System.Windows.Forms.OpenFileDialog();
            this.panSplitContainer = new System.Windows.Forms.SplitContainer();
            this.spectrumAnalyzer = new SDRSharp.PanView.SpectrumAnalyzer();
            this.panSplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.waterfall = new SDRSharp.PanView.Waterfall();
            this.panSplitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panSplitContainer4 = new System.Windows.Forms.SplitContainer();
            this.ifAnalyzer = new SDRSharp.PanView.SpectrumAnalyzer();
            this.ifWaterfall = new SDRSharp.PanView.Waterfall();
            this.panSplitContainer5 = new System.Windows.Forms.SplitContainer();
            this.panelAG = new System.Windows.Forms.Panel();
            this.btnShowLog = new SDRSharp.Controls.gButton();
            this.tbIntensityAg = new SDRSharp.Controls.gSliderV();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.frequencyNumericUpDown = new SDRSharp.PanView.Indicator();
            this.audioButton = new SDRSharp.Controls.gButton();
            this.tbContrastAg = new SDRSharp.Controls.gSliderV();
            this.tbAgSpeed = new SDRSharp.Controls.gSliderV();
            this.playBar = new SDRSharp.Controls.gProgress();
            this.fftSpeedTrackBar = new SDRSharp.Controls.gSliderV();
            this.tbFloorSA = new SDRSharp.Controls.gSliderV();
            this.tbSpanSA = new SDRSharp.Controls.gSliderV();
            this.tbContrastWv = new SDRSharp.Controls.gSliderV();
            this.gBsetScale = new SDRSharp.Controls.gButton();
            this.fftZoomCombo = new SDRSharp.Controls.gCombo();
            this.chkLock = new SDRSharp.Controls.gButton();
            this.cmbBandWidth = new SDRSharp.Controls.gCombo();
            this.iqSourceComboBox = new SDRSharp.Controls.gCombo();
            this.playStopButton = new SDRSharp.Controls.gButton();
            this.configureSourceButton = new SDRSharp.Controls.gButton();
            this.audioGainTrackBar = new SDRSharp.Controls.gSliderH();
            this.scope = new SDRSharp.PanView.Scope();
            this.fastFftCheckBox = new SDRSharp.Controls.gButton();
            this.bftResolutionComboBox = new SDRSharp.Controls.gCombo();
            this.chkBaseBand = new SDRSharp.Controls.gButton();
            this.latencyNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.fftWindowComboBox = new SDRSharp.Controls.gCombo();
            this.chkFastConv = new SDRSharp.Controls.gButton();
            this.filterOrderNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.filterTypeComboBox = new SDRSharp.Controls.gCombo();
            this.swapIQCheckBox = new SDRSharp.Controls.gButton();
            this.squelchNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.useSquelchCheckBox = new SDRSharp.Controls.gButton();
            this.stepSizeComboBox = new SDRSharp.Controls.gCombo();
            this.snapFrequencyCheckBox = new SDRSharp.Controls.gButton();
            this.cmbDbm = new SDRSharp.Controls.gCombo();
            this.agcCheckBox = new SDRSharp.Controls.gButton();
            this.rawRadioButton = new SDRSharp.Controls.gButton();
            this.correctIQCheckBox = new SDRSharp.Controls.gButton();
            this.gBexpand = new SDRSharp.Controls.gButton();
            this.tbvPeakRel = new SDRSharp.Controls.gSliderV();
            this.tbvPeakDelay = new SDRSharp.Controls.gSliderV();
            this.tbvAudioAvg = new SDRSharp.Controls.gSliderV();
            this.tbvAudioRel = new SDRSharp.Controls.gSliderV();
            this.tbvCarrierAvg = new SDRSharp.Controls.gSliderV();
            this.tbTrigL = new SDRSharp.Controls.gSliderH();
            this.cmbHchannel = new SDRSharp.Controls.gCombo();
            this.tbGain = new SDRSharp.Controls.gSliderH();
            this.tbAverage = new SDRSharp.Controls.gSliderH();
            this.chkHrunDC = new SDRSharp.Controls.gButton();
            this.chkAver = new SDRSharp.Controls.gButton();
            this.chkVrunDC = new SDRSharp.Controls.gButton();
            this.chkHinvert = new SDRSharp.Controls.gButton();
            this.chkXY = new SDRSharp.Controls.gButton();
            this.chkVinvert = new SDRSharp.Controls.gButton();
            this.cmbTim = new SDRSharp.Controls.gCombo();
            this.cmbHor = new SDRSharp.Controls.gCombo();
            this.cmbVer = new SDRSharp.Controls.gCombo();
            this.tbRFgain = new SDRSharp.Controls.gSliderH();
            this.cmbVchannel = new SDRSharp.Controls.gCombo();
            this.gBexpandScope = new SDRSharp.Controls.gButton();
            this.cwShiftNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.outputDeviceComboBox = new SDRSharp.Controls.gCombo();
            this.inputDeviceComboBox = new SDRSharp.Controls.gCombo();
            this.sampleRateComboBox = new SDRSharp.Controls.gCombo();
            this.filterAudioCheckBox = new SDRSharp.Controls.gButton();
            this.fmStereoCheckBox = new SDRSharp.Controls.gButton();
            this.chkNotch0 = new SDRSharp.Controls.gButton();
            this.chkNLimiter = new SDRSharp.Controls.gButton();
            this.tbNLRatio = new SDRSharp.Controls.gSliderH();
            this.tbNLTreshold = new SDRSharp.Controls.gSliderH();
            this.agcThresholdNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.agcDecayNumericUpDown = new SDRSharp.Controls.gSliderH();
            this.agcSlopeNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.agcUseHangCheckBox = new SDRSharp.Controls.gButton();
            this.cmbAudio = new SDRSharp.Controls.gCombo();
            this.chkIF = new SDRSharp.Controls.gButton();
            this.chkWF = new SDRSharp.Controls.gButton();
            this.dbmOffsetUpDown = new SDRSharp.Controls.gUpDown();
            this.chkAutoSize = new SDRSharp.Controls.gButton();
            this.frequencyShiftNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.markPeaksCheckBox = new SDRSharp.Controls.gButton();
            this.frequencyShiftCheckBox = new SDRSharp.Controls.gButton();
            this.fftResolutionComboBox = new SDRSharp.Controls.gCombo();
            this.fftAverageUpDown = new SDRSharp.Controls.gUpDown();
            this.useTimestampsCheckBox = new SDRSharp.Controls.gButton();
            this.chkIndepSideband = new SDRSharp.Controls.gButton();
            this.gradientButtonSA = new SDRSharp.Controls.gButton();
            this.remDcSlider = new SDRSharp.Controls.gSliderH();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.audiogram = new SDRSharp.PanView.Waterfall();
            this.afAnalyzer = new SDRSharp.PanView.SpectrumAnalyzer();
            this.wideScope = new SDRSharp.PanView.Scope();
            this.afWaterfall = new SDRSharp.PanView.Waterfall();
            this.label31 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.labSpeed = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.labInt = new System.Windows.Forms.Label();
            this.labCon = new System.Windows.Forms.Label();
            this.MnuSA = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSetNotch = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuClearNotch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuVfoA = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVfoB = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVfoC = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowWaterfall = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowBaseband = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowAudiogram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowEnvelope = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuStationList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoScale = new System.Windows.Forms.ToolStripMenuItem();
            this.setColoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iqTimer = new System.Windows.Forms.Timer(this.components);
            this.labZoom = new System.Windows.Forms.Label();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.AdvancedCollapsiblePanel = new SDRSharp.CollapsiblePanel.CollapsiblePanel();
            this.barMeter = new SDRSharp.Radio.BarMeter();
            this.labCPU = new SDRSharp.Controls.gLabel();
            this.labProcessTmr = new System.Windows.Forms.Label();
            this.labPerformTmr = new System.Windows.Forms.Label();
            this.labFftTmr = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.centerFreqNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.cmbCenterStep = new SDRSharp.Controls.gCombo();
            this.label5 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.chk1 = new SDRSharp.Controls.gButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.filterBandwidthNumericUpDown = new SDRSharp.Controls.gUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.radioCollapsiblePanel = new SDRSharp.CollapsiblePanel.CollapsiblePanel();
            this.label60 = new System.Windows.Forms.Label();
            this.samRadioButton = new SDRSharp.Controls.gButton();
            this.amRadioButton = new SDRSharp.Controls.gButton();
            this.dsbRadioButton = new SDRSharp.Controls.gButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butVfoC = new SDRSharp.Controls.gButton();
            this.butVfoB = new SDRSharp.Controls.gButton();
            this.butVfoA = new SDRSharp.Controls.gButton();
            this.label38 = new System.Windows.Forms.Label();
            this.lsbRadioButton = new SDRSharp.Controls.gButton();
            this.cwRadioButton = new SDRSharp.Controls.gButton();
            this.nfmRadioButton = new SDRSharp.Controls.gButton();
            this.usbRadioButton = new SDRSharp.Controls.gButton();
            this.wfmRadioButton = new SDRSharp.Controls.gButton();
            this.scopeCollapsiblePanel = new SDRSharp.CollapsiblePanel.CollapsiblePanel();
            this.label51 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.labTbGain = new System.Windows.Forms.Label();
            this.labTbAverage = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.labFast = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.audioCollapsiblePanel = new SDRSharp.CollapsiblePanel.CollapsiblePanel();
            this.label2 = new System.Windows.Forms.Label();
            this.chkNotch3 = new SDRSharp.Controls.gButton();
            this.chkNotch2 = new SDRSharp.Controls.gButton();
            this.chkNotch1 = new SDRSharp.Controls.gButton();
            this.label12 = new System.Windows.Forms.Label();
            this.labSampling = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.agcCollapsiblePanel = new SDRSharp.CollapsiblePanel.CollapsiblePanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label33 = new System.Windows.Forms.Label();
            this.labNLTreshold = new System.Windows.Forms.Label();
            this.labNLRatio = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.agcDecayLabel = new System.Windows.Forms.Label();
            this.displayCollapsiblePanel = new SDRSharp.CollapsiblePanel.CollapsiblePanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labSmeter = new System.Windows.Forms.Label();
            this.wDecayTrackBar = new SDRSharp.Controls.gSliderH();
            this.wAttackTrackBar = new SDRSharp.Controls.gSliderH();
            this.labFftAverage = new System.Windows.Forms.Label();
            this.sDecayTrackBar = new SDRSharp.Controls.gSliderH();
            this.sAttackTrackBar = new SDRSharp.Controls.gSliderH();
            this.label23 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.scrollPanel = new System.Windows.Forms.Panel();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.gThumb = new SDRSharp.Controls.gButton();
            this.labBandWidth = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.labSpectrum = new SDRSharp.Controls.gLabel();
            this.tbIntensityWv = new SDRSharp.Controls.gSliderV();
            this.fftZoomTrackBar = new SDRSharp.Controls.gSliderH();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.chkScrollPanel = new Telerik.WinControls.UI.RadCheckBox();
            this.gButton1 = new SDRSharp.Controls.gButton();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer)).BeginInit();
            this.panSplitContainer.Panel1.SuspendLayout();
            this.panSplitContainer.Panel2.SuspendLayout();
            this.panSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer2)).BeginInit();
            this.panSplitContainer2.Panel1.SuspendLayout();
            this.panSplitContainer2.Panel2.SuspendLayout();
            this.panSplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer3)).BeginInit();
            this.panSplitContainer3.Panel1.SuspendLayout();
            this.panSplitContainer3.Panel2.SuspendLayout();
            this.panSplitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer4)).BeginInit();
            this.panSplitContainer4.Panel1.SuspendLayout();
            this.panSplitContainer4.Panel2.SuspendLayout();
            this.panSplitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer5)).BeginInit();
            this.panSplitContainer5.Panel1.SuspendLayout();
            this.panSplitContainer5.Panel2.SuspendLayout();
            this.panSplitContainer5.SuspendLayout();
            this.panelAG.SuspendLayout();
            this.MnuSA.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.AdvancedCollapsiblePanel.SuspendLayout();
            this.radioCollapsiblePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.scopeCollapsiblePanel.SuspendLayout();
            this.audioCollapsiblePanel.SuspendLayout();
            this.agcCollapsiblePanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.displayCollapsiblePanel.SuspendLayout();
            this.scrollPanel.SuspendLayout();
            this.pnlScroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkScrollPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // openDlg
            // 
            this.openDlg.DefaultExt = "wav";
            this.openDlg.Filter = "WAV files|*.wav";
            // 
            // panSplitContainer
            // 
            this.panSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.panSplitContainer.Location = new System.Drawing.Point(228, 42);
            this.panSplitContainer.Name = "panSplitContainer";
            this.panSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // panSplitContainer.Panel1
            // 
            this.panSplitContainer.Panel1.Controls.Add(this.spectrumAnalyzer);
            this.panSplitContainer.Panel1MinSize = 70;
            // 
            // panSplitContainer.Panel2
            // 
            this.panSplitContainer.Panel2.Controls.Add(this.panSplitContainer2);
            this.panSplitContainer.Panel2.Controls.Add(this.label31);
            this.panSplitContainer.Panel2MinSize = 140;
            this.panSplitContainer.Size = new System.Drawing.Size(1023, 694);
            this.panSplitContainer.SplitterDistance = 189;
            this.panSplitContainer.TabIndex = 13;
            // 
            // spectrumAnalyzer
            // 
            this.spectrumAnalyzer.Attack = 0.9D;
            this.spectrumAnalyzer.BackgroundColor = System.Drawing.Color.Empty;
            this.spectrumAnalyzer.BandType = SDRSharp.PanView.BandType.Center;
            this.spectrumAnalyzer.CenterFixed = true;
            this.spectrumAnalyzer.CenterFrequency = ((long)(0));
            this.spectrumAnalyzer.CenterStep = 10000;
            this.spectrumAnalyzer.DataType = SDRSharp.Radio.DataType.RF;
            this.spectrumAnalyzer.Decay = 0.3D;
            this.spectrumAnalyzer.DisplayFrequency = ((long)(0));
            this.spectrumAnalyzer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spectrumAnalyzer.FilterBandwidth = 10000;
            this.spectrumAnalyzer.FilterOffset = 100;
            this.spectrumAnalyzer.Frequency = ((long)(0));
            this.spectrumAnalyzer.IndepSideband = false;
            this.spectrumAnalyzer.Location = new System.Drawing.Point(0, 0);
            this.spectrumAnalyzer.MarkPeaks = false;
            this.spectrumAnalyzer.MaxPower = 0F;
            this.spectrumAnalyzer.MinPower = -130F;
            this.spectrumAnalyzer.Name = "spectrumAnalyzer";
            this.spectrumAnalyzer.ShowDbm = 0;
            this.spectrumAnalyzer.Size = new System.Drawing.Size(1023, 189);
            this.spectrumAnalyzer.SpectrumColor = System.Drawing.Color.DarkGray;
            this.spectrumAnalyzer.SpectrumFill = 0;
            this.spectrumAnalyzer.SpectrumWidth = 2048001;
            this.spectrumAnalyzer.StationList = "";
            this.spectrumAnalyzer.StatusText = "";
            this.spectrumAnalyzer.StepSize = 1000;
            this.spectrumAnalyzer.TabIndex = 98;
            this.spectrumAnalyzer.UseSmoothing = true;
            this.spectrumAnalyzer.UseSnap = false;
            this.spectrumAnalyzer.Zoom = 1F;
            this.spectrumAnalyzer.FrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_FrequencyChanged);
            this.spectrumAnalyzer.DisplayFrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_DisplayFrequencyChanged);
            this.spectrumAnalyzer.CenterFrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_CenterFrequencyChanged);
            this.spectrumAnalyzer.BandwidthChanged += new SDRSharp.PanView.ManualBandwidthChange(this.panview_BandwidthChanged);
            this.spectrumAnalyzer.NotchChanged += new SDRSharp.PanView.ManualNotchChange(this.panview_NotchChanged);
            this.spectrumAnalyzer.StationDatachanged += new System.EventHandler(this.spectrumAnalyzer_StationDatachanged);
            this.spectrumAnalyzer.AutoZoomed += new System.EventHandler(this.panview_AutoZoomed);
            // 
            // panSplitContainer2
            // 
            this.panSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.panSplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.panSplitContainer2.Name = "panSplitContainer2";
            this.panSplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // panSplitContainer2.Panel1
            // 
            this.panSplitContainer2.Panel1.Controls.Add(this.waterfall);
            this.panSplitContainer2.Panel1MinSize = 70;
            // 
            // panSplitContainer2.Panel2
            // 
            this.panSplitContainer2.Panel2.Controls.Add(this.panSplitContainer3);
            this.panSplitContainer2.Panel2MinSize = 70;
            this.panSplitContainer2.Size = new System.Drawing.Size(1023, 501);
            this.panSplitContainer2.SplitterDistance = 228;
            this.panSplitContainer2.TabIndex = 107;
            // 
            // waterfall
            // 
            this.waterfall.Attack = 0.9D;
            this.waterfall.AutoSize = true;
            this.waterfall.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.waterfall.BandType = SDRSharp.PanView.BandType.Center;
            this.waterfall.CenterFixed = false;
            this.waterfall.CenterFrequency = ((long)(0));
            this.waterfall.CenterStep = 0;
            this.waterfall.DataType = SDRSharp.Radio.DataType.RF;
            this.waterfall.Decay = 0.5D;
            this.waterfall.DisplayFrequency = ((long)(0));
            this.waterfall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.waterfall.FilterBandwidth = 10000;
            this.waterfall.FilterOffset = 0;
            this.waterfall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waterfall.Frequency = ((long)(0));
            this.waterfall.Horizontal = false;
            this.waterfall.IndepSideband = false;
            this.waterfall.Location = new System.Drawing.Point(0, 0);
            this.waterfall.MaxPower = 0F;
            this.waterfall.MinPower = -130F;
            this.waterfall.Name = "waterfall";
            this.waterfall.RecordStart = new System.DateTime(((long)(0)));
            this.waterfall.ShowDbm = 0;
            this.waterfall.Size = new System.Drawing.Size(1023, 228);
            this.waterfall.SpectrumWidth = 2048001;
            this.waterfall.StepSize = 0;
            this.waterfall.TabIndex = 99;
            this.waterfall.TimestampInterval = 150;
            this.waterfall.UseSmoothing = true;
            this.waterfall.UseSnap = false;
            this.waterfall.UseTimestamps = 0;
            this.waterfall.WaveStart = new System.DateTime(((long)(0)));
            this.waterfall.Zoom = 1F;
            this.waterfall.FrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_FrequencyChanged);
            this.waterfall.CenterFrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_CenterFrequencyChanged);
            this.waterfall.DisplayFrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_DisplayFrequencyChanged);
            this.waterfall.BandwidthChanged += new SDRSharp.PanView.ManualBandwidthChange(this.panview_BandwidthChanged);
            this.waterfall.AutoZoomed += new System.EventHandler(this.panview_AutoZoomed);
            this.waterfall.MouseDown += new System.Windows.Forms.MouseEventHandler(this.waterfall_MouseDown);
            this.waterfall.Resize += new System.EventHandler(this.waterfall_Resize);
            // 
            // panSplitContainer3
            // 
            this.panSplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSplitContainer3.Location = new System.Drawing.Point(0, 0);
            this.panSplitContainer3.Name = "panSplitContainer3";
            // 
            // panSplitContainer3.Panel1
            // 
            this.panSplitContainer3.Panel1.Controls.Add(this.panSplitContainer4);
            this.panSplitContainer3.Panel1MinSize = 70;
            // 
            // panSplitContainer3.Panel2
            // 
            this.panSplitContainer3.Panel2.Controls.Add(this.panSplitContainer5);
            this.panSplitContainer3.Panel2.Resize += new System.EventHandler(this.panSplitContainer3_Panel2_Resize);
            this.panSplitContainer3.Panel2MinSize = 0;
            this.panSplitContainer3.Size = new System.Drawing.Size(1023, 269);
            this.panSplitContainer3.SplitterDistance = 542;
            this.panSplitContainer3.TabIndex = 0;
            // 
            // panSplitContainer4
            // 
            this.panSplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSplitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.panSplitContainer4.Location = new System.Drawing.Point(0, 0);
            this.panSplitContainer4.Name = "panSplitContainer4";
            this.panSplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // panSplitContainer4.Panel1
            // 
            this.panSplitContainer4.Panel1.Controls.Add(this.ifAnalyzer);
            // 
            // panSplitContainer4.Panel2
            // 
            this.panSplitContainer4.Panel2.Controls.Add(this.ifWaterfall);
            this.panSplitContainer4.Size = new System.Drawing.Size(542, 269);
            this.panSplitContainer4.SplitterDistance = 143;
            this.panSplitContainer4.TabIndex = 100;
            this.panSplitContainer4.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.panSplitContainer4_SplitterMoved);
            // 
            // ifAnalyzer
            // 
            this.ifAnalyzer.Attack = 0.9D;
            this.ifAnalyzer.BackgroundColor = System.Drawing.Color.Empty;
            this.ifAnalyzer.BandType = SDRSharp.PanView.BandType.Center;
            this.ifAnalyzer.CenterFixed = true;
            this.ifAnalyzer.CenterFrequency = ((long)(0));
            this.ifAnalyzer.CenterStep = 10000;
            this.ifAnalyzer.DataType = SDRSharp.Radio.DataType.IF;
            this.ifAnalyzer.Decay = 0.3D;
            this.ifAnalyzer.DisplayFrequency = ((long)(0));
            this.ifAnalyzer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifAnalyzer.FilterBandwidth = 10000;
            this.ifAnalyzer.FilterOffset = 0;
            this.ifAnalyzer.Frequency = ((long)(0));
            this.ifAnalyzer.IndepSideband = false;
            this.ifAnalyzer.Location = new System.Drawing.Point(0, 0);
            this.ifAnalyzer.MarkPeaks = false;
            this.ifAnalyzer.MaxPower = 0F;
            this.ifAnalyzer.MinPower = -130F;
            this.ifAnalyzer.Name = "ifAnalyzer";
            this.ifAnalyzer.ShowDbm = 0;
            this.ifAnalyzer.Size = new System.Drawing.Size(542, 143);
            this.ifAnalyzer.SpectrumColor = System.Drawing.Color.DarkGray;
            this.ifAnalyzer.SpectrumFill = 0;
            this.ifAnalyzer.SpectrumWidth = 96001;
            this.ifAnalyzer.StationList = "";
            this.ifAnalyzer.StatusText = "";
            this.ifAnalyzer.StepSize = 1000;
            this.ifAnalyzer.TabIndex = 99;
            this.ifAnalyzer.UseSmoothing = true;
            this.ifAnalyzer.UseSnap = false;
            this.ifAnalyzer.Zoom = 6.4F;
            this.ifAnalyzer.FrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_FrequencyChanged);
            this.ifAnalyzer.CenterFrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_CenterFrequencyChanged);
            this.ifAnalyzer.BandwidthChanged += new SDRSharp.PanView.ManualBandwidthChange(this.panview_BandwidthChanged);
            this.ifAnalyzer.NotchChanged += new SDRSharp.PanView.ManualNotchChange(this.panview_NotchChanged);
            this.ifAnalyzer.AutoZoomed += new System.EventHandler(this.panview_AutoZoomed);
            // 
            // ifWaterfall
            // 
            this.ifWaterfall.Attack = 0.9D;
            this.ifWaterfall.AutoSize = true;
            this.ifWaterfall.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.ifWaterfall.BandType = SDRSharp.PanView.BandType.Center;
            this.ifWaterfall.CenterFixed = false;
            this.ifWaterfall.CenterFrequency = ((long)(0));
            this.ifWaterfall.CenterStep = 0;
            this.ifWaterfall.DataType = SDRSharp.Radio.DataType.IF;
            this.ifWaterfall.Decay = 0.5D;
            this.ifWaterfall.DisplayFrequency = ((long)(0));
            this.ifWaterfall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifWaterfall.FilterBandwidth = 10000;
            this.ifWaterfall.FilterOffset = 0;
            this.ifWaterfall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifWaterfall.Frequency = ((long)(0));
            this.ifWaterfall.Horizontal = false;
            this.ifWaterfall.IndepSideband = false;
            this.ifWaterfall.Location = new System.Drawing.Point(0, 0);
            this.ifWaterfall.MaxPower = 0F;
            this.ifWaterfall.MinPower = -130F;
            this.ifWaterfall.Name = "ifWaterfall";
            this.ifWaterfall.RecordStart = new System.DateTime(((long)(0)));
            this.ifWaterfall.ShowDbm = 0;
            this.ifWaterfall.Size = new System.Drawing.Size(542, 122);
            this.ifWaterfall.SpectrumWidth = 96001;
            this.ifWaterfall.StepSize = 0;
            this.ifWaterfall.TabIndex = 100;
            this.ifWaterfall.TimestampInterval = 150;
            this.ifWaterfall.UseSmoothing = true;
            this.ifWaterfall.UseSnap = false;
            this.ifWaterfall.UseTimestamps = 0;
            this.ifWaterfall.WaveStart = new System.DateTime(((long)(0)));
            this.ifWaterfall.Zoom = 1F;
            this.ifWaterfall.FrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_FrequencyChanged);
            this.ifWaterfall.BandwidthChanged += new SDRSharp.PanView.ManualBandwidthChange(this.panview_BandwidthChanged);
            this.ifWaterfall.AutoZoomed += new System.EventHandler(this.panview_AutoZoomed);
            // 
            // panSplitContainer5
            // 
            this.panSplitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSplitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.panSplitContainer5.Location = new System.Drawing.Point(0, 0);
            this.panSplitContainer5.Name = "panSplitContainer5";
            this.panSplitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // panSplitContainer5.Panel1
            // 
            this.panSplitContainer5.Panel1.Controls.Add(this.panelAG);
            this.panSplitContainer5.Panel1.Controls.Add(this.audiogram);
            this.panSplitContainer5.Panel1.Controls.Add(this.afAnalyzer);
            this.panSplitContainer5.Panel1.Resize += new System.EventHandler(this.panSplitContainer5_Panel1_Resize);
            // 
            // panSplitContainer5.Panel2
            // 
            this.panSplitContainer5.Panel2.Controls.Add(this.wideScope);
            this.panSplitContainer5.Panel2.Controls.Add(this.afWaterfall);
            this.panSplitContainer5.Panel2.Resize += new System.EventHandler(this.panSplitContainer5_Panel2_Resize);
            this.panSplitContainer5.Size = new System.Drawing.Size(477, 269);
            this.panSplitContainer5.SplitterDistance = 150;
            this.panSplitContainer5.TabIndex = 0;
            // 
            // panelAG
            // 
            this.panelAG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAG.Controls.Add(this.btnShowLog);
            this.panelAG.Controls.Add(this.tbIntensityAg);
            this.panelAG.Controls.Add(this.tbContrastAg);
            this.panelAG.Controls.Add(this.tbAgSpeed);
            this.panelAG.Controls.Add(this.label50);
            this.panelAG.Controls.Add(this.label49);
            this.panelAG.Controls.Add(this.label44);
            this.panelAG.Location = new System.Drawing.Point(402, 0);
            this.panelAG.Name = "panelAG";
            this.panelAG.Size = new System.Drawing.Size(70, 120);
            this.panelAG.TabIndex = 125;
            // 
            // btnShowLog
            // 
            this.btnShowLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowLog.Arrow = 99;
            this.btnShowLog.Checked = false;
            this.btnShowLog.Edge = 0.15F;
            this.btnShowLog.EndColor = System.Drawing.Color.White;
            this.btnShowLog.EndFactor = 0.2F;
            this.btnShowLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowLog.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnShowLog.Location = new System.Drawing.Point(28, 92);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.NoBorder = false;
            this.btnShowLog.NoLed = false;
            this.btnShowLog.RadioButton = false;
            this.btnShowLog.Radius = 4;
            this.btnShowLog.RadiusB = 0;
            this.btnShowLog.Size = new System.Drawing.Size(36, 20);
            this.btnShowLog.StartColor = System.Drawing.Color.Black;
            this.btnShowLog.StartFactor = 0.35F;
            this.btnShowLog.TabIndex = 126;
            this.btnShowLog.Tag = "";
            this.btnShowLog.Text = "Log";
            this.toolTip.SetToolTip(this.btnShowLog, "Log scale on/off");
            this.btnShowLog.CheckedChanged += new System.EventHandler(this.btnShowLog_CheckedChanged);
            // 
            // tbIntensityAg
            // 
            this.tbIntensityAg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIntensityAg.Button = false;
            this.tbIntensityAg.Checked = false;
            this.tbIntensityAg.ColorFactor = 0.5F;
            this.tbIntensityAg.Location = new System.Drawing.Point(26, 19);
            this.tbIntensityAg.Margin = new System.Windows.Forms.Padding(4);
            this.tbIntensityAg.Maximum = -40;
            this.tbIntensityAg.Minimum = -140;
            this.tbIntensityAg.Name = "tbIntensityAg";
            this.tbIntensityAg.Size = new System.Drawing.Size(16, 68);
            this.tbIntensityAg.TabIndex = 109;
            this.tbIntensityAg.Tag = "";
            this.tbIntensityAg.TickColor = System.Drawing.Color.Silver;
            this.tbIntensityAg.Ticks = 0;
            this.tbIntensityAg.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbIntensityAg, "Intensity");
            this.tbIntensityAg.Value = -40;
            this.tbIntensityAg.ValueChanged += new System.EventHandler(this.tbIntensityAG_ValueChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 100;
            this.toolTip.AutoPopDelay = 3000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 20;
            this.toolTip.ShowAlways = true;
            this.toolTip.UseAnimation = false;
            this.toolTip.UseFading = false;
            // 
            // frequencyNumericUpDown
            // 
            this.frequencyNumericUpDown.AutoSize = true;
            this.frequencyNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequencyNumericUpDown.ForeColor = System.Drawing.SystemColors.ControlText;
            this.frequencyNumericUpDown.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.frequencyNumericUpDown.Label = "Freq";
            this.frequencyNumericUpDown.Location = new System.Drawing.Point(337, 5);
            this.frequencyNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.frequencyNumericUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.frequencyNumericUpDown.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.frequencyNumericUpDown.Name = "frequencyNumericUpDown";
            this.frequencyNumericUpDown.Size = new System.Drawing.Size(293, 38);
            this.frequencyNumericUpDown.TabIndex = 93;
            this.toolTip.SetToolTip(this.frequencyNumericUpDown, "Click/scroll digit to set VFO freq.");
            this.frequencyNumericUpDown.ToolTip = this.toolTip;
            this.frequencyNumericUpDown.Value = new decimal(new int[] {
            298954296,
            2,
            0,
            0});
            this.frequencyNumericUpDown.ValueChanged += new SDRSharp.PanView.ManualValueChange(this.frequencyNumericUpDown_ValueChanged);
            // 
            // audioButton
            // 
            this.audioButton.Arrow = 0;
            this.audioButton.Checked = true;
            this.audioButton.Edge = 0.15F;
            this.audioButton.EndColor = System.Drawing.Color.White;
            this.audioButton.EndFactor = 0.2F;
            this.audioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.audioButton.Location = new System.Drawing.Point(7, 39);
            this.audioButton.Name = "audioButton";
            this.audioButton.NoBorder = false;
            this.audioButton.NoLed = true;
            this.audioButton.RadioButton = false;
            this.audioButton.Radius = 6;
            this.audioButton.RadiusB = 0;
            this.audioButton.Size = new System.Drawing.Size(49, 26);
            this.audioButton.StartColor = System.Drawing.Color.Black;
            this.audioButton.StartFactor = 0.35F;
            this.audioButton.TabIndex = 109;
            this.audioButton.Text = "Mute";
            this.toolTip.SetToolTip(this.audioButton, "Mute audio [Spacebar]");
            this.audioButton.CheckedChanged += new System.EventHandler(this.audioButton_CheckedChanged);
            // 
            // tbContrastAg
            // 
            this.tbContrastAg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbContrastAg.Button = false;
            this.tbContrastAg.Checked = false;
            this.tbContrastAg.ColorFactor = 0.5F;
            this.tbContrastAg.Location = new System.Drawing.Point(48, 19);
            this.tbContrastAg.Margin = new System.Windows.Forms.Padding(4);
            this.tbContrastAg.Maximum = 80;
            this.tbContrastAg.Minimum = 10;
            this.tbContrastAg.Name = "tbContrastAg";
            this.tbContrastAg.Size = new System.Drawing.Size(16, 68);
            this.tbContrastAg.TabIndex = 110;
            this.tbContrastAg.Tag = "";
            this.tbContrastAg.TickColor = System.Drawing.Color.Silver;
            this.tbContrastAg.Ticks = 0;
            this.tbContrastAg.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbContrastAg, "Contrast");
            this.tbContrastAg.Value = 50;
            this.tbContrastAg.ValueChanged += new System.EventHandler(this.tbContrasAG_ValueChanged);
            // 
            // tbAgSpeed
            // 
            this.tbAgSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAgSpeed.Button = false;
            this.tbAgSpeed.Checked = false;
            this.tbAgSpeed.ColorFactor = 0.5F;
            this.tbAgSpeed.ForeColor = System.Drawing.Color.Black;
            this.tbAgSpeed.Location = new System.Drawing.Point(5, 19);
            this.tbAgSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.tbAgSpeed.Maximum = 8;
            this.tbAgSpeed.Minimum = 0;
            this.tbAgSpeed.Name = "tbAgSpeed";
            this.tbAgSpeed.Size = new System.Drawing.Size(16, 68);
            this.tbAgSpeed.TabIndex = 111;
            this.tbAgSpeed.Tag = "";
            this.tbAgSpeed.TickColor = System.Drawing.Color.Silver;
            this.tbAgSpeed.Ticks = 5;
            this.tbAgSpeed.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbAgSpeed, "Audiogram speed");
            this.tbAgSpeed.Value = 0;
            this.tbAgSpeed.ValueChanged += new System.EventHandler(this.tbAgSpeed_ValueChanged);
            // 
            // playBar
            // 
            this.playBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playBar.Location = new System.Drawing.Point(229, 735);
            this.playBar.Margin = new System.Windows.Forms.Padding(4);
            this.playBar.Name = "playBar";
            this.playBar.Size = new System.Drawing.Size(1020, 22);
            this.playBar.TabIndex = 107;
            this.playBar.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.playBar, "Playback progress, click to position");
            this.playBar.ValueChanged += new System.EventHandler(this.playBar_ValueChanged);
            // 
            // fftSpeedTrackBar
            // 
            this.fftSpeedTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fftSpeedTrackBar.Button = false;
            this.fftSpeedTrackBar.Checked = false;
            this.fftSpeedTrackBar.ColorFactor = 0.5F;
            this.fftSpeedTrackBar.ForeColor = System.Drawing.Color.Black;
            this.fftSpeedTrackBar.Location = new System.Drawing.Point(1259, 387);
            this.fftSpeedTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.fftSpeedTrackBar.Maximum = 11;
            this.fftSpeedTrackBar.Minimum = 0;
            this.fftSpeedTrackBar.Name = "fftSpeedTrackBar";
            this.fftSpeedTrackBar.Size = new System.Drawing.Size(15, 68);
            this.fftSpeedTrackBar.TabIndex = 105;
            this.fftSpeedTrackBar.Tag = "";
            this.fftSpeedTrackBar.TickColor = System.Drawing.Color.Silver;
            this.fftSpeedTrackBar.Ticks = 5;
            this.fftSpeedTrackBar.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.fftSpeedTrackBar, "Waterfall speed");
            this.fftSpeedTrackBar.Value = 0;
            this.fftSpeedTrackBar.ValueChanged += new System.EventHandler(this.fftSpeedTrackBar_ValueChanged);
            // 
            // tbFloorSA
            // 
            this.tbFloorSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFloorSA.Button = false;
            this.tbFloorSA.Checked = false;
            this.tbFloorSA.ColorFactor = 0.5F;
            this.tbFloorSA.Location = new System.Drawing.Point(1259, 148);
            this.tbFloorSA.Margin = new System.Windows.Forms.Padding(4);
            this.tbFloorSA.Maximum = -60;
            this.tbFloorSA.Minimum = -160;
            this.tbFloorSA.Name = "tbFloorSA";
            this.tbFloorSA.Size = new System.Drawing.Size(15, 68);
            this.tbFloorSA.TabIndex = 101;
            this.tbFloorSA.Tag = "";
            this.tbFloorSA.TickColor = System.Drawing.Color.Orange;
            this.tbFloorSA.Ticks = 0;
            this.tbFloorSA.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbFloorSA, "Spectrum floor");
            this.tbFloorSA.Value = -100;
            this.tbFloorSA.ValueChanged += new System.EventHandler(this.tbFloorSA_Changed);
            // 
            // tbSpanSA
            // 
            this.tbSpanSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSpanSA.Button = false;
            this.tbSpanSA.Checked = false;
            this.tbSpanSA.ColorFactor = 0.5F;
            this.tbSpanSA.Location = new System.Drawing.Point(1259, 59);
            this.tbSpanSA.Margin = new System.Windows.Forms.Padding(4);
            this.tbSpanSA.Maximum = 140;
            this.tbSpanSA.Minimum = 30;
            this.tbSpanSA.Name = "tbSpanSA";
            this.tbSpanSA.Size = new System.Drawing.Size(15, 68);
            this.tbSpanSA.TabIndex = 100;
            this.tbSpanSA.Tag = "";
            this.tbSpanSA.TickColor = System.Drawing.Color.Orange;
            this.tbSpanSA.Ticks = 0;
            this.tbSpanSA.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbSpanSA, "Spectrum span");
            this.tbSpanSA.Value = 50;
            this.tbSpanSA.ValueChanged += new System.EventHandler(this.tbSpanSA_Changed);
            // 
            // tbContrastWv
            // 
            this.tbContrastWv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbContrastWv.Button = false;
            this.tbContrastWv.Checked = false;
            this.tbContrastWv.ColorFactor = 0.5F;
            this.tbContrastWv.Location = new System.Drawing.Point(1259, 264);
            this.tbContrastWv.Margin = new System.Windows.Forms.Padding(4);
            this.tbContrastWv.Maximum = 150;
            this.tbContrastWv.Minimum = 40;
            this.tbContrastWv.Name = "tbContrastWv";
            this.tbContrastWv.Size = new System.Drawing.Size(15, 68);
            this.tbContrastWv.TabIndex = 103;
            this.tbContrastWv.Tag = "";
            this.tbContrastWv.TickColor = System.Drawing.Color.Orange;
            this.tbContrastWv.Ticks = 0;
            this.tbContrastWv.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbContrastWv, "Waterfall contrast");
            this.tbContrastWv.Value = 50;
            this.tbContrastWv.ValueChanged += new System.EventHandler(this.tbContrastWv_Changed);
            // 
            // gBsetScale
            // 
            this.gBsetScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gBsetScale.Arrow = 0;
            this.gBsetScale.Checked = false;
            this.gBsetScale.Edge = 0.15F;
            this.gBsetScale.EndColor = System.Drawing.Color.White;
            this.gBsetScale.EndFactor = 0.2F;
            this.gBsetScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gBsetScale.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.gBsetScale.Location = new System.Drawing.Point(1180, 11);
            this.gBsetScale.Name = "gBsetScale";
            this.gBsetScale.NoBorder = false;
            this.gBsetScale.NoLed = true;
            this.gBsetScale.RadioButton = false;
            this.gBsetScale.Radius = 6;
            this.gBsetScale.RadiusB = 0;
            this.gBsetScale.Size = new System.Drawing.Size(33, 20);
            this.gBsetScale.StartColor = System.Drawing.Color.Black;
            this.gBsetScale.StartFactor = 0.35F;
            this.gBsetScale.TabIndex = 102;
            this.gBsetScale.Tag = "";
            this.gBsetScale.Text = "Set ";
            this.toolTip.SetToolTip(this.gBsetScale, "(Re)set spectrum floor");
            this.gBsetScale.CheckedChanged += new System.EventHandler(this.gBsetScale_CheckedChanged);
            // 
            // fftZoomCombo
            // 
            this.fftZoomCombo.BackColor = System.Drawing.Color.Transparent;
            this.fftZoomCombo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.fftZoomCombo.Items.Add("Full");
            this.fftZoomCombo.Items.Add("2.0 Mhz");
            this.fftZoomCombo.Items.Add("1.6 Mhz");
            this.fftZoomCombo.Items.Add("1.0 Mhz");
            this.fftZoomCombo.Items.Add("800 kHz");
            this.fftZoomCombo.Items.Add("500 kHz");
            this.fftZoomCombo.Items.Add("400 kHz");
            this.fftZoomCombo.Items.Add("250 kHz");
            this.fftZoomCombo.Items.Add("192 kHz");
            this.fftZoomCombo.Items.Add("96 kHz");
            this.fftZoomCombo.Items.Add("48 kHz");
            this.fftZoomCombo.Items.Add("25 kHz");
            this.fftZoomCombo.Items.Add("10 kHz");
            this.fftZoomCombo.Items.Add("5 kHz");
            this.fftZoomCombo.Items.Add("2 kHz");
            this.fftZoomCombo.Items.Add("1 kHz");
            this.fftZoomCombo.Location = new System.Drawing.Point(983, 11);
            this.fftZoomCombo.Margin = new System.Windows.Forms.Padding(5);
            this.fftZoomCombo.Name = "fftZoomCombo";
            this.fftZoomCombo.SelectedIndex = -1;
            this.fftZoomCombo.Size = new System.Drawing.Size(80, 21);
            this.fftZoomCombo.TabIndex = 100;
            this.fftZoomCombo.Text = "xxxx";
            this.toolTip.SetToolTip(this.fftZoomCombo, "RF zoom bandwidth");
            this.fftZoomCombo.ToolTip = this.toolTip;
            this.fftZoomCombo.SelectedIndexChanged += new System.EventHandler(this.fftZoomCombo_SelectedIndexChanged);
            // 
            // chkLock
            // 
            this.chkLock.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkLock.Arrow = 0;
            this.chkLock.BackColor = System.Drawing.Color.White;
            this.chkLock.Checked = false;
            this.chkLock.Edge = 0.15F;
            this.chkLock.EndColor = System.Drawing.Color.White;
            this.chkLock.EndFactor = 0.2F;
            this.chkLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLock.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkLock.Location = new System.Drawing.Point(638, 7);
            this.chkLock.Name = "chkLock";
            this.chkLock.NoBorder = false;
            this.chkLock.NoLed = false;
            this.chkLock.RadioButton = false;
            this.chkLock.Radius = 6;
            this.chkLock.RadiusB = 0;
            this.chkLock.Size = new System.Drawing.Size(40, 29);
            this.chkLock.StartColor = System.Drawing.Color.Black;
            this.chkLock.StartFactor = 0.35F;
            this.chkLock.TabIndex = 94;
            this.chkLock.Text = "Lock";
            this.toolTip.SetToolTip(this.chkLock, "Enable carrier locking");
            this.chkLock.CheckedChanged += new System.EventHandler(this.chk1_CheckedChanged);
            // 
            // cmbBandWidth
            // 
            this.cmbBandWidth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbBandWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbBandWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBandWidth.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbBandWidth.Items.Add("manual");
            this.cmbBandWidth.Items.Add("100 Hz");
            this.cmbBandWidth.Items.Add("300 Hz");
            this.cmbBandWidth.Items.Add("500 Hz");
            this.cmbBandWidth.Items.Add("1.0 kHz");
            this.cmbBandWidth.Items.Add("2.4 kHz");
            this.cmbBandWidth.Items.Add("3.0 kHz");
            this.cmbBandWidth.Items.Add("4.5 kHz");
            this.cmbBandWidth.Items.Add("5 kHz");
            this.cmbBandWidth.Items.Add("6 kHz");
            this.cmbBandWidth.Items.Add("8 kHz");
            this.cmbBandWidth.Items.Add("10 kHz");
            this.cmbBandWidth.Items.Add("20 kHz");
            this.cmbBandWidth.Items.Add("25 kHz");
            this.cmbBandWidth.Items.Add("50 kHz");
            this.cmbBandWidth.Items.Add("100 kHz");
            this.cmbBandWidth.Items.Add("180 kHz");
            this.cmbBandWidth.Items.Add("250 kHz");
            this.cmbBandWidth.Location = new System.Drawing.Point(816, 9);
            this.cmbBandWidth.Margin = new System.Windows.Forms.Padding(5);
            this.cmbBandWidth.Name = "cmbBandWidth";
            this.cmbBandWidth.SelectedIndex = -1;
            this.cmbBandWidth.Size = new System.Drawing.Size(90, 24);
            this.cmbBandWidth.TabIndex = 95;
            this.cmbBandWidth.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbBandWidth, "AF bandwidth");
            this.cmbBandWidth.ToolTip = this.toolTip;
            this.cmbBandWidth.SelectedIndexChanged += new System.EventHandler(this.cmbBandWidth_SelectedIndexChanged);
            // 
            // iqSourceComboBox
            // 
            this.iqSourceComboBox.BackColor = System.Drawing.Color.Transparent;
            this.iqSourceComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iqSourceComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.iqSourceComboBox.Location = new System.Drawing.Point(76, 9);
            this.iqSourceComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.iqSourceComboBox.Name = "iqSourceComboBox";
            this.iqSourceComboBox.SelectedIndex = -1;
            this.iqSourceComboBox.Size = new System.Drawing.Size(146, 24);
            this.iqSourceComboBox.TabIndex = 0;
            this.iqSourceComboBox.Text = "xxxx";
            this.toolTip.SetToolTip(this.iqSourceComboBox, "Select RF input device");
            this.iqSourceComboBox.ToolTip = this.toolTip;
            this.iqSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.iqSourceComboBox_SelectedIndexChanged);
            // 
            // playStopButton
            // 
            this.playStopButton.Arrow = 99;
            this.playStopButton.Checked = false;
            this.playStopButton.Edge = 0.2F;
            this.playStopButton.EndColor = System.Drawing.Color.Silver;
            this.playStopButton.EndFactor = 0.3F;
            this.playStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playStopButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.playStopButton.Location = new System.Drawing.Point(281, 7);
            this.playStopButton.Name = "playStopButton";
            this.playStopButton.NoBorder = false;
            this.playStopButton.NoLed = false;
            this.playStopButton.RadioButton = false;
            this.playStopButton.Radius = 6;
            this.playStopButton.RadiusB = 0;
            this.playStopButton.Size = new System.Drawing.Size(45, 29);
            this.playStopButton.StartColor = System.Drawing.Color.Black;
            this.playStopButton.StartFactor = 0.35F;
            this.playStopButton.TabIndex = 2;
            this.playStopButton.Text = "Play";
            this.toolTip.SetToolTip(this.playStopButton, "Start/stop radio");
            this.playStopButton.CheckedChanged += new System.EventHandler(this.playStopButton_Click);
            this.playStopButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.playStopButton_PreviewKeyDown);
            // 
            // configureSourceButton
            // 
            this.configureSourceButton.Arrow = 0;
            this.configureSourceButton.Checked = false;
            this.configureSourceButton.Edge = 0.15F;
            this.configureSourceButton.EndColor = System.Drawing.Color.White;
            this.configureSourceButton.EndFactor = 0.2F;
            this.configureSourceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configureSourceButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.configureSourceButton.Location = new System.Drawing.Point(228, 9);
            this.configureSourceButton.Name = "configureSourceButton";
            this.configureSourceButton.NoBorder = false;
            this.configureSourceButton.NoLed = true;
            this.configureSourceButton.RadioButton = false;
            this.configureSourceButton.Radius = 6;
            this.configureSourceButton.RadiusB = 0;
            this.configureSourceButton.Size = new System.Drawing.Size(45, 24);
            this.configureSourceButton.StartColor = System.Drawing.Color.Black;
            this.configureSourceButton.StartFactor = 0.35F;
            this.configureSourceButton.TabIndex = 1;
            this.configureSourceButton.Text = "Config";
            this.toolTip.SetToolTip(this.configureSourceButton, "Configure RF input or select wave-file.");
            this.configureSourceButton.Click += new System.EventHandler(this.frontendGuiButton_Click);
            // 
            // audioGainTrackBar
            // 
            this.audioGainTrackBar.Button = false;
            this.audioGainTrackBar.Checked = false;
            this.audioGainTrackBar.ColorFactor = 0.5F;
            this.audioGainTrackBar.ForeColor = System.Drawing.Color.Maroon;
            this.audioGainTrackBar.Location = new System.Drawing.Point(62, 42);
            this.audioGainTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.audioGainTrackBar.Maximum = 57;
            this.audioGainTrackBar.Minimum = 40;
            this.audioGainTrackBar.Name = "audioGainTrackBar";
            this.audioGainTrackBar.Size = new System.Drawing.Size(144, 20);
            this.audioGainTrackBar.TabIndex = 1;
            this.audioGainTrackBar.TickColor = System.Drawing.Color.Gainsboro;
            this.audioGainTrackBar.Ticks = 10;
            this.audioGainTrackBar.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.audioGainTrackBar, "AF audio gain");
            this.audioGainTrackBar.Value = 40;
            this.audioGainTrackBar.ValueChanged += new System.EventHandler(this.audioGainTrackBar_ValueChanged);
            // 
            // scope
            // 
            this.scope.AudioAvg = 0;
            this.scope.AudioRel = 6;
            this.scope.BackgoundColor = System.Drawing.Color.Empty;
            this.scope.CarrierAvg = 0;
            this.scope.HblockDC = false;
            this.scope.Hchannel = SDRSharp.Radio.DemodType.Empty;
            this.scope.Hdiv = 1F;
            this.scope.Hinvert = false;
            this.scope.Hshift = 0F;
            this.scope.Location = new System.Drawing.Point(1, 52);
            this.scope.Name = "scope";
            this.scope.PeakDelay = 0;
            this.scope.PeakRel = 0;
            this.scope.ShowBars = true;
            this.scope.ShowLines = false;
            this.scope.Size = new System.Drawing.Size(197, 237);
            this.scope.SpectrumFill = 0;
            this.scope.StatusText = null;
            this.scope.TabIndex = 28;
            this.scope.Tdiv = 0F;
            this.toolTip.SetToolTip(this.scope, "AF/audio scope");
            this.scope.TraceColor = System.Drawing.Color.DarkGray;
            this.scope.TrigLevel = 0F;
            this.scope.VblockDC = false;
            this.scope.Vchannel = SDRSharp.Radio.DemodType.Empty;
            this.scope.Vdiv = 1F;
            this.scope.Vinvert = false;
            this.scope.Vshift = 0F;
            this.scope.XYmode = false;
            this.scope.XYPositionChanged += new SDRSharp.PanView.Scope.delPositionChanged(this.scope_XYPositionChanged);
            // 
            // fastFftCheckBox
            // 
            this.fastFftCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fastFftCheckBox.Arrow = 99;
            this.fastFftCheckBox.Checked = false;
            this.fastFftCheckBox.Edge = 0.15F;
            this.fastFftCheckBox.EndColor = System.Drawing.Color.White;
            this.fastFftCheckBox.EndFactor = 0.2F;
            this.fastFftCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fastFftCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.fastFftCheckBox.Location = new System.Drawing.Point(152, 76);
            this.fastFftCheckBox.Name = "fastFftCheckBox";
            this.fastFftCheckBox.NoBorder = false;
            this.fastFftCheckBox.NoLed = false;
            this.fastFftCheckBox.RadioButton = false;
            this.fastFftCheckBox.Radius = 6;
            this.fastFftCheckBox.RadiusB = 0;
            this.fastFftCheckBox.Size = new System.Drawing.Size(46, 25);
            this.fastFftCheckBox.StartColor = System.Drawing.Color.Black;
            this.fastFftCheckBox.StartFactor = 0.35F;
            this.fastFftCheckBox.TabIndex = 136;
            this.fastFftCheckBox.Text = "Fast";
            this.toolTip.SetToolTip(this.fastFftCheckBox, "use Fast scrolling FFT display");
            this.fastFftCheckBox.CheckedChanged += new System.EventHandler(this.fastFftCheckBox_CheckedChanged);
            // 
            // bftResolutionComboBox
            // 
            this.bftResolutionComboBox.BackColor = System.Drawing.Color.Transparent;
            this.bftResolutionComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.bftResolutionComboBox.Items.Add("512");
            this.bftResolutionComboBox.Items.Add("1024");
            this.bftResolutionComboBox.Items.Add("2048");
            this.bftResolutionComboBox.Items.Add("4096");
            this.bftResolutionComboBox.Items.Add("8192");
            this.bftResolutionComboBox.Items.Add("16384");
            this.bftResolutionComboBox.Location = new System.Drawing.Point(57, 104);
            this.bftResolutionComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.bftResolutionComboBox.Name = "bftResolutionComboBox";
            this.bftResolutionComboBox.SelectedIndex = -1;
            this.bftResolutionComboBox.Size = new System.Drawing.Size(83, 20);
            this.bftResolutionComboBox.TabIndex = 109;
            this.bftResolutionComboBox.Text = "xxxx";
            this.toolTip.SetToolTip(this.bftResolutionComboBox, "FFT resolution IF/AF spectrum");
            this.bftResolutionComboBox.ToolTip = this.toolTip;
            this.bftResolutionComboBox.SelectedIndexChanged += new System.EventHandler(this.bftResolutionComboBox_SelectedIndexChanged);
            // 
            // chkBaseBand
            // 
            this.chkBaseBand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBaseBand.Arrow = 99;
            this.chkBaseBand.Checked = false;
            this.chkBaseBand.Edge = 0.15F;
            this.chkBaseBand.EndColor = System.Drawing.Color.White;
            this.chkBaseBand.EndFactor = 0.2F;
            this.chkBaseBand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBaseBand.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkBaseBand.Location = new System.Drawing.Point(152, 136);
            this.chkBaseBand.Name = "chkBaseBand";
            this.chkBaseBand.NoBorder = false;
            this.chkBaseBand.NoLed = false;
            this.chkBaseBand.RadioButton = false;
            this.chkBaseBand.Radius = 6;
            this.chkBaseBand.RadiusB = 0;
            this.chkBaseBand.Size = new System.Drawing.Size(46, 23);
            this.chkBaseBand.StartColor = System.Drawing.Color.Black;
            this.chkBaseBand.StartFactor = 0.35F;
            this.chkBaseBand.TabIndex = 99;
            this.chkBaseBand.Text = "IF";
            this.toolTip.SetToolTip(this.chkBaseBand, "Show IF in RF spectrum");
            this.chkBaseBand.CheckedChanged += new System.EventHandler(this.chkBaseBand_CheckedChanged);
            // 
            // latencyNumericUpDown
            // 
            this.latencyNumericUpDown.ForeColor = System.Drawing.Color.Yellow;
            this.latencyNumericUpDown.Increment = 10;
            this.latencyNumericUpDown.Location = new System.Drawing.Point(56, 321);
            this.latencyNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.latencyNumericUpDown.Maximum = ((long)(2000));
            this.latencyNumericUpDown.Minimum = ((long)(1));
            this.latencyNumericUpDown.Name = "latencyNumericUpDown";
            this.latencyNumericUpDown.Size = new System.Drawing.Size(83, 21);
            this.latencyNumericUpDown.TabIndex = 89;
            this.latencyNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.latencyNumericUpDown, "Audio latency (100ms)");
            this.latencyNumericUpDown.Value = ((long)(100));
            this.latencyNumericUpDown.Enter += new System.EventHandler(this.gUpDown_Enter);
            this.latencyNumericUpDown.Leave += new System.EventHandler(this.gpUpDown_Leave);
            // 
            // fftWindowComboBox
            // 
            this.fftWindowComboBox.BackColor = System.Drawing.Color.Transparent;
            this.fftWindowComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.fftWindowComboBox.Items.Add("None");
            this.fftWindowComboBox.Items.Add("Hamming");
            this.fftWindowComboBox.Items.Add("Blackman");
            this.fftWindowComboBox.Items.Add("Blackman-Harris 4");
            this.fftWindowComboBox.Items.Add("Blackman-Harris 7");
            this.fftWindowComboBox.Items.Add("Hann-Poisson");
            this.fftWindowComboBox.Items.Add("Youssef");
            this.fftWindowComboBox.Location = new System.Drawing.Point(57, 25);
            this.fftWindowComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.fftWindowComboBox.Name = "fftWindowComboBox";
            this.fftWindowComboBox.SelectedIndex = -1;
            this.fftWindowComboBox.Size = new System.Drawing.Size(139, 20);
            this.fftWindowComboBox.TabIndex = 85;
            this.fftWindowComboBox.Tag = "";
            this.fftWindowComboBox.Text = "xxxx";
            this.toolTip.SetToolTip(this.fftWindowComboBox, "FFT/spectrum window type");
            this.fftWindowComboBox.ToolTip = this.toolTip;
            this.fftWindowComboBox.SelectedIndexChanged += new System.EventHandler(this.fftWindowComboBox_SelectedIndexChanged);
            // 
            // chkFastConv
            // 
            this.chkFastConv.Arrow = 99;
            this.chkFastConv.Checked = false;
            this.chkFastConv.Edge = 0.15F;
            this.chkFastConv.EndColor = System.Drawing.Color.White;
            this.chkFastConv.EndFactor = 0.2F;
            this.chkFastConv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFastConv.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkFastConv.Location = new System.Drawing.Point(152, 106);
            this.chkFastConv.Name = "chkFastConv";
            this.chkFastConv.NoBorder = false;
            this.chkFastConv.NoLed = false;
            this.chkFastConv.RadioButton = false;
            this.chkFastConv.Radius = 6;
            this.chkFastConv.RadiusB = 0;
            this.chkFastConv.Size = new System.Drawing.Size(46, 21);
            this.chkFastConv.StartColor = System.Drawing.Color.Black;
            this.chkFastConv.StartFactor = 0.35F;
            this.chkFastConv.TabIndex = 91;
            this.chkFastConv.Tag = "";
            this.chkFastConv.Text = " Conv.";
            this.toolTip.SetToolTip(this.chkFastConv, "use \'Fast-Convolve\' for FFT calculation");
            this.chkFastConv.CheckedChanged += new System.EventHandler(this.chkFastConv_CheckedChanged);
            // 
            // filterOrderNumericUpDown
            // 
            this.filterOrderNumericUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.filterOrderNumericUpDown.Increment = 10;
            this.filterOrderNumericUpDown.Location = new System.Drawing.Point(57, 78);
            this.filterOrderNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.filterOrderNumericUpDown.Maximum = ((long)(9999));
            this.filterOrderNumericUpDown.Minimum = ((long)(10));
            this.filterOrderNumericUpDown.Name = "filterOrderNumericUpDown";
            this.filterOrderNumericUpDown.Size = new System.Drawing.Size(83, 20);
            this.filterOrderNumericUpDown.TabIndex = 88;
            this.filterOrderNumericUpDown.Tag = "";
            this.filterOrderNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.filterOrderNumericUpDown, "AF/bandwidth filter order");
            this.filterOrderNumericUpDown.Value = ((long)(400));
            this.filterOrderNumericUpDown.ValueChanged += new System.EventHandler(this.filterOrderNumericUpDown_ValueChanged);
            this.filterOrderNumericUpDown.Enter += new System.EventHandler(this.gUpDown_Enter);
            this.filterOrderNumericUpDown.Leave += new System.EventHandler(this.gpUpDown_Leave);
            // 
            // filterTypeComboBox
            // 
            this.filterTypeComboBox.BackColor = System.Drawing.Color.Transparent;
            this.filterTypeComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.filterTypeComboBox.Items.Add("Hamming");
            this.filterTypeComboBox.Items.Add("Blackman");
            this.filterTypeComboBox.Items.Add("Blackman-Harris 4");
            this.filterTypeComboBox.Items.Add("Blackman-Harris 7");
            this.filterTypeComboBox.Items.Add("Hann-Poisson");
            this.filterTypeComboBox.Items.Add("Youssef");
            this.filterTypeComboBox.Location = new System.Drawing.Point(57, 50);
            this.filterTypeComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.filterTypeComboBox.Name = "filterTypeComboBox";
            this.filterTypeComboBox.SelectedIndex = -1;
            this.filterTypeComboBox.Size = new System.Drawing.Size(139, 20);
            this.filterTypeComboBox.TabIndex = 86;
            this.filterTypeComboBox.Tag = "";
            this.filterTypeComboBox.Text = "xxxx";
            this.toolTip.SetToolTip(this.filterTypeComboBox, "AF/bandwidth filter type.");
            this.filterTypeComboBox.ToolTip = this.toolTip;
            this.filterTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.filterTypeComboBox_SelectedIndexChanged);
            // 
            // swapIQCheckBox
            // 
            this.swapIQCheckBox.Arrow = 99;
            this.swapIQCheckBox.Checked = false;
            this.swapIQCheckBox.Edge = 0.15F;
            this.swapIQCheckBox.EndColor = System.Drawing.Color.White;
            this.swapIQCheckBox.EndFactor = 0.2F;
            this.swapIQCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapIQCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.swapIQCheckBox.Location = new System.Drawing.Point(149, 109);
            this.swapIQCheckBox.Name = "swapIQCheckBox";
            this.swapIQCheckBox.NoBorder = false;
            this.swapIQCheckBox.NoLed = false;
            this.swapIQCheckBox.RadioButton = false;
            this.swapIQCheckBox.Radius = 6;
            this.swapIQCheckBox.RadiusB = 0;
            this.swapIQCheckBox.Size = new System.Drawing.Size(46, 24);
            this.swapIQCheckBox.StartColor = System.Drawing.Color.Black;
            this.swapIQCheckBox.StartFactor = 0.35F;
            this.swapIQCheckBox.TabIndex = 18;
            this.swapIQCheckBox.Text = "Swap";
            this.toolTip.SetToolTip(this.swapIQCheckBox, "Swap RF spectrum ");
            this.swapIQCheckBox.CheckedChanged += new System.EventHandler(this.swapIQCheckBox_CheckedChanged);
            // 
            // squelchNumericUpDown
            // 
            this.squelchNumericUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.squelchNumericUpDown.Increment = 1;
            this.squelchNumericUpDown.Location = new System.Drawing.Point(51, 132);
            this.squelchNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.squelchNumericUpDown.Maximum = ((long)(32767));
            this.squelchNumericUpDown.Minimum = ((long)(-32767));
            this.squelchNumericUpDown.Name = "squelchNumericUpDown";
            this.squelchNumericUpDown.Size = new System.Drawing.Size(79, 21);
            this.squelchNumericUpDown.TabIndex = 14;
            this.squelchNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.squelchNumericUpDown, "Squelch level (dBm)");
            this.squelchNumericUpDown.Value = ((long)(22));
            this.squelchNumericUpDown.ValueChanged += new System.EventHandler(this.squelchNumericUpDown_ValueChanged);
            this.squelchNumericUpDown.Enter += new System.EventHandler(this.gUpDown_Enter);
            this.squelchNumericUpDown.Leave += new System.EventHandler(this.gpUpDown_Leave);
            // 
            // useSquelchCheckBox
            // 
            this.useSquelchCheckBox.Arrow = 99;
            this.useSquelchCheckBox.Checked = false;
            this.useSquelchCheckBox.Edge = 0.15F;
            this.useSquelchCheckBox.EndColor = System.Drawing.Color.White;
            this.useSquelchCheckBox.EndFactor = 0.2F;
            this.useSquelchCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useSquelchCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.useSquelchCheckBox.Location = new System.Drawing.Point(0, 129);
            this.useSquelchCheckBox.Name = "useSquelchCheckBox";
            this.useSquelchCheckBox.NoBorder = false;
            this.useSquelchCheckBox.NoLed = false;
            this.useSquelchCheckBox.RadioButton = false;
            this.useSquelchCheckBox.Radius = 6;
            this.useSquelchCheckBox.RadiusB = 0;
            this.useSquelchCheckBox.Size = new System.Drawing.Size(46, 24);
            this.useSquelchCheckBox.StartColor = System.Drawing.Color.Black;
            this.useSquelchCheckBox.StartFactor = 0.35F;
            this.useSquelchCheckBox.TabIndex = 13;
            this.useSquelchCheckBox.Text = "SQ";
            this.toolTip.SetToolTip(this.useSquelchCheckBox, "AM/FM squelch on/off");
            this.useSquelchCheckBox.CheckedChanged += new System.EventHandler(this.useSquelchCheckBox_CheckedChanged);
            // 
            // stepSizeComboBox
            // 
            this.stepSizeComboBox.BackColor = System.Drawing.Color.Transparent;
            this.stepSizeComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.stepSizeComboBox.Items.Add("1 Hz");
            this.stepSizeComboBox.Items.Add("10 Hz");
            this.stepSizeComboBox.Items.Add("100 Hz");
            this.stepSizeComboBox.Items.Add("500 Hz");
            this.stepSizeComboBox.Items.Add("1 kHz");
            this.stepSizeComboBox.Items.Add("2.5 kHz");
            this.stepSizeComboBox.Items.Add("5 kHz");
            this.stepSizeComboBox.Items.Add("6.25 kHz");
            this.stepSizeComboBox.Items.Add("8.33 kHz");
            this.stepSizeComboBox.Items.Add("9 kHz");
            this.stepSizeComboBox.Items.Add("10 kHz");
            this.stepSizeComboBox.Items.Add("12.5 kHz");
            this.stepSizeComboBox.Items.Add("25 kHz");
            this.stepSizeComboBox.Items.Add("50 kHz");
            this.stepSizeComboBox.Items.Add("100 kHz");
            this.stepSizeComboBox.Items.Add("150 kHz");
            this.stepSizeComboBox.Items.Add("200 kHz");
            this.stepSizeComboBox.Items.Add("250 kHz");
            this.stepSizeComboBox.Location = new System.Drawing.Point(51, 106);
            this.stepSizeComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.stepSizeComboBox.Name = "stepSizeComboBox";
            this.stepSizeComboBox.SelectedIndex = -1;
            this.stepSizeComboBox.Size = new System.Drawing.Size(79, 21);
            this.stepSizeComboBox.TabIndex = 10;
            this.stepSizeComboBox.Text = "xxxx";
            this.toolTip.SetToolTip(this.stepSizeComboBox, "Stepsize for VFO freq. snap");
            this.stepSizeComboBox.ToolTip = this.toolTip;
            this.stepSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.stepSizeComboBox_SelectedIndexChanged);
            // 
            // snapFrequencyCheckBox
            // 
            this.snapFrequencyCheckBox.Arrow = 99;
            this.snapFrequencyCheckBox.Checked = false;
            this.snapFrequencyCheckBox.Edge = 0.15F;
            this.snapFrequencyCheckBox.EndColor = System.Drawing.Color.White;
            this.snapFrequencyCheckBox.EndFactor = 0.2F;
            this.snapFrequencyCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.snapFrequencyCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.snapFrequencyCheckBox.Location = new System.Drawing.Point(0, 102);
            this.snapFrequencyCheckBox.Name = "snapFrequencyCheckBox";
            this.snapFrequencyCheckBox.NoBorder = false;
            this.snapFrequencyCheckBox.NoLed = false;
            this.snapFrequencyCheckBox.RadioButton = false;
            this.snapFrequencyCheckBox.Radius = 6;
            this.snapFrequencyCheckBox.RadiusB = 0;
            this.snapFrequencyCheckBox.Size = new System.Drawing.Size(46, 24);
            this.snapFrequencyCheckBox.StartColor = System.Drawing.Color.Black;
            this.snapFrequencyCheckBox.StartFactor = 0.35F;
            this.snapFrequencyCheckBox.TabIndex = 16;
            this.snapFrequencyCheckBox.Tag = "";
            this.snapFrequencyCheckBox.Text = "Snap";
            this.toolTip.SetToolTip(this.snapFrequencyCheckBox, "Snap VFO freq. to stepsize");
            this.snapFrequencyCheckBox.CheckedChanged += new System.EventHandler(this.stepSizeComboBox_SelectedIndexChanged);
            // 
            // cmbDbm
            // 
            this.cmbDbm.BackColor = System.Drawing.Color.Transparent;
            this.cmbDbm.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbDbm.Items.Add("dBm");
            this.cmbDbm.Items.Add("dBuV");
            this.cmbDbm.Items.Add("S-pts");
            this.cmbDbm.Items.Add("%");
            this.cmbDbm.Location = new System.Drawing.Point(51, 80);
            this.cmbDbm.Margin = new System.Windows.Forms.Padding(5);
            this.cmbDbm.Name = "cmbDbm";
            this.cmbDbm.SelectedIndex = -1;
            this.cmbDbm.Size = new System.Drawing.Size(79, 21);
            this.cmbDbm.TabIndex = 11;
            this.cmbDbm.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbDbm, "Spectrum/S-meter labels");
            this.cmbDbm.ToolTip = this.toolTip;
            this.cmbDbm.SelectedIndexChanged += new System.EventHandler(this.cmbDbm_SelectedIndexChanged);
            // 
            // agcCheckBox
            // 
            this.agcCheckBox.Arrow = 99;
            this.agcCheckBox.Checked = false;
            this.agcCheckBox.Edge = 0.15F;
            this.agcCheckBox.EndColor = System.Drawing.Color.White;
            this.agcCheckBox.EndFactor = 0.2F;
            this.agcCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agcCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.agcCheckBox.Location = new System.Drawing.Point(149, 166);
            this.agcCheckBox.Name = "agcCheckBox";
            this.agcCheckBox.NoBorder = false;
            this.agcCheckBox.NoLed = false;
            this.agcCheckBox.RadioButton = false;
            this.agcCheckBox.Radius = 6;
            this.agcCheckBox.RadiusB = 0;
            this.agcCheckBox.Size = new System.Drawing.Size(46, 24);
            this.agcCheckBox.StartColor = System.Drawing.Color.Black;
            this.agcCheckBox.StartFactor = 0.35F;
            this.agcCheckBox.TabIndex = 19;
            this.agcCheckBox.Text = "AGC";
            this.toolTip.SetToolTip(this.agcCheckBox, "Automatic Gain Control on/off");
            this.agcCheckBox.CheckedChanged += new System.EventHandler(this.agcCheckBox_CheckedChanged);
            // 
            // rawRadioButton
            // 
            this.rawRadioButton.Arrow = 99;
            this.rawRadioButton.Checked = false;
            this.rawRadioButton.Edge = 0.15F;
            this.rawRadioButton.EndColor = System.Drawing.Color.White;
            this.rawRadioButton.EndFactor = 0.2F;
            this.rawRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rawRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.rawRadioButton.Location = new System.Drawing.Point(149, 76);
            this.rawRadioButton.Name = "rawRadioButton";
            this.rawRadioButton.NoBorder = false;
            this.rawRadioButton.NoLed = false;
            this.rawRadioButton.RadioButton = true;
            this.rawRadioButton.Radius = 6;
            this.rawRadioButton.RadiusB = 0;
            this.rawRadioButton.Size = new System.Drawing.Size(46, 24);
            this.rawRadioButton.StartColor = System.Drawing.Color.Black;
            this.rawRadioButton.StartFactor = 0.35F;
            this.rawRadioButton.TabIndex = 10;
            this.rawRadioButton.Text = "RAW";
            this.toolTip.SetToolTip(this.rawRadioButton, "RAW mode for use with VB cable");
            this.rawRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // correctIQCheckBox
            // 
            this.correctIQCheckBox.Arrow = 99;
            this.correctIQCheckBox.Checked = false;
            this.correctIQCheckBox.Edge = 0.15F;
            this.correctIQCheckBox.EndColor = System.Drawing.Color.White;
            this.correctIQCheckBox.EndFactor = 0.2F;
            this.correctIQCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.correctIQCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.correctIQCheckBox.Location = new System.Drawing.Point(149, 137);
            this.correctIQCheckBox.Name = "correctIQCheckBox";
            this.correctIQCheckBox.NoBorder = false;
            this.correctIQCheckBox.NoLed = false;
            this.correctIQCheckBox.RadioButton = false;
            this.correctIQCheckBox.Radius = 6;
            this.correctIQCheckBox.RadiusB = 0;
            this.correctIQCheckBox.Size = new System.Drawing.Size(46, 24);
            this.correctIQCheckBox.StartColor = System.Drawing.Color.Black;
            this.correctIQCheckBox.StartFactor = 0.35F;
            this.correctIQCheckBox.TabIndex = 73;
            this.correctIQCheckBox.Text = " Corr";
            this.toolTip.SetToolTip(this.correctIQCheckBox, "Correct RF center spike");
            this.correctIQCheckBox.CheckedChanged += new System.EventHandler(this.autoCorrectIQCheckBox_CheckStateChanged);
            // 
            // gBexpand
            // 
            this.gBexpand.Arrow = 0;
            this.gBexpand.Checked = false;
            this.gBexpand.Edge = 0.15F;
            this.gBexpand.EndColor = System.Drawing.Color.White;
            this.gBexpand.EndFactor = 0.2F;
            this.gBexpand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gBexpand.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.gBexpand.Location = new System.Drawing.Point(179, 271);
            this.gBexpand.Name = "gBexpand";
            this.gBexpand.NoBorder = false;
            this.gBexpand.NoLed = true;
            this.gBexpand.RadioButton = false;
            this.gBexpand.Radius = 4;
            this.gBexpand.RadiusB = 0;
            this.gBexpand.Size = new System.Drawing.Size(17, 16);
            this.gBexpand.StartColor = System.Drawing.Color.Black;
            this.gBexpand.StartFactor = 0.35F;
            this.gBexpand.TabIndex = 123;
            this.gBexpand.Text = "...";
            this.toolTip.SetToolTip(this.gBexpand, "More scope options");
            this.gBexpand.CheckedChanged += new System.EventHandler(this.gBexpand_CheckedChanged);
            // 
            // tbvPeakRel
            // 
            this.tbvPeakRel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbvPeakRel.Button = false;
            this.tbvPeakRel.Checked = false;
            this.tbvPeakRel.ColorFactor = 0.6F;
            this.tbvPeakRel.ForeColor = System.Drawing.Color.Red;
            this.tbvPeakRel.Location = new System.Drawing.Point(168, 450);
            this.tbvPeakRel.Margin = new System.Windows.Forms.Padding(4);
            this.tbvPeakRel.Maximum = 10;
            this.tbvPeakRel.Minimum = -10;
            this.tbvPeakRel.Name = "tbvPeakRel";
            this.tbvPeakRel.Size = new System.Drawing.Size(15, 73);
            this.tbvPeakRel.TabIndex = 48;
            this.tbvPeakRel.Tag = "1";
            this.tbvPeakRel.TickColor = System.Drawing.Color.Silver;
            this.tbvPeakRel.Ticks = 5;
            this.tbvPeakRel.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbvPeakRel, "Peak release speed");
            this.tbvPeakRel.Value = 0;
            this.tbvPeakRel.ValueChanged += new System.EventHandler(this.tbvPeakRel_ValueChanged);
            // 
            // tbvPeakDelay
            // 
            this.tbvPeakDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbvPeakDelay.Button = false;
            this.tbvPeakDelay.Checked = false;
            this.tbvPeakDelay.ColorFactor = 0.6F;
            this.tbvPeakDelay.ForeColor = System.Drawing.Color.Red;
            this.tbvPeakDelay.Location = new System.Drawing.Point(131, 449);
            this.tbvPeakDelay.Margin = new System.Windows.Forms.Padding(4);
            this.tbvPeakDelay.Maximum = 10;
            this.tbvPeakDelay.Minimum = -10;
            this.tbvPeakDelay.Name = "tbvPeakDelay";
            this.tbvPeakDelay.Size = new System.Drawing.Size(15, 73);
            this.tbvPeakDelay.TabIndex = 47;
            this.tbvPeakDelay.Tag = "1";
            this.tbvPeakDelay.TickColor = System.Drawing.Color.Silver;
            this.tbvPeakDelay.Ticks = 5;
            this.tbvPeakDelay.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbvPeakDelay, "Peak attack speed");
            this.tbvPeakDelay.Value = 0;
            this.tbvPeakDelay.ValueChanged += new System.EventHandler(this.tbvPeakDelay_ValueChanged);
            // 
            // tbvAudioAvg
            // 
            this.tbvAudioAvg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbvAudioAvg.Button = false;
            this.tbvAudioAvg.Checked = false;
            this.tbvAudioAvg.ColorFactor = 0.6F;
            this.tbvAudioAvg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.tbvAudioAvg.Location = new System.Drawing.Point(92, 450);
            this.tbvAudioAvg.Margin = new System.Windows.Forms.Padding(4);
            this.tbvAudioAvg.Maximum = 10;
            this.tbvAudioAvg.Minimum = -10;
            this.tbvAudioAvg.Name = "tbvAudioAvg";
            this.tbvAudioAvg.Size = new System.Drawing.Size(15, 73);
            this.tbvAudioAvg.TabIndex = 46;
            this.tbvAudioAvg.Tag = "1";
            this.tbvAudioAvg.TickColor = System.Drawing.Color.Silver;
            this.tbvAudioAvg.Ticks = 5;
            this.tbvAudioAvg.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbvAudioAvg, "Audio averaging speed");
            this.tbvAudioAvg.Value = 0;
            this.tbvAudioAvg.ValueChanged += new System.EventHandler(this.tbvAudioAvg_ValueChanged);
            // 
            // tbvAudioRel
            // 
            this.tbvAudioRel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbvAudioRel.Button = false;
            this.tbvAudioRel.Checked = false;
            this.tbvAudioRel.ColorFactor = 0.6F;
            this.tbvAudioRel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.tbvAudioRel.Location = new System.Drawing.Point(53, 449);
            this.tbvAudioRel.Margin = new System.Windows.Forms.Padding(4);
            this.tbvAudioRel.Maximum = 10;
            this.tbvAudioRel.Minimum = -10;
            this.tbvAudioRel.Name = "tbvAudioRel";
            this.tbvAudioRel.Size = new System.Drawing.Size(15, 73);
            this.tbvAudioRel.TabIndex = 45;
            this.tbvAudioRel.Tag = "1";
            this.tbvAudioRel.TickColor = System.Drawing.Color.Silver;
            this.tbvAudioRel.Ticks = 5;
            this.tbvAudioRel.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbvAudioRel, "Audio release speed");
            this.tbvAudioRel.Value = 0;
            this.tbvAudioRel.ValueChanged += new System.EventHandler(this.tbvAudioRel_ValueChanged);
            // 
            // tbvCarrierAvg
            // 
            this.tbvCarrierAvg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbvCarrierAvg.Button = false;
            this.tbvCarrierAvg.Checked = false;
            this.tbvCarrierAvg.ColorFactor = 0.6F;
            this.tbvCarrierAvg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.tbvCarrierAvg.Location = new System.Drawing.Point(13, 450);
            this.tbvCarrierAvg.Margin = new System.Windows.Forms.Padding(4);
            this.tbvCarrierAvg.Maximum = 10;
            this.tbvCarrierAvg.Minimum = -10;
            this.tbvCarrierAvg.Name = "tbvCarrierAvg";
            this.tbvCarrierAvg.Size = new System.Drawing.Size(15, 73);
            this.tbvCarrierAvg.TabIndex = 44;
            this.tbvCarrierAvg.Tag = "1";
            this.tbvCarrierAvg.TickColor = System.Drawing.Color.Silver;
            this.tbvCarrierAvg.Ticks = 5;
            this.tbvCarrierAvg.ToolTip = null;
            this.toolTip.SetToolTip(this.tbvCarrierAvg, "Carrier avaraging speed");
            this.tbvCarrierAvg.Value = 0;
            this.tbvCarrierAvg.ValueChanged += new System.EventHandler(this.tbvCarrierAvg_ValueChanged);
            // 
            // tbTrigL
            // 
            this.tbTrigL.Button = false;
            this.tbTrigL.Checked = false;
            this.tbTrigL.ColorFactor = 0.55F;
            this.tbTrigL.ForeColor = System.Drawing.Color.Black;
            this.tbTrigL.Location = new System.Drawing.Point(132, 360);
            this.tbTrigL.Margin = new System.Windows.Forms.Padding(4);
            this.tbTrigL.Maximum = 50;
            this.tbTrigL.Minimum = -50;
            this.tbTrigL.Name = "tbTrigL";
            this.tbTrigL.Size = new System.Drawing.Size(66, 17);
            this.tbTrigL.TabIndex = 33;
            this.tbTrigL.TickColor = System.Drawing.Color.Black;
            this.tbTrigL.Ticks = 0;
            this.tbTrigL.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbTrigL, "Vertical triggel level");
            this.tbTrigL.Value = 0;
            this.tbTrigL.ValueChanged += new System.EventHandler(this.tbTrigL_ValueChanged);
            // 
            // cmbHchannel
            // 
            this.cmbHchannel.BackColor = System.Drawing.Color.Transparent;
            this.cmbHchannel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbHchannel.Items.Add("AM");
            this.cmbHchannel.Items.Add("FM");
            this.cmbHchannel.Items.Add("PM");
            this.cmbHchannel.Items.Add("Audio");
            this.cmbHchannel.Items.Add("Envel.");
            this.cmbHchannel.Items.Add("IQ");
            this.cmbHchannel.Location = new System.Drawing.Point(68, 391);
            this.cmbHchannel.Margin = new System.Windows.Forms.Padding(5);
            this.cmbHchannel.Name = "cmbHchannel";
            this.cmbHchannel.SelectedIndex = -1;
            this.cmbHchannel.Size = new System.Drawing.Size(62, 21);
            this.cmbHchannel.TabIndex = 31;
            this.cmbHchannel.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbHchannel, "Vetical input");
            this.cmbHchannel.ToolTip = this.toolTip;
            this.cmbHchannel.SelectedIndexChanged += new System.EventHandler(this.cmbHchannel_SelectedIndexChanged);
            // 
            // tbGain
            // 
            this.tbGain.Button = false;
            this.tbGain.Checked = false;
            this.tbGain.ColorFactor = 0.55F;
            this.tbGain.ForeColor = System.Drawing.Color.Black;
            this.tbGain.Location = new System.Drawing.Point(99, 570);
            this.tbGain.Margin = new System.Windows.Forms.Padding(4);
            this.tbGain.Maximum = 100;
            this.tbGain.Minimum = 1;
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(78, 16);
            this.tbGain.TabIndex = 42;
            this.tbGain.Tag = "1";
            this.tbGain.TickColor = System.Drawing.Color.Black;
            this.tbGain.Ticks = 0;
            this.tbGain.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbGain, "Carrier lock speed");
            this.tbGain.Value = 15;
            this.tbGain.ValueChanged += new System.EventHandler(this.tbGain_ValueChanged);
            // 
            // tbAverage
            // 
            this.tbAverage.Button = false;
            this.tbAverage.Checked = false;
            this.tbAverage.ColorFactor = 0.55F;
            this.tbAverage.ForeColor = System.Drawing.Color.Black;
            this.tbAverage.Location = new System.Drawing.Point(99, 549);
            this.tbAverage.Margin = new System.Windows.Forms.Padding(4);
            this.tbAverage.Maximum = 100;
            this.tbAverage.Minimum = 1;
            this.tbAverage.Name = "tbAverage";
            this.tbAverage.Size = new System.Drawing.Size(78, 15);
            this.tbAverage.TabIndex = 31;
            this.tbAverage.Tag = "1";
            this.tbAverage.TickColor = System.Drawing.Color.Black;
            this.tbAverage.Ticks = 0;
            this.tbAverage.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbAverage, "Carrier lock averaging");
            this.tbAverage.Value = 7;
            this.tbAverage.ValueChanged += new System.EventHandler(this.tbAverage_ValueChanged);
            // 
            // chkHrunDC
            // 
            this.chkHrunDC.Arrow = 99;
            this.chkHrunDC.Checked = false;
            this.chkHrunDC.Edge = 0.15F;
            this.chkHrunDC.EndColor = System.Drawing.Color.White;
            this.chkHrunDC.EndFactor = 0.2F;
            this.chkHrunDC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHrunDC.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkHrunDC.Location = new System.Drawing.Point(67, 362);
            this.chkHrunDC.Name = "chkHrunDC";
            this.chkHrunDC.NoBorder = false;
            this.chkHrunDC.NoLed = false;
            this.chkHrunDC.RadioButton = false;
            this.chkHrunDC.Radius = 6;
            this.chkHrunDC.RadiusB = 0;
            this.chkHrunDC.Size = new System.Drawing.Size(46, 23);
            this.chkHrunDC.StartColor = System.Drawing.Color.Black;
            this.chkHrunDC.StartFactor = 0.35F;
            this.chkHrunDC.TabIndex = 30;
            this.chkHrunDC.Text = "DC";
            this.toolTip.SetToolTip(this.chkHrunDC, "Enable DC");
            this.chkHrunDC.CheckedChanged += new System.EventHandler(this.chkHrunDC_CheckedChanged);
            // 
            // chkAver
            // 
            this.chkAver.Arrow = 0;
            this.chkAver.Checked = false;
            this.chkAver.Edge = 0.15F;
            this.chkAver.EndColor = System.Drawing.Color.White;
            this.chkAver.EndFactor = 0.2F;
            this.chkAver.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAver.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkAver.Location = new System.Drawing.Point(14, 561);
            this.chkAver.Name = "chkAver";
            this.chkAver.NoBorder = false;
            this.chkAver.NoLed = false;
            this.chkAver.RadioButton = false;
            this.chkAver.Radius = 6;
            this.chkAver.RadiusB = 0;
            this.chkAver.Size = new System.Drawing.Size(38, 26);
            this.chkAver.StartColor = System.Drawing.Color.Black;
            this.chkAver.StartFactor = 0.35F;
            this.chkAver.TabIndex = 43;
            this.chkAver.Tag = "1";
            this.chkAver.Text = " Filter";
            this.toolTip.SetToolTip(this.chkAver, "Carrier lock filtering");
            this.chkAver.CheckedChanged += new System.EventHandler(this.chkAver_CheckedChanged);
            // 
            // chkVrunDC
            // 
            this.chkVrunDC.Arrow = 99;
            this.chkVrunDC.Checked = false;
            this.chkVrunDC.Edge = 0.15F;
            this.chkVrunDC.EndColor = System.Drawing.Color.White;
            this.chkVrunDC.EndFactor = 0.2F;
            this.chkVrunDC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVrunDC.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkVrunDC.Location = new System.Drawing.Point(0, 362);
            this.chkVrunDC.Name = "chkVrunDC";
            this.chkVrunDC.NoBorder = false;
            this.chkVrunDC.NoLed = false;
            this.chkVrunDC.RadioButton = false;
            this.chkVrunDC.Radius = 6;
            this.chkVrunDC.RadiusB = 0;
            this.chkVrunDC.Size = new System.Drawing.Size(47, 23);
            this.chkVrunDC.StartColor = System.Drawing.Color.Black;
            this.chkVrunDC.StartFactor = 0.35F;
            this.chkVrunDC.TabIndex = 26;
            this.chkVrunDC.Text = "DC";
            this.toolTip.SetToolTip(this.chkVrunDC, "Enable DC");
            this.chkVrunDC.CheckedChanged += new System.EventHandler(this.chkVrunDC_CheckedChanged);
            // 
            // chkHinvert
            // 
            this.chkHinvert.Arrow = 99;
            this.chkHinvert.Checked = false;
            this.chkHinvert.Edge = 0.15F;
            this.chkHinvert.EndColor = System.Drawing.Color.White;
            this.chkHinvert.EndFactor = 0.2F;
            this.chkHinvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHinvert.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkHinvert.Location = new System.Drawing.Point(67, 336);
            this.chkHinvert.Name = "chkHinvert";
            this.chkHinvert.NoBorder = false;
            this.chkHinvert.NoLed = false;
            this.chkHinvert.RadioButton = false;
            this.chkHinvert.Radius = 6;
            this.chkHinvert.RadiusB = 0;
            this.chkHinvert.Size = new System.Drawing.Size(47, 22);
            this.chkHinvert.StartColor = System.Drawing.Color.Black;
            this.chkHinvert.StartFactor = 0.35F;
            this.chkHinvert.TabIndex = 29;
            this.chkHinvert.Text = "Inv. ";
            this.toolTip.SetToolTip(this.chkHinvert, "Invert H channel");
            this.chkHinvert.CheckedChanged += new System.EventHandler(this.chkHinvert_CheckedChanged);
            // 
            // chkXY
            // 
            this.chkXY.Arrow = 99;
            this.chkXY.BackColor = System.Drawing.Color.White;
            this.chkXY.Checked = false;
            this.chkXY.Edge = 0.15F;
            this.chkXY.EndColor = System.Drawing.Color.White;
            this.chkXY.EndFactor = 0.2F;
            this.chkXY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkXY.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkXY.Location = new System.Drawing.Point(138, 388);
            this.chkXY.Name = "chkXY";
            this.chkXY.NoBorder = false;
            this.chkXY.NoLed = false;
            this.chkXY.RadioButton = false;
            this.chkXY.Radius = 6;
            this.chkXY.RadiusB = 0;
            this.chkXY.Size = new System.Drawing.Size(38, 24);
            this.chkXY.StartColor = System.Drawing.Color.Black;
            this.chkXY.StartFactor = 0.35F;
            this.chkXY.TabIndex = 34;
            this.chkXY.Text = "XY";
            this.toolTip.SetToolTip(this.chkXY, "XY mode on/off");
            this.chkXY.CheckedChanged += new System.EventHandler(this.chkXY_CheckedChanged);
            // 
            // chkVinvert
            // 
            this.chkVinvert.Arrow = 99;
            this.chkVinvert.Checked = false;
            this.chkVinvert.Edge = 0.15F;
            this.chkVinvert.EndColor = System.Drawing.Color.White;
            this.chkVinvert.EndFactor = 0.2F;
            this.chkVinvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVinvert.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkVinvert.Location = new System.Drawing.Point(0, 336);
            this.chkVinvert.Name = "chkVinvert";
            this.chkVinvert.NoBorder = false;
            this.chkVinvert.NoLed = false;
            this.chkVinvert.RadioButton = false;
            this.chkVinvert.Radius = 6;
            this.chkVinvert.RadiusB = 0;
            this.chkVinvert.Size = new System.Drawing.Size(48, 22);
            this.chkVinvert.StartColor = System.Drawing.Color.Black;
            this.chkVinvert.StartFactor = 0.35F;
            this.chkVinvert.TabIndex = 25;
            this.chkVinvert.Text = "Inv.";
            this.toolTip.SetToolTip(this.chkVinvert, "Invert V channel");
            this.chkVinvert.CheckedChanged += new System.EventHandler(this.chkVinvert_CheckedChanged);
            // 
            // cmbTim
            // 
            this.cmbTim.BackColor = System.Drawing.Color.Transparent;
            this.cmbTim.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbTim.Items.Add("10 ms");
            this.cmbTim.Items.Add("5 ms");
            this.cmbTim.Items.Add("2 ms");
            this.cmbTim.Items.Add("1 ms");
            this.cmbTim.Items.Add("500 us");
            this.cmbTim.Items.Add("200 us");
            this.cmbTim.Items.Add("100 us");
            this.cmbTim.Location = new System.Drawing.Point(137, 309);
            this.cmbTim.Margin = new System.Windows.Forms.Padding(5);
            this.cmbTim.Name = "cmbTim";
            this.cmbTim.SelectedIndex = -1;
            this.cmbTim.Size = new System.Drawing.Size(62, 21);
            this.cmbTim.TabIndex = 32;
            this.cmbTim.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbTim, "Timebase/speed");
            this.cmbTim.ToolTip = this.toolTip;
            this.cmbTim.SelectedIndexChanged += new System.EventHandler(this.cmbTim_SelectedIndexChanged);
            // 
            // cmbHor
            // 
            this.cmbHor.BackColor = System.Drawing.Color.Transparent;
            this.cmbHor.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbHor.Items.Add("10 mV");
            this.cmbHor.Items.Add("5 mV");
            this.cmbHor.Items.Add("2 mV");
            this.cmbHor.Items.Add("1 mV");
            this.cmbHor.Items.Add("0.5 mV");
            this.cmbHor.Items.Add("0.2 mV");
            this.cmbHor.Items.Add("0.1 mV");
            this.cmbHor.Location = new System.Drawing.Point(69, 309);
            this.cmbHor.Margin = new System.Windows.Forms.Padding(5);
            this.cmbHor.Name = "cmbHor";
            this.cmbHor.SelectedIndex = -1;
            this.cmbHor.Size = new System.Drawing.Size(62, 21);
            this.cmbHor.TabIndex = 28;
            this.cmbHor.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbHor, "Horizontal scale");
            this.cmbHor.ToolTip = this.toolTip;
            this.cmbHor.SelectedIndexChanged += new System.EventHandler(this.cmbHor_SelectedIndexChanged);
            // 
            // cmbVer
            // 
            this.cmbVer.BackColor = System.Drawing.Color.Transparent;
            this.cmbVer.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbVer.Items.Add("10 mV");
            this.cmbVer.Items.Add("5 mV");
            this.cmbVer.Items.Add("2 mV");
            this.cmbVer.Items.Add("1 mV");
            this.cmbVer.Items.Add("0.5 mV");
            this.cmbVer.Items.Add("0.2 mV");
            this.cmbVer.Items.Add("0.1 mV");
            this.cmbVer.Location = new System.Drawing.Point(1, 309);
            this.cmbVer.Margin = new System.Windows.Forms.Padding(5);
            this.cmbVer.Name = "cmbVer";
            this.cmbVer.SelectedIndex = -1;
            this.cmbVer.Size = new System.Drawing.Size(62, 21);
            this.cmbVer.TabIndex = 24;
            this.cmbVer.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbVer, "Vertical scale");
            this.cmbVer.ToolTip = this.toolTip;
            this.cmbVer.SelectedIndexChanged += new System.EventHandler(this.cmbVer_SelectedIndexChanged);
            // 
            // tbRFgain
            // 
            this.tbRFgain.Button = false;
            this.tbRFgain.Checked = false;
            this.tbRFgain.ColorFactor = 0.5F;
            this.tbRFgain.ForeColor = System.Drawing.Color.Black;
            this.tbRFgain.Location = new System.Drawing.Point(54, 24);
            this.tbRFgain.Margin = new System.Windows.Forms.Padding(4);
            this.tbRFgain.Maximum = 40;
            this.tbRFgain.Minimum = -55;
            this.tbRFgain.Name = "tbRFgain";
            this.tbRFgain.Size = new System.Drawing.Size(140, 18);
            this.tbRFgain.TabIndex = 23;
            this.tbRFgain.TickColor = System.Drawing.Color.Silver;
            this.tbRFgain.Ticks = 10;
            this.tbRFgain.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbRFgain, "Gain for sope display.");
            this.tbRFgain.Value = 0;
            this.tbRFgain.ValueChanged += new System.EventHandler(this.tbRFgain_ValueChanged);
            // 
            // cmbVchannel
            // 
            this.cmbVchannel.BackColor = System.Drawing.Color.Transparent;
            this.cmbVchannel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbVchannel.Items.Add("AM");
            this.cmbVchannel.Items.Add("FM");
            this.cmbVchannel.Items.Add("PM");
            this.cmbVchannel.Items.Add("Audio");
            this.cmbVchannel.Items.Add("Envel.");
            this.cmbVchannel.Items.Add("IQ");
            this.cmbVchannel.Location = new System.Drawing.Point(1, 390);
            this.cmbVchannel.Margin = new System.Windows.Forms.Padding(5);
            this.cmbVchannel.Name = "cmbVchannel";
            this.cmbVchannel.SelectedIndex = -1;
            this.cmbVchannel.Size = new System.Drawing.Size(62, 21);
            this.cmbVchannel.TabIndex = 27;
            this.cmbVchannel.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbVchannel, "Horizontal input");
            this.cmbVchannel.ToolTip = this.toolTip;
            this.cmbVchannel.SelectedIndexChanged += new System.EventHandler(this.cmbVchannel_SelectedIndexChanged);
            // 
            // gBexpandScope
            // 
            this.gBexpandScope.Arrow = 0;
            this.gBexpandScope.Checked = false;
            this.gBexpandScope.Edge = 0.15F;
            this.gBexpandScope.EndColor = System.Drawing.Color.White;
            this.gBexpandScope.EndFactor = 0.2F;
            this.gBexpandScope.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gBexpandScope.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.gBexpandScope.Location = new System.Drawing.Point(180, 396);
            this.gBexpandScope.Name = "gBexpandScope";
            this.gBexpandScope.NoBorder = false;
            this.gBexpandScope.NoLed = true;
            this.gBexpandScope.RadioButton = false;
            this.gBexpandScope.Radius = 4;
            this.gBexpandScope.RadiusB = 0;
            this.gBexpandScope.Size = new System.Drawing.Size(17, 16);
            this.gBexpandScope.StartColor = System.Drawing.Color.Black;
            this.gBexpandScope.StartFactor = 0.35F;
            this.gBexpandScope.TabIndex = 35;
            this.gBexpandScope.Text = "...";
            this.toolTip.SetToolTip(this.gBexpandScope, "More audio-bar options");
            this.gBexpandScope.CheckedChanged += new System.EventHandler(this.gBexpandScope_CheckedChanged);
            // 
            // cwShiftNumericUpDown
            // 
            this.cwShiftNumericUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cwShiftNumericUpDown.Increment = 10;
            this.cwShiftNumericUpDown.Location = new System.Drawing.Point(54, 144);
            this.cwShiftNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.cwShiftNumericUpDown.Maximum = ((long)(1200));
            this.cwShiftNumericUpDown.Minimum = ((long)(200));
            this.cwShiftNumericUpDown.Name = "cwShiftNumericUpDown";
            this.cwShiftNumericUpDown.Size = new System.Drawing.Size(82, 21);
            this.cwShiftNumericUpDown.TabIndex = 57;
            this.cwShiftNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.cwShiftNumericUpDown, "CW shift (tone)");
            this.cwShiftNumericUpDown.Value = ((long)(600));
            this.cwShiftNumericUpDown.ValueChanged += new System.EventHandler(this.cwShiftNumericUpDown_ValueChanged);
            this.cwShiftNumericUpDown.Enter += new System.EventHandler(this.gUpDown_Enter);
            this.cwShiftNumericUpDown.Leave += new System.EventHandler(this.gpUpDown_Leave);
            // 
            // outputDeviceComboBox
            // 
            this.outputDeviceComboBox.BackColor = System.Drawing.Color.Transparent;
            this.outputDeviceComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.outputDeviceComboBox.Location = new System.Drawing.Point(54, 85);
            this.outputDeviceComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.outputDeviceComboBox.Name = "outputDeviceComboBox";
            this.outputDeviceComboBox.SelectedIndex = -1;
            this.outputDeviceComboBox.Size = new System.Drawing.Size(142, 21);
            this.outputDeviceComboBox.TabIndex = 55;
            this.outputDeviceComboBox.Text = "(no device found)";
            this.toolTip.SetToolTip(this.outputDeviceComboBox, "Audio output device");
            this.outputDeviceComboBox.ToolTip = this.toolTip;
            this.outputDeviceComboBox.SelectedIndexChanged += new System.EventHandler(this.audioDeviceComboBox_SelectedIndexChanged);
            // 
            // inputDeviceComboBox
            // 
            this.inputDeviceComboBox.BackColor = System.Drawing.Color.Transparent;
            this.inputDeviceComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.inputDeviceComboBox.Location = new System.Drawing.Point(54, 57);
            this.inputDeviceComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.inputDeviceComboBox.Name = "inputDeviceComboBox";
            this.inputDeviceComboBox.SelectedIndex = -1;
            this.inputDeviceComboBox.Size = new System.Drawing.Size(142, 21);
            this.inputDeviceComboBox.TabIndex = 54;
            this.inputDeviceComboBox.Text = "(no device found)";
            this.toolTip.SetToolTip(this.inputDeviceComboBox, "Audio input device");
            this.inputDeviceComboBox.ToolTip = this.toolTip;
            this.inputDeviceComboBox.SelectedIndexChanged += new System.EventHandler(this.audioDeviceComboBox_SelectedIndexChanged);
            // 
            // sampleRateComboBox
            // 
            this.sampleRateComboBox.BackColor = System.Drawing.Color.Transparent;
            this.sampleRateComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.sampleRateComboBox.Items.Add("1000 s/sec");
            this.sampleRateComboBox.Items.Add("3000 s/sec");
            this.sampleRateComboBox.Items.Add("5000 s/sec");
            this.sampleRateComboBox.Items.Add("8000 s/sec");
            this.sampleRateComboBox.Items.Add("11025 s/sec");
            this.sampleRateComboBox.Items.Add("16000 s/sec");
            this.sampleRateComboBox.Items.Add("22050 s/sec");
            this.sampleRateComboBox.Items.Add("24000 s/sec");
            this.sampleRateComboBox.Items.Add("32000 s/sec");
            this.sampleRateComboBox.Items.Add("44100 s/sec");
            this.sampleRateComboBox.Items.Add("48000 s/sec");
            this.sampleRateComboBox.Items.Add("80000 s/sec");
            this.sampleRateComboBox.Items.Add("96000 s/sec");
            this.sampleRateComboBox.Items.Add("120000 s/sec");
            this.sampleRateComboBox.Items.Add("125000 s/sec");
            this.sampleRateComboBox.Items.Add("150000 s/sec");
            this.sampleRateComboBox.Items.Add("192000 s/sec");
            this.sampleRateComboBox.Location = new System.Drawing.Point(54, 116);
            this.sampleRateComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.sampleRateComboBox.Name = "sampleRateComboBox";
            this.sampleRateComboBox.SelectedIndex = -1;
            this.sampleRateComboBox.Size = new System.Drawing.Size(88, 21);
            this.sampleRateComboBox.TabIndex = 56;
            this.sampleRateComboBox.Text = "(unknown)";
            this.toolTip.SetToolTip(this.sampleRateComboBox, "Minimum audio sampling rate");
            this.sampleRateComboBox.ToolTip = this.toolTip;
            this.sampleRateComboBox.SelectedIndexChanged += new System.EventHandler(this.sampleRateComboBox_SelectedIndexChanged);
            // 
            // filterAudioCheckBox
            // 
            this.filterAudioCheckBox.Arrow = 99;
            this.filterAudioCheckBox.Checked = false;
            this.filterAudioCheckBox.Edge = 0.15F;
            this.filterAudioCheckBox.EndColor = System.Drawing.Color.White;
            this.filterAudioCheckBox.EndFactor = 0.2F;
            this.filterAudioCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterAudioCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.filterAudioCheckBox.Location = new System.Drawing.Point(148, 116);
            this.filterAudioCheckBox.Name = "filterAudioCheckBox";
            this.filterAudioCheckBox.NoBorder = false;
            this.filterAudioCheckBox.NoLed = false;
            this.filterAudioCheckBox.RadioButton = false;
            this.filterAudioCheckBox.Radius = 6;
            this.filterAudioCheckBox.RadiusB = 0;
            this.filterAudioCheckBox.Size = new System.Drawing.Size(50, 24);
            this.filterAudioCheckBox.StartColor = System.Drawing.Color.Black;
            this.filterAudioCheckBox.StartFactor = 0.35F;
            this.filterAudioCheckBox.TabIndex = 58;
            this.filterAudioCheckBox.Text = "Filter";
            this.toolTip.SetToolTip(this.filterAudioCheckBox, "Enable audio filter");
            this.filterAudioCheckBox.CheckedChanged += new System.EventHandler(this.filterAudioCheckBox_CheckStateChanged);
            // 
            // fmStereoCheckBox
            // 
            this.fmStereoCheckBox.Arrow = 99;
            this.fmStereoCheckBox.Checked = false;
            this.fmStereoCheckBox.Edge = 0.15F;
            this.fmStereoCheckBox.EndColor = System.Drawing.Color.White;
            this.fmStereoCheckBox.EndFactor = 0.2F;
            this.fmStereoCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fmStereoCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.fmStereoCheckBox.Location = new System.Drawing.Point(148, 144);
            this.fmStereoCheckBox.Name = "fmStereoCheckBox";
            this.fmStereoCheckBox.NoBorder = false;
            this.fmStereoCheckBox.NoLed = false;
            this.fmStereoCheckBox.RadioButton = false;
            this.fmStereoCheckBox.Radius = 6;
            this.fmStereoCheckBox.RadiusB = 0;
            this.fmStereoCheckBox.Size = new System.Drawing.Size(50, 24);
            this.fmStereoCheckBox.StartColor = System.Drawing.Color.Black;
            this.fmStereoCheckBox.StartFactor = 0.35F;
            this.fmStereoCheckBox.TabIndex = 59;
            this.fmStereoCheckBox.Text = "Ster.";
            this.toolTip.SetToolTip(this.fmStereoCheckBox, "Enable FM stereo / AM pseudo");
            this.fmStereoCheckBox.CheckedChanged += new System.EventHandler(this.fmStereoCheckBox_CheckedChanged);
            // 
            // chkNotch0
            // 
            this.chkNotch0.Arrow = 0;
            this.chkNotch0.Checked = false;
            this.chkNotch0.Edge = 0.15F;
            this.chkNotch0.EndColor = System.Drawing.Color.White;
            this.chkNotch0.EndFactor = 0.2F;
            this.chkNotch0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNotch0.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkNotch0.Location = new System.Drawing.Point(54, 23);
            this.chkNotch0.Name = "chkNotch0";
            this.chkNotch0.NoBorder = false;
            this.chkNotch0.NoLed = false;
            this.chkNotch0.RadioButton = false;
            this.chkNotch0.Radius = 6;
            this.chkNotch0.RadiusB = 0;
            this.chkNotch0.Size = new System.Drawing.Size(30, 26);
            this.chkNotch0.StartColor = System.Drawing.Color.Black;
            this.chkNotch0.StartFactor = 0.35F;
            this.chkNotch0.TabIndex = 50;
            this.chkNotch0.Text = "1";
            this.toolTip.SetToolTip(this.chkNotch0, "IF/baseband notch");
            this.chkNotch0.CheckedChanged += new System.EventHandler(this.chkNotch_CheckedChanged);
            // 
            // chkNLimiter
            // 
            this.chkNLimiter.Arrow = 99;
            this.chkNLimiter.Checked = false;
            this.chkNLimiter.Edge = 0.15F;
            this.chkNLimiter.EndColor = System.Drawing.Color.White;
            this.chkNLimiter.EndFactor = 0.2F;
            this.chkNLimiter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNLimiter.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkNLimiter.Location = new System.Drawing.Point(1, 7);
            this.chkNLimiter.Name = "chkNLimiter";
            this.chkNLimiter.NoBorder = false;
            this.chkNLimiter.NoLed = false;
            this.chkNLimiter.RadioButton = false;
            this.chkNLimiter.Radius = 6;
            this.chkNLimiter.RadiusB = 0;
            this.chkNLimiter.Size = new System.Drawing.Size(46, 24);
            this.chkNLimiter.StartColor = System.Drawing.Color.Black;
            this.chkNLimiter.StartFactor = 0.35F;
            this.chkNLimiter.TabIndex = 65;
            this.chkNLimiter.Tag = "";
            this.chkNLimiter.Text = "NLim";
            this.toolTip.SetToolTip(this.chkNLimiter, "Noise Limiter on/off");
            this.chkNLimiter.CheckedChanged += new System.EventHandler(this.chkNLimiter_CheckedChanged);
            // 
            // tbNLRatio
            // 
            this.tbNLRatio.Button = false;
            this.tbNLRatio.Checked = false;
            this.tbNLRatio.ColorFactor = 0.55F;
            this.tbNLRatio.ForeColor = System.Drawing.Color.Black;
            this.tbNLRatio.Location = new System.Drawing.Point(86, 33);
            this.tbNLRatio.Margin = new System.Windows.Forms.Padding(4);
            this.tbNLRatio.Maximum = 100;
            this.tbNLRatio.Minimum = 0;
            this.tbNLRatio.Name = "tbNLRatio";
            this.tbNLRatio.Size = new System.Drawing.Size(110, 16);
            this.tbNLRatio.TabIndex = 67;
            this.tbNLRatio.TickColor = System.Drawing.Color.Orange;
            this.tbNLRatio.Ticks = 0;
            this.tbNLRatio.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbNLRatio, "Agressiveness");
            this.tbNLRatio.Value = 50;
            this.tbNLRatio.ValueChanged += new System.EventHandler(this.tbNLRatio_ValueChanged);
            // 
            // tbNLTreshold
            // 
            this.tbNLTreshold.Button = false;
            this.tbNLTreshold.Checked = false;
            this.tbNLTreshold.ColorFactor = 0.55F;
            this.tbNLTreshold.ForeColor = System.Drawing.Color.Black;
            this.tbNLTreshold.Location = new System.Drawing.Point(86, 9);
            this.tbNLTreshold.Margin = new System.Windows.Forms.Padding(4);
            this.tbNLTreshold.Maximum = -90;
            this.tbNLTreshold.Minimum = -140;
            this.tbNLTreshold.Name = "tbNLTreshold";
            this.tbNLTreshold.Size = new System.Drawing.Size(110, 15);
            this.tbNLTreshold.TabIndex = 66;
            this.tbNLTreshold.TickColor = System.Drawing.Color.Orange;
            this.tbNLTreshold.Ticks = 0;
            this.tbNLTreshold.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.tbNLTreshold, "Limiting level (dBm)");
            this.tbNLTreshold.Value = -120;
            this.tbNLTreshold.ValueChanged += new System.EventHandler(this.tbNLTreshold_ValueChanged);
            // 
            // agcThresholdNumericUpDown
            // 
            this.agcThresholdNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agcThresholdNumericUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.agcThresholdNumericUpDown.Increment = 10;
            this.agcThresholdNumericUpDown.Location = new System.Drawing.Point(122, 25);
            this.agcThresholdNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.agcThresholdNumericUpDown.Maximum = ((long)(0));
            this.agcThresholdNumericUpDown.Minimum = ((long)(-160));
            this.agcThresholdNumericUpDown.Name = "agcThresholdNumericUpDown";
            this.agcThresholdNumericUpDown.Size = new System.Drawing.Size(75, 21);
            this.agcThresholdNumericUpDown.TabIndex = 62;
            this.agcThresholdNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.agcThresholdNumericUpDown, "AGC treshold level (dBm)");
            this.agcThresholdNumericUpDown.Value = ((long)(-50));
            this.agcThresholdNumericUpDown.ValueChanged += new System.EventHandler(this.agcThresholdNumericUpDown_ValueChanged);
            // 
            // agcDecayNumericUpDown
            // 
            this.agcDecayNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agcDecayNumericUpDown.Button = false;
            this.agcDecayNumericUpDown.Checked = false;
            this.agcDecayNumericUpDown.ColorFactor = 0.5F;
            this.agcDecayNumericUpDown.ForeColor = System.Drawing.Color.Black;
            this.agcDecayNumericUpDown.Location = new System.Drawing.Point(86, 79);
            this.agcDecayNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.agcDecayNumericUpDown.Maximum = 2000;
            this.agcDecayNumericUpDown.Minimum = 10;
            this.agcDecayNumericUpDown.Name = "agcDecayNumericUpDown";
            this.agcDecayNumericUpDown.Size = new System.Drawing.Size(110, 15);
            this.agcDecayNumericUpDown.TabIndex = 64;
            this.agcDecayNumericUpDown.Tag = "AGC decay";
            this.agcDecayNumericUpDown.TickColor = System.Drawing.Color.Silver;
            this.agcDecayNumericUpDown.Ticks = 0;
            this.agcDecayNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.agcDecayNumericUpDown, "AGC decay speed");
            this.agcDecayNumericUpDown.Value = 100;
            this.agcDecayNumericUpDown.ValueChanged += new System.EventHandler(this.agcDecayNumericUpDown_ValueChanged);
            // 
            // agcSlopeNumericUpDown
            // 
            this.agcSlopeNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agcSlopeNumericUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.agcSlopeNumericUpDown.Increment = 1;
            this.agcSlopeNumericUpDown.Location = new System.Drawing.Point(122, 52);
            this.agcSlopeNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.agcSlopeNumericUpDown.Maximum = ((long)(10));
            this.agcSlopeNumericUpDown.Minimum = ((long)(0));
            this.agcSlopeNumericUpDown.Name = "agcSlopeNumericUpDown";
            this.agcSlopeNumericUpDown.Size = new System.Drawing.Size(75, 21);
            this.agcSlopeNumericUpDown.TabIndex = 63;
            this.agcSlopeNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.agcSlopeNumericUpDown, "AGC slope");
            this.agcSlopeNumericUpDown.Value = ((long)(0));
            this.agcSlopeNumericUpDown.ValueChanged += new System.EventHandler(this.agcSlopeNumericUpDown_ValueChanged);
            this.agcSlopeNumericUpDown.Enter += new System.EventHandler(this.gUpDown_Enter);
            this.agcSlopeNumericUpDown.Leave += new System.EventHandler(this.gpUpDown_Leave);
            // 
            // agcUseHangCheckBox
            // 
            this.agcUseHangCheckBox.Arrow = 99;
            this.agcUseHangCheckBox.Checked = false;
            this.agcUseHangCheckBox.Edge = 0.15F;
            this.agcUseHangCheckBox.EndColor = System.Drawing.Color.White;
            this.agcUseHangCheckBox.EndFactor = 0.2F;
            this.agcUseHangCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agcUseHangCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.agcUseHangCheckBox.Location = new System.Drawing.Point(0, 27);
            this.agcUseHangCheckBox.Name = "agcUseHangCheckBox";
            this.agcUseHangCheckBox.NoBorder = false;
            this.agcUseHangCheckBox.NoLed = false;
            this.agcUseHangCheckBox.RadioButton = false;
            this.agcUseHangCheckBox.Radius = 6;
            this.agcUseHangCheckBox.RadiusB = 0;
            this.agcUseHangCheckBox.Size = new System.Drawing.Size(46, 24);
            this.agcUseHangCheckBox.StartColor = System.Drawing.Color.Black;
            this.agcUseHangCheckBox.StartFactor = 0.35F;
            this.agcUseHangCheckBox.TabIndex = 61;
            this.agcUseHangCheckBox.Text = "Hang";
            this.toolTip.SetToolTip(this.agcUseHangCheckBox, "AGC hang on/off");
            this.agcUseHangCheckBox.CheckedChanged += new System.EventHandler(this.agcUseHangCheckBox_CheckedChanged);
            // 
            // cmbAudio
            // 
            this.cmbAudio.BackColor = System.Drawing.Color.Transparent;
            this.cmbAudio.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbAudio.Items.Add("None");
            this.cmbAudio.Items.Add("Spect.");
            this.cmbAudio.Items.Add("A-gram");
            this.cmbAudio.Items.Add("Envelope");
            this.cmbAudio.Location = new System.Drawing.Point(126, 34);
            this.cmbAudio.Margin = new System.Windows.Forms.Padding(5);
            this.cmbAudio.Name = "cmbAudio";
            this.cmbAudio.SelectedIndex = -1;
            this.cmbAudio.Size = new System.Drawing.Size(71, 21);
            this.cmbAudio.TabIndex = 128;
            this.cmbAudio.Text = "xxxx";
            this.toolTip.SetToolTip(this.cmbAudio, "Select AF audio display");
            this.cmbAudio.ToolTip = this.toolTip;
            this.cmbAudio.SelectedIndexChanged += new System.EventHandler(this.cmbAudio_SelectedIndexChanged);
            // 
            // chkIF
            // 
            this.chkIF.Arrow = 0;
            this.chkIF.Checked = true;
            this.chkIF.Edge = 0.15F;
            this.chkIF.EndColor = System.Drawing.Color.White;
            this.chkIF.EndFactor = 0.2F;
            this.chkIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIF.ForeColor = System.Drawing.Color.Orange;
            this.chkIF.Location = new System.Drawing.Point(54, 39);
            this.chkIF.Name = "chkIF";
            this.chkIF.NoBorder = false;
            this.chkIF.NoLed = false;
            this.chkIF.RadioButton = false;
            this.chkIF.Radius = 4;
            this.chkIF.RadiusB = 0;
            this.chkIF.Size = new System.Drawing.Size(30, 20);
            this.chkIF.StartColor = System.Drawing.Color.Black;
            this.chkIF.StartFactor = 0.35F;
            this.chkIF.TabIndex = 70;
            this.toolTip.SetToolTip(this.chkIF, "Show IF/baseband spectrum");
            this.chkIF.CheckedChanged += new System.EventHandler(this.chkFFT_CheckedChanged);
            // 
            // chkWF
            // 
            this.chkWF.Arrow = 0;
            this.chkWF.Checked = true;
            this.chkWF.Edge = 0.15F;
            this.chkWF.EndColor = System.Drawing.Color.White;
            this.chkWF.EndFactor = 0.2F;
            this.chkWF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWF.ForeColor = System.Drawing.Color.Orange;
            this.chkWF.Location = new System.Drawing.Point(9, 39);
            this.chkWF.Name = "chkWF";
            this.chkWF.NoBorder = false;
            this.chkWF.NoLed = false;
            this.chkWF.RadioButton = false;
            this.chkWF.Radius = 4;
            this.chkWF.RadiusB = 0;
            this.chkWF.Size = new System.Drawing.Size(30, 20);
            this.chkWF.StartColor = System.Drawing.Color.Black;
            this.chkWF.StartFactor = 0.35F;
            this.chkWF.TabIndex = 69;
            this.toolTip.SetToolTip(this.chkWF, "Show RF waterfall");
            this.chkWF.CheckedChanged += new System.EventHandler(this.chkFFT_CheckedChanged);
            // 
            // dbmOffsetUpDown
            // 
            this.dbmOffsetUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.dbmOffsetUpDown.Increment = 6;
            this.dbmOffsetUpDown.Location = new System.Drawing.Point(53, 258);
            this.dbmOffsetUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.dbmOffsetUpDown.Maximum = ((long)(120));
            this.dbmOffsetUpDown.Minimum = ((long)(-120));
            this.dbmOffsetUpDown.Name = "dbmOffsetUpDown";
            this.dbmOffsetUpDown.Size = new System.Drawing.Size(62, 21);
            this.dbmOffsetUpDown.TabIndex = 95;
            this.dbmOffsetUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.dbmOffsetUpDown, "S-meter offset/correction");
            this.dbmOffsetUpDown.Value = ((long)(0));
            this.dbmOffsetUpDown.ValueChanged += new System.EventHandler(this.dbmOffsetUpDown_ValueChanged);
            // 
            // chkAutoSize
            // 
            this.chkAutoSize.Arrow = 99;
            this.chkAutoSize.Checked = false;
            this.chkAutoSize.Edge = 0.15F;
            this.chkAutoSize.EndColor = System.Drawing.Color.White;
            this.chkAutoSize.EndFactor = 0.2F;
            this.chkAutoSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoSize.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkAutoSize.Location = new System.Drawing.Point(152, 153);
            this.chkAutoSize.Name = "chkAutoSize";
            this.chkAutoSize.NoBorder = false;
            this.chkAutoSize.NoLed = false;
            this.chkAutoSize.RadioButton = false;
            this.chkAutoSize.Radius = 6;
            this.chkAutoSize.RadiusB = 0;
            this.chkAutoSize.Size = new System.Drawing.Size(46, 30);
            this.chkAutoSize.StartColor = System.Drawing.Color.Black;
            this.chkAutoSize.StartFactor = 0.35F;
            this.chkAutoSize.TabIndex = 110;
            this.chkAutoSize.Tag = "";
            this.chkAutoSize.Text = "Auto\\size";
            this.toolTip.SetToolTip(this.chkAutoSize, "Auto IF/AF spectrum resizing");
            this.chkAutoSize.CheckedChanged += new System.EventHandler(this.chkAutoSize_CheckedChanged);
            // 
            // frequencyShiftNumericUpDown
            // 
            this.frequencyShiftNumericUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.frequencyShiftNumericUpDown.Increment = 1000;
            this.frequencyShiftNumericUpDown.Location = new System.Drawing.Point(55, 68);
            this.frequencyShiftNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.frequencyShiftNumericUpDown.Maximum = ((long)(100000000000000));
            this.frequencyShiftNumericUpDown.Minimum = ((long)(-100000000000000));
            this.frequencyShiftNumericUpDown.Name = "frequencyShiftNumericUpDown";
            this.frequencyShiftNumericUpDown.Size = new System.Drawing.Size(124, 21);
            this.frequencyShiftNumericUpDown.TabIndex = 72;
            this.frequencyShiftNumericUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.frequencyShiftNumericUpDown, "Frequency shift for up/down convertor");
            this.frequencyShiftNumericUpDown.Value = ((long)(10));
            this.frequencyShiftNumericUpDown.ValueChanged += new System.EventHandler(this.frequencyShiftNumericUpDown_ValueChanged);
            this.frequencyShiftNumericUpDown.Enter += new System.EventHandler(this.gUpDown_Enter);
            this.frequencyShiftNumericUpDown.Leave += new System.EventHandler(this.gpUpDown_Leave);
            // 
            // markPeaksCheckBox
            // 
            this.markPeaksCheckBox.Arrow = 99;
            this.markPeaksCheckBox.Checked = false;
            this.markPeaksCheckBox.Edge = 0.15F;
            this.markPeaksCheckBox.EndColor = System.Drawing.Color.White;
            this.markPeaksCheckBox.EndFactor = 0.2F;
            this.markPeaksCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.markPeaksCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.markPeaksCheckBox.Location = new System.Drawing.Point(153, 96);
            this.markPeaksCheckBox.Name = "markPeaksCheckBox";
            this.markPeaksCheckBox.NoBorder = false;
            this.markPeaksCheckBox.NoLed = false;
            this.markPeaksCheckBox.RadioButton = false;
            this.markPeaksCheckBox.Radius = 6;
            this.markPeaksCheckBox.RadiusB = 0;
            this.markPeaksCheckBox.Size = new System.Drawing.Size(46, 24);
            this.markPeaksCheckBox.StartColor = System.Drawing.Color.Black;
            this.markPeaksCheckBox.StartFactor = 0.35F;
            this.markPeaksCheckBox.TabIndex = 17;
            this.markPeaksCheckBox.Text = "Max";
            this.toolTip.SetToolTip(this.markPeaksCheckBox, "Perform max hold");
            this.markPeaksCheckBox.CheckedChanged += new System.EventHandler(this.markPeaksCheckBox_CheckedChanged);
            // 
            // frequencyShiftCheckBox
            // 
            this.frequencyShiftCheckBox.Arrow = 99;
            this.frequencyShiftCheckBox.Checked = false;
            this.frequencyShiftCheckBox.Edge = 0.15F;
            this.frequencyShiftCheckBox.EndColor = System.Drawing.Color.White;
            this.frequencyShiftCheckBox.EndFactor = 0.2F;
            this.frequencyShiftCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequencyShiftCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.frequencyShiftCheckBox.Location = new System.Drawing.Point(-1, 66);
            this.frequencyShiftCheckBox.Name = "frequencyShiftCheckBox";
            this.frequencyShiftCheckBox.NoBorder = false;
            this.frequencyShiftCheckBox.NoLed = false;
            this.frequencyShiftCheckBox.RadioButton = false;
            this.frequencyShiftCheckBox.Radius = 6;
            this.frequencyShiftCheckBox.RadiusB = 0;
            this.frequencyShiftCheckBox.Size = new System.Drawing.Size(45, 24);
            this.frequencyShiftCheckBox.StartColor = System.Drawing.Color.Black;
            this.frequencyShiftCheckBox.StartFactor = 0.35F;
            this.frequencyShiftCheckBox.TabIndex = 71;
            this.frequencyShiftCheckBox.Tag = "";
            this.frequencyShiftCheckBox.Text = "Shift";
            this.toolTip.SetToolTip(this.frequencyShiftCheckBox, "Frequency shift on/off");
            this.frequencyShiftCheckBox.CheckedChanged += new System.EventHandler(this.frequencyShiftCheckBox_CheckStateChanged);
            // 
            // fftResolutionComboBox
            // 
            this.fftResolutionComboBox.BackColor = System.Drawing.Color.Transparent;
            this.fftResolutionComboBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.fftResolutionComboBox.Items.Add("512");
            this.fftResolutionComboBox.Items.Add("1024");
            this.fftResolutionComboBox.Items.Add("2048");
            this.fftResolutionComboBox.Items.Add("4096");
            this.fftResolutionComboBox.Items.Add("8192");
            this.fftResolutionComboBox.Items.Add("16384");
            this.fftResolutionComboBox.Items.Add("32768");
            this.fftResolutionComboBox.Items.Add("65536");
            this.fftResolutionComboBox.Items.Add("131072");
            this.fftResolutionComboBox.Items.Add("262144");
            this.fftResolutionComboBox.Items.Add("524288");
            this.fftResolutionComboBox.Items.Add("1048576");
            this.fftResolutionComboBox.Items.Add("2097152");
            this.fftResolutionComboBox.Items.Add("4194304");
            this.fftResolutionComboBox.Location = new System.Drawing.Point(53, 198);
            this.fftResolutionComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.fftResolutionComboBox.Name = "fftResolutionComboBox";
            this.fftResolutionComboBox.SelectedIndex = -1;
            this.fftResolutionComboBox.Size = new System.Drawing.Size(92, 21);
            this.fftResolutionComboBox.TabIndex = 70;
            this.fftResolutionComboBox.Text = "xxxx";
            this.toolTip.SetToolTip(this.fftResolutionComboBox, "FFT resolution RF spectrum");
            this.fftResolutionComboBox.ToolTip = this.toolTip;
            this.fftResolutionComboBox.SelectedIndexChanged += new System.EventHandler(this.fftResolutionComboBox_SelectedIndexChanged);
            // 
            // fftAverageUpDown
            // 
            this.fftAverageUpDown.ForeColor = System.Drawing.Color.RoyalBlue;
            this.fftAverageUpDown.Increment = 1;
            this.fftAverageUpDown.Location = new System.Drawing.Point(53, 229);
            this.fftAverageUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.fftAverageUpDown.Maximum = ((long)(9));
            this.fftAverageUpDown.Minimum = ((long)(0));
            this.fftAverageUpDown.Name = "fftAverageUpDown";
            this.fftAverageUpDown.Size = new System.Drawing.Size(62, 21);
            this.fftAverageUpDown.TabIndex = 97;
            this.fftAverageUpDown.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.fftAverageUpDown, "FFT/spectrum  avaraging");
            this.fftAverageUpDown.Value = ((long)(0));
            this.fftAverageUpDown.ValueChanged += new System.EventHandler(this.fftAverageUpDown_ValueChanged);
            // 
            // useTimestampsCheckBox
            // 
            this.useTimestampsCheckBox.Arrow = 99;
            this.useTimestampsCheckBox.Checked = false;
            this.useTimestampsCheckBox.Edge = 0.15F;
            this.useTimestampsCheckBox.EndColor = System.Drawing.Color.White;
            this.useTimestampsCheckBox.EndFactor = 0.2F;
            this.useTimestampsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useTimestampsCheckBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.useTimestampsCheckBox.Location = new System.Drawing.Point(152, 124);
            this.useTimestampsCheckBox.Name = "useTimestampsCheckBox";
            this.useTimestampsCheckBox.NoBorder = false;
            this.useTimestampsCheckBox.NoLed = false;
            this.useTimestampsCheckBox.RadioButton = false;
            this.useTimestampsCheckBox.Radius = 6;
            this.useTimestampsCheckBox.RadiusB = 0;
            this.useTimestampsCheckBox.Size = new System.Drawing.Size(46, 24);
            this.useTimestampsCheckBox.StartColor = System.Drawing.Color.Black;
            this.useTimestampsCheckBox.StartFactor = 0.35F;
            this.useTimestampsCheckBox.TabIndex = 74;
            this.useTimestampsCheckBox.Text = "Time";
            this.toolTip.SetToolTip(this.useTimestampsCheckBox, "Show time markers");
            this.useTimestampsCheckBox.CheckedChanged += new System.EventHandler(this.useTimestampCheckBox_CheckedChanged);
            // 
            // chkIndepSideband
            // 
            this.chkIndepSideband.Arrow = 99;
            this.chkIndepSideband.Checked = false;
            this.chkIndepSideband.Edge = 0.15F;
            this.chkIndepSideband.EndColor = System.Drawing.Color.White;
            this.chkIndepSideband.EndFactor = 0.2F;
            this.chkIndepSideband.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIndepSideband.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkIndepSideband.Location = new System.Drawing.Point(152, 189);
            this.chkIndepSideband.Name = "chkIndepSideband";
            this.chkIndepSideband.NoBorder = false;
            this.chkIndepSideband.NoLed = false;
            this.chkIndepSideband.RadioButton = false;
            this.chkIndepSideband.Radius = 6;
            this.chkIndepSideband.RadiusB = 0;
            this.chkIndepSideband.Size = new System.Drawing.Size(46, 24);
            this.chkIndepSideband.StartColor = System.Drawing.Color.Black;
            this.chkIndepSideband.StartFactor = 0.35F;
            this.chkIndepSideband.TabIndex = 75;
            this.chkIndepSideband.Tag = "";
            this.chkIndepSideband.Text = "Indep";
            this.toolTip.SetToolTip(this.chkIndepSideband, "move filter sidebands independently");
            this.chkIndepSideband.CheckedChanged += new System.EventHandler(this.chkIndepSideband_Changed);
            // 
            // gradientButtonSA
            // 
            this.gradientButtonSA.Arrow = 0;
            this.gradientButtonSA.Checked = false;
            this.gradientButtonSA.Edge = 0.15F;
            this.gradientButtonSA.EndColor = System.Drawing.Color.White;
            this.gradientButtonSA.EndFactor = 0.2F;
            this.gradientButtonSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButtonSA.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.gradientButtonSA.Location = new System.Drawing.Point(152, 258);
            this.gradientButtonSA.Name = "gradientButtonSA";
            this.gradientButtonSA.NoBorder = false;
            this.gradientButtonSA.NoLed = true;
            this.gradientButtonSA.RadioButton = false;
            this.gradientButtonSA.Radius = 6;
            this.gradientButtonSA.RadiusB = 0;
            this.gradientButtonSA.Size = new System.Drawing.Size(46, 24);
            this.gradientButtonSA.StartColor = System.Drawing.Color.Black;
            this.gradientButtonSA.StartFactor = 0.35F;
            this.gradientButtonSA.TabIndex = 80;
            this.gradientButtonSA.Tag = "";
            this.gradientButtonSA.Text = "Colors";
            this.toolTip.SetToolTip(this.gradientButtonSA, "Change color settings");
            this.gradientButtonSA.Click += new System.EventHandler(this.gradientButtonSA_Click);
            // 
            // remDcSlider
            // 
            this.remDcSlider.Button = false;
            this.remDcSlider.Checked = false;
            this.remDcSlider.ColorFactor = 0.55F;
            this.remDcSlider.ForeColor = System.Drawing.Color.Black;
            this.remDcSlider.Location = new System.Drawing.Point(53, 288);
            this.remDcSlider.Margin = new System.Windows.Forms.Padding(4);
            this.remDcSlider.Maximum = 200;
            this.remDcSlider.Minimum = 0;
            this.remDcSlider.Name = "remDcSlider";
            this.remDcSlider.Size = new System.Drawing.Size(92, 16);
            this.remDcSlider.TabIndex = 130;
            this.remDcSlider.TickColor = System.Drawing.Color.Silver;
            this.remDcSlider.Ticks = 5;
            this.remDcSlider.ToolTip = this.toolTip;
            this.toolTip.SetToolTip(this.remDcSlider, "Adjust for removing center spike");
            this.remDcSlider.Value = 100;
            this.remDcSlider.ValueChanged += new System.EventHandler(this.remDcSlider_ValueChanged);
            // 
            // label50
            // 
            this.label50.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label50.AutoSize = true;
            this.label50.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label50.Location = new System.Drawing.Point(1, 3);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(23, 13);
            this.label50.TabIndex = 114;
            this.label50.Text = "Sp.";
            // 
            // label49
            // 
            this.label49.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label49.AutoSize = true;
            this.label49.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label49.Location = new System.Drawing.Point(23, 3);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(24, 13);
            this.label49.TabIndex = 113;
            this.label49.Text = "Int.";
            // 
            // label44
            // 
            this.label44.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label44.AutoSize = true;
            this.label44.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label44.Location = new System.Drawing.Point(43, 3);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(31, 13);
            this.label44.TabIndex = 112;
            this.label44.Text = "Con.";
            // 
            // audiogram
            // 
            this.audiogram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audiogram.Attack = 0.9D;
            this.audiogram.AutoSize = true;
            this.audiogram.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.audiogram.BandType = SDRSharp.PanView.BandType.Center;
            this.audiogram.CenterFixed = false;
            this.audiogram.CenterFrequency = ((long)(0));
            this.audiogram.CenterStep = 0;
            this.audiogram.DataType = SDRSharp.Radio.DataType.AF;
            this.audiogram.Decay = 0.5D;
            this.audiogram.DisplayFrequency = ((long)(0));
            this.audiogram.FilterBandwidth = 10000;
            this.audiogram.FilterOffset = 0;
            this.audiogram.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audiogram.Frequency = ((long)(2500));
            this.audiogram.Horizontal = true;
            this.audiogram.IndepSideband = false;
            this.audiogram.Location = new System.Drawing.Point(0, 0);
            this.audiogram.MaxPower = 0F;
            this.audiogram.MinPower = -130F;
            this.audiogram.Name = "audiogram";
            this.audiogram.RecordStart = new System.DateTime(((long)(0)));
            this.audiogram.ShowDbm = 0;
            this.audiogram.Size = new System.Drawing.Size(402, 150);
            this.audiogram.SpectrumWidth = 5001;
            this.audiogram.StepSize = 0;
            this.audiogram.TabIndex = 108;
            this.audiogram.TimestampInterval = 100;
            this.audiogram.UseSmoothing = true;
            this.audiogram.UseSnap = false;
            this.audiogram.UseTimestamps = 0;
            this.audiogram.WaveStart = new System.DateTime(((long)(0)));
            this.audiogram.Zoom = 1F;
            this.audiogram.FrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_FrequencyChanged);
            // 
            // afAnalyzer
            // 
            this.afAnalyzer.Attack = 0.9D;
            this.afAnalyzer.BackgroundColor = System.Drawing.Color.Empty;
            this.afAnalyzer.BandType = SDRSharp.PanView.BandType.Center;
            this.afAnalyzer.CenterFixed = true;
            this.afAnalyzer.CenterFrequency = ((long)(0));
            this.afAnalyzer.CenterStep = 10000;
            this.afAnalyzer.DataType = SDRSharp.Radio.DataType.AF;
            this.afAnalyzer.Decay = 0.3D;
            this.afAnalyzer.DisplayFrequency = ((long)(0));
            this.afAnalyzer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.afAnalyzer.FilterBandwidth = 10000;
            this.afAnalyzer.FilterOffset = 100;
            this.afAnalyzer.Frequency = ((long)(0));
            this.afAnalyzer.IndepSideband = false;
            this.afAnalyzer.Location = new System.Drawing.Point(0, 0);
            this.afAnalyzer.MarkPeaks = false;
            this.afAnalyzer.MaxPower = 0F;
            this.afAnalyzer.MinPower = -130F;
            this.afAnalyzer.Name = "afAnalyzer";
            this.afAnalyzer.ShowDbm = 0;
            this.afAnalyzer.Size = new System.Drawing.Size(477, 150);
            this.afAnalyzer.SpectrumColor = System.Drawing.Color.DarkGray;
            this.afAnalyzer.SpectrumFill = 0;
            this.afAnalyzer.SpectrumWidth = 5001;
            this.afAnalyzer.StationList = "";
            this.afAnalyzer.StatusText = "";
            this.afAnalyzer.StepSize = 1000;
            this.afAnalyzer.TabIndex = 126;
            this.afAnalyzer.UseSmoothing = true;
            this.afAnalyzer.UseSnap = false;
            this.afAnalyzer.Zoom = 12.8F;
            this.afAnalyzer.FrequencyChanged += new SDRSharp.PanView.ManualFrequencyChange(this.panview_FrequencyChanged);
            this.afAnalyzer.BandwidthChanged += new SDRSharp.PanView.ManualBandwidthChange(this.panview_BandwidthChanged);
            this.afAnalyzer.AutoZoomed += new System.EventHandler(this.panview_AutoZoomed);
            // 
            // wideScope
            // 
            this.wideScope.AudioAvg = 0;
            this.wideScope.AudioRel = 6;
            this.wideScope.BackgoundColor = System.Drawing.Color.Empty;
            this.wideScope.CarrierAvg = 0;
            this.wideScope.HblockDC = false;
            this.wideScope.Hchannel = SDRSharp.Radio.DemodType.Empty;
            this.wideScope.Hdiv = 1F;
            this.wideScope.Hinvert = false;
            this.wideScope.Hshift = 0F;
            this.wideScope.Location = new System.Drawing.Point(0, 0);
            this.wideScope.Name = "wideScope";
            this.wideScope.PeakDelay = 0;
            this.wideScope.PeakRel = 0;
            this.wideScope.ShowBars = false;
            this.wideScope.ShowLines = false;
            this.wideScope.Size = new System.Drawing.Size(316, 130);
            this.wideScope.SpectrumFill = 0;
            this.wideScope.StatusText = "";
            this.wideScope.TabIndex = 123;
            this.wideScope.Tdiv = 0F;
            this.wideScope.TraceColor = System.Drawing.Color.DarkGray;
            this.wideScope.TrigLevel = 0F;
            this.wideScope.VblockDC = false;
            this.wideScope.Vchannel = SDRSharp.Radio.DemodType.Empty;
            this.wideScope.Vdiv = 1F;
            this.wideScope.Vinvert = false;
            this.wideScope.Vshift = 0F;
            this.wideScope.XYmode = false;
            // 
            // afWaterfall
            // 
            this.afWaterfall.Attack = 0.9D;
            this.afWaterfall.AutoSize = true;
            this.afWaterfall.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.afWaterfall.BandType = SDRSharp.PanView.BandType.Center;
            this.afWaterfall.CenterFixed = false;
            this.afWaterfall.CenterFrequency = ((long)(0));
            this.afWaterfall.CenterStep = 0;
            this.afWaterfall.DataType = SDRSharp.Radio.DataType.AF;
            this.afWaterfall.Decay = 0.5D;
            this.afWaterfall.DisplayFrequency = ((long)(0));
            this.afWaterfall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.afWaterfall.FilterBandwidth = 10000;
            this.afWaterfall.FilterOffset = 0;
            this.afWaterfall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.afWaterfall.Frequency = ((long)(0));
            this.afWaterfall.Horizontal = false;
            this.afWaterfall.IndepSideband = false;
            this.afWaterfall.Location = new System.Drawing.Point(0, 0);
            this.afWaterfall.MaxPower = 0F;
            this.afWaterfall.MinPower = -130F;
            this.afWaterfall.Name = "afWaterfall";
            this.afWaterfall.RecordStart = new System.DateTime(((long)(0)));
            this.afWaterfall.ShowDbm = 0;
            this.afWaterfall.Size = new System.Drawing.Size(477, 115);
            this.afWaterfall.SpectrumWidth = 96001;
            this.afWaterfall.StepSize = 0;
            this.afWaterfall.TabIndex = 126;
            this.afWaterfall.TimestampInterval = 150;
            this.afWaterfall.UseSmoothing = true;
            this.afWaterfall.UseSnap = false;
            this.afWaterfall.UseTimestamps = 0;
            this.afWaterfall.WaveStart = new System.DateTime(((long)(0)));
            this.afWaterfall.Zoom = 1F;
            this.afWaterfall.BandwidthChanged += new SDRSharp.PanView.ManualBandwidthChange(this.panview_BandwidthChanged);
            this.afWaterfall.AutoZoomed += new System.EventHandler(this.panview_AutoZoomed);
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.Color.Orange;
            this.label31.Location = new System.Drawing.Point(989, 127);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(39, 13);
            this.label31.TabIndex = 49;
            this.label31.Text = "Intens";
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label29.Location = new System.Drawing.Point(1254, 131);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(34, 13);
            this.label29.TabIndex = 39;
            this.label29.Text = "Floor";
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label30.AutoSize = true;
            this.label30.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label30.Location = new System.Drawing.Point(1254, 42);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(25, 13);
            this.label30.TabIndex = 38;
            this.label30.Text = "Top";
            // 
            // labSpeed
            // 
            this.labSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labSpeed.AutoSize = true;
            this.labSpeed.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labSpeed.Location = new System.Drawing.Point(1254, 459);
            this.labSpeed.Name = "labSpeed";
            this.labSpeed.Size = new System.Drawing.Size(23, 13);
            this.labSpeed.TabIndex = 52;
            this.labSpeed.Text = "-99";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label17.Location = new System.Drawing.Point(1249, 370);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "Speed";
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label32.Location = new System.Drawing.Point(1253, 247);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(36, 13);
            this.label32.TabIndex = 48;
            this.label32.Text = "Contr";
            // 
            // labInt
            // 
            this.labInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labInt.AutoSize = true;
            this.labInt.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labInt.Location = new System.Drawing.Point(1254, 588);
            this.labInt.Name = "labInt";
            this.labInt.Size = new System.Drawing.Size(23, 13);
            this.labInt.TabIndex = 51;
            this.labInt.Text = "-99";
            this.labInt.Visible = false;
            // 
            // labCon
            // 
            this.labCon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labCon.AutoSize = true;
            this.labCon.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labCon.Location = new System.Drawing.Point(1254, 575);
            this.labCon.Name = "labCon";
            this.labCon.Size = new System.Drawing.Size(23, 13);
            this.labCon.TabIndex = 50;
            this.labCon.Text = "-99";
            this.labCon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labCon.Visible = false;
            // 
            // MnuSA
            // 
            this.MnuSA.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSetNotch,
            this.MnuClearNotch,
            this.toolStripSeparator4,
            this.mnuVfoA,
            this.mnuVfoB,
            this.mnuVfoC,
            this.toolStripSeparator2,
            this.mnuShowWaterfall,
            this.mnuShowBaseband,
            this.toolStripSeparator3,
            this.mnuShowAudio,
            this.mnuShowAudiogram,
            this.mnuShowEnvelope,
            this.toolStripSeparator1,
            this.mnuStationList,
            this.mnuAutoScale,
            this.setColoursToolStripMenuItem});
            this.MnuSA.Name = "MnuSA";
            this.MnuSA.Size = new System.Drawing.Size(171, 314);
            this.MnuSA.Opening += new System.ComponentModel.CancelEventHandler(this.MnuSA_Opening);
            // 
            // mnuSetNotch
            // 
            this.mnuSetNotch.Name = "mnuSetNotch";
            this.mnuSetNotch.Size = new System.Drawing.Size(170, 22);
            this.mnuSetNotch.Text = "Set notch";
            this.mnuSetNotch.Click += new System.EventHandler(this.mnuSetNotch_Click);
            // 
            // MnuClearNotch
            // 
            this.MnuClearNotch.Name = "MnuClearNotch";
            this.MnuClearNotch.Size = new System.Drawing.Size(170, 22);
            this.MnuClearNotch.Text = "Clear notch";
            this.MnuClearNotch.Click += new System.EventHandler(this.MnuClearNotch_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(167, 6);
            // 
            // mnuVfoA
            // 
            this.mnuVfoA.Name = "mnuVfoA";
            this.mnuVfoA.Size = new System.Drawing.Size(170, 22);
            this.mnuVfoA.Text = "Vfo A";
            this.mnuVfoA.Click += new System.EventHandler(this.mnuVfo_Click);
            // 
            // mnuVfoB
            // 
            this.mnuVfoB.Name = "mnuVfoB";
            this.mnuVfoB.Size = new System.Drawing.Size(170, 22);
            this.mnuVfoB.Text = "Vfo B";
            this.mnuVfoB.Click += new System.EventHandler(this.mnuVfo_Click);
            // 
            // mnuVfoC
            // 
            this.mnuVfoC.Name = "mnuVfoC";
            this.mnuVfoC.Size = new System.Drawing.Size(170, 22);
            this.mnuVfoC.Text = "Vfo C";
            this.mnuVfoC.Click += new System.EventHandler(this.mnuVfo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            // 
            // mnuShowWaterfall
            // 
            this.mnuShowWaterfall.Name = "mnuShowWaterfall";
            this.mnuShowWaterfall.Size = new System.Drawing.Size(170, 22);
            this.mnuShowWaterfall.Text = "Show Waterfall";
            this.mnuShowWaterfall.Click += new System.EventHandler(this.mnuShowWaterfall_Click);
            // 
            // mnuShowBaseband
            // 
            this.mnuShowBaseband.Name = "mnuShowBaseband";
            this.mnuShowBaseband.Size = new System.Drawing.Size(170, 22);
            this.mnuShowBaseband.Text = "Show IF baseband";
            this.mnuShowBaseband.Click += new System.EventHandler(this.mnuShowBaseband_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(167, 6);
            // 
            // mnuShowAudio
            // 
            this.mnuShowAudio.Name = "mnuShowAudio";
            this.mnuShowAudio.Size = new System.Drawing.Size(170, 22);
            this.mnuShowAudio.Text = "Show AF audio";
            this.mnuShowAudio.Click += new System.EventHandler(this.mnuShowAudio_Click);
            // 
            // mnuShowAudiogram
            // 
            this.mnuShowAudiogram.Name = "mnuShowAudiogram";
            this.mnuShowAudiogram.Size = new System.Drawing.Size(170, 22);
            this.mnuShowAudiogram.Text = "Show audiogram";
            this.mnuShowAudiogram.Click += new System.EventHandler(this.mnuShowAudiogram_Click);
            // 
            // mnuShowEnvelope
            // 
            this.mnuShowEnvelope.Name = "mnuShowEnvelope";
            this.mnuShowEnvelope.Size = new System.Drawing.Size(170, 22);
            this.mnuShowEnvelope.Text = "Show AM envlope";
            this.mnuShowEnvelope.Click += new System.EventHandler(this.mnuShowEnvelope_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // mnuStationList
            // 
            this.mnuStationList.Name = "mnuStationList";
            this.mnuStationList.Size = new System.Drawing.Size(170, 22);
            this.mnuStationList.Text = "Show StationList";
            this.mnuStationList.Click += new System.EventHandler(this.mnuStationList_Click);
            // 
            // mnuAutoScale
            // 
            this.mnuAutoScale.Name = "mnuAutoScale";
            this.mnuAutoScale.Size = new System.Drawing.Size(170, 22);
            this.mnuAutoScale.Text = "Do autoscaling";
            this.mnuAutoScale.Click += new System.EventHandler(this.mnuAutoScale_Click);
            // 
            // setColoursToolStripMenuItem
            // 
            this.setColoursToolStripMenuItem.Name = "setColoursToolStripMenuItem";
            this.setColoursToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.setColoursToolStripMenuItem.Text = "Set colors";
            this.setColoursToolStripMenuItem.Click += new System.EventHandler(this.mnuSetColors_Click);
            // 
            // iqTimer
            // 
            this.iqTimer.Enabled = true;
            this.iqTimer.Interval = 500;
            this.iqTimer.Tick += new System.EventHandler(this.iqTimer_Tick);
            // 
            // labZoom
            // 
            this.labZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labZoom.AutoSize = true;
            this.labZoom.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labZoom.Location = new System.Drawing.Point(941, 15);
            this.labZoom.Name = "labZoom";
            this.labZoom.Size = new System.Drawing.Size(36, 13);
            this.labZoom.TabIndex = 19;
            this.labZoom.Text = "Zoom";
            // 
            // controlPanel
            // 
            this.controlPanel.AutoSize = true;
            this.controlPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.controlPanel.Controls.Add(this.AdvancedCollapsiblePanel);
            this.controlPanel.Controls.Add(this.radioCollapsiblePanel);
            this.controlPanel.Controls.Add(this.displayCollapsiblePanel);
            this.controlPanel.Controls.Add(this.agcCollapsiblePanel);
            this.controlPanel.Controls.Add(this.audioCollapsiblePanel);
            this.controlPanel.Controls.Add(this.scopeCollapsiblePanel);
            this.controlPanel.Location = new System.Drawing.Point(0, 1);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(202, 1695);
            this.controlPanel.TabIndex = 25;
            this.controlPanel.Resize += new System.EventHandler(this.controlPanel_Resize);
            // 
            // AdvancedCollapsiblePanel
            // 
            this.AdvancedCollapsiblePanel.Controls.Add(this.barMeter);
            this.AdvancedCollapsiblePanel.Controls.Add(this.fastFftCheckBox);
            this.AdvancedCollapsiblePanel.Controls.Add(this.labCPU);
            this.AdvancedCollapsiblePanel.Controls.Add(this.labProcessTmr);
            this.AdvancedCollapsiblePanel.Controls.Add(this.labPerformTmr);
            this.AdvancedCollapsiblePanel.Controls.Add(this.labFftTmr);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label58);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label57);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label52);
            this.AdvancedCollapsiblePanel.Controls.Add(this.bftResolutionComboBox);
            this.AdvancedCollapsiblePanel.Controls.Add(this.chkBaseBand);
            this.AdvancedCollapsiblePanel.Controls.Add(this.centerFreqNumericUpDown);
            this.AdvancedCollapsiblePanel.Controls.Add(this.latencyNumericUpDown);
            this.AdvancedCollapsiblePanel.Controls.Add(this.fftWindowComboBox);
            this.AdvancedCollapsiblePanel.Controls.Add(this.cmbCenterStep);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label5);
            this.AdvancedCollapsiblePanel.Controls.Add(this.chkFastConv);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label28);
            this.AdvancedCollapsiblePanel.Controls.Add(this.chk1);
            this.AdvancedCollapsiblePanel.Controls.Add(this.filterOrderNumericUpDown);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label1);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label16);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label6);
            this.AdvancedCollapsiblePanel.Controls.Add(this.filterTypeComboBox);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label8);
            this.AdvancedCollapsiblePanel.Controls.Add(this.filterBandwidthNumericUpDown);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label55);
            this.AdvancedCollapsiblePanel.Controls.Add(this.label59);
            this.AdvancedCollapsiblePanel.ExpandedHeight = 240;
            this.AdvancedCollapsiblePanel.ForeColor = System.Drawing.Color.Black;
            this.AdvancedCollapsiblePanel.Location = new System.Drawing.Point(0, 1451);
            this.AdvancedCollapsiblePanel.Margin = new System.Windows.Forms.Padding(4);
            this.AdvancedCollapsiblePanel.Name = "AdvancedCollapsiblePanel";
            this.AdvancedCollapsiblePanel.NextPanel = null;
            this.AdvancedCollapsiblePanel.PanelTitle = "Advanced";
            this.AdvancedCollapsiblePanel.Size = new System.Drawing.Size(198, 240);
            this.AdvancedCollapsiblePanel.TabIndex = 84;
            // 
            // barMeter
            // 
            this.barMeter.Location = new System.Drawing.Point(4, 204);
            this.barMeter.Name = "barMeter";
            this.barMeter.Size = new System.Drawing.Size(155, 27);
            this.barMeter.TabIndex = 137;
            // 
            // labCPU
            // 
            this.labCPU.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labCPU.Location = new System.Drawing.Point(158, 215);
            this.labCPU.Margin = new System.Windows.Forms.Padding(4);
            this.labCPU.Name = "labCPU";
            this.labCPU.Size = new System.Drawing.Size(39, 20);
            this.labCPU.TabIndex = 135;
            this.labCPU.Text = null;
            // 
            // labProcessTmr
            // 
            this.labProcessTmr.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labProcessTmr.Location = new System.Drawing.Point(152, 181);
            this.labProcessTmr.Name = "labProcessTmr";
            this.labProcessTmr.Size = new System.Drawing.Size(25, 15);
            this.labProcessTmr.TabIndex = 134;
            this.labProcessTmr.Text = "999";
            // 
            // labPerformTmr
            // 
            this.labPerformTmr.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labPerformTmr.Location = new System.Drawing.Point(82, 180);
            this.labPerformTmr.Name = "labPerformTmr";
            this.labPerformTmr.Size = new System.Drawing.Size(26, 14);
            this.labPerformTmr.TabIndex = 133;
            this.labPerformTmr.Text = "999";
            // 
            // labFftTmr
            // 
            this.labFftTmr.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labFftTmr.Location = new System.Drawing.Point(12, 181);
            this.labFftTmr.Name = "labFftTmr";
            this.labFftTmr.Size = new System.Drawing.Size(26, 15);
            this.labFftTmr.TabIndex = 132;
            this.labFftTmr.Text = "999";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label58.Location = new System.Drawing.Point(143, 167);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(50, 13);
            this.label58.TabIndex = 109;
            this.label58.Text = "Proc-FFT";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label57.Location = new System.Drawing.Point(69, 164);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(48, 13);
            this.label57.TabIndex = 108;
            this.label57.Text = "Perf-tmr";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label52.Location = new System.Drawing.Point(0, 164);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(45, 13);
            this.label52.TabIndex = 107;
            this.label52.Text = "FFT-tmr";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // centerFreqNumericUpDown
            // 
            this.centerFreqNumericUpDown.Enabled = false;
            this.centerFreqNumericUpDown.ForeColor = System.Drawing.Color.Yellow;
            this.centerFreqNumericUpDown.Increment = 10;
            this.centerFreqNumericUpDown.Location = new System.Drawing.Point(57, 265);
            this.centerFreqNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.centerFreqNumericUpDown.Maximum = ((long)(9999999999));
            this.centerFreqNumericUpDown.Minimum = ((long)(0));
            this.centerFreqNumericUpDown.Name = "centerFreqNumericUpDown";
            this.centerFreqNumericUpDown.Size = new System.Drawing.Size(136, 20);
            this.centerFreqNumericUpDown.TabIndex = 101;
            this.centerFreqNumericUpDown.ToolTip = null;
            this.centerFreqNumericUpDown.Value = ((long)(6000));
            this.centerFreqNumericUpDown.Visible = false;
            this.centerFreqNumericUpDown.ValueChanged += new System.EventHandler(this.centerFreqNumericUpDown_ValueChanged);
            // 
            // cmbCenterStep
            // 
            this.cmbCenterStep.BackColor = System.Drawing.Color.Transparent;
            this.cmbCenterStep.ForeColor = System.Drawing.Color.Yellow;
            this.cmbCenterStep.Items.Add("1 kHz");
            this.cmbCenterStep.Items.Add("5 kHz");
            this.cmbCenterStep.Items.Add("10 kHz");
            this.cmbCenterStep.Items.Add("25 kHz");
            this.cmbCenterStep.Items.Add("50 kHz");
            this.cmbCenterStep.Items.Add("100 kHz");
            this.cmbCenterStep.Items.Add("200 kHz");
            this.cmbCenterStep.Items.Add("250 kHz");
            this.cmbCenterStep.Items.Add("500 kHz");
            this.cmbCenterStep.Items.Add("1 Mhz");
            this.cmbCenterStep.Items.Add("2 Mhz");
            this.cmbCenterStep.Location = new System.Drawing.Point(57, 241);
            this.cmbCenterStep.Margin = new System.Windows.Forms.Padding(5);
            this.cmbCenterStep.Name = "cmbCenterStep";
            this.cmbCenterStep.SelectedIndex = -1;
            this.cmbCenterStep.Size = new System.Drawing.Size(83, 20);
            this.cmbCenterStep.TabIndex = 12;
            this.cmbCenterStep.Text = "xxxx";
            this.cmbCenterStep.ToolTip = null;
            this.cmbCenterStep.Visible = false;
            this.cmbCenterStep.SelectedIndexChanged += new System.EventHandler(this.cmbCenterStep_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label5.Location = new System.Drawing.Point(0, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 39);
            this.label5.TabIndex = 20;
            this.label5.Text = "Vfo filter order";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.Color.Orange;
            this.label28.Location = new System.Drawing.Point(-1, 242);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(57, 13);
            this.label28.TabIndex = 37;
            this.label28.Text = "Cent-step";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label28.Visible = false;
            // 
            // chk1
            // 
            this.chk1.Arrow = 99;
            this.chk1.Checked = false;
            this.chk1.Edge = 0.15F;
            this.chk1.EndColor = System.Drawing.Color.White;
            this.chk1.EndFactor = 0.2F;
            this.chk1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chk1.Location = new System.Drawing.Point(100, 137);
            this.chk1.Name = "chk1";
            this.chk1.NoBorder = false;
            this.chk1.NoLed = false;
            this.chk1.RadioButton = false;
            this.chk1.Radius = 6;
            this.chk1.RadiusB = 0;
            this.chk1.Size = new System.Drawing.Size(46, 22);
            this.chk1.StartColor = System.Drawing.Color.Black;
            this.chk1.StartFactor = 0.35F;
            this.chk1.TabIndex = 92;
            this.chk1.Text = "chk1";
            this.chk1.CheckedChanged += new System.EventHandler(this.chk1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(0, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Bandwdt";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label16.Location = new System.Drawing.Point(0, 51);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(57, 13);
            this.label16.TabIndex = 26;
            this.label16.Text = "Vfo wind.";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Orange;
            this.label6.Location = new System.Drawing.Point(0, 317);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 31);
            this.label6.TabIndex = 30;
            this.label6.Text = "Latency (ms)";
            this.label6.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label8.Location = new System.Drawing.Point(0, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "FFT wind,";
            // 
            // filterBandwidthNumericUpDown
            // 
            this.filterBandwidthNumericUpDown.Enabled = false;
            this.filterBandwidthNumericUpDown.ForeColor = System.Drawing.Color.Yellow;
            this.filterBandwidthNumericUpDown.Increment = 10;
            this.filterBandwidthNumericUpDown.Location = new System.Drawing.Point(56, 288);
            this.filterBandwidthNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.filterBandwidthNumericUpDown.Maximum = ((long)(250000));
            this.filterBandwidthNumericUpDown.Minimum = ((long)(10));
            this.filterBandwidthNumericUpDown.Name = "filterBandwidthNumericUpDown";
            this.filterBandwidthNumericUpDown.Size = new System.Drawing.Size(80, 20);
            this.filterBandwidthNumericUpDown.TabIndex = 87;
            this.filterBandwidthNumericUpDown.ToolTip = null;
            this.filterBandwidthNumericUpDown.Value = ((long)(6000));
            this.filterBandwidthNumericUpDown.Visible = false;
            this.filterBandwidthNumericUpDown.ValueChanged += new System.EventHandler(this.filterBandwidthNumericUpDown_ValueChanged);
            this.filterBandwidthNumericUpDown.Enter += new System.EventHandler(this.gUpDown_Enter);
            this.filterBandwidthNumericUpDown.Leave += new System.EventHandler(this.gpUpDown_Leave);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.ForeColor = System.Drawing.Color.Orange;
            this.label55.Location = new System.Drawing.Point(0, 268);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(58, 13);
            this.label55.TabIndex = 102;
            this.label55.Text = "Cent-Freq";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label59.Location = new System.Drawing.Point(0, 106);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(54, 13);
            this.label59.TabIndex = 131;
            this.label59.Text = "IF/AF res.";
            // 
            // radioCollapsiblePanel
            // 
            this.radioCollapsiblePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.radioCollapsiblePanel.Controls.Add(this.label60);
            this.radioCollapsiblePanel.Controls.Add(this.samRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.swapIQCheckBox);
            this.radioCollapsiblePanel.Controls.Add(this.squelchNumericUpDown);
            this.radioCollapsiblePanel.Controls.Add(this.useSquelchCheckBox);
            this.radioCollapsiblePanel.Controls.Add(this.stepSizeComboBox);
            this.radioCollapsiblePanel.Controls.Add(this.snapFrequencyCheckBox);
            this.radioCollapsiblePanel.Controls.Add(this.cmbDbm);
            this.radioCollapsiblePanel.Controls.Add(this.amRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.agcCheckBox);
            this.radioCollapsiblePanel.Controls.Add(this.dsbRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.panel1);
            this.radioCollapsiblePanel.Controls.Add(this.lsbRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.cwRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.rawRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.nfmRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.usbRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.wfmRadioButton);
            this.radioCollapsiblePanel.Controls.Add(this.correctIQCheckBox);
            this.radioCollapsiblePanel.ExpandedHeight = 197;
            this.radioCollapsiblePanel.ForeColor = System.Drawing.Color.Black;
            this.radioCollapsiblePanel.Location = new System.Drawing.Point(0, 4);
            this.radioCollapsiblePanel.Margin = new System.Windows.Forms.Padding(4);
            this.radioCollapsiblePanel.Name = "radioCollapsiblePanel";
            this.radioCollapsiblePanel.NextPanel = this.scopeCollapsiblePanel;
            this.radioCollapsiblePanel.PanelTitle = "Radio";
            this.radioCollapsiblePanel.Size = new System.Drawing.Size(198, 197);
            this.radioCollapsiblePanel.TabIndex = 3;
            this.radioCollapsiblePanel.StateChanged += new System.EventHandler(this.collapsiblePanel_StateChanged);
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label60.Location = new System.Drawing.Point(4, 81);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(39, 13);
            this.label60.TabIndex = 94;
            this.label60.Text = "Signal";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // samRadioButton
            // 
            this.samRadioButton.Arrow = 99;
            this.samRadioButton.Checked = false;
            this.samRadioButton.Edge = 0.15F;
            this.samRadioButton.EndColor = System.Drawing.Color.White;
            this.samRadioButton.EndFactor = 0.2F;
            this.samRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.samRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.samRadioButton.Location = new System.Drawing.Point(0, 50);
            this.samRadioButton.Name = "samRadioButton";
            this.samRadioButton.NoBorder = false;
            this.samRadioButton.NoLed = false;
            this.samRadioButton.RadioButton = true;
            this.samRadioButton.Radius = 6;
            this.samRadioButton.RadiusB = 0;
            this.samRadioButton.Size = new System.Drawing.Size(46, 24);
            this.samRadioButton.StartColor = System.Drawing.Color.Black;
            this.samRadioButton.StartFactor = 0.35F;
            this.samRadioButton.TabIndex = 5;
            this.samRadioButton.Text = "SAM";
            this.samRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // amRadioButton
            // 
            this.amRadioButton.Arrow = 99;
            this.amRadioButton.Checked = false;
            this.amRadioButton.Edge = 0.15F;
            this.amRadioButton.EndColor = System.Drawing.Color.White;
            this.amRadioButton.EndFactor = 0.2F;
            this.amRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.amRadioButton.Location = new System.Drawing.Point(1, 24);
            this.amRadioButton.Name = "amRadioButton";
            this.amRadioButton.NoBorder = false;
            this.amRadioButton.NoLed = false;
            this.amRadioButton.RadioButton = true;
            this.amRadioButton.Radius = 6;
            this.amRadioButton.RadiusB = 0;
            this.amRadioButton.Size = new System.Drawing.Size(46, 24);
            this.amRadioButton.StartColor = System.Drawing.Color.Black;
            this.amRadioButton.StartFactor = 0.35F;
            this.amRadioButton.TabIndex = 2;
            this.amRadioButton.Text = "AM";
            this.amRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // dsbRadioButton
            // 
            this.dsbRadioButton.Arrow = 99;
            this.dsbRadioButton.Checked = false;
            this.dsbRadioButton.Edge = 0.15F;
            this.dsbRadioButton.EndColor = System.Drawing.Color.White;
            this.dsbRadioButton.EndFactor = 0.2F;
            this.dsbRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dsbRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.dsbRadioButton.Location = new System.Drawing.Point(50, 50);
            this.dsbRadioButton.Name = "dsbRadioButton";
            this.dsbRadioButton.NoBorder = false;
            this.dsbRadioButton.NoLed = false;
            this.dsbRadioButton.RadioButton = true;
            this.dsbRadioButton.Radius = 6;
            this.dsbRadioButton.RadiusB = 0;
            this.dsbRadioButton.Size = new System.Drawing.Size(46, 24);
            this.dsbRadioButton.StartColor = System.Drawing.Color.Black;
            this.dsbRadioButton.StartFactor = 0.35F;
            this.dsbRadioButton.TabIndex = 6;
            this.dsbRadioButton.Text = "DSB";
            this.dsbRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butVfoC);
            this.panel1.Controls.Add(this.butVfoB);
            this.panel1.Controls.Add(this.butVfoA);
            this.panel1.Controls.Add(this.label38);
            this.panel1.Location = new System.Drawing.Point(0, 158);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(141, 33);
            this.panel1.TabIndex = 67;
            // 
            // butVfoC
            // 
            this.butVfoC.Arrow = 0;
            this.butVfoC.Checked = false;
            this.butVfoC.Edge = 0.15F;
            this.butVfoC.EndColor = System.Drawing.Color.Magenta;
            this.butVfoC.EndFactor = 0.2F;
            this.butVfoC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butVfoC.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.butVfoC.Location = new System.Drawing.Point(102, 3);
            this.butVfoC.Name = "butVfoC";
            this.butVfoC.NoBorder = false;
            this.butVfoC.NoLed = false;
            this.butVfoC.RadioButton = true;
            this.butVfoC.Radius = 6;
            this.butVfoC.RadiusB = 0;
            this.butVfoC.Size = new System.Drawing.Size(33, 26);
            this.butVfoC.StartColor = System.Drawing.Color.Black;
            this.butVfoC.StartFactor = 0.35F;
            this.butVfoC.TabIndex = 22;
            this.butVfoC.Text = "C";
            this.butVfoC.CheckedChanged += new System.EventHandler(this.butVfo_CheckedChanged);
            // 
            // butVfoB
            // 
            this.butVfoB.Arrow = 0;
            this.butVfoB.Checked = false;
            this.butVfoB.Edge = 0.15F;
            this.butVfoB.EndColor = System.Drawing.Color.Aqua;
            this.butVfoB.EndFactor = 0.2F;
            this.butVfoB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butVfoB.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.butVfoB.Location = new System.Drawing.Point(66, 3);
            this.butVfoB.Name = "butVfoB";
            this.butVfoB.NoBorder = false;
            this.butVfoB.NoLed = false;
            this.butVfoB.RadioButton = true;
            this.butVfoB.Radius = 6;
            this.butVfoB.RadiusB = 0;
            this.butVfoB.Size = new System.Drawing.Size(33, 26);
            this.butVfoB.StartColor = System.Drawing.Color.Black;
            this.butVfoB.StartFactor = 0.35F;
            this.butVfoB.TabIndex = 21;
            this.butVfoB.Text = "B";
            this.butVfoB.CheckedChanged += new System.EventHandler(this.butVfo_CheckedChanged);
            // 
            // butVfoA
            // 
            this.butVfoA.Arrow = 0;
            this.butVfoA.Checked = false;
            this.butVfoA.Edge = 0.15F;
            this.butVfoA.EndColor = System.Drawing.Color.GreenYellow;
            this.butVfoA.EndFactor = 0.2F;
            this.butVfoA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butVfoA.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.butVfoA.Location = new System.Drawing.Point(30, 3);
            this.butVfoA.Name = "butVfoA";
            this.butVfoA.NoBorder = false;
            this.butVfoA.NoLed = false;
            this.butVfoA.RadioButton = true;
            this.butVfoA.Radius = 6;
            this.butVfoA.RadiusB = 0;
            this.butVfoA.Size = new System.Drawing.Size(33, 26);
            this.butVfoA.StartColor = System.Drawing.Color.Black;
            this.butVfoA.StartFactor = 0.35F;
            this.butVfoA.TabIndex = 20;
            this.butVfoA.Text = "A";
            this.butVfoA.CheckedChanged += new System.EventHandler(this.butVfo_CheckedChanged);
            // 
            // label38
            // 
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label38.Location = new System.Drawing.Point(1, 11);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(30, 14);
            this.label38.TabIndex = 39;
            this.label38.Text = "Vfo";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lsbRadioButton
            // 
            this.lsbRadioButton.Arrow = 99;
            this.lsbRadioButton.Checked = false;
            this.lsbRadioButton.Edge = 0.15F;
            this.lsbRadioButton.EndColor = System.Drawing.Color.White;
            this.lsbRadioButton.EndFactor = 0.2F;
            this.lsbRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lsbRadioButton.Location = new System.Drawing.Point(50, 24);
            this.lsbRadioButton.Name = "lsbRadioButton";
            this.lsbRadioButton.NoBorder = false;
            this.lsbRadioButton.NoLed = false;
            this.lsbRadioButton.RadioButton = true;
            this.lsbRadioButton.Radius = 6;
            this.lsbRadioButton.RadiusB = 0;
            this.lsbRadioButton.Size = new System.Drawing.Size(46, 24);
            this.lsbRadioButton.StartColor = System.Drawing.Color.Black;
            this.lsbRadioButton.StartFactor = 0.35F;
            this.lsbRadioButton.TabIndex = 3;
            this.lsbRadioButton.Text = "LSB";
            this.lsbRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // cwRadioButton
            // 
            this.cwRadioButton.Arrow = 99;
            this.cwRadioButton.Checked = false;
            this.cwRadioButton.Edge = 0.15F;
            this.cwRadioButton.EndColor = System.Drawing.Color.White;
            this.cwRadioButton.EndFactor = 0.2F;
            this.cwRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cwRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.cwRadioButton.Location = new System.Drawing.Point(100, 50);
            this.cwRadioButton.Name = "cwRadioButton";
            this.cwRadioButton.NoBorder = false;
            this.cwRadioButton.NoLed = false;
            this.cwRadioButton.RadioButton = true;
            this.cwRadioButton.Radius = 6;
            this.cwRadioButton.RadiusB = 0;
            this.cwRadioButton.Size = new System.Drawing.Size(46, 24);
            this.cwRadioButton.StartColor = System.Drawing.Color.Black;
            this.cwRadioButton.StartFactor = 0.35F;
            this.cwRadioButton.TabIndex = 7;
            this.cwRadioButton.Text = "CW";
            this.cwRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // nfmRadioButton
            // 
            this.nfmRadioButton.Arrow = 99;
            this.nfmRadioButton.Checked = false;
            this.nfmRadioButton.Edge = 0.15F;
            this.nfmRadioButton.EndColor = System.Drawing.Color.White;
            this.nfmRadioButton.EndFactor = 0.2F;
            this.nfmRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nfmRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.nfmRadioButton.Location = new System.Drawing.Point(150, 50);
            this.nfmRadioButton.Name = "nfmRadioButton";
            this.nfmRadioButton.NoBorder = false;
            this.nfmRadioButton.NoLed = false;
            this.nfmRadioButton.RadioButton = true;
            this.nfmRadioButton.Radius = 6;
            this.nfmRadioButton.RadiusB = 0;
            this.nfmRadioButton.Size = new System.Drawing.Size(46, 24);
            this.nfmRadioButton.StartColor = System.Drawing.Color.Black;
            this.nfmRadioButton.StartFactor = 0.35F;
            this.nfmRadioButton.TabIndex = 9;
            this.nfmRadioButton.Text = "nFM";
            this.nfmRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // usbRadioButton
            // 
            this.usbRadioButton.Arrow = 99;
            this.usbRadioButton.Checked = false;
            this.usbRadioButton.Edge = 0.15F;
            this.usbRadioButton.EndColor = System.Drawing.Color.White;
            this.usbRadioButton.EndFactor = 0.2F;
            this.usbRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usbRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.usbRadioButton.Location = new System.Drawing.Point(100, 24);
            this.usbRadioButton.Name = "usbRadioButton";
            this.usbRadioButton.NoBorder = false;
            this.usbRadioButton.NoLed = false;
            this.usbRadioButton.RadioButton = true;
            this.usbRadioButton.Radius = 6;
            this.usbRadioButton.RadiusB = 0;
            this.usbRadioButton.Size = new System.Drawing.Size(46, 24);
            this.usbRadioButton.StartColor = System.Drawing.Color.Black;
            this.usbRadioButton.StartFactor = 0.35F;
            this.usbRadioButton.TabIndex = 4;
            this.usbRadioButton.Text = "USB";
            this.usbRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // wfmRadioButton
            // 
            this.wfmRadioButton.Arrow = 99;
            this.wfmRadioButton.Checked = false;
            this.wfmRadioButton.Edge = 0.15F;
            this.wfmRadioButton.EndColor = System.Drawing.Color.White;
            this.wfmRadioButton.EndFactor = 0.2F;
            this.wfmRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wfmRadioButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.wfmRadioButton.Location = new System.Drawing.Point(150, 24);
            this.wfmRadioButton.Name = "wfmRadioButton";
            this.wfmRadioButton.NoBorder = false;
            this.wfmRadioButton.NoLed = false;
            this.wfmRadioButton.RadioButton = true;
            this.wfmRadioButton.Radius = 6;
            this.wfmRadioButton.RadiusB = 0;
            this.wfmRadioButton.Size = new System.Drawing.Size(46, 24);
            this.wfmRadioButton.StartColor = System.Drawing.Color.Black;
            this.wfmRadioButton.StartFactor = 0.35F;
            this.wfmRadioButton.TabIndex = 8;
            this.wfmRadioButton.Text = "wFM";
            this.wfmRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckStateChanged);
            // 
            // scopeCollapsiblePanel
            // 
            this.scopeCollapsiblePanel.Controls.Add(this.gBexpand);
            this.scopeCollapsiblePanel.Controls.Add(this.label51);
            this.scopeCollapsiblePanel.Controls.Add(this.label43);
            this.scopeCollapsiblePanel.Controls.Add(this.label42);
            this.scopeCollapsiblePanel.Controls.Add(this.label41);
            this.scopeCollapsiblePanel.Controls.Add(this.scope);
            this.scopeCollapsiblePanel.Controls.Add(this.label40);
            this.scopeCollapsiblePanel.Controls.Add(this.label19);
            this.scopeCollapsiblePanel.Controls.Add(this.tbvPeakRel);
            this.scopeCollapsiblePanel.Controls.Add(this.tbvPeakDelay);
            this.scopeCollapsiblePanel.Controls.Add(this.tbvAudioAvg);
            this.scopeCollapsiblePanel.Controls.Add(this.tbvAudioRel);
            this.scopeCollapsiblePanel.Controls.Add(this.tbvCarrierAvg);
            this.scopeCollapsiblePanel.Controls.Add(this.tbTrigL);
            this.scopeCollapsiblePanel.Controls.Add(this.cmbHchannel);
            this.scopeCollapsiblePanel.Controls.Add(this.tbGain);
            this.scopeCollapsiblePanel.Controls.Add(this.tbAverage);
            this.scopeCollapsiblePanel.Controls.Add(this.chkHrunDC);
            this.scopeCollapsiblePanel.Controls.Add(this.chkAver);
            this.scopeCollapsiblePanel.Controls.Add(this.chkVrunDC);
            this.scopeCollapsiblePanel.Controls.Add(this.chkHinvert);
            this.scopeCollapsiblePanel.Controls.Add(this.chkXY);
            this.scopeCollapsiblePanel.Controls.Add(this.chkVinvert);
            this.scopeCollapsiblePanel.Controls.Add(this.cmbTim);
            this.scopeCollapsiblePanel.Controls.Add(this.cmbHor);
            this.scopeCollapsiblePanel.Controls.Add(this.cmbVer);
            this.scopeCollapsiblePanel.Controls.Add(this.label39);
            this.scopeCollapsiblePanel.Controls.Add(this.tbRFgain);
            this.scopeCollapsiblePanel.Controls.Add(this.label27);
            this.scopeCollapsiblePanel.Controls.Add(this.label20);
            this.scopeCollapsiblePanel.Controls.Add(this.label9);
            this.scopeCollapsiblePanel.Controls.Add(this.label35);
            this.scopeCollapsiblePanel.Controls.Add(this.labTbGain);
            this.scopeCollapsiblePanel.Controls.Add(this.labTbAverage);
            this.scopeCollapsiblePanel.Controls.Add(this.label36);
            this.scopeCollapsiblePanel.Controls.Add(this.label34);
            this.scopeCollapsiblePanel.Controls.Add(this.cmbVchannel);
            this.scopeCollapsiblePanel.Controls.Add(this.labFast);
            this.scopeCollapsiblePanel.Controls.Add(this.label48);
            this.scopeCollapsiblePanel.Controls.Add(this.label45);
            this.scopeCollapsiblePanel.Controls.Add(this.label46);
            this.scopeCollapsiblePanel.Controls.Add(this.label47);
            this.scopeCollapsiblePanel.Controls.Add(this.gBexpandScope);
            this.scopeCollapsiblePanel.ExpandedHeight = 595;
            this.scopeCollapsiblePanel.ForeColor = System.Drawing.Color.Black;
            this.scopeCollapsiblePanel.Location = new System.Drawing.Point(0, 201);
            this.scopeCollapsiblePanel.Margin = new System.Windows.Forms.Padding(4);
            this.scopeCollapsiblePanel.Name = "scopeCollapsiblePanel";
            this.scopeCollapsiblePanel.NextPanel = this.audioCollapsiblePanel;
            this.scopeCollapsiblePanel.PanelTitle = "Scope (/1000)";
            this.scopeCollapsiblePanel.Size = new System.Drawing.Size(198, 595);
            this.scopeCollapsiblePanel.TabIndex = 26;
            this.scopeCollapsiblePanel.Tag = "d";
            // 
            // label51
            // 
            this.label51.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label51.AutoSize = true;
            this.label51.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label51.Location = new System.Drawing.Point(4, 545);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(64, 13);
            this.label51.TabIndex = 122;
            this.label51.Tag = "1";
            this.label51.Text = "Phase lock:";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label43
            // 
            this.label43.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label43.AutoSize = true;
            this.label43.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label43.Location = new System.Drawing.Point(150, 433);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(50, 13);
            this.label43.TabIndex = 116;
            this.label43.Tag = "1";
            this.label43.Text = "Peak Rel";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label42
            // 
            this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label42.AutoSize = true;
            this.label42.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label42.Location = new System.Drawing.Point(110, 420);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(59, 13);
            this.label42.TabIndex = 115;
            this.label42.Tag = "1";
            this.label42.Text = "PeakDelay";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label41.AutoSize = true;
            this.label41.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label41.Location = new System.Drawing.Point(74, 433);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(60, 13);
            this.label41.TabIndex = 114;
            this.label41.Tag = "1";
            this.label41.Text = "Audio Avg";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label40.AutoSize = true;
            this.label40.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label40.Location = new System.Drawing.Point(38, 420);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(57, 13);
            this.label40.TabIndex = 113;
            this.label40.Tag = "1";
            this.label40.Text = "Audio Rel";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label19.Location = new System.Drawing.Point(-1, 434);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 13);
            this.label19.TabIndex = 112;
            this.label19.Tag = "1";
            this.label19.Text = "Carrier Avg";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label39
            // 
            this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label39.Location = new System.Drawing.Point(0, 25);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(51, 13);
            this.label39.TabIndex = 83;
            this.label39.Text = "RF gain";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label27.Location = new System.Drawing.Point(142, 341);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(52, 13);
            this.label27.TabIndex = 70;
            this.label27.Text = "Trig-level";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label20.Location = new System.Drawing.Point(72, 569);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(31, 13);
            this.label20.TabIndex = 76;
            this.label20.Tag = "1";
            this.label20.Text = "Gain";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label9.Location = new System.Drawing.Point(74, 549);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 75;
            this.label9.Tag = "1";
            this.label9.Text = "Avg";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label35.Location = new System.Drawing.Point(72, 292);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(45, 13);
            this.label35.TabIndex = 55;
            this.label35.Text = "Hor/div";
            // 
            // labTbGain
            // 
            this.labTbGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labTbGain.AutoSize = true;
            this.labTbGain.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labTbGain.Location = new System.Drawing.Point(176, 570);
            this.labTbGain.Name = "labTbGain";
            this.labTbGain.Size = new System.Drawing.Size(23, 13);
            this.labTbGain.TabIndex = 74;
            this.labTbGain.Tag = "1";
            this.labTbGain.Text = "-99";
            this.labTbGain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labTbAverage
            // 
            this.labTbAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labTbAverage.AutoSize = true;
            this.labTbAverage.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labTbAverage.Location = new System.Drawing.Point(176, 550);
            this.labTbAverage.Name = "labTbAverage";
            this.labTbAverage.Size = new System.Drawing.Size(23, 13);
            this.labTbAverage.TabIndex = 73;
            this.labTbAverage.Tag = "1";
            this.labTbAverage.Text = "-99";
            this.labTbAverage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label36.Location = new System.Drawing.Point(4, 292);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(42, 13);
            this.label36.TabIndex = 57;
            this.label36.Text = "Ver/div";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label34.Location = new System.Drawing.Point(135, 292);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(54, 13);
            this.label34.TabIndex = 65;
            this.label34.Text = "Timebase";
            // 
            // labFast
            // 
            this.labFast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labFast.AutoSize = true;
            this.labFast.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labFast.Location = new System.Drawing.Point(9, 521);
            this.labFast.Name = "labFast";
            this.labFast.Size = new System.Drawing.Size(26, 13);
            this.labFast.TabIndex = 117;
            this.labFast.Tag = "1";
            this.labFast.Text = "fast";
            this.labFast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label48
            // 
            this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label48.AutoSize = true;
            this.label48.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label48.Location = new System.Drawing.Point(48, 521);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(31, 13);
            this.label48.TabIndex = 121;
            this.label48.Tag = "1";
            this.label48.Text = "slow";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label45.AutoSize = true;
            this.label45.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label45.Location = new System.Drawing.Point(89, 521);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(26, 13);
            this.label45.TabIndex = 118;
            this.label45.Tag = "1";
            this.label45.Text = "fast";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label46
            // 
            this.label46.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label46.AutoSize = true;
            this.label46.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label46.Location = new System.Drawing.Point(124, 521);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(34, 13);
            this.label46.TabIndex = 119;
            this.label46.Tag = "1";
            this.label46.Text = "short";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label47
            // 
            this.label47.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label47.AutoSize = true;
            this.label47.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label47.Location = new System.Drawing.Point(164, 521);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(31, 13);
            this.label47.TabIndex = 120;
            this.label47.Tag = "1";
            this.label47.Text = "slow";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // audioCollapsiblePanel
            // 
            this.audioCollapsiblePanel.Controls.Add(this.cwShiftNumericUpDown);
            this.audioCollapsiblePanel.Controls.Add(this.label2);
            this.audioCollapsiblePanel.Controls.Add(this.outputDeviceComboBox);
            this.audioCollapsiblePanel.Controls.Add(this.inputDeviceComboBox);
            this.audioCollapsiblePanel.Controls.Add(this.sampleRateComboBox);
            this.audioCollapsiblePanel.Controls.Add(this.filterAudioCheckBox);
            this.audioCollapsiblePanel.Controls.Add(this.chkNotch3);
            this.audioCollapsiblePanel.Controls.Add(this.chkNotch2);
            this.audioCollapsiblePanel.Controls.Add(this.chkNotch1);
            this.audioCollapsiblePanel.Controls.Add(this.fmStereoCheckBox);
            this.audioCollapsiblePanel.Controls.Add(this.chkNotch0);
            this.audioCollapsiblePanel.Controls.Add(this.label12);
            this.audioCollapsiblePanel.Controls.Add(this.labSampling);
            this.audioCollapsiblePanel.Controls.Add(this.label11);
            this.audioCollapsiblePanel.Controls.Add(this.label15);
            this.audioCollapsiblePanel.ExpandedHeight = 176;
            this.audioCollapsiblePanel.ForeColor = System.Drawing.Color.Black;
            this.audioCollapsiblePanel.Location = new System.Drawing.Point(0, 796);
            this.audioCollapsiblePanel.Margin = new System.Windows.Forms.Padding(4);
            this.audioCollapsiblePanel.Name = "audioCollapsiblePanel";
            this.audioCollapsiblePanel.NextPanel = this.agcCollapsiblePanel;
            this.audioCollapsiblePanel.PanelTitle = "Audio";
            this.audioCollapsiblePanel.Size = new System.Drawing.Size(198, 176);
            this.audioCollapsiblePanel.TabIndex = 49;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label2.Location = new System.Drawing.Point(0, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 43;
            this.label2.Text = "Notches";
            // 
            // chkNotch3
            // 
            this.chkNotch3.Arrow = 0;
            this.chkNotch3.Checked = false;
            this.chkNotch3.Edge = 0.15F;
            this.chkNotch3.EndColor = System.Drawing.Color.White;
            this.chkNotch3.EndFactor = 0.2F;
            this.chkNotch3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNotch3.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkNotch3.Location = new System.Drawing.Point(166, 23);
            this.chkNotch3.Name = "chkNotch3";
            this.chkNotch3.NoBorder = false;
            this.chkNotch3.NoLed = false;
            this.chkNotch3.RadioButton = false;
            this.chkNotch3.Radius = 6;
            this.chkNotch3.RadiusB = 0;
            this.chkNotch3.Size = new System.Drawing.Size(30, 26);
            this.chkNotch3.StartColor = System.Drawing.Color.Black;
            this.chkNotch3.StartFactor = 0.35F;
            this.chkNotch3.TabIndex = 53;
            this.chkNotch3.Text = "4";
            this.chkNotch3.CheckedChanged += new System.EventHandler(this.chkNotch_CheckedChanged);
            // 
            // chkNotch2
            // 
            this.chkNotch2.Arrow = 0;
            this.chkNotch2.Checked = false;
            this.chkNotch2.Edge = 0.15F;
            this.chkNotch2.EndColor = System.Drawing.Color.White;
            this.chkNotch2.EndFactor = 0.2F;
            this.chkNotch2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNotch2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkNotch2.Location = new System.Drawing.Point(129, 23);
            this.chkNotch2.Name = "chkNotch2";
            this.chkNotch2.NoBorder = false;
            this.chkNotch2.NoLed = false;
            this.chkNotch2.RadioButton = false;
            this.chkNotch2.Radius = 6;
            this.chkNotch2.RadiusB = 0;
            this.chkNotch2.Size = new System.Drawing.Size(30, 26);
            this.chkNotch2.StartColor = System.Drawing.Color.Black;
            this.chkNotch2.StartFactor = 0.35F;
            this.chkNotch2.TabIndex = 52;
            this.chkNotch2.Text = "3";
            this.chkNotch2.CheckedChanged += new System.EventHandler(this.chkNotch_CheckedChanged);
            // 
            // chkNotch1
            // 
            this.chkNotch1.Arrow = 0;
            this.chkNotch1.Checked = false;
            this.chkNotch1.Edge = 0.15F;
            this.chkNotch1.EndColor = System.Drawing.Color.White;
            this.chkNotch1.EndFactor = 0.2F;
            this.chkNotch1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNotch1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkNotch1.Location = new System.Drawing.Point(91, 23);
            this.chkNotch1.Name = "chkNotch1";
            this.chkNotch1.NoBorder = false;
            this.chkNotch1.NoLed = false;
            this.chkNotch1.RadioButton = false;
            this.chkNotch1.Radius = 6;
            this.chkNotch1.RadiusB = 0;
            this.chkNotch1.Size = new System.Drawing.Size(30, 26);
            this.chkNotch1.StartColor = System.Drawing.Color.Black;
            this.chkNotch1.StartFactor = 0.35F;
            this.chkNotch1.TabIndex = 51;
            this.chkNotch1.Text = "2";
            this.chkNotch1.CheckedChanged += new System.EventHandler(this.chkNotch_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label12.Location = new System.Drawing.Point(0, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Output";
            // 
            // labSampling
            // 
            this.labSampling.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labSampling.Location = new System.Drawing.Point(0, 111);
            this.labSampling.Name = "labSampling";
            this.labSampling.Size = new System.Drawing.Size(57, 30);
            this.labSampling.TabIndex = 28;
            this.labSampling.Text = "min. sampling";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label11.Location = new System.Drawing.Point(0, 61);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Input";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label15.Location = new System.Drawing.Point(0, 146);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 13);
            this.label15.TabIndex = 32;
            this.label15.Text = "CW Shift";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // agcCollapsiblePanel
            // 
            this.agcCollapsiblePanel.Controls.Add(this.panel2);
            this.agcCollapsiblePanel.Controls.Add(this.agcThresholdNumericUpDown);
            this.agcCollapsiblePanel.Controls.Add(this.agcDecayNumericUpDown);
            this.agcCollapsiblePanel.Controls.Add(this.agcSlopeNumericUpDown);
            this.agcCollapsiblePanel.Controls.Add(this.agcUseHangCheckBox);
            this.agcCollapsiblePanel.Controls.Add(this.label22);
            this.agcCollapsiblePanel.Controls.Add(this.label53);
            this.agcCollapsiblePanel.Controls.Add(this.label56);
            this.agcCollapsiblePanel.Controls.Add(this.agcDecayLabel);
            this.agcCollapsiblePanel.ExpandedHeight = 165;
            this.agcCollapsiblePanel.ForeColor = System.Drawing.Color.Black;
            this.agcCollapsiblePanel.Location = new System.Drawing.Point(0, 972);
            this.agcCollapsiblePanel.Margin = new System.Windows.Forms.Padding(4);
            this.agcCollapsiblePanel.Name = "agcCollapsiblePanel";
            this.agcCollapsiblePanel.NextPanel = this.displayCollapsiblePanel;
            this.agcCollapsiblePanel.PanelTitle = "AGC";
            this.agcCollapsiblePanel.Size = new System.Drawing.Size(198, 165);
            this.agcCollapsiblePanel.TabIndex = 60;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkNLimiter);
            this.panel2.Controls.Add(this.label33);
            this.panel2.Controls.Add(this.tbNLRatio);
            this.panel2.Controls.Add(this.labNLTreshold);
            this.panel2.Controls.Add(this.tbNLTreshold);
            this.panel2.Controls.Add(this.labNLRatio);
            this.panel2.Location = new System.Drawing.Point(0, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(198, 58);
            this.panel2.TabIndex = 103;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label33.Location = new System.Drawing.Point(4, 35);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(34, 13);
            this.label33.TabIndex = 76;
            this.label33.Tag = "Ratio";
            this.label33.Text = "Ratio";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labNLTreshold
            // 
            this.labNLTreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labNLTreshold.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labNLTreshold.Location = new System.Drawing.Point(48, 3);
            this.labNLTreshold.Name = "labNLTreshold";
            this.labNLTreshold.Size = new System.Drawing.Size(33, 23);
            this.labNLTreshold.TabIndex = 102;
            this.labNLTreshold.Text = "100";
            this.labNLTreshold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labNLRatio
            // 
            this.labNLRatio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labNLRatio.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labNLRatio.Location = new System.Drawing.Point(48, 30);
            this.labNLRatio.Name = "labNLRatio";
            this.labNLRatio.Size = new System.Drawing.Size(33, 23);
            this.labNLRatio.TabIndex = 101;
            this.labNLRatio.Text = "100";
            this.labNLRatio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label22.Location = new System.Drawing.Point(55, 46);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(66, 26);
            this.label22.TabIndex = 13;
            this.label22.Text = "Slope (dB)";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label53
            // 
            this.label53.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label53.Location = new System.Drawing.Point(54, 23);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(70, 26);
            this.label53.TabIndex = 96;
            this.label53.Text = "Thresh. (dB)";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label56
            // 
            this.label56.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label56.Location = new System.Drawing.Point(-1, 63);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(56, 38);
            this.label56.TabIndex = 99;
            this.label56.Text = "Decay (ms)";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // agcDecayLabel
            // 
            this.agcDecayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.agcDecayLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.agcDecayLabel.Location = new System.Drawing.Point(46, 80);
            this.agcDecayLabel.Name = "agcDecayLabel";
            this.agcDecayLabel.Size = new System.Drawing.Size(38, 19);
            this.agcDecayLabel.TabIndex = 98;
            this.agcDecayLabel.Text = "-100 dB";
            this.agcDecayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // displayCollapsiblePanel
            // 
            this.displayCollapsiblePanel.Controls.Add(this.label3);
            this.displayCollapsiblePanel.Controls.Add(this.remDcSlider);
            this.displayCollapsiblePanel.Controls.Add(this.cmbAudio);
            this.displayCollapsiblePanel.Controls.Add(this.label54);
            this.displayCollapsiblePanel.Controls.Add(this.label7);
            this.displayCollapsiblePanel.Controls.Add(this.chkIF);
            this.displayCollapsiblePanel.Controls.Add(this.chkWF);
            this.displayCollapsiblePanel.Controls.Add(this.dbmOffsetUpDown);
            this.displayCollapsiblePanel.Controls.Add(this.chkAutoSize);
            this.displayCollapsiblePanel.Controls.Add(this.labSmeter);
            this.displayCollapsiblePanel.Controls.Add(this.frequencyShiftNumericUpDown);
            this.displayCollapsiblePanel.Controls.Add(this.markPeaksCheckBox);
            this.displayCollapsiblePanel.Controls.Add(this.frequencyShiftCheckBox);
            this.displayCollapsiblePanel.Controls.Add(this.fftResolutionComboBox);
            this.displayCollapsiblePanel.Controls.Add(this.wDecayTrackBar);
            this.displayCollapsiblePanel.Controls.Add(this.fftAverageUpDown);
            this.displayCollapsiblePanel.Controls.Add(this.wAttackTrackBar);
            this.displayCollapsiblePanel.Controls.Add(this.labFftAverage);
            this.displayCollapsiblePanel.Controls.Add(this.useTimestampsCheckBox);
            this.displayCollapsiblePanel.Controls.Add(this.sDecayTrackBar);
            this.displayCollapsiblePanel.Controls.Add(this.sAttackTrackBar);
            this.displayCollapsiblePanel.Controls.Add(this.label23);
            this.displayCollapsiblePanel.Controls.Add(this.label26);
            this.displayCollapsiblePanel.Controls.Add(this.label24);
            this.displayCollapsiblePanel.Controls.Add(this.label25);
            this.displayCollapsiblePanel.Controls.Add(this.label4);
            this.displayCollapsiblePanel.Controls.Add(this.chkIndepSideband);
            this.displayCollapsiblePanel.Controls.Add(this.label21);
            this.displayCollapsiblePanel.Controls.Add(this.label13);
            this.displayCollapsiblePanel.Controls.Add(this.gradientButtonSA);
            this.displayCollapsiblePanel.ExpandedHeight = 314;
            this.displayCollapsiblePanel.ForeColor = System.Drawing.Color.Black;
            this.displayCollapsiblePanel.Location = new System.Drawing.Point(0, 1137);
            this.displayCollapsiblePanel.Margin = new System.Windows.Forms.Padding(4);
            this.displayCollapsiblePanel.Name = "displayCollapsiblePanel";
            this.displayCollapsiblePanel.NextPanel = this.AdvancedCollapsiblePanel;
            this.displayCollapsiblePanel.PanelTitle = "FFT Display";
            this.displayCollapsiblePanel.Size = new System.Drawing.Size(198, 314);
            this.displayCollapsiblePanel.TabIndex = 68;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label3.Location = new System.Drawing.Point(0, 288);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 131;
            this.label3.Text = "DC spike";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.ForeColor = System.Drawing.Color.Orange;
            this.label54.Location = new System.Drawing.Point(180, 70);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(20, 13);
            this.label54.TabIndex = 127;
            this.label54.Text = "Hz";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label7.Location = new System.Drawing.Point(51, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 124;
            this.label7.Text = "show IF";
            // 
            // labSmeter
            // 
            this.labSmeter.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labSmeter.Location = new System.Drawing.Point(0, 251);
            this.labSmeter.Name = "labSmeter";
            this.labSmeter.Size = new System.Drawing.Size(63, 30);
            this.labSmeter.TabIndex = 94;
            this.labSmeter.Text = "S-meter offs. (dB)";
            // 
            // wDecayTrackBar
            // 
            this.wDecayTrackBar.Button = false;
            this.wDecayTrackBar.Checked = false;
            this.wDecayTrackBar.ColorFactor = 0.55F;
            this.wDecayTrackBar.ForeColor = System.Drawing.Color.Black;
            this.wDecayTrackBar.Location = new System.Drawing.Point(53, 171);
            this.wDecayTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.wDecayTrackBar.Maximum = 50;
            this.wDecayTrackBar.Minimum = 0;
            this.wDecayTrackBar.Name = "wDecayTrackBar";
            this.wDecayTrackBar.Size = new System.Drawing.Size(92, 15);
            this.wDecayTrackBar.TabIndex = 79;
            this.wDecayTrackBar.TickColor = System.Drawing.Color.Orange;
            this.wDecayTrackBar.Ticks = 0;
            this.wDecayTrackBar.ToolTip = null;
            this.wDecayTrackBar.Value = 10;
            this.wDecayTrackBar.ValueChanged += new System.EventHandler(this.wDecayTrackBar_ValueChanged);
            // 
            // wAttackTrackBar
            // 
            this.wAttackTrackBar.Button = false;
            this.wAttackTrackBar.Checked = false;
            this.wAttackTrackBar.ColorFactor = 0.55F;
            this.wAttackTrackBar.ForeColor = System.Drawing.Color.Black;
            this.wAttackTrackBar.Location = new System.Drawing.Point(53, 147);
            this.wAttackTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.wAttackTrackBar.Maximum = 50;
            this.wAttackTrackBar.Minimum = 0;
            this.wAttackTrackBar.Name = "wAttackTrackBar";
            this.wAttackTrackBar.Size = new System.Drawing.Size(92, 16);
            this.wAttackTrackBar.TabIndex = 78;
            this.wAttackTrackBar.TickColor = System.Drawing.Color.Orange;
            this.wAttackTrackBar.Ticks = 0;
            this.wAttackTrackBar.ToolTip = null;
            this.wAttackTrackBar.Value = 10;
            this.wAttackTrackBar.ValueChanged += new System.EventHandler(this.wAttackTrackBar_ValueChanged);
            // 
            // labFftAverage
            // 
            this.labFftAverage.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labFftAverage.Location = new System.Drawing.Point(0, 231);
            this.labFftAverage.Name = "labFftAverage";
            this.labFftAverage.Size = new System.Drawing.Size(59, 19);
            this.labFftAverage.TabIndex = 96;
            this.labFftAverage.Text = "FFT aver.";
            // 
            // sDecayTrackBar
            // 
            this.sDecayTrackBar.Button = false;
            this.sDecayTrackBar.Checked = false;
            this.sDecayTrackBar.ColorFactor = 0.55F;
            this.sDecayTrackBar.ForeColor = System.Drawing.Color.Black;
            this.sDecayTrackBar.Location = new System.Drawing.Point(53, 121);
            this.sDecayTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.sDecayTrackBar.Maximum = 50;
            this.sDecayTrackBar.Minimum = 0;
            this.sDecayTrackBar.Name = "sDecayTrackBar";
            this.sDecayTrackBar.Size = new System.Drawing.Size(92, 16);
            this.sDecayTrackBar.TabIndex = 77;
            this.sDecayTrackBar.TickColor = System.Drawing.Color.Orange;
            this.sDecayTrackBar.Ticks = 0;
            this.sDecayTrackBar.ToolTip = null;
            this.sDecayTrackBar.Value = 10;
            this.sDecayTrackBar.ValueChanged += new System.EventHandler(this.sDecayTrackBar_ValueChanged);
            // 
            // sAttackTrackBar
            // 
            this.sAttackTrackBar.Button = false;
            this.sAttackTrackBar.Checked = false;
            this.sAttackTrackBar.ColorFactor = 0.55F;
            this.sAttackTrackBar.ForeColor = System.Drawing.Color.Black;
            this.sAttackTrackBar.Location = new System.Drawing.Point(53, 100);
            this.sAttackTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.sAttackTrackBar.Maximum = 50;
            this.sAttackTrackBar.Minimum = 0;
            this.sAttackTrackBar.Name = "sAttackTrackBar";
            this.sAttackTrackBar.Size = new System.Drawing.Size(92, 15);
            this.sAttackTrackBar.TabIndex = 76;
            this.sAttackTrackBar.TickColor = System.Drawing.Color.Orange;
            this.sAttackTrackBar.Ticks = 0;
            this.sAttackTrackBar.ToolTip = null;
            this.sAttackTrackBar.Value = 10;
            this.sAttackTrackBar.ValueChanged += new System.EventHandler(this.sAttackTrackBar_ValueChanged);
            // 
            // label23
            // 
            this.label23.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label23.Location = new System.Drawing.Point(0, 101);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(48, 14);
            this.label23.TabIndex = 23;
            this.label23.Text = "SA att.";
            // 
            // label26
            // 
            this.label26.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label26.Location = new System.Drawing.Point(0, 149);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(59, 15);
            this.label26.TabIndex = 25;
            this.label26.Text = "WF att.";
            // 
            // label24
            // 
            this.label24.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label24.Location = new System.Drawing.Point(0, 121);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(49, 14);
            this.label24.TabIndex = 24;
            this.label24.Text = "decay";
            // 
            // label25
            // 
            this.label25.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label25.Location = new System.Drawing.Point(0, 172);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(59, 15);
            this.label25.TabIndex = 26;
            this.label25.Text = "decay";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label4.Location = new System.Drawing.Point(1, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 123;
            this.label4.Text = "Waterfall";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label21.Location = new System.Drawing.Point(0, 200);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(49, 13);
            this.label21.TabIndex = 18;
            this.label21.Text = "Resolut.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label13.Location = new System.Drawing.Point(92, 36);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 13);
            this.label13.TabIndex = 129;
            this.label13.Text = "Audio";
            // 
            // scrollPanel
            // 
            this.scrollPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.scrollPanel.AutoScroll = true;
            this.scrollPanel.Controls.Add(this.pnlScroll);
            this.scrollPanel.Controls.Add(this.controlPanel);
            this.scrollPanel.Location = new System.Drawing.Point(7, 68);
            this.scrollPanel.Name = "scrollPanel";
            this.scrollPanel.Size = new System.Drawing.Size(227, 689);
            this.scrollPanel.TabIndex = 28;
            // 
            // pnlScroll
            // 
            this.pnlScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlScroll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnlScroll.Controls.Add(this.gThumb);
            this.pnlScroll.Location = new System.Drawing.Point(203, 7017);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(13, 56075);
            this.pnlScroll.TabIndex = 26;
            this.pnlScroll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlScroll_MouseDown);
            // 
            // gThumb
            // 
            this.gThumb.Arrow = 5;
            this.gThumb.Checked = false;
            this.gThumb.Edge = 0.01F;
            this.gThumb.EndColor = System.Drawing.Color.LightGray;
            this.gThumb.EndFactor = 0.45F;
            this.gThumb.Location = new System.Drawing.Point(1, 0);
            this.gThumb.Name = "gThumb";
            this.gThumb.NoBorder = true;
            this.gThumb.NoLed = true;
            this.gThumb.RadioButton = false;
            this.gThumb.Radius = 1;
            this.gThumb.RadiusB = 0;
            this.gThumb.Size = new System.Drawing.Size(11, 100);
            this.gThumb.StartColor = System.Drawing.Color.Black;
            this.gThumb.StartFactor = 0.45F;
            this.gThumb.TabIndex = 0;
            this.gThumb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gThumb_MouseDown);
            this.gThumb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gThumb_MouseMove);
            this.gThumb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gThumb_MouseUp);
            // 
            // labBandWidth
            // 
            this.labBandWidth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labBandWidth.AutoSize = true;
            this.labBandWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labBandWidth.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labBandWidth.Location = new System.Drawing.Point(783, 14);
            this.labBandWidth.Name = "labBandWidth";
            this.labBandWidth.Size = new System.Drawing.Size(25, 13);
            this.labBandWidth.TabIndex = 45;
            this.labBandWidth.Text = "BW";
            this.labBandWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.AutoSize = true;
            this.label37.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label37.Location = new System.Drawing.Point(1073, 14);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(38, 13);
            this.label37.TabIndex = 101;
            this.label37.Text = "Spect.";
            // 
            // labSpectrum
            // 
            this.labSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labSpectrum.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labSpectrum.Location = new System.Drawing.Point(1113, 12);
            this.labSpectrum.Margin = new System.Windows.Forms.Padding(4);
            this.labSpectrum.Name = "labSpectrum";
            this.labSpectrum.Size = new System.Drawing.Size(59, 20);
            this.labSpectrum.TabIndex = 108;
            this.labSpectrum.Text = "250 Khz";
            // 
            // tbIntensityWv
            // 
            this.tbIntensityWv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIntensityWv.Button = false;
            this.tbIntensityWv.Checked = false;
            this.tbIntensityWv.ColorFactor = 0.5F;
            this.tbIntensityWv.Location = new System.Drawing.Point(1259, 492);
            this.tbIntensityWv.Margin = new System.Windows.Forms.Padding(4);
            this.tbIntensityWv.Maximum = -60;
            this.tbIntensityWv.Minimum = -160;
            this.tbIntensityWv.Name = "tbIntensityWv";
            this.tbIntensityWv.Size = new System.Drawing.Size(17, 68);
            this.tbIntensityWv.TabIndex = 104;
            this.tbIntensityWv.Tag = "Waterfall intensity";
            this.tbIntensityWv.TickColor = System.Drawing.Color.Orange;
            this.tbIntensityWv.Ticks = 0;
            this.tbIntensityWv.ToolTip = null;
            this.tbIntensityWv.Value = -100;
            this.tbIntensityWv.Visible = false;
            this.tbIntensityWv.ValueChanged += new System.EventHandler(this.tbIntensityWv_Changed);
            // 
            // fftZoomTrackBar
            // 
            this.fftZoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fftZoomTrackBar.Button = false;
            this.fftZoomTrackBar.Checked = false;
            this.fftZoomTrackBar.ColorFactor = 0.5F;
            this.fftZoomTrackBar.Location = new System.Drawing.Point(981, 11);
            this.fftZoomTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.fftZoomTrackBar.Maximum = 50;
            this.fftZoomTrackBar.Minimum = 0;
            this.fftZoomTrackBar.Name = "fftZoomTrackBar";
            this.fftZoomTrackBar.Size = new System.Drawing.Size(65, 18);
            this.fftZoomTrackBar.TabIndex = 96;
            this.fftZoomTrackBar.TickColor = System.Drawing.Color.Orange;
            this.fftZoomTrackBar.Ticks = 0;
            this.fftZoomTrackBar.ToolTip = null;
            this.fftZoomTrackBar.Value = 0;
            this.fftZoomTrackBar.Visible = false;
            this.fftZoomTrackBar.ValueChanged += new System.EventHandler(this.fftZoomTrackBar_ValueChanged);
            // 
            // chkScrollPanel
            // 
            this.chkScrollPanel.AllowShowFocusCues = false;
            this.chkScrollPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chkScrollPanel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScrollPanel.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.chkScrollPanel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkScrollPanel.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkScrollPanel.Location = new System.Drawing.Point(9, 13);
            this.chkScrollPanel.Margin = new System.Windows.Forms.Padding(0);
            this.chkScrollPanel.Name = "chkScrollPanel";
            // 
            // 
            // 
            this.chkScrollPanel.RootElement.EnableFocusBorderAnimation = false;
            this.chkScrollPanel.RootElement.FocusBorderWidth = 0;
            this.chkScrollPanel.Size = new System.Drawing.Size(62, 16);
            this.chkScrollPanel.TabIndex = 110;
            this.chkScrollPanel.Text = "SideBar";
            this.chkScrollPanel.ThemeName = "VisualStudio2012Dark";
            this.chkScrollPanel.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.chkScrollPanel.UseMnemonic = false;
            this.chkScrollPanel.CheckStateChanged += new System.EventHandler(this.chkScrollPanel_CheckedChanged);
            ((Telerik.WinControls.UI.RadCheckBoxElement)(this.chkScrollPanel.GetChildAt(0))).ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            ((Telerik.WinControls.UI.RadCheckBoxElement)(this.chkScrollPanel.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            ((Telerik.WinControls.UI.RadCheckBoxElement)(this.chkScrollPanel.GetChildAt(0))).Text = "SideBar";
            ((Telerik.WinControls.UI.RadCheckBoxElement)(this.chkScrollPanel.GetChildAt(0))).FocusBorderWidth = 0;
            ((Telerik.WinControls.UI.RadCheckBoxElement)(this.chkScrollPanel.GetChildAt(0))).EnableFocusBorderAnimation = false;
            ((Telerik.WinControls.UI.RadCheckmark)(this.chkScrollPanel.GetChildAt(0).GetChildAt(1).GetChildAt(1))).FocusBorderWidth = 0;
            ((Telerik.WinControls.UI.RadCheckmark)(this.chkScrollPanel.GetChildAt(0).GetChildAt(1).GetChildAt(1))).EnableFocusBorderAnimation = false;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.chkScrollPanel.GetChildAt(0).GetChildAt(2))).Width = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.chkScrollPanel.GetChildAt(0).GetChildAt(2))).LeftWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.chkScrollPanel.GetChildAt(0).GetChildAt(2))).TopWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.chkScrollPanel.GetChildAt(0).GetChildAt(2))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.chkScrollPanel.GetChildAt(0).GetChildAt(2))).BottomWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.chkScrollPanel.GetChildAt(0).GetChildAt(2))).AutoSize = true;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.chkScrollPanel.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // gButton1
            // 
            this.gButton1.Arrow = 0;
            this.gButton1.Checked = false;
            this.gButton1.Edge = 0.15F;
            this.gButton1.EndColor = System.Drawing.Color.White;
            this.gButton1.EndFactor = 0.2F;
            this.gButton1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gButton1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.gButton1.Location = new System.Drawing.Point(1420, 10);
            this.gButton1.Name = "gButton1";
            this.gButton1.NoBorder = false;
            this.gButton1.NoLed = true;
            this.gButton1.RadioButton = false;
            this.gButton1.Radius = 6;
            this.gButton1.RadiusB = 0;
            this.gButton1.Size = new System.Drawing.Size(58, 20);
            this.gButton1.StartColor = System.Drawing.Color.Black;
            this.gButton1.StartFactor = 0.35F;
            this.gButton1.TabIndex = 112;
            this.gButton1.Text = "About";
            this.gButton1.CheckedChanged += new System.EventHandler(this.gButton1_CheckedChanged);
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1284, 763);
            this.Controls.Add(this.gButton1);
            this.Controls.Add(this.chkScrollPanel);
            this.Controls.Add(this.labSpectrum);
            this.Controls.Add(this.audioButton);
            this.Controls.Add(this.panSplitContainer);
            this.Controls.Add(this.playBar);
            this.Controls.Add(this.labSpeed);
            this.Controls.Add(this.fftSpeedTrackBar);
            this.Controls.Add(this.tbIntensityWv);
            this.Controls.Add(this.tbFloorSA);
            this.Controls.Add(this.tbSpanSA);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.tbContrastWv);
            this.Controls.Add(this.labInt);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.labCon);
            this.Controls.Add(this.gBsetScale);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.fftZoomCombo);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.scrollPanel);
            this.Controls.Add(this.frequencyNumericUpDown);
            this.Controls.Add(this.chkLock);
            this.Controls.Add(this.cmbBandWidth);
            this.Controls.Add(this.labBandWidth);
            this.Controls.Add(this.iqSourceComboBox);
            this.Controls.Add(this.playStopButton);
            this.Controls.Add(this.configureSourceButton);
            this.Controls.Add(this.fftZoomTrackBar);
            this.Controls.Add(this.audioGainTrackBar);
            this.Controls.Add(this.labZoom);
            this.Controls.Add(this.label17);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SDRSharper Revised";
            this.ThemeName = "VisualStudio2012Dark";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panSplitContainer.Panel1.ResumeLayout(false);
            this.panSplitContainer.Panel2.ResumeLayout(false);
            this.panSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer)).EndInit();
            this.panSplitContainer.ResumeLayout(false);
            this.panSplitContainer2.Panel1.ResumeLayout(false);
            this.panSplitContainer2.Panel1.PerformLayout();
            this.panSplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer2)).EndInit();
            this.panSplitContainer2.ResumeLayout(false);
            this.panSplitContainer3.Panel1.ResumeLayout(false);
            this.panSplitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer3)).EndInit();
            this.panSplitContainer3.ResumeLayout(false);
            this.panSplitContainer4.Panel1.ResumeLayout(false);
            this.panSplitContainer4.Panel2.ResumeLayout(false);
            this.panSplitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer4)).EndInit();
            this.panSplitContainer4.ResumeLayout(false);
            this.panSplitContainer5.Panel1.ResumeLayout(false);
            this.panSplitContainer5.Panel1.PerformLayout();
            this.panSplitContainer5.Panel2.ResumeLayout(false);
            this.panSplitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panSplitContainer5)).EndInit();
            this.panSplitContainer5.ResumeLayout(false);
            this.panelAG.ResumeLayout(false);
            this.panelAG.PerformLayout();
            this.MnuSA.ResumeLayout(false);
            this.controlPanel.ResumeLayout(false);
            this.AdvancedCollapsiblePanel.ResumeLayout(false);
            this.AdvancedCollapsiblePanel.PerformLayout();
            this.radioCollapsiblePanel.ResumeLayout(false);
            this.radioCollapsiblePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.scopeCollapsiblePanel.ResumeLayout(false);
            this.scopeCollapsiblePanel.PerformLayout();
            this.audioCollapsiblePanel.ResumeLayout(false);
            this.audioCollapsiblePanel.PerformLayout();
            this.agcCollapsiblePanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.displayCollapsiblePanel.ResumeLayout(false);
            this.displayCollapsiblePanel.PerformLayout();
            this.scrollPanel.ResumeLayout(false);
            this.scrollPanel.PerformLayout();
            this.pnlScroll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkScrollPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private global::System.ComponentModel.IContainer components;
        private global::System.Windows.Forms.OpenFileDialog openDlg;
        private global::SDRSharp.PanView.SpectrumAnalyzer spectrumAnalyzer;
        private global::SDRSharp.PanView.Waterfall waterfall;
        private global::System.Windows.Forms.Label label12;
        private global::System.Windows.Forms.Label label11;
        private global::System.Windows.Forms.Label labSampling;
        private global::System.Windows.Forms.SplitContainer panSplitContainer;
        private global::System.Windows.Forms.Label label8;
        private global::System.Windows.Forms.Timer iqTimer;
        private global::System.Windows.Forms.Label labZoom;
        private global::System.Windows.Forms.Label label21;
        private global::System.Windows.Forms.Label label22;
        private global::System.Windows.Forms.Label label1;
        private global::System.Windows.Forms.Label label16;
        private global::System.Windows.Forms.Label label5;
        private global::SDRSharp.CollapsiblePanel.CollapsiblePanel radioCollapsiblePanel;
        private global::SDRSharp.CollapsiblePanel.CollapsiblePanel audioCollapsiblePanel;
        private global::SDRSharp.CollapsiblePanel.CollapsiblePanel agcCollapsiblePanel;
        private global::SDRSharp.CollapsiblePanel.CollapsiblePanel displayCollapsiblePanel;
        private global::System.Windows.Forms.Label label6;
        private global::System.Windows.Forms.Label label15;
        private global::System.Windows.Forms.Panel controlPanel;
        private global::System.Windows.Forms.Label label25;
        private global::System.Windows.Forms.Label label26;
        private global::System.Windows.Forms.Label label24;
        private global::System.Windows.Forms.Label label23;
        private global::System.Windows.Forms.Panel scrollPanel;
        private global::System.Windows.Forms.Label label29;
        private global::System.Windows.Forms.Label label30;
        private global::System.Windows.Forms.Label labInt;
        private global::System.Windows.Forms.Label labCon;
        private global::System.Windows.Forms.Label label31;
        private global::System.Windows.Forms.Label label32;
        private global::System.Windows.Forms.Label label17;
        private global::System.Windows.Forms.Label labBandWidth;
        private global::System.Windows.Forms.Label label34;
        private global::System.Windows.Forms.Label label35;
        private global::System.Windows.Forms.Label label36;
        private global::SDRSharp.PanView.Scope scope;
        private global::SDRSharp.CollapsiblePanel.CollapsiblePanel scopeCollapsiblePanel;
        private global::System.Windows.Forms.Label label27;
        private global::System.Windows.Forms.Label labTbGain;
        private global::System.Windows.Forms.Label labTbAverage;
        private global::System.Windows.Forms.Label label28;
        private global::System.Windows.Forms.Label labSpeed;
        private global::SDRSharp.PanView.Indicator frequencyNumericUpDown;
        private global::System.Windows.Forms.ContextMenuStrip MnuSA;
        private global::System.Windows.Forms.ToolStripMenuItem mnuVfoA;
        private global::System.Windows.Forms.ToolStripMenuItem mnuVfoB;
        private global::System.Windows.Forms.ToolStripMenuItem mnuVfoC;
        private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private global::System.Windows.Forms.ToolStripMenuItem mnuStationList;
        private global::System.Windows.Forms.ToolStripMenuItem mnuAutoScale;
        private global::System.Windows.Forms.Label label20;
        private global::System.Windows.Forms.Label label9;
        private global::System.Windows.Forms.Label label2;
        private global::System.Windows.Forms.Label label33;
        private global::System.Windows.Forms.Label label39;
        private global::SDRSharp.Controls.gButton playStopButton;
        private global::SDRSharp.Controls.gSliderH tbRFgain;
        private global::SDRSharp.Controls.gButton configureSourceButton;
        private global::SDRSharp.Controls.gCombo iqSourceComboBox;
        private global::SDRSharp.Controls.gButton chkLock;
        private global::SDRSharp.Controls.gCombo cmbBandWidth;
        private global::SDRSharp.Controls.gSliderH audioGainTrackBar;
        private global::SDRSharp.Controls.gButton amRadioButton;
        private global::SDRSharp.Controls.gButton dsbRadioButton;
        private global::SDRSharp.Controls.gCombo cmbCenterStep;
        private global::SDRSharp.Controls.gCombo stepSizeComboBox;
        private global::SDRSharp.Controls.gButton wfmRadioButton;
        private global::SDRSharp.Controls.gButton nfmRadioButton;
        private global::SDRSharp.Controls.gButton rawRadioButton;
        private global::SDRSharp.Controls.gButton usbRadioButton;
        private global::SDRSharp.Controls.gButton cwRadioButton;
        private global::SDRSharp.Controls.gButton lsbRadioButton;
        private global::SDRSharp.Controls.gButton swapIQCheckBox;
        private global::SDRSharp.Controls.gSliderH fftZoomTrackBar;
        private global::SDRSharp.Controls.gButton chkVinvert;
        private global::SDRSharp.Controls.gCombo cmbTim;
        private global::SDRSharp.Controls.gCombo cmbHor;
        private global::SDRSharp.Controls.gCombo cmbVer;
        private global::SDRSharp.Controls.gButton chkXY;
        private global::SDRSharp.Controls.gCombo cmbHchannel;
        private global::SDRSharp.Controls.gCombo cmbVchannel;
        private global::SDRSharp.Controls.gButton chkHrunDC;
        private global::SDRSharp.Controls.gButton chkVrunDC;
        private global::SDRSharp.Controls.gButton chkHinvert;
        private global::System.Windows.Forms.Panel pnlScroll;
        private global::SDRSharp.Controls.gButton gThumb;
        private global::SDRSharp.Controls.gSliderV tbFloorSA;
        private global::SDRSharp.Controls.gSliderV tbSpanSA;
        private global::SDRSharp.Controls.gSliderV fftSpeedTrackBar;
        private global::SDRSharp.Controls.gSliderV tbIntensityWv;
        private global::SDRSharp.Controls.gSliderV tbContrastWv;
        private global::SDRSharp.Controls.gCombo filterTypeComboBox;
        private global::SDRSharp.Controls.gCombo outputDeviceComboBox;
        private global::SDRSharp.Controls.gCombo inputDeviceComboBox;
        private global::SDRSharp.Controls.gCombo sampleRateComboBox;
        private global::SDRSharp.Controls.gButton chkNotch3;
        private global::SDRSharp.Controls.gButton chkNotch2;
        private global::SDRSharp.Controls.gButton chkNotch1;
        private global::SDRSharp.Controls.gButton chkNotch0;
        private global::SDRSharp.Controls.gButton chkAver;
        private global::SDRSharp.Controls.gButton fmStereoCheckBox;
        private global::SDRSharp.Controls.gButton filterAudioCheckBox;
        private global::SDRSharp.Controls.gButton chkFastConv;
        private global::SDRSharp.Controls.gButton chk1;
        private global::SDRSharp.Controls.gUpDown latencyNumericUpDown;
        private global::SDRSharp.Controls.gUpDown cwShiftNumericUpDown;
        private global::SDRSharp.Controls.gUpDown filterOrderNumericUpDown;
        private global::SDRSharp.Controls.gUpDown filterBandwidthNumericUpDown;
        private global::SDRSharp.Controls.gButton agcUseHangCheckBox;
        private global::SDRSharp.Controls.gUpDown agcSlopeNumericUpDown;
        private global::SDRSharp.Controls.gSliderH tbNLTreshold;
        private global::SDRSharp.Controls.gSliderH tbNLRatio;
        private global::SDRSharp.Controls.gButton chkNLimiter;
        private global::SDRSharp.Controls.gSliderH sAttackTrackBar;
        private global::SDRSharp.Controls.gUpDown frequencyShiftNumericUpDown;
        private global::SDRSharp.Controls.gButton frequencyShiftCheckBox;
        private global::SDRSharp.Controls.gButton gradientButtonSA;
        private global::SDRSharp.Controls.gButton useTimestampsCheckBox;
        private global::SDRSharp.Controls.gButton chkIndepSideband;
        private global::SDRSharp.Controls.gButton correctIQCheckBox;
        private global::SDRSharp.Controls.gSliderH wDecayTrackBar;
        private global::SDRSharp.Controls.gSliderH wAttackTrackBar;
        private global::SDRSharp.Controls.gSliderH sDecayTrackBar;
        private global::SDRSharp.Controls.gSliderH tbTrigL;
        private global::SDRSharp.Controls.gCombo fftWindowComboBox;
        private global::SDRSharp.Controls.gCombo fftResolutionComboBox;
        private global::SDRSharp.Controls.gSliderH tbGain;
        private global::SDRSharp.Controls.gSliderH tbAverage;
        private global::SDRSharp.Controls.gSliderV tbvPeakDelay;
        private global::SDRSharp.Controls.gSliderV tbvAudioAvg;
        private global::SDRSharp.Controls.gSliderV tbvAudioRel;
        private global::SDRSharp.Controls.gSliderV tbvCarrierAvg;
        private global::System.Windows.Forms.Label label43;
        private global::System.Windows.Forms.Label label42;
        private global::System.Windows.Forms.Label label41;
        private global::System.Windows.Forms.Label label40;
        private global::System.Windows.Forms.Label label19;
        private global::SDRSharp.Controls.gSliderV tbvPeakRel;
        private global::System.Windows.Forms.Label label48;
        private global::System.Windows.Forms.Label label47;
        private global::System.Windows.Forms.Label label46;
        private global::System.Windows.Forms.Label label45;
        private global::System.Windows.Forms.Label labFast;
        private global::SDRSharp.Controls.gButton gBexpandScope;
        private global::SDRSharp.Controls.gButton gBsetScale;
        private global::System.Windows.Forms.SplitContainer panSplitContainer2;
        private global::SDRSharp.PanView.Waterfall audiogram;
        private global::SDRSharp.Controls.gSliderV tbContrastAg;
        private global::SDRSharp.Controls.gSliderV tbIntensityAg;
        private global::SDRSharp.Controls.gSliderV tbAgSpeed;
        private global::System.Windows.Forms.Label label49;
        private global::System.Windows.Forms.Label label44;
        private global::System.Windows.Forms.Label label50;
        private global::System.Windows.Forms.Label label51;
        private global::System.Windows.Forms.ToolStripMenuItem mnuSetNotch;
        private global::System.Windows.Forms.ToolStripMenuItem MnuClearNotch;
        private global::SDRSharp.PanView.Scope wideScope;
        private global::System.Windows.Forms.Panel panelAG;
        private global::System.Windows.Forms.Label label53;
        private global::System.Windows.Forms.Label label56;
        private global::System.Windows.Forms.Label agcDecayLabel;
        private global::SDRSharp.Controls.gSliderH agcDecayNumericUpDown;
        private global::SDRSharp.Controls.gUpDown agcThresholdNumericUpDown;
        private global::System.Windows.Forms.Label labNLTreshold;
        private global::System.Windows.Forms.Label labNLRatio;
        private global::SDRSharp.CollapsiblePanel.CollapsiblePanel AdvancedCollapsiblePanel;
        private global::System.Windows.Forms.Panel panel2;
        private global::SDRSharp.Controls.gUpDown dbmOffsetUpDown;
        private global::System.Windows.Forms.Label labSmeter;
        private global::System.Windows.Forms.Label labFftAverage;
        private global::SDRSharp.Controls.gButton btnShowLog;
        private global::System.Windows.Forms.SplitContainer panSplitContainer3;
        private global::SDRSharp.PanView.SpectrumAnalyzer ifAnalyzer;
        private global::SDRSharp.Controls.gButton chkIF;
        private global::SDRSharp.Controls.gButton chkWF;
        private global::System.Windows.Forms.Label label4;
        private global::System.Windows.Forms.Label label7;
        private global::System.Windows.Forms.Label label54;
        private global::System.Windows.Forms.Label label55;
        private global::SDRSharp.Controls.gCombo fftZoomCombo;
        private global::SDRSharp.PanView.SpectrumAnalyzer afAnalyzer;
        private global::SDRSharp.Controls.gCombo cmbAudio;
        private global::System.Windows.Forms.Label label13;
        private global::System.Windows.Forms.ToolStripMenuItem setColoursToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private global::System.Windows.Forms.ToolStripMenuItem mnuShowWaterfall;
        private global::System.Windows.Forms.ToolStripMenuItem mnuShowBaseband;
        private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private global::SDRSharp.Controls.gButton samRadioButton;
        private global::SDRSharp.Controls.gCombo cmbDbm;
        private global::SDRSharp.Controls.gUpDown squelchNumericUpDown;
        private global::SDRSharp.Controls.gButton snapFrequencyCheckBox;
        private global::SDRSharp.Controls.gButton useSquelchCheckBox;
        private global::SDRSharp.Controls.gButton agcCheckBox;
        private global::System.Windows.Forms.Panel panel1;
        private global::SDRSharp.Controls.gButton butVfoC;
        private global::SDRSharp.Controls.gButton butVfoB;
        private global::SDRSharp.Controls.gButton butVfoA;
        private global::System.Windows.Forms.Label label38;
        private global::System.Windows.Forms.Label label37;
        private global::SDRSharp.Controls.gButton gBexpand;
        private global::System.Windows.Forms.ToolTip toolTip;
        private global::SDRSharp.Controls.gCombo bftResolutionComboBox;
        private global::System.Windows.Forms.Label label59;
        private global::System.Windows.Forms.Label label58;
        private global::System.Windows.Forms.Label label57;
        private global::System.Windows.Forms.Label label52;
        private global::SDRSharp.Controls.gButton chkBaseBand;
        private global::SDRSharp.Controls.gUpDown centerFreqNumericUpDown;
        private global::SDRSharp.Controls.gUpDown fftAverageUpDown;
        private global::SDRSharp.Controls.gButton chkAutoSize;
        private global::SDRSharp.Controls.gProgress playBar;
        private global::System.Windows.Forms.Label label60;
        private global::SDRSharp.Controls.gButton markPeaksCheckBox;
        private global::System.Windows.Forms.Label labProcessTmr;
        private global::System.Windows.Forms.Label labPerformTmr;
        private global::System.Windows.Forms.Label labFftTmr;
        private global::SDRSharp.Controls.gLabel labCPU;
        private global::SDRSharp.Controls.gLabel labSpectrum;
        private global::System.Windows.Forms.SplitContainer panSplitContainer4;
        private global::SDRSharp.PanView.Waterfall ifWaterfall;
        private global::System.Windows.Forms.SplitContainer panSplitContainer5;
        private global::SDRSharp.PanView.Waterfall afWaterfall;
        private global::System.Windows.Forms.ToolStripMenuItem mnuShowAudio;
        private global::System.Windows.Forms.ToolStripMenuItem mnuShowAudiogram;
        private global::System.Windows.Forms.ToolStripMenuItem mnuShowEnvelope;
        private global::SDRSharp.Controls.gButton fastFftCheckBox;
        private global::SDRSharp.Radio.BarMeter barMeter;
        private global::SDRSharp.Controls.gButton audioButton;
        private global::SDRSharp.Controls.gSliderH remDcSlider;
        private global::System.Windows.Forms.Label label3;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.UI.RadCheckBox chkScrollPanel;
        private Controls.gButton gButton1;
    }
}
