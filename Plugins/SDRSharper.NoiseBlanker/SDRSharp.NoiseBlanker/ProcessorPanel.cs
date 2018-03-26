using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.NoiseBlanker
{
	public class ProcessorPanel : UserControl
	{
		private IContainer components;

		private CheckBox enableCheckBox;

		private Label thresholdLabel;

		private TrackBar thresholdTrackBar;

		private Label pulseWidthLabel;

		private TrackBar pulseWidthTrackBar;

		private Label label2;

		private NoiseBlankerProcessor _processor;

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
			this.enableCheckBox = new CheckBox();
			this.thresholdLabel = new Label();
			this.thresholdTrackBar = new TrackBar();
			this.pulseWidthLabel = new Label();
			this.pulseWidthTrackBar = new TrackBar();
			this.label2 = new Label();
			((ISupportInitialize)this.thresholdTrackBar).BeginInit();
			((ISupportInitialize)this.pulseWidthTrackBar).BeginInit();
			base.SuspendLayout();
			this.enableCheckBox.Anchor = AnchorStyles.Top;
			this.enableCheckBox.AutoSize = true;
			this.enableCheckBox.Location = new Point(6, 3);
			this.enableCheckBox.Name = "enableCheckBox";
			this.enableCheckBox.Size = new Size(65, 17);
			this.enableCheckBox.TabIndex = 0;
			this.enableCheckBox.Text = "Enabled";
			this.enableCheckBox.UseVisualStyleBackColor = true;
			this.enableCheckBox.CheckedChanged += this.enableCheckBox_CheckedChanged;
			this.thresholdLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.thresholdLabel.ForeColor = SystemColors.ControlText;
			this.thresholdLabel.Location = new Point(91, 3);
			this.thresholdLabel.Name = "thresholdLabel";
			this.thresholdLabel.Size = new Size(87, 23);
			this.thresholdLabel.TabIndex = 6;
			this.thresholdLabel.Text = "50";
			this.thresholdLabel.TextAlign = ContentAlignment.MiddleRight;
			this.thresholdLabel.Click += this.thresholdLabel_Click;
			this.thresholdTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.thresholdTrackBar.Location = new Point(0, 29);
			this.thresholdTrackBar.Maximum = 100;
			this.thresholdTrackBar.Name = "thresholdTrackBar";
			this.thresholdTrackBar.Size = new Size(198, 45);
			this.thresholdTrackBar.TabIndex = 1;
			this.thresholdTrackBar.TickFrequency = 5;
			this.thresholdTrackBar.Value = 20;
			this.thresholdTrackBar.Scroll += this.thresholdTrackBar_Scroll;
			this.pulseWidthLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.pulseWidthLabel.ForeColor = SystemColors.ControlText;
			this.pulseWidthLabel.Location = new Point(91, 57);
			this.pulseWidthLabel.Name = "pulseWidthLabel";
			this.pulseWidthLabel.Size = new Size(87, 23);
			this.pulseWidthLabel.TabIndex = 8;
			this.pulseWidthLabel.Text = "50";
			this.pulseWidthLabel.TextAlign = ContentAlignment.MiddleRight;
			this.pulseWidthLabel.Click += this.pulseWidthLabel_Click;
			this.pulseWidthTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.pulseWidthTrackBar.Location = new Point(6, 75);
			this.pulseWidthTrackBar.Maximum = 1000;
			this.pulseWidthTrackBar.Minimum = 1;
			this.pulseWidthTrackBar.Name = "pulseWidthTrackBar";
			this.pulseWidthTrackBar.Size = new Size(198, 45);
			this.pulseWidthTrackBar.TabIndex = 2;
			this.pulseWidthTrackBar.TickFrequency = 50;
			this.pulseWidthTrackBar.Value = 100;
			this.pulseWidthTrackBar.Scroll += this.pulseWidthTrackBar_Scroll;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(-7, 63);
			this.label2.Name = "label2";
			this.label2.Size = new Size(61, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Pulse width";
			this.label2.Click += this.label2_Click;
			this.BackColor = SystemColors.ControlLight;
			base.Controls.Add(this.label2);
			base.Controls.Add(this.pulseWidthLabel);
			base.Controls.Add(this.pulseWidthTrackBar);
			base.Controls.Add(this.thresholdLabel);
			base.Controls.Add(this.thresholdTrackBar);
			base.Controls.Add(this.enableCheckBox);
			base.Name = "ProcessorPanel";
			base.Size = new Size(204, 134);
			((ISupportInitialize)this.thresholdTrackBar).EndInit();
			((ISupportInitialize)this.pulseWidthTrackBar).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public ProcessorPanel(NoiseBlankerProcessor processor)
		{
			this._processor = processor;
			this.InitializeComponent();
			this.enableCheckBox.Checked = this._processor.Enabled;
			this.thresholdTrackBar.Value = this._processor.NoiseThreshold;
			this.pulseWidthTrackBar.Value = (int)(this._processor.PulseWidth * 10.0);
			this.thresholdTrackBar_Scroll(null, null);
			this.pulseWidthTrackBar_Scroll(null, null);
		}

		private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this._processor.Enabled = this.enableCheckBox.Checked;
		}

		private void thresholdTrackBar_Scroll(object sender, EventArgs e)
		{
			this._processor.NoiseThreshold = this.thresholdTrackBar.Value;
			this.thresholdLabel.Text = this._processor.NoiseThreshold.ToString();
		}

		private void pulseWidthTrackBar_Scroll(object sender, EventArgs e)
		{
			this._processor.PulseWidth = (double)((float)this.pulseWidthTrackBar.Value * 0.1f);
			this.pulseWidthLabel.Text = this._processor.PulseWidth.ToString("0.00") + " µS";
		}

		private void pulseWidthLabel_Click(object sender, EventArgs e)
		{
		}

		private void label2_Click(object sender, EventArgs e)
		{
		}

		private void thresholdLabel_Click(object sender, EventArgs e)
		{
		}
	}
}
