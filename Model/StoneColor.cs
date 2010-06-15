using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.TreeDocuments;

namespace Model
{
	public enum StoneColor
	{
		None,
		White,
		Black,
	}

	public static class StoneColorHelper
	{
		public static StoneColor Invert(this StoneColor color)
		{
			switch (color)
			{
				case StoneColor.None:
					return StoneColor.None;
				case StoneColor.White:
					return StoneColor.Black;
				case StoneColor.Black:
					return StoneColor.White;
				default:
					throw new NotImplementedException();
			}
		}

		public static string ShortName(this StoneColor color)
		{
			if (color == StoneColor.Black)
				return "B";
			else if (color == StoneColor.White)
				return "W";
			else if (color == StoneColor.None)
				return "-";
			else
				throw new ArgumentException();
		}

		public static bool TryParse(string s,out StoneColor color)
		{
			switch (s)
			{
				case "B":
					color = StoneColor.Black;
					return true;
				case "W":
					color = StoneColor.White;
					return true;
				case "-":
					color = StoneColor.None;
					return true;
				default:
					color = StoneColor.None;
					return false;
			}
		}

		public static StoneColor Parse(string s)
		{
			switch (s)
			{
				case "B":
					return StoneColor.Black;
				case "W":
					return StoneColor.White;
				case "-":
					return StoneColor.None;
				default:
					throw new ArgumentException(s + " is no valid StoneColor");
			}
		}

		public static StoneColor Parse(TreeDoc td)
		{
			return Parse((string)td);
		}
	}
}
