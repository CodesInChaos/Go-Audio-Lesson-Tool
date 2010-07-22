using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoClient.Drawing
{
	public class GraphicsSystem:CommonGui.Drawing.GraphicsSystem
	{
		public override CommonGui.Drawing.Bitmap CreateBitmap(int width, int height)
		{
			return new Bitmap(width, height);
		}
	}
}
