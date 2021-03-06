﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using AudioLessons;
using System.IO;
using Core;

namespace CommonGui.ViewModels
{
	public class ViewModel : IDisposable
	{
		private static int replayCounter = 1;

		private TimeSpan? lastSaveTime;
		private int lastSaveAction;

		private void SetUnmodified()
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

		public static ViewModel CreateNew()
		{
			Replay replay = new Replay();
			replay.AddAction(new CreateBoardAction(19, 19));
			ViewModel view = OpenReplayInternal(replay);
			view.Name = "New Replay " + replayCounter;
			replayCounter++;
			view.SetUnmodified();
			return view;
		}

		public bool CanAddAudio
		{
			get
			{
				return (Media == null) && (Game.Replay.EndTime == TimeSpan.Zero);
			}
		}

		public void AddAudio()
		{
			if (!CanAddAudio)
				throw new InvalidOperationException();
			Media = new Recorder(0.3f);
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
			view.Media = new Player(audio);
			view.SetUnmodified();
			return view;
		}

		private static ViewModel OpenReplayInternal(Replay replay)
		{
			ViewModel view = new ViewModel();
			view.Editor = new Editor(view);
			view.Editor.ActiveTool = Tools.Move;
			view.Game = new Game(replay);
			view.Game.Seek(view.Game.Replay.Actions.Count - 1);
			return view;
		}

		public static ViewModel OpenReplay(string filename)
		{
			Replay replay = Replay.Load(filename);
			ViewModel view = OpenReplayInternal(replay);
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
			if (Editor != null)
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
			if (Game.Replay.Actions.Count < 3000)
				Game.Replay.Save(GlobalSettings.UserDataDir + "Current.Replay.gor");
			Game.Seek(Game.Replay.Actions.Count - 1);
		}

		public void TogglePause()
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

		public void NavigateVariation(int delta)
		{
			NavigateVariation(delta, (g, i) => true);
		}

		public void NavigateVariationFork(int delta)
		{
			NavigateVariation(delta, (game, index) => game.Tree.Children(index).Count() > 1);
		}

		public void NavigateVariation(int delta, Func<Game, int, bool> filter)
		{
			if (delta == 0)
				return;

			List<int> actions;
			if (delta > 0)
				actions = Game.Tree.VariationFuture().ToList();
			else
				actions = Game.Tree.VariationPast().ToList();
			actions = actions.Where(action => Game.Tree.IsLastActionOfNode(action)).Where(actionIndex => filter(Game, actionIndex)).ToList();
			int index = Math.Abs(delta) - 1;
			if (index > actions.Count - 1)
				index = actions.Count - 1;
			if (index < 0)
				return;

			int newNode = actions[index];
			SendActions(new SelectStateAction(newNode));
		}

		public void SaveWithAudio(string fileName)
		{
			if (Recorder == null || Recorder.State != RecorderState.Finished)
				throw new InvalidOperationException();
			string replay = Game.Replay.Save();
			Stream audio = Recorder.Data;
			audio.Position = 0;
			AudioLessonFile.Save(fileName, replay, audio);
			SetUnmodified();
		}

		public void Dispose()
		{
			if (Media != null)
				Media.Dispose();
		}

		public void SaveWithoutAudio(string fileName)
		{
			Game.Replay.Save(fileName);
			string replay = Game.Replay.Save();
			if (Recorder == null)
				SetUnmodified();
		}
	}
}
