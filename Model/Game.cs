using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class Game
	{
		private int mSelectedAction = -1;

		public GameState State { get; internal set; }

		public int SelectedAction
		{
			get { return mSelectedAction; }
			private set
			{
				if (value < -1)
					throw new ArgumentException("value<-1");
				if (value > Replay.Actions.Count - 1)
					throw new ArgumentException("value>Actions.Count-1");
				mSelectedAction = value;
			}
		}

		public Replay Replay { get; private set; }
		public GraphicalGameTree Tree { get; private set; }

		public Game(Replay replay)
		{
			Replay = replay;
		}

		public int? CurrentMoveIndex
		{
			get
			{
				if (SelectedAction >= 0)
					return Replay.History(SelectedAction).Where(i => Replay.Actions[i] is MoveAction).FirstOrNull();
				else
					return null;
			}
		}

		public void Seek(int actionIndex)
		{
			if (actionIndex == SelectedAction)
				return;
			if (actionIndex < -1 || actionIndex >= Replay.Actions.Count)
				throw new ArgumentOutOfRangeException("actionIndex");
			if (actionIndex != -1)
			{
				int[] chain = Replay.History(actionIndex).TakeWhile(i => i != SelectedAction).Reverse().ToArray();
				if (chain.Length > 0 && Replay.Predecessor(chain[0]) == null)
				{//From beginning
					State = null;
					SelectedAction = -1;
				}
				foreach (int i in chain)
				{
					Replay.Actions[i].Apply(this);
				}
			}
			else
			{
				State = null;
			}
			SelectedAction = actionIndex;
			Tree = new GraphicalGameTree(this, actionIndex + 1);
		}

		public void Seek(TimeSpan time)
		{
			for (int i = 0; i < Replay.Actions.Count; i++)
			{
				ReplayTimeAction timeAction = Replay.Actions[i] as ReplayTimeAction;
				if (timeAction != null && timeAction.Time > time)
				{
					Seek(i - 1);
					return;
				}
			}
			Seek(Replay.Actions.Count - 1);
		}
	}
}
