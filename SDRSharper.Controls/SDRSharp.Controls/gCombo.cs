using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace SDRSharp.Controls
{
	[DefaultEvent("SelectedIndexChanged")]
	public class gCombo : UserControl
	{
		private List<string> _items = new List<string>();

		private int _selectedIndex = -1;

		private string _selectedItem;

		private ToolTip _toolTip;

		private int _parentHeight;

		private bool _expanded;

		private IContainer components;

		private BorderGradientPanel gradientPanel;

		private gButton gButton0;

		private gButton gButton1;

		private gButton gButton2;

		private gButton gBexpand;

		private gButton gButton4;

		private gButton gButton5;

		private gButton gButton6;

		private gButton gButton7;

		private gButton gButton8;

		private gButton gButton9;

		private gButton gButton3;

		private gButton gButton10;

		private gButton gButton11;

		private gButton gButton12;

		private gButton gButton13;

		private gButton gButton14;

		private gButton gButton15;

		private gButton gButton16;

		private gButton gButton17;

		private gButton gButton18;

		private Label textBox;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", typeof(UITypeEditor))]
		public List<string> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				this._items = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return this.textBox.Text;
			}
			set
			{
				this.textBox.Text = value;
			}
		}

		public string SelectedItem => this._selectedItem;

		public int SelectedIndex
		{
			get
			{
				return this._selectedIndex;
			}
			set
			{
				this.SetIndex(value);
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

		public event EventHandler SelectedIndexChanged;

		public gCombo()
		{
			this.InitializeComponent();
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
		}

		public void Add(string item)
		{
			this._items.Add(item);
		}

		public void setEnabled(int index, bool enabled)
		{
			if (index >= 0 && index < this.Items.Count)
			{
				gButton gButton = (gButton)base.Controls["gButton" + index.ToString()];
				gButton.Enabled = enabled;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (!this._expanded)
			{
				this.gBexpand.Height = base.Height - 1;
				this.gBexpand.Width = this.gBexpand.Height - 4;
				this.gBexpand.Left = base.Width - this.gBexpand.Width;
				this.gradientPanel.Height = base.Height;
				this.gradientPanel.Width = this.gBexpand.Left - 2;
				this.textBox.Left = 5;
				this.textBox.Width = this.gradientPanel.Width - this.textBox.Left - 5;
				this.textBox.Height = this.gradientPanel.Height - 8;
				this.textBox.Top = (base.Height - this.textBox.Height) / 2;
			}
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

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (this._toolTip != null)
			{
				string toolTip = this._toolTip.GetToolTip(this);
				this._toolTip.SetToolTip(this.textBox, toolTip);
				this._toolTip.SetToolTip(this.gBexpand, toolTip);
			}
			foreach (Control control in base.Controls)
			{
				string name = control.Name;
				if (name.IndexOf("gButton") >= 0)
				{
					int num = this.iVal(name.Substring(7));
					gButton gButton = (gButton)control;
					gButton.Height = this.textBox.Height + 2;
					gButton.Top = this.gradientPanel.Height + 1 + num * gButton.Height;
					gButton.Visible = false;
					if (num < this._items.Count)
					{
						gButton.Text = this._items[num];
						gButton.ForeColor = ((num == this._selectedIndex) ? Color.Cyan : Color.White);
					}
				}
			}
		}

		private void textBox_MouseDown(object sender, MouseEventArgs e)
		{
			this.gBexpand_MouseDown(sender, null);
		}

		private void gBexpand_MouseDown(object sender, MouseEventArgs e)
		{
			this._expanded = !this._expanded;
			if (this._expanded)
			{
				this._parentHeight = 0;
				base.Height = this.gButton0.Top + this._items.Count * this.gButton0.Height;
				if (base.Top + base.Height > base.Parent.Height)
				{
					this._parentHeight = base.Parent.Height;
					base.Parent.Height = base.Top + base.Height;
				}
				base.BringToFront();
			}
			else
			{
				base.Height = this.gradientPanel.Height;
				if (this._parentHeight > 0)
				{
					base.Parent.Height = this._parentHeight;
				}
			}
			for (int i = 0; i < this._items.Count; i++)
			{
				gButton gButton = (gButton)base.Controls["gButton" + i.ToString()];
				gButton.Visible = this._expanded;
			}
			if (sender != null)
			{
				this.gBexpand.Focus();
			}
		}

		private void gList_Click(object sender, MouseEventArgs e)
		{
			this.gBexpand.Checked = false;
			this.gBexpand_MouseDown(sender, null);
			gButton gButton = (gButton)sender;
			this.SetIndex(this.iVal(gButton.Name.Substring(7)));
		}

		private void SetIndex(int index)
		{
			if (index >= 0 && index < this._items.Count)
			{
				this._selectedItem = this._items[index];
				gButton gButton = (gButton)base.Controls["gButton" + index.ToString()];
				this.textBox.Text = this._selectedItem;
				gButton.Text = this._selectedItem;
				if (index != this._selectedIndex)
				{
					this._selectedIndex = index;
					string name = gButton.Name;
					for (int i = 0; i < this._items.Count; i++)
					{
						gButton gButton2 = (gButton)base.Controls["gButton" + i.ToString()];
						if (gButton2.Name != name)
						{
							gButton2.ForeColor = Color.White;
							gButton2.Checked = false;
						}
					}
					gButton.ForeColor = Color.Cyan;
					if (this.SelectedIndexChanged != null)
					{
						this.SelectedIndexChanged(this, new EventArgs());
					}
				}
			}
		}

		private int iVal(string text)
		{
			int result = 0;
			if (int.TryParse(text, out result))
			{
				return result;
			}
			return 0;
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
			this.textBox = new Label();
			this.gBexpand = new gButton();
			this.gradientPanel = new BorderGradientPanel();
			this.gButton18 = new gButton();
			this.gButton17 = new gButton();
			this.gButton16 = new gButton();
			this.gButton15 = new gButton();
			this.gButton14 = new gButton();
			this.gButton13 = new gButton();
			this.gButton12 = new gButton();
			this.gButton11 = new gButton();
			this.gButton10 = new gButton();
			this.gButton9 = new gButton();
			this.gButton8 = new gButton();
			this.gButton7 = new gButton();
			this.gButton6 = new gButton();
			this.gButton5 = new gButton();
			this.gButton4 = new gButton();
			this.gButton3 = new gButton();
			this.gButton2 = new gButton();
			this.gButton1 = new gButton();
			this.gButton0 = new gButton();
			base.SuspendLayout();
			this.textBox.BackColor = Color.FromArgb(54, 54, 54);
			this.textBox.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.textBox.ForeColor = Color.Yellow;
			this.textBox.Location = new Point(9, 5);
			this.textBox.Name = "textBox";
			this.textBox.Size = new Size(98, 16);
			this.textBox.TabIndex = 21;
			this.textBox.Text = "label1";
			this.textBox.MouseDown += this.textBox_MouseDown;
			this.gBexpand.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.gBexpand.Arrow = 3;
			this.gBexpand.BackColor = SystemColors.Control;
			this.gBexpand.Checked = false;
			this.gBexpand.Edge = 0.15f;
			this.gBexpand.EndColor = Color.White;
			this.gBexpand.EndFactor = 0.2f;
			this.gBexpand.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.gBexpand.Location = new Point(113, 1);
			this.gBexpand.Name = "gBexpand";
			this.gBexpand.NoBorder = false;
			this.gBexpand.NoLed = true;
			this.gBexpand.RadioButton = false;
			this.gBexpand.Radius = 4;
			this.gBexpand.RadiusB = 6;
			this.gBexpand.Size = new Size(18, 23);
			this.gBexpand.StartColor = Color.Black;
			this.gBexpand.StartFactor = 0.35f;
			this.gBexpand.TabIndex = 1;
			this.gBexpand.MouseDown += this.gBexpand_MouseDown;
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
			this.gradientPanel.TabIndex = 15;
			this.gradientPanel.TabStop = false;
			this.gradientPanel.Text = "borderGradientPanel3";
			this.gButton18.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton18.Arrow = 0;
			this.gButton18.BackColor = SystemColors.Control;
			this.gButton18.Checked = false;
			this.gButton18.Edge = 0.15f;
			this.gButton18.EndColor = Color.White;
			this.gButton18.EndFactor = 0.2f;
			this.gButton18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton18.ForeColor = Color.White;
			this.gButton18.Location = new Point(0, 440);
			this.gButton18.Name = "gButton18";
			this.gButton18.NoBorder = false;
			this.gButton18.NoLed = true;
			this.gButton18.RadioButton = false;
			this.gButton18.Radius = 6;
			this.gButton18.RadiusB = 0;
			this.gButton18.Size = new Size(131, 23);
			this.gButton18.StartColor = Color.Black;
			this.gButton18.StartFactor = 0.35f;
			this.gButton18.TabIndex = 20;
			this.gButton18.TabStop = false;
			this.gButton18.Text = "19";
			this.gButton18.MouseUp += this.gList_Click;
			this.gButton17.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton17.Arrow = 0;
			this.gButton17.BackColor = SystemColors.Control;
			this.gButton17.Checked = false;
			this.gButton17.Edge = 0.15f;
			this.gButton17.EndColor = Color.White;
			this.gButton17.EndFactor = 0.2f;
			this.gButton17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton17.ForeColor = Color.White;
			this.gButton17.Location = new Point(0, 417);
			this.gButton17.Name = "gButton17";
			this.gButton17.NoBorder = false;
			this.gButton17.NoLed = true;
			this.gButton17.RadioButton = false;
			this.gButton17.Radius = 6;
			this.gButton17.RadiusB = 0;
			this.gButton17.Size = new Size(131, 23);
			this.gButton17.StartColor = Color.Black;
			this.gButton17.StartFactor = 0.35f;
			this.gButton17.TabIndex = 19;
			this.gButton17.TabStop = false;
			this.gButton17.Text = "18";
			this.gButton17.MouseUp += this.gList_Click;
			this.gButton16.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton16.Arrow = 0;
			this.gButton16.BackColor = SystemColors.Control;
			this.gButton16.Checked = false;
			this.gButton16.Edge = 0.15f;
			this.gButton16.EndColor = Color.White;
			this.gButton16.EndFactor = 0.2f;
			this.gButton16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton16.ForeColor = Color.White;
			this.gButton16.Location = new Point(0, 394);
			this.gButton16.Name = "gButton16";
			this.gButton16.NoBorder = false;
			this.gButton16.NoLed = true;
			this.gButton16.RadioButton = false;
			this.gButton16.Radius = 6;
			this.gButton16.RadiusB = 0;
			this.gButton16.Size = new Size(131, 23);
			this.gButton16.StartColor = Color.Black;
			this.gButton16.StartFactor = 0.35f;
			this.gButton16.TabIndex = 18;
			this.gButton16.TabStop = false;
			this.gButton16.Text = "17";
			this.gButton16.MouseUp += this.gList_Click;
			this.gButton15.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton15.Arrow = 0;
			this.gButton15.BackColor = SystemColors.Control;
			this.gButton15.Checked = false;
			this.gButton15.Edge = 0.15f;
			this.gButton15.EndColor = Color.White;
			this.gButton15.EndFactor = 0.2f;
			this.gButton15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton15.ForeColor = Color.White;
			this.gButton15.Location = new Point(0, 371);
			this.gButton15.Name = "gButton15";
			this.gButton15.NoBorder = false;
			this.gButton15.NoLed = true;
			this.gButton15.RadioButton = false;
			this.gButton15.Radius = 6;
			this.gButton15.RadiusB = 0;
			this.gButton15.Size = new Size(131, 23);
			this.gButton15.StartColor = Color.Black;
			this.gButton15.StartFactor = 0.35f;
			this.gButton15.TabIndex = 17;
			this.gButton15.TabStop = false;
			this.gButton15.Text = "16";
			this.gButton15.MouseUp += this.gList_Click;
			this.gButton14.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton14.Arrow = 0;
			this.gButton14.BackColor = SystemColors.Control;
			this.gButton14.Checked = false;
			this.gButton14.Edge = 0.15f;
			this.gButton14.EndColor = Color.White;
			this.gButton14.EndFactor = 0.2f;
			this.gButton14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton14.ForeColor = Color.White;
			this.gButton14.Location = new Point(0, 348);
			this.gButton14.Name = "gButton14";
			this.gButton14.NoBorder = false;
			this.gButton14.NoLed = true;
			this.gButton14.RadioButton = false;
			this.gButton14.Radius = 6;
			this.gButton14.RadiusB = 0;
			this.gButton14.Size = new Size(131, 23);
			this.gButton14.StartColor = Color.Black;
			this.gButton14.StartFactor = 0.35f;
			this.gButton14.TabIndex = 16;
			this.gButton14.TabStop = false;
			this.gButton14.Text = "15";
			this.gButton14.MouseUp += this.gList_Click;
			this.gButton13.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton13.Arrow = 0;
			this.gButton13.BackColor = SystemColors.Control;
			this.gButton13.Checked = false;
			this.gButton13.Edge = 0.15f;
			this.gButton13.EndColor = Color.White;
			this.gButton13.EndFactor = 0.2f;
			this.gButton13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton13.ForeColor = Color.White;
			this.gButton13.Location = new Point(0, 325);
			this.gButton13.Name = "gButton13";
			this.gButton13.NoBorder = false;
			this.gButton13.NoLed = true;
			this.gButton13.RadioButton = false;
			this.gButton13.Radius = 6;
			this.gButton13.RadiusB = 0;
			this.gButton13.Size = new Size(131, 23);
			this.gButton13.StartColor = Color.Black;
			this.gButton13.StartFactor = 0.35f;
			this.gButton13.TabIndex = 15;
			this.gButton13.TabStop = false;
			this.gButton13.Text = "14";
			this.gButton13.MouseUp += this.gList_Click;
			this.gButton12.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton12.Arrow = 0;
			this.gButton12.BackColor = SystemColors.Control;
			this.gButton12.Checked = false;
			this.gButton12.Edge = 0.15f;
			this.gButton12.EndColor = Color.White;
			this.gButton12.EndFactor = 0.2f;
			this.gButton12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton12.ForeColor = Color.White;
			this.gButton12.Location = new Point(0, 302);
			this.gButton12.Name = "gButton12";
			this.gButton12.NoBorder = false;
			this.gButton12.NoLed = true;
			this.gButton12.RadioButton = false;
			this.gButton12.Radius = 6;
			this.gButton12.RadiusB = 0;
			this.gButton12.Size = new Size(131, 23);
			this.gButton12.StartColor = Color.Black;
			this.gButton12.StartFactor = 0.35f;
			this.gButton12.TabIndex = 14;
			this.gButton12.TabStop = false;
			this.gButton12.Text = "13";
			this.gButton12.MouseUp += this.gList_Click;
			this.gButton11.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton11.Arrow = 0;
			this.gButton11.BackColor = SystemColors.Control;
			this.gButton11.Checked = false;
			this.gButton11.Edge = 0.15f;
			this.gButton11.EndColor = Color.White;
			this.gButton11.EndFactor = 0.2f;
			this.gButton11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton11.ForeColor = Color.White;
			this.gButton11.Location = new Point(0, 279);
			this.gButton11.Name = "gButton11";
			this.gButton11.NoBorder = false;
			this.gButton11.NoLed = true;
			this.gButton11.RadioButton = false;
			this.gButton11.Radius = 6;
			this.gButton11.RadiusB = 0;
			this.gButton11.Size = new Size(131, 23);
			this.gButton11.StartColor = Color.Black;
			this.gButton11.StartFactor = 0.35f;
			this.gButton11.TabIndex = 13;
			this.gButton11.TabStop = false;
			this.gButton11.Text = "12";
			this.gButton11.MouseUp += this.gList_Click;
			this.gButton10.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton10.Arrow = 0;
			this.gButton10.BackColor = SystemColors.Control;
			this.gButton10.Checked = false;
			this.gButton10.Edge = 0.15f;
			this.gButton10.EndColor = Color.White;
			this.gButton10.EndFactor = 0.2f;
			this.gButton10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton10.ForeColor = Color.White;
			this.gButton10.Location = new Point(0, 256);
			this.gButton10.Name = "gButton10";
			this.gButton10.NoBorder = false;
			this.gButton10.NoLed = true;
			this.gButton10.RadioButton = false;
			this.gButton10.Radius = 6;
			this.gButton10.RadiusB = 0;
			this.gButton10.Size = new Size(131, 23);
			this.gButton10.StartColor = Color.Black;
			this.gButton10.StartFactor = 0.35f;
			this.gButton10.TabIndex = 12;
			this.gButton10.TabStop = false;
			this.gButton10.Text = "11";
			this.gButton10.MouseUp += this.gList_Click;
			this.gButton9.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton9.Arrow = 0;
			this.gButton9.BackColor = SystemColors.Control;
			this.gButton9.Checked = false;
			this.gButton9.Edge = 0.15f;
			this.gButton9.EndColor = Color.White;
			this.gButton9.EndFactor = 0.2f;
			this.gButton9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton9.ForeColor = Color.White;
			this.gButton9.Location = new Point(0, 233);
			this.gButton9.Name = "gButton9";
			this.gButton9.NoBorder = false;
			this.gButton9.NoLed = true;
			this.gButton9.RadioButton = false;
			this.gButton9.Radius = 6;
			this.gButton9.RadiusB = 0;
			this.gButton9.Size = new Size(131, 23);
			this.gButton9.StartColor = Color.Black;
			this.gButton9.StartFactor = 0.35f;
			this.gButton9.TabIndex = 11;
			this.gButton9.TabStop = false;
			this.gButton9.Text = "10";
			this.gButton9.MouseUp += this.gList_Click;
			this.gButton8.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton8.Arrow = 0;
			this.gButton8.BackColor = SystemColors.Control;
			this.gButton8.Checked = false;
			this.gButton8.Edge = 0.15f;
			this.gButton8.EndColor = Color.White;
			this.gButton8.EndFactor = 0.2f;
			this.gButton8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton8.ForeColor = Color.White;
			this.gButton8.Location = new Point(0, 210);
			this.gButton8.Name = "gButton8";
			this.gButton8.NoBorder = false;
			this.gButton8.NoLed = true;
			this.gButton8.RadioButton = false;
			this.gButton8.Radius = 6;
			this.gButton8.RadiusB = 0;
			this.gButton8.Size = new Size(131, 23);
			this.gButton8.StartColor = Color.Black;
			this.gButton8.StartFactor = 0.35f;
			this.gButton8.TabIndex = 10;
			this.gButton8.TabStop = false;
			this.gButton8.Text = "9";
			this.gButton8.MouseUp += this.gList_Click;
			this.gButton7.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton7.Arrow = 0;
			this.gButton7.BackColor = SystemColors.Control;
			this.gButton7.Checked = false;
			this.gButton7.Edge = 0.15f;
			this.gButton7.EndColor = Color.White;
			this.gButton7.EndFactor = 0.2f;
			this.gButton7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton7.ForeColor = Color.White;
			this.gButton7.Location = new Point(0, 187);
			this.gButton7.Name = "gButton7";
			this.gButton7.NoBorder = false;
			this.gButton7.NoLed = true;
			this.gButton7.RadioButton = false;
			this.gButton7.Radius = 6;
			this.gButton7.RadiusB = 0;
			this.gButton7.Size = new Size(131, 23);
			this.gButton7.StartColor = Color.Black;
			this.gButton7.StartFactor = 0.35f;
			this.gButton7.TabIndex = 9;
			this.gButton7.TabStop = false;
			this.gButton7.Text = "8";
			this.gButton7.MouseUp += this.gList_Click;
			this.gButton6.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton6.Arrow = 0;
			this.gButton6.BackColor = SystemColors.Control;
			this.gButton6.Checked = false;
			this.gButton6.Edge = 0.15f;
			this.gButton6.EndColor = Color.White;
			this.gButton6.EndFactor = 0.2f;
			this.gButton6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton6.ForeColor = Color.White;
			this.gButton6.Location = new Point(0, 164);
			this.gButton6.Name = "gButton6";
			this.gButton6.NoBorder = false;
			this.gButton6.NoLed = true;
			this.gButton6.RadioButton = false;
			this.gButton6.Radius = 6;
			this.gButton6.RadiusB = 0;
			this.gButton6.Size = new Size(131, 23);
			this.gButton6.StartColor = Color.Black;
			this.gButton6.StartFactor = 0.35f;
			this.gButton6.TabIndex = 8;
			this.gButton6.TabStop = false;
			this.gButton6.Text = "7";
			this.gButton6.MouseUp += this.gList_Click;
			this.gButton5.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton5.Arrow = 0;
			this.gButton5.BackColor = SystemColors.Control;
			this.gButton5.Checked = false;
			this.gButton5.Edge = 0.15f;
			this.gButton5.EndColor = Color.White;
			this.gButton5.EndFactor = 0.2f;
			this.gButton5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton5.ForeColor = Color.White;
			this.gButton5.Location = new Point(0, 141);
			this.gButton5.Name = "gButton5";
			this.gButton5.NoBorder = false;
			this.gButton5.NoLed = true;
			this.gButton5.RadioButton = false;
			this.gButton5.Radius = 6;
			this.gButton5.RadiusB = 0;
			this.gButton5.Size = new Size(131, 23);
			this.gButton5.StartColor = Color.Black;
			this.gButton5.StartFactor = 0.35f;
			this.gButton5.TabIndex = 7;
			this.gButton5.TabStop = false;
			this.gButton5.Text = "6";
			this.gButton5.MouseUp += this.gList_Click;
			this.gButton4.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton4.Arrow = 0;
			this.gButton4.BackColor = SystemColors.Control;
			this.gButton4.Checked = false;
			this.gButton4.Edge = 0.15f;
			this.gButton4.EndColor = Color.White;
			this.gButton4.EndFactor = 0.2f;
			this.gButton4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton4.ForeColor = Color.White;
			this.gButton4.Location = new Point(-1, 118);
			this.gButton4.Name = "gButton4";
			this.gButton4.NoBorder = false;
			this.gButton4.NoLed = true;
			this.gButton4.RadioButton = false;
			this.gButton4.Radius = 6;
			this.gButton4.RadiusB = 0;
			this.gButton4.Size = new Size(131, 23);
			this.gButton4.StartColor = Color.Black;
			this.gButton4.StartFactor = 0.35f;
			this.gButton4.TabIndex = 6;
			this.gButton4.TabStop = false;
			this.gButton4.Text = "5";
			this.gButton4.MouseUp += this.gList_Click;
			this.gButton3.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton3.Arrow = 0;
			this.gButton3.BackColor = SystemColors.Control;
			this.gButton3.Checked = false;
			this.gButton3.Edge = 0.15f;
			this.gButton3.EndColor = Color.White;
			this.gButton3.EndFactor = 0.2f;
			this.gButton3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton3.ForeColor = Color.White;
			this.gButton3.Location = new Point(0, 95);
			this.gButton3.Name = "gButton3";
			this.gButton3.NoBorder = false;
			this.gButton3.NoLed = true;
			this.gButton3.RadioButton = false;
			this.gButton3.Radius = 6;
			this.gButton3.RadiusB = 0;
			this.gButton3.Size = new Size(131, 23);
			this.gButton3.StartColor = Color.Black;
			this.gButton3.StartFactor = 0.35f;
			this.gButton3.TabIndex = 5;
			this.gButton3.TabStop = false;
			this.gButton3.Text = "4";
			this.gButton3.MouseUp += this.gList_Click;
			this.gButton2.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton2.Arrow = 0;
			this.gButton2.BackColor = SystemColors.Control;
			this.gButton2.Checked = false;
			this.gButton2.Edge = 0.15f;
			this.gButton2.EndColor = Color.White;
			this.gButton2.EndFactor = 0.2f;
			this.gButton2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton2.ForeColor = Color.White;
			this.gButton2.Location = new Point(0, 72);
			this.gButton2.Name = "gButton2";
			this.gButton2.NoBorder = false;
			this.gButton2.NoLed = true;
			this.gButton2.RadioButton = false;
			this.gButton2.Radius = 6;
			this.gButton2.RadiusB = 0;
			this.gButton2.Size = new Size(131, 23);
			this.gButton2.StartColor = Color.Black;
			this.gButton2.StartFactor = 0.35f;
			this.gButton2.TabIndex = 4;
			this.gButton2.TabStop = false;
			this.gButton2.Text = "3";
			this.gButton2.MouseUp += this.gList_Click;
			this.gButton1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton1.Arrow = 0;
			this.gButton1.BackColor = SystemColors.Control;
			this.gButton1.Checked = false;
			this.gButton1.Edge = 0.15f;
			this.gButton1.EndColor = Color.White;
			this.gButton1.EndFactor = 0.2f;
			this.gButton1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton1.ForeColor = Color.White;
			this.gButton1.Location = new Point(0, 49);
			this.gButton1.Name = "gButton1";
			this.gButton1.NoBorder = false;
			this.gButton1.NoLed = true;
			this.gButton1.RadioButton = false;
			this.gButton1.Radius = 6;
			this.gButton1.RadiusB = 0;
			this.gButton1.Size = new Size(131, 23);
			this.gButton1.StartColor = Color.Black;
			this.gButton1.StartFactor = 0.35f;
			this.gButton1.TabIndex = 3;
			this.gButton1.TabStop = false;
			this.gButton1.Text = "2";
			this.gButton1.MouseUp += this.gList_Click;
			this.gButton0.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.gButton0.Arrow = 0;
			this.gButton0.BackColor = SystemColors.Control;
			this.gButton0.Checked = false;
			this.gButton0.Edge = 0.15f;
			this.gButton0.EndColor = Color.White;
			this.gButton0.EndFactor = 0.2f;
			this.gButton0.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.gButton0.ForeColor = Color.White;
			this.gButton0.Location = new Point(0, 26);
			this.gButton0.Name = "gButton0";
			this.gButton0.NoBorder = false;
			this.gButton0.NoLed = true;
			this.gButton0.RadioButton = false;
			this.gButton0.Radius = 6;
			this.gButton0.RadiusB = 0;
			this.gButton0.Size = new Size(131, 23);
			this.gButton0.StartColor = Color.Black;
			this.gButton0.StartFactor = 0.35f;
			this.gButton0.TabIndex = 2;
			this.gButton0.Text = "1";
			this.gButton0.MouseUp += this.gList_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.gBexpand);
			base.Controls.Add(this.textBox);
			base.Controls.Add(this.gradientPanel);
			base.Controls.Add(this.gButton18);
			base.Controls.Add(this.gButton17);
			base.Controls.Add(this.gButton16);
			base.Controls.Add(this.gButton15);
			base.Controls.Add(this.gButton14);
			base.Controls.Add(this.gButton13);
			base.Controls.Add(this.gButton12);
			base.Controls.Add(this.gButton11);
			base.Controls.Add(this.gButton10);
			base.Controls.Add(this.gButton9);
			base.Controls.Add(this.gButton8);
			base.Controls.Add(this.gButton7);
			base.Controls.Add(this.gButton6);
			base.Controls.Add(this.gButton5);
			base.Controls.Add(this.gButton4);
			base.Controls.Add(this.gButton3);
			base.Controls.Add(this.gButton2);
			base.Controls.Add(this.gButton1);
			base.Controls.Add(this.gButton0);
			base.Name = "gCombo";
			base.Size = new Size(131, 495);
			base.ResumeLayout(false);
		}
	}
}
