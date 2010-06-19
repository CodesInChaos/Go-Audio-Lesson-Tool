using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using AudioLessons;
using System.IO;

namespace CommonGui.ViewModels
{
	public class ViewModel
	{
		private static int replayCounter = 1;

		private TimeSpan? lastSaveTime;
		private int lastSaveAction;

		public void SetUnmodified()
		{
			TimeSpan? duration = null;
			if (Recorder != null)
				duration = Recorder.Duration;
			lastSaveTime = duration;
			lastSaveAction = Game.Replay.Actions.Count;
		}

		public bool IsModified()
		{
			bool replaySaved = Game.Replay.Actions.Count == lastSaveAction;
			TimeSpan? duration = null;
			if (Recorder != null)
				duration = Recorder.Duration;
			bool audioSaved = duration == lastSaveTime;
			return !(replaySaved && audioSaved);
		}

		public static ViewModel CreateReplay()
		{
			ViewModel view = new ViewModel();
			view.Editor = new Editor(view);
			view.Editor.ActiveTool = Tools.Move;
			view.Game = new Game(new Replay());
			view.SendActions(new InitStateAction(19, 19));
			view.Game.Seek(0);
			view.Name = "New Replay " + replayCounter;
			replayCounter++;
			view.SetUnmodified();
			return view;
		}

		public static ViewModel CreateLesson()
		{
			ViewModel view = CreateReplay();
			view.Media = new Recorder(view, 0.3f);
			view.Name = "New Lesson " + replayCounter;
			view.SetUnmodified();
			return view;
		}

		public static ViewModel PlayLesson(String filename)
		{
			string replay;
			Stream audio;
			AudioLessonFile.Load(filename, out replay, out audio);
			ViewModel view = new ViewModel();
			view.Game = new Game(Replay.Parse(replay));
			view.Game.Seek(0);
			view.Name = Path.GetFileName(filename);
			view.Media = new Player(null);
			view.SetUnmodified();
			return view;
		}

		public static ViewModel PlayReplay(string filename)
		{
			ViewModel view = new ViewModel();
			view.Game = new Game(Replay.Load(filename));
			view.Game.Seek(0);
			view.Name = Path.GetFileName(filename);
			view.SetUnmodified();
			return view;
		}

		public Game Game { get; private set; }
		public TimeSpan Time
		{
			get
			{
				if (Recorder != null)
					return Recorder.Duration;
				if (Player != null)
					return Player.Position;
				return TimeSpan.Zero;
			}
		}

		public TimeSpan Duration
		{
			get
			{
				TimeSpan mediaDuration = TimeSpan.Zero;
				if (Media != null)
					mediaDuration = Media.Duration;
				TimeSpan replayDuration = Game.Replay.EndTime;
				if (replayDuration > mediaDuration)
					return replayDuration;
				else
					return mediaDuration;
			}
		}
		public Editor Editor { get; private set; }
		public Media Media { get; private set; }
		public Player Player { get { return Media as Player; } }
		public Recorder Recorder { get { return Media as Recorder; } }
		public string Name { get; set; }

		public ViewModel()
		{
			Name = "Unnamed Audiolesson";
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
