using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonGui.Drawing
{
	public abstract class GraphicsSystem
	{
		public abstract Bitmap CreateBitmap(int width, int height);
	}
}
