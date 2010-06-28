using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.TreeDocuments;
using Model;

namespace ImageAnalyzer
{
	public class VideoRecorder
	{
		public readonly TreeDoc video = TreeDoc.CreateNull();
		private BoardInfo previous;

		public VideoRecorder()
		{
			video.ForceExpand = true;
		}

		public void Add(BoardInfo current)
		{
			if (previous != null && (previous.Width != current.Width || previous.Height != current.Height))
				previous = null;
			if (previous == null)
				video.Add(TreeDoc.CreateList("Size", current.Width, current.Height));

			TreeDoc node = TreeDoc.CreateNull("Node");
			node.ForceExpand = true;
			for (int y = 0; y < current.Height; y++)
				for (int x = 0; x < current.Width; x++)
				{
					PointInfo p0;
					if (previous != null)
						p0 = previous.Board[x, y];
					else
						p0 = new PointInfo();
					PointInfo p1 = current.Board[x, y];
					string pos = new Position(x, y).ToString();
					if (p1.StoneColor != p0.StoneColor)
						node.Add(TreeDoc.CreateList("S", pos, p1.StoneColor.ShortName()));
					if (p1.SmallStoneColor != p0.SmallStoneColor)
						node.Add(TreeDoc.CreateList("s", pos, p1.SmallStoneColor.ShortName()));
					if (p1.Label != p0.Label)
						node.Add(TreeDoc.CreateList("L", pos, p1.Label));
				}
			previous = current;
			if (node.Children.Count > 0)
				video.Add(node);
		}
	}
}
