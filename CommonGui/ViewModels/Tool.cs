using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace CommonGui.ViewModels
{
	public abstract class Tool
	{
		public abstract IEnumerable<GameAction> Click(Game game, int commandID, Position p);
	}

	public class EditTool : Tool
	{
		public override IEnumerable<GameAction> Click(Game game, int commandID, Position p)
		{
			StoneColor oldColor = game.State.Stones[p.X, p.Y];
			StoneColor newColor;
			if (commandID == 1)
				newColor = StoneColor.Black;
			else if (commandID == 2)
				newColor = StoneColor.White;
			else
				newColor = StoneColor.None;
			if (newColor == oldColor)
				newColor = StoneColor.None;
			yield return new SetStoneAction(p, newColor);
		}
	}

	public abstract class LabelTool : Tool
	{
		protected abstract string getLabel(Game game, int commandID, HashSet<String> existingLabels, string oldLabel);

		public override IEnumerable<GameAction> Click(Game game, int actionIndex, Position p)
		{
			string oldLabel = game.State.Labels[p.X, p.Y];
			string newLabel;

			HashSet<string> existingLabels = new HashSet<string>();
			foreach (string s in game.State.Labels)
			{
				existingLabels.Add(s);
			}

			newLabel = getLabel(game, actionIndex, existingLabels, oldLabel);
			if (newLabel == null)
				throw new InvalidOperationException();
			yield return new LabelAction(p, newLabel);
		}
	}

	public class TextLabelTool : LabelTool
	{
		protected override string getLabel(Game game, int commandID, HashSet<String> existingLabels, string oldLabel)
		{
			if (!String.IsNullOrEmpty(oldLabel))
				return "";
			string s;
			//if (actionIndex == 1)
			{
				int i = 0;
				do
				{
					s = ((char)((int)'A' + i % 26)).ToString();
					if (i > 25)
						s += (i / 26 + 1).ToString();
					i++;
				} while (existingLabels.Contains(s));
				return s;
			}
			/*else
			{
				throw new NotImplementedException();
			}*/
		}
	}

	public class NumberLabelTool : LabelTool
	{
		protected override string getLabel(Game game, int commandID, HashSet<string> existingLabels, string oldLabel)
		{
			if (!String.IsNullOrEmpty(oldLabel))
				return "";
			int i = 1;
			string s;
			do
			{
				s = i.ToString();
				i++;
			} while (existingLabels.Contains(s));
			return s;
		}
	}

	public class ConstantLabelTool : LabelTool
	{
		public readonly string Label1;
		public readonly string Label2;
		public readonly string Label3;
		protected override string getLabel(Game game, int commandID, HashSet<string> existingLabels, string oldLabel)
		{
			if (!String.IsNullOrEmpty(oldLabel))
				return "";
			string newLabel;
			if (commandID == 1)
				newLabel = Label1;
			else if (commandID == 2)
				newLabel = Label2;
			else if (commandID == 3)
				newLabel = Label3;
			else
				newLabel = "";
			if (newLabel == oldLabel)
				newLabel = "";
			return newLabel;
		}

		public ConstantLabelTool(string label1, string label2, string label3)
		{
			Label1 = label1;
			Label2 = label2;
			Label3 = label3;
		}
	}

	public class MoveTool : Tool
	{
		public override IEnumerable<GameAction> Click(Game game, int commandID, Position p)
		{
			if (commandID == 1)
			{
				if (game.State.Stones[p.X, p.Y] == StoneColor.None)
				{
					int? alreadyPlayed = game.Tree.MoveAlreadyPlayed(p);
					if (alreadyPlayed != null)
					{
						yield return new SelectStateAction((int)alreadyPlayed);
					}
					else
					{
						yield return new StoneMoveAction(p, game.State.PlayerToMove);
						if (game.State.Labels.Any(s => s != null))
							yield return LabelAction.ClearLabels;
					}
				}
			}
			if (commandID == 2)
			{
				int? moveIndex = game.Tree.FindMove(p);
				if (moveIndex != null)
				{
					yield return new SelectStateAction((int)moveIndex);
				}
			}
		}
	}

	public static class Tools
	{
		public static readonly Tool Move = new MoveTool();
		public static readonly Tool Edit = new EditTool();
		public static readonly Tool Score = null;
		public static readonly Tool Triangle = new ConstantLabelTool("#TR", "#SQ", "");
		public static readonly Tool Square = new ConstantLabelTool("#SQ", "#CR", "");
		public static readonly Tool Circle = new ConstantLabelTool("#CR", "#TR", "");
		public static readonly Tool Text = new TextLabelTool();
		public static readonly Tool Number = new NumberLabelTool();
		public static readonly Tool Symbol = new ConstantLabelTool("#TR", "#SQ", "#CR");
	}
}
