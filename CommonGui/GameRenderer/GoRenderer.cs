using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using CommonGui.Drawing;
using Chaos.Util.Mathematics;
using Chaos.Image;

namespace CommonGui.GameRenderer
{
	public class GoRenderer
	{
		public readonly GraphicsSystem GraphicsSystem;
		public Graphics Graphics;
		public static readonly RawColor BackgroundColor = RawColor.FromRGB(254, 214, 121);

		public GoRenderer(GraphicsSystem graphicsSystem)
		{
			GraphicsSystem = graphicsSystem;
		}

		public void DrawString(string s, Font font, RawColor color, Vector2f point, Vector2f align)
		{
			Vector2f size = Graphics.MeasureString(s, font);
			Vector2f newPoint = new Vector2f(point.X - size.X * align.X, point.Y - size.Y * align.Y);
			Graphics.DrawString(s, font, color, newPoint);
		}

		public void RenderVariationMarker(Vector2f center, float diameter, bool active)
		{
			float radius = diameter / 2;
			Pen pen;
			if (active)
				pen = new Pen(RawColor.Blue, 2);
			else
				pen = new Pen(RawColor.Blue);
			Graphics.DrawCircle(pen, center, radius);
		}

		public void RenderCurrentMoveMarker(Vector2f center, float diameter)
		{
			float radius = diameter / 2;
			Graphics.DrawCircle(new Pen(RawColor.Red, 2), center, radius);
		}

		public void RenderStone(Vector2f center, float diameter, StoneColor color)
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
						Graphics.FillCircle(RawColor.Black, center, radius);
						break;
					}
				case StoneColor.White:
					{
						Graphics.FillCircle(RawColor.White, center, radius);
						Graphics.DrawCircle(new Pen(RawColor.Black, 1), center, radius);
						break;
					}
				default:
					throw new NotImplementedException();
			}
		}
	}
}
