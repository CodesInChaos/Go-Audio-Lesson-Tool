using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;
using System.Diagnostics;

namespace Model
{
	[Flags]
	public enum Directions { Up = 1, Left = 2, Right = 4, Down = 8 };


	public class GraphicalGameTree : GameTree
	{
		private Dictionary<int, int> mRelativePositions = new Dictionary<int, int>();
		private Dictionary<int, Vector2i> mPositionOfNode = new Dictionary<int, Vector2i>();
		private Dictionary<Vector2i, int> mNodeAtPosition = new Dictionary<Vector2i, int>();
		private Dictionary<Vector2i, Directions> mConnectionsAtPosition = new Dictionary<Vector2i, Directions>();

		public int Width { get; private set; }
		public int Height { get; private set; }

		internal struct NodeSize
		{
			public readonly int? Node;
			public readonly int[] Upper;
			public readonly int[] Lower;
			public int Length { get { return Lower.Length; } }

			public NodeSize(int? node, int[] lower, int[] upper)
			{
				Debug.Assert(lower.Length == upper.Length);
				Node = node;
				Lower = lower;
				Upper = upper;
			}
		}

		private int CalculateOffset(int[] upper, int[] childLower)
		{
			int max = 0;
			for (int i = 0; i < childLower.Length; i++)
			{
				int diff = upper[i + 1] - childLower[i];
				if (diff > max)
					max = diff;
			}
			return max;
		}

		private NodeSize CalculateRelativePositions(int? node)
		{
			NodeSize[] children = Children(node).Select(child => CalculateRelativePositions(child)).ToArray();
			int length;
			if (children.Length > 0)
				length = children.Select(c => c.Length).Max() + 1;
			else
				length = 1;
			int[] lower = new int[length];
			int[] upper = new int[length];
			for (int i = 0; i < length; i++)
			{
				lower[i] = int.MaxValue;
				upper[i] = 0;
			}
			int relativePosition = 0;
			for (int childIndex = 0; childIndex < children.Length; childIndex++)
			{
				relativePosition = CalculateOffset(upper, children[childIndex].Lower);
				mRelativePositions[(int)children[childIndex].Node] = relativePosition;
				for (int colIndex = 0; colIndex < children[childIndex].Length; colIndex++)
				{
					upper[colIndex + 1] = Math.Max(upper[colIndex + 1], children[childIndex].Upper[colIndex] + relativePosition);
					lower[colIndex + 1] = Math.Min(lower[colIndex + 1], children[childIndex].Lower[colIndex] + relativePosition);
				}
			}
			lower[0] = 0;
			upper[0] = relativePosition + 1;
			Debug.Assert(lower.Min() >= 0);
			Debug.Assert(lower.Max() < int.MaxValue);
			Debug.Assert(upper.Min() >= 1);
			Debug.Assert(Enumerable.Range(0, length).All(i => lower[i] < upper[i]));
			return new NodeSize(node, lower, upper);
		}

		private void CalculateAbsolutePositions(Vector2i parentPosition, int? node)
		{
			Vector2i position;
			if (node != null)
			{
				position = parentPosition + new Vector2i(1, mRelativePositions[(int)node]);
				mPositionOfNode.Add((int)node, position);
				mNodeAtPosition.Add(position, (int)node);
				if (position.X > Width - 1)
					Width = position.X + 1;
				if (position.Y > Height - 1)
					Height = position.Y + 1;
			}
			else
				position = new Vector2i(-1, 0);
			foreach (int child in Children(node))
				CalculateAbsolutePositions(position, child);
		}

		public int? NodeAtPosition(Vector2i position)
		{
			int node;
			if (mNodeAtPosition.TryGetValue(position, out node))
				return node;
			else return null;
		}

		public Vector2i PositionOfNode(int node)
		{
			Vector2i result;
			if (mPositionOfNode.TryGetValue(node, out result))
				return result;
			else
				return new Vector2i(-1, -1);
		}

		public int RelativePositionOfNode(int node)
		{
			return mRelativePositions[node];
		}

		public GraphicalGameTree(Game game, int limit)
			: base(game, limit)
		{
			CalculateRelativePositions(null);
			CalculateAbsolutePositions(new Vector2i(), null);
			CalculateConnections();
		}

		private void CalculateConnections()
		{
			foreach (GameTreeNode node in this)
			{
				Vector2i position = PositionOfNode(node.LastActionIndex);
				int[] children = Children(node.LastActionIndex).ToArray();
				{
					Directions dirs = Directions.Left;
					if (children.Length >= 1)
						dirs |= Directions.Right;
					if (children.Length >= 2)
						dirs |= Directions.Down;
					mConnectionsAtPosition.Add(position, dirs);
				}
				HashSet<int> offsets = new HashSet<int>(children.Select(childIndex => RelativePositionOfNode(childIndex)));
				if (children.Length >= 2)
				{
					int lastOffset = RelativePositionOfNode(children[children.Length - 1]);
					for (int i = 1; i <= lastOffset; i++)
					{
						Directions dirs = Directions.Up;
						if (offsets.Contains(i))
							dirs |= Directions.Right;
						if (i != lastOffset)
							dirs |= Directions.Down;
						mConnectionsAtPosition.Add(new Vector2i(position.X, position.Y + i), dirs);
					}
				}
			}
		}

		public Directions ConnectionsAtPosition(Vector2i position)
		{
			Directions connections;
			mConnectionsAtPosition.TryGetValue(position, out connections);
			return connections;
		}
	}
}
