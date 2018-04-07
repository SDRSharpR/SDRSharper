using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.FrequencyManager
{
    public class CustomRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected)
            {
                base.OnRenderButtonBackground(e);
            }
            else
            {
                Rectangle rectangle = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height - 1);

                var brush = new SolidBrush(Color.FromArgb(255, (byte)45, (byte)45, (byte)48));
                var pen = new Pen(Color.FromArgb(255, (byte)45, (byte)45, (byte)48));

                e.Graphics.FillRectangle(brush, rectangle);
                e.Graphics.DrawRectangle(pen, rectangle);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item.Bounds.Contains(e.ToolStrip.PointToClient(Cursor.Position)))
            {
                e.TextColor = Color.Lime;
            }

            base.OnRenderItemText(e);
        }
    }
}
