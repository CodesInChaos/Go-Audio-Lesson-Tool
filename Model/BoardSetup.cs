using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Model
{
	public class BoardSetup
	{
		private readonly int mWidth;
		private readonly int mHeight;

		public int Width { get { return mWidth; } }
		public int Height { get { return mHeight; } }

		public BoardSetup(int width, int height)
		{
			if (width < 1)
				throw new ArgumentException("Width<1");
			if (height < 1)
				throw new ArgumentException("Height<1");
			mWidth = width;
			mHeight = height;
		}

		public bool IsPositionValid(Position p)
		{
			return ((p.X >= 0) && (p.Y >= 0) && (p.X < Width) && (p.Y < Height));
		}

		public IEnumerable<Position> Neighbours(Position p)
		{
			Debug.Assert(IsPositionValid(p));
			if (p.X > 0)
				yield return new Position(p.X - 1, p.Y);
			if (p.Y > 0)
				yield return new Position(p.X, p.Y - 1);
			if (p.Y < Height - 1)
				yield return new Position(p.X, p.Y + 1);
			if (p.X < Width - 1)
				yield return new Position(p.X + 1, p.Y);
		}

		public IEnumerable<Position> AllPositions
		{
			get
			{
				for (int y = 0; y < Height; y++)
					for (int x = 0; x < Width; x++)
						yield return new Position(x, y);
			}
		}
	}
}
