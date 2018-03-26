using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.DNR
{
	public class ProcessorPanel : UserControl
	{
		private IContainer components;

		private CheckBox ifEnableCheckBox;

		private GroupBox groupBox1;

		private Label ifThresholdLabel;

		private TrackBar ifThresholdTrackBar;

		private GroupBox groupBox2;

		private Label audioThresholdLabel;

		private TrackBar audioThresholdTrackBar;

		private CheckBox audioEnableCheckBox;

		private INoiseProcessor _aControl;

		private INoiseProcessor _iControl;

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
			this.ifEnableCheckBox = new CheckBox();
			this.groupBox1 = new GroupBox();
			this.ifThresholdLabel = new Label();
			this.ifThresholdTrackBar = new TrackBar();
			this.groupBox2 = new GroupBox();
			this.audioThresholdLabel = new Label();
			this.audioThresholdTrackBar = new TrackBar();
			this.audioEnableCheckBox = new CheckBox();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.ifThresholdTrackBar).BeginInit();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.audioThresholdTrackBar).BeginInit();
			base.SuspendLayout();
			this.ifEnableCheckBox.Anchor = AnchorStyles.Top;
			this.ifEnableCheckBox.AutoSize = true;
			this.ifEnableCheckBox.Location = new Point(6, 19);
			this.ifEnableCheckBox.Name = "ifEnableCheckBox";
			this.ifEnableCheckBox.RightToLeft = RightToLeft.Yes;
			this.ifEnableCheckBox.Size = new Size(65, 17);
			this.ifEnableCheckBox.TabIndex = 4;
			this.ifEnableCheckBox.Text = "Enabled";
			this.ifEnableCheckBox.UseVisualStyleBackColor = true;
			this.ifEnableCheckBox.CheckedChanged += this.ifEnableCheckBox_CheckedChanged;
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.ifThresholdLabel);
			this.groupBox1.Controls.Add(this.ifThresholdTrackBar);
			this.groupBox1.Controls.Add(this.ifEnableCheckBox);
			this.groupBox1.Location = new Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(204, 100);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "IF";
			this.ifThresholdLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.ifThresholdLabel.Location = new Point(111, 15);
			this.ifThresholdLabel.Name = "ifThresholdLabel";
			this.ifThresholdLabel.Size = new Size(87, 23);
			this.ifThresholdLabel.TabIndex = 6;
			this.ifThresholdLabel.Text = "-5 dB";
			this.ifThresholdLabel.TextAlign = ContentAlignment.MiddleRight;
			this.ifThresholdTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.ifThresholdTrackBar.Location = new Point(3, 40);
			this.ifThresholdTrackBar.Maximum = 20;
			this.ifThresholdTrackBar.Minimum = -80;
			this.ifThresholdTrackBar.Name = "ifThresholdTrackBar";
			this.ifThresholdTrackBar.Size = new Size(198, 45);
			this.ifThresholdTrackBar.TabIndex = 5;
			this.ifThresholdTrackBar.TickFrequency = 5;
			this.ifThresholdTrackBar.Value = -30;
			this.ifThresholdTrackBar.Scroll += this.ifThresholdTrackBar_Scroll;
			this.groupBox2.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox2.Controls.Add(this.audioThresholdLabel);
			this.groupBox2.Controls.Add(this.audioThresholdTrackBar);
			this.groupBox2.Controls.Add(this.audioEnableCheckBox);
			this.groupBox2.Location = new Point(0, 102);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(204, 100);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Audio";
			this.audioThresholdLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.audioThresholdLabel.Location = new Point(111, 15);
			this.audioThresholdLabel.Name = "audioThresholdLabel";
			this.audioThresholdLabel.Size = new Size(87, 23);
			this.audioThresholdLabel.TabIndex = 6;
			this.audioThresholdLabel.Text = "-5 dB";
			this.audioThresholdLabel.TextAlign = ContentAlignment.MiddleRight;
			this.audioThresholdTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.audioThresholdTrackBar.Location = new Point(3, 40);
			this.audioThresholdTrackBar.Maximum = -20;
			this.audioThresholdTrackBar.Minimum = -120;
			this.audioThresholdTrackBar.Name = "audioThresholdTrackBar";
			this.audioThresholdTrackBar.Size = new Size(198, 45);
			this.audioThresholdTrackBar.TabIndex = 5;
			this.audioThresholdTrackBar.TickFrequency = 5;
			this.audioThresholdTrackBar.Value = -70;
			this.audioThresholdTrackBar.Scroll += this.audioThresholdTrackBar_Scroll;
			this.audioEnableCheckBox.Anchor = AnchorStyles.Top;
			this.audioEnableCheckBox.AutoSize = true;
			this.audioEnableCheckBox.Location = new Point(6, 19);
			this.audioEnableCheckBox.Name = "audioEnableCheckBox";
			this.audioEnableCheckBox.RightToLeft = RightToLeft.Yes;
			this.audioEnableCheckBox.Size = new Size(65, 17);
			this.audioEnableCheckBox.TabIndex = 4;
			this.audioEnableCheckBox.Text = "Enabled";
			this.audioEnableCheckBox.UseVisualStyleBackColor = true;
			this.audioEnableCheckBox.CheckedChanged += this.audioEnableCheckbox_CheckedChanged;
			this.BackColor = SystemColors.ControlLight;
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Name = "ProcessorPanel";
			base.Size = new Size(204, 227);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.ifThresholdTrackBar).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((ISupportInitialize)this.audioThresholdTrackBar).EndInit();
			base.ResumeLayout(false);
		}

		public ProcessorPanel(INoiseProcessor iControl, INoiseProcessor aControl)
		{
			this._iControl = iControl;
			this._aControl = aControl;
			this.InitializeComponent();
			this.ifThresholdTrackBar.Value = this._iControl.NoiseThreshold;
			this.ifEnableCheckBox.Checked = this._iControl.Enabled;
			this.audioThresholdTrackBar.Value = this._aControl.NoiseThreshold;
			this.audioEnableCheckBox.Checked = this._aControl.Enabled;
			this.ifThresholdTrackBar_Scroll(null, null);
			this.audioThresholdTrackBar_Scroll(null, null);
		}

		private void ifEnableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this._iControl.Enabled = this.ifEnableCheckBox.Checked;
			this.ifThresholdTrackBar_Scroll(null, null);
		}

		private void audioEnableCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this._aControl.Enabled = this.audioEnableCheckBox.Checked;
			this.audioThresholdTrackBar_Scroll(null, null);
		}

		private void ifThresholdTrackBar_Scroll(object sender, EventArgs e)
		{
			this.ifThresholdLabel.Text = this.ifThresholdTrackBar.Value + " dB";
			this._iControl.NoiseThreshold = this.ifThresholdTrackBar.Value;
		}

		private void audioThresholdTrackBar_Scroll(object sender, EventArgs e)
		{
			this.audioThresholdLabel.Text = this.audioThresholdTrackBar.Value + " dB";
			this._aControl.NoiseThreshold = this.audioThresholdTrackBar.Value;
		}
	}
}
