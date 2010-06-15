using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Model
{
	/*public class ActionGroup
	{
		public Game Game { get; private set; }
		public TimeSpan Time { get; private set; }
		public ActionGroup Parent { get; private set; }
		public ReadOnlyCollection<GameAction> Actions { get; private set; }

		public ActionGroup(Game game, ActionGroup parent, TimeSpan time, IEnumerable<GameAction> actions)
		{
			Game = game;
			Time = time;
			if (parent == null && game.ActionGroups.Count > 0)
			{
				parent = game.ActionGroups[game.ActionGroups.Count - 1];
			}
			Parent = parent;
			Actions = new ReadOnlyCollection<GameAction>(actions.ToList());

			game.AddActionGroup(this);
		}

		public ActionGroup(Game game, ActionGroup parent, TimeSpan time, params GameAction[] actions)
			: this(game, parent, time, actions.AsEnumerable())
		{

		}

		private WeakReference cachedState;
		public State State
		{
			get
			{
				if (cachedState != null)
				{
					State readCachedState = (State)cachedState.Target;
					if (readCachedState != null)
						return readCachedState;
				}
				State newState = null;
				if (Parent != null)
					newState = Parent.State;
				foreach (GameAction action in Actions)
				{
					newState = action.CreateState(Game, newState);
				}
				cachedState = new WeakReference(newState);
				return newState;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Time.TotalSeconds.ToString());
			sb.Append(" ");
			foreach (GameAction action in Actions)
			{
				sb.Append(action.ToString());
				sb.Append(";");
			}
			sb.Length = sb.Length - 1;
			return sb.ToString();
		}
	}*/
}
