using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SDRSharp.Radio
{
	public class BarMeter : UserControl
	{
		private int _min;

		private int _max = 100;

		private int _step = 20;

		private float _len = 1f;

		private int tickSize = 5;

		private Graphics _grU;

		private Graphics _grP;

		private LinearGradientBrush _brush;

		private int _timOut = 10;

		private IContainer components;

		private PictureBox pic;

		public int Max => this._max;

		public BarMeter()
		{
			this.InitializeComponent();
		}

		public bool Draw(int value, int timOut = 0)
		{
			if (timOut > 0)
			{
				if (--this._timOut > 0)
				{
					return false;
				}
				this._timOut = timOut;
			}
			float num = (float)value / (float)this._max;
			if ((double)num > 1.0)
			{
				this._step = (int)((float)this._max * num / 5f);
				int num2 = 1;
				for (int i = 1; i <= 4; i++)
				{
					int num3 = num2;
					if (num2 >= this._step)
					{
						break;
					}
					num2 = num3 * 2;
					if (num2 >= this._step)
					{
						break;
					}
					num2 = num3 * 5;
					if (num2 > this._step)
					{
						break;
					}
					num2 = num3 * 10;
				}
				this._step = num2;
				this._max = this._step * 5;
				num = (float)value / (float)this._max;
				this.DrawBackground();
			}
			if (num < this._len)
			{
				this._grP.Clear(Color.FromArgb(50, 50, 50));
			}
			this._grP.FillRectangle(this._brush, 0f, 0f, num * (float)this.pic.Width, (float)this.pic.Height);
			this._len = num;
			return true;
		}

		public void DrawBackground()
		{
			this.DrawBackground(this._min, this._max, this._step);
		}

		public void DrawBackground(int min, int max, int step)
		{
			this._min = min;
			this._max = max;
			this._step = step;
			this._grU.Clear(Color.FromArgb(64, 64, 64));
			using (Font font = new Font("Aerial", 6f))
			{
				using (Pen pen = new Pen(Color.CornflowerBlue))
				{
					for (int i = min; i <= max; i += step)
					{
						string text = i.ToString();
						float num = Math.Max(1f, (float)this.pic.Size.Width * (float)i / (float)max);
						this._grU.DrawLine(pen, num, (float)(this.pic.Location.Y - this.tickSize), num, (float)this.pic.Location.Y);
						float height = this._grU.MeasureString(text, font).Height;
						float num2 = (i == min) ? 2f : this._grU.MeasureString(text, font).Width;
						this._grU.DrawString(text, font, Brushes.CornflowerBlue, num - num2 / 2f, (float)(this.pic.Location.Y - this.tickSize) - height);
					}
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			this.DrawBackground();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this._grU != null)
			{
				this._grU.Dispose();
				this._grP.Dispose();
			}
			this._grU = base.CreateGraphics();
			this._grP = this.pic.CreateGraphics();
			if (this._brush != null)
			{
				this._brush.Dispose();
			}
			Rectangle rect = new Rectangle(0, 0, this.pic.Width, this.pic.Height);
			this._brush = new LinearGradientBrush(rect, Color.Black, Color.White, LinearGradientMode.Vertical);
			ColorBlend colorBlend = new ColorBlend();
			colorBlend.Colors = new Color[3]
			{
				Color.Green,
				Color.LightGreen,
				Color.Green
			};
			colorBlend.Positions = new float[3]
			{
				0f,
				0.33f,
				1f
			};
			this._brush.InterpolationColors = colorBlend;
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
            this.pic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.SuspendLayout();
            // 
            // pic
            // 
            this.pic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pic.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic.Location = new System.Drawing.Point(0, 15);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(190, 20);
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            // 
            // BarMeter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.pic);
            this.Name = "BarMeter";
            this.Size = new System.Drawing.Size(200, 35);
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
