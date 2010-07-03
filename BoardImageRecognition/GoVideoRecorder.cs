using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.TreeDocuments;
using Model;

namespace BoardImageRecognition
{
	public class GoVideoRecorder
	{
		public readonly Replay Replay = new Replay();
		private BoardInfo previous;

		public GoVideoRecorder()
		{
			Replay.Info.Video = true;
		}

		public void Add(TimeSpan time, BoardInfo current)
		{
			Replay.SetEndTime(time);
			if (previous != null && (previous.Width != current.Width || previous.Height != current.Height))
				previous = null;
			if (previous == null)
				Replay.AddAction(new CreateBoardAction(current.Width, current.Height));
			List<Position> added = new List<Position>();
			List<Position> removed = new List<Position>();

			for (int y = 0; y < current.Height; y++)
				for (int x = 0; x < current.Width; x++)
				{
					PointInfo p0;
					if (previous != null)
						p0 = previous.Board[x, y];
					else
						p0 = new PointInfo();
					PointInfo p1 = current.Board[x, y];
					Position pos = new Position(x, y);
					if (p1.StoneColor != p0.StoneColor)
					{
						if (p1.StoneColor != StoneColor.None)
							added.Add(pos);
						else
							removed.Add(pos);
					}
					if (p1.SmallStoneColor != p0.SmallStoneColor)
						Replay.AddAction(new TerritoryAction(pos, p1.SmallStoneColor));
					if (p1.Label != p0.Label)
						Replay.AddAction(new LabelAction(pos, p1.Label));
				}
			previous = current;
			if (removed.Count > 0)
				Replay.AddAction(new SetStoneAction(Positions.FromList(removed), StoneColor.None));
			foreach (Position p in added)
				Replay.AddAction(new SetStoneAction(p, current.Board[p.X, p.Y].StoneColor));
		}
	}
}
