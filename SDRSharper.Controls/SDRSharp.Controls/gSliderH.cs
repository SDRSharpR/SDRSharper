using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	[DefaultEvent("ValueChanged")]
	public class gSliderH : UserControl
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

		private gButton gButton;

		private gButton gThumb;

		private Panel panel;

		private BorderGradientPanel gradientPanel;

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

		public gSliderH()
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
			this.gThumb.Height = base.Height;
			this.gButton.Height = base.Height;
			this.gButton.Width = base.Height;
			this.gButton.Visible = this._button;
			this.gradientPanel.Left = (this._button ? (this.gButton.Width + 2) : 0);
			this.gradientPanel.Width = base.Width - this.gradientPanel.Left - 1;
			this.gradientPanel.Height = base.Height;
			this.panel.Left = this.gradientPanel.Left + 10;
			this.panel.Width = this.gradientPanel.Width - 20;
			this.panel.Top = (base.Height - this.panel.Height) / 2;
			base.OnResize(e);
		}

		private void hThumb_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._oldPos = this.gThumb.Left + this.gThumb.Width / 2;
				this._oldCur = Control.MousePosition.X;
			}
		}

		private void hThumb_MouseMove(object sender, MouseEventArgs e)
		{
			if (this._oldCur != 0 && e.Button == MouseButtons.Left)
			{
				this.SetThumb(this._oldPos + Control.MousePosition.X - this._oldCur);
			}
		}

		private void hThumb_MouseUp(object sender, MouseEventArgs e)
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
				this._factor = (float)(this._val - this._min) / (float)(this._max - this._min);
				int num = (int)((float)this.panel.Left + this._factor * (float)this.panel.Width);
				this.gThumb.Left = num - this.gThumb.Width / 2;
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this, new EventArgs());
				}
			}
		}

		private void SetThumb(int newPos)
		{
			newPos = Math.Max(this.panel.Left, newPos);
			newPos = Math.Min(this.panel.Left + this.panel.Width, newPos);
			this.gThumb.Left = newPos - this.gThumb.Width / 2;
			this._factor = (float)(newPos - this.panel.Left) / (float)this.panel.Width;
			this._val = (int)((float)this._min + (float)(this._max - this._min) * this._factor);
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}

		private void gradientPanel_MouseDown(object sender, MouseEventArgs e)
		{
			this.SetThumb(this.gradientPanel.Left + e.X);
		}

		private void panel_MouseDown(object sender, MouseEventArgs e)
		{
			this.SetThumb(this.panel.Left + e.X);
		}

		private void gButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.ValueChanged != null)
			{
				this.CheckedChanged(this, new EventArgs());
			}
		}

		private void gThumb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && e.KeyCode != Keys.Space)
			{
				return;
			}
			e.IsInputKey = true;
		}

		private void gThumb_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left)
			{
				this.Value = this._val - 1;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Right)
			{
				this.Value = this._val + 1;
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
				int num = this.gradientPanel.Height / 4;
				using (Pen pen = new Pen(this._tickColor, 1f))
				{
					for (int i = 0; i <= this._ticks; i++)
					{
						int num2 = this.panel.Left - this.gradientPanel.Left + (int)((float)(this.panel.Width - 2) / (float)this._ticks * (float)i) + 1;
						e.Graphics.DrawLine(pen, num2, num, num2, this.gradientPanel.Height - num - 1);
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
			this.panel.Location = new Point(38, 14);
			this.panel.Name = "panel";
			this.panel.Size = new Size(263, 4);
			this.panel.TabIndex = 30;
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
			this.gButton.Size = new Size(32, 32);
			this.gButton.StartColor = Color.Black;
			this.gButton.StartFactor = 0.35f;
			this.gButton.TabIndex = 0;
			this.gThumb.Anchor = AnchorStyles.None;
			this.gThumb.Arrow = 101;
			this.gThumb.BackColor = SystemColors.Control;
			this.gThumb.Checked = false;
			this.gThumb.Edge = 0.15f;
			this.gThumb.EndColor = Color.White;
			this.gThumb.EndFactor = 0.2f;
			this.gThumb.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.gThumb.ForeColor = Color.Black;
			this.gThumb.Location = new Point(156, 0);
			this.gThumb.Name = "gThumb";
			this.gThumb.NoBorder = false;
			this.gThumb.NoLed = true;
			this.gThumb.RadioButton = false;
			this.gThumb.Radius = 1;
			this.gThumb.RadiusB = 1;
			this.gThumb.Size = new Size(11, 34);
			this.gThumb.StartColor = Color.DarkGray;
			this.gThumb.StartFactor = 0.35f;
			this.gThumb.TabIndex = 1;
			this.gThumb.KeyDown += this.gThumb_KeyDown;
			this.gThumb.MouseDown += this.hThumb_MouseDown;
			this.gThumb.MouseMove += this.hThumb_MouseMove;
			this.gThumb.MouseUp += this.hThumb_MouseUp;
			this.gThumb.PreviewKeyDown += this.gThumb_PreviewKeyDown;
			this.gradientPanel.BackColor = Color.Black;
			this.gradientPanel.Edge = 0.18f;
			this.gradientPanel.EndColor = Color.Black;
			this.gradientPanel.EndFactor = 0.5f;
			this.gradientPanel.Location = new Point(34, 0);
			this.gradientPanel.Margin = new Padding(2);
			this.gradientPanel.Name = "gradientPanel";
			this.gradientPanel.NoBorder = true;
			this.gradientPanel.Radius = 4;
			this.gradientPanel.RadiusB = 6;
			this.gradientPanel.Size = new Size(282, 34);
			this.gradientPanel.StartColor = Color.Gray;
			this.gradientPanel.StartFactor = 0.5f;
			this.gradientPanel.TabIndex = 29;
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
			base.Name = "gSliderH";
			base.Size = new Size(330, 46);
			base.ResumeLayout(false);
		}
	}
}
