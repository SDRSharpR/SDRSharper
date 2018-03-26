using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	[DefaultEvent("CheckedChanged")]
	public class gButton : BorderGradientPanel
	{
		private int _arrow;

		private bool _noLed;

		private bool _radioButton;

		private bool _checked;

		private bool _keyDown = true;

		private bool _init;

		private bool _down;

		private Color _startColor;

		private Color _endColor;

		private float _startFactor;

		private float _endFactor;

		[Description("1=up, 2=right, 3=down, 4=left, 5=up+down, 100=V_thumb, 101=H_thumb, 98=yellow text 99=side led")]
		public int Arrow
		{
			get
			{
				return this._arrow;
			}
			set
			{
				this._arrow = value;
			}
		}

		public bool NoLed
		{
			get
			{
				return this._noLed;
			}
			set
			{
				this._noLed = value;
			}
		}

		public bool NoKeyDown
		{
			set
			{
				this._keyDown = !value;
			}
		}

		public bool Checked
		{
			get
			{
				return this._checked;
			}
			set
			{
				this.SetChecked(value);
			}
		}

		public bool RadioButton
		{
			get
			{
				return this._radioButton;
			}
			set
			{
				this._radioButton = value;
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				base.Invalidate();
			}
		}

		public event EventHandler CheckedChanged;

		public void SetColor(Color color)
		{
			this._endColor = color;
			this.UpdateAppearance();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				this._down = true;
				this.UpdateAppearance();
				base.Focus();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Button == MouseButtons.Left)
			{
				this._down = false;
				if (!this._radioButton)
				{
					this.SetChecked(!this._checked);
				}
				else
				{
					this.SetChecked(true);
				}
				this.UpdateAppearance();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Space && this._keyDown)
			{
				this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
			}
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.KeyCode == Keys.Space && this._keyDown)
			{
				this.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
			}
		}

		private void UpdateAppearance()
		{
			if (!this._init)
			{
				this._init = true;
				this._startColor = base.StartColor;
				this._endColor = base.EndColor;
				this._startFactor = base.StartFactor;
				this._endFactor = base.EndFactor;
			}
			if (this._down)
			{
				base.StartColor = this._endColor;
				base.EndColor = this._startColor;
				base.StartFactor = 1f - this._endFactor;
				base.EndFactor = (1f - this._startFactor) * 1.1f;
			}
			else
			{
				base.StartColor = this._startColor;
				base.EndColor = this._endColor;
				base.StartFactor = this._startFactor;
				base.EndFactor = this._endFactor;
			}
			base.Invalidate();
		}

		private void SetChecked(bool value)
		{
			if (this._checked != value)
			{
				if (this._radioButton)
				{
					foreach (Control control in base.Parent.Controls)
					{
						if (control.GetType() == typeof(gButton))
						{
							gButton gButton = (gButton)control;
							if (gButton.Name != base.Name && gButton.RadioButton && gButton.Checked)
							{
								gButton.Checked = false;
							}
						}
					}
				}
				this._checked = value;
				base.Invalidate();
				if (this.CheckedChanged != null)
				{
					this.CheckedChanged(this, new EventArgs());
				}
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			if (this._noLed)
			{
				int num = base.Height / 2;
				int num2 = base.Width / 2;
				int num3 = Math.Min(num2 / 2, num / 2);
				if (this._arrow == 1)
				{
					Point[] points = new Point[4]
					{
						new Point(num2 - num3, num + num3),
						new Point(num2 + num3 + 1, num + num3),
						new Point(num2, num - num3 - 1),
						new Point(num2 - num3, num + num3)
					};
					pe.Graphics.FillPolygon(base.Enabled ? Brushes.RoyalBlue : Brushes.Gray, points);
				}
				else if (this._arrow == 2)
				{
					Point[] points2 = new Point[4]
					{
						new Point(num2 - num3, num - num3),
						new Point(num2 + num3, num),
						new Point(num2 - num3, num + num3),
						new Point(num2 - num3, num - num3)
					};
					pe.Graphics.FillPolygon(base.Enabled ? Brushes.RoyalBlue : Brushes.Gray, points2);
				}
				else if (this._arrow == 3)
				{
					Point[] points3 = new Point[4]
					{
						new Point(num2 - num3, num - num3 + 1),
						new Point(num2 + num3, num - num3 + 1),
						new Point(num2, num + num3 + 1),
						new Point(num2 - num3, num - num3 + 1)
					};
					pe.Graphics.FillPolygon(base.Enabled ? Brushes.RoyalBlue : Brushes.Gray, points3);
				}
				else if (this._arrow == 4)
				{
					new Point(num2 - num3, num);
					new Point(num2 + num3, num - num3);
					new Point(num2 + num3, num + num3);
					new Point(num2 - num3, num);
				}
				else if (this._arrow != 5)
				{
					if (this._arrow == 100)
					{
						using (Brush brush = new SolidBrush(this.ForeColor))
						{
							pe.Graphics.FillRectangle(brush, 1, num - 2, base.ClientRectangle.Width - 1, 4);
						}
					}
					else if (this._arrow == 101)
					{
						using (Brush brush2 = new SolidBrush(this.ForeColor))
						{
							pe.Graphics.FillRectangle(brush2, num2 - 2, 0, 4, base.ClientRectangle.Height);
						}
					}
				}
			}
			else
			{
				int num4 = 12;
				if (this._arrow == 99)
				{
					float x = (float)base.Width * 0.14f;
					float y = (float)((base.Height - num4) / 2);
					pe.Graphics.FillRectangle(this._checked ? Brushes.Lime : Brushes.DimGray, x, y, (float)(num4 / 2), (float)num4);
				}
				else
				{
					float num5 = (float)((base.Width - num4) / 2);
					float y2 = (this.Text.Length == 0) ? ((float)((base.Height - num4 / 2) / 2)) : ((float)base.Height * 0.18f);
					pe.Graphics.FillRectangle(this._checked ? Brushes.Lime : Brushes.DimGray, num5 + 1f, y2, (float)num4, (float)(num4 / 2));
				}
			}
			if (this.Text.Length != 0)
			{
				string[] array = this.Text.Split('\\');
				float width;
				float num6;
				float num7;
				if (array.GetUpperBound(0) == 0)
				{
					if (this.Text == "...")
					{
						num6 = 0f;
					}
					SizeF sizeF = pe.Graphics.MeasureString(this.Text, this.Font);
					width = sizeF.Width;
					num6 = 0f;
					num7 = sizeF.Height;
				}
				else
				{
					SizeF sizeF2 = pe.Graphics.MeasureString(array[0], this.Font);
					SizeF sizeF3 = pe.Graphics.MeasureString(array[1], this.Font);
					width = sizeF2.Width;
					num6 = sizeF3.Width;
					num7 = sizeF2.Height + sizeF3.Height;
				}
				float x2 = ((float)base.Width - width) / 2f;
				float num8 = (float)base.Height * 0.68f;
				if (this._noLed && this._arrow == 99)
				{
					this._arrow = 0;
				}
				if (this._noLed || this._arrow == 99)
				{
					num8 = (float)(base.Height / 2 + 1);
				}
				Brush brush3 = new SolidBrush(base.Enabled ? this.ForeColor : Color.Gray);
				if (array.GetUpperBound(0) > 0)
				{
					if (this._arrow == 99)
					{
						x2 = 0.14f * (float)base.Width + 2f + (0.86f * (float)base.Width - width) / 2f;
					}
					pe.Graphics.DrawString(array[0], this.Font, brush3, x2, num8 - num7 / 2f + 2f);
					if (this._arrow == 99)
					{
						x2 = 0.14f * (float)base.Width + 2f + (0.86f * (float)base.Width - num6) / 2f;
					}
					pe.Graphics.DrawString(array[1], this.Font, brush3, x2, num8 - 1f);
				}
				else
				{
					if (this._arrow == 99)
					{
						x2 = 0.14f * (float)base.Width + 2f + (0.86f * (float)base.Width - width) / 2f;
					}
					pe.Graphics.DrawString(this.Text, this.Font, brush3, x2, num8 - num7 / 2f);
				}
				brush3.Dispose();
			}
		}
	}
}
