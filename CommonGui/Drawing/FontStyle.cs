using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonGui.Drawing
{
	[Flags]
	public enum FontStyle
	{
		Regular = 0,
		Bold = 1,
		Italic = 2,
		Underline = 4,
		Strikeout = 8,
	}
}
