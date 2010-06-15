using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public sealed class MutableState : GameState
	{
		public void SetStone(Position p, StoneColor value)
		{
			stones[p.X, p.Y] = value;
		}

		public void SetLabel(Position p, string value)
		{
			labels[p.X, p.Y] = value;
		}

		new public StoneColor PlayerToMove { get { return base.PlayerToMove; } set { base.PlayerToMove = value; } }
		new public int MoveIndex { get { return base.MoveIndex; } set { base.MoveIndex = value; } }

		new public Position? Ko { get { return base.Ko; } set { base.Ko = value; } }
		new public int Passes { get { return base.Passes; } set { base.Passes = value; } }

		internal MutableState(Replay game, int width, int height)
			: base(game, width, height)
		{

		}



	}
}
