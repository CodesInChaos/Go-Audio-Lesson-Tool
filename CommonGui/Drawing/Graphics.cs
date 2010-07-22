using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;
using Chaos.Image;

namespace CommonGui.Drawing
{
	public abstract class Graphics
	{
		public abstract Vector2i Size { get; }
		public int Width { get { return Size.X; } }
		public int Height { get { return Size.Y; } }

		public abstract void DrawCircle(Pen pen, Vector2f center, float radius);
		public abstract void DrawRectangle(Pen pen, RectangleF rect);
		public abstract void DrawLine(Pen pen, Vector2f p1, Vector2f p2);
		public abstract void DrawPolygon(Pen pen, params Vector2f[] points);

		public abstract void FillCircle(RawColor color, Vector2f center, float radius);
		public abstract void FillRectangle(RawColor color, RectangleF rect);

		public abstract Vector2f MeasureString(string text, Font font);
		public abstract void DrawString(string text, Font font, RawColor color, Vector2f position);
		public abstract void DrawImage(Bitmap bmp, Vector2i position);
	}
}
