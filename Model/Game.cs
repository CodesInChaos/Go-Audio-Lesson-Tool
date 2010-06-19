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

		public Game(Replay replay)
		{
			Replay = replay;
		}

		public void Seek(int actionIndex)
		{
			if (actionIndex < SelectedAction)
			{
				State = null;
				SelectedAction = -1;
			}
			for (int i = SelectedAction + 1; i <= actionIndex; i++)
			{
				Replay.Actions[i].Apply(this);
			}
			SelectedAction = actionIndex;
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
