using SDRSharp.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.NoiseReduction
{
	public class NoiseReductionPanel : UserControl
	{
		private INoiseProcessor _aControl;

		private INoiseProcessor _iControl;

		private NoiseBlankerProcessor _processor;

		private IContainer components;

		private Label ifThresholdLabel;

		private Label audioThresholdLabel;

		private gSliderH ifThresholdTrackBar;

		private gSliderH audioThresholdTrackBar;

		private Label label3;

		private Label pulseWidthLabel;

		private Label thresholdLabel;

		private gSliderH thresholdTrackBar;

		private gSliderH pulseWidthTrackBar;

		private gButton ifEnableCheckBox;

		private gButton audioEnableCheckbox;

		private gButton enableCheckBox;

		private Panel panel1;

		private Panel panel2;

		public NoiseReductionPanel(INoiseProcessor iControl, INoiseProcessor aControl, NoiseBlankerProcessor processor)
		{
			this._iControl = iControl;
			this._aControl = aControl;
			this._processor = processor;
			this.InitializeComponent();
			this.ifThresholdTrackBar.Value = this._iControl.NoiseThreshold;
			this.ifEnableCheckBox.Checked = this._iControl.Enabled;
			this.audioThresholdTrackBar.Value = this._aControl.NoiseThreshold;
			this.audioEnableCheckbox.Checked = this._aControl.Enabled;
			this.ifThresholdTrackBar_Scroll(null, null);
			this.audioThresholdTrackBar_Scroll(null, null);
			this.thresholdTrackBar.Value = this._processor.NoiseThreshold;
			this.pulseWidthTrackBar.Value = (int)(this._processor.PulseWidth * 10.0);
			this.enableCheckBox.Checked = this._processor.Enabled;
			this.thresholdTrackBar_Scroll(null, null);
			this.pulseWidthTrackBar_Scroll(null, null);
		}

		private void ifEnableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this._iControl.Enabled = this.ifEnableCheckBox.Checked;
			this.ifThresholdTrackBar_Scroll(null, null);
		}

		private void audioEnableCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this._aControl.Enabled = this.audioEnableCheckbox.Checked;
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

		private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this._processor.Enabled = this.enableCheckBox.Checked;
			this.thresholdTrackBar_Scroll(null, null);
			this.pulseWidthTrackBar_Scroll(null, null);
		}

		private void thresholdTrackBar_Scroll(object sender, EventArgs e)
		{
			this._processor.NoiseThreshold = this.thresholdTrackBar.Value;
			this.thresholdLabel.Text = this._processor.NoiseThreshold.ToString();
		}

		private void pulseWidthTrackBar_Scroll(object sender, EventArgs e)
		{
			this._processor.PulseWidth = (double)((float)this.pulseWidthTrackBar.Value * 0.1f);
			this.pulseWidthLabel.Text = this._processor.PulseWidth.ToString("0.0") + " µs";
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
			this.ifThresholdLabel = new Label();
			this.audioThresholdLabel = new Label();
			this.label3 = new Label();
			this.pulseWidthLabel = new Label();
			this.thresholdLabel = new Label();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
			this.thresholdTrackBar = new gSliderH();
			this.enableCheckBox = new gButton();
			this.pulseWidthTrackBar = new gSliderH();
			this.ifThresholdTrackBar = new gSliderH();
			this.audioEnableCheckbox = new gButton();
			this.ifEnableCheckBox = new gButton();
			this.audioThresholdTrackBar = new gSliderH();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.ifThresholdLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.ifThresholdLabel.ForeColor = Color.Yellow;
			this.ifThresholdLabel.Location = new Point(45, 8);
			this.ifThresholdLabel.Name = "ifThresholdLabel";
			this.ifThresholdLabel.Size = new Size(44, 23);
			this.ifThresholdLabel.TabIndex = 6;
			this.ifThresholdLabel.Text = "-5 dB";
			this.ifThresholdLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.audioThresholdLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.audioThresholdLabel.ForeColor = Color.Yellow;
			this.audioThresholdLabel.Location = new Point(45, 34);
			this.audioThresholdLabel.Name = "audioThresholdLabel";
			this.audioThresholdLabel.Size = new Size(44, 23);
			this.audioThresholdLabel.TabIndex = 6;
			this.audioThresholdLabel.Text = "-5 dB";
			this.audioThresholdLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.label3.ForeColor = Color.Orange;
			this.label3.Location = new Point(7, 32);
			this.label3.Name = "label3";
			this.label3.Size = new Size(37, 30);
			this.label3.TabIndex = 17;
			this.label3.Text = "Pulse width";
			this.pulseWidthLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.pulseWidthLabel.ForeColor = Color.Yellow;
			this.pulseWidthLabel.Location = new Point(46, 31);
			this.pulseWidthLabel.Name = "pulseWidthLabel";
			this.pulseWidthLabel.Size = new Size(44, 23);
			this.pulseWidthLabel.TabIndex = 16;
			this.pulseWidthLabel.Text = "50";
			this.pulseWidthLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.thresholdLabel.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.thresholdLabel.ForeColor = Color.Yellow;
			this.thresholdLabel.Location = new Point(46, 6);
			this.thresholdLabel.Name = "thresholdLabel";
			this.thresholdLabel.Size = new Size(44, 23);
			this.thresholdLabel.TabIndex = 15;
			this.thresholdLabel.Text = "50";
			this.thresholdLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.panel1.BackColor = Color.FromArgb(64, 64, 64);
			this.panel1.Controls.Add(this.ifThresholdTrackBar);
			this.panel1.Controls.Add(this.audioEnableCheckbox);
			this.panel1.Controls.Add(this.ifEnableCheckBox);
			this.panel1.Controls.Add(this.audioThresholdTrackBar);
			this.panel1.Controls.Add(this.ifThresholdLabel);
			this.panel1.Controls.Add(this.audioThresholdLabel);
			this.panel1.Location = new Point(0, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(198, 62);
			this.panel1.TabIndex = 71;
			this.panel2.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.panel2.BackColor = Color.FromArgb(64, 64, 64);
			this.panel2.Controls.Add(this.thresholdTrackBar);
			this.panel2.Controls.Add(this.enableCheckBox);
			this.panel2.Controls.Add(this.pulseWidthTrackBar);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.thresholdLabel);
			this.panel2.Controls.Add(this.pulseWidthLabel);
			this.panel2.Location = new Point(0, 63);
			this.panel2.Name = "panel2";
			this.panel2.Size = new Size(198, 62);
			this.panel2.TabIndex = 72;
			this.thresholdTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.thresholdTrackBar.Button = false;
			this.thresholdTrackBar.Checked = false;
			this.thresholdTrackBar.ColorFactor = 0.5f;
			this.thresholdTrackBar.ForeColor = Color.Black;
			this.thresholdTrackBar.Location = new Point(89, 10);
			this.thresholdTrackBar.Margin = new Padding(4);
			this.thresholdTrackBar.Maximum = 100;
			this.thresholdTrackBar.Minimum = 0;
			this.thresholdTrackBar.Name = "thresholdTrackBar";
			this.thresholdTrackBar.Size = new Size(107, 16);
			this.thresholdTrackBar.TabIndex = 8;
			this.thresholdTrackBar.TickColor = Color.Silver;
			this.thresholdTrackBar.Ticks = 8;
			this.thresholdTrackBar.ToolTip = null;
			this.thresholdTrackBar.Value = 20;
			this.thresholdTrackBar.ValueChanged += this.thresholdTrackBar_Scroll;
			this.enableCheckBox.Arrow = 99;
			this.enableCheckBox.Checked = false;
			this.enableCheckBox.Edge = 0.15f;
			this.enableCheckBox.EndColor = Color.White;
			this.enableCheckBox.EndFactor = 0.2f;
			this.enableCheckBox.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.enableCheckBox.ForeColor = Color.Orange;
			this.enableCheckBox.Location = new Point(2, 6);
			this.enableCheckBox.Name = "enableCheckBox";
			this.enableCheckBox.NoBorder = false;
			this.enableCheckBox.NoLed = false;
			this.enableCheckBox.RadioButton = false;
			this.enableCheckBox.Radius = 6;
			this.enableCheckBox.RadiusB = 0;
			this.enableCheckBox.Size = new Size(44, 24);
			this.enableCheckBox.StartColor = Color.Black;
			this.enableCheckBox.StartFactor = 0.35f;
			this.enableCheckBox.TabIndex = 70;
			this.enableCheckBox.Text = "N-Bl.";
			this.enableCheckBox.CheckedChanged += this.enableCheckBox_CheckedChanged;
			this.pulseWidthTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.pulseWidthTrackBar.Button = false;
			this.pulseWidthTrackBar.Checked = false;
			this.pulseWidthTrackBar.ColorFactor = 0.5f;
			this.pulseWidthTrackBar.ForeColor = Color.Black;
			this.pulseWidthTrackBar.Location = new Point(89, 36);
			this.pulseWidthTrackBar.Margin = new Padding(4);
			this.pulseWidthTrackBar.Maximum = 999;
			this.pulseWidthTrackBar.Minimum = 1;
			this.pulseWidthTrackBar.Name = "pulseWidthTrackBar";
			this.pulseWidthTrackBar.Size = new Size(107, 16);
			this.pulseWidthTrackBar.TabIndex = 12;
			this.pulseWidthTrackBar.TickColor = Color.Silver;
			this.pulseWidthTrackBar.Ticks = 8;
			this.pulseWidthTrackBar.ToolTip = null;
			this.pulseWidthTrackBar.Value = 100;
			this.pulseWidthTrackBar.ValueChanged += this.pulseWidthTrackBar_Scroll;
			this.ifThresholdTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.ifThresholdTrackBar.Button = false;
			this.ifThresholdTrackBar.Checked = false;
			this.ifThresholdTrackBar.ColorFactor = 0.5f;
			this.ifThresholdTrackBar.ForeColor = Color.Black;
			this.ifThresholdTrackBar.Location = new Point(90, 10);
			this.ifThresholdTrackBar.Margin = new Padding(4);
			this.ifThresholdTrackBar.Maximum = 20;
			this.ifThresholdTrackBar.Minimum = -80;
			this.ifThresholdTrackBar.Name = "ifThresholdTrackBar";
			this.ifThresholdTrackBar.Size = new Size(105, 16);
			this.ifThresholdTrackBar.TabIndex = 7;
			this.ifThresholdTrackBar.TickColor = Color.Silver;
			this.ifThresholdTrackBar.Ticks = 8;
			this.ifThresholdTrackBar.ToolTip = null;
			this.ifThresholdTrackBar.Value = -30;
			this.ifThresholdTrackBar.ValueChanged += this.ifThresholdTrackBar_Scroll;
			this.audioEnableCheckbox.Arrow = 99;
			this.audioEnableCheckbox.Checked = false;
			this.audioEnableCheckbox.Edge = 0.15f;
			this.audioEnableCheckbox.EndColor = Color.White;
			this.audioEnableCheckbox.EndFactor = 0.2f;
			this.audioEnableCheckbox.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.audioEnableCheckbox.ForeColor = Color.Orange;
			this.audioEnableCheckbox.Location = new Point(2, 33);
			this.audioEnableCheckbox.Name = "audioEnableCheckbox";
			this.audioEnableCheckbox.NoBorder = false;
			this.audioEnableCheckbox.NoLed = false;
			this.audioEnableCheckbox.RadioButton = false;
			this.audioEnableCheckbox.Radius = 6;
			this.audioEnableCheckbox.RadiusB = 0;
			this.audioEnableCheckbox.Size = new Size(44, 24);
			this.audioEnableCheckbox.StartColor = Color.Black;
			this.audioEnableCheckbox.StartFactor = 0.35f;
			this.audioEnableCheckbox.TabIndex = 69;
			this.audioEnableCheckbox.Text = "A.F.";
			this.audioEnableCheckbox.CheckedChanged += this.audioEnableCheckbox_CheckedChanged;
			this.ifEnableCheckBox.Arrow = 99;
			this.ifEnableCheckBox.Checked = false;
			this.ifEnableCheckBox.Edge = 0.15f;
			this.ifEnableCheckBox.EndColor = Color.White;
			this.ifEnableCheckBox.EndFactor = 0.2f;
			this.ifEnableCheckBox.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ifEnableCheckBox.ForeColor = Color.Orange;
			this.ifEnableCheckBox.Location = new Point(2, 6);
			this.ifEnableCheckBox.Name = "ifEnableCheckBox";
			this.ifEnableCheckBox.NoBorder = false;
			this.ifEnableCheckBox.NoLed = false;
			this.ifEnableCheckBox.RadioButton = false;
			this.ifEnableCheckBox.Radius = 6;
			this.ifEnableCheckBox.RadiusB = 0;
			this.ifEnableCheckBox.Size = new Size(44, 24);
			this.ifEnableCheckBox.StartColor = Color.Black;
			this.ifEnableCheckBox.StartFactor = 0.35f;
			this.ifEnableCheckBox.TabIndex = 68;
			this.ifEnableCheckBox.Text = "I.F.";
			this.ifEnableCheckBox.CheckedChanged += this.ifEnableCheckBox_CheckedChanged;
			this.audioThresholdTrackBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.audioThresholdTrackBar.Button = false;
			this.audioThresholdTrackBar.Checked = false;
			this.audioThresholdTrackBar.ColorFactor = 0.5f;
			this.audioThresholdTrackBar.ForeColor = Color.Black;
			this.audioThresholdTrackBar.Location = new Point(90, 36);
			this.audioThresholdTrackBar.Margin = new Padding(4);
			this.audioThresholdTrackBar.Maximum = 20;
			this.audioThresholdTrackBar.Minimum = -120;
			this.audioThresholdTrackBar.Name = "audioThresholdTrackBar";
			this.audioThresholdTrackBar.Size = new Size(105, 16);
			this.audioThresholdTrackBar.TabIndex = 8;
			this.audioThresholdTrackBar.TickColor = Color.Silver;
			this.audioThresholdTrackBar.Ticks = 8;
			this.audioThresholdTrackBar.ToolTip = null;
			this.audioThresholdTrackBar.Value = -30;
			this.audioThresholdTrackBar.ValueChanged += this.audioThresholdTrackBar_Scroll;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.Name = "NoiseReductionPanel";
			base.Size = new Size(198, 145);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
