using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace CommonGui.ViewModels
{
	public class Editor : ModelPart<ViewModel>
	{
		private Tool mActiveTool;

		public Tool ActiveTool { get { return mActiveTool; } set { mActiveTool = value; OnChanged(); } }

		public void AddActions(IEnumerable<GameAction> actions)
		{
			foreach (GameAction a in actions)
				Model.AddAction(a);
		}

		public Editor(ViewModel model)
			: base(model)
		{
		}
	}
}
