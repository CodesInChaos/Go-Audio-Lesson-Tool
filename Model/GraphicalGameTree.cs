using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.Mathematics;
using System.Diagnostics;

namespace Model
{
	public class GraphicalGameTree : GameTree
	{
		private Dictionary<int, int> mRelativePositions = new Dictionary<int, int>();
		private Dictionary<int, Vector2i> mPositionOfNode = new Dictionary<int, Vector2i>();
		private Dictionary<Vector2i, int> mNodeAtPosition = new Dictionary<Vector2i, int>();

		internal struct NodeSize
		{
			public readonly int? Node;
			public readonly int[] Upper;
			public readonly int[] Lower;

			public NodeSize(int? node, int[] lower, int[] upper)
			{
				Debug.Assert(lower.Length == upper.Length);
				Node = node;
				Lower = lower;
				Upper = upper;
			}
		}

		private int CalculateOffset(int[] upper1, int[] lower2)
		{
			int max = 0;
			for (int i = 0; (i < upper1.Length) && (i < lower2.Length); i++)
			{
				int diff = upper1[i] - lower2[i];
				if (diff > max)
					max = diff;
			}
			return max;
		}

		private NodeSize CalculateRelativePositions(int? node)
		{
			NodeSize[] children = Children(node).Select(child => CalculateRelativePositions(child)).ToArray();
			int relativePosition = 0;
			int length;
			if (children.Length > 0)
			{
				mRelativePositions[(int)children[0].Node] = 0;
				for (int i = 1; i < children.Length; i++)
				{
					int offset = CalculateOffset(children[i - 1].Upper, children[i].Lower);
					relativePosition += offset;
					mRelativePositions[(int)children[i].Node] = relativePosition;
				}
				length = children.Select(c => c.Lower.Length).Max() + 1;
			}
			else length = 1;
			int[] lower = new int[length];
			lower[0] = 0;
			for (int i = 1; i < length; i++)
				lower[i] = children.Select(c => c.Lower.Length >= i ? c.Lower[i - 1] : 1000000 + mRelativePositions[(int)c.Node]).Min();
			int[] upper = new int[length];
			upper[0] = children.Length;
			for (int i = 1; i < length; i++)
				upper[i] = children.Select(c => c.Upper.Length >= i ? c.Upper[i - 1] : -1000000 + mRelativePositions[(int)c.Node]).Max();
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
			return mPositionOfNode[node];
		}

		public GraphicalGameTree(Replay replay, int limit)
			: base(replay, limit)
		{
			CalculateRelativePositions(null);
			CalculateAbsolutePositions(new Vector2i(), null);
		}
	}
}
