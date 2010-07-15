using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class GameState : BoardSetup
	{
		public readonly Array2D<StoneColor> Stones;
		public readonly Array2D<string> Labels;
		public readonly Array2D<StoneColor> Territory;

		public StoneColor PlayerToMove { get; set; }
		public int MoveIndex { get; set; }

		public Position? Ko { get; set; }
		public int Passes { get; set; }

		public HashSet<Position> GetChain(Position p)
		{
			HashSet<Position> result = new HashSet<Position>();
			HashSet<Position> all = new HashSet<Position>();
			Queue<Position> todo = new Queue<Position>();
			StoneColor color = Stones[p];
			todo.Enqueue(p);
			all.Add(p);
			while (todo.Count > 0)
			{
				Position current = todo.Dequeue();
				if (Stones[current.X, current.Y] != color)
					continue;
				result.Add(current);
				foreach (Position np in Neighbours(current))
				{
					if (all.Add(np))
						todo.Enqueue(np);
				}
			}
			return result;
		}

		public IEnumerable<Position> GetNeighboursOfChain(Position p)
		{
			HashSet<Position> chain = GetChain(p);
			foreach (Position sp in chain)
			{
				foreach (Position np in Neighbours(sp))
				{
					if (Stones[np] != Stones[p])
						yield return np;
				}
			}
		}

		public IEnumerable<Position> GetLibertiesOfChain(Position p)
		{
			return GetNeighboursOfChain(p).Where((np) => Stones[np.X, np.Y] == StoneColor.None);
		}

		public GameState(int width, int height)
			: base(width, height)
		{
			Stones = new Array2D<StoneColor>(width, height);
			Labels = new Array2D<String>(width, height);
			Territory = new Array2D<StoneColor>(width, height);
			PlayerToMove = StoneColor.Black;
		}

		private int RemoveSuffocated(Position p)
		{
			IEnumerable<Position> liberites = GetLibertiesOfChain(p);
			if (!liberites.Any())
			{
				HashSet<Position> chain = GetChain(p);
				foreach (Position sp in chain)
				{
					Stones[sp] = StoneColor.None;
				}
				return chain.Count;
			}
			return 0;
		}

		//Allows move on top of other stone
		//Kills opponents stones
		//Allows multi stone suicide
		internal void PutStone(Position p, StoneColor color)
		{
			if (color == StoneColor.None)
			{
				Stones[p] = StoneColor.None;
				return;
			}
			bool allNeighboursOpponent = Neighbours(p).All(np => Stones[np] == color.Invert());

			Stones[p] = color;
			Position? potentialKo = null;
			int totalCaptureCount = 0;

			//kill
			foreach (Position np in Neighbours(p))
			{
				if (Stones[np] != color.Invert())
					continue;
				int captureCount = RemoveSuffocated(np);
				totalCaptureCount += captureCount;
				if (captureCount > 0)
					potentialKo = np;
			}

			//suicide
			RemoveSuffocated(p);

			if (totalCaptureCount == 1 && allNeighboursOpponent)
				Ko = potentialKo;
			else
				Ko = null;
		}
	}
}
