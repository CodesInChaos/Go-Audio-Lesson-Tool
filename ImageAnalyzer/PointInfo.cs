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

	public static class MarkerHelper
	{
		public static string MarkerToString(Marker marker)
		{
			switch (marker)
			{
				case Marker.Unknown:
					return "#Unknown";
				case Marker.None:
					return "";
				case Marker.Square:
					return "#SQ";
				case Marker.Circle:
					return "#CI";
				case Marker.Triangle:
					return "#TR";
				default:
					throw new NotImplementedException();
			}
		}

		public static Marker StringToMarker(string s)
		{
			switch (s)
			{
				case "#SQ":
					return Marker.Square;
				case "#CI":
					return Marker.Circle;
				case "#TR":
					return Marker.Triangle;
				default:
					return Marker.None;
			}
		}
	}

	public struct PointInfo
	{
		public StoneColor StoneColor;
		public StoneColor SmallStoneColor;
		public Marker Marker;

		public string Label { get { return MarkerHelper.MarkerToString(Marker); } }
	}
}
