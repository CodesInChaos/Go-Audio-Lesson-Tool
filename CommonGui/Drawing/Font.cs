using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonGui.Drawing
{
	public class Font
	{
		public string Name { get; private set; }
		public float PixelSize { get; private set; }
		public FontStyle Style { get; private set; }

		public Font(string name, float pixelSize, FontStyle style)
		{
			Name = name;
			PixelSize = pixelSize;
			Style = style;
		}

		public Font(string name, float pixelSize)
			: this(name, pixelSize, FontStyle.Regular)
		{

		}
	}
}
