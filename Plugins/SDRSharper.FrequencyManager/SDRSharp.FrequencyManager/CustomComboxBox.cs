using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.FrequencyManager
{
    class CustomComboxBox : ComboBox
    {
        private Color _borderColor = Color.FromArgb(45, 45, 48);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private static int WM_PAINT = 0x000F;

        new public DrawMode DrawMode { get; set; }
        public Color HighlightColor { get; set; }
        
        public CustomComboxBox()
        {
            base.DrawMode = DrawMode.OwnerDrawFixed;
            this.HighlightColor = Color.FromArgb(64, 64, 64);
            this.DrawItem += new DrawItemEventHandler(CustomComboxBox_DrawItem);
        }

        public void CustomComboxBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox box = ((ComboBox)sender);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(HighlightColor), e.Bounds);
                }

                else { e.Graphics.FillRectangle(new SolidBrush(box.BackColor), e.Bounds); }

                e.Graphics.DrawString(box.Items[e.Index].ToString(),
                     e.Font, new SolidBrush(box.ForeColor),
                     new Point(e.Bounds.X, e.Bounds.Y));
                e.DrawFocusRectangle();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                Rectangle bounds = new Rectangle(0, 0, Width, Height);
                ControlPaint.DrawBorder(g, bounds, _borderColor, _borderStyle);
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate(); // causes control to be redrawn
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

    }
}
