using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Image;

namespace CommonGui.Drawing
{
	public struct Pen
	{
		public RawColor Color { get; private set; }
		public float LineWidth { get; private set; }

		public Pen(RawColor color)
			: this(color, 1)
		{
		}

		public Pen(RawColor color, float lineWidth)
			:this()
		{
			Color = color;
			LineWidth = lineWidth;
		}
	}
}
