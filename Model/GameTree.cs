using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.Collections;
using ChaosUtil.Mathematics;

namespace Model
{
	public struct GameTreeNode
	{
		public readonly GameTree Tree;
		public int? ParentActionIndex { get { return Tree.Replay.Predecessor(FirstActionIndex); } }
		public int FirstActionIndex { get { return History.First(Tree.IsLastActionOfNode); } }
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
		public readonly Replay Replay;
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

		public GameTree(Replay replay, int limit)
		{
			Replay = replay;
			for (int i = 0; i < limit; i++)
			{
				GameStateAction action = Replay.Actions[i] as GameStateAction;
				if (action != null && action.StartsNewNode(Replay, i))
					mNodes.Add((int)Replay.Predecessor(i));
			}
			//mNodes.Add(limit - 1);
			mChildren = this.ToLookup(n => n.ParentActionIndex, n => n.LastActionIndex);
		}

		public IEnumerator<GameTreeNode> GetEnumerator()
		{
			foreach (int index in mNodes)
				yield return Node(index);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
