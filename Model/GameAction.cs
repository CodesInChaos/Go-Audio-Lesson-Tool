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

		internal virtual int? _Previous(int current)
		{
			return current - 1;
		}

		public abstract void Apply(Game game);
	}

	public abstract class GameStateAction : GameAction
	{
		public virtual bool StartsNewNode(Replay replay, int index)
		{
			return false;
		}
	}

	public abstract class ReplayStateAction : GameAction
	{
	}

	public abstract class ControlAction : GameAction
	{
		public sealed override void Apply(Game game)
		{
		}
	}

	public abstract class ModifyingAction : GameStateAction
	{
		public Position Position { get; protected set; }
		protected abstract void ModifyState(GameState state);
		public sealed override void Apply(Game game)
		{
			ModifyState(game.State);
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

		public override bool StartsNewNode(Replay replay, int index)
		{
			return true;
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
			if (Text == "")
				state.Labels[Position] = null;
			else
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
			state.Ko = null;
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("S", Position, Color.ShortName());
		}

		public override bool StartsNewNode(Replay replay, int actionIndex)
		{
			foreach (int oldIndex in replay.History(actionIndex).Skip(1))
			{
				GameAction oldAction = replay.Actions[oldIndex];
				if (oldAction is SetStoneAction)
					return false;
				if (oldAction is MoveAction)
					return true;
				if (oldAction is InitStateAction)
					return true;
			}
			throw new InvalidOperationException();
		}
	}

	public class ClearLabelsAction : ModifyingAction
	{
		protected override void ModifyState(GameState state)
		{
			for (int y = 0; y < state.Height; y++)
				for (int x = 0; x < state.Width; x++)
					state.Labels[x, y] = null;
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("ClearLabels");
		}
	}

	public class InitStateAction : GameStateAction
	{
		public int Width { get; private set; }
		public int Height { get; private set; }

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
			if (game.State != null)
				throw new InvalidOperationException("Init State action requires a null state before");
			game.State = new GameState(game.Replay, Width, Height);
		}

		internal override int? _Previous(int current)
		{
			return null;
		}
	}

	//Known Action! User by Seek(Time)
	public class ReplayTimeAction : ControlAction
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
	}

	public class SelectStateAction : ControlAction
	{
		public int SelectedActionIndex { get; private set; }

		public SelectStateAction(int selectedActionIndex)
		{
			SelectedActionIndex = selectedActionIndex;
		}

		public override TreeDoc ToTreeDoc()
		{
			return TreeDoc.CreateList("Select", SelectedActionIndex);
		}

		internal override int? _Previous(int current)
		{
			return SelectedActionIndex;
		}
	}
}
