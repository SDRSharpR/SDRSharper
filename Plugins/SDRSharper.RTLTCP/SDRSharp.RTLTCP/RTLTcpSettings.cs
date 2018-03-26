using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.RTLTCP
{
	public class RTLTcpSettings : Form
	{
		private IContainer components;

		private TextBox hostBox;

		private TextBox portBox;

		private TextBox srBox;

		private Button button1;

		private Label label1;

		private Label label2;

		private Label label3;

		private RadioButton autoRB;

		private RadioButton manualRB;

		private Label label4;

		private TextBox gainBox;

		private Label label5;

		private TextBox fcBox;

		private RtlTcpIO _owner;

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
			this.hostBox = new TextBox();
			this.portBox = new TextBox();
			this.srBox = new TextBox();
			this.button1 = new Button();
			this.label1 = new Label();
			this.label2 = new Label();
			this.label3 = new Label();
			this.autoRB = new RadioButton();
			this.manualRB = new RadioButton();
			this.label4 = new Label();
			this.gainBox = new TextBox();
			this.label5 = new Label();
			this.fcBox = new TextBox();
			base.SuspendLayout();
			this.hostBox.Location = new Point(139, 11);
			this.hostBox.Name = "hostBox";
			this.hostBox.Size = new Size(133, 20);
			this.hostBox.TabIndex = 0;
			this.portBox.Location = new Point(139, 37);
			this.portBox.Name = "portBox";
			this.portBox.Size = new Size(133, 20);
			this.portBox.TabIndex = 1;
			this.srBox.Location = new Point(139, 88);
			this.srBox.Name = "srBox";
			this.srBox.Size = new Size(133, 20);
			this.srBox.TabIndex = 2;
			this.button1.Location = new Point(133, 213);
			this.button1.Name = "button1";
			this.button1.Size = new Size(139, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Update Settings";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += this.button1_Click;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(27, 11);
			this.label1.Name = "label1";
			this.label1.Size = new Size(55, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Hostname";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(27, 37);
			this.label2.Name = "label2";
			this.label2.Size = new Size(26, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Port";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(27, 88);
			this.label3.Name = "label3";
			this.label3.Size = new Size(68, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Sample Rate";
			this.autoRB.AutoSize = true;
			this.autoRB.Location = new Point(30, 123);
			this.autoRB.Name = "autoRB";
			this.autoRB.Size = new Size(72, 17);
			this.autoRB.TabIndex = 7;
			this.autoRB.TabStop = true;
			this.autoRB.Text = "Auto Gain";
			this.autoRB.UseVisualStyleBackColor = true;
			this.autoRB.CheckedChanged += this.autoRB_CheckedChanged;
			this.manualRB.AutoSize = true;
			this.manualRB.Location = new Point(139, 123);
			this.manualRB.Name = "manualRB";
			this.manualRB.Size = new Size(85, 17);
			this.manualRB.TabIndex = 8;
			this.manualRB.TabStop = true;
			this.manualRB.Text = "Manual Gain";
			this.manualRB.UseVisualStyleBackColor = true;
			this.manualRB.CheckedChanged += this.manualRB_CheckedChanged;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(27, 151);
			this.label4.Name = "label4";
			this.label4.Size = new Size(101, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Gain setting (dB*10)";
			this.gainBox.Location = new Point(139, 148);
			this.gainBox.Name = "gainBox";
			this.gainBox.Size = new Size(133, 20);
			this.gainBox.TabIndex = 10;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(27, 181);
			this.label5.Name = "label5";
			this.label5.Size = new Size(108, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Freq Correction (ppm)";
			this.fcBox.Location = new Point(139, 181);
			this.fcBox.Name = "fcBox";
			this.fcBox.Size = new Size(133, 20);
			this.fcBox.TabIndex = 12;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(284, 248);
			base.Controls.Add(this.fcBox);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.gainBox);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.manualRB);
			base.Controls.Add(this.autoRB);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.srBox);
			base.Controls.Add(this.portBox);
			base.Controls.Add(this.hostBox);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RTLTcpSettings";
			base.ShowInTaskbar = false;
			this.Text = "RTLTcpSettings";
			base.TopMost = true;
			base.Load += this.RTLTcpSettings_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public RTLTcpSettings(RtlTcpIO owner)
		{
			this._owner = owner;
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this._owner.hostName = this.hostBox.Text;
			this._owner.port = Convert.ToUInt16(this.portBox.Text);
			this._owner.Samplerate = Convert.ToDouble(this.srBox.Text);
			if (this.autoRB.Checked)
			{
				this._owner.GainMode = 0u;
			}
			else
			{
				this._owner.GainMode = 1u;
				this._owner.Gain = Convert.ToInt32(this.gainBox.Text);
			}
			this._owner.FreqCorrection = Convert.ToInt32(this.fcBox.Text);
		}

		private void RTLTcpSettings_Load(object sender, EventArgs e)
		{
			this.hostBox.Text = this._owner.hostName;
			this.portBox.Text = this._owner.port.ToString();
			this.srBox.Text = this._owner.Samplerate.ToString();
			this.gainBox.Text = this._owner.Gain.ToString();
			if (this._owner.GainMode == 0)
			{
				this.autoRB.Checked = true;
				this.manualRB.Checked = false;
				this.gainBox.Enabled = false;
			}
			else
			{
				this.autoRB.Checked = false;
				this.manualRB.Checked = true;
				this.gainBox.Enabled = true;
			}
			this.fcBox.Text = this._owner.FreqCorrection.ToString();
		}

		private void updateRB()
		{
			this.gainBox.Enabled = this.manualRB.Checked;
		}

		private void manualRB_CheckedChanged(object sender, EventArgs e)
		{
			this.updateRB();
		}

		private void autoRB_CheckedChanged(object sender, EventArgs e)
		{
			this.updateRB();
		}
	}
}
