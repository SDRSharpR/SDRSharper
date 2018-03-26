using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	[DefaultEvent("TextChanged")]
	public class gLabel : UserControl
	{
		private string _text;

		private IContainer components;

		private BorderGradientPanel gradientPanel;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				this.gradientPanel.Invalidate();
			}
		}

		public gLabel()
		{
			this.InitializeComponent();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.gradientPanel.Height = base.Height;
			this.gradientPanel.Width = base.Width;
		}

		private void gradientPanel_Paint(object sender, PaintEventArgs e)
		{
			using (Brush brush = new SolidBrush(this.ForeColor))
			{
				e.Graphics.DrawString(this._text, this.Font, brush, 3f, ((float)this.gradientPanel.Height - e.Graphics.MeasureString(this._text, this.Font).Height) / 2f);
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
			this.gradientPanel = new BorderGradientPanel();
			base.SuspendLayout();
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
			this.gradientPanel.Paint += this.gradientPanel_Paint;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.gradientPanel);
			base.Name = "gLabel";
			base.Size = new Size(150, 55);
			base.ResumeLayout(false);
		}
	}
}
