using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	[DefaultEvent("ValueChanged")]
	public class gSliderV : UserControl
	{
		private int _oldPos;

		private int _oldCur;

		private float _factor;

		private int _min;

		private int _max = 100;

		private int _val = -1;

		private bool _button = true;

		private int _ticks;

		private Color _tickColor = Color.Orange;

		public ToolTip _toolTip;

		private IContainer components;

		private BorderGradientPanel gradientPanel;

		private Panel panel;

		private gButton gThumb;

		private gButton gButton;

		public int Minimum
		{
			get
			{
				return this._min;
			}
			set
			{
				this._min = value;
			}
		}

		public int Maximum
		{
			get
			{
				return this._max;
			}
			set
			{
				this._max = value;
			}
		}

		public int Value
		{
			get
			{
				return this._val;
			}
			set
			{
				this.SetValue(value);
			}
		}

		public bool Button
		{
			get
			{
				return this._button;
			}
			set
			{
				this._button = value;
			}
		}

		public bool Checked
		{
			get
			{
				return this.gButton.Checked;
			}
			set
			{
				this.gButton.Checked = value;
			}
		}

		public int Ticks
		{
			get
			{
				return this._ticks + 1;
			}
			set
			{
				this._ticks = value - 1;
			}
		}

		public Color TickColor
		{
			get
			{
				return this._tickColor;
			}
			set
			{
				this._tickColor = value;
			}
		}

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

		public float ColorFactor
		{
			get
			{
				return this.gradientPanel.EndFactor;
			}
			set
			{
				this.gradientPanel.EndFactor = value;
				this.gradientPanel.StartFactor = value;
			}
		}

		public event EventHandler ValueChanged;

		public event EventHandler CheckedChanged;

		public gSliderV()
		{
			this.InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (this._toolTip != null)
			{
				string toolTip = this._toolTip.GetToolTip(this);
				this._toolTip.SetToolTip(this.gThumb, toolTip);
			}
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.gThumb.ForeColor = this.ForeColor;
		}

		protected override void OnResize(EventArgs e)
		{
			this.gThumb.Width = base.Width;
			this.gButton.Width = base.Width;
			this.gButton.Height = base.Width;
			this.gButton.Visible = this._button;
			this.gradientPanel.Top = (this._button ? (this.gButton.Height + 2) : 0);
			this.gradientPanel.Height = base.Height - this.gradientPanel.Top - 1;
			this.gradientPanel.Width = base.Width;
			this.panel.Top = this.gradientPanel.Top + 10;
			this.panel.Height = this.gradientPanel.Height - 20;
			this.panel.Left = (base.Width - this.panel.Width) / 2;
			base.OnResize(e);
		}

		private void gThumb_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._oldPos = this.gThumb.Top + this.gThumb.Height / 2;
				this._oldCur = Control.MousePosition.Y;
			}
		}

		private void gThumb_MouseMove(object sender, MouseEventArgs e)
		{
			if (this._oldCur != 0 && e.Button == MouseButtons.Left)
			{
				this.SetThumb(this._oldPos + Control.MousePosition.Y - this._oldCur);
			}
		}

		private void gThumb_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._oldCur = 0;
			}
		}

		private void SetValue(int value)
		{
			if (this._val != value)
			{
				this._val = Math.Min(this._max, value);
				this._val = Math.Max(this._min, this._val);
				this._factor = (float)(this._max - this._val) / (float)(this._max - this._min);
				int num = (int)((float)this.panel.Top + this._factor * (float)this.panel.Height);
				this.gThumb.Top = num - this.gThumb.Height / 2;
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this, new EventArgs());
				}
			}
		}

		private void SetThumb(int newPos)
		{
			newPos = Math.Max(this.panel.Top, newPos);
			newPos = Math.Min(this.panel.Top + this.panel.Height, newPos);
			this.gThumb.Top = newPos - this.gThumb.Height / 2;
			this._factor = (float)(newPos - this.panel.Top) / (float)this.panel.Height;
			this._val = (int)((float)this._max - (float)(this._max - this._min) * this._factor);
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}

		private void gradientPanel_MouseDown(object sender, MouseEventArgs e)
		{
			this.gThumb.Focus();
			this.SetThumb(this.gradientPanel.Top + e.Y);
		}

		private void panel_MouseDown(object sender, MouseEventArgs e)
		{
			this.gThumb.Focus();
			this.SetThumb(this.panel.Top + e.Y);
		}

		private void gButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.CheckedChanged != null)
			{
				this.CheckedChanged(this, new EventArgs());
			}
		}

		private void gThumb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Space)
			{
				return;
			}
			e.IsInputKey = true;
		}

		private void gThumb_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
			{
				this.Value = this._val + 1;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Down)
			{
				this.Value = this._val - 1;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Space)
			{
				this.gButton.Checked = !this.gButton.Checked;
				e.Handled = true;
			}
		}

		private void gradientPanel_Paint(object sender, PaintEventArgs e)
		{
			if (this._ticks > 0)
			{
				int num = this.gradientPanel.Width / 4;
				using (Pen pen = new Pen(this._tickColor, 1f))
				{
					for (int i = 0; i <= this._ticks; i++)
					{
						int num2 = this.panel.Top - this.gradientPanel.Top + (int)((float)(this.panel.Height - 2) / (float)this._ticks * (float)i) + 1;
						e.Graphics.DrawLine(pen, num, num2, this.gradientPanel.Width - num, num2);
					}
				}
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
			this.panel = new Panel();
			this.gButton = new gButton();
			this.gThumb = new gButton();
			this.gradientPanel = new BorderGradientPanel();
			base.SuspendLayout();
			this.panel.BackColor = Color.Black;
			this.panel.Location = new Point(16, 41);
			this.panel.Name = "panel";
			this.panel.Size = new Size(4, 285);
			this.panel.TabIndex = 17;
			this.panel.MouseDown += this.panel_MouseDown;
			this.gButton.Arrow = 0;
			this.gButton.BackColor = SystemColors.Control;
			this.gButton.Checked = false;
			this.gButton.Edge = 0.15f;
			this.gButton.EndColor = Color.White;
			this.gButton.EndFactor = 0.2f;
			this.gButton.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.gButton.Location = new Point(0, 0);
			this.gButton.Name = "gButton";
			this.gButton.NoBorder = false;
			this.gButton.NoLed = false;
			this.gButton.RadioButton = false;
			this.gButton.Radius = 4;
			this.gButton.RadiusB = 6;
			this.gButton.Size = new Size(36, 32);
			this.gButton.StartColor = Color.Black;
			this.gButton.StartFactor = 0.35f;
			this.gButton.TabIndex = 0;
			this.gButton.CheckedChanged += this.gButton_CheckedChanged;
			this.gThumb.Arrow = 100;
			this.gThumb.BackColor = SystemColors.Control;
			this.gThumb.Checked = false;
			this.gThumb.Edge = 0.15f;
			this.gThumb.EndColor = Color.White;
			this.gThumb.EndFactor = 0.2f;
			this.gThumb.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.gThumb.Location = new Point(0, 169);
			this.gThumb.Name = "gThumb";
			this.gThumb.NoBorder = false;
			this.gThumb.NoLed = true;
			this.gThumb.RadioButton = false;
			this.gThumb.Radius = 1;
			this.gThumb.RadiusB = 0;
			this.gThumb.Size = new Size(36, 10);
			this.gThumb.StartColor = Color.DarkGray;
			this.gThumb.StartFactor = 0.35f;
			this.gThumb.TabIndex = 1;
			this.gThumb.KeyDown += this.gThumb_KeyDown;
			this.gThumb.MouseDown += this.gThumb_MouseDown;
			this.gThumb.MouseMove += this.gThumb_MouseMove;
			this.gThumb.MouseUp += this.gThumb_MouseUp;
			this.gThumb.PreviewKeyDown += this.gThumb_PreviewKeyDown;
			this.gradientPanel.BackColor = Color.Black;
			this.gradientPanel.Edge = 0.08f;
			this.gradientPanel.EndColor = Color.Black;
			this.gradientPanel.EndFactor = 0.55f;
			this.gradientPanel.ForeColor = Color.Black;
			this.gradientPanel.Location = new Point(0, 37);
			this.gradientPanel.Margin = new Padding(2);
			this.gradientPanel.Name = "gradientPanel";
			this.gradientPanel.NoBorder = false;
			this.gradientPanel.Radius = 2;
			this.gradientPanel.RadiusB = 4;
			this.gradientPanel.Size = new Size(36, 308);
			this.gradientPanel.StartColor = Color.Gray;
			this.gradientPanel.StartFactor = 0.55f;
			this.gradientPanel.TabIndex = 16;
			this.gradientPanel.TabStop = false;
			this.gradientPanel.Text = "borderGradientPanel3";
			this.gradientPanel.Paint += this.gradientPanel_Paint;
			this.gradientPanel.MouseDown += this.gradientPanel_MouseDown;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.gButton);
			base.Controls.Add(this.gThumb);
			base.Controls.Add(this.panel);
			base.Controls.Add(this.gradientPanel);
			base.Name = "gSliderV";
			base.Size = new Size(49, 357);
			base.ResumeLayout(false);
		}
	}
}
