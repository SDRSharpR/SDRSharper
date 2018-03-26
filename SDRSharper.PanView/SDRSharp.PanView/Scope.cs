using SDRSharp.Radio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	public class Scope : UserControl
	{
		public delegate void delPositionChanged(object sender, PositionEventArgs e);

		private const int GradientAlpha = 180;

		private const int AxisMargin = 5;

		private const int LabelMargin = 16;

		private const int TicksMargin = 5;

		private const int BarSize = 10;

		private const float TimeConst = 0.001f;

		private const float Amplification = 100f;

		private int _carrierAvg = 2000;

		private float _audioRel = 5E-07f;

		private bool _showLines;

		private int _audioAvg = 25;

		private int _peakDelay = 20;

		private float _peakRel = 0.01f;

		private int _scopeHeight;

		private int _barsHeight;

		private bool _showBars;

		private int _yNeg;

		private int _yPos;

		private Bitmap _bkgBuffer;

		private Bitmap _buffer;

		private Graphics _graphics;

		private Point[] _points;

		private Point[] _upper;

		private Point[] _lower;

		private int _uppIdx;

		private int _lowIdx;

		private bool _performNeeded;

		private bool _drawBackgroundNeeded;

		private int _oldX;

		private int _oldY;

		private float _oldHshift;

		private float _oldVshift;

		private DcRemover _dcRemover1 = new DcRemover(0.001f);

		private DcRemover _dcRemover2 = new DcRemover(0.001f);

		private float[] _vBuf;

		private float[] _hBuf;

		private DemodType _vChannel;

		private DemodType _hChannel;

		private float _hShift;

		private float _vShift;

		private float _vDiv;

		private float _hDiv;

		private float _tDiv;

		private bool _hInvert;

		private bool _vInvert;

		private bool _hBlockDC;

		private bool _vBlockDC;

		private bool _xyMode;

		private float _trigLevel;

		private bool _trigChange;

		private float _average;

		private float _neg;

		private float _pos;

		private float[] _avgBuf;

		private float[] _negBuf;

		private float[] _posBuf;

		private int _posTmo;

		private int _negTmo;

		private float _posPeak;

		private float _negPeak;

		private float _posAver;

		private float _negAver;

		private int _timeOut;

		private string _statusText;

		private static Pen _gridPen = new Pen(Color.FromArgb(100, 100, 100));

		private static Pen _axisPen = new Pen(Color.FromArgb(100, 100, 250));

		private static Pen _carrierPen = new Pen(Color.Red);

		private static Pen _audioPen = new Pen(Color.Yellow);

		private static Brush _audioBrush = new SolidBrush(Color.FromArgb(225, Color.LightGreen));

		private static Brush _clippingBrush = new SolidBrush(Color.FromArgb(205, Color.Red));

		private static Brush _overmodBrush = new SolidBrush(Color.FromArgb(190, Color.Yellow));

		private static Pen _PeakPen = new Pen(Color.Red, 3f);

		private static Pen _AverPen = new Pen(Color.Yellow, 3f);

		private static Pen _tracePen = new Pen(Color.White);

		private static Font _font = new Font("Arial", 7f);

		private static Font _statusFont = new Font("Lucida Console", 9f);

		private static ColorBlend _gradientColorBlend;

		private static int[] _gradientPixels = new int[255];

		private Rectangle _clipRectangle;

		private LinearGradientBrush _gradientBrush;

		private static int _spectrumFill;

		private static Color _traceColor;

		private static Color _backgroundColor;

		private PathGradientBrush _backgroundBrush;

		public int CarrierAvg
		{
			get
			{
				return this.log((float)this._carrierAvg / 2000f);
			}
			set
			{
				this._carrierAvg = (int)(this.pow(value) * 2000f);
			}
		}

		public int AudioRel
		{
			get
			{
				return this.log(this._audioRel / 5E-07f);
			}
			set
			{
				this._audioRel = this.pow(value) * 5E-07f;
			}
		}

		public int AudioAvg
		{
			get
			{
				return this.log((float)this._audioAvg / 25f);
			}
			set
			{
				this._audioAvg = (int)(this.pow(value) * 25f);
			}
		}

		public int PeakDelay
		{
			get
			{
				return this.log((float)this._peakDelay / 20f);
			}
			set
			{
				this._peakDelay = (int)(this.pow(value) * 20f);
			}
		}

		public int PeakRel
		{
			get
			{
				return this.log(this._peakRel / 0.01f);
			}
			set
			{
				this._peakRel = this.pow(value) * 0.01f;
			}
		}

		public float Vdiv
		{
			get
			{
				return this._vDiv;
			}
			set
			{
				this._vDiv = value;
			}
		}

		public float Hdiv
		{
			get
			{
				return this._hDiv;
			}
			set
			{
				this._hDiv = value;
			}
		}

		public float Tdiv
		{
			get
			{
				return this._tDiv;
			}
			set
			{
				this._tDiv = value;
			}
		}

		public float Vshift
		{
			get
			{
				return this._vShift;
			}
			set
			{
				this._vShift = value;
				this._drawBackgroundNeeded = true;
			}
		}

		public float Hshift
		{
			get
			{
				return this._hShift;
			}
			set
			{
				this._hShift = value;
			}
		}

		public bool Vinvert
		{
			get
			{
				return this._vInvert;
			}
			set
			{
				this._vInvert = value;
			}
		}

		public bool Hinvert
		{
			get
			{
				return this._hInvert;
			}
			set
			{
				this._hInvert = value;
			}
		}

		public bool HblockDC
		{
			get
			{
				return this._hBlockDC;
			}
			set
			{
				this._hBlockDC = value;
			}
		}

		public bool VblockDC
		{
			get
			{
				return this._vBlockDC;
			}
			set
			{
				this._vBlockDC = value;
			}
		}

		public bool XYmode
		{
			get
			{
				return this._xyMode;
			}
			set
			{
				this._xyMode = value;
				this._drawBackgroundNeeded = true;
			}
		}

		public DemodType Vchannel
		{
			get
			{
				return this._vChannel;
			}
			set
			{
				this._vChannel = value;
				this._drawBackgroundNeeded = true;
			}
		}

		public DemodType Hchannel
		{
			get
			{
				return this._hChannel;
			}
			set
			{
				this._hChannel = value;
			}
		}

		public float TrigLevel
		{
			get
			{
				return this._trigLevel;
			}
			set
			{
				this._trigLevel = value;
				this._drawBackgroundNeeded = true;
			}
		}

		public bool ShowLines
		{
			get
			{
				return this._showLines;
			}
			set
			{
				this._showLines = value;
			}
		}

		public bool ShowBars
		{
			get
			{
				return this._showBars;
			}
			set
			{
				this._showBars = value;
			}
		}

		public int SpectrumFill
		{
			get
			{
				return Scope._spectrumFill;
			}
			set
			{
				Scope._spectrumFill = value;
			}
		}

		public Color BackgoundColor
		{
			get
			{
				return Scope._backgroundColor;
			}
			set
			{
				this.newBackgroundBrush(value);
			}
		}

		public string StatusText
		{
			get
			{
				return this._statusText;
			}
			set
			{
				this._statusText = value;
				this._performNeeded = true;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ColorBlend GradientColorBlend
		{
			get
			{
				return Scope._gradientColorBlend;
			}
			set
			{
				this.newGradientBrush(value);
			}
		}

		public Color TraceColor
		{
			get
			{
				return Scope._traceColor;
			}
			set
			{
				if (!(Scope._traceColor == value))
				{
					Scope._traceColor = value;
					Scope._tracePen.Dispose();
					Scope._tracePen = new Pen(Scope._traceColor);
				}
			}
		}

		public event delPositionChanged XYPositionChanged;

		public Scope()
		{
			this._bkgBuffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
			this._buffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
			this._graphics = Graphics.FromImage(this._buffer);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.UpdateStyles();
			Scope._audioPen.DashStyle = DashStyle.Dot;
			Scope._carrierPen.DashStyle = DashStyle.Dot;
			Scope._audioPen.DashStyle = DashStyle.Dot;
			Scope._gridPen.DashStyle = DashStyle.Dot;
			Scope._axisPen.DashStyle = DashStyle.Dash;
		}

		~Scope()
		{
			this._buffer.Dispose();
			this._graphics.Dispose();
			this._gradientBrush.Dispose();
			this._backgroundBrush.Dispose();
		}

		public void Reset()
		{
			this._neg = 0f;
			this._pos = 0f;
		}

		private void fillGradientVector(ColorBlend blend, int size)
		{
			if (blend != null)
			{
				int num = 4;
				Bitmap bitmap = new Bitmap(num, size, PixelFormat.Format32bppArgb);
				Graphics graphics = Graphics.FromImage(bitmap);
				LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, size), Color.Black, Color.White);
				linearGradientBrush.InterpolationColors = blend;
				graphics.FillRectangle(linearGradientBrush, new Rectangle(0, 0, num, size));
				int num2 = 255;
				for (int i = 0; i < size; i++)
				{
					if (i < size / 20)
					{
						Scope._gradientPixels[i] = Color.White.ToArgb();
					}
					else
					{
						num2 = 255 - i * 60 / size;
						Color color = Color.FromArgb(num2, bitmap.GetPixel(num / 2, i));
						Scope._gradientPixels[i] = color.ToArgb();
					}
				}
				graphics.Dispose();
				bitmap.Dispose();
			}
		}

		public void Perform(bool hideTrace = false)
		{
			if (this._drawBackgroundNeeded)
			{
				this.DrawBackground();
			}
			if (this._performNeeded || this._drawBackgroundNeeded)
			{
				this.DrawForeground(hideTrace);
				this.DrawStatusText();
				base.Invalidate();
			}
			this._performNeeded = false;
			this._drawBackgroundNeeded = false;
		}

		public unsafe void Render(float* xBuf, float* yBuf, int length, int latency, int timeOut)
		{
			this._timeOut = Math.Min(this._timeOut, timeOut);
			if (--this._timeOut <= 0 && yBuf != null)
			{
				this._timeOut = timeOut;
				float num = (float)latency / 1000f;
				num = this._tDiv * 10f / num;
				int num2 = Math.Min(length, (int)(num * (float)length));
				int num3 = Math.Min(length, base.ClientRectangle.Width - 10);
				float factor = (float)num2 / (float)num3;
				int num4 = 0;
				float[] vBuf = this._vBuf;
				fixed (float* ptr = vBuf)
				{
					bool flag = (double)this._trigLevel < 0.45 && (double)this._trigLevel > -0.45;
					if (flag)
					{
						if (this._vBlockDC)
						{
							this._dcRemover1.Process(yBuf, length);
						}
						num4 = this.TriggerPoint(yBuf, num2);
					}
					Fourier.SubCopy(yBuf + num4, ptr, length - num4, num3, factor);
					if (!flag && this._vBlockDC)
					{
						this._dcRemover1.Process(ptr, num3);
					}
				}
				if (this._xyMode)
				{
					float[] hBuf = this._hBuf;
					fixed (float* ptr2 = hBuf)
					{
						Fourier.SubCopy(xBuf + num4, ptr2, length - num4, num3, factor);
						if (this._hBlockDC)
						{
							this._dcRemover2.Process(ptr2, num3);
						}
					}
				}
				this._performNeeded = true;
			}
		}

		private unsafe int TriggerPoint(float* buf, int len)
		{
			float num = (this._trigLevel - this._vShift) * 0.8f * this._vDiv / 100f;
			for (int i = 1; i < len - 1; i++)
			{
				if (buf[i] >= num && buf[i - 1] < num)
				{
					return i;
				}
			}
			return 0;
		}

		private void DrawBackground()
		{
			if (Scope._gradientColorBlend != null)
			{
				float num10 = this._average * 100f / (this._vDiv * 0.8f);
				using (Graphics graphics = Graphics.FromImage(this._bkgBuffer))
				{
					Scope.ConfigureGraphics(graphics);
					graphics.Clear(Color.FromArgb(40, 40, 40));
					graphics.FillRectangle(this._backgroundBrush, 0, 0, base.ClientRectangle.Width, this._scopeHeight);
					int num = this._scopeHeight - 10;
					float num2 = (float)num / 8f;
					float num3 = (float)(base.ClientRectangle.Width - 10) / 10f;
					float num4 = (!this._vBlockDC) ? (this._vShift * (float)num) : 0f;
					for (int i = -8; i < 17; i++)
					{
						float num5 = 5f + (float)i * num2 - num4;
						if (num5 < (float)this._scopeHeight)
						{
							graphics.DrawLine(Scope._gridPen, 5f, num5, (float)(base.ClientRectangle.Width - 5), num5);
						}
					}
					for (int j = 0; j < 11; j++)
					{
						float num6 = 5f + (float)j * num3;
						graphics.DrawLine(Scope._gridPen, num6, 5f, num6, (float)(this._scopeHeight - 5));
					}
					graphics.DrawLine(Scope._axisPen, base.ClientRectangle.Width / 2, 5, base.ClientRectangle.Width / 2, this._scopeHeight - 5);
					float num7 = (float)(this._scopeHeight / 2) - num4;
					if (num7 < (float)this._scopeHeight)
					{
						graphics.DrawLine(Scope._axisPen, 5f, num7, (float)(base.ClientRectangle.Width - 5), num7);
					}
					num7 = (float)(this._scopeHeight / 2) - this._trigLevel * (float)num;
					if (!this.XYmode)
					{
						graphics.DrawLine(Pens.Yellow, 0f, num7 + 2f, 2f, num7);
						graphics.DrawLine(Pens.Yellow, 0f, num7 - 2f, 2f, num7);
					}
					if (this._showBars)
					{
						using (Pen pen = new Pen(this.BackColor, 3f))
						{
							graphics.DrawLine(pen, 0, this._scopeHeight, base.ClientRectangle.Width, this._scopeHeight);
						}
						int num8 = base.ClientRectangle.Width - 10;
						num = this._scopeHeight - 10;
						num3 = (float)num8 / 15f;
						this._yNeg = base.ClientRectangle.Height - 16;
						this._yPos = this._yNeg - 16 - 5 - 10;
						graphics.DrawLine(Scope._axisPen, 5f, (float)this._yPos, 5f + num3 * 15f, (float)this._yPos);
						graphics.DrawLine(Scope._axisPen, 5f, (float)this._yNeg, 5f + num3 * 10f, (float)this._yNeg);
						for (int k = 0; k < 15; k++)
						{
							int num9 = (int)(5f + num3 * (float)k);
							graphics.DrawLine(Scope._axisPen, num9, this._yPos, num9, this._yPos - 3);
							if (k == 0)
							{
								graphics.DrawString("0", Scope._font, Brushes.LightGray, (float)num9, (float)(this._yPos + 1));
							}
							else if (k % 2 == 0)
							{
								graphics.DrawString((k * 10).ToString(), Scope._font, Brushes.LightGray, (float)((k < 10) ? (num9 - 6) : (num9 - 9)), (float)(this._yPos + 1));
							}
							if (k < 11)
							{
								graphics.DrawLine(Scope._axisPen, num9, this._yNeg, num9, this._yNeg - 3);
								if (k == 0)
								{
									graphics.DrawString("0", Scope._font, Brushes.LightGray, (float)num9, (float)(this._yNeg + 1));
								}
								else if (k % 2 == 0)
								{
									graphics.DrawString((k * 10).ToString(), Scope._font, Brushes.LightGray, (float)((k < 10) ? (num9 - 6) : (num9 - 9)), (float)(this._yNeg + 1));
								}
							}
						}
					}
				}
			}
		}

		private void DrawTrace()
		{
			int num;
			int num2;
			float num4;
			bool flag;
			int num13;
			int num14;
			Point[] array;
			Point[] array2;
			if (this._vBuf != null && this._vBuf.Length != 0)
			{
				if (this._hDiv == 0f)
				{
					this._hDiv = 1f;
				}
				if (this._vDiv == 0f)
				{
					this._vDiv = 1f;
				}
				num = base.ClientRectangle.Width - 10;
				num2 = this._scopeHeight - 10;
				float num3 = this.XYmode ? ((float)num / this._hDiv * 100f) : ((float)(num / this._vBuf.Length));
				num4 = (float)num2 / this._vDiv * 100f * 1.25f;
				int num5 = 0;
				int num6 = 0;
				float num7 = 0f;
				float num8 = 0f;
				for (int i = 0; i < this._hBuf.Length; i++)
				{
					if (!this._xyMode)
					{
						num8 = (float)(-num / 2) + (float)i * num3;
						num7 = this._vBuf[i];
					}
					else if (this.Vchannel == DemodType.AM && this.Hchannel == DemodType.PM)
					{
						float num9 = this._vBuf[i];
						float num10 = this._hBuf[i] * 500000f;
						num8 = (float)((double)num9 * Math.Sin((double)num10));
						num7 = (float)((double)num9 * Math.Cos((double)num10));
					}
					else
					{
						num8 = this._hBuf[i];
						num7 = this._vBuf[i];
					}
					if (this._hInvert)
					{
						num8 = 0f - num8;
					}
					if (this._vInvert)
					{
						num7 = 0f - num7;
					}
					num5 = (int)((float)(5 + num / 2) + num8 * num3);
					num6 = (int)((float)(this._scopeHeight - 5 - num2 / 2) - num7 * num4);
					this._points[i + 1].X = num5 + (int)((float)num * this.Hshift);
					this._points[i + 1].Y = num6 - (int)((float)num2 * this.Vshift);
				}
				this._points[0] = this._points[1];
				this._points[this._points.Length - 1] = this._points[this._points.Length - 2];
				this._graphics.SetClip(this._clipRectangle);
				flag = false;
				flag = (this._vChannel == DemodType.Envelope && !this._xyMode);
				if (flag)
				{
					int num11 = 0;
					int num12 = this._scopeHeight - (int)((float)(2 * num2) * this.Vshift);
					num13 = this._scopeHeight / 2 - (int)((float)num2 * this.Vshift);
					this._lowIdx = (this._uppIdx = 0);
					this.addUpper(0, num13);
					this.addLower(0, num13);
					num14 = 10;
					for (int j = 0; j < this._points.Length - 1; j++)
					{
						if (this._points[j + 1].Y > this._points[j].Y)
						{
							if (num11 <= 0)
							{
								num11 = 1;
								if (this._points[j].Y <= num13)
								{
									this.addUpper(this._points[j].X, this._points[j].Y);
									this.addLower(this._points[j].X, num12 - this._points[j].Y);
									if (Math.Abs(this._points[j].Y - num13) > num14)
									{
										num14 = Math.Abs(this._points[j].Y - num13);
									}
								}
							}
						}
						else if (this._points[j + 1].Y < this._points[j].Y && num11 >= 0)
						{
							num11 = -1;
							if (this._points[j].Y >= num13)
							{
								this.addLower(this._points[j].X, this._points[j].Y);
								this.addUpper(this._points[j].X, num12 - this._points[j].Y);
								if (Math.Abs(this._points[j].Y - num13) > num14)
								{
									num14 = Math.Abs(this._points[j].Y - num13);
								}
							}
						}
					}
					num14 = (int)((float)num14 * 1.1f);
					if (this._uppIdx > 1 && this._lowIdx > 1)
					{
						this._upper[0].X = this._upper[1].X;
						this._upper[0].Y = num13;
						this._lower[0].X = this._lower[1].X;
						this._lower[0].Y = num13;
						this.addUpper(this._upper[this._uppIdx - 1].X, num13);
						this.addLower(this._lower[this._lowIdx - 1].X, num13);
						array = new Point[this._uppIdx];
						array2 = new Point[this._lowIdx];
						Array.Copy(this._upper, array, this._uppIdx);
						Array.Copy(this._lower, array2, this._lowIdx);
						if (Scope._spectrumFill == 1)
						{
							this._graphics.FillPolygon(this._gradientBrush, array);
							this._graphics.FillPolygon(this._gradientBrush, array2);
							goto IL_0700;
						}
						if (Scope._gradientColorBlend != null)
						{
							num14 = Math.Min(num14 * 2, base.ClientRectangle.Height);
							for (int k = 2; k < this._uppIdx - 1; k++)
							{
								int x = this._upper[k - 1].X;
								int y = this._upper[k - 1].Y;
								int x2 = this._upper[k].X;
								int y2 = this._upper[k].Y;
								for (int l = x; l < x2; l++)
								{
									int num15 = (int)((float)y + (float)(l - x) / (float)(x2 - x) * (float)(y2 - y));
									int num16 = 2 * Math.Abs(num15 - num13);
									int val = (int)((float)num16 / (float)num14 * 255f);
									Color color = Color.FromArgb(Scope._gradientPixels[Math.Min(val, 255)]);
									if (num16 > num14 / 40)
									{
										this._graphics.DrawLine(new Pen(color), l, num13 + num16 / 2, l, num13 - num16 / 2);
									}
								}
							}
							goto IL_0700;
						}
						return;
					}
				}
				goto IL_086e;
			}
			return;
			IL_0700:
			array[0].Y = array[1].Y;
			array[this._uppIdx - 1] = array[this._uppIdx - 2];
			array2[0].Y = array2[1].Y;
			array2[this._lowIdx - 1] = array2[this._lowIdx - 2];
			this._graphics.DrawLines(new Pen(this.TraceColor), array);
			this._graphics.DrawLines(new Pen(this.TraceColor), array2);
			for (int m = 2; m < this._uppIdx - 1; m++)
			{
				if (num13 - this._upper[m].Y < num14 / 40)
				{
					this._graphics.FillEllipse(Brushes.Red, this._upper[m].X - 3, num13 - 3, 6, 6);
				}
			}
			for (int n = 2; n < this._lowIdx - 2; n++)
			{
				if (this._lower[n].Y - num13 < num14 / 40)
				{
					this._graphics.FillEllipse(Brushes.Red, this._lower[n].X - 3, num13 - 3, 6, 6);
				}
			}
			goto IL_086e;
			IL_086e:
			if (!flag)
			{
				this._graphics.DrawLines(Scope._tracePen, this._points);
			}
			if (this._vChannel != DemodType.AM && this._vChannel != DemodType.Envelope)
			{
				return;
			}
			this.getAudioLevels();
			if (this._showLines)
			{
				int num5 = (int)(5f + (float)num * this.Hshift);
				int num6 = (int)((float)(this._scopeHeight - 5 - num2 / 2) + (this._vInvert ? this._average : (0f - this._average)) * num4 - (float)num2 * this._vShift);
				if (this._vChannel != DemodType.Envelope)
				{
					this._graphics.DrawLine(Scope._carrierPen, num5, num6, num5 + num, num6);
				}
				num6 = (int)((float)(this._scopeHeight - 5 - num2 / 2) + (this._vInvert ? this._pos : (0f - this._pos)) * num4 - (float)num2 * this._vShift);
				this._graphics.DrawLine(Scope._audioPen, num5, num6, num5 + num, num6);
				num6 = (int)((float)(this._scopeHeight - 5 - num2 / 2) + (this._vInvert ? this._neg : (0f - this._neg)) * num4 - (float)num2 * this._vShift);
				this._graphics.DrawLine(Scope._audioPen, num5, num6, num5 + num, num6);
				if (this._showBars && !this.VblockDC)
				{
					this.drawBars();
				}
			}
		}

		private void addLower(int x, int y)
		{
			this._lower[this._lowIdx].X = x;
			this._lower[this._lowIdx++].Y = y;
		}

		private void addUpper(int x, int y)
		{
			this._upper[this._uppIdx].X = x;
			this._upper[this._uppIdx++].Y = y;
		}

		private void getAudioLevels()
		{
			if (this._vChannel == DemodType.AM)
			{
				int num = this._carrierAvg - 1;
				for (int i = 0; i < this._vBuf.Length; i++)
				{
					this._average = ((float)num * this._average + this._vBuf[i]) / (float)this._carrierAvg;
					this._avgBuf[i] = this._average;
					if (this._vBuf[i] > this._pos)
					{
						if (this.VblockDC || (double)this._vBuf[i] < (double)this._average + 1.5 * (double)(this._average - this._neg))
						{
							this._pos = this._vBuf[i];
						}
					}
					else if (!this.VblockDC && this._pos > 4f * this._average)
					{
						this._pos = 3.9f * this._average;
					}
					else if (this._pos > this._average)
					{
						this._pos -= this._audioRel * this._vDiv;
					}
					if (this._vBuf[i] < this._neg)
					{
						if (this.VblockDC || this._vBuf[i] > 0f)
						{
							this._neg = this._vBuf[i];
						}
					}
					else if (this._neg < this._average)
					{
						this._neg += this._audioRel * this._vDiv;
					}
				}
			}
			else if (this._vChannel == DemodType.Envelope)
			{
				float num2 = this._audioRel * 1000f / (float)this._vBuf.Length;
				for (int j = 2; j < this._vBuf.Length - 2; j++)
				{
					if (this._vBuf[j] > this._pos)
					{
						this._pos = this._vBuf[j];
					}
					else
					{
						this._pos -= num2 / 30f * this._vDiv;
					}
					if (this._vBuf[j] < this._neg)
					{
						this._neg = this._vBuf[j];
					}
					else
					{
						this._neg += num2 / 30f * this._vDiv;
					}
				}
			}
		}

		private void drawBars()
		{
			if (this._vChannel == DemodType.AM && this._average != 0f)
			{
				int num = this._audioAvg - 1;
				this._graphics.SetClip(new Rectangle(0, this._scopeHeight, base.ClientRectangle.Width, this._barsHeight));
				float num2 = (float)(base.ClientRectangle.Width - 10) / 15f;
				int num3 = 0;
				int num4 = this._yPos - 5 - 10;
				int num5 = this._yNeg - 5 - 10;
				float num6 = Math.Min(1.45f, (this._pos - this._average) / this._average);
				if ((double)num6 < 1.0)
				{
					this._graphics.FillRectangle(Scope._audioBrush, 5f, (float)num4, num6 * num2 * 10f, 10f);
				}
				else
				{
					this._graphics.FillRectangle(Scope._overmodBrush, 5f, (float)num4, num6 * num2 * 10f, 10f);
				}
				this._posAver = Math.Max(0f, ((float)num * this._posAver + num6) / (float)this._audioAvg);
				if (num6 > this._posPeak)
				{
					this._posPeak = num6;
					this._posTmo = this._peakDelay;
				}
				else if (this._posTmo > 0)
				{
					this._posTmo--;
				}
				else if (this._posPeak > this._posAver)
				{
					this._posPeak -= this._peakRel;
				}
				num3 = (int)(5f + this._posAver * num2 * 10f);
				this._graphics.DrawLine(Scope._AverPen, num3, num4, num3, num4 + 10);
				num3 = (int)(5f + this._posPeak * num2 * 10f);
				this._graphics.DrawLine(Scope._PeakPen, num3, num4, num3, num4 + 10);
				float num7 = Math.Min(1.01f, (this._average - this._neg) / this._average);
				if ((double)num7 < 0.97)
				{
					this._graphics.FillRectangle(Scope._audioBrush, 5f, (float)num5, num7 * num2 * 10f, 10f);
				}
				else
				{
					this._graphics.FillRectangle(Scope._clippingBrush, 5f, (float)num5, num7 * num2 * 10f, 10f);
				}
				this._negAver = Math.Max(0f, ((float)num * this._negAver + num7) / (float)this._audioAvg);
				if (num7 > this._negPeak)
				{
					this._negPeak = num7;
					this._negTmo = this._peakDelay;
				}
				else if (this._negTmo > 0)
				{
					this._negTmo--;
				}
				else if (this._negPeak > this._negAver)
				{
					this._negPeak -= this._peakRel;
				}
				num3 = (int)(5f + this._negAver * num2 * 10f);
				this._graphics.DrawLine(Scope._AverPen, num3, num5, num3, num5 + 10);
				num3 = (int)(5f + this._negPeak * num2 * 10f);
				this._graphics.DrawLine(Scope._PeakPen, num3, num5, num3, num5 + 10);
			}
		}

		private void DrawStatusText()
		{
			if (!string.IsNullOrEmpty(this._statusText))
			{
				this._graphics.DrawString(this._statusText, Scope._statusFont, Brushes.Silver, 7f, 7f);
			}
		}

		private static void ConfigureGraphics(Graphics graphics)
		{
			graphics.CompositingMode = CompositingMode.SourceOver;
			graphics.CompositingQuality = CompositingQuality.HighSpeed;
			graphics.SmoothingMode = SmoothingMode.None;
			graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			graphics.InterpolationMode = InterpolationMode.High;
		}

		private void DrawForeground(bool hideTrace)
		{
			if (base.ClientRectangle.Width > 5 && base.ClientRectangle.Height > 5)
			{
				this.CopyBackground();
				this.DrawTrace();
			}
		}

		private unsafe void CopyBackground()
		{
			BitmapData bitmapData = this._buffer.LockBits(base.ClientRectangle, ImageLockMode.WriteOnly, this._buffer.PixelFormat);
			BitmapData bitmapData2 = this._bkgBuffer.LockBits(base.ClientRectangle, ImageLockMode.ReadOnly, this._bkgBuffer.PixelFormat);
			Utils.Memcpy((void*)bitmapData.Scan0, (void*)bitmapData2.Scan0, Math.Abs(bitmapData.Stride) * bitmapData.Height);
			this._buffer.UnlockBits(bitmapData);
			this._bkgBuffer.UnlockBits(bitmapData2);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Scope.ConfigureGraphics(e.Graphics);
			e.Graphics.DrawImageUnscaled(this._buffer, 0, 0);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (base.ClientRectangle.Width > 0 && base.ClientRectangle.Height > 0)
			{
				this._buffer.Dispose();
				this._graphics.Dispose();
				this._bkgBuffer.Dispose();
				this._bkgBuffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
				this._buffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
				this._graphics = Graphics.FromImage(this._buffer);
				Scope.ConfigureGraphics(this._graphics);
				if (!this._showBars)
				{
					this._barsHeight = 0;
				}
				else
				{
					this._barsHeight = 62;
				}
				if (!this._showBars)
				{
					this._scopeHeight = base.ClientRectangle.Height;
				}
				else
				{
					this._scopeHeight = base.ClientRectangle.Height - this._barsHeight - 5 - 3;
				}
				int num = base.ClientRectangle.Width - 10;
				this._hBuf = new float[num];
				this._points = new Point[num + 2];
				this._upper = new Point[num + 2];
				this._lower = new Point[num + 2];
				this._vBuf = new float[num];
				this._avgBuf = new float[num];
				this._negBuf = new float[num];
				this._posBuf = new float[num];
				if (this._gradientBrush != null)
				{
					this._gradientBrush.Dispose();
				}
				this.newGradientBrush(null);
				this._clipRectangle = new Rectangle(0, 0, base.ClientRectangle.Width, this._scopeHeight);
				this.newBackgroundBrush(null);
				this._drawBackgroundNeeded = true;
				this.Perform(false);
				if (Scope._gradientPixels != null && Scope._gradientPixels.Length >= base.ClientRectangle.Height)
				{
					return;
				}
				Scope._gradientPixels = null;
				Scope._gradientPixels = new int[base.ClientRectangle.Height];
			}
		}

		private void newGradientBrush(ColorBlend blend = null)
		{
			int y = (int)((float)(this._scopeHeight / 2) - this._vShift * (float)this._scopeHeight);
			this._gradientBrush = new LinearGradientBrush(new Rectangle(0, y, base.ClientRectangle.Width, this._scopeHeight / 2), Color.White, Color.Black, LinearGradientMode.Vertical);
			if (blend != null)
			{
				if (Scope._gradientColorBlend == null)
				{
					Scope._gradientColorBlend = new ColorBlend(2);
				}
				Scope._gradientColorBlend.Positions[0] = 0f;
				Scope._gradientColorBlend.Positions[1] = 1f;
				int num = blend.Colors.Length - 1;
				if (Scope._spectrumFill == 1)
				{
					Scope._gradientColorBlend.Colors[0] = Color.FromArgb(180, blend.Colors[num]);
					Scope._gradientColorBlend.Colors[1] = Color.FromArgb(180, blend.Colors[0]);
				}
				else if (Scope._spectrumFill == 2)
				{
					Scope._gradientColorBlend.Colors[0] = blend.Colors[0];
					Math.Max(blend.Colors[num].R, Math.Max(blend.Colors[num].G, blend.Colors[num].B));
					Scope._gradientColorBlend.Colors[1] = blend.Colors[num];
				}
				else
				{
					Scope._gradientColorBlend.Colors[0] = Scope._traceColor;
					Scope._gradientColorBlend.Colors[1] = Scope._backgroundColor;
				}
			}
			else if (Scope._gradientColorBlend == null)
			{
				return;
			}
			this._gradientBrush.InterpolationColors = Scope._gradientColorBlend;
			this._gradientBrush.WrapMode = WrapMode.TileFlipX;
			if (Scope._spectrumFill > 1)
			{
				this.fillGradientVector(Scope._gradientColorBlend, 255);
			}
		}

		private void newBackgroundBrush(Color? color = default(Color?))
		{
			if (color.HasValue)
			{
				Scope._backgroundColor = color.Value;
			}
			if (this._backgroundBrush != null)
			{
				this._backgroundBrush.Dispose();
			}
			this._backgroundBrush = Utils.BackgroundBrush(base.Name, Scope._backgroundColor, base.ClientRectangle.Width, this._scopeHeight, false, false);
			this._drawBackgroundNeeded = true;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			this.BackColor = SystemColors.Control;
			base.Name = "Scope";
			base.ResumeLayout(false);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.Cursor = Cursors.Hand;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.Cursor = Cursors.Default;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._oldHshift = this._hShift;
				this._oldVshift = this._vShift;
				this._oldX = e.X;
				this._oldY = e.Y;
				if (this.Cursor == Cursors.SizeNS)
				{
					this._trigChange = true;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (this._trigChange)
			{
				this._trigChange = false;
				this.Cursor = Cursors.Hand;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			float num = -1f;
			if (this._trigChange || (e.X < 10 && !this._xyMode))
			{
				num = -1f * (float)(e.Y - 5) / (float)(this._scopeHeight - 10) + 0.5f;
			}
			this.Cursor = (((double)Math.Abs(num - this._trigLevel) < 0.1) ? Cursors.SizeNS : Cursors.Hand);
			if (e.Button == MouseButtons.Left)
			{
				if (this._trigChange)
				{
					PositionEventArgs positionEventArgs = new PositionEventArgs(0f, -1f * (float)(e.Y - 5) / (float)(this._scopeHeight - 10) + 0.5f, true);
					this._trigLevel = positionEventArgs.Ypos;
					this._drawBackgroundNeeded = true;
					if (this.XYPositionChanged != null)
					{
						this.XYPositionChanged(this, positionEventArgs);
					}
				}
				else
				{
					this._vShift = this._oldVshift + (float)(this._oldY - e.Y) / (float)(this._scopeHeight - 10);
					this._hShift = this._oldHshift - (float)(this._oldX - e.X) / (float)(base.ClientRectangle.Width - 10);
					if ((double)this._vShift > 1.0)
					{
						this._vShift = 1f;
					}
					if ((double)this._vShift < -1.0)
					{
						this._vShift = -1f;
					}
					if ((double)this._hShift > 1.0)
					{
						this._hShift = 1f;
					}
					if ((double)this._hShift < -1.0)
					{
						this._hShift = -1f;
					}
					this.newGradientBrush(null);
					this._drawBackgroundNeeded = true;
				}
			}
		}

		private float pow(int factor)
		{
			return (float)Math.Pow(10.0, (double)factor / 10.0);
		}

		private int log(float value)
		{
			return (int)(Math.Log10((double)value) * 10.0);
		}
	}
}
