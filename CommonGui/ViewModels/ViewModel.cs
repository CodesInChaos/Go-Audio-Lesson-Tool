using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace CommonGui.ViewModels
{
	public class ViewModel
	{
		public static ViewModel CreateReplay()
		{
			ViewModel view = new ViewModel();
			view.Game = new Game(new Replay());
			view.SendActions(new InitStateAction(19, 19));
			return view;
		}

		public static ViewModel CreateLesson()
		{
			ViewModel view = CreateReplay();
			view.Media = new Recorder(view);
			return view;
		}

		public static ViewModel OpenLesson()
		{
			throw new NotImplementedException();
		}

		public static ViewModel OpenReplay()
		{
			throw new NotImplementedException();
		}

		public Game Game { get; set; }
		private TimeSpan mTime;
		public TimeSpan Time { get; set; }
		public Editor Editor { get; set; }
		public Media Media { get; set; }
		public Player Player { get { return Media as Player; } }
		public Recorder Recorder { get { return Media as Recorder; } }

		public ViewModel()
		{
		}

		public void Timer()
		{
		}

		public void SelectTool(Tool tool)
		{
			Editor.ActiveTool = tool;
		}

		public void Pass()
		{
			throw new NotImplementedException();
		}

		public void Resign()
		{
			throw new NotImplementedException();
		}

		public void ToolClick(int actionIndex, Position position)
		{
			throw new NotImplementedException();
		}

		public void SendActions(IEnumerable<GameAction> actions)
		{
			ReceiveActions(actions);
		}

		public void SendActions(params GameAction[] actions)
		{
			SendActions(actions.AsEnumerable());
		}

		public void ReceiveActions(IEnumerable<GameAction> actions)
		{
			foreach (GameAction action in actions)
				Game.Replay.AddAction(action);
		}

		public void TogglePause()
		{
			throw new NotImplementedException();
		}

		public void FinishAndSave()
		{
			throw new NotImplementedException();
		}

		public void ClearGame()
		{
			throw new NotImplementedException();
		}

		public void LoadGameFromSgf(string sgf)
		{
			throw new NotImplementedException();
		}

		public string SaveGameToSgf()
		{
			throw new NotImplementedException();
		}

		public void AddGameFromSgs(string sgf)
		{
			throw new NotImplementedException();
		}
	}
}
