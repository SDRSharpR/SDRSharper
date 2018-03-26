using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	public class BorderGradientPanel : Control
	{
		public static int mode;

		private Color _startColor = Color.Black;

		private Color _endColor = Color.White;

		private float _startFactor = 0.35f;

		private float _endFactor = 0.2f;

		private Color _borderColor = Color.Black;

		private bool _noBorder;

		private float _edge = 0.15f;

		private int _radius = 8;

		private int _radiusB;

		public bool NoBorder
		{
			get
			{
				return this._noBorder;
			}
			set
			{
				this._noBorder = value;
			}
		}

		public int Radius
		{
			get
			{
				return this._radius;
			}
			set
			{
				this._radius = value;
			}
		}

		public int RadiusB
		{
			get
			{
				return this._radiusB;
			}
			set
			{
				this._radiusB = value;
			}
		}

		public float Edge
		{
			get
			{
				return this._edge;
			}
			set
			{
				this._edge = value;
			}
		}

		[Description("Top of panel/button")]
		public Color EndColor
		{
			get
			{
				return this._endColor;
			}
			set
			{
				if (!(this._endColor == value))
				{
					this._endColor = value;
					base.Invalidate();
				}
			}
		}

		[Description("Bottom of panel/button")]
		public Color StartColor
		{
			get
			{
				return this._startColor;
			}
			set
			{
				if (!(this._startColor == value))
				{
					this._startColor = value;
					base.Invalidate();
				}
			}
		}

		[Description("Top of panel/button")]
		public float StartFactor
		{
			get
			{
				return this._startFactor;
			}
			set
			{
				if (this._startFactor != value)
				{
					this._startFactor = value;
					base.Invalidate();
				}
			}
		}

		[Description("Bottom of panel/button")]
		public float EndFactor
		{
			get
			{
				return this._endFactor;
			}
			set
			{
				if (this._endFactor != value)
				{
					this._endFactor = value;
					base.Invalidate();
				}
			}
		}

		public BorderGradientPanel()
		{
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			this.ShowFocus(true);
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			this.ShowFocus(false);
		}

		public void ShowFocus(bool focus)
		{
			this._borderColor = (focus ? Color.Gray : Color.Black);
			base.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Blend blend = new Blend();
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, base.Height), this._startColor, this._endColor);
			blend.Positions = new float[6]
			{
				0f,
				this._edge / 3f,
				this._edge,
				1f - this._edge,
				1f - this._edge / 3f,
				1f
			};
			blend.Factors = new float[6]
			{
				1f,
				1f,
				this._startFactor,
				this._endFactor,
				0f,
				0f
			};
			linearGradientBrush.Blend = blend;
			e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
			SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			bool flag = this is gButton;
			Rectangle rect = (!flag) ? new Rectangle(-1, -1, base.ClientRectangle.Width + 1, base.ClientRectangle.Height + 1) : new Rectangle(0, -1, base.ClientRectangle.Width - 1, base.ClientRectangle.Height + 1);
			GraphicsPath path = RoundedRectangle.Create(rect, this._radius, this._radiusB);
			if (!this.NoBorder)
			{
				int num = flag ? 3 : 2;
				using (Pen pen = new Pen(this._borderColor, (float)num))
				{
					e.Graphics.DrawPath(pen, path);
				}
			}
			path = RoundedRectangle.Create(base.ClientRectangle, this._radius, this._radiusB);
			base.Region = new Region(path);
			e.Graphics.SmoothingMode = smoothingMode;
			linearGradientBrush.Dispose();
			base.OnPaint(e);
		}
	}
}
