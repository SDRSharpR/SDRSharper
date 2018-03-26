using SDRSharp.Radio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	public class SpectrumAnalyzer : UserControl
	{
		private const float TrackingFontSize = 14f;

		private const int ScaleMargin = 30;

		private const int GradientAlpha = 180;

		public const float DefaultCursorHeight = 32f;

		private int _lrMargin;

		private int _tbMargin = 22;

		private double _attack;

		private double _decay;

		private bool _performNeeded;

		private int _resetMaximumNeeded;

		private bool _drawBackgroundNeeded;

		private byte[] _spectrum;

		private byte[] _maximum;

		private byte[] _timout;

		private bool[] _peaks;

		private byte[] _temp;

		private byte[] _scaledPowerSpectrum;

		private Bitmap _bkgBuffer;

		private Bitmap _buffer;

		private Graphics _bkgGraphics;

		private Graphics _graphics;

		private long _spectrumWidth;

		private long _centerFrequency;

		private long _displayCenterFrequency;

		private Point[] _points;

		private BandType _bandType;

		private int _filterBandwidth;

		private int _filterOffset;

		private int _oldFilterOffset;

		private bool _indepSideband;

		private int _stepSize = 1000;

		private int _centerStep = 10000;

		private float _xIncrement;

		private long _frequency;

		private float _lower;

		private float _upper;

		private float _scale = 1f;

		private int _oldX;

		private int _oldY;

		private int _trackingX;

		private int _trackingY;

		private long _trackingFrequency;

		private int _oldFilterBandwidth;

		private long _oldFrequency;

		private long _oldCenterFrequency;

		private long _oldDisplayFrequency;

		private int _changingBandwidth;

		private bool _changingFrequency;

		private bool _changingZoomFrequency;

		private bool _changingCenterFrequency;

		private int _changingScale;

		private float _oldPower;

		private bool _useSmoothing;

		private bool _hotTrackNeeded;

		private bool _useSnap;

		private bool _centerSnap;

		private bool _markPeaks;

		private bool _savePeaks;

		private bool _carrierMode;

		private float _trackingPower;

		private string _statusText;

		private static Color _backgroundColor;

		private PathGradientBrush _backgroundBrush;

		private static Color _spectrumColor;

		private static int _spectrumFill = 0;

		private static ColorBlend _dynamicBlend = new ColorBlend(3);

		private static ColorBlend _gradientColorBlend;

		private LinearGradientBrush _gradientBrush;

		private int[] _gradientPixels = new int[255];

		private float _minPower;

		private float _maxPower;

		private int _showDbm;

		private int _signalDbm;

		private bool _centerFixed;

		private DataType _dataType;

		private int _notch;

		public Notch[] _notches = new Notch[4]
		{
			new Notch(),
			new Notch(),
			new Notch(),
			new Notch()
		};

		private bool _changingNotchOffset;

		private bool _changingNotchWidth;

		private int _oldValue;

		private frmStationList _frmStationList;

		private string _stationList;

		private Meter _sMeter = new Meter();

		private ContextMenuStrip _contextMenu;

		private static Pen _gridPen = new Pen(Color.FromArgb(100, 100, 100));

		private static Pen _axisPen = new Pen(Color.FromArgb(150, 150, 150));

		private static Pen _tickPen = new Pen(Color.FromArgb(255, 255, 255));

		private static Brush _fontBrush = new SolidBrush(Color.FromArgb(220, 220, 220));

		private static Pen _spectrumPen = new Pen(Color.White);

		private static Pen _maximumPen = new Pen(Color.White);

		private static Pen _carrierPen = new Pen(Color.Red);

		private static Pen _hotTrackPen = new Pen(Color.Yellow);

		private static Pen _notchPen = new Pen(Color.Green);

		private static Pen _outlinePen = new Pen(Color.Black, 4f);

		private static Pen _overflowPen = new Pen(Color.FromArgb(255, 120, 120));

		private static Brush _transparentBrush = new SolidBrush(Color.FromArgb(100, Color.DarkGray));

		private static Brush _transparentBrush2 = new SolidBrush(Color.FromArgb(50, Color.DarkGray));

		private static Brush _redBrush = new SolidBrush(Color.Red);

		private static FontFamily _fontFamily = new FontFamily("Arial");

		private static Font _font = new Font("Arial", 8f);

		private static Font _statusFont = new Font("Lucida Console", 9f);

		public int SignalDbm => this._signalDbm;

		public int ShowDbm
		{
			get
			{
				return this._showDbm;
			}
			set
			{
				this._showDbm = value;
				this._drawBackgroundNeeded = true;
			}
		}

		public DetectorType DetectorType
		{
			set
			{
				this._sMeter.DetectorType = value;
				this._carrierMode = (value == DetectorType.AM || value == DetectorType.SAM || value == DetectorType.CW);
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
				this._drawBackgroundNeeded = true;
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
				this._drawBackgroundNeeded = true;
				this._resetMaximumNeeded = 10;
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
				this._drawBackgroundNeeded = true;
				if (this._dataType != 0)
				{
					this.CenterFrequency = this.getBasebandCenter();
					if (!base.Name.Contains("pectrum"))
					{
						this._lrMargin = 30;
					}
				}
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

		public string StationList
		{
			get
			{
				if (this._frmStationList != null)
				{
					return this._frmStationList.Stations;
				}
				return "";
			}
			set
			{
				this._stationList = value;
				if (this._frmStationList != null)
				{
					this._frmStationList.Stations = value;
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
					this.ApplyZoom();
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
					this._performNeeded = true;
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
					this._performNeeded = true;
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
					this._performNeeded = true;
					if (this._dataType != 0)
					{
						this.CenterFrequency = this.getBasebandCenter();
					}
				}
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
						if (value < num2 + 10 || value > num - 10)
						{
							this.UpdateDisplayFrequency(f);
						}
					}
					this._frequency = value;
					this._performNeeded = true;
					if (this._dataType != 0)
					{
						this.CenterFrequency = this.getBasebandCenter();
						this._drawBackgroundNeeded = false;
						this._resetMaximumNeeded = 10;
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
					this._drawBackgroundNeeded = true;
					Console.WriteLine(base.Name + " disp=" + this._displayCenterFrequency.ToString());
					this._resetMaximumNeeded = 1;
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
					this._drawBackgroundNeeded = true;
					this._resetMaximumNeeded = 10;
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
					if (this._dataType == DataType.AF && this._sMeter.DetectorType == DetectorType.CW)
					{
						num = 800;
					}
					value = Math.Max(1f, (float)this._spectrumWidth / ((float)num * 1.5f));
					this.resetMaximum();
				}
				this._drawBackgroundNeeded = true;
				if (this._scale != value)
				{
					this._scale = value;
					this.ApplyZoom();
				}
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

		public Color SpectrumColor
		{
			get
			{
				return SpectrumAnalyzer._spectrumColor;
			}
			set
			{
				if (!(SpectrumAnalyzer._spectrumColor == value))
				{
					SpectrumAnalyzer._spectrumColor = value;
					SpectrumAnalyzer._spectrumPen.Dispose();
					SpectrumAnalyzer._spectrumPen = new Pen(SpectrumAnalyzer._spectrumColor);
				}
			}
		}

		public int SpectrumFill
		{
			get
			{
				return SpectrumAnalyzer._spectrumFill;
			}
			set
			{
				SpectrumAnalyzer._spectrumFill = value;
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return SpectrumAnalyzer._backgroundColor;
			}
			set
			{
				this.newBackgroundBrush(value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ColorBlend GradientColorBlend
		{
			set
			{
				SpectrumAnalyzer._dynamicBlend.Colors[0] = value.Colors[0];
				SpectrumAnalyzer._dynamicBlend.Positions[0] = 0f;
				SpectrumAnalyzer._dynamicBlend.Colors[1] = value.Colors[0];
				SpectrumAnalyzer._dynamicBlend.Positions[1] = 0f;
				SpectrumAnalyzer._dynamicBlend.Colors[2] = value.Colors[value.Colors.Length - 1];
				SpectrumAnalyzer._dynamicBlend.Positions[2] = 1f;
				this.newGradientBrush(value);
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

		public int StepSize
		{
			get
			{
				return this._stepSize;
			}
			set
			{
				if (this._stepSize != value)
				{
					this._stepSize = value;
					this._drawBackgroundNeeded = true;
					this._performNeeded = true;
				}
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
				this._useSnap = (this._dataType == DataType.RF && value);
			}
		}

		public bool MarkPeaks
		{
			get
			{
				return this._markPeaks;
			}
			set
			{
				this._markPeaks = value;
				this._savePeaks = value;
				this.resetMaximum();
			}
		}

		public bool CenterSnap
		{
			set
			{
				this._centerSnap = value;
			}
		}

		public event ManualFrequencyChange FrequencyChanged;

		public event ManualFrequencyChange DisplayFrequencyChanged;

		public event ManualFrequencyChange CenterFrequencyChanged;

		public event ManualBandwidthChange BandwidthChanged;

		public event ManualNotchChange NotchChanged;

		public event EventHandler StationDatachanged;

		public event EventHandler AutoZoomed;

		public event EventHandler TimeChanged;

		public SpectrumAnalyzer()
		{
			this._bkgBuffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
			this._bkgGraphics = Graphics.FromImage(this._bkgBuffer);
			this._buffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
			this._graphics = Graphics.FromImage(this._buffer);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.UpdateStyles();
			SpectrumAnalyzer._gridPen.DashStyle = DashStyle.Dash;
		}

		~SpectrumAnalyzer()
		{
			this._buffer.Dispose();
			this._graphics.Dispose();
			this._bkgGraphics.Dispose();
			this._gradientBrush.Dispose();
			this._backgroundBrush.Dispose();
		}

		public void Test()
		{
			this._gradientBrush.InterpolationColors = SpectrumAnalyzer._gradientColorBlend;
			this.fillGradientVector(SpectrumAnalyzer._gradientColorBlend, 255);
		}

		public void SetNotch(int num, int offset, int width, bool active)
		{
			if (width == 0)
			{
				if (this._notches[num].Width == 0)
				{
					width = this._filterBandwidth / 30;
					offset = this._filterBandwidth / 2 - (num + 1) * this._filterBandwidth / 10;
				}
				else
				{
					width = Math.Abs(this._notches[num].Width);
					offset = this._notches[num].Offset;
				}
				if (!active)
				{
					width = -width;
				}
			}
			this.UpdateNotch(num, offset, width, active);
		}

		public void ShowStationList()
		{
			if (this._frmStationList == null || this._frmStationList.IsDisposed)
			{
				this._frmStationList = new frmStationList();
				this._frmStationList.StationDataChanged += this.frmStationList_StationDataChanged;
			}
			if (!this._frmStationList.Visible)
			{
				this._frmStationList.Show();
			}
			this._frmStationList.ShowDbm = this._showDbm;
			this._frmStationList.Stations = this._stationList;
			this._frmStationList.Activate();
		}

		private void frmStationList_StationDataChanged(object sender, EventArgs e)
		{
			this._stationList = this._frmStationList.Stations;
			this.StationDatachanged(sender, e);
		}

		private void ApplyZoom()
		{
			if (this._spectrumWidth > 0)
			{
				if (this._dataType == DataType.RF)
				{
					this._displayCenterFrequency = this.GetDisplayCenterFrequency();
				}
				else if (this._dataType == DataType.AF)
				{
					this._displayCenterFrequency = (long)((float)this._centerFrequency / this._scale);
				}
				else
				{
					this._displayCenterFrequency = this._centerFrequency;
				}
				this._xIncrement = this._scale * (float)(base.ClientRectangle.Width - 2 * this._lrMargin) / (float)this._spectrumWidth;
				this._drawBackgroundNeeded = true;
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
			long num2 = (long)((float)(this._centerFrequency - this._spectrumWidth / 2) - ((float)num - (float)this._spectrumWidth / this._scale / 2f));
			if (num2 > 0)
			{
				num += num2 + 10;
			}
			long num3 = (long)((float)num + (float)this._spectrumWidth / this._scale / 2f - (float)(this._centerFrequency + this._spectrumWidth / 2));
			if (num3 > 0)
			{
				num -= num3 + 10;
			}
			return num;
		}

		private void fillGradientVector(ColorBlend blend, int size)
		{
			int num = 4;
			Bitmap bitmap = new Bitmap(num, size, PixelFormat.Format32bppArgb);
			Graphics graphics = Graphics.FromImage(bitmap);
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, size), Color.Black, Color.White);
			linearGradientBrush.InterpolationColors = blend;
			graphics.FillRectangle(linearGradientBrush, new Rectangle(0, 0, num, size));
			int num2 = 220;
			for (int i = 0; i < size; i++)
			{
				num2 = 255 - (int)((float)i / (float)size * 127f);
				Color color = Color.FromArgb(num2, bitmap.GetPixel(num / 2, i));
				this._gradientPixels[i] = color.ToArgb();
			}
			graphics.Dispose();
			bitmap.Dispose();
		}

		public void Perform()
		{
			if (base.ClientRectangle.Width > 30 && base.ClientRectangle.Height > 30 && (this._performNeeded || this._drawBackgroundNeeded))
			{
				if (this._drawBackgroundNeeded)
				{
					this.DrawBackground();
				}
				this.CopyBackground();
				this.DrawSpectrum();
				this.DrawCursor();
				this.DrawStatusText();
				base.Invalidate();
				if (this._resetMaximumNeeded > 0)
				{
					this.resetMaximum();
				}
				this._drawBackgroundNeeded = false;
				this._performNeeded = false;
			}
		}

		private void DrawCursor()
		{
			float num = 0f;
			float num2 = 0f;
			this._lower = 0f;
			float num3 = Math.Max((float)(this._filterBandwidth + this._filterOffset) * this._xIncrement, 2f);
			float num4 = (float)base.ClientRectangle.Width / 2f + (float)(this._frequency - this._displayCenterFrequency) * this._xIncrement;
			switch (this._bandType)
			{
			case BandType.Upper:
				num = (float)this._filterOffset * this._xIncrement;
				num2 = num3 - num;
				this._lower = num4;
				break;
			case BandType.Lower:
				num = (float)this._filterOffset * this._xIncrement;
				num2 = num3 - num;
				this._lower = num4 - num2;
				break;
			case BandType.Center:
			{
				num3 = Math.Max((float)this._filterBandwidth * this._xIncrement, 2f);
				num = (float)this._filterOffset * this._xIncrement;
				num2 = num3;
				float num5 = num4 + num;
				this._lower = num5 - num3 / 2f;
				break;
			}
			}
			if (this.DataType == DataType.AF && this._bandType != BandType.Center)
			{
				this._lower = num4;
			}
			this._upper = this._lower + num2;
			using (Graphics graphics = Graphics.FromImage(this._buffer))
			{
				using (GraphicsPath path = new GraphicsPath())
				{
					if (num3 < (float)base.ClientRectangle.Width || (this._dataType == DataType.AF && num3 < (float)(2 * base.ClientRectangle.Width)))
					{
						if (this._dataType == DataType.RF)
						{
							graphics.FillRectangle(SpectrumAnalyzer._transparentBrush, (int)this._lower + 1, 0, (int)num2, base.ClientRectangle.Height);
						}
						else if (this._dataType == DataType.IF && this._hotTrackNeeded)
						{
							graphics.FillRectangle(SpectrumAnalyzer._transparentBrush2, this._lrMargin, 0, (int)this._lower - this._lrMargin + 1, base.ClientRectangle.Height);
							graphics.FillRectangle(SpectrumAnalyzer._transparentBrush2, (float)((int)this._upper + 1), 0f, (float)(base.ClientRectangle.Width - this._lrMargin) - this._upper - 1f, (float)base.ClientRectangle.Height);
						}
						else if (this._dataType == DataType.AF && this._hotTrackNeeded)
						{
							graphics.FillRectangle(SpectrumAnalyzer._transparentBrush2, (float)((int)this._upper + 1), 0f, (float)(base.ClientRectangle.Width - this._lrMargin) - this._upper - 1f, (float)base.ClientRectangle.Height);
						}
						if (num4 >= (float)this._lrMargin && num4 <= (float)(base.ClientRectangle.Width - this._lrMargin) && this._dataType != DataType.AF)
						{
							graphics.DrawLine(SpectrumAnalyzer._carrierPen, num4, (float)this._tbMargin, num4, (float)(base.ClientRectangle.Height - this._tbMargin));
						}
					}
					if (this._dataType == DataType.IF)
					{
						for (int i = 0; i < 4; i++)
						{
							if (this._notches[i].Width > 0)
							{
								this._notches[i].Xhalf = Math.Max((float)this._notches[i].Width * this._xIncrement, 4f) / 2f;
								this._notches[i].Xpos = (float)base.ClientRectangle.Width / 2f + (float)this._notches[i].Offset * this._xIncrement;
								if (this._notches[i].Xhalf * 2f < (float)base.ClientRectangle.Width)
								{
									graphics.FillRectangle(SpectrumAnalyzer._transparentBrush, (float)(int)this._notches[i].Xpos - this._notches[i].Xhalf + 1f, 0f, (float)((int)this._notches[i].Xhalf * 2), (float)base.ClientRectangle.Height);
									if (!(this._notches[i].Xpos < (float)this._lrMargin) && !(this._notches[i].Xpos > (float)(base.ClientRectangle.Width - this._lrMargin)))
									{
										SpectrumAnalyzer._notchPen.Color = (this._notches[i].Active ? Color.Red : Color.FromArgb(100, 255, 100));
										graphics.DrawLine(SpectrumAnalyzer._notchPen, this._notches[i].Xpos, (float)this._tbMargin, this._notches[i].Xpos, (float)(base.ClientRectangle.Height - this._tbMargin));
									}
								}
							}
						}
					}
					int val = (int)num2;
					val = Math.Max(val, 10);
					val = Math.Min(val, this._spectrum.Length);
					if (this._hotTrackNeeded && this._trackingX >= this._lrMargin && this._trackingX <= base.ClientRectangle.Width - this._lrMargin && this._trackingY >= this._tbMargin && this._trackingY <= base.ClientRectangle.Height - this._tbMargin)
					{
						if (this._spectrum != null && !this._changingFrequency && !this._changingCenterFrequency && this._changingBandwidth == 0 && !this._changingNotchOffset && !this._changingNotchWidth)
						{
							int num6 = this._trackingX - this._lrMargin;
							if (num6 > 0 && num6 < this._spectrum.Length)
							{
								graphics.DrawLine(SpectrumAnalyzer._hotTrackPen, this._trackingX, 0, this._trackingX, base.ClientRectangle.Height);
							}
						}
						string text;
						if (this._changingNotchOffset)
						{
							text = "Notch=" + SpectrumAnalyzer.GetFrequencyDisplay(this._frequency + this._notches[this._notch].Offset, true);
						}
						else if (this._changingFrequency)
						{
							text = "VFO = " + SpectrumAnalyzer.GetFrequencyDisplay(this._frequency, true);
						}
						else if (this._changingBandwidth != 0)
						{
							text = "BW = " + SpectrumAnalyzer.GetFrequencyDisplay(this._filterBandwidth, true);
						}
						else if (this._changingNotchWidth)
						{
							text = "Width = " + SpectrumAnalyzer.GetFrequencyDisplay(this._notches[this._notch].Width, true);
						}
						else if (this._changingCenterFrequency)
						{
							text = ((this._dataType == DataType.RF) ? "Center Freq. = " : "VFO = ") + SpectrumAnalyzer.GetFrequencyDisplay(this._centerFrequency, true);
						}
						else if (this._changingScale != 0)
						{
							text = "Scale";
						}
						else
						{
							long num7 = this._trackingFrequency;
							if (this._dataType == DataType.IF)
							{
								num7 -= this._frequency;
							}
							text = $"{SpectrumAnalyzer.GetFrequencyDisplay(num7, true)}\r\n{Utils.Signal((int)this._trackingPower, this._showDbm, true)}";
						}
						Utils.AddString(path, text, 14f, Math.Min(base.ClientRectangle.Width - 80, this._trackingX + 10), Math.Max(0, this._trackingY - 35));
						Utils.DrawPath(graphics, path, Brushes.White, 4);
					}
					if (this._dataType == DataType.RF || this._carrierMode)
					{
						int num8 = 0;
						int num9 = 0;
						for (int j = (int)this._lower - this._lrMargin; j < (int)this._upper - this._lrMargin; j++)
						{
							if (j >= 0 && j < this._spectrum.Length && this._spectrum[j] > num8)
							{
								num8 = this._spectrum[j];
								num9 = j;
							}
						}
						this._signalDbm = (int)(this._minPower + (this._maxPower - this._minPower) / 255f * (float)num8);
						bool flag = num8 >= 255;
						if (this._dataType == DataType.RF)
						{
							this._sMeter.Draw(graphics, (float)this._signalDbm, this._showDbm, flag);
						}
						if (this._carrierMode && this._dataType != DataType.AF)
						{
							float num10 = (float)(base.ClientRectangle.Height - 2 * this._tbMargin) / 255f;
							int num11 = (int)((float)(base.ClientRectangle.Height - this._tbMargin) - (float)(int)this._spectrum[num9] * num10);
							graphics.DrawEllipse(flag ? SpectrumAnalyzer._overflowPen : Pens.Yellow, this._lrMargin + num9 - 2, num11 - 2, 4, 4);
						}
					}
				}
			}
		}

		public unsafe void Render(float* powerSpectrum, int length)
		{
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
			if (this._useSmoothing)
			{
				Fourier.SmoothCopy(this._scaledPowerSpectrum, this._temp, length, this._scale, offset);
				for (int i = 0; i < this._spectrum.Length; i++)
				{
					double num2 = (this._spectrum[i] < this._temp[i]) ? this.Attack : this.Decay;
					this._spectrum[i] = (byte)Math.Round((double)(int)this._spectrum[i] * (1.0 - num2) + (double)(int)this._temp[i] * num2);
				}
				byte val = 4;
				if (this._markPeaks)
				{
					for (int j = 0; j < this._spectrum.Length; j++)
					{
						if (this._temp[j] > this._maximum[j])
						{
							this._maximum[j] = this._temp[j];
							this._timout[j] = 100;
						}
						else if (this._timout[j] > 0)
						{
							this._timout[j]--;
						}
						else if (this._maximum[j] > 0)
						{
							this._maximum[j] -= Math.Min(val, this._maximum[j]);
						}
					}
				}
			}
			else
			{
				Fourier.SmoothCopy(this._scaledPowerSpectrum, this._spectrum, length, this._scale, offset);
			}
			this._performNeeded = true;
		}

		private void DrawBackground()
		{
			Graphics bkgGraphics = this._bkgGraphics;
			bkgGraphics.Clear(Utils.ChangeColor(SpectrumAnalyzer._backgroundColor, 0.4f));
			bkgGraphics.FillRectangle(this._backgroundBrush, 0, 0, base.ClientRectangle.Width, base.ClientRectangle.Height - this._tbMargin);
			if (this._spectrumWidth > 0)
			{
				long num = (long)((float)this._spectrumWidth / this._scale);
				int num2 = (int)bkgGraphics.MeasureString("30000", SpectrumAnalyzer._font).Width;
				int num3 = (int)(num * num2 / (base.ClientRectangle.Width - 2 * this._lrMargin));
				int num4 = 50000;
				if (num <= 500)
				{
					num4 = 10;
				}
				else if (num <= 5000)
				{
					num4 = 100;
				}
				else if (num <= 50000)
				{
					num4 = 500;
				}
				else if (num <= 500000)
				{
					num4 = 5000;
				}
				num3 = num3 / num4 * num4 + num4;
				int num5 = (int)(num / num3) + 4;
				float num6 = (float)((base.ClientRectangle.Width - 2 * this._lrMargin) * num3) * this._scale / (float)this._spectrumWidth;
				long num7 = this._displayCenterFrequency;
				if (this._dataType == DataType.IF)
				{
					num7 -= this._frequency;
				}
				int num8 = (int)((float)(num7 % num3 * (base.ClientRectangle.Width - 2 * this._lrMargin)) * this._scale / (float)this._spectrumWidth);
				int num9 = (int)((double)num3 / Math.Pow(10.0, Math.Log10((double)(num3 / 10))));
				if (num9 <= 10)
				{
					num9 = 5;
				}
				else
				{
					switch (num9)
					{
					case 15:
						num9 = 3;
						break;
					case 20:
						num9 = 4;
						break;
					case 25:
						num9 = 5;
						break;
					case 50:
						num9 = 5;
						break;
					}
				}
				for (int i = -num5 / 2; i < num5 / 2; i++)
				{
					float num10 = (float)((base.ClientRectangle.Width - 2 * this._lrMargin) / 2 + this._lrMargin) + num6 * (float)i - (float)num8;
					if (num10 >= 30f && num10 < (float)(base.ClientRectangle.Width - this._lrMargin))
					{
						bkgGraphics.DrawLine(SpectrumAnalyzer._gridPen, num10, (float)this._tbMargin, num10, (float)(base.ClientRectangle.Height - this._tbMargin));
					}
					for (int j = 0; j < num9; j++)
					{
						float num11 = num10 + (float)j * (num6 / (float)num9);
						if (num11 > (float)this._lrMargin && num11 < (float)(base.ClientRectangle.Width - this._lrMargin))
						{
							bkgGraphics.DrawLine((j == 0) ? SpectrumAnalyzer._tickPen : SpectrumAnalyzer._axisPen, num11, (float)(base.ClientRectangle.Height - this._tbMargin), num11, (float)(base.ClientRectangle.Height - this._tbMargin + ((j == 0) ? 5 : 3)));
						}
					}
					string frequencyDisplay = SpectrumAnalyzer.GetFrequencyDisplay(num7 + i * num3 - num7 % num3, false);
					SizeF sizeF = bkgGraphics.MeasureString(frequencyDisplay, SpectrumAnalyzer._font);
					if (num10 > (float)this._lrMargin && num10 < (float)(base.ClientRectangle.Width - 45))
					{
						bkgGraphics.DrawString(frequencyDisplay, SpectrumAnalyzer._font, SpectrumAnalyzer._fontBrush, num10 - sizeF.Width / 2f, (float)(base.ClientRectangle.Height - this._tbMargin + 6));
					}
				}
				string s = "kHz";
				if ((float)num7 + (float)this._spectrumWidth / (2f * this._scale) > 3E+07f)
				{
					s = "Mhz";
				}
				else if ((float)num7 + (float)this._spectrumWidth / (2f * this._scale) < 1000f)
				{
					s = "Hz";
				}
				bkgGraphics.DrawString(s, SpectrumAnalyzer._font, SpectrumAnalyzer._fontBrush, (float)(base.ClientRectangle.Width - 30), (float)(base.ClientRectangle.Height - this._tbMargin + 6));
			}
			bkgGraphics.DrawLine(SpectrumAnalyzer._axisPen, this._lrMargin, base.ClientRectangle.Height - this._tbMargin, base.ClientRectangle.Width - this._lrMargin, base.ClientRectangle.Height - this._tbMargin);
			int num12 = (int)this._minPower;
			int num13 = (int)this._maxPower;
			int num14 = num13 - num12;
			int num15 = base.ClientRectangle.Height - 2 * this._tbMargin;
			float num16 = (float)num15 / (float)num14;
			int num17 = 20;
			int num18 = 12;
			string text = num13.ToString();
			SizeF sizeF2 = bkgGraphics.MeasureString(text, SpectrumAnalyzer._font);
			float height = sizeF2.Height;
			double num19 = (double)num15 / ((double)height * 1.2);
			if ((double)num14 <= num19)
			{
				num17 = 1;
			}
			else if ((double)(num14 / 2) <= num19)
			{
				num17 = 2;
			}
			else if ((double)(num14 / 3) <= num19)
			{
				num17 = 3;
			}
			else if ((double)(num14 / 6) <= num19)
			{
				num17 = 5;
			}
			else if ((double)(num14 / 10) <= num19)
			{
				num17 = 10;
			}
			if (this._showDbm >= 2)
			{
				num17 = ((num17 > 7) ? 10 : 5);
			}
			for (int k = num12; k < num13; k++)
			{
				bool flag = false;
				if (this._showDbm == 0)
				{
					if (k % num17 == 0)
					{
						flag = true;
					}
				}
				else if (this._showDbm == 1)
				{
					if ((k + 7) % num17 == 0)
					{
						flag = true;
					}
				}
				else if (this._showDbm == 3)
				{
					if ((k + 3) % num17 == 0)
					{
						flag = true;
					}
				}
				else if (k > -73)
				{
					if ((k + 73) % num17 == 0)
					{
						flag = true;
					}
				}
				else
				{
					switch (num17)
					{
					case 5:
						num18 = 6;
						break;
					case 10:
						num18 = 12;
						break;
					}
					if ((k + 121) % num18 == 0)
					{
						flag = true;
					}
				}
				if (flag)
				{
					float num20 = (float)this._tbMargin + (float)(num13 - k) * num16;
					bkgGraphics.DrawLine(SpectrumAnalyzer._gridPen, (float)this._lrMargin, num20, (float)(base.ClientRectangle.Width - this._lrMargin), num20);
					text = Utils.Signal(k, this._showDbm, false);
					sizeF2 = bkgGraphics.MeasureString(text, SpectrumAnalyzer._font);
					if (num20 < (float)(base.ClientRectangle.Height - this._tbMargin) - sizeF2.Height / 2f)
					{
						bkgGraphics.DrawString(text, SpectrumAnalyzer._font, SpectrumAnalyzer._fontBrush, 30f - sizeF2.Width - 1f, num20 - sizeF2.Height / 2f);
					}
				}
			}
			if ((float)this._lrMargin > sizeF2.Width)
			{
				bkgGraphics.DrawLine(SpectrumAnalyzer._axisPen, this._lrMargin, this._tbMargin, this._lrMargin, base.ClientRectangle.Height - this._tbMargin);
			}
			if (this._dataType == DataType.RF)
			{
				this._sMeter.DrawBackground(bkgGraphics);
			}
		}

		private void resetMaximum()
		{
			if (this._spectrum != null)
			{
				for (int i = 0; i < this._spectrum.Length; i++)
				{
					this._maximum[i] = 0;
					this._timout[i] = 0;
				}
				if (this._resetMaximumNeeded > 0)
				{
					this._resetMaximumNeeded--;
					this._markPeaks = false;
					if (this._resetMaximumNeeded == 0)
					{
						this._markPeaks = this._savePeaks;
					}
				}
			}
		}

		public static string GetFrequencyDisplay(long frequency, bool unit)
		{
			if (frequency == 0)
			{
				return "0";
			}
			if (Math.Abs(frequency) > 30000000)
			{
				return string.Format(unit ? "{0:0.##}MHz" : "{0:0.##}", (double)frequency / 1000000.0);
			}
			if (Math.Abs(frequency) >= 1000)
			{
				return string.Format(unit ? "{0:0.##}kHz" : "{0:0.##}", (double)frequency / 1000.0);
			}
			return string.Format(unit ? "{0}Hz" : "{0}", frequency);
		}

		public static void ConfigureGraphics(Graphics graphics)
		{
			graphics.CompositingMode = CompositingMode.SourceOver;
			graphics.CompositingQuality = CompositingQuality.HighSpeed;
			graphics.SmoothingMode = SmoothingMode.None;
			graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			graphics.InterpolationMode = InterpolationMode.High;
		}

		private void DrawForeground()
		{
			if (base.ClientRectangle.Width > 30 && base.ClientRectangle.Height > 30)
			{
				this.CopyBackground();
				this.DrawSpectrum();
				this.DrawStatusText();
				this.DrawCursor();
			}
		}

		private unsafe void CopyBackground()
		{
			BitmapData bitmapData = this._buffer.LockBits(base.ClientRectangle, ImageLockMode.WriteOnly, this._buffer.PixelFormat);
			BitmapData bitmapData2 = this._bkgBuffer.LockBits(base.ClientRectangle, ImageLockMode.ReadOnly, this._bkgBuffer.PixelFormat);
			Utils.Memcpy((void*)bitmapData.Scan0, (void*)bitmapData2.Scan0, Math.Abs(bitmapData.Stride) * bitmapData.Height);
			this._bkgBuffer.UnlockBits(bitmapData2);
			this._buffer.UnlockBits(bitmapData);
		}

		private void DrawSpectrum()
		{
			if (this._spectrum != null && this._spectrum.Length != 0)
			{
				float num = (float)(base.ClientRectangle.Width - 2 * this._lrMargin) / (float)this._spectrum.Length;
				float num2 = (float)(base.ClientRectangle.Height - 2 * this._tbMargin) / 255f;
				Point pt = new Point(0, base.ClientRectangle.Height - this._tbMargin + 1);
				Point pt2 = new Point(0, this._tbMargin - 1);
				if (SpectrumAnalyzer._dynamicBlend != null)
				{
					int num3 = SpectrumAnalyzer._dynamicBlend.Positions.Length;
					for (int i = 0; i < this._spectrum.Length; i++)
					{
						byte b = this._spectrum[i];
						int x = (int)((float)this._lrMargin + (float)i * num);
						int y = (int)((float)(base.ClientRectangle.Height - this._tbMargin) - (float)(int)b * num2);
						this._points[i + 1].X = x;
						this._points[i + 1].Y = y;
						if (SpectrumAnalyzer._spectrumFill >= 2)
						{
							pt.X = x;
							pt.Y = base.ClientRectangle.Height - this._tbMargin - 1;
							pt2.X = x;
							pt2.Y = y;
							if (SpectrumAnalyzer._spectrumFill == 2)
							{
								float num4 = Math.Max(0f, (float)(int)b / 255f);
								for (int j = 0; j < num3 - 2; j++)
								{
									SpectrumAnalyzer._dynamicBlend.Positions[j + 1] = 1f - (1f - (float)j / (float)(num3 - 2)) * num4;
								}
								this._gradientBrush.InterpolationColors = SpectrumAnalyzer._dynamicBlend;
								if (pt.Y > pt2.Y)
								{
									this._graphics.DrawLine(new Pen(this._gradientBrush), pt, pt2);
								}
							}
							else
							{
								b = (byte)Math.Max(0, 255 - b - 1);
								if (pt.Y > pt2.Y)
								{
									this._graphics.DrawLine(new Pen(Color.FromArgb(this._gradientPixels[b])), pt, pt2);
								}
							}
						}
					}
					if (SpectrumAnalyzer._spectrumFill == 1)
					{
						this._points[0].X = this._lrMargin + 1;
						this._points[0].Y = base.ClientRectangle.Height - this._tbMargin;
						this._points[this._points.Length - 1].X = base.ClientRectangle.Width - this._lrMargin - 1;
						this._points[this._points.Length - 1].Y = base.ClientRectangle.Height - this._tbMargin;
						this._graphics.FillPolygon(this._gradientBrush, this._points);
					}
					if (this._markPeaks)
					{
						for (int k = 0; k < this._spectrum.Length; k++)
						{
							this._points[k + 1].X = (int)((float)this._lrMargin + (float)k * num);
							this._points[k + 1].Y = (int)((float)(base.ClientRectangle.Height - this._tbMargin) - (float)(int)this._maximum[k] * num2);
						}
						this._points[0] = this._points[1];
						this._points[this._points.Length - 1] = this._points[this._points.Length - 2];
						this._graphics.DrawLines(SpectrumAnalyzer._maximumPen, this._points);
					}
					else
					{
						this._points[0] = this._points[1];
						this._points[this._points.Length - 1] = this._points[this._points.Length - 2];
						this._graphics.DrawLines(SpectrumAnalyzer._spectrumPen, this._points);
					}
				}
			}
		}

		private void DrawStatusText()
		{
			if (!string.IsNullOrEmpty(this._statusText))
			{
				this._graphics.DrawString(this._statusText, SpectrumAnalyzer._statusFont, Brushes.Silver, 7f, 7f);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			SpectrumAnalyzer.ConfigureGraphics(e.Graphics);
			e.Graphics.DrawImageUnscaled(this._buffer, 0, 0);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (base.ClientRectangle.Width > this._lrMargin && base.ClientRectangle.Height >= this._tbMargin)
			{
				this._buffer.Dispose();
				this._graphics.Dispose();
				this._bkgBuffer.Dispose();
				this._bkgGraphics.Dispose();
				this._buffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
				this._graphics = Graphics.FromImage(this._buffer);
				SpectrumAnalyzer.ConfigureGraphics(this._graphics);
				this._bkgBuffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
				this._bkgGraphics = Graphics.FromImage(this._bkgBuffer);
				SpectrumAnalyzer.ConfigureGraphics(this._graphics);
				int num = base.ClientRectangle.Width - 2 * this._lrMargin;
				byte[] spectrum = this._spectrum;
				this._spectrum = new byte[num];
				this._maximum = new byte[num];
				this._timout = new byte[num];
				this._peaks = new bool[num];
				if (spectrum != null)
				{
					Fourier.SmoothCopy(spectrum, this._spectrum, spectrum.Length, 1f, 0);
				}
				else
				{
					for (int i = 0; i < this._spectrum.Length; i++)
					{
						this._spectrum[i] = 0;
					}
				}
				this._temp = new byte[num];
				this._points = new Point[num + 2];
				if (this._spectrumWidth > 0)
				{
					this._xIncrement = this._scale * (float)base.ClientRectangle.Width / (float)this._spectrumWidth;
				}
				if (this._gradientBrush != null)
				{
					this._gradientBrush.Dispose();
				}
				this._gradientBrush = new LinearGradientBrush(new Rectangle(0, this._tbMargin, base.ClientRectangle.Width, base.ClientRectangle.Height - 2 * this._tbMargin), Color.White, Color.Black, LinearGradientMode.Vertical);
				this.newGradientBrush(null);
				this.newBackgroundBrush(null);
				this._sMeter.Position(base.ClientRectangle.Width - 130, 25, 120, 120);
				if (this._dataType == DataType.IF)
				{
					this.ApplyZoom();
				}
				this._drawBackgroundNeeded = true;
				this.Perform();
			}
		}

		private void newGradientBrush(ColorBlend blend = null)
		{
			if (blend != null)
			{
				if (SpectrumAnalyzer._gradientColorBlend == null || SpectrumAnalyzer._gradientColorBlend.Colors.Length != blend.Colors.Length)
				{
					SpectrumAnalyzer._gradientColorBlend = new ColorBlend(blend.Colors.Length);
				}
				for (int i = 0; i < blend.Colors.Length; i++)
				{
					SpectrumAnalyzer._gradientColorBlend.Colors[i] = Color.FromArgb(180, blend.Colors[i]);
					SpectrumAnalyzer._gradientColorBlend.Positions[i] = blend.Positions[i];
				}
			}
			else if (SpectrumAnalyzer._gradientColorBlend == null)
			{
				return;
			}
			if (SpectrumAnalyzer._spectrumFill == 1)
			{
				this._gradientBrush.InterpolationColors = SpectrumAnalyzer._gradientColorBlend;
			}
			if (SpectrumAnalyzer._spectrumFill == 3)
			{
				this.fillGradientVector(SpectrumAnalyzer._gradientColorBlend, 255);
			}
		}

		private void newBackgroundBrush(Color? color = default(Color?))
		{
			if (color.HasValue)
			{
				SpectrumAnalyzer._backgroundColor = color.Value;
			}
			if (this._backgroundBrush != null)
			{
				this._backgroundBrush.Dispose();
			}
			this._backgroundBrush = Utils.BackgroundBrush(base.Name, SpectrumAnalyzer._backgroundColor, base.ClientRectangle, true, false);
			this._drawBackgroundNeeded = true;
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

		protected virtual void OnTimeChanged()
		{
			if (this.TimeChanged != null)
			{
				this.TimeChanged(this, null);
			}
		}

		protected virtual void OnNotchChanged(NotchEventArgs e)
		{
			if (this.NotchChanged != null)
			{
				this.NotchChanged(this, e);
			}
		}

		private void UpdateNotch(int n, int f, int bw, bool act)
		{
			if (f == this._notches[n].Offset && bw == this._notches[n].Width && act == this._notches[n].Active)
			{
				return;
			}
			this._notches[n].Offset = f;
			this._notches[n].Width = bw;
			this._notches[n].Active = act;
			this.OnNotchChanged(new NotchEventArgs(n, f, bw, act));
			this._performNeeded = true;
		}

		private void UpdateFrequency(long f)
		{
			long num = this._centerFrequency - this._spectrumWidth / 2;
			if (f < num)
			{
				f = num;
			}
			long num2 = this._centerFrequency + this._spectrumWidth / 2;
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
					this._performNeeded = true;
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
					this._drawBackgroundNeeded = true;
				}
			}
		}

		private void UpdateBandwidth(int bw, int of)
		{
			int num = 1;
			if (this._bandType == BandType.Center && this._sMeter.DetectorType != DetectorType.CW)
			{
				num = 2;
			}
			bw /= num;
			int num2 = (bw < 100) ? 1 : ((bw >= 1000) ? ((bw >= 10000) ? ((bw >= 30000) ? 10000 : 1000) : 100) : 10);
			bw = Math.Max(10, num2 * (bw / num2)) * num;
			if (bw == this._filterBandwidth && of == this._filterOffset)
			{
				return;
			}
			BandwidthEventArgs bandwidthEventArgs = new BandwidthEventArgs(bw, of);
			this.OnBandwidthChanged(bandwidthEventArgs);
			if (!bandwidthEventArgs.Cancel)
			{
				this._filterBandwidth = bandwidthEventArgs.Bandwidth;
				this._filterOffset = bandwidthEventArgs.Offset;
				this._performNeeded = true;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				this._oldX = e.X;
				this._oldY = e.Y;
				int notch;
				if (this._dataType == DataType.IF && (notch = this.insideNotch((float)e.X)) >= 0)
				{
					this._notch = notch;
					this._oldValue = this._notches[this._notch].Offset;
					this._changingNotchOffset = true;
				}
				else if (this._dataType == DataType.IF && (notch = this.sideOfNotch((float)e.X)) >= 0)
				{
					this._notch = notch;
					this._oldValue = this._notches[this._notch].Width;
					this._changingNotchWidth = true;
				}
				else if (this._dataType == DataType.RF && this.insidePassband((float)e.X))
				{
					this._oldFrequency = this._frequency;
					this._changingFrequency = true;
				}
				else if (this.sideOfPassband((float)e.X))
				{
					this._oldFilterBandwidth = this._filterBandwidth;
					this._oldFilterOffset = this._filterOffset;
					this._changingBandwidth = (((float)e.X > (this._lower + this._upper) / 2f) ? 1 : (-1));
				}
				else if (this.inScalingCorner((float)e.X, (float)e.Y))
				{
					this._drawBackgroundNeeded = true;
					if ((double)e.Y < 1.5 * (double)this._tbMargin)
					{
						this._changingScale = 1;
						this._oldPower = this._maxPower;
					}
					else
					{
						this._changingScale = -1;
						this._oldPower = this._minPower;
					}
				}
				else if (this._dataType == DataType.RF && e.X < 30)
				{
					if (this._centerFixed && (double)this._scale > 1.001)
					{
						this.UpdateFrequency(this._frequency - this._centerStep);
					}
					else
					{
						this.UpdateCenterFrequency(this._centerFrequency - this._centerStep);
					}
				}
				else if (this._dataType == DataType.RF && e.X > base.ClientRectangle.Width - 30)
				{
					if (this._centerFixed && (double)this._scale > 1.001)
					{
						this.UpdateFrequency(this._frequency + this._centerStep);
					}
					else
					{
						this.UpdateCenterFrequency(this._centerFrequency + this._centerStep);
					}
				}
				else if (e.Y < this._tbMargin && this._centerFrequency <= 30000000)
				{
					if (this._dataType == DataType.RF)
					{
						this.ShowStationList();
					}
				}
				else if (this._dataType != 0)
				{
					this._oldFrequency = this._frequency;
					if (this._dataType == DataType.IF)
					{
						this._changingFrequency = true;
					}
				}
				else if (!this._centerFixed || (double)this._scale <= 1.001)
				{
					this._oldCenterFrequency = this._centerFrequency;
					this._changingCenterFrequency = true;
					this._oldFrequency = 1L;
				}
				else
				{
					this._oldDisplayFrequency = this._displayCenterFrequency;
					this._changingZoomFrequency = true;
					this._oldFrequency = this._frequency;
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				this.ContextMenuStrip = this._contextMenu;
				if (this._dataType == DataType.RF)
				{
					this._oldX = e.X;
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
				else if (this._dataType == DataType.IF && this.insidePassband((float)e.X))
				{
					this.ContextMenuStrip = null;
					int num = this.insideNotch((float)e.X);
					if (num >= 0 && this._notches[num].Width < 0)
					{
						num = -1;
					}
					if (num >= 0)
					{
						this.UpdateNotch(num, this._notches[num].Offset, -Math.Abs(this._notches[num].Width), false);
					}
					else
					{
						int f = (int)((float)((e.X - base.ClientRectangle.Width / 2) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin));
						for (num = 0; num < 4 && this._notches[num].Active; num++)
						{
						}
						if (num < 4)
						{
							if (this._notches[num].Width == 0)
							{
								this._notches[num].Width = this._filterBandwidth / 30;
							}
							this.UpdateNotch(num, f, Math.Abs(this._notches[num].Width), true);
						}
					}
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ((this._changingCenterFrequency || this._changingFrequency || this._changingZoomFrequency) && e.X == this._oldX)
			{
				long f = (long)((float)((this._oldX - base.ClientRectangle.Width / 2) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._displayCenterFrequency);
				if (this._oldFrequency >= 0)
				{
					this.UpdateFrequency(f);
				}
			}
			if (this._changingNotchOffset && e.X == this._oldX)
			{
				this.UpdateNotch(this._notch, this._notches[this._notch].Offset, this._notches[this._notch].Width, !this._notches[this._notch].Active);
			}
			this._changingCenterFrequency = false;
			if (this._changingBandwidth != 0)
			{
				this.OnAutoZoomed();
			}
			this._changingBandwidth = 0;
			this._changingZoomFrequency = false;
			this._changingFrequency = false;
			this._changingNotchOffset = false;
			this._changingNotchWidth = false;
			if (this._changingScale != 0)
			{
				this._drawBackgroundNeeded = true;
			}
			this._changingScale = 0;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this._hotTrackNeeded = true;
			this._trackingX = e.X;
			this._trackingY = e.Y;
			this._trackingFrequency = (long)((float)((e.X - base.ClientRectangle.Width / 2) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._displayCenterFrequency);
			if (this._useSnap)
			{
				this._trackingFrequency = (this._trackingFrequency + Math.Sign(this._trackingFrequency) * this._stepSize / 2) / this._stepSize * this._stepSize;
			}
			float num = (float)(base.ClientRectangle.Height - 2 * this._tbMargin) / (this._maxPower - this._minPower);
			this._trackingPower = this._minPower - (float)(this._trackingY + this._tbMargin - base.ClientRectangle.Height) / num;
			if (this._changingNotchOffset)
			{
				int num2 = e.X - this._oldX;
				long num3 = (long)((float)(num2 * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldValue);
				this.UpdateNotch(this._notch, (int)num3, this._notches[this._notch].Width, this._notches[this._notch].Active);
			}
			else if (this._changingNotchWidth)
			{
				int num4 = (((float)this._oldX > this._notches[this._notch].Xpos) ? (e.X - this._oldX) : (this._oldX - e.X)) * 2;
				num4 = (int)((float)(num4 * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldValue);
				this.UpdateNotch(this._notch, this._notches[this._notch].Offset, Math.Max(num4, 50), this._notches[this._notch].Active);
			}
			else if (this._changingFrequency)
			{
				int num5 = (this._dataType == DataType.RF) ? (e.X - this._oldX) : (this._oldX - e.X);
				long f = (long)((float)(num5 * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldFrequency);
				this.UpdateFrequency(f);
			}
			else
			{
				if (this._changingZoomFrequency)
				{
					if (this._oldX != e.X)
					{
						this.ContextMenuStrip = null;
						long num6 = (long)((float)((this._oldX - e.X) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin));
						if (this.UpdateDisplayFrequency(this._oldDisplayFrequency + num6) && this._oldFrequency > 0)
						{
							bool useSnap = this._useSnap;
							this._useSnap = false;
							this.UpdateFrequency(this._oldFrequency + num6);
							this._useSnap = useSnap;
						}
						goto IL_0846;
					}
					return;
				}
				if (this._changingCenterFrequency)
				{
					if (this._oldX != e.X)
					{
						long f2 = (long)((float)((this._oldX - e.X) * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldCenterFrequency);
						this.UpdateCenterFrequency(f2);
						if (this._oldFrequency < 0)
						{
							this.ContextMenuStrip = null;
							this.UpdateFrequency(-this._oldFrequency);
						}
						goto IL_0846;
					}
					return;
				}
				if (this._changingBandwidth != 0)
				{
					if (e.X != this._oldX)
					{
						int of = this._filterOffset;
						int num7 = 0;
						switch (this._bandType)
						{
						case BandType.Upper:
							num7 = e.X - this._oldX;
							if (this._dataType == DataType.IF)
							{
								num7 *= 2;
							}
							else if (this._dataType == DataType.AF)
							{
								num7 = num7;
							}
							break;
						case BandType.Lower:
							num7 = this._oldX - e.X;
							if (this._dataType == DataType.IF)
							{
								num7 *= 2;
							}
							else if (this._dataType == DataType.AF)
							{
								num7 *= -1;
							}
							break;
						case BandType.Center:
							if (!this._indepSideband)
							{
								num7 = (((float)this._oldX > (this._lower + this._upper) / 2f) ? (e.X - this._oldX) : (this._oldX - e.X)) * 2;
							}
							else
							{
								float num8 = (float)base.ClientRectangle.Width / 2f + (float)(this._frequency - this._displayCenterFrequency) * this._xIncrement;
								float num9 = 10f * this._xIncrement;
								float num10 = Math.Max((float)this._oldFilterBandwidth * this._xIncrement, 2f);
								float num11 = num8 + (float)this._oldFilterOffset * this._xIncrement;
								if (this._changingBandwidth > 0)
								{
									num7 = e.X - this._oldX;
									if (this._dataType == DataType.IF)
									{
										num7 *= 2;
									}
									this._upper = num11 + num10 / 2f + (float)num7;
									if (this._upper < num8 + num9)
									{
										return;
									}
								}
								else
								{
									num7 = this._oldX - e.X;
									if (this._dataType == DataType.IF)
									{
										num7 *= 2;
									}
									this._lower = num11 - num10 / 2f - (float)num7;
									if (this._lower > num8 - num9)
									{
										return;
									}
								}
								of = (int)(((this._lower + this._upper) / 2f - num8) / this._xIncrement);
							}
							break;
						}
						num7 = (int)((float)(num7 * this._spectrumWidth) / this._scale / (float)(base.ClientRectangle.Width - 2 * this._lrMargin) + (float)this._oldFilterBandwidth);
						this.UpdateBandwidth(num7, of);
						goto IL_0846;
					}
					return;
				}
				if (this._changingScale != 0)
				{
					float num12 = (float)(e.Y - this._oldY) * (this._maxPower - this._minPower) / (float)(base.ClientRectangle.Height - 2 * this._tbMargin);
					if (this._changingScale > 0)
					{
						this._maxPower = Math.Min(20f, Math.Max(this._minPower + 30f, this._oldPower + num12));
					}
					else
					{
						this._minPower = Math.Max(-160f, Math.Min(this._maxPower - 30f, this._oldPower + num12));
					}
					this._drawBackgroundNeeded = true;
				}
				else if (this._dataType == DataType.IF && this.sideOfNotch((float)e.X) >= 0)
				{
					this.Cursor = Mouses.ChangeBW;
				}
				else if (this.sideOfPassband((float)e.X))
				{
					this.Cursor = Mouses.ChangeBW;
				}
				else if (this._dataType == DataType.IF && this.insideNotch((float)e.X) >= 0)
				{
					this.Cursor = Mouses.Passband;
				}
				else if (this._changingScale != 0 || this.inScalingCorner((float)e.X, (float)e.Y))
				{
					this.Cursor = Mouses.Scale;
				}
				else if (this._dataType == DataType.RF && e.X < 30)
				{
					this.Cursor = Mouses.RangeDown;
				}
				else if (this._dataType == DataType.RF && e.X > base.ClientRectangle.Width - 30)
				{
					this.Cursor = Mouses.RangeUp;
				}
				else if (this._dataType == DataType.RF && (float)e.X > this._lower && (float)e.X < this._upper)
				{
					this.Cursor = Mouses.Passband;
				}
				else if (e.Y < this._tbMargin)
				{
					this.Cursor = Mouses.StationList;
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
			goto IL_0846;
			IL_0846:
			this._performNeeded = true;
		}

		private bool inScalingCorner(float x, float y)
		{
			if (!(x < 30f) && !(x > (float)(base.ClientRectangle.Width - 30)))
			{
				return false;
			}
			if (!((double)y < (double)this._tbMargin * 1.5))
			{
				return (double)y > (double)base.ClientRectangle.Height - (double)this._tbMargin * 1.5;
			}
			return true;
		}

		private bool insidePassband(float x)
		{
			float num = Math.Max((float)(this._filterBandwidth + this._filterOffset) * this._xIncrement, 2f);
			if (x > this._lower && x < this._upper)
			{
				return num < (float)base.ClientRectangle.Width;
			}
			return false;
		}

		private bool sideOfPassband(float x)
		{
			if (Math.Abs(x - this._lower + 4f) <= 4f && (this._bandType == BandType.Center || this._bandType == BandType.Lower) && this._dataType != DataType.AF)
			{
				return true;
			}
			if (Math.Abs(x - this._upper - 4f) <= 4f)
			{
				if (this._bandType != BandType.Center && this._bandType != BandType.Upper)
				{
					return this._dataType == DataType.AF;
				}
				return true;
			}
			return false;
		}

		private int insideNotch(float x)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this._notches[i].Width > 0 && x > this._notches[i].Xpos - this._notches[i].Xhalf && x < this._notches[i].Xpos + this._notches[i].Xhalf)
				{
					return i;
				}
			}
			return -1;
		}

		private int sideOfNotch(float x)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this._notches[i].Width > 0 && (Math.Abs(x - (this._notches[i].Xpos - this._notches[i].Xhalf) + 4f) <= 4f || Math.Abs(x - (this._notches[i].Xpos + this._notches[i].Xhalf) - 4f) <= 4f))
				{
					return i;
				}
			}
			return -1;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			int num = -1;
			if (this._dataType == DataType.IF)
			{
				num = this.insideNotch((float)e.X);
			}
			if (num < 0)
			{
				this.UpdateFrequency(this._frequency + (this._useSnap ? (this._stepSize * Math.Sign(e.Delta)) : e.Delta));
			}
			else
			{
				this.UpdateNotch(num, this._notches[num].Offset, this._notches[num].Width + e.Delta, this._notches[num].Active);
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this._hotTrackNeeded = false;
			this._performNeeded = true;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this._hotTrackNeeded = true;
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
				if (this._centerFixed && (double)this._scale > 1.001)
				{
					this.UpdateFrequency(this._frequency + this._centerStep);
				}
				else
				{
					this.UpdateCenterFrequency(this._centerFrequency + this._centerStep);
				}
				break;
			case Keys.Next:
			case Keys.Down:
				if (this._centerFixed && (double)this._scale > 1.001)
				{
					this.UpdateFrequency(this._frequency - this._centerStep);
				}
				else
				{
					this.UpdateCenterFrequency(this._centerFrequency - this._centerStep);
				}
				break;
			case Keys.Left:
				this._hotTrackNeeded = false;
				if (!this._centerFixed && (double)(this._frequency - this.StepSize) < (double)this._displayCenterFrequency - (double)((float)this._spectrumWidth / this._scale) / 2.2)
				{
					this.UpdateCenterFrequency(this._centerFrequency - this.StepSize);
				}
				else
				{
					this.UpdateFrequency(this._frequency - this.StepSize);
				}
				break;
			case Keys.Right:
				this._hotTrackNeeded = false;
				if (!this._centerFixed && (double)(this._frequency + this.StepSize) > (double)this._displayCenterFrequency + (double)((float)this._spectrumWidth / this._scale) / 2.2)
				{
					this.UpdateCenterFrequency(this._centerFrequency + this.StepSize);
				}
				else
				{
					this.UpdateFrequency(this._frequency + this.StepSize);
				}
				break;
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
