using SDRSharp.Radio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	public class GradientDialog : Form
	{
		public delegate void ManualGradientChange(object sender, GradientEventArgs e);

		private static int _blendIndex = -1;

		private static ColorBlend[] _blends;

		private static int[] _traceColors;

		private static int[] _backgrounds;

		private static int[] _spectrumFill;

		private static GradientDialog _form;

		private bool _editColor;

		private bool _editGradient;

		private bool _editTrace;

		private bool _editBackground;

		private static Bitmap _hslBitmap;

		private static Bitmap _original;

		private static Graphics _graphics;

		private static int _saturation = -1;

		private bool _moving0;

		private bool _moving1;

		private bool _save;

		private int _x0;

		private int _y0;

		private int _x1;

		private int _y1;

		private IContainer components;

		private ListBox colorListBox;

		private Button upButton;

		private Button downButton;

		private PictureBox gradientPictureBox;

		private Button addButton;

		private Button deleteButton;

		private Button cancelButton;

		private ColorDialog colorDialog;

		private RadioButton btn1;

		private RadioButton btn2;

		private RadioButton btn3;

		private RadioButton btn4;

		private RadioButton btn5;

		private Button traceButton;

		private Button backgroundButton;

		private ComboBox cmbFill;

		private Label labFill;

		private Label label2;

		private PictureBox picBox;

		private GroupBox groupBox1;

		private Button saButton;

		private Button wfButton;

		private Button agButton;

		private ToolTip toolTip;

		private TrackBar trackBar;

		private Button fastButton;

		private Button okButton;

		public static event ManualGradientChange GradientChanged;

		private GradientDialog()
		{
			this.InitializeComponent();
			this.drawHslBitmap(100);
			this.picBox.BackgroundImage = GradientDialog._hslBitmap;
			GradientDialog._original = (Bitmap)GradientDialog._hslBitmap.Clone();
			GradientDialog._graphics = Graphics.FromImage(GradientDialog._hslBitmap);
		}

		private void picBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (Math.Abs(e.X - this._x0) < 5 && Math.Abs(e.Y - this._y0) < 5)
			{
				this._moving0 = true;
			}
			else if (Math.Abs(e.X - this._x1) < 5 && Math.Abs(e.Y - this._y1) < 5)
			{
				this._moving1 = true;
			}
		}

		private void picBox_MouseUp(object sender, MouseEventArgs e)
		{
			this._moving0 = false;
			this._moving1 = false;
		}

		private void picBox_MouseMove(object sender, MouseEventArgs e)
		{
			if ((Math.Abs(e.X - this._x0) >= 5 || Math.Abs(e.Y - this._y0) >= 5) && !this._moving0 && (Math.Abs(e.X - this._x1) >= 5 || Math.Abs(e.Y - this._y1) >= 5) && !this._moving1)
			{
				this.Cursor = Cursors.Default;
				goto IL_007c;
			}
			this.Cursor = Cursors.Cross;
			goto IL_007c;
			IL_007c:
			if (e.Button == MouseButtons.Left)
			{
				int num = Math.Max(0, Math.Min(GradientDialog._hslBitmap.Width - 1, e.X));
				int num2 = Math.Max(0, Math.Min(GradientDialog._hslBitmap.Height - 1, e.Y));
				if (this._moving0)
				{
					this._x0 = num;
					this._y0 = num2;
				}
				else
				{
					if (!this._moving1)
					{
						return;
					}
					this._x1 = num;
					this._y1 = num2;
				}
				if (this._editColor || this._editTrace || this._editBackground)
				{
					GradientDialog._graphics.DrawImageUnscaled(GradientDialog._original, 0, 0);
					GradientDialog._graphics.DrawRectangle(((double)this._y0 > (double)this.picBox.Height * 0.5) ? Pens.White : Pens.Black, this._x0 - 4, this._y0 - 4, 8, 8);
					this.picBox.Invalidate();
					Color pixel = GradientDialog._original.GetPixel(num, num2);
					if (this._editTrace)
					{
						this.traceButton.BackColor = pixel;
					}
					else if (this._editBackground)
					{
						this.backgroundButton.BackColor = pixel;
					}
					else if (this.colorListBox.SelectedIndex >= 0)
					{
						this.colorListBox.Items[this.colorListBox.SelectedIndex] = pixel;
					}
					this.gradientPictureBox.Invalidate();
					this.signalGradientChanged();
				}
				else if (this._editGradient)
				{
					GradientDialog._graphics.DrawImageUnscaled(GradientDialog._original, 0, 0);
					GradientDialog._graphics.DrawRectangle(((double)this._y0 > (double)this.picBox.Height * 0.5) ? Pens.White : Pens.Black, this._x0 - 4, this._y0 - 4, 8, 8);
					GradientDialog._graphics.DrawRectangle(((double)this._y1 > (double)this.picBox.Height * 0.5) ? Pens.White : Pens.Black, this._x1 - 4, this._y1 - 4, 8, 8);
					GradientDialog._graphics.DrawLine(Pens.Gray, this._x0, this._y0, this._x1, this._y1);
					this.picBox.Invalidate();
					int num3 = (Math.Abs(this._x1 - this._x0) < GradientDialog._hslBitmap.Width / 2 || Math.Abs(this._y1 - this._y0) < GradientDialog._hslBitmap.Height / 2) ? 6 : 9;
					this.colorListBox.Items.Clear();
					this.colorListBox.SelectedIndex = -1;
					for (int i = 0; i <= num3; i++)
					{
						Color pixel2 = GradientDialog._original.GetPixel(this._x1 - i * (this._x1 - this._x0) / num3, this._y1 - i * (this._y1 - this._y0) / num3);
						this.colorListBox.Items.Add(pixel2);
					}
					this.gradientPictureBox.Invalidate();
					this.signalGradientChanged();
				}
			}
		}

		public static void ShowGradient(int blendIndex, ColorBlend[] blends, int[] backgrounds, int[] traceColors, int[] spectrumFill, string parent)
		{
			GradientDialog._blends = new ColorBlend[blends.GetUpperBound(0)];
			GradientDialog._blendIndex = Math.Abs(blendIndex);
			GradientDialog._blends = blends;
			GradientDialog._backgrounds = backgrounds;
			GradientDialog._traceColors = traceColors;
			GradientDialog._spectrumFill = spectrumFill;
			if (GradientDialog._form == null || GradientDialog._form.IsDisposed)
			{
				GradientDialog._form = new GradientDialog();
			}
			GradientDialog._form.TopMost = true;
			GradientDialog._form.Text = parent + " color editor";
			string a = parent.Substring(0, 1);
			GradientDialog._form.saButton.BackColor = ((a != "S") ? SystemColors.Control : Color.LightGreen);
			GradientDialog._form.wfButton.BackColor = ((a != "W") ? SystemColors.Control : Color.LightGreen);
			GradientDialog._form.agButton.BackColor = ((a != "A") ? SystemColors.Control : Color.LightGreen);
			if (a != "S")
			{
				GradientDialog._form.trackBar.Top = GradientDialog._form.groupBox1.Top + 4;
			}
			else
			{
				GradientDialog._form.trackBar.Top = GradientDialog._form.groupBox1.Top + GradientDialog._form.groupBox1.Height - GradientDialog._form.picBox.Height - GradientDialog._form.trackBar.Height - 4;
			}
			GradientDialog._form.picBox.Top = GradientDialog._form.trackBar.Top + GradientDialog._form.trackBar.Height;
			for (int i = 1; i <= 5; i++)
			{
				RadioButton radioButton = (RadioButton)GradientDialog._form.Controls["btn" + i.ToString()];
				radioButton.Checked = false;
				if (i == GradientDialog._blendIndex)
				{
					radioButton.Checked = true;
				}
			}
			GradientDialog._form.okButton.Enabled = false;
			if (!GradientDialog._form.Visible)
			{
				GradientDialog._form.ShowDialog();
			}
			else
			{
				GradientDialog._form.Activate();
			}
		}

		public static void CloseGradient()
		{
			if (GradientDialog._form != null && !GradientDialog._form.IsDisposed)
			{
				GradientDialog._form.Close();
			}
		}

		private ColorBlend GetColorBlend()
		{
			ColorBlend colorBlend = new ColorBlend(this.colorListBox.Items.Count);
			float num = 1f / (float)(colorBlend.Positions.Length - 1);
			for (int i = 0; i < colorBlend.Positions.Length; i++)
			{
				colorBlend.Positions[i] = (float)i * num;
				colorBlend.Colors[i] = (Color)this.colorListBox.Items[i];
			}
			return colorBlend;
		}

		private void SetColorBlend(ColorBlend colorBlend)
		{
			this.colorListBox.Items.Clear();
			this.colorListBox.SelectedIndex = -1;
			for (int i = 0; i < colorBlend.Positions.Length; i++)
			{
				this.colorListBox.Items.Add(colorBlend.Colors[i]);
			}
		}

		private void colorListBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index >= 0)
			{
				Color color = (Color)this.colorListBox.Items[e.Index];
				using (SolidBrush brush = new SolidBrush(color))
				{
					e.Graphics.FillRectangle(brush, e.Bounds.Left + 1, e.Bounds.Top + 1, e.Bounds.Width - 2, e.Bounds.Height - 1);
				}
				if ((e.State & DrawItemState.Selected) != 0)
				{
					e.Graphics.DrawRectangle(((double)color.GetBrightness() < 0.5) ? Pens.White : Pens.Black, e.Bounds.Left + 2, e.Bounds.Top + 2, e.Bounds.Width - 5, e.Bounds.Height - 3);
				}
			}
		}

		private void signalGradientChanged()
		{
			this.okButton.Enabled = true;
			if (base.Visible && GradientDialog.GradientChanged != null)
			{
				int num = GradientDialog._blendIndex - 1;
				GradientDialog._blends[num] = GradientDialog._form.GetColorBlend();
				GradientDialog._traceColors[num] = this.traceButton.BackColor.ToArgb();
				GradientDialog._backgrounds[num] = this.backgroundButton.BackColor.ToArgb();
				GradientDialog._spectrumFill[num] = GradientDialog._form.cmbFill.SelectedIndex;
				string text = this.Text.Substring(0, 1);
				if (text == " ")
				{
					text = "S";
				}
				GradientDialog.GradientChanged(this, new GradientEventArgs(text, GradientDialog._blendIndex, GradientDialog._blends[num], GradientDialog._backgrounds[num], GradientDialog._traceColors[num], GradientDialog._spectrumFill[num]));
			}
		}

		private void upButton_Click(object sender, EventArgs e)
		{
			if (this.colorListBox.SelectedIndex < 0)
			{
				MessageBox.Show("Please, select color to move.");
			}
			else if (this.colorListBox.SelectedIndex > 0)
			{
				int selectedIndex = this.colorListBox.SelectedIndex;
				object item = this.colorListBox.Items[selectedIndex];
				this.colorListBox.Items.RemoveAt(selectedIndex);
				this.colorListBox.Items.Insert(selectedIndex - 1, item);
				this.colorListBox.SelectedIndex = selectedIndex - 1;
				this.gradientPictureBox.Invalidate();
				this.signalGradientChanged();
			}
		}

		private void downButton_Click(object sender, EventArgs e)
		{
			if (this.colorListBox.SelectedIndex < 0)
			{
				MessageBox.Show("Please, select color to move.");
			}
			else if (this.colorListBox.SelectedIndex < this.colorListBox.Items.Count - 1)
			{
				int selectedIndex = this.colorListBox.SelectedIndex;
				object item = this.colorListBox.Items[selectedIndex];
				this.colorListBox.Items.RemoveAt(selectedIndex);
				this.colorListBox.Items.Insert(selectedIndex + 1, item);
				this.colorListBox.SelectedIndex = selectedIndex + 1;
				this.gradientPictureBox.Invalidate();
				this.signalGradientChanged();
			}
		}

		private void fastButton_Click(object sender, EventArgs e)
		{
			this.deleteButton.Enabled = false;
			this.upButton.Enabled = false;
			this.downButton.Enabled = false;
			this.colorListBox.SelectedIndex = -1;
			this._editGradient = false;
			this._editColor = false;
			this._editTrace = false;
			this._editBackground = false;
			this.drawHslLine(this.GetColorBlend(), -1);
			this._editGradient = true;
			this.picBox.Enabled = true;
			this.trackBar.Enabled = true;
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			Color color = (this.colorListBox.Items.Count == 0) ? Color.White : ((Color)this.colorListBox.Items[this.colorListBox.Items.Count - 1]);
			if (this.colorListBox.SelectedIndex >= 0)
			{
				color = (Color)this.colorListBox.Items[this.colorListBox.SelectedIndex];
			}
			this.colorListBox.Items.Insert(this.colorListBox.SelectedIndex, color);
			this.colorListBox.SelectedIndex--;
			this.drawHslPoint(color, -1);
			this._editGradient = false;
			this._editColor = true;
			this._editTrace = false;
			this._editBackground = false;
			this.picBox.Enabled = true;
			this.trackBar.Enabled = true;
			this.signalGradientChanged();
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			if (this.colorListBox.SelectedIndex < 0)
			{
				MessageBox.Show("Please, select color to delete first.");
			}
			else if (this.colorListBox.Items.Count <= 2)
			{
				MessageBox.Show("Minumum number of gradient colors is two.");
			}
			else
			{
				int selectedIndex = this.colorListBox.SelectedIndex;
				this.colorListBox.Items.RemoveAt(this.colorListBox.SelectedIndex);
				this.colorListBox.SelectedIndex = Math.Min(selectedIndex, this.colorListBox.Items.Count - 1);
				this.gradientPictureBox.Invalidate();
				this.signalGradientChanged();
			}
		}

		private void gradientPictureBox_Paint(object sender, PaintEventArgs e)
		{
			ColorBlend colorBlend = this.GetColorBlend();
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.gradientPictureBox.ClientRectangle, Color.White, Color.Black, LinearGradientMode.Vertical))
			{
				linearGradientBrush.InterpolationColors = colorBlend;
				e.Graphics.FillRectangle(linearGradientBrush, e.ClipRectangle);
			}
		}

		private void btnGradient_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Checked)
			{
				int.TryParse(radioButton.Text, out GradientDialog._blendIndex);
				int num = GradientDialog._blendIndex - 1;
				this.SetColorBlend(GradientDialog._blends[num]);
				this.backgroundButton.BackColor = Color.FromArgb(GradientDialog._backgrounds[num]);
				this.traceButton.BackColor = Color.FromArgb(GradientDialog._traceColors[num]);
				this.cmbFill.SelectedIndex = GradientDialog._spectrumFill[num];
				this.gradientPictureBox.Invalidate();
				this.signalGradientChanged();
				this.deleteButton.Enabled = false;
				this.upButton.Enabled = false;
				this.downButton.Enabled = false;
				GradientDialog._graphics.FillRectangle(Brushes.Gray, 0, 0, GradientDialog._hslBitmap.Width, GradientDialog._hslBitmap.Height);
				this._editGradient = false;
				this._editColor = false;
				this._editTrace = false;
				this._editBackground = false;
				this.picBox.Enabled = false;
				this.trackBar.Enabled = false;
				this.picBox.Invalidate();
			}
		}

		private void traceButton_Click(object sender, EventArgs e)
		{
			this.drawHslPoint(this.traceButton.BackColor, -1);
			this._editGradient = false;
			this._editColor = false;
			this._editTrace = true;
			this._editBackground = false;
			this.picBox.Enabled = true;
			this.trackBar.Enabled = true;
		}

		private void backgroundButton_Click(object sender, EventArgs e)
		{
			this.drawHslPoint(this.backgroundButton.BackColor, -1);
			this._editGradient = false;
			this._editColor = false;
			this._editTrace = false;
			this._editBackground = true;
			this.picBox.Enabled = true;
			this.trackBar.Enabled = true;
		}

		private void cmbFill_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (GradientDialog._spectrumFill[GradientDialog._blendIndex - 1] < 2 && this.cmbFill.SelectedIndex > 1)
			{
				MessageBox.Show("Note: selecting one of the dynamic fill colors may impact performance on older systems.");
			}
			this.signalGradientChanged();
		}

		private void colorListBox_Click(object sender, EventArgs e)
		{
			this.deleteButton.Enabled = true;
			this.upButton.Enabled = true;
			this.downButton.Enabled = true;
			Color color = (Color)this.colorListBox.Items[this.colorListBox.SelectedIndex];
			this._editGradient = false;
			this._editColor = false;
			this._editTrace = false;
			this._editBackground = false;
			this.drawHslPoint(color, -1);
			this.picBox.Enabled = true;
			this.trackBar.Enabled = true;
			this._editColor = true;
		}

		private void saButton_Click(object sender, EventArgs e)
		{
			if (GradientDialog.GradientChanged != null)
			{
				GradientDialog.GradientChanged(this, new GradientEventArgs("S", -1, GradientDialog._blends[0], 0, 0, 0));
			}
		}

		private void wfButton_Click(object sender, EventArgs e)
		{
			if (GradientDialog.GradientChanged != null)
			{
				GradientDialog.GradientChanged(this, new GradientEventArgs("W", -1, GradientDialog._blends[0], 0, 0, 0));
			}
		}

		private void agButton_Click(object sender, EventArgs e)
		{
			if (GradientDialog.GradientChanged != null)
			{
				GradientDialog.GradientChanged(this, new GradientEventArgs("A", -1, GradientDialog._blends[0], 0, 0, 0));
			}
		}

		private void trackBar_ValueChanged(object sender, EventArgs e)
		{
			if (this._editTrace)
			{
				this.drawHslPoint(this.traceButton.BackColor, this.trackBar.Value);
			}
			else if (this._editBackground)
			{
				this.drawHslPoint(this.backgroundButton.BackColor, this.trackBar.Value);
			}
			else if (this._editColor)
			{
				this.drawHslPoint((Color)this.colorListBox.Items[this.colorListBox.SelectedIndex], this.trackBar.Value);
			}
			else if (this._editGradient)
			{
				this.drawHslLine(this.GetColorBlend(), this.trackBar.Value);
			}
		}

		private void drawHslBitmap(int saturation)
		{
			if (saturation == GradientDialog._saturation)
			{
				GradientDialog._graphics.DrawImageUnscaled(GradientDialog._original, 0, 0);
			}
			else
			{
				if (GradientDialog._hslBitmap == null)
				{
					GradientDialog._hslBitmap = new Bitmap(this.picBox.Width, this.picBox.Height);
				}
				float s = (float)saturation / 100f;
				for (int i = 0; i < GradientDialog._hslBitmap.Height; i++)
				{
					for (int j = 0; j < GradientDialog._hslBitmap.Width; j++)
					{
						float h = (float)j / (float)GradientDialog._hslBitmap.Width;
						float l = (float)(GradientDialog._hslBitmap.Height - i) / (float)GradientDialog._hslBitmap.Height;
						Color color = Utils.HslToRgb(h, s, l);
						GradientDialog._hslBitmap.SetPixel(j, i, color);
					}
				}
				GradientDialog._original = (Bitmap)GradientDialog._hslBitmap.Clone();
				GradientDialog._saturation = saturation;
			}
		}

		private void drawHslLine(ColorBlend blend, int sat = -1)
		{
			int num = sat;
			Color color = blend.Colors[blend.Colors.Length - 1];
			Color color2 = blend.Colors[0];
			float num2 = color.GetHue() / 360f;
			float num3 = color.GetSaturation();
			float brightness = color.GetBrightness();
			float num4 = color2.GetHue() / 360f;
			float saturation = color2.GetSaturation();
			float brightness2 = color2.GetBrightness();
			if ((double)num3 < 0.001)
			{
				num2 = 0.5f;
			}
			if ((double)brightness < 0.001 || (double)brightness > 0.999)
			{
				num3 = 1f;
			}
			if ((double)saturation < 0.001)
			{
				num4 = 0.5f;
			}
			if ((double)brightness2 < 0.001 || (double)brightness2 > 0.999)
			{
				saturation = 1f;
			}
			if ((double)brightness < 0.001 || (double)brightness > 0.999)
			{
				int num5 = this.colorListBox.Items.Count / 2;
				float num6 = ((Color)this.colorListBox.Items[num5]).GetHue() / 360f;
				num2 = Math.Max(0f, Math.Min(1f, num4 + (num6 - num4) / (float)num5 * (float)(this.colorListBox.Items.Count - 1)));
			}
			if ((double)brightness2 < 0.001 || (double)brightness2 > 0.999)
			{
				int num7 = this.colorListBox.Items.Count / 2;
				float num8 = ((Color)this.colorListBox.Items[num7]).GetHue() / 360f;
				num4 = Math.Max(0f, Math.Min(1f, num2 + (num8 - num2) / (float)num7 * (float)(this.colorListBox.Items.Count - 1)));
			}
			if (num < 0)
			{
				sat = (int)(num3 * 100f);
				this.trackBar.Value = sat;
				this._x0 = (int)(num2 * (float)(this.picBox.Width - 1));
				this._y0 = this.picBox.Height - 1 - (int)(brightness * (float)(this.picBox.Height - 1));
				this._x1 = (int)(num4 * (float)(this.picBox.Width - 1));
				this._y1 = this.picBox.Height - 1 - (int)(brightness2 * (float)(this.picBox.Height - 1));
			}
			this.drawHslBitmap(sat);
			GradientDialog._graphics.DrawRectangle((brightness < 0.5f) ? Pens.White : Pens.Black, this._x0 - 4, this._y0 - 4, 8, 8);
			GradientDialog._graphics.DrawRectangle((brightness2 < 0.5f) ? Pens.White : Pens.Black, this._x1 - 4, this._y1 - 4, 8, 8);
			GradientDialog._graphics.DrawLine(Pens.Gray, this._x0, this._y0, this._x1, this._y1);
			this.picBox.Invalidate();
			if (num >= 0 && this.colorListBox.Items.Count > 1)
			{
				int count = this.colorListBox.Items.Count;
				for (int i = 0; i < count; i++)
				{
					this.colorListBox.Items[i] = GradientDialog._original.GetPixel(this._x1 - i * (this._x1 - this._x0) / count, this._y1 - i * (this._y1 - this._y0) / count);
				}
				this.gradientPictureBox.Invalidate();
				this.signalGradientChanged();
			}
		}

		private void drawHslPoint(Color color, int sat = -1)
		{
			int num = sat;
			float num2 = color.GetHue() / 360f;
			float num3 = color.GetSaturation();
			float brightness = color.GetBrightness();
			if ((double)num3 < 0.001)
			{
				num2 = 0.5f;
			}
			if ((double)brightness < 0.001 || (double)brightness > 0.999)
			{
				num3 = 1f;
			}
			if (num < 0)
			{
				sat = (int)(num3 * 100f);
				this.trackBar.Value = sat;
				this._x0 = (int)(num2 * (float)(this.picBox.Width - 1));
				this._y0 = this.picBox.Height - 1 - (int)(brightness * (float)(this.picBox.Height - 1));
				this._x1 = -999;
				this._y1 = -999;
			}
			this.drawHslBitmap(sat);
			GradientDialog._graphics.DrawRectangle((brightness < 0.5f) ? Pens.White : Pens.Black, this._x0 - 4, this._y0 - 4, 8, 8);
			this.picBox.Invalidate();
			if (num >= 0)
			{
				color = GradientDialog._original.GetPixel(this._x0, this._y0);
				if (this._editTrace)
				{
					this.traceButton.BackColor = color;
				}
				else if (this._editBackground)
				{
					this.backgroundButton.BackColor = color;
				}
				else if (this._editColor && this.colorListBox.SelectedIndex >= 0)
				{
					this.colorListBox.Items[this.colorListBox.SelectedIndex] = color;
				}
				this.gradientPictureBox.Invalidate();
				this.signalGradientChanged();
			}
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			this._save = false;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			this._save = true;
		}

		private void GradientDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (GradientDialog.GradientChanged != null)
			{
				GradientDialog.GradientChanged(this, new GradientEventArgs(this._save ? "save" : "undo", 0, null, 0, 0, 0));
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
			this.components = new Container();
			this.colorListBox = new ListBox();
			this.upButton = new Button();
			this.downButton = new Button();
			this.gradientPictureBox = new PictureBox();
			this.addButton = new Button();
			this.deleteButton = new Button();
			this.cancelButton = new Button();
			this.colorDialog = new ColorDialog();
			this.btn1 = new RadioButton();
			this.btn2 = new RadioButton();
			this.btn3 = new RadioButton();
			this.btn4 = new RadioButton();
			this.btn5 = new RadioButton();
			this.traceButton = new Button();
			this.backgroundButton = new Button();
			this.cmbFill = new ComboBox();
			this.labFill = new Label();
			this.label2 = new Label();
			this.picBox = new PictureBox();
			this.groupBox1 = new GroupBox();
			this.fastButton = new Button();
			this.saButton = new Button();
			this.wfButton = new Button();
			this.agButton = new Button();
			this.toolTip = new ToolTip(this.components);
			this.trackBar = new TrackBar();
			this.okButton = new Button();
			((ISupportInitialize)this.gradientPictureBox).BeginInit();
			((ISupportInitialize)this.picBox).BeginInit();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.trackBar).BeginInit();
			base.SuspendLayout();
			this.colorListBox.BackColor = Color.FromArgb(224, 224, 224);
			this.colorListBox.DrawMode = DrawMode.OwnerDrawVariable;
			this.colorListBox.FormattingEnabled = true;
			this.colorListBox.ItemHeight = 15;
			this.colorListBox.Location = new Point(7, 21);
			this.colorListBox.Name = "colorListBox";
			this.colorListBox.Size = new Size(62, 210);
			this.colorListBox.TabIndex = 0;
			this.toolTip.SetToolTip(this.colorListBox, "Select. move or edit gradient color.");
			this.colorListBox.Click += this.colorListBox_Click;
			this.colorListBox.DrawItem += this.colorListBox_DrawItem;
			this.upButton.Enabled = false;
			this.upButton.Location = new Point(110, 161);
			this.upButton.Name = "upButton";
			this.upButton.Size = new Size(55, 23);
			this.upButton.TabIndex = 1;
			this.upButton.Text = "Up";
			this.toolTip.SetToolTip(this.upButton, "Move color up");
			this.upButton.UseVisualStyleBackColor = true;
			this.upButton.Click += this.upButton_Click;
			this.downButton.Enabled = false;
			this.downButton.Location = new Point(111, 191);
			this.downButton.Name = "downButton";
			this.downButton.Size = new Size(53, 23);
			this.downButton.TabIndex = 2;
			this.downButton.Text = "Down";
			this.toolTip.SetToolTip(this.downButton, "Move color down");
			this.downButton.UseVisualStyleBackColor = true;
			this.downButton.Click += this.downButton_Click;
			this.gradientPictureBox.BorderStyle = BorderStyle.FixedSingle;
			this.gradientPictureBox.Location = new Point(71, 23);
			this.gradientPictureBox.Name = "gradientPictureBox";
			this.gradientPictureBox.Size = new Size(29, 209);
			this.gradientPictureBox.TabIndex = 3;
			this.gradientPictureBox.TabStop = false;
			this.gradientPictureBox.Paint += this.gradientPictureBox_Paint;
			this.addButton.Location = new Point(105, 76);
			this.addButton.Name = "addButton";
			this.addButton.Size = new Size(63, 23);
			this.addButton.TabIndex = 3;
			this.addButton.Text = "Add color";
			this.toolTip.SetToolTip(this.addButton, "Add new color to gradient.");
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += this.addButton_Click;
			this.deleteButton.Enabled = false;
			this.deleteButton.Location = new Point(105, 106);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new Size(64, 23);
			this.deleteButton.TabIndex = 4;
			this.deleteButton.Text = "Delete";
			this.toolTip.SetToolTip(this.deleteButton, "Delete selected color");
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += this.deleteButton_Click;
			this.cancelButton.DialogResult = DialogResult.Cancel;
			this.cancelButton.Location = new Point(332, 280);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new Size(48, 20);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "Cancel";
			this.toolTip.SetToolTip(this.cancelButton, "Cancel/undo changes");
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += this.closeButton_Click;
			this.colorDialog.AnyColor = true;
			this.colorDialog.FullOpen = true;
			this.btn1.AutoSize = true;
			this.btn1.BackColor = SystemColors.Control;
			this.btn1.ForeColor = SystemColors.ControlText;
			this.btn1.Location = new Point(62, 10);
			this.btn1.Margin = new Padding(2);
			this.btn1.Name = "btn1";
			this.btn1.RightToLeft = RightToLeft.No;
			this.btn1.Size = new Size(31, 17);
			this.btn1.TabIndex = 8;
			this.btn1.TabStop = true;
			this.btn1.Text = "1";
			this.toolTip.SetToolTip(this.btn1, "Select gradient to use");
			this.btn1.UseVisualStyleBackColor = false;
			this.btn1.CheckedChanged += this.btnGradient_CheckedChanged;
			this.btn2.AutoSize = true;
			this.btn2.Location = new Point(92, 10);
			this.btn2.Margin = new Padding(2);
			this.btn2.Name = "btn2";
			this.btn2.RightToLeft = RightToLeft.No;
			this.btn2.Size = new Size(31, 17);
			this.btn2.TabIndex = 9;
			this.btn2.TabStop = true;
			this.btn2.Text = "2";
			this.btn2.UseVisualStyleBackColor = true;
			this.btn2.CheckedChanged += this.btnGradient_CheckedChanged;
			this.btn3.AutoSize = true;
			this.btn3.Location = new Point(122, 10);
			this.btn3.Margin = new Padding(2);
			this.btn3.Name = "btn3";
			this.btn3.RightToLeft = RightToLeft.No;
			this.btn3.Size = new Size(31, 17);
			this.btn3.TabIndex = 10;
			this.btn3.TabStop = true;
			this.btn3.Text = "3";
			this.btn3.UseVisualStyleBackColor = true;
			this.btn3.CheckedChanged += this.btnGradient_CheckedChanged;
			this.btn4.AutoSize = true;
			this.btn4.Location = new Point(152, 10);
			this.btn4.Margin = new Padding(2);
			this.btn4.Name = "btn4";
			this.btn4.RightToLeft = RightToLeft.No;
			this.btn4.Size = new Size(31, 17);
			this.btn4.TabIndex = 11;
			this.btn4.TabStop = true;
			this.btn4.Text = "4";
			this.btn4.UseVisualStyleBackColor = true;
			this.btn4.CheckedChanged += this.btnGradient_CheckedChanged;
			this.btn5.AutoSize = true;
			this.btn5.Location = new Point(182, 10);
			this.btn5.Margin = new Padding(2);
			this.btn5.Name = "btn5";
			this.btn5.RightToLeft = RightToLeft.No;
			this.btn5.Size = new Size(31, 17);
			this.btn5.TabIndex = 12;
			this.btn5.TabStop = true;
			this.btn5.Text = "5";
			this.btn5.UseVisualStyleBackColor = true;
			this.btn5.CheckedChanged += this.btnGradient_CheckedChanged;
			this.traceButton.Location = new Point(183, 42);
			this.traceButton.Name = "traceButton";
			this.traceButton.Size = new Size(54, 23);
			this.traceButton.TabIndex = 15;
			this.traceButton.Text = "Trace";
			this.toolTip.SetToolTip(this.traceButton, "Change spectrum trace color");
			this.traceButton.UseVisualStyleBackColor = true;
			this.traceButton.Click += this.traceButton_Click;
			this.backgroundButton.ForeColor = Color.White;
			this.backgroundButton.Location = new Point(245, 42);
			this.backgroundButton.Name = "backgroundButton";
			this.backgroundButton.Size = new Size(53, 23);
			this.backgroundButton.TabIndex = 17;
			this.backgroundButton.Text = "B-gnd";
			this.toolTip.SetToolTip(this.backgroundButton, "Change background color");
			this.backgroundButton.UseVisualStyleBackColor = true;
			this.backgroundButton.Click += this.backgroundButton_Click;
			this.cmbFill.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbFill.FormattingEnabled = true;
			this.cmbFill.Items.AddRange(new object[4]
			{
				"none\t",
				"static",
				"dyn-1",
				"dyn-2"
			});
			this.cmbFill.Location = new Point(330, 45);
			this.cmbFill.Name = "cmbFill";
			this.cmbFill.Size = new Size(49, 21);
			this.cmbFill.TabIndex = 18;
			this.toolTip.SetToolTip(this.cmbFill, "Select SA fill type.");
			this.cmbFill.SelectedIndexChanged += this.cmbFill_SelectedIndexChanged;
			this.labFill.AutoSize = true;
			this.labFill.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.labFill.Location = new Point(305, 46);
			this.labFill.Name = "labFill";
			this.labFill.Size = new Size(28, 16);
			this.labFill.TabIndex = 19;
			this.labFill.Text = "Fill:";
			this.label2.AutoSize = true;
			this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label2.Location = new Point(5, 10);
			this.label2.Name = "label2";
			this.label2.Size = new Size(46, 16);
			this.label2.TabIndex = 20;
			this.label2.Text = "Select";
			this.picBox.BackColor = SystemColors.ButtonHighlight;
			this.picBox.BackgroundImageLayout = ImageLayout.Center;
			this.picBox.Cursor = Cursors.Default;
			this.picBox.Enabled = false;
			this.picBox.InitialImage = null;
			this.picBox.Location = new Point(181, 96);
			this.picBox.Name = "picBox";
			this.picBox.Size = new Size(200, 177);
			this.picBox.TabIndex = 22;
			this.picBox.TabStop = false;
			this.toolTip.SetToolTip(this.picBox, "Pick/move color or gradient");
			this.picBox.MouseDown += this.picBox_MouseDown;
			this.picBox.MouseMove += this.picBox_MouseMove;
			this.picBox.MouseUp += this.picBox_MouseUp;
			this.groupBox1.Controls.Add(this.fastButton);
			this.groupBox1.Controls.Add(this.colorListBox);
			this.groupBox1.Controls.Add(this.gradientPictureBox);
			this.groupBox1.Controls.Add(this.upButton);
			this.groupBox1.Controls.Add(this.downButton);
			this.groupBox1.Controls.Add(this.addButton);
			this.groupBox1.Controls.Add(this.deleteButton);
			this.groupBox1.FlatStyle = FlatStyle.System;
			this.groupBox1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.groupBox1.Location = new Point(2, 35);
			this.groupBox1.Margin = new Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new Padding(2);
			this.groupBox1.Size = new Size(172, 240);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Edit color gradient";
			this.fastButton.Location = new Point(105, 23);
			this.fastButton.Name = "fastButton";
			this.fastButton.Size = new Size(63, 23);
			this.fastButton.TabIndex = 8;
			this.fastButton.Text = "Fast edit";
			this.toolTip.SetToolTip(this.fastButton, "Edit whole gradient");
			this.fastButton.UseVisualStyleBackColor = true;
			this.fastButton.Click += this.fastButton_Click;
			this.saButton.Location = new Point(7, 280);
			this.saButton.Name = "saButton";
			this.saButton.Size = new Size(43, 20);
			this.saButton.TabIndex = 24;
			this.saButton.Text = "Spect";
			this.toolTip.SetToolTip(this.saButton, "Change SpectrumAnalyzer colors");
			this.saButton.UseVisualStyleBackColor = true;
			this.saButton.Click += this.saButton_Click;
			this.wfButton.Location = new Point(57, 280);
			this.wfButton.Name = "wfButton";
			this.wfButton.Size = new Size(43, 20);
			this.wfButton.TabIndex = 25;
			this.wfButton.Text = "W-fall";
			this.toolTip.SetToolTip(this.wfButton, "Change Waterfall colors");
			this.wfButton.UseVisualStyleBackColor = true;
			this.wfButton.Click += this.wfButton_Click;
			this.agButton.Location = new Point(106, 280);
			this.agButton.Name = "agButton";
			this.agButton.Size = new Size(43, 20);
			this.agButton.TabIndex = 25;
			this.agButton.Text = "Audio";
			this.toolTip.SetToolTip(this.agButton, "Change Audiogram colors");
			this.agButton.UseVisualStyleBackColor = true;
			this.agButton.Click += this.agButton_Click;
			this.toolTip.AutoPopDelay = 2000;
			this.toolTip.InitialDelay = 500;
			this.toolTip.ReshowDelay = 100;
			this.trackBar.AutoSize = false;
			this.trackBar.Location = new Point(181, 79);
			this.trackBar.Margin = new Padding(2);
			this.trackBar.Maximum = 100;
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new Size(200, 20);
			this.trackBar.TabIndex = 39;
			this.trackBar.TickFrequency = 10;
			this.trackBar.TickStyle = TickStyle.None;
			this.trackBar.Value = 100;
			this.trackBar.ValueChanged += this.trackBar_ValueChanged;
			this.okButton.DialogResult = DialogResult.OK;
			this.okButton.Enabled = false;
			this.okButton.Location = new Point(242, 280);
			this.okButton.Name = "okButton";
			this.okButton.Size = new Size(54, 20);
			this.okButton.TabIndex = 40;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += this.saveButton_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new Size(388, 306);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.trackBar);
			base.Controls.Add(this.agButton);
			base.Controls.Add(this.wfButton);
			base.Controls.Add(this.saButton);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.picBox);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.cmbFill);
			base.Controls.Add(this.backgroundButton);
			base.Controls.Add(this.traceButton);
			base.Controls.Add(this.btn5);
			base.Controls.Add(this.btn4);
			base.Controls.Add(this.btn3);
			base.Controls.Add(this.btn2);
			base.Controls.Add(this.btn1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.labFill);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "GradientDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Color Editor";
			this.toolTip.SetToolTip(this, "Accept/save changes");
			base.FormClosing += this.GradientDialog_FormClosing;
			((ISupportInitialize)this.gradientPictureBox).EndInit();
			((ISupportInitialize)this.picBox).EndInit();
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.trackBar).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
