using System.Drawing;
using System.Drawing.Drawing2D;

namespace SDRSharp.Controls
{
	public abstract class RoundedRectangle
	{
		public enum RectangleCorners
		{
			None,
			TopLeft,
			TopRight,
			BottomLeft = 4,
			BottomRight = 8,
			All = 0xF
		}

		public static GraphicsPath Create(int x, int y, int width, int height, int radius, int radiusB = 0)
		{
			int num = x + width;
			int num2 = y + height;
			if (radiusB == 0)
			{
				radiusB = radius;
			}
			int x2 = num - radius;
			int x3 = x + radius;
			int num3 = y + radius;
			int num4 = radius * 2;
			int x4 = num - num4;
			int x5 = num - radiusB;
			int num5 = num2 - radiusB;
			int x6 = x + radiusB;
			int num6 = radiusB * 2;
			int x7 = num - num6;
			int y2 = num2 - num6;
			RectangleCorners rectangleCorners = RectangleCorners.All;
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.StartFigure();
			if ((RectangleCorners.TopLeft & rectangleCorners) == RectangleCorners.TopLeft)
			{
				graphicsPath.AddArc(x, y, num4, num4, 180f, 90f);
			}
			graphicsPath.AddLine(x3, y, x2, y);
			if ((RectangleCorners.TopRight & rectangleCorners) == RectangleCorners.TopRight)
			{
				graphicsPath.AddArc(x4, y, num4, num4, 270f, 90f);
			}
			graphicsPath.AddLine(num, num3, num, num5);
			if ((RectangleCorners.BottomRight & rectangleCorners) == RectangleCorners.BottomRight)
			{
				graphicsPath.AddArc(x7, y2, num6, num6, 0f, 90f);
			}
			graphicsPath.AddLine(x5, num2, x6, num2);
			if ((RectangleCorners.BottomLeft & rectangleCorners) == RectangleCorners.BottomLeft)
			{
				graphicsPath.AddArc(x, y2, num6, num6, 90f, 90f);
			}
			graphicsPath.AddLine(x, num5, x, num3);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		public static GraphicsPath Create(Rectangle rect, int radius, int radiusB)
		{
			return RoundedRectangle.Create(rect.X, rect.Y, rect.Width, rect.Height, radius, radiusB);
		}
	}
}
