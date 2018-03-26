using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	[DefaultEvent("TextChanged")]
	public class gTextBox : UserControl
	{
		private IContainer components;

		private TextBox textBox1;

		private BorderGradientPanel gradientPanel;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return this.textBox1.Text;
			}
			set
			{
				this.SetText(value);
			}
		}

		public new event EventHandler TextChanged;

		public gTextBox()
		{
			this.InitializeComponent();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.gradientPanel.Height = base.Height;
			this.gradientPanel.Width = base.Width;
			this.textBox1.Left = 5;
			this.textBox1.Width = this.gradientPanel.Width - this.textBox1.Left - 5;
			this.textBox1.Top = (base.Height - this.textBox1.Height) / 2 + 1;
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.textBox1.Font = this.Font;
			base.Invalidate();
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.textBox1.ForeColor = this.ForeColor;
			base.Invalidate();
		}

		private void SetText(string value)
		{
			if (!(this.textBox1.Text == value))
			{
				this.textBox1.Text = value;
				if (this.TextChanged != null)
				{
					this.TextChanged(this, new EventArgs());
				}
			}
		}

		private void textBox1_Validating(object sender, CancelEventArgs e)
		{
			if (this.TextChanged != null)
			{
				this.TextChanged(this, new EventArgs());
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
			this.textBox1 = new TextBox();
			this.gradientPanel = new BorderGradientPanel();
			base.SuspendLayout();
			this.textBox1.BackColor = Color.FromArgb(50, 50, 50);
			this.textBox1.BorderStyle = BorderStyle.None;
			this.textBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.textBox1.ForeColor = Color.Yellow;
			this.textBox1.Location = new Point(13, 10);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(116, 13);
			this.textBox1.TabIndex = 18;
			this.textBox1.Text = "xxxx";
			this.textBox1.WordWrap = false;
			this.textBox1.Validating += this.textBox1_Validating;
			this.gradientPanel.BackColor = Color.Black;
			this.gradientPanel.Edge = 0.18f;
			this.gradientPanel.EndColor = Color.Black;
			this.gradientPanel.EndFactor = 0.6f;
			this.gradientPanel.Location = new Point(0, 0);
			this.gradientPanel.Margin = new Padding(2);
			this.gradientPanel.Name = "gradientPanel";
			this.gradientPanel.NoBorder = true;
			this.gradientPanel.Radius = 6;
			this.gradientPanel.RadiusB = 0;
			this.gradientPanel.Size = new Size(138, 38);
			this.gradientPanel.StartColor = Color.Gray;
			this.gradientPanel.StartFactor = 0.6f;
			this.gradientPanel.TabIndex = 17;
			this.gradientPanel.TabStop = false;
			this.gradientPanel.Text = "borderGradientPanel3";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.gradientPanel);
			base.Name = "gTextBox";
			base.Size = new Size(150, 55);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
