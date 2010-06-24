using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using ChaosUtil.TreeDocuments;
using System.Diagnostics;
using ChaosUtil.Collections;

namespace Model
{
	public class Replay
	{
		private List<GameAction> actions = new List<GameAction>();
		private Dictionary<int, int> mMoveNumbers = new Dictionary<int, int>();
		private MultiDictionary<int, int> mSuccessors = new MultiDictionary<int, int>();

		public ReadOnlyCollection<GameAction> Actions { get; private set; }

		public event Action<Replay, int> OnActionAdded;

		private TimeSpan endTime;
		public TimeSpan EndTime
		{
			get { return endTime; }
			private set
			{
				if (value == EndTime)
					return;
				if (value < EndTime)
					throw new ArgumentException("value");
				endTime = value;
			}
		}

		public void SetEndTime(TimeSpan value)
		{
			EndTime = value;
			AddAction(new ReplayTimeAction(value));
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
			int actionIndex = Actions.Count;
			actions.Add(action);
			ReplayTimeAction timeAction = action as ReplayTimeAction;
			if (timeAction != null)
				EndTime = timeAction.Time;
			if (Actions[actionIndex] is MoveAction)
			{
				int moveNumber = History(actionIndex)
					.Where(i => Actions[i] is MoveAction)
					.Skip(1)//the new action
					.Select(i => MoveNumber(i))
					.FirstOrDefault() + 1;
				mMoveNumbers.Add(actionIndex, moveNumber);
			}

			int? pred = Predecessor(actionIndex);
			if (pred != null && Actions[actionIndex] is GameStateAction)
			{
				mSuccessors[(int)pred].Add(actionIndex);
			}

			Debug.Assert(Actions.Count == actionIndex + 1);
			if (OnActionAdded != null)
				OnActionAdded(this, actionIndex);
			Debug.Assert(Actions.Count == actionIndex + 1);

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

		private static IEnumerable<GameAction> ParseActions(TreeDoc actionsTD)
		{
			foreach (TreeDoc actionTD in actionsTD.Elements())
			{
				yield return GameActionParser.Parse(actionTD);
			}
		}

		public static Replay Parse(TreeDoc td)
		{
			Replay replay = new Replay();
			TreeDoc actionsTD = td.Element("Actions");
			foreach (GameAction action in ParseActions(actionsTD))
			{
				replay.AddAction(action);
			}
			return replay;
		}

		public static Replay Parse(string s)
		{
			return Parse(TreeDoc.Parse(s));
		}

		public static Replay Load(string fileName)
		{
			return Parse(TreeDoc.Load(fileName));
		}

		public int? Predecessor(int actionIndex)
		{
			int? result = actionIndex;
			do
			{
				result = Actions[(int)result]._Previous((int)result);
			} while (result != null && !(Actions[(int)result] is GameStateAction));

			if (actionIndex < 0 || actionIndex >= Actions.Count)
				throw new ArgumentOutOfRangeException("actionIndex");
			return result;
		}

		public IEnumerable<int> Successors(int actionIndex)
		{
			if (!(Actions[actionIndex] is GameStateAction))
				throw new ArgumentException();
			return mSuccessors[actionIndex];
		}

		public IEnumerable<int> History(int actionIndex)
		{
			if (actionIndex < 0 || actionIndex >= Actions.Count)
				throw new ArgumentOutOfRangeException("actionIndex");
			int? currentIndex = actionIndex;
			while (currentIndex != null)
			{
				if (Actions[(int)currentIndex] is GameStateAction)
					yield return (int)currentIndex;
				currentIndex = Predecessor((int)currentIndex);
			}
		}

		public int MoveNumber(int actionIndex)
		{
			if (!(Actions[actionIndex] is MoveAction))
				throw new ArgumentException("Action is no MoveAction");
			return mMoveNumbers[actionIndex];
		}
	}
}
