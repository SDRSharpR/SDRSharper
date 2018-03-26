namespace SDRSharp.RTLSDR
{
	// Token: 0x02000003 RID: 3
	public partial class RtlSdrControllerDialog : global::System.Windows.Forms.Form
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000028C2 File Offset: 0x00000AC2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000028E4 File Offset: 0x00000AE4
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.refreshTimer = new global::System.Windows.Forms.Timer(this.components);
			this.closeButton = new global::System.Windows.Forms.Button();
			this.deviceComboBox = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.tunerGainTrackBar = new global::System.Windows.Forms.TrackBar();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.samplerateComboBox = new global::System.Windows.Forms.ComboBox();
			this.tunerAgcCheckBox = new global::System.Windows.Forms.CheckBox();
			this.gainLabel = new global::System.Windows.Forms.Label();
			this.frequencyCorrectionNumericUpDown = new global::System.Windows.Forms.NumericUpDown();
			this.label4 = new global::System.Windows.Forms.Label();
			this.tunerTypeLabel = new global::System.Windows.Forms.Label();
			this.rtlAgcCheckBox = new global::System.Windows.Forms.CheckBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.samplingModeComboBox = new global::System.Windows.Forms.ComboBox();
			this.offsetTuningCheckBox = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.tunerGainTrackBar).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.frequencyCorrectionNumericUpDown).BeginInit();
			base.SuspendLayout();
			this.refreshTimer.Interval = 1000;
			this.refreshTimer.Tick += new global::System.EventHandler(this.refreshTimer_Tick);
			this.closeButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new global::System.Drawing.Point(184, 306);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new global::System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 8;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new global::System.EventHandler(this.closeButton_Click);
			this.deviceComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.deviceComboBox.FormattingEnabled = true;
			this.deviceComboBox.Location = new global::System.Drawing.Point(12, 26);
			this.deviceComboBox.Name = "deviceComboBox";
			this.deviceComboBox.Size = new global::System.Drawing.Size(247, 21);
			this.deviceComboBox.TabIndex = 0;
			this.deviceComboBox.SelectedIndexChanged += new global::System.EventHandler(this.deviceComboBox_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 13);
			this.label1.TabIndex = 20;
			this.label1.Text = "Device";
			this.tunerGainTrackBar.Enabled = false;
			this.tunerGainTrackBar.Location = new global::System.Drawing.Point(3, 229);
			this.tunerGainTrackBar.Maximum = 10000;
			this.tunerGainTrackBar.Name = "tunerGainTrackBar";
			this.tunerGainTrackBar.Size = new global::System.Drawing.Size(267, 45);
			this.tunerGainTrackBar.TabIndex = 6;
			this.tunerGainTrackBar.Scroll += new global::System.EventHandler(this.tunerGainTrackBar_Scroll);
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(12, 213);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(46, 13);
			this.label2.TabIndex = 22;
			this.label2.Text = "RF Gain";
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(12, 53);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(68, 13);
			this.label3.TabIndex = 24;
			this.label3.Text = "Sample Rate";
			this.samplerateComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.samplerateComboBox.FormattingEnabled = true;
			this.samplerateComboBox.Items.AddRange(new object[]
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
			this.samplerateComboBox.Location = new global::System.Drawing.Point(12, 70);
			this.samplerateComboBox.Name = "samplerateComboBox";
			this.samplerateComboBox.Size = new global::System.Drawing.Size(247, 21);
			this.samplerateComboBox.TabIndex = 1;
			this.samplerateComboBox.SelectedIndexChanged += new global::System.EventHandler(this.samplerateComboBox_SelectedIndexChanged);
			this.tunerAgcCheckBox.AutoSize = true;
			this.tunerAgcCheckBox.Checked = true;
			this.tunerAgcCheckBox.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.tunerAgcCheckBox.Location = new global::System.Drawing.Point(12, 193);
			this.tunerAgcCheckBox.Name = "tunerAgcCheckBox";
			this.tunerAgcCheckBox.Size = new global::System.Drawing.Size(79, 17);
			this.tunerAgcCheckBox.TabIndex = 5;
			this.tunerAgcCheckBox.Text = "Tuner AGC";
			this.tunerAgcCheckBox.UseVisualStyleBackColor = true;
			this.tunerAgcCheckBox.CheckedChanged += new global::System.EventHandler(this.tunerAgcCheckBox_CheckedChanged);
			this.gainLabel.Location = new global::System.Drawing.Point(191, 213);
			this.gainLabel.Name = "gainLabel";
			this.gainLabel.Size = new global::System.Drawing.Size(68, 13);
			this.gainLabel.TabIndex = 26;
			this.gainLabel.Text = "1000dB";
			this.gainLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.gainLabel.Visible = false;
			this.frequencyCorrectionNumericUpDown.Location = new global::System.Drawing.Point(169, 275);
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.frequencyCorrectionNumericUpDown;
			int[] array = new int[4];
			array[0] = 1000;
			numericUpDown.Maximum = new decimal(array);
			this.frequencyCorrectionNumericUpDown.Minimum = new decimal(new int[]
			{
				1000,
				0,
				0,
				int.MinValue
			});
			this.frequencyCorrectionNumericUpDown.Name = "frequencyCorrectionNumericUpDown";
			this.frequencyCorrectionNumericUpDown.Size = new global::System.Drawing.Size(90, 20);
			this.frequencyCorrectionNumericUpDown.TabIndex = 7;
			this.frequencyCorrectionNumericUpDown.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Right;
			this.frequencyCorrectionNumericUpDown.ValueChanged += new global::System.EventHandler(this.frequencyCorrectionNumericUpDown_ValueChanged);
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(12, 277);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(136, 13);
			this.label4.TabIndex = 28;
			this.label4.Text = "Frequency correction (ppm)";
			this.tunerTypeLabel.Location = new global::System.Drawing.Point(166, 9);
			this.tunerTypeLabel.Name = "tunerTypeLabel";
			this.tunerTypeLabel.Size = new global::System.Drawing.Size(93, 13);
			this.tunerTypeLabel.TabIndex = 29;
			this.tunerTypeLabel.Text = "E4000";
			this.tunerTypeLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.rtlAgcCheckBox.AutoSize = true;
			this.rtlAgcCheckBox.Location = new global::System.Drawing.Point(12, 170);
			this.rtlAgcCheckBox.Name = "rtlAgcCheckBox";
			this.rtlAgcCheckBox.Size = new global::System.Drawing.Size(72, 17);
			this.rtlAgcCheckBox.TabIndex = 4;
			this.rtlAgcCheckBox.Text = "RTL AGC";
			this.rtlAgcCheckBox.UseVisualStyleBackColor = true;
			this.rtlAgcCheckBox.CheckedChanged += new global::System.EventHandler(this.rtlAgcCheckBox_CheckedChanged);
			this.label5.AutoSize = true;
			this.label5.Location = new global::System.Drawing.Point(12, 97);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(80, 13);
			this.label5.TabIndex = 31;
			this.label5.Text = "Sampling Mode";
			this.samplingModeComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.samplingModeComboBox.FormattingEnabled = true;
			this.samplingModeComboBox.Items.AddRange(new object[]
			{
				"Quadrature sampling",
				"Direct sampling (I branch)",
				"Direct sampling (Q branch)"
			});
			this.samplingModeComboBox.Location = new global::System.Drawing.Point(12, 114);
			this.samplingModeComboBox.Name = "samplingModeComboBox";
			this.samplingModeComboBox.Size = new global::System.Drawing.Size(247, 21);
			this.samplingModeComboBox.TabIndex = 2;
			this.samplingModeComboBox.SelectedIndexChanged += new global::System.EventHandler(this.samplingModeComboBox_SelectedIndexChanged);
			this.offsetTuningCheckBox.AutoSize = true;
			this.offsetTuningCheckBox.Location = new global::System.Drawing.Point(12, 147);
			this.offsetTuningCheckBox.Name = "offsetTuningCheckBox";
			this.offsetTuningCheckBox.Size = new global::System.Drawing.Size(90, 17);
			this.offsetTuningCheckBox.TabIndex = 3;
			this.offsetTuningCheckBox.Text = "Offset Tuning";
			this.offsetTuningCheckBox.UseVisualStyleBackColor = true;
			this.offsetTuningCheckBox.CheckedChanged += new global::System.EventHandler(this.offsetTuningCheckBox_CheckedChanged);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.closeButton;
			base.ClientSize = new global::System.Drawing.Size(271, 342);
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
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RtlSdrControllerDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "RTL-SDR Controller";
			base.TopMost = true;
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.RtlSdrControllerDialog_FormClosing);
			base.VisibleChanged += new global::System.EventHandler(this.RtlSdrControllerDialog_VisibleChanged);
			((global::System.ComponentModel.ISupportInitialize)this.tunerGainTrackBar).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.frequencyCorrectionNumericUpDown).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000005 RID: 5
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.Timer refreshTimer;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.Button closeButton;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.ComboBox deviceComboBox;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.TrackBar tunerGainTrackBar;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.ComboBox samplerateComboBox;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.CheckBox tunerAgcCheckBox;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.Label gainLabel;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.NumericUpDown frequencyCorrectionNumericUpDown;

		// Token: 0x04000011 RID: 17
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000012 RID: 18
		private global::System.Windows.Forms.Label tunerTypeLabel;

		// Token: 0x04000013 RID: 19
		private global::System.Windows.Forms.CheckBox rtlAgcCheckBox;

		// Token: 0x04000014 RID: 20
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04000015 RID: 21
		private global::System.Windows.Forms.ComboBox samplingModeComboBox;

		// Token: 0x04000016 RID: 22
		private global::System.Windows.Forms.CheckBox offsetTuningCheckBox;
	}
}
