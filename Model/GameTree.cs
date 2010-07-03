using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.Collections;
using ChaosUtil.Mathematics;
using System.Diagnostics;

namespace Model
{
	public struct GameTreeNode
	{
		public readonly GameTree Tree;
		public int? ParentActionIndex { get { return Tree.Replay.Predecessor(FirstActionIndex); } }
		public int FirstActionIndex
		{
			get
			{
				GameTree tree = Tree;
				int? first = History
					.Skip(1)
					.TakeWhile(i => !tree.IsLastActionOfNode(i))
					.Select(i => (int?)i).LastOrDefault();//Last or null
				if (first == null)
					return History.First();
				else
					return (int)first;
			}
		}
		public readonly int LastActionIndex;

		internal GameTreeNode(GameTree tree, int lastActionIndex)
		{
			Tree = tree;
			LastActionIndex = lastActionIndex;
		}

		public GameStateAction LastAction { get { return (GameStateAction)Tree.Replay.Actions[LastActionIndex]; } }
		public GameStateAction FirstAction { get { return (GameStateAction)Tree.Replay.Actions[FirstActionIndex]; } }
		public IEnumerable<int> History { get { return Tree.Replay.History(LastActionIndex); } }
	}

	public class GameTree : IEnumerable<GameTreeNode>
	{
		private HashSet<int> mNodes = new HashSet<int>();
		private ILookup<int?, int> mChildren;
		public Replay Replay { get { return Game.Replay; } }
		public readonly Game Game;

		public readonly int Limit;

		public GameTreeNode Node(int lastActionIndex)
		{
			return new GameTreeNode(this, lastActionIndex);
		}

		public bool IsLastActionOfNode(int actionIndex)
		{
			return mNodes.Contains(actionIndex);
		}

		public IEnumerable<int> Children(int? parent)
		{
			return mChildren[parent];
		}

		public GameTree(Game game, int limit)
		{
			Game = game;
			for (int i = 0; i < limit; i++)
			{
				GameStateAction action = Replay.Actions[i] as GameStateAction;

				//Last GameStateAction before a MoveAction ends a node
				if (action != null && action.StartsNewNode(Replay, i))
					mNodes.Add((int)Replay.Predecessor(i));

				//Every GameStateAction with not exactly one child ends a node
				if (action != null && Replay.Successors(i).Where(succIndex => succIndex < limit).Count() != 1)
					mNodes.Add(i);
			}
			Debug.Assert(mNodes.All(i => Replay.Actions[i] is GameStateAction));

			mChildren = this.ToLookup(n => n.ParentActionIndex, n => n.LastActionIndex);
		}

		public IEnumerator<GameTreeNode> GetEnumerator()
		{
			foreach (int index in mNodes.OrderBy(i => i))
				yield return Node(index);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int? SelectedNode
		{
			get
			{
				if (Game.SelectedAction < 0)
					return null;
				return Replay.History(Game.SelectedAction).Select(i => (int?)i).FirstOrDefault();
			}
		}
	}
}
