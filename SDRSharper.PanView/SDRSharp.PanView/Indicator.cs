using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	public class Indicator : UserControl
	{
		private decimal _value = new decimal(8888888888L);

		private string _newText;

		private string _oldText;

		private decimal _min;

		private decimal _max;

		private decimal _inc;

		private bool _init;

		private Color _backColor;

		private ToolTip _toolTip;

		private IContainer components;

		private Label txt8;

		private Label txt7;

		private Label txt6;

		private Label txt3;

		private Label txt4;

		private Label txt5;

		private Label txt0;

		private Label txt1;

		private Label txt2;

		private Label txt10;

		private Label txt9;

		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (!(value == this._value))
				{
					this._oldText = this._value.ToString().PadLeft(10);
					this._value = value;
					this._newText = this._value.ToString().PadLeft(10);
					for (int i = 0; i <= 9; i++)
					{
						if (this._newText.Substring(i, 1) != this._oldText.Substring(i, 1))
						{
							this.GetDigit(9 - i).Text = this._newText.Substring(i, 1);
						}
					}
					if (this._init)
					{
						this.ValueChanged(this, new EventArgs());
					}
				}
			}
		}

		public string Label
		{
			get
			{
				return this.txt10.Text;
			}
			set
			{
				this.txt10.Text = value;
			}
		}

		public decimal Minimum
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

		public decimal Maximum
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

		public decimal Increment
		{
			get
			{
				return this._inc;
			}
			set
			{
				this._inc = value;
				if (this._inc > 0m)
				{
					this._init = true;
				}
			}
		}

		public event ManualValueChange ValueChanged;

		[DllImport("user32.dll")]
		public static extern long HideCaret(IntPtr hwnd);

		public Indicator()
		{
			this.InitializeComponent();
			this._backColor = this.txt0.BackColor;
		}

		public void ShowValue(int value)
		{
			this._newText = value.ToString().PadLeft(10);
			for (int i = 0; i <= 9; i++)
			{
				this.GetDigit(9 - i).Text = this._newText.Substring(i, 1);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (this._toolTip != null)
			{
				string toolTip = this._toolTip.GetToolTip(this);
				for (int i = 0; i <= 9; i++)
				{
					this._toolTip.SetToolTip(this.GetDigit(i), toolTip);
				}
			}
		}

		private void txt_MouseEnter(object sender, EventArgs e)
		{
			((Label)sender).Focus();
		}

		private void txt_Enter(object sender, EventArgs e)
		{
			((Label)sender).BackColor = Color.RoyalBlue;
		}

		private void txt_Leave(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			label.BackColor = this._backColor;
		}

		private void txt_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Label label = (Label)sender;
				int pos = this.GetPos(label);
				long value = (pos == 10) ? ((long)this._inc) : ((long)Math.Pow(10.0, (double)pos));
				if (e.Location.Y < label.Height / 2 - 3)
				{
					this.Value = this._value + (decimal)value;
				}
				else
				{
					this.Value = this._value - (decimal)value;
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				FrequencyDialog frequencyDialog = new FrequencyDialog();
				Label label2 = (Label)sender;
				frequencyDialog.StartPosition = FormStartPosition.CenterParent;
				if (frequencyDialog.ShowDialog() == DialogResult.Yes)
				{
					this.Value = this.Val(frequencyDialog.Frequency) * 1000;
				}
				else
				{
					this.Value = this.Val(frequencyDialog.Frequency) * 1000000;
				}
			}
		}

		private void txt_MouseWheel(object sender, MouseEventArgs e)
		{
			int pos = this.GetPos((Label)sender);
			long value = (pos == 10) ? ((long)this._inc) : ((long)Math.Pow(10.0, (double)pos));
			if (e.Delta > 0)
			{
				this.Value = this._value + (decimal)value;
			}
			else if (e.Delta < 0)
			{
				this.Value = this._value - (decimal)value;
			}
		}

		private void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			int pos = this.GetPos((Label)sender);
			e.IsInputKey = true;
			switch (e.KeyCode)
			{
			case Keys.Left:
				if (pos < 10)
				{
					this.GetDigit(pos + 1).Focus();
				}
				break;
			case Keys.Right:
				if (pos > 0)
				{
					this.GetDigit(pos - 1).Focus();
				}
				break;
			case Keys.Down:
				this.Value = this._value - (decimal)((pos == 10) ? ((long)this._inc) : ((long)Math.Pow(10.0, (double)pos)));
				break;
			case Keys.Up:
				this.Value = this._value + (decimal)((pos == 10) ? ((long)this._inc) : ((long)Math.Pow(10.0, (double)pos)));
				break;
			default:
				e.IsInputKey = false;
				break;
			}
		}

		private string Str(int val)
		{
			return val.ToString();
		}

		private int Val(string str)
		{
			int.TryParse(str, out int result);
			return result;
		}

		private Label GetDigit(int digit)
		{
			if (digit < 0)
			{
				digit = 0;
			}
			else if (digit > 10)
			{
				digit = 10;
			}
			switch (digit)
			{
			case 0:
				return this.txt0;
			case 1:
				return this.txt1;
			case 2:
				return this.txt2;
			case 3:
				return this.txt3;
			case 4:
				return this.txt4;
			case 5:
				return this.txt5;
			case 6:
				return this.txt6;
			case 7:
				return this.txt7;
			case 8:
				return this.txt8;
			case 9:
				return this.txt9;
			default:
				return this.txt10;
			}
		}

		private int GetPos(Label txt)
		{
			return this.Val(txt.Name.Substring(3));
		}

		protected override void OnResize(EventArgs e)
		{
			base.Width = this.txt0.Left + this.txt0.Width;
			base.OnResize(e);
			base.Invalidate();
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
			this.txt8 = new Label();
			this.txt7 = new Label();
			this.txt6 = new Label();
			this.txt3 = new Label();
			this.txt4 = new Label();
			this.txt5 = new Label();
			this.txt0 = new Label();
			this.txt1 = new Label();
			this.txt2 = new Label();
			this.txt10 = new Label();
			this.txt9 = new Label();
			base.SuspendLayout();
			this.txt8.BackColor = Color.FromArgb(80, 80, 80);
			this.txt8.BorderStyle = BorderStyle.FixedSingle;
			this.txt8.Font = new Font("LCD", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt8.ForeColor = Color.Yellow;
			this.txt8.Location = new Point(77, 0);
			this.txt8.Margin = new Padding(0);
			this.txt8.Name = "txt8";
			this.txt8.Padding = new Padding(0, 2, 0, 0);
			this.txt8.Size = new Size(24, 34);
			this.txt8.TabIndex = 0;
			this.txt8.Text = "8";
			this.txt8.TextAlign = ContentAlignment.TopCenter;
			this.txt8.Enter += this.txt_Enter;
			this.txt8.Leave += this.txt_Leave;
			this.txt8.MouseDown += this.txt_MouseDown;
			this.txt8.MouseEnter += this.txt_MouseEnter;
			this.txt8.MouseWheel += this.txt_MouseWheel;
			this.txt8.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt7.BackColor = Color.FromArgb(80, 80, 80);
			this.txt7.BorderStyle = BorderStyle.FixedSingle;
			this.txt7.Font = new Font("LCD", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt7.ForeColor = Color.Yellow;
			this.txt7.Location = new Point(101, 0);
			this.txt7.Margin = new Padding(0);
			this.txt7.Name = "txt7";
			this.txt7.Padding = new Padding(0, 2, 0, 0);
			this.txt7.Size = new Size(24, 34);
			this.txt7.TabIndex = 1;
			this.txt7.Text = "8";
			this.txt7.TextAlign = ContentAlignment.TopCenter;
			this.txt7.Enter += this.txt_Enter;
			this.txt7.Leave += this.txt_Leave;
			this.txt7.MouseDown += this.txt_MouseDown;
			this.txt7.MouseEnter += this.txt_MouseEnter;
			this.txt7.MouseWheel += this.txt_MouseWheel;
			this.txt7.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt6.BackColor = Color.FromArgb(80, 80, 80);
			this.txt6.BorderStyle = BorderStyle.FixedSingle;
			this.txt6.Font = new Font("LCD", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt6.ForeColor = Color.Yellow;
			this.txt6.Location = new Point(125, 0);
			this.txt6.Margin = new Padding(0);
			this.txt6.Name = "txt6";
			this.txt6.Padding = new Padding(0, 2, 0, 0);
			this.txt6.Size = new Size(24, 34);
			this.txt6.TabIndex = 2;
			this.txt6.Text = "8";
			this.txt6.TextAlign = ContentAlignment.TopCenter;
			this.txt6.Enter += this.txt_Enter;
			this.txt6.Leave += this.txt_Leave;
			this.txt6.MouseDown += this.txt_MouseDown;
			this.txt6.MouseEnter += this.txt_MouseEnter;
			this.txt6.MouseWheel += this.txt_MouseWheel;
			this.txt6.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt3.BackColor = Color.FromArgb(80, 80, 80);
			this.txt3.BorderStyle = BorderStyle.FixedSingle;
			this.txt3.Font = new Font("LCD", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt3.ForeColor = Color.Yellow;
			this.txt3.Location = new Point(202, 0);
			this.txt3.Margin = new Padding(0);
			this.txt3.Name = "txt3";
			this.txt3.Padding = new Padding(0, 2, 0, 0);
			this.txt3.Size = new Size(24, 34);
			this.txt3.TabIndex = 5;
			this.txt3.Text = "8";
			this.txt3.TextAlign = ContentAlignment.TopCenter;
			this.txt3.Enter += this.txt_Enter;
			this.txt3.Leave += this.txt_Leave;
			this.txt3.MouseDown += this.txt_MouseDown;
			this.txt3.MouseEnter += this.txt_MouseEnter;
			this.txt3.MouseWheel += this.txt_MouseWheel;
			this.txt3.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt4.BackColor = Color.FromArgb(80, 80, 80);
			this.txt4.BorderStyle = BorderStyle.FixedSingle;
			this.txt4.Font = new Font("LCD", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt4.ForeColor = Color.Yellow;
			this.txt4.Location = new Point(178, 0);
			this.txt4.Margin = new Padding(0);
			this.txt4.Name = "txt4";
			this.txt4.Padding = new Padding(0, 2, 0, 0);
			this.txt4.Size = new Size(24, 34);
			this.txt4.TabIndex = 4;
			this.txt4.Text = "8";
			this.txt4.TextAlign = ContentAlignment.TopCenter;
			this.txt4.Enter += this.txt_Enter;
			this.txt4.Leave += this.txt_Leave;
			this.txt4.MouseDown += this.txt_MouseDown;
			this.txt4.MouseEnter += this.txt_MouseEnter;
			this.txt4.MouseWheel += this.txt_MouseWheel;
			this.txt4.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt5.BackColor = Color.FromArgb(80, 80, 80);
			this.txt5.BorderStyle = BorderStyle.FixedSingle;
			this.txt5.Font = new Font("LCD", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt5.ForeColor = Color.Yellow;
			this.txt5.Location = new Point(154, 0);
			this.txt5.Margin = new Padding(0);
			this.txt5.Name = "txt5";
			this.txt5.Padding = new Padding(0, 2, 0, 0);
			this.txt5.Size = new Size(24, 34);
			this.txt5.TabIndex = 3;
			this.txt5.Text = "8";
			this.txt5.TextAlign = ContentAlignment.TopCenter;
			this.txt5.Enter += this.txt_Enter;
			this.txt5.Leave += this.txt_Leave;
			this.txt5.MouseDown += this.txt_MouseDown;
			this.txt5.MouseEnter += this.txt_MouseEnter;
			this.txt5.MouseWheel += this.txt_MouseWheel;
			this.txt5.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt0.BackColor = Color.FromArgb(80, 80, 80);
			this.txt0.BorderStyle = BorderStyle.FixedSingle;
			this.txt0.Font = new Font("LCD", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt0.ForeColor = Color.Yellow;
			this.txt0.Location = new Point(271, 0);
			this.txt0.Margin = new Padding(0);
			this.txt0.Name = "txt0";
			this.txt0.Padding = new Padding(0, 2, 0, 0);
			this.txt0.Size = new Size(22, 30);
			this.txt0.TabIndex = 8;
			this.txt0.Text = "8";
			this.txt0.TextAlign = ContentAlignment.TopCenter;
			this.txt0.Enter += this.txt_Enter;
			this.txt0.Leave += this.txt_Leave;
			this.txt0.MouseDown += this.txt_MouseDown;
			this.txt0.MouseEnter += this.txt_MouseEnter;
			this.txt0.MouseWheel += this.txt_MouseWheel;
			this.txt0.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt1.BackColor = Color.FromArgb(80, 80, 80);
			this.txt1.BorderStyle = BorderStyle.FixedSingle;
			this.txt1.Font = new Font("LCD", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt1.ForeColor = Color.Yellow;
			this.txt1.Location = new Point(251, 0);
			this.txt1.Margin = new Padding(0);
			this.txt1.Name = "txt1";
			this.txt1.Padding = new Padding(0, 2, 0, 0);
			this.txt1.Size = new Size(22, 30);
			this.txt1.TabIndex = 7;
			this.txt1.Text = "8";
			this.txt1.TextAlign = ContentAlignment.TopCenter;
			this.txt1.Enter += this.txt_Enter;
			this.txt1.Leave += this.txt_Leave;
			this.txt1.MouseDown += this.txt_MouseDown;
			this.txt1.MouseEnter += this.txt_MouseEnter;
			this.txt1.MouseWheel += this.txt_MouseWheel;
			this.txt1.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt2.BackColor = Color.FromArgb(80, 80, 80);
			this.txt2.BorderStyle = BorderStyle.FixedSingle;
			this.txt2.Font = new Font("LCD", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt2.ForeColor = Color.Yellow;
			this.txt2.Location = new Point(231, 0);
			this.txt2.Margin = new Padding(0);
			this.txt2.Name = "txt2";
			this.txt2.Padding = new Padding(0, 2, 0, 0);
			this.txt2.Size = new Size(22, 30);
			this.txt2.TabIndex = 6;
			this.txt2.Text = "8";
			this.txt2.TextAlign = ContentAlignment.TopCenter;
			this.txt2.Enter += this.txt_Enter;
			this.txt2.Leave += this.txt_Leave;
			this.txt2.MouseDown += this.txt_MouseDown;
			this.txt2.MouseEnter += this.txt_MouseEnter;
			this.txt2.MouseWheel += this.txt_MouseWheel;
			this.txt2.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt10.BorderStyle = BorderStyle.FixedSingle;
			this.txt10.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt10.ForeColor = Color.Yellow;
			this.txt10.Location = new Point(0, 0);
			this.txt10.Name = "txt10";
			this.txt10.Size = new Size(44, 34);
			this.txt10.TabIndex = 9;
			this.txt10.Text = "Freq";
			this.txt10.TextAlign = ContentAlignment.MiddleLeft;
			this.txt10.Enter += this.txt_Enter;
			this.txt10.Leave += this.txt_Leave;
			this.txt10.MouseDown += this.txt_MouseDown;
			this.txt10.MouseEnter += this.txt_MouseEnter;
			this.txt10.MouseWheel += this.txt_MouseWheel;
			this.txt10.PreviewKeyDown += this.txt_PreviewKeyDown;
			this.txt9.BackColor = Color.FromArgb(80, 80, 80);
			this.txt9.BorderStyle = BorderStyle.FixedSingle;
			this.txt9.Font = new Font("LCD", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txt9.ForeColor = Color.Yellow;
			this.txt9.Location = new Point(48, 0);
			this.txt9.Margin = new Padding(0);
			this.txt9.Name = "txt9";
			this.txt9.Padding = new Padding(0, 2, 0, 0);
			this.txt9.Size = new Size(24, 34);
			this.txt9.TabIndex = 10;
			this.txt9.Text = "8";
			this.txt9.TextAlign = ContentAlignment.TopCenter;
			this.txt9.Enter += this.txt_Enter;
			this.txt9.Leave += this.txt_Leave;
			this.txt9.MouseDown += this.txt_MouseDown;
			this.txt9.MouseEnter += this.txt_MouseEnter;
			this.txt9.PreviewKeyDown += this.txt_PreviewKeyDown;
			base.AutoScaleMode = AutoScaleMode.None;
			base.Controls.Add(this.txt9);
			base.Controls.Add(this.txt3);
			base.Controls.Add(this.txt4);
			base.Controls.Add(this.txt5);
			base.Controls.Add(this.txt6);
			base.Controls.Add(this.txt7);
			base.Controls.Add(this.txt8);
			base.Controls.Add(this.txt0);
			base.Controls.Add(this.txt1);
			base.Controls.Add(this.txt2);
			base.Controls.Add(this.txt10);
			base.Margin = new Padding(3, 2, 3, 2);
			base.Name = "Indicator";
			base.Size = new Size(300, 42);
			base.ResumeLayout(false);
		}
	}
}
