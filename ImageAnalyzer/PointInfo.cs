using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace ImageAnalyzer
{
	public enum Marker
	{
		None,
		Unknown,
		Square,
		Circle,
		Triangle
	}

	public struct PointInfo
	{
		public StoneColor StoneColor;
		public StoneColor SmallStoneColor;
		public Marker Marker;
	}
}
