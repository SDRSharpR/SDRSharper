using SDRSharp.Radio;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SDRSharp.PanView
{
	public class Meter
	{
		private const double D2R = 0.017453292519943295;

		private Pen _redPen = new Pen(Color.FromArgb(255, 120, 120));

		private Pen _whitePen = new Pen(Color.FromArgb(220, 220, 220));

		private Pen _meterPen = new Pen(Color.Yellow, 2f);

		private Brush _labelBrush = new SolidBrush(Color.FromArgb(220, 220, 220));

		private Brush _valueBrush = new SolidBrush(Color.White);

		private Brush _overflowBrush = new SolidBrush(Color.FromArgb(255, 120, 120));

		private Font _font = new Font("Aerial", 8f);

		private int _dA = 100;

		private int _mindB = -127;

		private int _maxdB = -3;

		private float _x0;

		private float _y0;

		private float _rad;

		private int _meterTmo;

		private int _labelTmo;

		private float _angle = -180f;

		private int _sample;

		private DetectorType _detectorType;

		public DetectorType DetectorType
		{
			get
			{
				return this._detectorType;
			}
			set
			{
				this._detectorType = value;
			}
		}

		public void Position(int posX, int posY, int sizeX, int sizeY)
		{
			this._rad = (float)(((sizeX > sizeY) ? sizeX : sizeY) / 2);
			this._x0 = (float)posX + this._rad;
			this._y0 = (float)posY + this._rad;
		}

		public void DrawBackground(Graphics graphics)
		{
			int num = -90 - this._dA / 2;
			int num2 = -90 + this._dA / 2;
			float num3 = (-73f - (float)this._mindB) / (float)(this._maxdB - this._mindB);
			num3 = (float)num + num3 * (float)this._dA;
			bool flag = false;
			bool flag2 = false;
			string text = "";
			Font font = new Font("Aerial", 6f);
			float height = graphics.MeasureString("+60", font).Height;
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			InterpolationMode interpolationMode = graphics.InterpolationMode;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			this._whitePen.Width = 4f;
			graphics.DrawArc(this._whitePen, this._x0 - this._rad, this._y0 - this._rad, this._rad * 2f, this._rad * 2f, (float)num, num3 - (float)num);
			this._redPen.Width = 4f;
			graphics.DrawArc(this._redPen, this._x0 - this._rad, this._y0 - this._rad, this._rad * 2f, this._rad * 2f, num3, (float)num2 - num3);
			for (int i = this._mindB; i <= this._maxdB; i++)
			{
				if (i <= -73)
				{
					if ((i + 121) % 6 == 0)
					{
						flag = true;
					}
					if ((i + 121) % 12 == 0)
					{
						flag2 = true;
					}
					if (flag2)
					{
						text = $"{(i + 127) / 6:0}";
					}
				}
				else
				{
					if ((i + 73) % 10 == 0)
					{
						flag = true;
					}
					if ((i + 73) % 20 == 0)
					{
						flag2 = true;
					}
					if (flag2)
					{
						text = $"+{i + 73:00}";
					}
				}
				if (flag)
				{
					flag = false;
					num3 = (float)(i - this._mindB) / (float)(this._maxdB - this._mindB);
					num3 = (float)num + num3 * (float)this._dA;
					float num4 = (float)((double)(this._rad * 1.02f) * Math.Cos((double)num3 * 0.017453292519943295));
					float num5 = (float)((double)(this._rad * 1.02f) * Math.Sin((double)num3 * 0.017453292519943295));
					float num6 = flag2 ? 1.15f : 1.1f;
					this._whitePen.Width = 1f;
					this._redPen.Width = 1f;
					graphics.DrawLine((i <= -73) ? this._whitePen : this._redPen, this._x0 + num4, this._y0 + num5, this._x0 + num4 * num6, this._y0 + num5 * num6);
					if (flag2)
					{
						flag2 = false;
						num4 = this._x0 + num4 * 1.28f;
						num5 = this._y0 + num5 * 1.28f;
						float num7 = graphics.MeasureString(text, font).Width + 2f;
						graphics.DrawString(text, this._font, this._labelBrush, num4 - num7 / 2f, num5 - height / 2f);
					}
				}
			}
			graphics.SmoothingMode = smoothingMode;
			graphics.InterpolationMode = interpolationMode;
		}

		public void Draw(Graphics graphics, float dB, int showDbm, bool overFlow)
		{
			float num = (dB - (float)this._mindB) / (float)(this._maxdB - this._mindB);
			num = (float)(-90 - this._dA / 2) + num * (float)this._dA;
			int num2 = 2;
			if (this._detectorType != DetectorType.AM && this._detectorType != DetectorType.SAM && this._detectorType != DetectorType.WFM && this._detectorType != 0)
			{
				if (num > this._angle)
				{
					this._angle = num;
					this._meterTmo = 20;
					this._sample = (int)dB;
				}
				else if (this._meterTmo > 0)
				{
					this._meterTmo--;
				}
				else if (this._angle > -180f)
				{
					this._angle -= 1f;
				}
			}
			else
			{
				this._angle = ((float)(num2 - 1) * this._angle + num) / (float)num2;
				if (--this._labelTmo <= 0)
				{
					this._labelTmo = 10;
					this._sample = (int)dB;
				}
			}
			float num3 = (float)((double)this._rad * 1.1 * Math.Cos((double)this._angle * 0.017453292519943295));
			float num4 = (float)((double)this._rad * 1.1 * Math.Sin((double)this._angle * 0.017453292519943295));
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			InterpolationMode interpolationMode = graphics.InterpolationMode;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.DrawLine(this._meterPen, this._x0 + num3, this._y0 + num4, this._x0 + num3 / 3f, this._y0 + num4 / 3f);
			graphics.SmoothingMode = smoothingMode;
			graphics.InterpolationMode = interpolationMode;
			string str = overFlow ? "> " : "";
			str = ((showDbm < 2) ? (str + Utils.Signal(this._sample, showDbm, false) + "dB") : (str + Utils.Signal(this._sample, showDbm, true)));
			float width = graphics.MeasureString(str, this._font).Width;
			graphics.DrawString(str, this._font, overFlow ? this._overflowBrush : this._valueBrush, this._x0 + this._rad - width, this._y0 - 0.5f * this._rad);
		}
	}
}
