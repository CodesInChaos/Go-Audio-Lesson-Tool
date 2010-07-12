﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AudioLessons;
using CommonGui.ViewModels;
using Model;
using csvorbis;
using Chaos.Util;
using Chaos.Util.Mathematics;
using System.Text;
using BoardImageRecognition;

namespace GoClient
{
	public partial class GameForm : Form
	{
		const int TreeBlockSize = 20;

		StateRenderer ren = new StateRenderer();
		public readonly ViewModel View;
		public Game Game { get { return View.Game; } }

		private static string TimeSpanToString(TimeSpan t)
		{
			return TimeSpan.FromSeconds(Math.Ceiling(t.TotalSeconds)).ToString();
		}

		private void FormUpdate()
		{
			Text = View.Name;
			bool isGame = Game != null;
			bool canEdit = View.Editor != null;
			bool isRecording = View.Recorder != null;

			if (View.Player != null)
				PlayTimeLabel.Text = TimeSpanToString(View.Time) + "/" + TimeSpanToString(View.Duration);

			UpdateActiveToolMenuItem();
			RecordingBox.Visible = isRecording;
			PlayBox.Visible = View.Player != null;
			FinishLessonMenuItem.Visible = isRecording;
			PauseLessonMenuItem.Enabled = View.Media != null;
			CloseLessonMenuItem.Visible = !isRecording;
			CloseLessonMenuItem.Enabled = isGame && !isRecording;
			CancelLessonMenuItem.Visible = isRecording;
			toolsToolStripMenuItem.Visible = canEdit;
			navigationToolStripMenuItem.Visible = isGame;
			GameMenuItem.Visible = isGame;
			ActionMenuItem.Visible = canEdit;
			ClearGameMenuItem.Visible = canEdit;
			AddGameMenuItem.Visible = canEdit;
			LoadGameMenuItem.Visible = canEdit;
			if (View.Media != null)
			{
				PauseLessonMenuItem.Checked = View.Media.Paused;
				if (View.Media.Paused)

					PlayButton.Text = "Play";
				else
					PlayButton.Text = "Pause";
			}
			if (View.Player != null)
			{
				PlayProgress.Value = (int)Math.Ceiling(View.Player.Position.TotalSeconds);
			}
			if (View.Recorder != null)
			{
				RecordingState.Text = View.Recorder.State.ToString();
				switch (View.Recorder.State)
				{
					case (RecorderState.Recording):
						RecordButton.Text = "Pause";
						break;
					case (RecorderState.Paused):
						RecordButton.Text = "Record";
						break;
					case (RecorderState.Finished):
						RecordButton.Text = "Finished";
						break;
				}
				RecordButton.Enabled = View.Recorder.State != RecorderState.Finished;
				PauseLessonMenuItem.Enabled = RecordButton.Enabled;
				RecordTimeLabel.Text = TimeSpanToString(View.Duration);
			}
			//OpenLessonMenuItem.Visible = Mode == UsageMode.Record;
		}


		public GameForm(ViewModel view)
		{
			View = view;
			InitializeComponent();
			menuStrip1.Visible = false;
			Game.Replay.OnActionAdded += game_OnActionAdded;
			if (View.Player != null)
			{
				PlayProgress.Maximum = (int)Math.Ceiling(View.Duration.TotalSeconds);
			}
			//this.Paint += Form1_Paint;
			//this.FormChanged();
			//view.Changed += view_Changed;
		}

		void game_OnActionAdded(Replay replay, int actionIndex)
		{
			//game.Save(@"C:\Dokumente und Einstellungen\W\Desktop\GoReplay.gor");
			Game.Replay.Save("Current.Replay.gor");
			Game.Seek(Game.Replay.Actions.Count - 1);
			/*if (view.ActiveAction.Action == null || view.ActiveAction == new ActionReference(Game.Replay, Game.Replay.Actions.Count - 2))
			{
				view.ActiveAction = actionReference;
				//view.State = view.ActiveAction.State;
			}*/
		}

		private int renderedAction = -1;
		private float lastSize;

		private void RenderField()
		{
			if (Game == null || Game.State == null)
			{
				Field.Image = null;
				return;
			}
			float boardSize = Math.Min(Field.Height * 1.0f, this.ClientSize.Width - 200f);
			if (Game.SelectedAction == renderedAction && lastSize == boardSize)
				return;
			renderedAction = Game.SelectedAction;
			lastSize = boardSize;
			ren.BlockSize = boardSize / (float)(Game.State.Height - 1 + 3);
			ren.State = Game.State;
			//ren.active = ren.ImageToGame();
			Field.Width = (int)Math.Round(ren.BlockSize * (Game.State.Width - 1 + 3));
			Field.Image = ren.Render();
			Field.Refresh();
			MoveIndex.Text = "Move " + Game.State.MoveIndex;
			PlayerToMove.Text = Game.State.PlayerToMove + " to move";

			GameTreePaintBox.AutoScrollMinSize = new Size(TreeBlockSize * Game.Tree.Width, TreeBlockSize * Game.Tree.Height);

			if (Game.Tree.SelectedNode != null)
			{
				Vector2i pos = Game.Tree.PositionOfNode((int)Game.Tree.SelectedNode);
				int sx = -(TreeBlockSize * (pos.X + 1) - GameTreePaintBox.ClientSize.Width);
				int sy = -(TreeBlockSize * (pos.Y + 1) - GameTreePaintBox.ClientSize.Height);
				Point scroll = GameTreePaintBox.AutoScrollPosition;
				if (scroll.X > sx)
					scroll.X = sx;
				if (scroll.Y > sy)
					scroll.Y = sy;
				GameTreePaintBox.AutoScrollPosition = new Point(-scroll.X, -scroll.Y);
				//AutoScrollPosition has some inconsistant sign convention...
			}
			GameTreePaintBox.Invalidate();
		}

		private void Field_Click(object sender, EventArgs e)
		{
			if (View.Editor == null || View.Editor.ActiveTool == null)
				return;
			var mE = (MouseEventArgs)e;
			PointF pf = ren.ImageToGame(mE.X, mE.Y - (Field.Height - Field.Image.Height) / 2);
			Position p = new Position((int)Math.Round(pf.X), (int)Math.Round(pf.Y));
			int actionIndex = 0;
			if (mE.Button == MouseButtons.Left)
				actionIndex = 1;
			if (mE.Button == MouseButtons.Right)
				actionIndex = 2;
			if (mE.Button == MouseButtons.Left && ModifierKeys == Keys.Shift)
				actionIndex = 2;
			if (mE.Button == MouseButtons.Middle)
				actionIndex = 3;
			if (mE.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
				actionIndex = 3;
			if (actionIndex == 0)
				return;
			if (!Game.State.IsPositionValid(p))
				return;
			List<GameAction> actions = View.Editor.ActiveTool.Click(Game.State, actionIndex, p).ToList();
			View.Editor.AddActions(actions);
		}

		private Tool lastActiveTool;
		private void UpdateActiveToolMenuItem()
		{
			toolsToolStripMenuItem.Visible = View.Editor != null;
			if (View.Editor == null)
				return;
			Tool activeTool = View.Editor.ActiveTool;
			if (lastActiveTool == activeTool)
				return;
			lastActiveTool = activeTool;
			MoveToolMenuItem.Checked = activeTool == Tools.Move;
			PutStoneToolMenuItem.Checked = activeTool == Tools.Edit;
			ScoreToolMenuItem.Checked = activeTool == Tools.Score;
			TriangleToolMenuItem.Checked = activeTool == Tools.Triangle;
			SquareToolMenuItem.Checked = activeTool == Tools.Square;
			CircleToolMenuItem.Checked = activeTool == Tools.Circle;
			TextLabelToolMenuItem.Checked = activeTool == Tools.Text;
			NumberLabelToolMenuItem.Checked = activeTool == Tools.Number;
			SymbolLabelToolMenuItem.Checked = activeTool == Tools.Symbol;
		}

		private void moveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Move);
		}

		private void putStoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Edit);
		}

		private void scoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Score);
		}

		private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Triangle);
		}

		private void squareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Square);
		}

		private void circleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Circle);
		}

		private void textLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Text);
		}

		private void numberLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Number);
		}

		private void symbolLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			View.SelectTool(Tools.Symbol);
		}


		private void StartRecordingButton_Click(object sender, EventArgs e)
		{
			AudioLessonFile.Save("test.goal", "abc", new MemoryStream());
			/*if (encoderState == null)
			{
				view.Controller = new RecordController(view);
			}
			else
			{
				var controller = (RecordController)view.Controller;
			}*/
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (View.Player != null)
				View.Game.Seek(View.Player.Position);
			FormUpdate();
			RenderField();

			/*if (Mode == UsageMode.Play)
			{
				view.Time += TimeSpan.FromMilliseconds(timer1.Interval);//Fixme
				while (view.ActiveAction != null)
				{
					ReplayTimeAction ac = view.ActiveAction.Action as ReplayTimeAction;
					if (ac != null && ac.Time > view.Time)
						break;
					view.ActiveAction++;
				}
			}*/
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void ExitClick(object sender, EventArgs e)
		{
			Close();
		}

		private void FinishLessonMenuItem_Click(object sender, EventArgs e)
		{
			bool wasPaused = View.Recorder.Paused;
			if (View.Recorder.State != RecorderState.Finished)
				View.Recorder.Paused = true;
			if (SaveAudioLessonDialog.ShowDialog() == DialogResult.OK)
			{
				string replay = Game.Replay.Save();
				View.Recorder.Finish();
				Stream audio = View.Recorder.Data;
				audio.Position = 0;
				AudioLessonFile.Save(SaveAudioLessonDialog.FileName, replay, audio);
				View.SetUnmodified();
			}
			if (View.Recorder.State != RecorderState.Finished)
				View.Recorder.Paused = wasPaused;
		}


		private void RecordButton_Click(object sender, EventArgs e)
		{
			View.Recorder.Paused = !View.Recorder.Paused;
		}

		private void PlayButton_Click(object sender, EventArgs e)
		{
			View.Player.Paused = !View.Player.Paused;
		}

		private void CancelLessonMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CloseLessonMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (View.IsModified())
			{
				//Fixme: Use better dialog
				DialogResult result = MessageBox.Show(
					this,
					"Are you sure you want to quit?\r" + View.Name + " has not been saved.",
					"Close Confirmation",
					MessageBoxButtons.OKCancel);
				e.Cancel = result != DialogResult.OK;
			}
		}

		private void PlayProgress_Scroll(object sender, EventArgs e)
		{
			View.Player.Seek(TimeSpan.FromSeconds(PlayProgress.Value));
		}

		private void PassActionMenuItem_Click(object sender, EventArgs e)
		{
			var actions = new List<GameAction>();
			actions.Add(new PassMoveAction(Game.State.PlayerToMove));
			if (View.Game.State.Labels.Any(s => s != null))
				actions.Add(LabelAction.ClearLabels);
			View.Editor.AddActions(actions);
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			View.Dispose();
		}

		private void ScreenshotMenuItem_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.DefaultExt = ".png";
				dialog.Filter = "PNG Images|*.png|All Files|*.*";
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					Field.Image.Save(dialog.FileName);
				}
			}
		}

		private void GameTreeBox_Paint(object sender, PaintEventArgs e)
		{
			TreeRenderer treeRenderer = new TreeRenderer();
			treeRenderer.Scroll = GameTreePaintBox.AutoScrollPosition;
			treeRenderer.Graphics = e.Graphics;
			treeRenderer.ClipRect = e.ClipRectangle;
			treeRenderer.Game = Game;
			treeRenderer.BlockSize = TreeBlockSize;
			treeRenderer.Render();
		}

		private void GameTreePaintBox_Click(object sender, EventArgs e)
		{
			if (View.Editor == null)
				return;
			var mE = (MouseEventArgs)e;
			Point pixelPos = mE.Location;
			int x = (pixelPos.X - GameTreePaintBox.AutoScrollPosition.X) / TreeBlockSize;
			int y = (pixelPos.Y - GameTreePaintBox.AutoScrollPosition.Y) / TreeBlockSize;
			int? node = Game.Tree.NodeAtPosition(new Vector2i(x, y));
			if (node == null)
				return;
			View.Editor.AddActions(new GameAction[] { new SelectStateAction((int)node) });
		}

		private void AddGameMenuItem_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				dlg.Filter = "Smart Game Format (*.sgf)|*.sgf|All Files|*.*";
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					string sgf = File.ReadAllText(dlg.FileName, Encoding.UTF8);
					SgfLoader.Load(sgf, Game.Replay);
				}
			}
		}

		private void mergeVideoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GoVideoToReplay converter = new GoVideoToReplay(View.Game);
		}
	}
}