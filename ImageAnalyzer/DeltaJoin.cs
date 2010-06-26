using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Diagnostics;

namespace ImageAnalyzer
{
	public class DeltaJoin
	{
		public readonly Game Game;
		public Replay Replay { get { return Game.Replay; } }

		/*public Position? CheckForMove(GameState old, BoardInfo scan)
		{
			if (old.Width != scan.Width || old.Height != scan.Height)
				return null;
			HashSet<Position> removes = new HashSet<Position>();
			HashSet<Position> adds = new HashSet<Position>();
			HashSet<Position> deads = new HashSet<Position>();
			for (int y = 0; y < old.Height; y++)
			{
				int ys = scan.Height - 1 - y;
				for (int x = 0; x < old.Width; x++)
				{
					if (old.Stones[x, y] == scan.Board[x, ys])
						continue;
					if (scan.Board[x, ys].StoneColor == StoneColor.None)
						removes.Add(new Position(x, y));
					else
						adds.Add(new Position(x, y));
				}
			}
			if (adds.Count != 1)
				return null;
			Position p = adds.Single();
			foreach (Position np in old.Neighbours(p))
			{
				if (old.Stones[np] != color.Invert())
					continue;
				HashSet<Position> chain = GetChain(np);
				IEnumerable<Position> liberites = GetLibertiesOfChain(np);
				if (liberites.Count() == 0)
				{
					foreach (Position sp in chain)
					{
						deads.Add(sp);
					}
				}
			}
			if (!deads.SetEquals(removes))
				return null;
			return np;
		}*/

		double ChangeCost(BoardInfo old, BoardInfo scan)
		{
			if (old.Width != scan.Width || old.Height != scan.Height)
				return double.PositiveInfinity;
			double result = 0;
			for (int y = 0; y < scan.Height; y++)
				for (int x = 0; x < scan.Width; x++)
				{
					if (scan.Board[x, y].StoneColor != old.Board[x, y].StoneColor)
					{
						if (scan.Board[x, y].StoneColor == StoneColor.None)
							result += 0.001;//fixme: Cheap because it includes captures
						else
							result += 1000;
					}
					if (scan.Board[x, y].Marker != old.Board[x, y].Marker)
						result += 1;
				}
			return result;
		}

		Dictionary<int, BoardInfo> Images = new Dictionary<int, BoardInfo>(100);

		static string MarkerToString(Marker marker)
		{
			switch (marker)
			{
				case Marker.Unknown:
					throw new NotSupportedException();
				case Marker.None:
					return "";
				case Marker.Square:
					return "#SQ";
				case Marker.Circle:
					return "#CI";
				case Marker.Triangle:
					return "#TR";
				default:
					throw new NotImplementedException();
			}
		}

		static Marker StringToMarker(string s)
		{
			switch (s)
			{
				case "#SQ":
					return Marker.Square;
				case "#CI":
					return Marker.Circle;
				case "#TR":
					return Marker.Triangle;
				default:
					return Marker.None;
			}
		}

		void FillImages()
		{
			for (int i = 0; i < Replay.Actions.Count; i++)
			{
				if (!(Replay.Actions[i] is GameStateAction))
					continue;
				if (Images.ContainsKey(i))
					continue;
				Game.Seek(i);
				GameState state = Game.State;
				BoardInfo info = new BoardInfo(state.Width, state.Height);
				for (int y = 0; y < state.Height; y++)
					for (int x = 0; x < state.Width; x++)
					{
						info.Board[x, y].StoneColor = state.Stones[x, y];
						string label = state.Labels[x, y];
						info.Board[x, y].Marker = StringToMarker(label);
					}
				Images.Add(i, info);
			}
		}

		void FindParent(BoardInfo scan, out int? parentIndex, out double changeCost)
		{
			FillImages();
			changeCost = double.PositiveInfinity;
			parentIndex = null;
			foreach (var kv in Images)
			{
				double cost = ChangeCost(kv.Value, scan);
				if (cost < changeCost)
				{
					changeCost = cost;
					parentIndex = kv.Key;
				}
			}
			if (double.IsPositiveInfinity(changeCost))
				parentIndex = null;
		}

		public void Add(TimeSpan? time, BoardInfo scan)
		{
			int? parent;
			double changeCost;
			FindParent(scan, out parent, out changeCost);
			if (changeCost == 0)
				return;
			if (time != null)
				Replay.SetEndTime((TimeSpan)time);
			if (parent == null)
			{
				Replay.AddAction(new InitStateAction(scan.Width, scan.Height));
				parent = Replay.Actions.Count - 1;
			}
			else
			{
				Replay.AddAction(new SelectStateAction((int)parent));
			}
			FillImages();
			BoardInfo old = Images[(int)parent];
			Debug.Assert(old.Width == scan.Width && old.Height == scan.Height);
			List<GameAction> labelActions = new List<GameAction>();
			List<GameAction> addActions = new List<GameAction>();
			List<GameAction> removeActions = new List<GameAction>();
			for (int y = 0; y < scan.Height; y++)
				for (int x = 0; x < scan.Width; x++)
				{
					PointInfo oldPoint = old.Board[x, y];
					PointInfo newPoint = scan.Board[x, y];
					if (newPoint.Marker != oldPoint.Marker && newPoint.Marker != Marker.Unknown)
						Replay.AddAction(new LabelAction(new Position(x, y), MarkerToString(newPoint.Marker)));
					if (newPoint.StoneColor != oldPoint.StoneColor)
					{
						var action = new SetStoneAction(new Position(x, y), newPoint.StoneColor);
						if (action.Color == StoneColor.None)
							removeActions.Add(action);
						else
							addActions.Add(action);
					}
				}
			//Removes first so we don't accidentially capture stones/block moves
			//Labels last so they end up in the same node
			IEnumerable<GameAction> actionsToAdd = removeActions.Concat(addActions).Concat(labelActions);
			foreach (GameAction action in actionsToAdd)
				Replay.AddAction(action);
		}

		public DeltaJoin(Game game)
		{
			Game = game;
		}
	}
}
