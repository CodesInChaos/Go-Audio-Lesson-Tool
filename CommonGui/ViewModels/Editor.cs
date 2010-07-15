using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace CommonGui.ViewModels
{
	public class Editor : ModelPart
	{
		public Tool ActiveTool { get; set; }

		public void AddActions(params GameAction[] actions)
		{
			AddActions(actions.AsEnumerable());
		}

		public void AddActions(IEnumerable<GameAction> actions)
		{
			List<GameAction> actionList = actions.ToList();
			if (actionList.Count == 0)
				return;
			if (Model.Game.Replay.EndTime != Model.Time)
				actionList.Insert(0, new ReplayTimeAction(Model.Time));
			Model.SendActions(actionList);
		}

		public Editor(ViewModel model)
			: base(model)
		{
		}
	}
}
