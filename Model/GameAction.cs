using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil;
using ChaosUtil.TreeDocuments;
using System.IO;

namespace Model
{
	public abstract class GameAction
	{
		public abstract TreeDoc ToTreeDoc();
		public sealed override string ToString()
		{
			return ToTreeDoc().SaveAsElement();
		}
		public abstract void Apply(Game game);
	}

	public abstract class ModifyingAction : GameAction
	{
		public Position Position { get; protected set; }
		protected abstract void ModifyState(GameState state);
		public sealed override void Apply(Game game)
		{
			ModifyState(game.GameState);
		}
	}

	public abstract class MoveAction : ModifyingAction
	{
		public StoneColor Color { get; private set; }
		public MoveAction(StoneColor color)
		{
			Color = color;
			if (color == StoneColor.None)
				throw new ArgumentException();
		}

		protected override void ModifyState(GameState state)
		{
			state.MoveIndex++;
			state.PlayerToMove = state.PlayerToMove.Invert();
		}
	}

	public class PassMoveAction : MoveAction
	{
		public static PassMoveAction FromTreeDoc(TreeDoc treeDoc)
		{
			return new PassMoveAction(
				StoneColorHelper.Parse(treeDoc.Element("", 0))
				);
		}

		public PassMoveAction(StoneColor color)
			: base(color)
		{
		}

		protected override void ModifyState(GameState state)
		{
			state.Passes++;
			state.Ko = null;
			base.ModifyState(state);
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("M", "P", Color.ShortName());
		}
	}

	public class StoneMoveAction : MoveAction
	{
		public static StoneMoveAction FromTreeDoc(TreeDoc treeDoc)
		{
			return new StoneMoveAction(
				(Position)treeDoc.Element("", 0),
				StoneColorHelper.Parse((string)treeDoc.Element("", 1))
				);
		}

		public StoneMoveAction(Position position, StoneColor color)
			: base(color)
		{
			Position = position;
		}

		protected override void ModifyState(GameState state)
		{
			if (!state.PutStone(Position, Color))
				return;
			state.Passes = 0;
			base.ModifyState(state);
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("M", Position, Color.ShortName());
		}
	}

	public class LabelAction : ModifyingAction
	{
		public string Text { get; private set; }

		public LabelAction(Position position, string text)
			: base()
		{
			if (text == null)
				throw new ArgumentNullException("text");
			Text = text;
			Position = position;
		}

		protected override void ModifyState(GameState state)
		{
			state.Labels[Position] = Text;
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("L", Position, Text);
		}
	}

	public class SetStoneAction : ModifyingAction
	{
		public StoneColor Color;

		public SetStoneAction(Position position, StoneColor color)
		{
			Color = color;
			Position = position;
		}

		protected override void ModifyState(GameState state)
		{
			if (Color == StoneColor.None)
				state.Stones[Position] = StoneColor.None;
			else
				state.PutStone(Position, Color);
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("S", Position, Color.ShortName());
		}
	}

	public class InitStateAction : GameAction
	{
		public int Width { get; private set; }
		public int Height { get; private set; }

		/*public override GameState CreateState(Replay game, GameState parentState)
		{
			return new GameState(game, Width, Height);
		}*/

		public InitStateAction(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("Board", Width, Height);
		}

		public override void Apply(Game game)
		{
			game.GameState = new GameState(game.Replay, Width, Height);
		}
	}

	public class ReplayTimeAction : GameAction
	{
		public TimeSpan Time { get; private set; }
		public ReplayTimeAction(TimeSpan time)
		{
			Time = time;
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("T", Time.TotalSeconds);
		}

		public override void Apply(Game game)
		{
			//Doesn't do anything by itself
			//Known action for Seek(time)
		}
	}

	public static class GameActionParser
	{
		public static GameAction Parse(TreeDoc doc)
		{
			switch (doc.Name)
			{
				case "M":
					if ((string)doc.Element("", 0) == "P")
						return new PassMoveAction(StoneColorHelper.Parse(doc.Element("", 1)));
					else
						return new StoneMoveAction(
							(Position)doc.Element("", 0),
							StoneColorHelper.Parse(doc.Element("", 1))
							);
				default:
					throw new InvalidDataException("Unknown Action " + doc.Name);
			}
		}
	}

	/*public class SelectStateAction : GameAction
	{
		public override State CreateState(Replay game, State parentState)
		{
			throw new NotImplementedException();
		}

		public State State { get; private set; }

		public SelectStateAction(State state)
		{
			State = state;
		}
	}*/
}
