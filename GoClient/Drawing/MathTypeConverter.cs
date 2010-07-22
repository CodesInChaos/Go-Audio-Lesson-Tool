using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Image;
using Chaos.Util.Mathematics;

namespace GoClient
{
	public static class MathTypeConverter
	{
		public static System.Drawing.Color Convert(this RawColor color)
		{
			return System.Drawing.Color.FromArgb((int)color.ARGB);
		}

		public static System.Drawing.RectangleF Convert(this RectangleF rect)
		{
			return new System.Drawing.RectangleF(rect.Left, rect.Top, rect.Width, rect.Height);
		}

		public static System.Drawing.Point Convert(this Vector2i v)
		{
			return new System.Drawing.Point(v.X, v.Y);
		}

		public static System.Drawing.PointF Convert(this Vector2f v)
		{
			return new System.Drawing.PointF(v.X, v.Y);
		}

		//Drawing=>ChaosUtil
		public static Vector2i Convert(this System.Drawing.Point p)
		{
			return new Vector2i(p.X, p.Y);
		}

		public static Vector2i Convert(this System.Drawing.Size s)
		{
			return new Vector2i(s.Width, s.Height);
		}

		public static Vector2f Convert(this System.Drawing.SizeF s)
		{
			return new Vector2f(s.Width, s.Height);
		}

		public static RectangleF Convert(this System.Drawing.RectangleF rect)
		{
			return RectangleF.FromLTWH(rect.Left, rect.Top, rect.Width, rect.Height);
		}

		public static RectangleI Convert(this System.Drawing.Rectangle rect)
		{
			return RectangleI.FromLTWH(rect.Left, rect.Top, rect.Width, rect.Height);
		}
	}
}
