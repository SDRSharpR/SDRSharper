using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	public class gProgress : UserControl
	{
		private int _min;

		private int _max = 60000;

		private int _value;

		private int _position = 1;

		private float _fraction;

		private int _tickSize = 2;

		private int _barPosition;

		private Graphics _graphics;

		private Color _backColor = Color.FromArgb(64, 64, 64);

		private Brush _backBrush;

		private ToolTip _toolTip;

		private Bitmap _bitmap;

		private int _curX = -1;

		private int _curY = -1;

		private static Font LabelFont = new Font("Aerial", 7f);

		private static Font CursorFont = new Font("Aerial", 8f);

		private Rectangle _barRect;

		private Rectangle _drawRect;

		private Point _point;

		private Size _size;

		private IContainer components;

		public long Max => this._max;

		public long Min => this._min;

		public float Fraction => this._fraction;

		public ToolTip ToolTip
		{
			get
			{
				return this._toolTip;
			}
			set
			{
				this._toolTip = value;
			}
		}

		public event EventHandler ValueChanged;

		public gProgress()
		{
			this.InitializeComponent();
			this._backBrush = new SolidBrush(this._backColor);
		}

		public void Draw(int value)
		{
			if (value >= 0)
			{
				int num = Math.Max(1, Math.Min(base.Width, (int)((float)(value - this._min) / (float)(this._max - this._min) * (float)(base.Width - 2))));
				if (this._curX > 0)
				{
					this.drawBackground();
					string text = this.time((int)((float)this._curX / (float)base.Width * (float)(this._max - this._min)), 0);
					SizeF sizeF = this._graphics.MeasureString(text, gProgress.CursorFont);
					this._graphics.DrawString(text, gProgress.CursorFont, Brushes.Yellow, (float)this._curX - sizeF.Width / 2f, 0f);
					base.Invalidate();
				}
				else if (num < this._position)
				{
					this._position = 1;
					this._graphics.FillRectangle(this._backBrush, this._barRect);
					base.Invalidate(this._barRect);
				}
				if (num != this._position)
				{
					this._point.X = this._position;
					this._size.Width = num - this._position;
					this._drawRect.Location = this._point;
					this._drawRect.Size = this._size;
					this._graphics.FillRectangle(Brushes.Gray, this._drawRect);
					if (this._curX < 0 && num > this._position)
					{
						base.Invalidate(this._drawRect);
					}
					this._position = num;
					this._value = value;
				}
			}
		}

		public void DrawBackground(int min, int max)
		{
			if (max > 0)
			{
				this._min = min;
			}
			if (max > 0)
			{
				this._max = max;
			}
			this.drawBackground();
			base.Invalidate();
		}

		private void drawBackground()
		{
			int num = 300;
			int num2 = (this._max - this._min) / 100;
			if (num2 <= 10)
			{
				num = 1;
			}
			else if (num2 <= 50)
			{
				num = 5;
			}
			else if (num2 <= 100)
			{
				num = 10;
			}
			else if (num2 <= 300)
			{
				num = 30;
			}
			else if (num2 <= 600)
			{
				num = 60;
			}
			else if (num2 <= 1200)
			{
				num = 120;
			}
			num *= 100;
			this._position = 1;
			this._graphics.Clear(this._backColor);
			this._graphics.DrawRectangle(Pens.Black, 0, this._barPosition, base.Width - 1, base.Height - this._barPosition - 1);
			for (int i = this._min; i < this._max + num; i += num)
			{
				int num3 = Math.Min(i, this._max);
				string text = this.time(num3, num);
				SizeF sizeF = this._graphics.MeasureString(text, gProgress.LabelFont);
				if (num3 == this._min)
				{
					sizeF.Width = 4f;
				}
				else if (num3 == this._max)
				{
					sizeF.Width *= 2f;
				}
				float num4 = Math.Max(0f, Math.Min((float)(base.Width - 1), (float)base.Width * (float)num3 / (float)(this._max - this._min)));
				if (!(num4 > (float)base.Width - 2f * sizeF.Width) || !(num4 < (float)(base.Width - 1)))
				{
					this._barPosition = (int)sizeF.Height + this._tickSize;
					this._graphics.DrawString(text, gProgress.LabelFont, Brushes.Orange, num4 - sizeF.Width / 2f, 0f);
					this._graphics.DrawLine(Pens.Orange, num4, sizeF.Height - 1f, num4, (float)base.Height);
				}
			}
		}

		private string time(int pos, int step)
		{
			if (pos == 0)
			{
				return "0";
			}
			if (step < 6000)
			{
				return (pos / 6000).ToString() + "m" + (pos % 6000 / 100).ToString("00");
			}
			return (pos / 6000).ToString() + " m";
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawImageUnscaled(this._bitmap, 0, 0);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (base.Width > 0 && base.Height > 0)
			{
				if (this._bitmap != null)
				{
					this._bitmap.Dispose();
				}
				if (this._graphics != null)
				{
					this._graphics.Dispose();
				}
				this._bitmap = new Bitmap(base.Width, base.Height);
				this._graphics = Graphics.FromImage(this._bitmap);
				this._point = new Point(1, this._barPosition + 1);
				this._size = new Size(base.Width - 2, base.Height - this._barPosition - 2);
				this._barRect = new Rectangle(this._point, this._size);
				this._drawRect = new Rectangle(this._point, this._size);
				this.drawBackground();
				base.Invalidate();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this._fraction = (float)e.X / (float)base.Width;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this._curX = e.X;
			this._curY = e.Y;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.Cursor = Cursors.Default;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.Cursor = Cursors.Default;
			this._curX = -1;
			this._curY = -1;
			this.drawBackground();
			base.Invalidate();
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "gProgress";
			base.Size = new Size(200, 31);
			base.ResumeLayout(false);
		}
	}
}
