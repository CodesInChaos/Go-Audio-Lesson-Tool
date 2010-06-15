using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using ChaosUtil.TreeDocuments;

namespace Model
{
	public class Replay
	{
		private List<GameAction> actions = new List<GameAction>();
		public ReadOnlyCollection<GameAction> Actions { get; private set; }

		public event Action<ActionReference> OnActionAdded;

		private TimeSpan endTime;
		public TimeSpan EndTime
		{
			get { return endTime; }
			set
			{
				if (value == EndTime)
					return;
				if (value < EndTime)
					throw new ArgumentException("value");
				endTime = value;
				AddAction(new ReplayTimeAction(value));
			}
		}

		public int ActionIndexAtTime(TimeSpan t)
		{
			for (int i = 0; i < actions.Count; i++)
			{
				ReplayTimeAction timeAction = actions[i] as ReplayTimeAction;
				if (timeAction != null && timeAction.Time > t)
					return i - 1;
			}
			return actions.Count - 1;
		}

		public void AddAction(GameAction action)
		{

			actions.Add(action);
			if (OnActionAdded != null)
				OnActionAdded(new ActionReference(this, actions.Count - 1));
		}

		public Replay()
		{
			Actions = new ReadOnlyCollection<GameAction>(actions);
		}

		public string Save()
		{
			TreeDoc infoTD = TreeDoc.CreateList("Info");
			infoTD.ForceExpand = true;
			TreeDoc rulesTD = TreeDoc.CreateList("Rules");
			rulesTD.ForceExpand = true;
			TreeDoc actionsTD = TreeDoc.CreateListRange("Actions", actions.Select(a => a.ToTreeDoc()));
			actionsTD.ForceExpand = true;
			TreeDoc replayTD = TreeDoc.CreateList("", infoTD, rulesTD, actionsTD);
			return replayTD.SaveAsList();
		}

		public void Save(string filename)
		{
			File.WriteAllText(filename, Save(), Encoding.UTF8);
		}
	}
}
