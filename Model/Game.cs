using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class Game
	{
		private int mSelectedActionIndex;

		public GameState GameState { get; set; }

		public int SelectedActionIndex
		{
			get { return mSelectedActionIndex; }
			private set
			{
				if (mSelectedActionIndex < -1)
					throw new ArgumentException("value<-1");
				if (mSelectedActionIndex > Replay.Actions.Count - 1)
					throw new ArgumentException("value>Actions.Count-1");
				mSelectedActionIndex = value;
			}
		}

		public ActionReference SelectedAction
		{
			get { return new ActionReference(Replay, SelectedActionIndex); }
			set
			{
				if (value.Replay != Replay)
					throw new ArgumentException("Action from wrong Replay");
				SelectedActionIndex = value.Index;
			}
		}
		public Replay Replay { get; private set; }

		public Game()
		{
			Replay = new Replay();
			SelectedActionIndex = -1;
		}

		public void Seek(int actionIndex)
		{
			if (actionIndex < SelectedActionIndex)
			{
				GameState = null;
				SelectedActionIndex = -1;
			}
			for (int i = SelectedActionIndex + 1; i <= actionIndex; i++)
			{
				Replay.Actions[i].Apply(this);
			}
		}

		public void Seek(TimeSpan time)
		{
			for (int i = 0; i < Replay.Actions.Count; i++)
			{
				ReplayTimeAction timeAction = Replay.Actions[i] as ReplayTimeAction;
				if (timeAction.Time > time)
				{
					Seek(i - 1);
					return;
				}
			}
		}
	}
}
