using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Model.Sgf
{
	public sealed class SgfNode
	{
		private sealed class SgfNodeCollection : Collection<SgfNode>
		{
			private SgfNode node;

			private void AddNode(SgfNode child)
			{
				if (child.Parent != null)
					throw new InvalidOperationException("Node already has a parent");
				child.Parent = node;
			}

			private void RemoveNode(SgfNode child)
			{
				if (child.Parent != node)
					throw new InvalidOperationException("Wrong parent");
				child.Parent = null;
			}

			protected override void ClearItems()
			{
				base.ClearItems();
				foreach (SgfNode child in this)
					RemoveNode(child);
			}

			protected override void InsertItem(int index, SgfNode item)
			{
				base.InsertItem(index, item);
				AddNode(item);
			}

			protected override void RemoveItem(int index)
			{
				RemoveNode(this[index]);
				base.RemoveItem(index);
			}

			protected override void SetItem(int index, SgfNode item)
			{
				RemoveNode(this[index]);
				base.SetItem(index, item);
				AddNode(item);
			}
		}
		public SgfNode Parent { get; private set; }
		public List<SgfProperty> Commands { get; private set; }
		public IList<SgfNode> Children { get; private set; }

		public SgfNode()
		{
			Commands = new List<SgfProperty>();
			Children = new SgfNodeCollection();
		}

		public SgfNode(bool isRoot)
			:this()
		{
		}
	}
}
