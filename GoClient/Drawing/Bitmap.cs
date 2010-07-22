using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;

namespace GoClient.Drawing
{
	public class Bitmap : CommonGui.Drawing.Bitmap
	{
		public readonly System.Drawing.Bitmap InternalBitmap;

		public override CommonGui.Drawing.Graphics CreateGraphics()
		{
			var result = new Graphics(System.Drawing.Graphics.FromImage(InternalBitmap));
			//graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			result.InternalGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			result.InternalGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			return result;
		}

		public override CommonGui.Drawing.Bitmap Resize(int width, int height)
		{
			throw new NotImplementedException();
		}

		public override void Dispose()
		{
			InternalBitmap.Dispose();
		}

		public override Vector2i Size { get { return new Vector2i(InternalBitmap.Width, InternalBitmap.Height); } }

		public Bitmap(int width, int height)
		{
			InternalBitmap = new System.Drawing.Bitmap(width, height);
		}
	}
}
