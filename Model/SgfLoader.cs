using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Model
{
	public class SgfLoader
	{
		private Stack<int> variationCheckpoints = new Stack<int>();
		string s;
		int i;
		HashSet<char?> spaces = new HashSet<char?>(new char?[] { '\t', ' ', '\r', '\n' });
		Replay replay;
		int pos { get { return replay.Actions.Count - 1; } }

		char? c
		{
			get
			{
				if (i < s.Length)
					return s[i];
				else
					return null;
			}
		}

		void SkipSpaces()
		{
			while (spaces.Contains(c))
				i++;
		}

		void ParseElement()
		{
			SkipSpaces();
			if (c == null)
				return;
			if (c == '(')
			{
				variationCheckpoints.Push(pos);
				i++;
			}
			else if (c == ')')
			{
				replay.AddAction(new SelectStateAction(pos));
				i++;
			}
			else if (c == ';')
			{
				i++;
			}
			else if (Char.IsUpper((char)c))
				ParseProperty();
			else throw new InvalidDataException("Unknown char" + (char)c + " at " + i);
		}

		private string ParseValue()
		{
			SkipSpaces();
			string value = "";
			if (c != '[')
				return null;
			i++;
			while (c != ']')
			{
				if (c == '\\')
					i++;
				if (c == null)
					throw new InvalidDataException("Unexpected end of file at " + i);
				value += c;
				i++;
			}
			i++;
			return value;
		}

		private void ParseProperty()
		{
			string name = "";
			while (Char.IsUpper((char)c))
			{
				name += c;
				i++;
			}
			List<string> values = new List<string>();
			string value;
			while ((value = ParseValue()) != null)
				values.Add(value);
			ProcessProperty(name, values);
		}

		private void ProcessProperty(string name, List<string> values)
		{
			switch (name)
			{
				case "B":
					if (values[0] == "")
						replay.AddAction(new PassMoveAction(StoneColor.Black));
					else
						replay.AddAction(new StoneMoveAction(Position.Parse(values[0]), StoneColor.Black));
					break;
				case "W":
					if (values[0] == "")
						replay.AddAction(new PassMoveAction(StoneColor.White));
					else
						replay.AddAction(new StoneMoveAction(Position.Parse(values[0]), StoneColor.White));
					break;
				case "SZ":
					replay.AddAction(new InitStateAction(int.Parse(values[0]), int.Parse(values[0])));
					break;
			}
		}

		public static void Load(string sgf, Replay replay)
		{
			SgfLoader loader = new SgfLoader();
			loader.s = sgf;
			loader.replay = replay;
			while (loader.c != null)
				loader.ParseElement();
		}
	}
}
