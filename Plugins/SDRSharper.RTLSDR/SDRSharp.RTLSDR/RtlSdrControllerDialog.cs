using SDRSharp.Radio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SDRSharp.RTLSDR
{
	public class RtlSdrControllerDialog : Form
	{
		private readonly RtlSdrIO _owner;

		private bool _initialized;

		private IContainer components;

		private Timer refreshTimer;

		private Button closeButton;

		private ComboBox deviceComboBox;

		private Label label1;

		private TrackBar tunerGainTrackBar;

		private Label label2;

		private Label label3;

		private ComboBox samplerateComboBox;

		private CheckBox tunerAgcCheckBox;

		private Label gainLabel;

		private NumericUpDown frequencyCorrectionNumericUpDown;

		private Label label4;

		private Label tunerTypeLabel;

		private CheckBox rtlAgcCheckBox;

		private Label label5;

		private ComboBox samplingModeComboBox;

		private CheckBox offsetTuningCheckBox;

		public RtlSdrControllerDialog(RtlSdrIO owner)
		{
			this.InitializeComponent();
			this._owner = owner;
			DeviceDisplay[] activeDevices = DeviceDisplay.GetActiveDevices();
			this.deviceComboBox.Items.Clear();
			this.deviceComboBox.Items.AddRange(activeDevices);
			this.frequencyCorrectionNumericUpDown.Value = Utils.GetIntSetting("RTLFrequencyCorrection", 0);
			this.samplerateComboBox.SelectedIndex = Utils.GetIntSetting("RTLSampleRate", 3);
			this.samplingModeComboBox.SelectedIndex = Utils.GetIntSetting("RTLSamplingMode", 0);
			this.offsetTuningCheckBox.Checked = Utils.GetBooleanSetting("RTLOffsetTuning");
			this.rtlAgcCheckBox.Checked = Utils.GetBooleanSetting("RTLChipAgc");
			this.tunerAgcCheckBox.Checked = Utils.GetBooleanSetting("RTLTunerAgc");
			this.tunerGainTrackBar.Value = Utils.GetIntSetting("RTLTunerGain", 0);
			this.tunerAgcCheckBox.Enabled = (this.samplingModeComboBox.SelectedIndex == 0);
			this.gainLabel.Visible = (this.tunerAgcCheckBox.Enabled && !this.tunerAgcCheckBox.Checked);
			this.tunerGainTrackBar.Enabled = (this.tunerAgcCheckBox.Enabled && !this.tunerAgcCheckBox.Checked);
			this.offsetTuningCheckBox.Enabled = (this.samplingModeComboBox.SelectedIndex == 0);
			this._initialized = true;
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void RtlSdrControllerDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			base.Hide();
		}

		private void RtlSdrControllerDialog_VisibleChanged(object sender, EventArgs e)
		{
			this.refreshTimer.Enabled = base.Visible;
			if (base.Visible)
			{
				this.samplerateComboBox.Enabled = !this._owner.Device.IsStreaming;
				this.deviceComboBox.Enabled = !this._owner.Device.IsStreaming;
				this.samplingModeComboBox.Enabled = !this._owner.Device.IsStreaming;
				if (!this._owner.Device.IsStreaming)
				{
					DeviceDisplay[] activeDevices = DeviceDisplay.GetActiveDevices();
					this.deviceComboBox.Items.Clear();
					this.deviceComboBox.Items.AddRange(activeDevices);
					int num = 0;
					while (true)
					{
						if (num < activeDevices.Length)
						{
							if (activeDevices[num].Index != ((DeviceDisplay)this.deviceComboBox.Items[num]).Index)
							{
								num++;
								continue;
							}
							break;
						}
						return;
					}
					this._initialized = false;
					this.deviceComboBox.SelectedIndex = num;
					this._initialized = true;
				}
			}
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			this.samplerateComboBox.Enabled = !this._owner.Device.IsStreaming;
			this.deviceComboBox.Enabled = !this._owner.Device.IsStreaming;
			this.samplingModeComboBox.Enabled = !this._owner.Device.IsStreaming;
		}

		private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				DeviceDisplay deviceDisplay = (DeviceDisplay)this.deviceComboBox.SelectedItem;
				if (deviceDisplay != null)
				{
					try
					{
						this._owner.SelectDevice(deviceDisplay.Index);
					}
					catch (Exception ex)
					{
						this.deviceComboBox.SelectedIndex = -1;
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
			}
		}

		private void tunerGainTrackBar_Scroll(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				int num = this._owner.Device.SupportedGains[this.tunerGainTrackBar.Value];
				this._owner.Device.Gain = num;
				this.gainLabel.Text = (double)num / 10.0 + " dB";
				Utils.SaveSetting("RTLTunerGain", this.tunerGainTrackBar.Value);
			}
		}

		private void samplerateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				string s = this.samplerateComboBox.Items[this.samplerateComboBox.SelectedIndex].ToString().Split(' ')[0];
				double num = double.Parse(s, CultureInfo.InvariantCulture);
				this._owner.Device.Samplerate = (uint)(num * 1000000.0);
				Utils.SaveSetting("RTLSampleRate", this.samplerateComboBox.SelectedIndex);
			}
		}

		private void tunerAgcCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				this.tunerGainTrackBar.Enabled = (this.tunerAgcCheckBox.Enabled && !this.tunerAgcCheckBox.Checked);
				this._owner.Device.UseTunerAGC = this.tunerAgcCheckBox.Checked;
				this.gainLabel.Visible = (this.tunerAgcCheckBox.Enabled && !this.tunerAgcCheckBox.Checked);
				if (!this.tunerAgcCheckBox.Checked)
				{
					this.tunerGainTrackBar_Scroll(null, null);
				}
				Utils.SaveSetting("RTLTunerAgc", this.tunerAgcCheckBox.Checked);
			}
		}

		private void frequencyCorrectionNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				this._owner.Device.FrequencyCorrection = (int)this.frequencyCorrectionNumericUpDown.Value;
				Utils.SaveSetting("RTLFrequencyCorrection", this.frequencyCorrectionNumericUpDown.Value.ToString());
			}
		}

		private void rtlAgcCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				this._owner.Device.UseRtlAGC = this.rtlAgcCheckBox.Checked;
				Utils.SaveSetting("RTLChipAgc", this.rtlAgcCheckBox.Checked);
			}
		}

		private void samplingModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				this.tunerAgcCheckBox.Enabled = (this.samplingModeComboBox.SelectedIndex == 0);
				this.gainLabel.Visible = (this.tunerAgcCheckBox.Enabled && !this.tunerAgcCheckBox.Checked);
				this.tunerGainTrackBar.Enabled = (this.tunerAgcCheckBox.Enabled && !this.tunerAgcCheckBox.Checked);
				this.offsetTuningCheckBox.Enabled = (this.samplingModeComboBox.SelectedIndex == 0);
				if (this.samplingModeComboBox.SelectedIndex == 0)
				{
					this.offsetTuningCheckBox_CheckedChanged(null, null);
				}
				else
				{
					this._owner.Device.UseOffsetTuning = false;
				}
				this._owner.Device.SamplingMode = (SamplingMode)this.samplingModeComboBox.SelectedIndex;
				Utils.SaveSetting("RTLSamplingMode", this.samplingModeComboBox.SelectedIndex);
			}
		}

		private void offsetTuningCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				this._owner.Device.UseOffsetTuning = this.offsetTuningCheckBox.Checked;
				Utils.SaveSetting("RTLOffsetTuning", this.offsetTuningCheckBox.Checked);
			}
		}

		public void ConfigureGUI()
		{
			this.tunerTypeLabel.Text = this._owner.Device.TunerType.ToString();
			this.tunerGainTrackBar.Maximum = this._owner.Device.SupportedGains.Length - 1;
			this.offsetTuningCheckBox.Enabled = this._owner.Device.SupportsOffsetTuning;
			int num = 0;
			while (true)
			{
				if (num < this.deviceComboBox.Items.Count)
				{
					DeviceDisplay deviceDisplay = (DeviceDisplay)this.deviceComboBox.Items[num];
					if (deviceDisplay.Index != this._owner.Device.Index)
					{
						num++;
						continue;
					}
					break;
				}
				return;
			}
			this.deviceComboBox.SelectedIndex = num;
		}

		public void ConfigureDevice()
		{
			this.samplerateComboBox_SelectedIndexChanged(null, null);
			this.samplingModeComboBox_SelectedIndexChanged(null, null);
			this.offsetTuningCheckBox_CheckedChanged(null, null);
			this.frequencyCorrectionNumericUpDown_ValueChanged(null, null);
			this.rtlAgcCheckBox_CheckedChanged(null, null);
			this.tunerAgcCheckBox_CheckedChanged(null, null);
			if (!this.tunerAgcCheckBox.Checked)
			{
				this.tunerGainTrackBar_Scroll(null, null);
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
			this.refreshTimer = new Timer(this.components);
			this.closeButton = new Button();
			this.deviceComboBox = new ComboBox();
			this.label1 = new Label();
			this.tunerGainTrackBar = new TrackBar();
			this.label2 = new Label();
			this.label3 = new Label();
			this.samplerateComboBox = new ComboBox();
			this.tunerAgcCheckBox = new CheckBox();
			this.gainLabel = new Label();
			this.frequencyCorrectionNumericUpDown = new NumericUpDown();
			this.label4 = new Label();
			this.tunerTypeLabel = new Label();
			this.rtlAgcCheckBox = new CheckBox();
			this.label5 = new Label();
			this.samplingModeComboBox = new ComboBox();
			this.offsetTuningCheckBox = new CheckBox();
			((ISupportInitialize)this.tunerGainTrackBar).BeginInit();
			((ISupportInitialize)this.frequencyCorrectionNumericUpDown).BeginInit();
			base.SuspendLayout();
			this.refreshTimer.Interval = 1000;
			this.refreshTimer.Tick += this.refreshTimer_Tick;
			this.closeButton.DialogResult = DialogResult.Cancel;
			this.closeButton.Location = new Point(184, 306);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new Size(75, 23);
			this.closeButton.TabIndex = 8;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += this.closeButton_Click;
			this.deviceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.deviceComboBox.FormattingEnabled = true;
			this.deviceComboBox.Location = new Point(12, 26);
			this.deviceComboBox.Name = "deviceComboBox";
			this.deviceComboBox.Size = new Size(247, 21);
			this.deviceComboBox.TabIndex = 0;
			this.deviceComboBox.SelectedIndexChanged += this.deviceComboBox_SelectedIndexChanged;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(41, 13);
			this.label1.TabIndex = 20;
			this.label1.Text = "Device";
			this.tunerGainTrackBar.Enabled = false;
			this.tunerGainTrackBar.Location = new Point(3, 229);
			this.tunerGainTrackBar.Maximum = 10000;
			this.tunerGainTrackBar.Name = "tunerGainTrackBar";
			this.tunerGainTrackBar.Size = new Size(267, 45);
			this.tunerGainTrackBar.TabIndex = 6;
			this.tunerGainTrackBar.Scroll += this.tunerGainTrackBar_Scroll;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 213);
			this.label2.Name = "label2";
			this.label2.Size = new Size(46, 13);
			this.label2.TabIndex = 22;
			this.label2.Text = "RF Gain";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(12, 53);
			this.label3.Name = "label3";
			this.label3.Size = new Size(68, 13);
			this.label3.TabIndex = 24;
			this.label3.Text = "Sample Rate";
			this.samplerateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.samplerateComboBox.FormattingEnabled = true;
			this.samplerateComboBox.Items.AddRange(new object[10]
			{
				"3.2 MSPS",
				"2.8 MSPS",
				"2.4 MSPS",
				"2.048 MSPS",
				"1.92 MSPS",
				"1.8 MSPS",
				"1.4 MSPS",
				"1.024 MSPS",
				"0.900001 MSPS",
				"0.25 MSPS"
			});
			this.samplerateComboBox.Location = new Point(12, 70);
			this.samplerateComboBox.Name = "samplerateComboBox";
			this.samplerateComboBox.Size = new Size(247, 21);
			this.samplerateComboBox.TabIndex = 1;
			this.samplerateComboBox.SelectedIndexChanged += this.samplerateComboBox_SelectedIndexChanged;
			this.tunerAgcCheckBox.AutoSize = true;
			this.tunerAgcCheckBox.Checked = true;
			this.tunerAgcCheckBox.CheckState = CheckState.Checked;
			this.tunerAgcCheckBox.Location = new Point(12, 193);
			this.tunerAgcCheckBox.Name = "tunerAgcCheckBox";
			this.tunerAgcCheckBox.Size = new Size(79, 17);
			this.tunerAgcCheckBox.TabIndex = 5;
			this.tunerAgcCheckBox.Text = "Tuner AGC";
			this.tunerAgcCheckBox.UseVisualStyleBackColor = true;
			this.tunerAgcCheckBox.CheckedChanged += this.tunerAgcCheckBox_CheckedChanged;
			this.gainLabel.Location = new Point(191, 213);
			this.gainLabel.Name = "gainLabel";
			this.gainLabel.Size = new Size(68, 13);
			this.gainLabel.TabIndex = 26;
			this.gainLabel.Text = "1000dB";
			this.gainLabel.TextAlign = ContentAlignment.MiddleRight;
			this.gainLabel.Visible = false;
			this.frequencyCorrectionNumericUpDown.Location = new Point(169, 275);
			this.frequencyCorrectionNumericUpDown.Maximum = new decimal(new int[4]
			{
				1000,
				0,
				0,
				0
			});
			this.frequencyCorrectionNumericUpDown.Minimum = new decimal(new int[4]
			{
				1000,
				0,
				0,
				-2147483648
			});
			this.frequencyCorrectionNumericUpDown.Name = "frequencyCorrectionNumericUpDown";
			this.frequencyCorrectionNumericUpDown.Size = new Size(90, 20);
			this.frequencyCorrectionNumericUpDown.TabIndex = 7;
			this.frequencyCorrectionNumericUpDown.TextAlign = HorizontalAlignment.Right;
			this.frequencyCorrectionNumericUpDown.ValueChanged += this.frequencyCorrectionNumericUpDown_ValueChanged;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(12, 277);
			this.label4.Name = "label4";
			this.label4.Size = new Size(136, 13);
			this.label4.TabIndex = 28;
			this.label4.Text = "Frequency correction (ppm)";
			this.tunerTypeLabel.Location = new Point(166, 9);
			this.tunerTypeLabel.Name = "tunerTypeLabel";
			this.tunerTypeLabel.Size = new Size(93, 13);
			this.tunerTypeLabel.TabIndex = 29;
			this.tunerTypeLabel.Text = "E4000";
			this.tunerTypeLabel.TextAlign = ContentAlignment.MiddleRight;
			this.rtlAgcCheckBox.AutoSize = true;
			this.rtlAgcCheckBox.Location = new Point(12, 170);
			this.rtlAgcCheckBox.Name = "rtlAgcCheckBox";
			this.rtlAgcCheckBox.Size = new Size(72, 17);
			this.rtlAgcCheckBox.TabIndex = 4;
			this.rtlAgcCheckBox.Text = "RTL AGC";
			this.rtlAgcCheckBox.UseVisualStyleBackColor = true;
			this.rtlAgcCheckBox.CheckedChanged += this.rtlAgcCheckBox_CheckedChanged;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(12, 97);
			this.label5.Name = "label5";
			this.label5.Size = new Size(80, 13);
			this.label5.TabIndex = 31;
			this.label5.Text = "Sampling Mode";
			this.samplingModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.samplingModeComboBox.FormattingEnabled = true;
			this.samplingModeComboBox.Items.AddRange(new object[3]
			{
				"Quadrature sampling",
				"Direct sampling (I branch)",
				"Direct sampling (Q branch)"
			});
			this.samplingModeComboBox.Location = new Point(12, 114);
			this.samplingModeComboBox.Name = "samplingModeComboBox";
			this.samplingModeComboBox.Size = new Size(247, 21);
			this.samplingModeComboBox.TabIndex = 2;
			this.samplingModeComboBox.SelectedIndexChanged += this.samplingModeComboBox_SelectedIndexChanged;
			this.offsetTuningCheckBox.AutoSize = true;
			this.offsetTuningCheckBox.Location = new Point(12, 147);
			this.offsetTuningCheckBox.Name = "offsetTuningCheckBox";
			this.offsetTuningCheckBox.Size = new Size(90, 17);
			this.offsetTuningCheckBox.TabIndex = 3;
			this.offsetTuningCheckBox.Text = "Offset Tuning";
			this.offsetTuningCheckBox.UseVisualStyleBackColor = true;
			this.offsetTuningCheckBox.CheckedChanged += this.offsetTuningCheckBox_CheckedChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.closeButton;
			base.ClientSize = new Size(271, 342);
			base.Controls.Add(this.offsetTuningCheckBox);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.samplingModeComboBox);
			base.Controls.Add(this.rtlAgcCheckBox);
			base.Controls.Add(this.tunerTypeLabel);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.frequencyCorrectionNumericUpDown);
			base.Controls.Add(this.gainLabel);
			base.Controls.Add(this.tunerAgcCheckBox);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.samplerateComboBox);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.tunerGainTrackBar);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.deviceComboBox);
			base.Controls.Add(this.closeButton);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RtlSdrControllerDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "RTL-SDR Controller";
			base.TopMost = true;
			base.FormClosing += this.RtlSdrControllerDialog_FormClosing;
			base.VisibleChanged += this.RtlSdrControllerDialog_VisibleChanged;
			((ISupportInitialize)this.tunerGainTrackBar).EndInit();
			((ISupportInitialize)this.frequencyCorrectionNumericUpDown).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
