using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class GameTreeNode
	{
		public GameTreeNode Parent { get; private set; }
		public int Action { get; private set; }
	}

	public class GameTree
	{
		public readonly Game Game;
		private readonly ILookup<int, GameTreeNode> children;
		public IEnumerable<GameTreeNode> Children(GameTreeNode parent)
		{
			return children[parent.Action];
		}
		public IEnumerable<GameTreeNode> Nodes { get { return null; } }

		public GameTreeNode FindNodeOfAction(int action)
		{
			return Nodes.Where(n => n.Action >= action).OrderByDescending(n => n.Action).FirstOrDefault();
		}

		public GameTree(Game game)
		{
			Game = game;
			children = Nodes.ToLookup(node => node.Action);
		}
	}
}
