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
}
