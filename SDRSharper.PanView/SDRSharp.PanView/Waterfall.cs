using SDRSharp.Radio;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	public class Waterfall : UserControl
	{
		private const float TrackingFontSize = 14f;

		private const float TimestampFontSize = 12f;

		private const int MinLogFrequency = 40;

		private const int ScaleMargin = 30;

		private const int GradientMargin = 30;

		public const float MaxZoom = 4f;

		public const int CursorSnapDistance = 4;

		public const int RightClickSnapDistance = 500;

		private Counter _drawTmr = new Counter();

		private int _lrMargin = 30;

		private int _tbMargin = 5;

		private bool _ver = true;

		private double _attack;

		private double _decay;

		private Bitmap _buffer;

		private Bitmap _buffer2;

		private Rectangle _bufferRect;

		private Graphics _graphics;

		private Graphics _graphics2;

		private BandType _bandType;

		private int _filterBandwidth;

		private float _x1Increment;

		private float _yIncrement;

		private byte[] _temp;

		private byte[] _powerSpectrum;

		private byte[] _scaledPowerSpectrum;

		private long _centerFrequency;

		private long _spectrumWidth;

		private int _stepSize;

		private int _centerStep;

		private long _frequency;

		private float _lower;

		private float _upper;

		private float _scale = 1f;

		private long _displayCenterFrequency;

		private int _changingBandwidth;

		private int _changingScale;

		private float _oldPower;

		private bool _changingFrequency;

		private bool _changingZoomFrequency;

		private bool _changingCenterFrequency;

		private bool _mouseIn;

		private int _oldX1;

		private int _oldY;

		private long _oldFrequency;

		private long _oldCenterFrequency;

		private long _oldDisplayFrequency;

		private int _oldFilterBandwidth;

		private int _filterOffset;

		private int _oldFilterOffset;

		private bool _indepSideband;

		private bool _centerFixed;

		private int[] _gradientPixels;

		private Hashtable _gradientHash;

		private bool _useSmoothing;

		private bool _useSnap;

		private bool _centerSnap;

		private int _trackingY;

		private int _trackingX;

		private long _trackingFrequency;

		private int _bufLines;

		private int _scanLines;

		private int _useTimestamps;

		private int _timestampInterval;

		private int _scanLineMsec = 999;

		private DateTime _recordStart;

		private DateTime _waveStart;

		private DateTime _oldStart;

		private float _trackingPower;

		private int _timeOut;

		private int _signalTmo;

		private int _paintTmo;

		private Color _backgroundColor;

		private LinearGradientBrush _gradientBrush;

		private PathGradientBrush _backgroundBrush;

		private ColorBlend _gradientColorBlend;

		private float _minPower;

		private float _maxPower;

		private DataType _dataType;

		private int _showDbm;

		private DetectorType _detectorType;

		private ContextMenuStrip _contextMenu;

		private Brush _transparentBrush = new SolidBrush(Color.FromArgb(100, Color.DarkGray));

		private Brush _timeBackBrush = new SolidBrush(Color.FromArgb(170, 10, 10, 10));

		private static Brush _fontBrush = new SolidBrush(Color.Silver);

		private static Pen _hotTrackPen = new Pen(Color.Yellow);

		private static Pen _carrierPen = new Pen(Color.Red, 1f);

		private static Pen _outlinePen = new Pen(Color.Black, 4f);

		private static Pen _axisPen = new Pen(Color.FromArgb(120, 120, 120));

		private static FontFamily _fontFamily = new FontFamily("Arial");

		private static Font _font = new Font("Arial", 8f);

		private LinLog _linlog = new LinLog();

		private bool _showLog;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ColorBlend GradientColorBlend
		{
			get
			{
				return this._gradientColorBlend;
			}
			set
			{
				this.NewGradientBrush(value);
			}
		}

		public long Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				if (this._frequency != value)
				{
					if (this._dataType == DataType.RF && this._centerFixed && (double)this._scale > 1.001)
					{
						long num = (long)((float)this._displayCenterFrequency + (float)this._spectrumWidth / this._scale / 2f);
						long num2 = (long)((float)this._displayCenterFrequency - (float)this._spectrumWidth / this._scale / 2f);
						long f = this._displayCenterFrequency + value - this._frequency;
						if (value < num2 || value > num)
						{
							this.UpdateDisplayFrequency(f);
						}
					}
					this._frequency = value;
					if (this._dataType != 0)
					{
						this.CenterFrequency = this.getBasebandCenter();
					}
				}
			}
		}

		public long DisplayFrequency
		{
			get
			{
				return this._displayCenterFrequency;
			}
			set
			{
				if (this._displayCenterFrequency != value)
				{
					this._displayCenterFrequency = value;
				}
			}
		}

		public long CenterFrequency
		{
			get
			{
				return this._centerFrequency;
			}
			set
			{
				if (this._centerFrequency != value)
				{
					long num = value - this._centerFrequency;
					this._displayCenterFrequency += num;
					this._centerFrequency = value;
					this.DrawFrequencyScale();
				}
			}
		}

		public int SpectrumWidth
		{
			get
			{
				return (int)this._spectrumWidth;
			}
			set
			{
				if (this._spectrumWidth != value)
				{
					this._spectrumWidth = value;
					this.ApplyZoom(-1f);
				}
			}
		}

		public int FilterBandwidth
		{
			get
			{
				return this._filterBandwidth;
			}
			set
			{
				if (this._filterBandwidth != value)
				{
					this._filterBandwidth = value;
					if (this._dataType != 0)
					{
						this.CenterFrequency = this.getBasebandCenter();
					}
				}
			}
		}

		public int FilterOffset
		{
			get
			{
				return this._filterOffset;
			}
			set
			{
				if (this._filterOffset != value)
				{
					this._filterOffset = value;
					if (this._dataType != 0)
					{
						this.CenterFrequency = this.getBasebandCenter();
					}
				}
			}
		}

		public BandType BandType
		{
			get
			{
				return this._bandType;
			}
			set
			{
				if (this._bandType != value)
				{
					this._bandType = value;
					if (this._dataType != 0)
					{
						this.CenterFrequency = this.getBasebandCenter();
					}
				}
			}
		}

		public float Zoom
		{
			get
			{
				return this._scale;
			}
			set
			{
				if (value <= 0f)
				{
					int num = Math.Max(this._filterBandwidth, 340);
					if (this._dataType == DataType.AF && this._bandType == BandType.Center)
					{
						num /= 2;
					}
					if (this._dataType == DataType.AF && this._detectorType == DetectorType.CW)
					{
						num = 800;
					}
					value = Math.Max(1f, (float)this._spectrumWidth / ((float)num * 1.5f));
				}
				if (this._scale != value)
				{
					this._scale = value;
					this.ApplyZoom(-1f);
				}
			}
		}

		public DateTime RecordStart
		{
			get
			{
				return this._recordStart;
			}
			set
			{
				this._recordStart = value;
			}
		}

		public DateTime WaveStart
		{
			get
			{
				return this._waveStart;
			}
			set
			{
				this._waveStart = value;
				if (this.UseTimestamps == 0)
				{
					this._oldStart = value;
				}
				this._scanLines = (this._ver ? 133 : 175) - 300 / this._scanLineMsec;
			}
		}

		public int TimestampInterval
		{
			get
			{
				return this._timestampInterval;
			}
			set
			{
				this._timestampInterval = value;
			}
		}

		public int UseTimestamps
		{
			get
			{
				return this._useTimestamps;
			}
			set
			{
				this._useTimestamps = value;
			}
		}

		public int ScanLineMsec
		{
			set
			{
				this._scanLineMsec = value;
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this.newBackgroundBrush(value);
			}
		}

		public bool Horizontal
		{
			get
			{
				return !this._ver;
			}
			set
			{
				this._ver = !value;
				if (this._ver)
				{
					this._tbMargin = 0;
				}
				this.OnResize(null);
			}
		}

		public float MaxPower
		{
			get
			{
				return this._maxPower;
			}
			set
			{
				this._maxPower = value;
				this.DrawGradient();
			}
		}

		public float MinPower
		{
			get
			{
				return this._minPower;
			}
			set
			{
				this._minPower = value;
				this.DrawGradient();
			}
		}

		public int ShowDbm
		{
			get
			{
				return this._showDbm;
			}
			set
			{
				this._showDbm = value;
				this.DrawGradient();
			}
		}

		public DetectorType DetectorType
		{
			set
			{
				this._detectorType = value;
			}
		}

		public bool IndepSideband
		{
			get
			{
				return this._indepSideband;
			}
			set
			{
				this._indepSideband = value;
			}
		}

		public bool CenterFixed
		{
			get
			{
				return this._centerFixed;
			}
			set
			{
				this._centerFixed = value;
			}
		}

		public ContextMenuStrip ContextMnu
		{
			set
			{
				this._contextMenu = value;
			}
		}

		public bool UseSmoothing
		{
			get
			{
				return this._useSmoothing;
			}
			set
			{
				this._useSmoothing = value;
			}
		}

		public double Decay
		{
			get
			{
				return this._decay;
			}
			set
			{
				this._decay = value;
			}
		}

		public double Attack
		{
			get
			{
				return this._attack;
			}
			set
			{
				this._attack = value;
			}
		}

		public int StepSize
		{
			get
			{
				return this._stepSize;
			}
			set
			{
				this._stepSize = value;
			}
		}

		public int CenterStep
		{
			get
			{
				return this._centerStep;
			}
			set
			{
				this._centerStep = value;
			}
		}

		public bool UseSnap
		{
			get
			{
				return this._useSnap;
			}
			set
			{
				this._useSnap = (this._dataType != DataType.IF && value);
			}
		}

		public bool CenterSnap
		{
			set
			{
				this._centerSnap = value;
			}
		}

		public bool ShowLog
		{
			set
			{
				this._showLog = value;
				this.DrawFrequencyScale();
			}
		}

		public DataType DataType
		{
			get
			{
				return this._dataType;
			}
			set
			{
				this._dataType = value;
				if (this._dataType == DataType.RF)
				{
					this._lrMargin = 0;
				}
				else
				{
					this.CenterFrequency = this.getBasebandCenter();
				}
			}
		}

		public event ManualFrequencyChange FrequencyChanged;

		public event ManualFrequencyChange CenterFrequencyChanged;

		public event ManualFrequencyChange DisplayFrequencyChanged;

		public event ManualBandwidthChange BandwidthChanged;

		public event EventHandler AutoZoomed;

		public Waterfall()
		{
			if (this._ver)
			{
				this._powerSpectrum = new byte[base.ClientRectangle.Width - 2 * this._tbMargin];
			}
			else
			{
				this._powerSpectrum = new byte[base.ClientRectangle.Height - 2 * this._tbMargin];
			}
			this._temp = new byte[this._powerSpectrum.Length];
			this._buffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
			this._buffer2 = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
			this._graphics = Graphics.FromImage(this._buffer);
			SpectrumAnalyzer.ConfigureGraphics(this._graphics);
			this._graphics2 = Graphics.FromImage(this._buffer2);
			SpectrumAnalyzer.ConfigureGraphics(this._graphics2);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.UpdateStyles();
		}

		~Waterfall()
		{
			this._buffer.Dispose();
			this._buffer2.Dispose();
			this._graphics.Dispose();
			this._graphics2.Dispose();
			this._gradientBrush.Dispose();
		}

		private void ApplyZoom(float scale = -1f)
		{
			if (this._spectrumWidth > 0)
			{
				if (this._dataType != DataType.IF)
				{
					this._displayCenterFrequency = this.GetDisplayCenterFrequency();
				}
				if (this._ver)
				{
					this._x1Increment = this._scale * (float)(base.ClientRectangle.Width - 2 * this._lrMargin) / (float)this._spectrumWidth;
				}
				else
				{
					this._yIncrement = this._scale * (float)(base.ClientRectangle.Height - 2 * this._tbMargin) / (float)this._spectrumWidth;
				}
			}
		}

		private long GetDisplayCenterFrequency()
		{
			long num = this._frequency;
			switch (this._bandType)
			{
			case BandType.Lower:
				num -= this._filterBandwidth / 2 + this._filterOffset;
				break;
			case BandType.Upper:
				num += this._filterBandwidth / 2 + this._filterOffset;
				break;
			}
			int num2 = (this._dataType != DataType.AF) ? 10 : 0;
			long num3 = (long)((float)(this._centerFrequency - this._spectrumWidth / 2) - ((float)num - (float)this._spectrumWidth / this._scale / 2f));
			if (num3 > 0)
			{
				num += num3 + num2;
			}
			long num4 = (long)((float)num + (float)this._spectrumWidth / this._scale / 2f - (float)(this._centerFrequency + this._spectrumWidth / 2));
			if (num4 > 0)
			{
				num -= num4 + num2;
			}
			return num;
		}

		public unsafe void RenderAndDraw(float* powerSpectrum, int length, int timeOut, int interval)
		{
			this._timeOut = Math.Min(this._timeOut, timeOut);
			if (--this._timeOut <= 0)
			{
				this._timeOut = timeOut;
				if (this._scaledPowerSpectrum == null || this._scaledPowerSpectrum.Length != length)
				{
					this._scaledPowerSpectrum = new byte[length];
				}
				byte[] scaledPowerSpectrum = this._scaledPowerSpectrum;
				fixed (byte* dest = scaledPowerSpectrum)
				{
					Fourier.ScaleFFT(powerSpectrum, dest, length, this.MinPower, this.MaxPower);
				}
				int num = (int)((float)length / this._scale);
				int offset = (int)((double)(length - num) / 2.0 + (double)length * (double)(this._displayCenterFrequency - this._centerFrequency) / (double)this._spectrumWidth);
				if (this._showLog)
				{
					this._linlog.MakeLog(this._scaledPowerSpectrum, length, 40L, this._spectrumWidth);
				}
				if (this._useSmoothing)
				{
					Fourier.SmoothCopy(this._scaledPowerSpectrum, this._temp, length, this._scale, offset);
					for (int i = 0; i < this._powerSpectrum.Length; i++)
					{
						double num2 = (this._powerSpectrum[i] < this._temp[i]) ? this.Attack : this.Decay;
						this._powerSpectrum[i] = (byte)Math.Round((double)(int)this._powerSpectrum[i] * (1.0 - num2) + (double)(int)this._temp[i] * num2);
					}
				}
				else
				{
					Fourier.SmoothCopy(this._scaledPowerSpectrum, this._powerSpectrum, length, this._scale, offset);
				}
				this.Draw();
			}
			if (--this._paintTmo <= 0)
			{
				this._paintTmo = 120 / interval;
				if (!Utils.FastFFT)
				{
					this._paintTmo = 2;
				}
				if (this._paintTmo < 2)
				{
					this._paintTmo = 2;
				}
				if (this._mouseIn)
				{
					this.CopyMainBuffer();
					this.DrawCursor();
					if (this._dataType == DataType.RF)
					{
						this.DrawGradient();
					}
				}
				base.Invalidate();
			}
		}

		public void Perform()
		{
			bool visible = base.Visible;
		}

		private void Draw()
		{
			if (base.ClientRectangle.Width > this._lrMargin && base.ClientRectangle.Height > this._tbMargin)
			{
				this.InsertSpectrum();
				int num = this._ver ? this._timestampInterval : (this._timestampInterval * 2);
				if (this._useTimestamps > 0 && ++this._scanLines >= num)
				{
					this._scanLines = 0;
					this.DrawTimestamp();
				}
			}
		}

		private unsafe void InsertSpectrum()
		{
			if (this._gradientPixels != null && this._powerSpectrum != null && this._powerSpectrum.Length != 0)
			{
				BitmapData bitmapData = this._buffer.LockBits(base.ClientRectangle, ImageLockMode.ReadWrite, this._buffer.PixelFormat);
				if (bitmapData.PixelFormat == PixelFormat.Format32bppArgb)
				{
					throw new ApplicationException("Wrong pixelformat.");
				}
				if (this._ver)
				{
					int len = (bitmapData.Width - this._lrMargin - 1) * 4;
					for (int num = bitmapData.Height - 1; num > 0; num--)
					{
						int* src = (int*)((int)bitmapData.Scan0 + (num - 1) * bitmapData.Stride);
						int* dest = (int*)((int)bitmapData.Scan0 + num * bitmapData.Stride);
						Utils.Memcpy(dest, src, len);
					}
				}
				else
				{
					int num2 = this._lrMargin * 4;
					int num3 = (bitmapData.Width - 1) * 4 - num2;
					for (int i = 0; i < bitmapData.Height; i++)
					{
						int* src = (int*)((int)bitmapData.Scan0 + i * bitmapData.Stride + num2);
						if (num3 > 0)
						{
							Utils.Memcpy(src, src + 1, num3);
						}
					}
				}
				if (this._ver)
				{
					int* ptr = (int*)((byte*)(void*)bitmapData.Scan0 + (long)this._lrMargin * 4L);
					for (int j = 0; j < this._powerSpectrum.Length; j++)
					{
						int val = this._powerSpectrum[j] * this._gradientPixels.Length / 255;
						val = Math.Max(val, 0);
						val = Math.Min(val, this._gradientPixels.Length - 1);
						int* intPtr = ptr;
						ptr = intPtr + 1;
						*intPtr = this._gradientPixels[val];
					}
					this._bufLines = Math.Min(++this._bufLines, base.ClientRectangle.Height - 2 * this._tbMargin);
				}
				else
				{
					int num4 = (bitmapData.Width - 1) * 4;
					for (int k = 0; k < base.ClientRectangle.Height - 2 * this._tbMargin; k++)
					{
						int val2 = this._powerSpectrum[k] * this._gradientPixels.Length / 255;
						val2 = Math.Max(val2, 0);
						val2 = Math.Min(val2, this._gradientPixels.Length - 1);
						int* ptr = (int*)((int)bitmapData.Scan0 + (bitmapData.Height - this._tbMargin - k - 1) * bitmapData.Stride + num4);
						*ptr = this._gradientPixels[val2];
					}
					this._bufLines = Math.Min(++this._bufLines, base.ClientRectangle.Width - this._lrMargin);
				}
				this._buffer.UnlockBits(bitmapData);
			}
		}

		private void DrawTimestamp()
		{
			Brush brush = Brushes.White;
			DateTime dateTime;
			if (this._waveStart.CompareTo(DateTime.Now) > 0)
			{
				dateTime = ((this._useTimestamps == 1) ? DateTime.Now : DateTime.UtcNow);
			}
			else
			{
				dateTime = this._recordStart.Add(DateTime.Now.Subtract(this._waveStart));
				if (this._waveStart != this._oldStart)
				{
					brush = Brushes.Yellow;
					this._oldStart = this._waveStart;
					if (this._ver)
					{
						this._graphics.DrawLine(Pens.Cyan, this._lrMargin, 15, base.ClientRectangle.Width - this._lrMargin, 15);
					}
					else
					{
						this._graphics.DrawLine(Pens.Black, base.ClientRectangle.Width - 20, Math.Max(20, this._tbMargin), base.ClientRectangle.Width - 20, base.ClientRectangle.Height - this._tbMargin);
					}
				}
			}
			if (this._ver)
			{
				this._graphics.FillRectangle(this._timeBackBrush, 5, 1, 50, 28);
				this._graphics.DrawString(dateTime.ToString("dd:MMM:yy"), Waterfall._font, brush, 5f, 0f);
				this._graphics.DrawString(dateTime.ToString("HH:mm:ss"), Waterfall._font, brush, 5f, 15f);
			}
			else
			{
				this._graphics.FillRectangle(this._timeBackBrush, base.ClientRectangle.Width - 50, 6, 46, 13);
				this._graphics.DrawString(dateTime.ToString("HH:mm:ss"), Waterfall._font, brush, (float)(base.ClientRectangle.Width - 50), 5f);
			}
		}

		private void DrawCursor()
		{
			this._lower = 0f;
			float num = 0f;
			float num2 = 0f;
			float num3 = this._ver ? this._x1Increment : this._yIncrement;
			float num4 = Math.Max((float)(this._filterBandwidth + this._filterOffset) * num3, 2f);
			float num5 = (!this._ver) ? ((float)base.ClientRectangle.Height / 2f + (float)(this._frequency - this._displayCenterFrequency) * num3) : ((float)base.ClientRectangle.Width / 2f + (float)(this._frequency - this._displayCenterFrequency) * num3);
			switch (this._bandType)
			{
			case BandType.Upper:
				num2 = (float)this._filterOffset * num3;
				num = num4 - num2;
				this._lower = num5;
				break;
			case BandType.Lower:
				num2 = (float)this._filterOffset * num3;
				num = num4 - num2;
				this._lower = num5 - num;
				break;
			case BandType.Center:
			{
				num4 = Math.Max((float)this._filterBandwidth * num3, 2f);
				num2 = (float)this._filterOffset * num3;
				num = num4;
				float num6 = num5 + num2;
				this._lower = num6 - num4 / 2f;
				break;
			}
			}
			if (this.DataType == DataType.AF && this._bandType != BandType.Center)
			{
				this._lower = num5;
			}
			this._upper = this._lower + num;
			if (this._ver && (num4 < (float)base.ClientRectangle.Width || (this._dataType == DataType.AF && num4 < (float)(2 * base.ClientRectangle.Width))))
			{
				if (this._dataType == DataType.RF)
				{
					this._graphics2.FillRectangle(this._transparentBrush, (int)this._lower + 1, this._tbMargin, (int)num, base.ClientRectangle.Height - this._tbMargin);
				}
				else if (this._dataType == DataType.IF)
				{
					this._graphics2.FillRectangle(this._transparentBrush, this._lrMargin, 0, (int)this._lower - this._lrMargin + 1, base.ClientRectangle.Height);
					this._graphics2.FillRectangle(this._transparentBrush, (float)((int)this._upper + 1), 0f, (float)(base.ClientRectangle.Width - this._lrMargin) - this._upper - 1f, (float)base.ClientRectangle.Height);
				}
				else
				{
					this._graphics2.FillRectangle(this._transparentBrush, (float)((int)this._upper + 1), 0f, (float)(base.ClientRectangle.Width - this._lrMargin) - this._upper - 1f, (float)base.ClientRectangle.Height);
				}
				if (num5 >= (float)this._lrMargin && num5 <= (float)(base.ClientRectangle.Width - this._lrMargin) && this._dataType != DataType.AF)
				{
					this._graphics2.DrawLine(Waterfall._carrierPen, num5, 0f, num5, (float)base.ClientRectangle.Height);
				}
			}
			else if (!this._ver && (num4 < (float)base.ClientRectangle.Height || (this._dataType == DataType.AF && num4 < (float)(2 * base.ClientRectangle.Height))))
			{
				if (this._dataType == DataType.RF)
				{
					this._graphics2.FillRectangle(this._transparentBrush, 0f, (float)base.ClientRectangle.Height - this._upper - 1f, (float)base.ClientRectangle.Width, (float)(int)num);
				}
				else if (this._dataType == DataType.IF)
				{
					this._graphics2.FillRectangle(this._transparentBrush, 0f, (float)this._tbMargin, (float)base.ClientRectangle.Width, (float)(base.ClientRectangle.Height - this._tbMargin) - this._upper - 1f);
					this._graphics2.FillRectangle(this._transparentBrush, 0f, (float)base.ClientRectangle.Height - this._lower - 1f, (float)base.ClientRectangle.Width, this._lower - 1f - (float)this._tbMargin);
				}
				if (num5 >= (float)this._tbMargin && num5 <= (float)(base.ClientRectangle.Height - this._tbMargin) && this._dataType != DataType.AF)
				{
					this._graphics2.DrawLine(Waterfall._carrierPen, 0f, (float)base.ClientRectangle.Height - num5, (float)base.ClientRectangle.Width, (float)base.ClientRectangle.Height - num5);
				}
			}
			if (!this._ver || this._trackingX < this._lrMargin || this._trackingX > base.ClientRectangle.Width - this._lrMargin)
			{
				if (this._ver)
				{
					return;
				}
				if (this._trackingY < this._tbMargin)
				{
					return;
				}
				if (this._trackingY > base.ClientRectangle.Height - this._tbMargin)
				{
					return;
				}
			}
			if (!this._changingFrequency && !this._changingCenterFrequency && this._changingBandwidth == 0)
			{
				if (this._ver)
				{
					this._graphics2.DrawLine(Waterfall._hotTrackPen, this._trackingX, 0, this._trackingX, base.ClientRectangle.Height);
				}
				else
				{
					this._graphics2.DrawLine(Waterfall._hotTrackPen, 0, this._trackingY, base.ClientRectangle.Width, this._trackingY);
				}
			}
			string text = "";
			if (this._changingFrequency)
			{
				text = "VFO = " + SpectrumAnalyzer.GetFrequencyDisplay(this._frequency, true);
			}
			else if (this._changingBandwidth != 0)
			{
				text = "BW = " + SpectrumAnalyzer.GetFrequencyDisplay(this._filterBandwidth, true);
			}
			else if (this._changingCenterFrequency)
			{
				text = "Center = " + SpectrumAnalyzer.GetFrequencyDisplay(this._centerFrequency, true);
			}
			else if (this._changingScale != 0)
			{
				text = "Scale";
			}
			else
			{
				string arg = "";
				if (this._dataType != 0)
				{
					this._trackingPower = -128f;
				}
				else if (--this._signalTmo <= 0)
				{
					this._signalTmo = 15;
					if (this._trackingX < 0 || this._trackingY < 0 || this._trackingX >= base.ClientRectangle.Width || this._trackingY >= base.ClientRectangle.Height)
					{
						this._trackingPower = -128f;
					}
					else
					{
						int num7 = this._buffer.GetPixel(this._trackingX, this._trackingY).ToArgb();
						if (num7 == -16777216)
						{
							this._trackingPower = -128f;
						}
						else
						{
							int num8 = -1;
							if (this._gradientHash.Contains(num7))
							{
								num8 = (int)this._gradientHash[num7];
							}
							if (num8 >= 0)
							{
								this._trackingPower = this._minPower + (this._maxPower - this._minPower) * (float)num8 / (float)this._gradientPixels.Length;
							}
						}
					}
				}
				if (this._trackingPower > -128f)
				{
					arg = Utils.Signal((int)this._trackingPower, this._showDbm, true);
				}
				string text2 = (this._trackingPower >= this._maxPower - 1f) ? "{0}\r\n> {1}" : (text2 = "{0}\r\n{1}");
				long num9 = this._trackingFrequency;
				if (this._showLog)
				{
					double num10 = Math.Log10(40.0);
					double num11 = Math.Log10((double)this._spectrumWidth);
					double num12 = (double)num9 / (double)this._spectrumWidth;
					num9 = (long)Math.Pow(10.0, num10 + num12 * (num11 - num10));
				}
				text = string.Format(text2, SpectrumAnalyzer.GetFrequencyDisplay(num9, true), arg);
			}
			using (GraphicsPath path = new GraphicsPath())
			{
				Utils.AddString(path, text, 14f, Math.Min(base.ClientRectangle.Width - 80, this._trackingX + 10), Math.Max(0, this._trackingY - 35));
				Utils.DrawPath(this._graphics2, path, Brushes.White, 4);
			}
		}

		private unsafe void CopyMainBuffer()
		{
			BitmapData bitmapData = this._buffer.LockBits(this._bufferRect, ImageLockMode.ReadOnly, this._buffer.PixelFormat);
			BitmapData bitmapData2 = this._buffer2.LockBits(this._bufferRect, ImageLockMode.WriteOnly, this._buffer2.PixelFormat);
			Utils.Memcpy((void*)bitmapData2.Scan0, (void*)bitmapData.Scan0, Math.Abs(bitmapData.Stride) * bitmapData.Height);
			this._buffer.UnlockBits(bitmapData);
			this._buffer2.UnlockBits(bitmapData2);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImageUnscaled(this._mouseIn ? this._buffer2 : this._buffer, 0, 0);
		}

		protected override void OnResize(EventArgs e)
		{
			if (e != null)
			{
				base.OnResize(e);
			}
			if (base.ClientRectangle.Width > 2 * this._lrMargin && base.ClientRectangle.Height > 2 * this._tbMargin)
			{
				int num = this._ver ? (base.ClientRectangle.Width - 2 * this._lrMargin) : (base.ClientRectangle.Height - 2 * this._tbMargin);
				byte[] array = new byte[num];
				Fourier.SmoothCopy(this._powerSpectrum, array, this._powerSpectrum.Length, (float)(this._temp.Length + array.Length) / (float)this._temp.Length, 0);
				this._powerSpectrum = array;
				this._temp = new byte[num];
				Bitmap buffer = this._buffer;
				Bitmap buffer2 = this._buffer2;
				this._buffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
				this._buffer2 = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
				this._graphics.Dispose();
				this._graphics = Graphics.FromImage(this._buffer);
				SpectrumAnalyzer.ConfigureGraphics(this._graphics);
				this._bufferRect = new Rectangle(0, 0, this._buffer.Width, this._buffer.Height);
				this._graphics2.Dispose();
				this._graphics2 = Graphics.FromImage(this._buffer2);
				SpectrumAnalyzer.ConfigureGraphics(this._graphics2);
				this._graphics.Clear(Color.Black);
				this.newBackgroundBrush(null);
				if (this._backgroundBrush != null)
				{
					this._graphics.FillRectangle(this._backgroundBrush, base.ClientRectangle);
				}
				base.Invalidate();
				if (this._ver)
				{
					if (this._bufLines > 0)
					{
						this._bufLines = Math.Min(this._bufLines, this._buffer.Height - 2 * this._tbMargin);
						Rectangle destRect = new Rectangle(this._lrMargin, this._tbMargin, this._buffer.Width - 2 * this._lrMargin, this._bufLines);
						this._graphics.DrawImage(buffer, destRect, this._lrMargin, this._tbMargin, buffer.Width - 2 * this._lrMargin, this._bufLines, GraphicsUnit.Pixel);
					}
				}
				else if (this._bufLines > 0)
				{
					this._bufLines = Math.Min(this._bufLines, this._buffer.Width - this._lrMargin);
					Rectangle destRect2 = new Rectangle(this._buffer.Width - this._bufLines, this._tbMargin, this._bufLines, this._buffer.Height - 2 * this._tbMargin);
					this._graphics.DrawImage(buffer, destRect2, buffer.Width - this._bufLines, this._tbMargin, this._bufLines, buffer.Height - 2 * this._tbMargin, GraphicsUnit.Pixel);
				}
				buffer.Dispose();
				buffer2.Dispose();
				if (this._spectrumWidth > 0)
				{
					if (this._ver)
					{
						this._x1Increment = this._scale * (float)(base.ClientRectangle.Width - 2 * this._lrMargin) / (float)this._spectrumWidth;
					}
					else
					{
						this._yIncrement = this._scale * (float)(base.ClientRectangle.Height - 2 * this._tbMargin) / (float)this._spectrumWidth;
					}
				}
				this.DrawFrequencyScale();
				this.NewGradientBrush(null);
			}
		}

		private void newBackgroundBrush(Color? color = default(Color?))
		{
			if (base.Name.Length != 0 && (!color.HasValue || !(color == (Color?)this._backgroundColor)))
			{
				if (color.HasValue)
				{
					this._backgroundColor = color.Value;
				}
				if (this._backgroundBrush != null)
				{
					this._backgroundBrush.Dispose();
				}
				this._backgroundBrush = Utils.BackgroundBrush(base.Name, this._backgroundColor, base.ClientRectangle, false, false);
			}
		}

		private void NewGradientBrush(ColorBlend blend = null)
		{
			if (blend != null && blend == this._gradientColorBlend)
			{
				return;
			}
			if (blend != null)
			{
				this._gradientColorBlend = blend;
			}
			else if (this._gradientColorBlend == null)
			{
				return;
			}
			if (this._gradientBrush != null)
			{
				this._gradientBrush.Dispose();
			}
			this._gradientBrush = new LinearGradientBrush(new Rectangle(0, 15, 1, base.ClientRectangle.Height - 30), Color.White, Color.Black, LinearGradientMode.Vertical);
			this._gradientBrush.InterpolationColors = this._gradientColorBlend;
			this._gradientPixels = null;
			this._gradientHash = null;
			this.DrawGradient();
		}

		private void DrawGradient()
		{
			if (this._gradientBrush != null && this._backgroundBrush != null)
			{
				using (Pen pen = new Pen(this._gradientBrush, 6f))
				{
					if (this._lrMargin > 0)
					{
						this._graphics2.FillRectangle(this._backgroundBrush, base.ClientRectangle.Width - this._lrMargin + 1, 0, this._lrMargin, base.ClientRectangle.Height);
					}
					float num = (float)(base.ClientRectangle.Width - 30 + ((this._dataType == DataType.RF) ? 5 : 10));
					this._graphics2.DrawLine(pen, num, (float)(base.ClientRectangle.Height - 15), num, 15f);
				}
				this.BuildGradientVector();
				this.DrawGradientScale(this._graphics2);
			}
		}

		public void DrawGradientScale(Graphics graphics)
		{
			int num = (int)this._minPower;
			int num2 = (int)this._maxPower;
			int num3 = num2 - num;
			int num4 = 20;
			int num5 = 12;
			float num6 = (float)(base.ClientRectangle.Height - 30) / (float)num3;
			float num7 = (float)(base.ClientRectangle.Height - 30) / (graphics.MeasureString("0", Waterfall._font).Height * 4f);
			if ((float)num3 <= num7)
			{
				num4 = 1;
			}
			else if ((float)(num3 / 2) <= num7)
			{
				num4 = 2;
			}
			else if ((float)(num3 / 3) <= num7)
			{
				num4 = 3;
			}
			else if ((float)(num3 / 6) <= num7)
			{
				num4 = 5;
			}
			else if ((float)(num3 / 10) <= num7)
			{
				num4 = 10;
			}
			if (this._showDbm >= 2)
			{
				num4 = ((num4 > 7) ? 10 : 5);
			}
			for (int i = num; i < num2; i++)
			{
				bool flag = false;
				switch (this._showDbm)
				{
				case 0:
					if (i % num4 == 0)
					{
						flag = true;
					}
					break;
				case 1:
					if ((i + 7) % num4 == 0)
					{
						flag = true;
					}
					break;
				case 3:
					if ((i + 3) % num4 == 0)
					{
						flag = true;
					}
					break;
				case 2:
					if (i > -73)
					{
						num4 = ((num4 > 5) ? 10 : 5);
						if ((i + 73) % num4 == 0)
						{
							flag = true;
						}
					}
					else
					{
						num5 = ((num4 <= 5) ? 6 : 12);
						if ((i + 121) % num5 == 0)
						{
							flag = true;
						}
					}
					break;
				}
				if (flag)
				{
					string text = Utils.Signal(i, this._showDbm, false);
					SizeF sizeF = graphics.MeasureString(text, Waterfall._font);
					float num8 = 15f + (float)(num2 - i) * num6;
					graphics.DrawString(text, Waterfall._font, Waterfall._fontBrush, (float)base.ClientRectangle.Width - sizeF.Width, num8 - sizeF.Height);
				}
			}
		}

		public void DrawFrequencyScale()
		{
			if (!this._ver && this._spectrumWidth > 0)
			{
				if (this._backgroundBrush != null)
				{
					this._graphics.FillRectangle(this._backgroundBrush, 0, 0, this._lrMargin, base.ClientRectangle.Height);
				}
				if (this._showLog)
				{
					int[] array = new int[3]
					{
						1,
						2,
						5
					};
					int num = 40;
					int num2 = (int)this._spectrumWidth;
					double num3 = Math.Log10((double)num);
					double num4 = Math.Log10((double)num2);
					int num5 = (int)(num4 - num3 + 2.0);
					for (int i = 0; i < num5; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							long num6 = (int)((double)array[j] * Math.Pow(10.0, (double)(i + 1)));
							string frequencyDisplay = SpectrumAnalyzer.GetFrequencyDisplay(num6, false);
							float height = this._graphics.MeasureString(frequencyDisplay, Waterfall._font).Height;
							int num7 = (int)((double)base.ClientRectangle.Height - (Math.Log10((double)num6) - num3) / (num4 - num3) * (double)(base.ClientRectangle.Height - 2 * this._tbMargin) - (double)this._tbMargin);
							if (num7 >= this._tbMargin && num7 <= base.ClientRectangle.Height - this._tbMargin)
							{
								this._graphics.DrawLine(Waterfall._axisPen, 25, num7, 30, num7);
								if (num7 > 20)
								{
									this._graphics.DrawString(frequencyDisplay, Waterfall._font, Waterfall._fontBrush, 1f, (float)num7 - height / 2f);
								}
							}
						}
					}
				}
				else
				{
					double num8 = (double)(int)this._graphics.MeasureString("3", Waterfall._font).Height * 1.8;
					int num9 = (int)((double)((float)this._spectrumWidth / this._scale) * num8 / (double)(base.ClientRectangle.Height - 2 * this._tbMargin));
					int num10 = 50000;
					if ((float)this._spectrumWidth / this._scale <= 500f)
					{
						num10 = 10;
					}
					else if ((float)this._spectrumWidth / this._scale <= 5000f)
					{
						num10 = 100;
					}
					else if ((float)this._spectrumWidth / this._scale <= 50000f)
					{
						num10 = 500;
					}
					else if ((float)this._spectrumWidth / this._scale <= 500000f)
					{
						num10 = 5000;
					}
					num9 = num9 / num10 * num10 + num10;
					int num11 = (int)((float)this._spectrumWidth / this._scale / (float)num9) + 4;
					float num12 = (float)((base.ClientRectangle.Height - 2 * this._tbMargin) * num9) * this._scale / (float)this._spectrumWidth;
					long num13 = this._displayCenterFrequency;
					if (this._dataType == DataType.IF)
					{
						num13 -= this._frequency;
					}
					int num14 = (int)((float)(num13 % num9 * (base.ClientRectangle.Height - 2 * this._tbMargin)) * this._scale / (float)this._spectrumWidth);
					for (int k = -num11 / 2; k < num11 / 2; k++)
					{
						long frequency = num13 + k * num9 - num13 % num9;
						string frequencyDisplay2 = SpectrumAnalyzer.GetFrequencyDisplay(frequency, false);
						float height2 = this._graphics.MeasureString(frequencyDisplay2, Waterfall._font).Height;
						float num15 = (float)(base.ClientRectangle.Height / 2) - num12 * (float)k + (float)num14;
						if (num15 >= (float)this._tbMargin && num15 <= (float)(base.ClientRectangle.Height - this._tbMargin))
						{
							this._graphics.DrawLine(Waterfall._axisPen, 25f, num15, 30f, num15);
							if (num15 > 20f)
							{
								this._graphics.DrawString(frequencyDisplay2, Waterfall._font, Waterfall._fontBrush, 1f, num15 - height2 / 2f);
							}
						}
					}
				}
				this._graphics.DrawString("kHz", Waterfall._font, Waterfall._fontBrush, 1f, 3f);
			}
		}

		private void BuildGradientVector()
		{
			int height = base.ClientRectangle.Height;
			if (height - 30 - 1 > 0)
			{
				if (this._gradientPixels == null || this._gradientPixels.Length != height - 30)
				{
					this._gradientPixels = new int[height - 30 - 1];
				}
				if (this._gradientHash == null)
				{
					this._gradientHash = new Hashtable();
				}
				for (int i = 0; i < this._gradientPixels.Length; i++)
				{
					int num = this._buffer2.GetPixel(base.ClientRectangle.Width - 30 + 7, 15 + i + 1).ToArgb();
					this._gradientPixels[this._gradientPixels.Length - i - 1] = num;
					if (!this._gradientHash.Contains(num))
					{
						this._gradientHash.Add(num, this._gradientPixels.Length - i - 1);
					}
				}
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		protected virtual void OnFrequencyChanged(FrequencyEventArgs e)
		{
			if (this.FrequencyChanged != null)
			{
				this.FrequencyChanged(this, e);
			}
		}

		protected virtual void OnDisplayFrequencyChanged(FrequencyEventArgs e)
		{
			if (this.DisplayFrequencyChanged != null)
			{
				this.DisplayFrequencyChanged(this, e);
			}
		}

		protected virtual void OnCenterFrequencyChanged(FrequencyEventArgs e)
		{
			if (this.CenterFrequencyChanged != null)
			{
				this.CenterFrequencyChanged(this, e);
			}
			else
			{
				e.Cancel = true;
			}
		}

		protected virtual void OnBandwidthChanged(BandwidthEventArgs e)
		{
			if (this.BandwidthChanged != null)
			{
				this.BandwidthChanged(this, e);
			}
		}

		protected virtual void OnAutoZoomed()
		{
			if (this.AutoZoomed != null)
			{
				this.AutoZoomed(this, null);
			}
		}

		private void UpdateFrequency(long f)
		{
			long num = (long)((float)this._displayCenterFrequency - (float)this._spectrumWidth / this._scale / 2f);
			if (f < num)
			{
				f = num;
			}
			long num2 = (long)((float)this._displayCenterFrequency + (float)this._spectrumWidth / this._scale / 2f);
			if (f > num2)
			{
				f = num2;
			}
			if (this._useSnap)
			{
				f = (f + Math.Sign(f) * this._stepSize / 2) / this._stepSize * this._stepSize;
			}
			if (f != this._frequency)
			{
				FrequencyEventArgs frequencyEventArgs = new FrequencyEventArgs(f);
				this.OnFrequencyChanged(frequencyEventArgs);
				if (!frequencyEventArgs.Cancel)
				{
					this._frequency = frequencyEventArgs.Frequency;
				}
			}
		}

		private bool UpdateDisplayFrequency(long f)
		{
			long num = (long)((float)(this._centerFrequency - this.SpectrumWidth / 2) + (float)this._spectrumWidth / this._scale / 2f);
			if (f < num)
			{
				f = num;
			}
			long num2 = (long)((float)(this._centerFrequency + this.SpectrumWidth / 2) - (float)this._spectrumWidth / this._scale / 2f);
			if (f > num2)
			{
				f = num2;
			}
			if (f != this._displayCenterFrequency)
			{
				FrequencyEventArgs e = new FrequencyEventArgs(f);
				this.OnDisplayFrequencyChanged(e);
			}
			if (f > num)
			{
				return f < num2;
			}
			return false;
		}

		private void UpdateCenterFrequency(long f)
		{
			if (f < 0)
			{
				f = 0L;
			}
			int num = Math.Min(1000, this._stepSize);
			if (this._centerSnap)
			{
				f = (f + Math.Sign(f) * num / 2) / num * num;
			}
			if (f != this._centerFrequency)
			{
				FrequencyEventArgs frequencyEventArgs = new FrequencyEventArgs(f);
				this.OnCenterFrequencyChanged(frequencyEventArgs);
				if (!frequencyEventArgs.Cancel)
				{
					long num2 = frequencyEventArgs.Frequency - this._centerFrequency;
					this._displayCenterFrequency += num2;
					this._centerFrequency = frequencyEventArgs.Frequency;
				}
			}
		}

		private void UpdateBandwidth(int bw, int of)
		{
			int num = 1;
			if (this._bandType == BandType.Center && this._detectorType != DetectorType.CW)
			{
				num = 2;
			}
			bw /= num;
			int num2 = (bw < 100) ? 1 : ((bw >= 1000) ? ((bw >= 10000) ? ((bw >= 30000) ? 10000 : 1000) : 100) : 10);
			bw = Math.Max(10, num2 * (bw / num2)) * num;
			if (bw != this._filterBandwidth)
			{
				BandwidthEventArgs bandwidthEventArgs = new BandwidthEventArgs(bw, of);
				this.OnBandwidthChanged(bandwidthEventArgs);
				if (!bandwidthEventArgs.Cancel)
				{
					this._filterBandwidth = bandwidthEventArgs.Bandwidth;
					this._filterOffset = bandwidthEventArgs.Offset;
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				if (this._ver)
				{
					this._oldX1 = e.X;
					if (this.insidePassband((float)e.X))
					{
						this._oldFrequency = this._frequency;
						if (this._dataType != DataType.AF)
						{
							this._changingFrequency = true;
						}
					}
					else if (this.sideOfPassband((float)e.X))
					{
						this._oldFilterBandwidth = this._filterBandwidth;
						this._oldFilterOffset = this._filterOffset;
						this._changingBandwidth = (((float)e.X > (this._lower + this._upper) / 2f) ? 1 : (-1));
					}
					else if (this.inScalingCorner((float)e.X, (float)e.Y))
					{
						if (e.Y < 30)
						{
							this._changingScale = 1;
							this._oldPower = this._maxPower;
						}
						else
						{
							this._changingScale = -1;
							this._oldPower = this._minPower;
						}
						if (this._changingScale != 0)
						{
							this._oldY = e.Y;
						}
					}
					else if (this._dataType == DataType.IF)
					{
						this._oldFrequency = this._frequency;
						this._changingFrequency = true;
					}
					else if (!this._centerFixed || (double)this._scale <= 1.001)
					{
						this._oldCenterFrequency = this._centerFrequency;
						this._changingCenterFrequency = true;
						this._oldFrequency = 1L;
					}
					else
					{
						this._oldFrequency = this._frequency;
						this._oldDisplayFrequency = this._displayCenterFrequency;
						this._changingZoomFrequency = true;
					}
				}
				else
				{
					this._oldY = e.Y;
					Math.Max((float)this._filterBandwidth * this._yIncrement, 2f);
					if (this.insidePassband((float)e.Y) && this._dataType != DataType.AF)
					{
						this._oldFrequency = this._frequency;
						this._changingFrequency = true;
					}
					else if (this.sideOfPassband((float)e.Y))
					{
						this._oldFilterBandwidth = this._filterBandwidth;
						this._oldFilterOffset = this._filterOffset;
						this._changingBandwidth = (((float)e.Y > (float)base.ClientRectangle.Height - (this._lower + this._upper) / 2f) ? 1 : (-1));
					}
					else if (this.inScalingCorner((float)e.X, (float)e.Y))
					{
						if (e.X < 30)
						{
							this._changingScale = 1;
							this._oldPower = this._maxPower;
						}
						else
						{
							this._changingScale = -1;
							this._oldPower = this._minPower;
						}
						if (this._changingScale != 0)
						{
							this._oldY = e.Y;
						}
					}
					else
					{
						this._oldCenterFrequency = this._centerFrequency;
						this._changingCenterFrequency = true;
					}
				}
			}
			else if (e.Button == MouseButtons.Right && this._ver)
			{
				this._oldX1 = e.X;
				this.ContextMenuStrip = this._contextMenu;
				if (!this._centerFixed || (double)this._scale <= 1.001)
				{
					this._oldCenterFrequency = this._centerFrequency;
					this._changingCenterFrequency = true;
					this._oldFrequency = -this._frequency;
				}
				else
				{
					this._oldDisplayFrequency = this._displayCenterFrequency;
					this._changingZoomFrequency = true;
					this._oldFrequency = -this._frequency;
				}
			}
		}

		private bool inScalingCorner(float x, float y)
		{
			if (!(x < 30f) && !(x > (float)(base.ClientRectangle.Width - 30)))
			{
				return false;
			}
			if (!(y < 30f))
			{
				return y > (float)(base.ClientRectangle.Height - 30);
			}
			return true;
		}

		private bool insidePassband(float p)
		{
			if (this._ver)
			{
				float num = Math.Max((float)(this._filterBandwidth + this._filterOffset) * this._x1Increment, 2f);
				if (p > this._lower && p < this._upper)
				{
					return num < (float)base.ClientRectangle.Width;
				}
				return false;
			}
			Math.Max((float)this._filterBandwidth * this._yIncrement, 2f);
			if (p > (float)base.ClientRectangle.Height - this._upper)
			{
				return p < (float)base.ClientRectangle.Height - this._lower;
			}
			return false;
		}

		private bool sideOfPassband(float p)
		{
			if (this._ver)
			{
				if (Math.Abs(p - this._lower + 4f) <= 4f && (this._bandType == BandType.Center || this._bandType == BandType.Lower) && this._dataType != DataType.AF)
				{
					return true;
				}
				if (Math.Abs(p - this._upper - 4f) <= 4f)
				{
					if (this._bandType != BandType.Center && this._bandType != BandType.Upper)
					{
						return this._dataType == DataType.AF;
					}
					return true;
				}
				return false;
			}
			if (this._dataType == DataType.AF)
			{
				return false;
			}
			if (Math.Abs(p - ((float)base.ClientRectangle.Height - this._lower + 4f)) <= 4f && (this._bandType == BandType.Center || this._bandType == BandType.Lower))
			{
				return true;
			}
			if (Math.Abs(p - ((float)base.ClientRectangle.Height - this._upper - 4f)) <= 4f)
			{
				if (this._bandType != BandType.Center)
				{
					return this._bandType == BandType.Upper;
				}
				return true;
			}
			return false;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this._ver)
			{
				if ((this._changingCenterFrequency || this._changingZoomFrequency) && e.X == this._oldX1)
				{
					long f = (long)((float)((this._oldX1 - base.ClientRectangle.Width / 2) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._displayCenterFrequency);
					if (this._oldFrequency >= 0)
					{
						this.UpdateFrequency(f);
					}
				}
			}
			else if (this._changingCenterFrequency && e.Y == this._oldY)
			{
				long f2 = (long)((float)((base.ClientRectangle.Height / 2 - this._oldY) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Height - 2 * this._tbMargin) + (float)this._displayCenterFrequency);
				this.UpdateFrequency(f2);
			}
			this._changingCenterFrequency = false;
			if (this._changingBandwidth != 0)
			{
				this.OnAutoZoomed();
			}
			this._changingBandwidth = 0;
			this._changingZoomFrequency = false;
			this._changingFrequency = false;
			this._changingScale = 0;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this._trackingX = e.X;
			this._trackingY = e.Y;
			if (this._ver)
			{
				this._trackingFrequency = (long)((float)((e.X - base.ClientRectangle.Width / 2) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._displayCenterFrequency);
			}
			else
			{
				this._trackingFrequency = (long)((float)((base.ClientRectangle.Height / 2 - e.Y) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Height - 2 * this._tbMargin) + (float)this._displayCenterFrequency);
			}
			if (this._useSnap)
			{
				this._trackingFrequency = (this._trackingFrequency + Math.Sign(this._trackingFrequency) * this._stepSize / 2) / this._stepSize * this._stepSize;
			}
			if (this._ver)
			{
				if (this._changingFrequency)
				{
					int num = (this._dataType == DataType.RF) ? (e.X - this._oldX1) : (this._oldX1 - e.X);
					long f = (long)((float)(num * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldFrequency);
					this.UpdateFrequency(f);
				}
				else if (this._changingZoomFrequency)
				{
					if (this._oldX1 != e.X)
					{
						this.ContextMenuStrip = null;
						long num2 = (long)((float)((this._oldX1 - e.X) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin));
						if (this.UpdateDisplayFrequency(this._oldDisplayFrequency + num2) && this._oldFrequency > 0)
						{
							bool useSnap = this._useSnap;
							this._useSnap = false;
							this.UpdateFrequency(this._oldFrequency + num2);
							this._useSnap = useSnap;
						}
					}
				}
				else if (this._changingCenterFrequency)
				{
					if (this._oldX1 != e.X)
					{
						long f2 = (long)((float)((this._oldX1 - e.X) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldCenterFrequency);
						this.UpdateCenterFrequency(f2);
						if (this._oldFrequency < 0)
						{
							this.ContextMenuStrip = null;
							this.UpdateFrequency(-this._oldFrequency);
						}
					}
				}
				else if (this._changingBandwidth != 0)
				{
					int num3 = 0;
					int of = 0;
					switch (this._bandType)
					{
					case BandType.Upper:
						num3 = e.X - this._oldX1;
						if (this._dataType == DataType.IF)
						{
							num3 *= 2;
						}
						else if (this._dataType == DataType.AF)
						{
							num3 = num3;
						}
						break;
					case BandType.Lower:
						num3 = this._oldX1 - e.X;
						if (this._dataType == DataType.IF)
						{
							num3 *= 2;
						}
						else if (this._dataType == DataType.AF)
						{
							num3 *= -1;
						}
						break;
					case BandType.Center:
						if (!this._indepSideband)
						{
							num3 = (((float)this._oldX1 > (this._lower + this._upper) / 2f) ? (e.X - this._oldX1) : (this._oldX1 - e.X)) * 2;
						}
						else
						{
							float num4 = (float)base.ClientRectangle.Width / 2f + (float)(this._frequency - this._displayCenterFrequency) * this._x1Increment;
							float num5 = 10f * this._x1Increment;
							float num6 = Math.Max((float)this._oldFilterBandwidth * this._x1Increment, 2f);
							float num7 = num4 + (float)this._oldFilterOffset * this._x1Increment;
							if (this._changingBandwidth > 0)
							{
								num3 = e.X - this._oldX1;
								if (this._dataType != 0)
								{
									num3 *= 2;
								}
								this._upper = num7 + num6 / 2f + (float)num3;
								if (this._upper < num4 + num5)
								{
									return;
								}
							}
							else
							{
								num3 = this._oldX1 - e.X;
								if (this._dataType != 0)
								{
									num3 *= 2;
								}
								this._lower = num7 - num6 / 2f - (float)num3;
								if (this._lower > num4 - num5)
								{
									return;
								}
							}
							of = (int)(((this._lower + this._upper) / 2f - num4) / this._x1Increment);
						}
						break;
					}
					num3 = (int)((float)(num3 * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldFilterBandwidth);
					this.UpdateBandwidth(num3, of);
				}
				else if (this._changingScale != 0)
				{
					float num8 = (float)(e.Y - this._oldY) * (this._maxPower - this._minPower) / (float)(base.ClientRectangle.Height - 2 * this._tbMargin);
					if (this._changingScale > 0)
					{
						this._maxPower = Math.Min(20f, Math.Max(this._minPower + 30f, this._oldPower + num8));
					}
					else
					{
						this._minPower = Math.Max(-200f, Math.Min(this._maxPower - 30f, this._oldPower + num8));
					}
					this.DrawGradient();
				}
				else if (this.sideOfPassband((float)e.X))
				{
					this.Cursor = Mouses.ChangeBW;
				}
				else if (this.insidePassband((float)e.X) && this._dataType != DataType.AF)
				{
					this.Cursor = Mouses.Passband;
				}
				else if (this.inScalingCorner((float)e.X, (float)e.Y))
				{
					this.Cursor = Mouses.Scale;
				}
				else
				{
					this.Cursor = Mouses.Spectrum;
				}
			}
			else if (this._changingFrequency)
			{
				int num9 = (this._dataType == DataType.RF) ? (this._oldY - e.Y) : (e.Y - this._oldY);
				long f3 = (long)((float)(num9 * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Height - 2 * this._tbMargin) + (float)this._oldFrequency);
				this.UpdateFrequency(f3);
			}
			else if (this._changingCenterFrequency)
			{
				long f4 = (long)((float)((this._oldY - e.Y) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Height - 2 * this._tbMargin) + (float)this._oldCenterFrequency);
				this.UpdateCenterFrequency(f4);
			}
			else if (this._changingBandwidth != 0)
			{
				int num10 = 0;
				int of2 = 0;
				switch (this._bandType)
				{
				case BandType.Upper:
					num10 = e.Y - this._oldY;
					break;
				case BandType.Lower:
					num10 = this._oldY - e.Y;
					break;
				case BandType.Center:
					if (!this._indepSideband)
					{
						num10 = (((float)this._oldY > (float)base.ClientRectangle.Height - (this._lower + this._upper) / 2f) ? (e.Y - this._oldY - e.Y) : (this._oldY - e.Y)) * 2;
					}
					else
					{
						float num11 = (float)base.ClientRectangle.Height / 2f + (float)(this._frequency - this._displayCenterFrequency) * this._yIncrement;
						float num12 = 10f * this._yIncrement;
						float num13 = Math.Max((float)this._oldFilterBandwidth * this._yIncrement, 2f);
						float num14 = num11 + (float)this._oldFilterOffset * this._yIncrement;
						if (this._changingBandwidth > 0)
						{
							num10 = e.Y - this._oldY;
							if (this._dataType != 0)
							{
								num10 *= 2;
							}
							this._upper = num14 + num13 / 2f + (float)num10;
							if (this._upper < num11 + num12)
							{
								return;
							}
						}
						else
						{
							num10 = this._oldY - e.Y;
							if (this._dataType != 0)
							{
								num10 *= 2;
							}
							this._lower = num14 - num13 / 2f - (float)num10;
							if (this._lower > num11 - num12)
							{
								return;
							}
						}
						of2 = (int)(((this._lower + this._upper) / 2f - num11) / this._yIncrement);
					}
					break;
				}
				num10 = (int)((float)(num10 * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Height - 2 * this._tbMargin) + (float)this._oldFilterBandwidth);
				this.UpdateBandwidth(num10, of2);
			}
			else if (this._changingScale != 0)
			{
				float num15 = (float)(e.Y - this._oldY) * (this._maxPower - this._minPower) / (float)(base.ClientRectangle.Height - 2 * this._tbMargin);
				if (this._changingScale > 0)
				{
					this._maxPower = Math.Min(20f, Math.Max(this._minPower + 30f, this._oldPower + num15));
				}
				else
				{
					this._minPower = Math.Max(-200f, Math.Min(this._maxPower - 30f, this._oldPower + num15));
				}
				this.DrawGradient();
			}
			else if (this.sideOfPassband((float)e.Y))
			{
				this.Cursor = Mouses.ChangeBW;
			}
			else if (this.insidePassband((float)e.Y) && this._dataType != DataType.AF)
			{
				this.Cursor = Mouses.Passband;
			}
			else if (this.inScalingCorner((float)e.X, (float)e.Y))
			{
				this.Cursor = Mouses.Scale;
			}
			else if (this._dataType != DataType.AF)
			{
				this.Cursor = Mouses.Spectrum;
			}
			else
			{
				this.Cursor = Mouses.Static;
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this._mouseIn = true;
			this.CopyMainBuffer();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this._mouseIn = false;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			this.UpdateFrequency(this._frequency + (this._useSnap ? (this._stepSize * Math.Sign(e.Delta)) : (e.Delta / 10)));
		}

		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
			case Keys.Prior:
			case Keys.Next:
			case Keys.Left:
			case Keys.Up:
			case Keys.Right:
			case Keys.Down:
				return true;
			case Keys.LButton | Keys.MButton | Keys.Space | Keys.Shift:
			case Keys.RButton | Keys.MButton | Keys.Space | Keys.Shift:
			case Keys.LButton | Keys.RButton | Keys.MButton | Keys.Space | Keys.Shift:
			case Keys.Back | Keys.Space | Keys.Shift:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			switch (e.KeyData)
			{
			case Keys.LButton | Keys.RButton | Keys.MButton | Keys.Space | Keys.Shift:
				this.UpdateCenterFrequency(this._centerFrequency + this._stepSize);
				break;
			case Keys.LButton | Keys.MButton | Keys.Space | Keys.Shift:
				this.UpdateCenterFrequency(this._centerFrequency - this._stepSize);
				break;
			case Keys.Prior:
			case Keys.Up:
				this.UpdateCenterFrequency(this._centerFrequency + this._centerStep);
				break;
			case Keys.Next:
			case Keys.Down:
				this.UpdateCenterFrequency(this._centerFrequency - this._centerStep);
				break;
			case Keys.Left:
			case Keys.Right:
			{
				int num = (e.KeyData != Keys.Left) ? 1 : (-1);
				long num2 = this._frequency + num * this.StepSize;
				if (!this._centerFixed && (double)num2 < (double)this._centerFrequency + (double)(num * this.SpectrumWidth) * 0.4)
				{
					this.UpdateCenterFrequency(this._centerFrequency + num * this.StepSize);
				}
				else
				{
					this.UpdateFrequency(num2);
				}
				break;
			}
			}
		}

		private long getBasebandCenter()
		{
			if (this._dataType == DataType.AF)
			{
				return this._spectrumWidth / 2;
			}
			if (this._bandType == BandType.Lower)
			{
				return this._frequency - this._filterBandwidth / 2;
			}
			if (this._bandType == BandType.Upper)
			{
				return this._frequency + this._filterBandwidth / 2;
			}
			return this._frequency + this._filterOffset;
		}
	}
}
