using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	[DefaultEvent("ValueChanged")]
	public class gUpDown : UserControl
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		private long _min = -32767L;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		private long _max = 32767L;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		private long _value;

		private int _step = 1;

		private ToolTip _toolTip;

		private IContainer components;

		private gButton gBdown;

		private TextBox textBox;

		private BorderGradientPanel gradientPanel;

		private gButton gBup;

		public long Minimum
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

		public long Maximum
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

		public long Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this.SetValue(value);
			}
		}

		public int Increment
		{
			get
			{
				return this._step;
			}
			set
			{
				this._step = value;
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

		public event EventHandler ValueChanged;

		public gUpDown()
		{
			this.InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (this._toolTip != null)
			{
				this._toolTip.SetToolTip(this.textBox, this._toolTip.GetToolTip(this));
			}
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.gBup.Enabled = base.Enabled;
			this.gBdown.Enabled = base.Enabled;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.gBdown.Height = base.Height - 1;
			this.gBdown.Width = this.gBdown.Height - 4;
			this.gBdown.Left = base.Width - this.gBdown.Width;
			this.gBup.Height = this.gBdown.Height;
			this.gBup.Width = this.gBdown.Width;
			this.gBup.Left = this.gBdown.Left - this.gBup.Width - 1;
			this.gradientPanel.Height = base.Height;
			this.gradientPanel.Width = this.gBup.Left - 1;
			this.textBox.Left = 5;
			this.textBox.Width = this.gradientPanel.Width - this.textBox.Left - 5;
			this.textBox.Top = (base.Height - this.textBox.Height) / 2 + 1;
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.textBox.Font = this.Font;
			base.Invalidate();
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.textBox.ForeColor = this.ForeColor;
			base.Invalidate();
		}

		private void SetValue(long value)
		{
			if (this._value != value)
			{
				this._value = Math.Min(this._max, value);
				this._value = Math.Max(this._min, this._value);
				this.textBox.Text = this._value.ToString();
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this, new EventArgs());
				}
			}
		}

		private void gBup_MouseDown(object sender, MouseEventArgs e)
		{
			this.SetValue(this._value + this._step);
		}

		private void gBdown_MouseDown(object sender, MouseEventArgs e)
		{
			this.SetValue(this._value - this._step);
		}

		private void gB_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down)
			{
				return;
			}
			e.IsInputKey = true;
		}

		private void gB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
			{
				this.SetValue(this._value + this._step);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Down)
			{
				this.SetValue(this._value - this._step);
				e.Handled = true;
			}
		}

		private void textBox_Validating(object sender, CancelEventArgs e)
		{
			if (long.TryParse(this.textBox.Text, out long value))
			{
				this._value = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this, new EventArgs());
				}
			}
		}

		private void gBdown_MouseUp(object sender, MouseEventArgs e)
		{
			this.gBdown.Checked = false;
		}

		private void gBup_MouseUp(object sender, MouseEventArgs e)
		{
			this.gBup.Checked = false;
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
			this.textBox = new TextBox();
			this.gBup = new gButton();
			this.gBdown = new gButton();
			this.gradientPanel = new BorderGradientPanel();
			base.SuspendLayout();
			this.textBox.BackColor = Color.FromArgb(50, 50, 50);
			this.textBox.BorderStyle = BorderStyle.None;
			this.textBox.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.textBox.ForeColor = Color.Yellow;
			this.textBox.Location = new Point(11, 1);
			this.textBox.Name = "textBox";
			this.textBox.Size = new Size(85, 13);
			this.textBox.TabIndex = 28;
			this.textBox.Text = "0";
			this.textBox.KeyDown += this.gB_KeyDown;
			this.textBox.Validating += this.textBox_Validating;
			this.gBup.Arrow = 1;
			this.gBup.BackColor = SystemColors.Control;
			this.gBup.Checked = false;
			this.gBup.Edge = 0.15f;
			this.gBup.EndColor = Color.White;
			this.gBup.EndFactor = 0.2f;
			this.gBup.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.gBup.Location = new Point(113, 1);
			this.gBup.Name = "gBup";
			this.gBup.NoBorder = false;
			this.gBup.NoLed = true;
			this.gBup.RadioButton = false;
			this.gBup.Radius = 4;
			this.gBup.RadiusB = 6;
			this.gBup.Size = new Size(18, 23);
			this.gBup.StartColor = Color.Black;
			this.gBup.StartFactor = 0.35f;
			this.gBup.TabIndex = 30;
			this.gBup.KeyDown += this.gB_KeyDown;
			this.gBup.MouseDown += this.gBup_MouseDown;
			this.gBup.PreviewKeyDown += this.gB_PreviewKeyDown;
			this.gBdown.Arrow = 3;
			this.gBdown.BackColor = SystemColors.Control;
			this.gBdown.Checked = false;
			this.gBdown.Edge = 0.15f;
			this.gBdown.EndColor = Color.White;
			this.gBdown.EndFactor = 0.2f;
			this.gBdown.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.gBdown.Location = new Point(133, 1);
			this.gBdown.Name = "gBdown";
			this.gBdown.NoBorder = false;
			this.gBdown.NoLed = true;
			this.gBdown.RadioButton = false;
			this.gBdown.Radius = 4;
			this.gBdown.RadiusB = 6;
			this.gBdown.Size = new Size(18, 23);
			this.gBdown.StartColor = Color.Black;
			this.gBdown.StartFactor = 0.35f;
			this.gBdown.TabIndex = 29;
			this.gBdown.KeyDown += this.gB_KeyDown;
			this.gBdown.MouseDown += this.gBdown_MouseDown;
			this.gBdown.PreviewKeyDown += this.gB_PreviewKeyDown;
			this.gradientPanel.BackColor = Color.Black;
			this.gradientPanel.Edge = 0.18f;
			this.gradientPanel.EndColor = Color.Black;
			this.gradientPanel.EndFactor = 0.6f;
			this.gradientPanel.Location = new Point(0, 0);
			this.gradientPanel.Margin = new Padding(2);
			this.gradientPanel.Name = "gradientPanel";
			this.gradientPanel.NoBorder = true;
			this.gradientPanel.Radius = 6;
			this.gradientPanel.RadiusB = 0;
			this.gradientPanel.Size = new Size(112, 25);
			this.gradientPanel.StartColor = Color.Gray;
			this.gradientPanel.StartFactor = 0.6f;
			this.gradientPanel.TabIndex = 27;
			this.gradientPanel.TabStop = false;
			this.gradientPanel.Text = "borderGradientPanel3";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.gBup);
			base.Controls.Add(this.gBdown);
			base.Controls.Add(this.textBox);
			base.Controls.Add(this.gradientPanel);
			base.Name = "gUpDown";
			base.Size = new Size(153, 32);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
