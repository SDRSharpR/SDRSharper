using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.FrequencyScanner
{
	public class DialogConfigure : Form
	{
		private FrequencyScannerPanel _frequencyScannerPanel;

		private IContainer components;

		private CheckBox autoSkipCheckBox;

		private CheckBox autoLockCheckBox;

		private CheckBox autoClearCheckBox;

		private NumericUpDown LockIntervalNumericUpDown;

		private NumericUpDown activityNumericUpDown;

		private NumericUpDown autoClearIntervalNumericUpDown;

		private NumericUpDown skipIntervalNumericUpDown;

		private Label label1;

		private Label label2;

		private Button okButton;

		private Label label3;

		private Label label4;

		private Label label5;

		private CheckBox channelDetectMetodCheckBox;

		private CheckBox useMuteCheckBox;

		private NumericUpDown unMuteNumericUpDown;

		private Label label7;

		private CheckBox maximumChannelCheckBox;

		private GroupBox groupBox2;

		private CheckBox debugCheckBox;

		private Label label8;

		private NumericUpDown maxAlphaNumericUpDown;

		private NumericUpDown minAlphaNumericUpDown;

		private Label label6;

		private Label label9;

		private Label label10;

		private ComboBox pluginPositionComboBox;

		public DialogConfigure(FrequencyScannerPanel frequencyScannerPanel)
		{
			this.InitializeComponent();
			this._frequencyScannerPanel = frequencyScannerPanel;
			this.autoSkipCheckBox.Checked = this._frequencyScannerPanel.AutoSkipEnabled;
			this.skipIntervalNumericUpDown.Value = this._frequencyScannerPanel.AutoSkipInterval;
			this.autoLockCheckBox.Checked = this._frequencyScannerPanel.AutoLockEnabled;
			this.LockIntervalNumericUpDown.Value = this._frequencyScannerPanel.AutoLockInterval;
			this.autoClearCheckBox.Checked = this._frequencyScannerPanel.AutoClearEnabled;
			this.activityNumericUpDown.Value = (decimal)this._frequencyScannerPanel.AutoClearActivityLevel;
			this.autoClearIntervalNumericUpDown.Value = this._frequencyScannerPanel.AutoClearInterval;
			this.channelDetectMetodCheckBox.Checked = this._frequencyScannerPanel.ChannelDetectMetod;
			this.useMuteCheckBox.Checked = this._frequencyScannerPanel.UseMute;
			this.unMuteNumericUpDown.Value = this._frequencyScannerPanel.UnMuteDelay;
			this.maximumChannelCheckBox.Checked = this._frequencyScannerPanel.MaxLevelSelect;
			this.debugCheckBox.Checked = this._frequencyScannerPanel.ShowDebugInfo;
			this.maxAlphaNumericUpDown.Value = this._frequencyScannerPanel.MaxButtonsAlpha;
			this.minAlphaNumericUpDown.Value = this._frequencyScannerPanel.MinButtonsAlpha;
			this.pluginPositionComboBox.SelectedIndex = this._frequencyScannerPanel.ScannerPluginPosition;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this._frequencyScannerPanel.AutoSkipEnabled = this.autoSkipCheckBox.Checked;
			this._frequencyScannerPanel.AutoSkipInterval = (int)this.skipIntervalNumericUpDown.Value;
			this._frequencyScannerPanel.AutoLockEnabled = this.autoLockCheckBox.Checked;
			this._frequencyScannerPanel.AutoLockInterval = (int)this.LockIntervalNumericUpDown.Value;
			this._frequencyScannerPanel.AutoClearEnabled = this.autoClearCheckBox.Checked;
			this._frequencyScannerPanel.AutoClearActivityLevel = (float)this.activityNumericUpDown.Value;
			this._frequencyScannerPanel.AutoClearInterval = (int)this.autoClearIntervalNumericUpDown.Value;
			this._frequencyScannerPanel.ChannelDetectMetod = this.channelDetectMetodCheckBox.Checked;
			this._frequencyScannerPanel.UseMute = this.useMuteCheckBox.Checked;
			this._frequencyScannerPanel.UnMuteDelay = (int)this.unMuteNumericUpDown.Value;
			this._frequencyScannerPanel.MaxLevelSelect = this.maximumChannelCheckBox.Checked;
			this._frequencyScannerPanel.MaxButtonsAlpha = (int)this.maxAlphaNumericUpDown.Value;
			this._frequencyScannerPanel.MinButtonsAlpha = (int)this.minAlphaNumericUpDown.Value;
			this._frequencyScannerPanel.ScannerPluginPosition = this.pluginPositionComboBox.SelectedIndex;
		}

		private void useMuteCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this._frequencyScannerPanel.UseMute = this.useMuteCheckBox.Checked;
			this.unMuteNumericUpDown.Enabled = this.useMuteCheckBox.Checked;
		}

		private void unMuteNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			this._frequencyScannerPanel.UnMuteDelay = (int)this.unMuteNumericUpDown.Value;
		}

		private void debugCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this._frequencyScannerPanel.ShowDebugInfo = this.debugCheckBox.Checked;
		}

		private void minAlphaNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			this._frequencyScannerPanel.MinButtonsAlpha = (int)this.minAlphaNumericUpDown.Value;
		}

		private void maxAlphaNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			this._frequencyScannerPanel.MaxButtonsAlpha = (int)this.maxAlphaNumericUpDown.Value;
		}

		private void label10_Click(object sender, EventArgs e)
		{
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
			this.autoSkipCheckBox = new CheckBox();
			this.autoLockCheckBox = new CheckBox();
			this.autoClearCheckBox = new CheckBox();
			this.LockIntervalNumericUpDown = new NumericUpDown();
			this.activityNumericUpDown = new NumericUpDown();
			this.autoClearIntervalNumericUpDown = new NumericUpDown();
			this.skipIntervalNumericUpDown = new NumericUpDown();
			this.label1 = new Label();
			this.label2 = new Label();
			this.okButton = new Button();
			this.label3 = new Label();
			this.label4 = new Label();
			this.label5 = new Label();
			this.channelDetectMetodCheckBox = new CheckBox();
			this.useMuteCheckBox = new CheckBox();
			this.unMuteNumericUpDown = new NumericUpDown();
			this.label7 = new Label();
			this.maximumChannelCheckBox = new CheckBox();
			this.groupBox2 = new GroupBox();
			this.label9 = new Label();
			this.label8 = new Label();
			this.maxAlphaNumericUpDown = new NumericUpDown();
			this.minAlphaNumericUpDown = new NumericUpDown();
			this.label6 = new Label();
			this.debugCheckBox = new CheckBox();
			this.label10 = new Label();
			this.pluginPositionComboBox = new ComboBox();
			((ISupportInitialize)this.LockIntervalNumericUpDown).BeginInit();
			((ISupportInitialize)this.activityNumericUpDown).BeginInit();
			((ISupportInitialize)this.autoClearIntervalNumericUpDown).BeginInit();
			((ISupportInitialize)this.skipIntervalNumericUpDown).BeginInit();
			((ISupportInitialize)this.unMuteNumericUpDown).BeginInit();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.maxAlphaNumericUpDown).BeginInit();
			((ISupportInitialize)this.minAlphaNumericUpDown).BeginInit();
			base.SuspendLayout();
			this.autoSkipCheckBox.AutoSize = true;
			this.autoSkipCheckBox.Location = new Point(6, 19);
			this.autoSkipCheckBox.Name = "autoSkipCheckBox";
			this.autoSkipCheckBox.Size = new Size(70, 17);
			this.autoSkipCheckBox.TabIndex = 0;
			this.autoSkipCheckBox.Text = "Auto skip";
			this.autoSkipCheckBox.UseVisualStyleBackColor = true;
			this.autoLockCheckBox.AutoSize = true;
			this.autoLockCheckBox.Location = new Point(6, 42);
			this.autoLockCheckBox.Name = "autoLockCheckBox";
			this.autoLockCheckBox.Size = new Size(71, 17);
			this.autoLockCheckBox.TabIndex = 1;
			this.autoLockCheckBox.Text = "Auto lock";
			this.autoLockCheckBox.UseVisualStyleBackColor = true;
			this.autoClearCheckBox.AutoSize = true;
			this.autoClearCheckBox.Location = new Point(6, 65);
			this.autoClearCheckBox.Name = "autoClearCheckBox";
			this.autoClearCheckBox.Size = new Size(15, 14);
			this.autoClearCheckBox.TabIndex = 2;
			this.autoClearCheckBox.UseVisualStyleBackColor = true;
			this.LockIntervalNumericUpDown.Location = new Point(82, 41);
			this.LockIntervalNumericUpDown.Maximum = new decimal(new int[4]
			{
				3600,
				0,
				0,
				0
			});
			this.LockIntervalNumericUpDown.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.LockIntervalNumericUpDown.Name = "LockIntervalNumericUpDown";
			this.LockIntervalNumericUpDown.Size = new Size(55, 20);
			this.LockIntervalNumericUpDown.TabIndex = 3;
			this.LockIntervalNumericUpDown.Value = new decimal(new int[4]
			{
				30,
				0,
				0,
				0
			});
			this.activityNumericUpDown.DecimalPlaces = 1;
			this.activityNumericUpDown.Increment = new decimal(new int[4]
			{
				1,
				0,
				0,
				65536
			});
			this.activityNumericUpDown.Location = new Point(166, 63);
			this.activityNumericUpDown.Maximum = new decimal(new int[4]
			{
				1000,
				0,
				0,
				0
			});
			this.activityNumericUpDown.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				65536
			});
			this.activityNumericUpDown.Name = "activityNumericUpDown";
			this.activityNumericUpDown.Size = new Size(47, 20);
			this.activityNumericUpDown.TabIndex = 4;
			this.activityNumericUpDown.Value = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.autoClearIntervalNumericUpDown.Location = new Point(258, 63);
			this.autoClearIntervalNumericUpDown.Maximum = new decimal(new int[4]
			{
				1000,
				0,
				0,
				0
			});
			this.autoClearIntervalNumericUpDown.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.autoClearIntervalNumericUpDown.Name = "autoClearIntervalNumericUpDown";
			this.autoClearIntervalNumericUpDown.Size = new Size(43, 20);
			this.autoClearIntervalNumericUpDown.TabIndex = 5;
			this.autoClearIntervalNumericUpDown.Value = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.skipIntervalNumericUpDown.Location = new Point(82, 18);
			this.skipIntervalNumericUpDown.Maximum = new decimal(new int[4]
			{
				3600,
				0,
				0,
				0
			});
			this.skipIntervalNumericUpDown.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.skipIntervalNumericUpDown.Name = "skipIntervalNumericUpDown";
			this.skipIntervalNumericUpDown.Size = new Size(55, 20);
			this.skipIntervalNumericUpDown.TabIndex = 6;
			this.skipIntervalNumericUpDown.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.label1.AutoSize = true;
			this.label1.Location = new Point(27, 65);
			this.label1.Name = "label1";
			this.label1.Size = new Size(133, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Delete rows with activity  <";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(219, 65);
			this.label2.Name = "label2";
			this.label2.Size = new Size(33, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "every";
			this.okButton.DialogResult = DialogResult.OK;
			this.okButton.Location = new Point(304, 253);
			this.okButton.Name = "okButton";
			this.okButton.Size = new Size(75, 23);
			this.okButton.TabIndex = 9;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += this.okButton_Click;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(143, 20);
			this.label3.Name = "label3";
			this.label3.Size = new Size(47, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "seconds";
			this.label4.AutoSize = true;
			this.label4.Location = new Point(144, 43);
			this.label4.Name = "label4";
			this.label4.Size = new Size(47, 13);
			this.label4.TabIndex = 11;
			this.label4.Text = "seconds";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(307, 65);
			this.label5.Name = "label5";
			this.label5.Size = new Size(47, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "seconds";
			this.channelDetectMetodCheckBox.AutoSize = true;
			this.channelDetectMetodCheckBox.Location = new Point(6, 89);
			this.channelDetectMetodCheckBox.Name = "channelDetectMetodCheckBox";
			this.channelDetectMetodCheckBox.Size = new Size(310, 17);
			this.channelDetectMetodCheckBox.TabIndex = 13;
			this.channelDetectMetodCheckBox.Text = "Detect channel only center frequency (default all bandwidth)";
			this.channelDetectMetodCheckBox.UseVisualStyleBackColor = true;
			this.useMuteCheckBox.AutoSize = true;
			this.useMuteCheckBox.Location = new Point(6, 135);
			this.useMuteCheckBox.Name = "useMuteCheckBox";
			this.useMuteCheckBox.Size = new Size(100, 17);
			this.useMuteCheckBox.TabIndex = 16;
			this.useMuteCheckBox.Text = "Use audio mute";
			this.useMuteCheckBox.UseVisualStyleBackColor = true;
			this.useMuteCheckBox.CheckedChanged += this.useMuteCheckBox_CheckedChanged;
			this.unMuteNumericUpDown.Location = new Point(230, 134);
			this.unMuteNumericUpDown.Maximum = new decimal(new int[4]
			{
				20,
				0,
				0,
				0
			});
			this.unMuteNumericUpDown.Name = "unMuteNumericUpDown";
			this.unMuteNumericUpDown.Size = new Size(48, 20);
			this.unMuteNumericUpDown.TabIndex = 17;
			this.unMuteNumericUpDown.Value = new decimal(new int[4]
			{
				5,
				0,
				0,
				0
			});
			this.unMuteNumericUpDown.ValueChanged += this.unMuteNumericUpDown_ValueChanged;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(112, 136);
			this.label7.Name = "label7";
			this.label7.Size = new Size(112, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "Noise protection delay";
			this.maximumChannelCheckBox.AutoSize = true;
			this.maximumChannelCheckBox.Location = new Point(6, 112);
			this.maximumChannelCheckBox.Name = "maximumChannelCheckBox";
			this.maximumChannelCheckBox.Size = new Size(190, 17);
			this.maximumChannelCheckBox.TabIndex = 23;
			this.maximumChannelCheckBox.Text = "Select channel with maximum level";
			this.maximumChannelCheckBox.UseVisualStyleBackColor = true;
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.pluginPositionComboBox);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.maxAlphaNumericUpDown);
			this.groupBox2.Controls.Add(this.minAlphaNumericUpDown);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.debugCheckBox);
			this.groupBox2.Controls.Add(this.autoSkipCheckBox);
			this.groupBox2.Controls.Add(this.autoLockCheckBox);
			this.groupBox2.Controls.Add(this.channelDetectMetodCheckBox);
			this.groupBox2.Controls.Add(this.maximumChannelCheckBox);
			this.groupBox2.Controls.Add(this.autoClearCheckBox);
			this.groupBox2.Controls.Add(this.useMuteCheckBox);
			this.groupBox2.Controls.Add(this.LockIntervalNumericUpDown);
			this.groupBox2.Controls.Add(this.unMuteNumericUpDown);
			this.groupBox2.Controls.Add(this.activityNumericUpDown);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.autoClearIntervalNumericUpDown);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.skipIntervalNumericUpDown);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(370, 235);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Scanner settings";
			this.label9.AutoSize = true;
			this.label9.Location = new Point(80, 182);
			this.label9.Name = "label9";
			this.label9.Size = new Size(26, 13);
			this.label9.TabIndex = 31;
			this.label9.Text = "min.";
			this.label8.AutoSize = true;
			this.label8.Location = new Point(6, 182);
			this.label8.Name = "label8";
			this.label8.Size = new Size(72, 13);
			this.label8.TabIndex = 30;
			this.label8.Text = "Buttons alpha";
			this.maxAlphaNumericUpDown.Location = new Point(208, 180);
			this.maxAlphaNumericUpDown.Maximum = new decimal(new int[4]
			{
				255,
				0,
				0,
				0
			});
			this.maxAlphaNumericUpDown.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.maxAlphaNumericUpDown.Name = "maxAlphaNumericUpDown";
			this.maxAlphaNumericUpDown.Size = new Size(55, 20);
			this.maxAlphaNumericUpDown.TabIndex = 27;
			this.maxAlphaNumericUpDown.Value = new decimal(new int[4]
			{
				150,
				0,
				0,
				0
			});
			this.maxAlphaNumericUpDown.ValueChanged += this.maxAlphaNumericUpDown_ValueChanged;
			this.minAlphaNumericUpDown.Location = new Point(112, 180);
			this.minAlphaNumericUpDown.Maximum = new decimal(new int[4]
			{
				255,
				0,
				0,
				0
			});
			this.minAlphaNumericUpDown.Minimum = new decimal(new int[4]
			{
				1,
				0,
				0,
				0
			});
			this.minAlphaNumericUpDown.Name = "minAlphaNumericUpDown";
			this.minAlphaNumericUpDown.Size = new Size(55, 20);
			this.minAlphaNumericUpDown.TabIndex = 28;
			this.minAlphaNumericUpDown.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.minAlphaNumericUpDown.ValueChanged += this.minAlphaNumericUpDown_ValueChanged;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(173, 182);
			this.label6.Name = "label6";
			this.label6.Size = new Size(29, 13);
			this.label6.TabIndex = 29;
			this.label6.Text = "max.";
			this.debugCheckBox.AutoSize = true;
			this.debugCheckBox.Location = new Point(6, 158);
			this.debugCheckBox.Name = "debugCheckBox";
			this.debugCheckBox.Size = new Size(106, 17);
			this.debugCheckBox.TabIndex = 24;
			this.debugCheckBox.Text = "Show debug info";
			this.debugCheckBox.UseVisualStyleBackColor = true;
			this.debugCheckBox.CheckedChanged += this.debugCheckBox_CheckedChanged;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(6, 209);
			this.label10.Name = "label10";
			this.label10.Size = new Size(87, 13);
			this.label10.TabIndex = 37;
			this.label10.Text = "Panview position";
			this.label10.Click += this.label10_Click;
			this.pluginPositionComboBox.AutoCompleteCustomSource.AddRange(new string[3]
			{
				"Plugin panel",
				"Main screen left",
				"Main screen right"
			});
			this.pluginPositionComboBox.FormattingEnabled = true;
			this.pluginPositionComboBox.Items.AddRange(new object[4]
			{
				"Main screen up",
				"Main screen down",
				"Main screen left",
				"Main screen right"
			});
			this.pluginPositionComboBox.Location = new Point(97, 206);
			this.pluginPositionComboBox.Name = "pluginPositionComboBox";
			this.pluginPositionComboBox.Size = new Size(105, 21);
			this.pluginPositionComboBox.TabIndex = 36;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			base.ClientSize = new Size(391, 285);
			base.ControlBox = false;
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.okButton);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "DialogConfigure";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "Configure scanner";
			((ISupportInitialize)this.LockIntervalNumericUpDown).EndInit();
			((ISupportInitialize)this.activityNumericUpDown).EndInit();
			((ISupportInitialize)this.autoClearIntervalNumericUpDown).EndInit();
			((ISupportInitialize)this.skipIntervalNumericUpDown).EndInit();
			((ISupportInitialize)this.unMuteNumericUpDown).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((ISupportInitialize)this.maxAlphaNumericUpDown).EndInit();
			((ISupportInitialize)this.minAlphaNumericUpDown).EndInit();
			base.ResumeLayout(false);
		}
	}
}
