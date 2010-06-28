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
			PlayerToMove = StoneColor.Black;
		}

		internal bool PutStone(Position p, StoneColor color)
		{
			if (color == StoneColor.None)
				throw new ArgumentException();
			Stones[p] = color;
			Position? potentialKo = null;
			int captureCount = 0;
			bool allNeighboursOpponent = Neighbours(p).All(np => Stones[np] == color.Invert());
			//bool koPossible = Neighbours(p).All(np=>Stones[np.x,np.y]);
			foreach (Position np in Neighbours(p))
			{
				if (Stones[np] != color.Invert())
					continue;
				HashSet<Position> chain = GetChain(np);
				IEnumerable<Position> liberites = GetLibertiesOfChain(np);
				if (liberites.Count() == 0)
				{
					captureCount += chain.Count;
					potentialKo = np;
					foreach (Position sp in chain)
					{
						Stones[sp] = StoneColor.None;
					}
				}
			}
			if (GetLibertiesOfChain(p).Count() == 0)//Suicide
			{
				Stones[p] = StoneColor.None;
				return false;
			}

			if (captureCount == 1 && allNeighboursOpponent)
				Ko = potentialKo;
			else
				Ko = null;

			return true;
		}
	}
}
