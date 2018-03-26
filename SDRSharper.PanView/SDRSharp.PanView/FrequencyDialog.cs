using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	public class FrequencyDialog : Form
	{
		private IContainer components;

		private TextBox txtFrequency;

		private Button butKhz;

		private Button btnMhz;

		private Label label1;

		public string Frequency
		{
			get
			{
				return this.txtFrequency.Text;
			}
			set
			{
				this.txtFrequency.Text = value;
			}
		}

		public FrequencyDialog()
		{
			this.InitializeComponent();
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
			this.txtFrequency = new TextBox();
			this.butKhz = new Button();
			this.btnMhz = new Button();
			this.label1 = new Label();
			base.SuspendLayout();
			this.txtFrequency.Font = new Font("Microsoft Sans Serif", 10.2f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtFrequency.Location = new Point(11, 24);
			this.txtFrequency.Margin = new Padding(2, 2, 2, 2);
			this.txtFrequency.Name = "txtFrequency";
			this.txtFrequency.Size = new Size(91, 23);
			this.txtFrequency.TabIndex = 0;
			this.butKhz.DialogResult = DialogResult.Yes;
			this.butKhz.Location = new Point(12, 55);
			this.butKhz.Margin = new Padding(2, 2, 2, 2);
			this.butKhz.Name = "butKhz";
			this.butKhz.Size = new Size(38, 19);
			this.butKhz.TabIndex = 1;
			this.butKhz.Text = "kHz";
			this.butKhz.UseVisualStyleBackColor = true;
			this.btnMhz.DialogResult = DialogResult.No;
			this.btnMhz.Location = new Point(64, 55);
			this.btnMhz.Margin = new Padding(2, 2, 2, 2);
			this.btnMhz.Name = "btnMhz";
			this.btnMhz.Size = new Size(38, 19);
			this.btnMhz.TabIndex = 2;
			this.btnMhz.Text = "MHz";
			this.btnMhz.UseVisualStyleBackColor = true;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(16, 6);
			this.label1.Margin = new Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(82, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Enter frequency";
			base.AcceptButton = this.butKhz;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(115, 80);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnMhz);
			base.Controls.Add(this.butKhz);
			base.Controls.Add(this.txtFrequency);
			base.Margin = new Padding(2, 2, 2, 2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrequencyDialog";
			this.Text = "Frequency";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
