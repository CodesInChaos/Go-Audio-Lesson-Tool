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

		public void AddActions(IEnumerable<GameAction> actions)
		{
			Model.SendActions(actions);
		}

		public Editor(ViewModel model)
			: base(model)
		{
		}
	}
}
