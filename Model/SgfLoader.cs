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
		private int boardSize = 0;
		private bool newNode = false;
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
				replay.AddAction(new SelectStateAction(variationCheckpoints.Pop()));
				i++;
			}
			else if (c == ';')
			{
				if (boardSize > 0)
				{
					newNode = true;
				}
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
				case "W":
					StoneColor color = StoneColor.Black;
					if (name == "B")
						color = StoneColor.Black;
					if (name == "W")
						color = StoneColor.White;
					string position = values[0];
					bool isOldStylePass = (position == "tt" && boardSize <= 19);
					if (position == "" || isOldStylePass)
						replay.AddAction(new PassMoveAction(color));
					else
						replay.AddAction(new StoneMoveAction(Position.Parse(position), color));
					break;
				case "SZ":
					boardSize = int.Parse(values[0]);
					replay.AddAction(new CreateBoardAction(boardSize, boardSize));
					break;
				case "AB":
					foreach (string pos in values)
						replay.AddAction(new SetStoneAction(Position.Parse(pos), StoneColor.Black));
					break;
				case "AW":
					foreach (string pos in values)
						replay.AddAction(new SetStoneAction(Position.Parse(pos), StoneColor.White));
					break;
				case "AE":
					foreach (string pos in values)
						replay.AddAction(new SetStoneAction(Position.Parse(pos), StoneColor.None));
					break;
				case "SQ":
					foreach (string pos in values)
						replay.AddAction(new LabelAction(Position.Parse(pos), "#SQ"));
					break;
				case "TR":
					foreach (string pos in values)
						replay.AddAction(new LabelAction(Position.Parse(pos), "#TR"));
					break;
				case "CR":
					foreach (string pos in values)
						replay.AddAction(new LabelAction(Position.Parse(pos), "#CR"));
					break;
				case "MA":
					foreach (string pos in values)
						replay.AddAction(new LabelAction(Position.Parse(pos), "#MA"));
					break;
				case "LB":
					foreach (string pos in values)
					{
						string[] parts = pos.Split(':');
						replay.AddAction(new LabelAction(Position.Parse(parts[0]), parts[1]));
					}
					break;
			}
			if (newNode)
			{
				replay.AddAction(new LabelAction(Positions.All, ""));
				newNode = false;
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
