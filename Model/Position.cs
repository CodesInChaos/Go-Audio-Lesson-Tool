using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.TreeDocuments;

namespace Model
{
	public struct Position
	{
		public byte X { get; private set; }
		public byte Y { get; private set; }
		public Position(int x, int y)
			: this()
		{
			X = (byte)x;
			Y = (byte)y;
		}

		private static char letterOfPos(int i)
		{
			return (char)((int)'a' + i);
		}

		private static int PosOfLetter(char c)
		{
			int result = (int)(Char.ToUpper(c)) - (int)'A';
			if (result >= 0 && result < 26)
				return result;
			else
				throw new ArgumentException("Invalid Position");
		}

		public override string ToString()
		{
			return "" + letterOfPos(X) + letterOfPos(Y);
		}

		public static Position Parse(string s)
		{
			if (s.Length != 2)
				throw new ArgumentException("Invalid Position");
			return new Position(PosOfLetter(s[0]), PosOfLetter(s[1]));
		}

		public static explicit operator Position(TreeDoc td)
		{
			return Parse((string)td);
		}

		public static bool operator ==(Position p1, Position p2)
		{
			return (p1.X == p2.X) && (p1.Y == p2.Y);
		}

		public static bool operator !=(Position p1, Position p2)
		{
			return !(p1 == p2);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			return this == (Position)obj;
		}

		// override object.GetHashCode
		public override int GetHashCode()
		{
			return (X + 256 * Y);
		}

		/*public string SgfString()
		{
			return ToString().ToLower();
		}*/
	}

	public class Positions
	{
		private Position[] mPositions;
		public IEnumerable<Position> GetPositions(BoardSetup setup)
		{
			if (mPositions != null)
				return mPositions.Select(x => x);
			else
			{
				return setup.AllPositions;
			}
		}

		private Positions(Position[] positions)
		{
			mPositions = positions;
		}

		public TreeDoc ToTreeDoc()
		{
			if (mPositions == null)
				return TreeDoc.CreateLeaf("*");
			else if (mPositions.Length == 1)
				return TreeDoc.CreateLeaf(mPositions[0].ToString());
			else
				return TreeDoc.CreateListRange("", mPositions);
		}

		public override string ToString()
		{
			return ToTreeDoc().ToString();
		}

		public static implicit operator Positions(Position p)
		{
			return Positions.FromList(p);
		}

		public readonly static Positions All = new Positions(null);

		public static Positions FromList(IEnumerable<Position> positions)
		{
			return new Positions(positions.ToArray());
		}

		public static Positions FromList(params Position[] positions)
		{
			return new Positions(positions.ToArray());
		}

		public static Positions Parse(TreeDoc doc)
		{
			if (doc.Value == "*")
				return Positions.All;
			if (doc.Value != null)
				return Position.Parse(doc.Value);
			else
				return Positions.FromList(doc.Elements("").Select(td => (Position)td));
		}
	}
}
