using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Model
{
	public class StateDelta
	{
		public static List<GameAction> Delta(GameState oldState, GameState newState)
		{
			if (newState == null && oldState == null)
				return new List<GameAction>();
			if (newState == null)
				throw new ArgumentNullException();
			List<GameAction> result = new List<GameAction>();
			List<GameAction> labelActions = new List<GameAction>();
			List<GameAction> addActions = new List<GameAction>();
			List<GameAction> removeActions = new List<GameAction>();
			if (oldState == null || oldState.Width != newState.Width || oldState.Height != newState.Height)
			{
				result.Add(new CreateBoardAction(newState.Width, newState.Height));
				oldState = new GameState(newState.Width, newState.Height);
			}
			for (int y = 0; y < newState.Height; y++)
				for (int x = 0; x < newState.Width; x++)
				{
					if (newState.Labels[x, y] != oldState.Labels[x, y])
						result.Add(new LabelAction(new Position(x, y), newState.Labels[x, y]));
					if (newState.Stones[x, y] != oldState.Stones[x, y])
					{
						var action = new SetStoneAction(new Position(x, y), newState.Stones[x, y]);
						if (action.Color == StoneColor.None)
							removeActions.Add(action);
						else
							addActions.Add(action);
					}
				}
			//Removes first so we don't accidentially capture stones/block moves
			//Labels last so they end up in the same node
			result.AddRange(removeActions);
			result.AddRange(addActions);
			result.AddRange(labelActions);
			return result;
		}
	}
}
