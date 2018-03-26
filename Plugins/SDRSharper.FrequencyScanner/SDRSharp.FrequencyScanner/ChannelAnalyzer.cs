using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SDRSharp.FrequencyScanner
{
	public class ChannelAnalyzer : UserControl
	{
		private const float TrackingFontSize = 16f;

		private const int CarrierPenWidth = 1;

		private const int GradientAlpha = 180;

		public const int AxisMargin = 2;

		public const float DefaultCursorHeight = 32f;

		private bool _drawBackgroundNeeded;

		private Bitmap _bkgBuffer;

		private Bitmap _buffer;

		private Graphics _graphics;

		private float _xIncrement;

		private float _yIncrement;

		private long _playFrequency;

		private int _playFrequencyIndex;

		private List<ChannelAnalizerMemoryEntry> _channelArray;

		private int _zoom;

		private int _zoomPosition;

		private float _defXIncrement;

		public long Frequency
		{
			set
			{
				this._playFrequency = value;
			}
		}

		public int Zoom
		{
			get
			{
				return this._zoom;
			}
			set
			{
				this._zoom = value;
			}
		}

		public int ZoomPosition
		{
			get
			{
				return this._zoomPosition;
			}
			set
			{
				this._zoomPosition = value;
			}
		}

		public event CustomPaintEventHandler CustomPaint;

		public ChannelAnalyzer()
		{
			Rectangle clientRectangle = base.ClientRectangle;
			int width = clientRectangle.Width;
			clientRectangle = base.ClientRectangle;
			this._bkgBuffer = new Bitmap(width, clientRectangle.Height, PixelFormat.Format32bppPArgb);
			clientRectangle = base.ClientRectangle;
			int width2 = clientRectangle.Width;
			clientRectangle = base.ClientRectangle;
			this._buffer = new Bitmap(width2, clientRectangle.Height, PixelFormat.Format32bppPArgb);
			this._graphics = Graphics.FromImage(this._buffer);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.UpdateStyles();
		}

		~ChannelAnalyzer()
		{
			this._buffer.Dispose();
			this._graphics.Dispose();
		}

		public void Perform(List<ChannelAnalizerMemoryEntry> channelArray)
		{
			this._channelArray = channelArray;
			if (this._drawBackgroundNeeded)
			{
				this.DrawBackground();
			}
			this.DrawForeground();
			base.Invalidate();
			this._drawBackgroundNeeded = false;
		}

		private void DrawBackground()
		{
			using (SolidBrush solidBrush = new SolidBrush(Color.Silver))
			{
				using (Pen pen = new Pen(Color.FromArgb(80, 80, 80)))
				{
					using (new Pen(Color.DarkGray))
					{
						using (Font font = new Font("Arial", 8f))
						{
							using (Graphics graphics = Graphics.FromImage(this._bkgBuffer))
							{
								ChannelAnalyzer.ConfigureGraphics(graphics);
								graphics.Clear(Color.Black);
								pen.DashStyle = DashStyle.Dash;
								int num = 20;
								Rectangle clientRectangle = base.ClientRectangle;
								int num2 = (clientRectangle.Height - 4) / num;
								for (int i = 1; i <= num2; i++)
								{
									Graphics graphics2 = graphics;
									Pen pen2 = pen;
									clientRectangle = base.ClientRectangle;
									int y = clientRectangle.Height - 2 - i * num;
									clientRectangle = base.ClientRectangle;
									int x = clientRectangle.Width - 2;
									clientRectangle = base.ClientRectangle;
									graphics2.DrawLine(pen2, 2, y, x, clientRectangle.Height - 2 - i * num);
								}
								for (int j = 1; j <= num2; j++)
								{
									string text = (j * num / 2 - 10).ToString();
									SizeF sizeF = graphics.MeasureString(text, font);
									float width = sizeF.Width;
									float height = sizeF.Height;
									Graphics graphics3 = graphics;
									string s = text;
									Font font2 = font;
									SolidBrush brush = solidBrush;
									clientRectangle = base.ClientRectangle;
									graphics3.DrawString(s, font2, brush, 4f, (float)(clientRectangle.Height - 2 - j * num) - height + 1f);
								}
							}
						}
					}
				}
			}
		}

		private string GetFrequencyDisplay(long frequency)
		{
			if (frequency == 0L)
			{
				return "DC";
			}
			if (Math.Abs(frequency) > 1500000000)
			{
				return $"{(double)frequency / 1000000000.0:#,0.000 000}GHz";
			}
			if (Math.Abs(frequency) > 30000000)
			{
				return $"{(double)frequency / 1000000.0:0,0.000###}MHz";
			}
			if (Math.Abs(frequency) > 1000)
			{
				return $"{(double)frequency / 1000.0:#,#.###}kHz";
			}
			return $"{frequency}Hz";
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
			Rectangle clientRectangle = base.ClientRectangle;
			if (clientRectangle.Width > 2)
			{
				clientRectangle = base.ClientRectangle;
				if (clientRectangle.Height > 2)
				{
					this.CopyBackground();
					this.DrawSpectrum();
					this.OnCustomPaint(new CustomPaintEventArgs(this._graphics));
				}
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

		private void DrawSpectrum()
		{
			if (this._channelArray != null && this._channelArray.Count != 0)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				this._xIncrement = (float)(clientRectangle.Width - 4) / (float)this._channelArray.Count * (float)(this._zoom + 1);
				this._yIncrement = 2f;
				clientRectangle = base.ClientRectangle;
				this._defXIncrement = (float)(clientRectangle.Width - 4) / (float)this._channelArray.Count;
				float num = 0f;
				float num2 = 0f;
				this._playFrequencyIndex = -1;
				using (Pen pen = new Pen(Color.SkyBlue))
				{
					using (Pen pen4 = new Pen(Color.DarkRed))
					{
						using (Pen pen5 = new Pen(Color.Yellow))
						{
							using (Pen pen3 = new Pen(Color.Green))
							{
								for (int i = 0; i < this._channelArray.Count; i++)
								{
									float num3 = (float)Math.Max((int)this._channelArray[i].Level, 4) * this._yIncrement;
									float num4 = this._xIncrement - 1f;
									if (this._xIncrement < 3f)
									{
										num4 = this._xIncrement;
									}
									float num5 = 2f + (float)i * this._xIncrement - (float)(this._zoomPosition * (this._zoom + 1)) + (float)this._zoomPosition + num4 * 0.5f;
									clientRectangle = base.ClientRectangle;
									float num6 = (float)Math.Max(2, (int)((float)(clientRectangle.Height - 2) - num3));
									float val = num3;
									clientRectangle = base.ClientRectangle;
									num3 = Math.Min(val, (float)(clientRectangle.Height - 4));
									if (!(num5 < 2f))
									{
										float num7 = num5;
										clientRectangle = base.ClientRectangle;
										if (!(num7 > (float)(clientRectangle.Width - 2)))
										{
											Pen pen2 = pen;
											if (this._playFrequency > this._channelArray[i].Frequency - this._channelArray[i].StepSize / 2 && this._playFrequency < this._channelArray[i].Frequency + this._channelArray[i].StepSize / 2)
											{
												num = num5;
												num2 = num6;
												this._playFrequencyIndex = i;
											}
											if (this._channelArray[i].IsStore)
											{
												pen2 = pen3;
											}
											if (this._channelArray[i].Skipped)
											{
												pen2 = ((!this._channelArray[i].IsStore) ? pen4 : pen5);
											}
											pen2.Width = num4;
											this._graphics.DrawLine(pen2, num5, num6, num5, num6 + num3);
										}
									}
								}
							}
						}
					}
				}
				using (SolidBrush brush = new SolidBrush(Color.White))
				{
					using (Pen pen6 = new Pen(Color.White))
					{
						using (Font font = new Font("Arial", 8f))
						{
							if (this._playFrequencyIndex >= 0 && this._playFrequencyIndex < this._channelArray.Count)
							{
								string str = this.GetFrequencyDisplay(this._channelArray[this._playFrequencyIndex].Frequency) + " ";
								str = ((!this._channelArray[this._playFrequencyIndex].IsStore) ? (str + "Unknown") : (str + this._channelArray[this._playFrequencyIndex].StoreName));
								SizeF sizeF = this._graphics.MeasureString(str, font);
								float width = sizeF.Width;
								float height = sizeF.Height;
								float num8 = 0f;
								int num9 = 62;
								this._graphics.DrawLine(pen6, num, num2 - 10f, num, (float)num9 + height + 4f);
								float num10 = num + width + 10f;
								clientRectangle = base.ClientRectangle;
								num8 = ((!(num10 > (float)clientRectangle.Width)) ? num : (num - width));
								this._graphics.DrawLine(pen6, num8, (float)num9 + height + 4f, num8 + width, (float)num9 + height + 4f);
								this._graphics.DrawString(str, font, brush, num8, (float)num9);
							}
						}
					}
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			ChannelAnalyzer.ConfigureGraphics(e.Graphics);
			e.Graphics.DrawImageUnscaled(this._buffer, 0, 0);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Rectangle clientRectangle = base.ClientRectangle;
			if (clientRectangle.Width > 0)
			{
				clientRectangle = base.ClientRectangle;
				if (clientRectangle.Height > 0)
				{
					this._buffer.Dispose();
					this._graphics.Dispose();
					this._bkgBuffer.Dispose();
					clientRectangle = base.ClientRectangle;
					int width = clientRectangle.Width;
					clientRectangle = base.ClientRectangle;
					this._buffer = new Bitmap(width, clientRectangle.Height, PixelFormat.Format32bppPArgb);
					this._graphics = Graphics.FromImage(this._buffer);
					ChannelAnalyzer.ConfigureGraphics(this._graphics);
					clientRectangle = base.ClientRectangle;
					int width2 = clientRectangle.Width;
					clientRectangle = base.ClientRectangle;
					this._bkgBuffer = new Bitmap(width2, clientRectangle.Height, PixelFormat.Format32bppPArgb);
					this._drawBackgroundNeeded = true;
					this.Perform(this._channelArray);
				}
			}
		}

		protected virtual void OnCustomPaint(CustomPaintEventArgs e)
		{
			CustomPaintEventHandler customPaint = this.CustomPaint;
			customPaint?.Invoke(this, e);
		}

		public int CurrentChannelIndex()
		{
			return this._playFrequencyIndex;
		}

		public int PointToChannel(float point)
		{
			int num = 0;
			num = (int)(((float)(this._zoomPosition * (this._zoom + 1)) + point - (float)this._zoomPosition - 2f) / this._xIncrement);
			if (num < 0)
			{
				num = 0;
			}
			if (num >= this._channelArray.Count)
			{
				num = this._channelArray.Count - 1;
			}
			return num;
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			this.AutoSize = true;
			this.DoubleBuffered = true;
			base.Name = "ChannelAnalyzer";
			base.ResumeLayout(false);
		}
	}
}
