using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;
using Chaos.Image;

namespace GoClient.Drawing
{
	public class Graphics : CommonGui.Drawing.Graphics
	{


		private static System.Drawing.Pen CreatePen(CommonGui.Drawing.Pen pen)
		{
			return new System.Drawing.Pen(pen.Color.Convert(), pen.LineWidth);
		}

		private static System.Drawing.Brush CreateBrush(RawColor color)
		{
			return new System.Drawing.SolidBrush(color.Convert());
		}

		private static System.Drawing.Font CreateFont(CommonGui.Drawing.Font font)
		{
			System.Drawing.FontStyle style = (System.Drawing.FontStyle)(int)font.Style;
			return new System.Drawing.Font(font.Name, font.PixelSize, style, System.Drawing.GraphicsUnit.Pixel);
		}


		public readonly System.Drawing.Graphics InternalGraphics;

		public override Chaos.Util.Mathematics.Vector2i Size
		{
			get
			{
				return InternalGraphics.ClipBounds.Size.Convert().RoundToInt();
			}
		}

		public override void DrawCircle(CommonGui.Drawing.Pen pen, Vector2f center, float radius)
		{
			using (System.Drawing.Pen pen2 = CreatePen(pen))
			{
				InternalGraphics.DrawEllipse(pen2, center.X - radius, center.Y - radius, radius * 2, radius * 2);
			}
		}

		public override void FillCircle(RawColor color, Vector2f center, float radius)
		{
			using (System.Drawing.Brush brush2 = CreateBrush(color))
			{
				InternalGraphics.FillEllipse(brush2, center.X - radius, center.Y - radius, radius * 2, radius * 2);
			}
		}

		public override void FillRectangle(Chaos.Image.RawColor color, Chaos.Util.Mathematics.RectangleF rect)
		{
			using (System.Drawing.Brush brush2 = CreateBrush(color))
			{
				InternalGraphics.FillRectangle(brush2, rect.Convert());
			}
		}

		public override Vector2f MeasureString(string text, CommonGui.Drawing.Font font)
		{
			using (System.Drawing.Font font2 = CreateFont(font))
			{
				System.Drawing.SizeF size = InternalGraphics.MeasureString(text, font2);
				return new Vector2f(size.Width, size.Height);
			}
		}

		public override void DrawString(string text, CommonGui.Drawing.Font font, Chaos.Image.RawColor color, Vector2f position)
		{
			using (System.Drawing.Font font2 = CreateFont(font))
			{
				using (System.Drawing.Brush brush = CreateBrush(color))
				{
					InternalGraphics.DrawString(text, font2, brush, new System.Drawing.PointF(position.X, position.Y));
				}
			}
		}

		public override void DrawImage(CommonGui.Drawing.Bitmap bmp, Vector2i position)
		{
			InternalGraphics.DrawImage(((Bitmap)bmp).InternalBitmap, position.Convert());
		}

		public override void DrawLine(CommonGui.Drawing.Pen pen, Chaos.Util.Mathematics.Vector2f p1, Chaos.Util.Mathematics.Vector2f p2)
		{
			using (System.Drawing.Pen pen2 = CreatePen(pen))
			{
				InternalGraphics.DrawLine(pen2, p1.Convert(), p2.Convert());
			}
		}

		public Graphics(System.Drawing.Graphics internalGraphics)
		{
			InternalGraphics = internalGraphics;
		}

		public override void DrawPolygon(CommonGui.Drawing.Pen pen, params Vector2f[] points)
		{
			System.Drawing.PointF[] points2 = points.Select(p => new System.Drawing.PointF(p.X, p.Y)).ToArray();
			using (System.Drawing.Pen pen2 = CreatePen(pen))
			{
				InternalGraphics.DrawPolygon(pen2, points2);
			}
		}

		public override void DrawRectangle(CommonGui.Drawing.Pen pen, RectangleF rect)
		{
			using (System.Drawing.Pen pen2 = CreatePen(pen))
			{
				InternalGraphics.DrawRectangle(pen2, rect.Left, rect.Top, rect.Width, rect.Height);
			}
		}
	}
}
