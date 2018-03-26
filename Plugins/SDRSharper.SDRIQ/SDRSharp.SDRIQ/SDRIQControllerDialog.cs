using SDRSharp.Radio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SDRSharp.SDRIQ
{
	public class SDRIQControllerDialog : Form
	{
		private readonly SdrIqIO _owner;

		private readonly bool _initialized;

		private IContainer components = null;

		private ComboBox deviceComboBox;

		private Label label1;

		private Label label3;

		private ComboBox samplerateComboBox;

		private TrackBar rfGainTrackBar;

		private Label label2;

		private TrackBar ifGainTrackBar;

		private Label label4;

		private Label ifGainLabel;

		private Label rfGainLabel;

		private Timer refreshTimer;

		public SDRIQControllerDialog(SdrIqIO owner)
		{
			this.InitializeComponent();
			this._owner = owner;
			DeviceDisplay[] activeDevices = DeviceDisplay.GetActiveDevices();
			this.deviceComboBox.Items.Clear();
			this.deviceComboBox.Items.AddRange(activeDevices);
			this.samplerateComboBox.SelectedIndex = Utils.GetIntSetting("SDRIQSampleRate", 5);
			this.ifGainTrackBar.Value = Utils.GetIntSetting("SDRIQIFGain", 5);
			this.rfGainTrackBar.Value = Utils.GetIntSetting("SDRIQRFGain", 2);
			this._initialized = true;
		}

		private void SDRIQControllerDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			base.Hide();
		}

		public void ConfigureGUI()
		{
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
			this.ifGainTrackBar_Scroll(null, null);
			this.rfGainTrackBar_Scroll(null, null);
		}

		private void samplerateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				string s = this.samplerateComboBox.Items[this.samplerateComboBox.SelectedIndex].ToString().Split(' ')[0];
				uint samplerate = uint.Parse(s, CultureInfo.InvariantCulture);
				this._owner.Device.Samplerate = samplerate;
				Utils.SaveSetting("SDRIQSampleRate", this.samplerateComboBox.SelectedIndex);
			}
		}

		private void rfGainTrackBar_Scroll(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				int num = (this.rfGainTrackBar.Maximum - this.rfGainTrackBar.Value) * -10;
				this._owner.Device.RfGain = (sbyte)num;
				this.rfGainLabel.Text = num + " dB";
				Utils.SaveSetting("SDRIQRFGain", this.rfGainTrackBar.Value);
			}
		}

		private void ifGainTrackBar_Scroll(object sender, EventArgs e)
		{
			if (this._initialized)
			{
				int num = (sbyte)this.ifGainTrackBar.Value * 6;
				this._owner.Device.IfGain = (sbyte)num;
				this.ifGainLabel.Text = num + " dB";
				Utils.SaveSetting("SDRIQIFGain", this.ifGainTrackBar.Value);
			}
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

		private void SDRIQControllerDialog_VisibleChanged(object sender, EventArgs e)
		{
			this.refreshTimer.Enabled = base.Visible;
			if (base.Visible)
			{
				this.samplerateComboBox.Enabled = !this._owner.Device.IsStreaming;
				this.deviceComboBox.Enabled = !this._owner.Device.IsStreaming;
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
					this.deviceComboBox.SelectedIndex = num;
				}
			}
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			this.samplerateComboBox.Enabled = !this._owner.Device.IsStreaming;
			this.deviceComboBox.Enabled = !this._owner.Device.IsStreaming;
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
			this.deviceComboBox = new ComboBox();
			this.label1 = new Label();
			this.label3 = new Label();
			this.samplerateComboBox = new ComboBox();
			this.rfGainTrackBar = new TrackBar();
			this.label2 = new Label();
			this.ifGainTrackBar = new TrackBar();
			this.label4 = new Label();
			this.ifGainLabel = new Label();
			this.rfGainLabel = new Label();
			this.refreshTimer = new Timer(this.components);
			((ISupportInitialize)this.rfGainTrackBar).BeginInit();
			((ISupportInitialize)this.ifGainTrackBar).BeginInit();
			base.SuspendLayout();
			this.deviceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.deviceComboBox.FormattingEnabled = true;
			this.deviceComboBox.Location = new Point(12, 26);
			this.deviceComboBox.Name = "deviceComboBox";
			this.deviceComboBox.Size = new Size(237, 21);
			this.deviceComboBox.TabIndex = 1;
			this.deviceComboBox.SelectedIndexChanged += this.deviceComboBox_SelectedIndexChanged;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(41, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "Device";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(12, 62);
			this.label3.Name = "label3";
			this.label3.Size = new Size(68, 13);
			this.label3.TabIndex = 26;
			this.label3.Text = "Sample Rate";
			this.samplerateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.samplerateComboBox.FormattingEnabled = true;
			this.samplerateComboBox.Items.AddRange(new object[7]
			{
				"8138 Hz",
				"16276 Hz",
				"37793 Hz",
				"55556 Hz ",
				"111111 Hz",
				"158730 Hz",
				"196078 Hz"
			});
			this.samplerateComboBox.Location = new Point(12, 79);
			this.samplerateComboBox.Name = "samplerateComboBox";
			this.samplerateComboBox.Size = new Size(237, 21);
			this.samplerateComboBox.TabIndex = 25;
			this.samplerateComboBox.SelectedIndexChanged += this.samplerateComboBox_SelectedIndexChanged;
			this.rfGainTrackBar.LargeChange = 1;
			this.rfGainTrackBar.Location = new Point(12, 138);
			this.rfGainTrackBar.Maximum = 3;
			this.rfGainTrackBar.Name = "rfGainTrackBar";
			this.rfGainTrackBar.Size = new Size(237, 45);
			this.rfGainTrackBar.TabIndex = 27;
			this.rfGainTrackBar.Scroll += this.rfGainTrackBar_Scroll;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 122);
			this.label2.Name = "label2";
			this.label2.Size = new Size(46, 13);
			this.label2.TabIndex = 28;
			this.label2.Text = "RF Gain";
			this.ifGainTrackBar.LargeChange = 1;
			this.ifGainTrackBar.Location = new Point(12, 204);
			this.ifGainTrackBar.Maximum = 5;
			this.ifGainTrackBar.Name = "ifGainTrackBar";
			this.ifGainTrackBar.Size = new Size(237, 45);
			this.ifGainTrackBar.TabIndex = 29;
			this.ifGainTrackBar.Scroll += this.ifGainTrackBar_Scroll;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(12, 188);
			this.label4.Name = "label4";
			this.label4.Size = new Size(41, 13);
			this.label4.TabIndex = 30;
			this.label4.Text = "IF Gain";
			this.ifGainLabel.AutoSize = true;
			this.ifGainLabel.Location = new Point(199, 188);
			this.ifGainLabel.Name = "ifGainLabel";
			this.ifGainLabel.Size = new Size(44, 13);
			this.ifGainLabel.TabIndex = 31;
			this.ifGainLabel.Text = "1000dB";
			this.rfGainLabel.AutoSize = true;
			this.rfGainLabel.Location = new Point(199, 122);
			this.rfGainLabel.Name = "rfGainLabel";
			this.rfGainLabel.Size = new Size(44, 13);
			this.rfGainLabel.TabIndex = 32;
			this.rfGainLabel.Text = "1000dB";
			this.refreshTimer.Interval = 1000;
			this.refreshTimer.Tick += this.refreshTimer_Tick;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(261, 261);
			base.Controls.Add(this.rfGainLabel);
			base.Controls.Add(this.ifGainLabel);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.ifGainTrackBar);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.rfGainTrackBar);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.samplerateComboBox);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.deviceComboBox);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SDRIQControllerDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "SDR-IQ Controller";
			base.TopMost = true;
			base.FormClosing += this.SDRIQControllerDialog_FormClosing;
			base.VisibleChanged += this.SDRIQControllerDialog_VisibleChanged;
			((ISupportInitialize)this.rfGainTrackBar).EndInit();
			((ISupportInitialize)this.ifGainTrackBar).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
