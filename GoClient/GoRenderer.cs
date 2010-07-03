using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Model;

namespace GoClient
{
	public class GoRenderer
	{
		public Graphics Graphics;

		public void DrawString(string s, Font font, Brush brush, PointF point, PointF align)
		{
			SizeF size = Graphics.MeasureString(s, font);
			PointF newPoint = new PointF(point.X - size.Width * align.X, point.Y - size.Height * align.Y);
			Graphics.DrawString(s, font, brush, newPoint);
		}

		public void RenderStone(PointF center, float diameter, StoneColor color)
		{
			float radius = diameter / 2;
			switch (color)
			{
				case StoneColor.None:
					{
						break;
					}
				case StoneColor.Black:
					{
						Graphics.FillEllipse(Brushes.Black, center.X - radius, center.Y - radius, diameter, diameter);
						break;
					}
				case StoneColor.White:
					{
						Graphics.FillEllipse(Brushes.White, center.X - radius, center.Y - radius, diameter, diameter);
						Graphics.DrawEllipse(Pens.Black, center.X - radius, center.Y - radius, diameter, diameter);
						break;
					}
				default:
					throw new NotImplementedException();
			}
		}
	}
}
