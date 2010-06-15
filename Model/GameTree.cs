using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class GameTreeNode
	{
		public GameTreeNode Parent { get; private set; }
		public ActionReference Action { get; private set; }
	}

	public class GameTree
	{
		private readonly ILookup<int,GameTreeNode> children;
		public IEnumerable<GameTreeNode> Children(GameTreeNode parent)
		{
			return children[parent.Action.Index];
		}
		public IEnumerable<GameTreeNode> Nodes { get { return null; } }

		public GameTreeNode FindNodeOfAction(ActionReference action)
		{
			return Nodes.Where(n => n.Action.Index >= action.Index).OrderByDescending(n => n.Action.Index).FirstOrDefault();
		}

		public GameTree(Replay replay)
		{
			children = Nodes.ToLookup(node => node.Action.Index);
		}
	}
}
