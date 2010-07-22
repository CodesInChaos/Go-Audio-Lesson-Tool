using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;

namespace CommonGui.Drawing
{
	public abstract class Bitmap : IDisposable
	{
		public abstract Vector2i Size { get; }
		public int Width { get { return Size.X; } }
		public int Height { get { return Size.Y; } }
		public abstract Graphics CreateGraphics();
		public abstract Bitmap Resize(int width, int height);
		public abstract void Dispose();
	}
}
