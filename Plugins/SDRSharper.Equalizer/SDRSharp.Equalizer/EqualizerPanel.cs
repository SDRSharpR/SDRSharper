using SDRSharp.Common;
using SDRSharp.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Equalizer
{
	public class EqualizerPanel : UserControl
	{
		private IContainer components;

		private Panel panel1;

		private Label label7;

		private Label label6;

		private Label label5;

		private Label label4;

		private gButton bassButton;

		private Label label54;

		private gSliderV tbHighGain;

		private gSliderV tbMedGain;

		private Label label52;

		private gSliderV tbLowGain;

		private gButton enableButton;

		private Label label1;

		private gUpDown numHighCutoff;

		private gUpDown numLowCutoff;

		private ISharpControl _control;

		private EqualizerProcessor _audioProcessor;

		public ToolTip _toolTip;

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
			this.panel1 = new Panel();
			this.label1 = new Label();
			this.label7 = new Label();
			this.label6 = new Label();
			this.label5 = new Label();
			this.label4 = new Label();
			this.label54 = new Label();
			this.label52 = new Label();
			this.numLowCutoff = new gUpDown();
			this.numHighCutoff = new gUpDown();
			this.bassButton = new gButton();
			this.tbHighGain = new gSliderV();
			this.tbMedGain = new gSliderV();
			this.tbLowGain = new gSliderV();
			this.enableButton = new gButton();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.panel1.BackColor = Color.FromArgb(64, 64, 64);
			this.panel1.Controls.Add(this.numLowCutoff);
			this.panel1.Controls.Add(this.numHighCutoff);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.bassButton);
			this.panel1.Controls.Add(this.label54);
			this.panel1.Controls.Add(this.tbHighGain);
			this.panel1.Controls.Add(this.tbMedGain);
			this.panel1.Controls.Add(this.label52);
			this.panel1.Controls.Add(this.tbLowGain);
			this.panel1.Controls.Add(this.enableButton);
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(198, 134);
			this.panel1.TabIndex = 9;
			this.label1.AutoSize = true;
			this.label1.ForeColor = Color.Orange;
			this.label1.Location = new Point(31, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(21, 13);
			this.label1.TabIndex = 118;
			this.label1.Text = "On";
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			this.label7.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.label7.AutoSize = true;
			this.label7.ForeColor = Color.Orange;
			this.label7.Location = new Point(168, 29);
			this.label7.Name = "label7";
			this.label7.Size = new Size(29, 13);
			this.label7.TabIndex = 117;
			this.label7.Text = "High";
			this.label7.TextAlign = ContentAlignment.MiddleCenter;
			this.label6.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.label6.AutoSize = true;
			this.label6.ForeColor = Color.Orange;
			this.label6.Location = new Point(133, 29);
			this.label6.Name = "label6";
			this.label6.Size = new Size(28, 13);
			this.label6.TabIndex = 116;
			this.label6.Text = "Med";
			this.label6.TextAlign = ContentAlignment.MiddleCenter;
			this.label5.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.label5.AutoSize = true;
			this.label5.ForeColor = Color.Orange;
			this.label5.Location = new Point(97, 29);
			this.label5.Name = "label5";
			this.label5.Size = new Size(27, 13);
			this.label5.TabIndex = 115;
			this.label5.Text = "Low";
			this.label5.TextAlign = ContentAlignment.MiddleCenter;
			this.label4.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.label4.AutoSize = true;
			this.label4.ForeColor = Color.Orange;
			this.label4.Location = new Point(125, 9);
			this.label4.Name = "label4";
			this.label4.Size = new Size(59, 13);
			this.label4.TabIndex = 114;
			this.label4.Text = "Bass boost";
			this.label4.TextAlign = ContentAlignment.MiddleCenter;
			this.label54.AutoSize = true;
			this.label54.ForeColor = Color.Orange;
			this.label54.Location = new Point(0, 30);
			this.label54.Name = "label54";
			this.label54.Size = new Size(81, 13);
			this.label54.TabIndex = 108;
			this.label54.Text = "High cutoff (Hz)";
			this.label54.TextAlign = ContentAlignment.MiddleCenter;
			this.label52.AutoSize = true;
			this.label52.ForeColor = Color.Orange;
			this.label52.Location = new Point(0, 78);
			this.label52.Name = "label52";
			this.label52.Size = new Size(79, 13);
			this.label52.TabIndex = 105;
			this.label52.Text = "Low cutoff (Hz)";
			this.label52.TextAlign = ContentAlignment.MiddleCenter;
			this.numLowCutoff.ForeColor = Color.Yellow;
			this.numLowCutoff.Increment = 20;
			this.numLowCutoff.Location = new Point(3, 96);
			this.numLowCutoff.Margin = new Padding(5);
			this.numLowCutoff.Maximum = 800L;
			this.numLowCutoff.Minimum = 50L;
			this.numLowCutoff.Name = "numLowCutoff";
			this.numLowCutoff.Size = new Size(79, 20);
			this.numLowCutoff.TabIndex = 120;
			this.numLowCutoff.ToolTip = null;
			this.numLowCutoff.Value = 200L;
			this.numLowCutoff.ValueChanged += this.numLowCutOff_ValueChanged;
			this.numHighCutoff.ForeColor = Color.Yellow;
			this.numHighCutoff.Increment = 200;
			this.numHighCutoff.Location = new Point(3, 48);
			this.numHighCutoff.Margin = new Padding(5);
			this.numHighCutoff.Maximum = 8000L;
			this.numHighCutoff.Minimum = 2000L;
			this.numHighCutoff.Name = "numHighCutoff";
			this.numHighCutoff.Size = new Size(79, 20);
			this.numHighCutoff.TabIndex = 119;
			this.numHighCutoff.ToolTip = null;
			this.numHighCutoff.Value = 4000L;
			this.numHighCutoff.ValueChanged += this.numHighCutOff_ValueChanged;
			this.bassButton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.bassButton.Arrow = 0;
			this.bassButton.Checked = false;
			this.bassButton.Edge = 0.15f;
			this.bassButton.EndColor = Color.White;
			this.bassButton.EndFactor = 0.2f;
			this.bassButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.bassButton.ForeColor = Color.Orange;
			this.bassButton.Location = new Point(97, 6);
			this.bassButton.Name = "bassButton";
			this.bassButton.NoBorder = false;
			this.bassButton.NoLed = false;
			this.bassButton.RadioButton = false;
			this.bassButton.Radius = 6;
			this.bassButton.RadiusB = 0;
			this.bassButton.Size = new Size(26, 20);
			this.bassButton.StartColor = Color.Black;
			this.bassButton.StartFactor = 0.35f;
			this.bassButton.TabIndex = 113;
			this.bassButton.CheckedChanged += this.bassButton_CheckedChanged;
			this.tbHighGain.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.tbHighGain.Button = false;
			this.tbHighGain.Checked = false;
			this.tbHighGain.ColorFactor = 0.55f;
			this.tbHighGain.ForeColor = Color.Black;
			this.tbHighGain.Location = new Point(173, 44);
			this.tbHighGain.Margin = new Padding(4);
			this.tbHighGain.Maximum = 30;
			this.tbHighGain.Minimum = 1;
			this.tbHighGain.Name = "tbHighGain";
			this.tbHighGain.Size = new Size(17, 84);
			this.tbHighGain.TabIndex = 107;
			this.tbHighGain.Tag = "1";
			this.tbHighGain.TickColor = Color.Silver;
			this.tbHighGain.Ticks = 5;
			this.tbHighGain.ToolTip = null;
			this.tbHighGain.Value = 10;
			this.tbHighGain.ValueChanged += this.tbHighGain_ValueChanged;
			this.tbMedGain.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.tbMedGain.Button = false;
			this.tbMedGain.Checked = false;
			this.tbMedGain.ColorFactor = 0.6f;
			this.tbMedGain.ForeColor = Color.Black;
			this.tbMedGain.Location = new Point(137, 44);
			this.tbMedGain.Margin = new Padding(4);
			this.tbMedGain.Maximum = 30;
			this.tbMedGain.Minimum = 1;
			this.tbMedGain.Name = "tbMedGain";
			this.tbMedGain.Size = new Size(17, 84);
			this.tbMedGain.TabIndex = 106;
			this.tbMedGain.Tag = "1";
			this.tbMedGain.TickColor = Color.Silver;
			this.tbMedGain.Ticks = 5;
			this.tbMedGain.ToolTip = null;
			this.tbMedGain.Value = 10;
			this.tbMedGain.ValueChanged += this.tbMedGain_ValueChanged;
			this.tbLowGain.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.tbLowGain.Button = false;
			this.tbLowGain.Checked = false;
			this.tbLowGain.ColorFactor = 0.6f;
			this.tbLowGain.ForeColor = Color.Black;
			this.tbLowGain.Location = new Point(101, 44);
			this.tbLowGain.Margin = new Padding(4);
			this.tbLowGain.Maximum = 30;
			this.tbLowGain.Minimum = 1;
			this.tbLowGain.Name = "tbLowGain";
			this.tbLowGain.Size = new Size(17, 84);
			this.tbLowGain.TabIndex = 104;
			this.tbLowGain.Tag = "1";
			this.tbLowGain.TickColor = Color.Silver;
			this.tbLowGain.Ticks = 5;
			this.tbLowGain.ToolTip = null;
			this.tbLowGain.Value = 10;
			this.tbLowGain.ValueChanged += this.tbLowGain_ValueChanged;
			this.enableButton.Arrow = 0;
			this.enableButton.Checked = false;
			this.enableButton.Edge = 0.15f;
			this.enableButton.EndColor = Color.White;
			this.enableButton.EndFactor = 0.2f;
			this.enableButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.enableButton.ForeColor = Color.Orange;
			this.enableButton.Location = new Point(2, 6);
			this.enableButton.Name = "enableButton";
			this.enableButton.NoBorder = false;
			this.enableButton.NoLed = false;
			this.enableButton.RadioButton = false;
			this.enableButton.Radius = 6;
			this.enableButton.RadiusB = 0;
			this.enableButton.Size = new Size(26, 20);
			this.enableButton.StartColor = Color.Black;
			this.enableButton.StartFactor = 0.35f;
			this.enableButton.TabIndex = 17;
			this.enableButton.CheckedChanged += this.enableButton_CheckedChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.panel1);
			base.Name = "EqualizerPanel";
			base.Size = new Size(198, 154);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}

		public EqualizerPanel(ISharpControl control, EqualizerProcessor audioProcessor)
		{
			this.InitializeComponent();
			this._control = control;
			this._audioProcessor = audioProcessor;
			this.tbHighGain.Value = (int)(this._audioProcessor.HighGain * 8f);
			this.tbMedGain.Value = (int)(this._audioProcessor.MidGain * 8f);
			this.tbLowGain.Value = (int)(this._audioProcessor.LowGain * 8f);
			this.numHighCutoff.Value = (int)this._audioProcessor.HighCutoff;
			this.numLowCutoff.Value = (int)this._audioProcessor.LowCutoff;
			this.enableButton.Checked = this._audioProcessor.Enabled;
			this.bassButton.Checked = this._audioProcessor.BassBoost;
		}

		private void enableButton_CheckedChanged(object sender, EventArgs e)
		{
			this._audioProcessor.Enabled = this.enableButton.Checked;
		}

		private void bassButton_CheckedChanged(object sender, EventArgs e)
		{
			this._audioProcessor.BassBoost = this.bassButton.Checked;
		}

		private void numHighCutOff_ValueChanged(object sender, EventArgs e)
		{
			this._audioProcessor.HighCutoff = (float)this.numHighCutoff.Value;
		}

		private void numLowCutOff_ValueChanged(object sender, EventArgs e)
		{
			this._audioProcessor.LowCutoff = (float)this.numLowCutoff.Value;
		}

		private void tbLowGain_ValueChanged(object sender, EventArgs e)
		{
			this._audioProcessor.LowGain = (float)this.tbLowGain.Value / 8f;
		}

		private void tbMedGain_ValueChanged(object sender, EventArgs e)
		{
			this._audioProcessor.MidGain = (float)this.tbMedGain.Value / 8f;
		}

		private void tbHighGain_ValueChanged(object sender, EventArgs e)
		{
			this._audioProcessor.HighGain = (float)this.tbHighGain.Value / 8f;
		}
	}
}
