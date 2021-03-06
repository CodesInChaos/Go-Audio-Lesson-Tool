﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Collections;
using Chaos.Util.Mathematics;
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
		private HashSet<int> currentVariationElements = new HashSet<int>();
		private ILookup<int?, int> mChildren;
		public Replay Replay { get { return Game.Replay; } }
		public readonly Game Game;
		public readonly int? CurrentVariation;

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

		public bool IsInCurrentVariation(int actionIndex)
		{
			if (currentVariationElements != null)
				return currentVariationElements.Contains(actionIndex);
			else
				return false;
		}

		private int? FindCurrentVariation()
		{
			if (SelectedNode == null)
				return null;
			int current = (int)SelectedNode;
			for (int actionIndex = Replay.Actions.Count - 1; actionIndex >= 0; actionIndex--)
			{
				if (Replay.History(actionIndex)
						.TakeWhile(index => index >= current)//Performance
						.Contains(current))
				{
					current = Replay.History(actionIndex).First();
				}
			}
			return current;
		}

		private void VariationElements(out List<int> history, out int currentIndex)
		{
			int? currentVariation = CurrentVariation;
			if (currentVariation == null)
			{
				history = new List<int>();
				currentIndex = -1;
				return;
			}
			history = Replay.History((int)currentVariation).ToList();
			currentIndex = history.IndexOf((int)SelectedNode);
			if (currentIndex < 0)
				throw new InvalidOperationException();
		}

		public IEnumerable<int> VariationFuture()
		{
			List<int> history;
			int currentIndex;
			VariationElements(out history, out currentIndex);
			for (int i = currentIndex - 1; i >= 0; i--)
				yield return history[i];
		}

		public IEnumerable<int> VariationPast()
		{
			if (SelectedNode == null)
				return Enumerable.Empty<int>();
			return Replay.History((int)SelectedNode).Skip(1);
		}

		public GameTree(Game game, int limit)
		{
			Game = game;
			Limit = limit;
			for (int i = 0; i < limit; i++)
			{
				GameStateAction action = Replay.Actions[i] as GameStateAction;

				//Last GameStateAction before a MoveAction ends a node
				if (action != null && action.StartsNewNode(Replay, i))
					mNodes.Add((int)Replay.Predecessor(i));

				//Every GameStateAction with not exactly one child ends a node
				if (action != null && Replay.Successors(i).Where(succIndex => succIndex < limit).Take(2).Count() != 1)
					mNodes.Add(i);
			}
			Debug.Assert(mNodes.All(i => Replay.Actions[i] is GameStateAction));

			mChildren = this.ToLookup(n => n.ParentActionIndex, n => n.LastActionIndex);
			CurrentVariation = FindCurrentVariation();
			if (CurrentVariation != null)
				currentVariationElements = new HashSet<int>(Replay.History((int)CurrentVariation));
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

		public int? MoveAlreadyPlayed(Position p)
		{
			int? currentNode = SelectedNode;
			if (currentNode == null)
				return null;
			foreach (GameTreeNode childNode in Children(currentNode).Select(i => Node(i)))
			{
				StoneMoveAction action = childNode.FirstAction as StoneMoveAction;
				if (action != null && action.Position == p)
					return childNode.LastActionIndex;
			}
			return null;
		}

		public int? FindMove(Position p)
		{
			IEnumerable<int> moves = Replay.History((int)SelectedNode).Concat(VariationFuture().Reverse());
			return moves.FirstOrNull(
				i =>
				{
					var moveAction = Replay.Actions[i] as StoneMoveAction;
					return moveAction != null && moveAction.Position == p;
				}
			);
		}
	}
}
