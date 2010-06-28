using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageAnalyzer
{
	public class BoardInfo
	{
		public readonly PointInfo[,] Board;
		public readonly int Width;
		public readonly int Height;
		public TimeSpan ProcessDuration;

		public BoardInfo(int width, int height)
		{
			Width = width;
			Height = height;
			Board = new PointInfo[width, height];
		}
	}
}
