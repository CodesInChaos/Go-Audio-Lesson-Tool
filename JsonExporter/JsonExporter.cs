using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Newtonsoft.Json.Linq;

namespace JsonExport
{
	public class JsonExporter
	{
		private static JObject SerializeAction(GameAction action, TimeSpan time)
		{
			if (action is SetStoneAction)
			{
				var action2 = (SetStoneAction)action;
				Position pos = action2.Positions.GetPositions(null).Single();
				return new JObject(
					new JProperty("a", "S"),
					new JProperty("x", pos.X),
					new JProperty("y", pos.Y),
					new JProperty("v", action2.Color.ShortName()),
					new JProperty("t", time.TotalSeconds)
					);
			}
			else if (action is CreateBoardAction)
			{
				var action2 = (CreateBoardAction)action;
				return new JObject(
					new JProperty("a", "Board"),
					new JProperty("x", action2.Width),
					new JProperty("y", action2.Height),
					new JProperty("t", time.TotalSeconds)
					);
			}
			else if (action is LabelAction)
			{
				var action2 = (LabelAction)action;
				Position pos = action2.Positions.GetPositions(null).Single();
				return new JObject(
					new JProperty("a", "L"),
					new JProperty("x", pos.X),
					new JProperty("y", pos.Y),
					new JProperty("v", action2.Text),
					new JProperty("t", time.TotalSeconds)
					);
			}
			else
				throw new NotSupportedException();
		}

		public static string Export(Replay replay)
		{
			Game game = new Game(replay);
			GameState oldState = null;
			List<GameAction> actions = new List<GameAction>();
			for (int i = 0; i < replay.Actions.Count; i++)
			{
				if (replay.Actions[i] is ReplayTimeAction)
					actions.Add(replay.Actions[i]);
				game.Seek(i);
				List<GameAction> newActions = StateDelta.Delta(oldState, game.State);
				actions.AddRange(newActions);
				if (game.State != null)
					oldState = game.State.Clone();
			}
			List<JObject> jActions = new List<JObject>();
			TimeSpan time = TimeSpan.Zero;
			foreach (var action in actions)
			{
				if (action is ReplayTimeAction)
					time = ((ReplayTimeAction)action).Time;
				else
					jActions.Add(SerializeAction(action, time));
			}
			string s = new JObject(new JProperty("changes", new JArray(jActions))).ToString();
			return s;
		}
	}
}
