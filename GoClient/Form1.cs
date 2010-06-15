using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AudioLessons;
using CommonGui.ViewModels;
using Model;

namespace GoClient
{
	enum UsageMode
	{
		None,
		Play,
		Record
	}

	public partial class Form1 : Form
	{
		StateRenderer ren = new StateRenderer();
		ViewModel view;
		UsageMode Mode;

		bool mChanged = true;

		private void FormChanged()
		{
			mChanged = true;
			Invalidate();
		}

		private static string TimeSpanToString(TimeSpan t)
		{
			return TimeSpan.FromSeconds(Math.Ceiling(t.TotalSeconds)).ToString();
		}

		private void FormUpdate()
		{
			bool isGame = Mode != UsageMode.None;
			bool canEdit = view.Editor != null;
			bool isRecording = view.Recorder != null;

			if (!mChanged)
				return;
			mChanged = false;
			RenderField();
			if (view.Player != null)
				PlayTimeLabel.Text = TimeSpanToString(view.Time) + "/" + TimeSpanToString(view.Media.Duration);

			UpdateActiveToolMenuItem();
			RecordingBox.Visible = view.Recorder != null;
			PlayBox.Visible = view.Player != null;
			FinishLessonMenuItem.Visible = isRecording;
			PauseLessonMenuItem.Enabled = view.Media != null;
			CloseLessonMenuItem.Visible = !isRecording;
			CloseLessonMenuItem.Enabled = isGame && !isRecording;
			CancelLessonMenuItem.Visible = isRecording;
			NewLessonMenuItem.Visible = !isRecording;
			toolsToolStripMenuItem.Visible = canEdit;
			navigationToolStripMenuItem.Visible = isGame;
			GameMenuItem.Visible = isGame;
			ActionMenuItem.Visible = canEdit;
			ClearGameMenuItem.Visible = canEdit;
			AddGameMenuItem.Visible = canEdit;
			LoadGameMenuItem.Visible = canEdit;
			if (view.Media != null)
			{
				PauseLessonMenuItem.Checked = view.Media.Paused;
			}
			if (view.Recorder != null)
			{
				RecordingState.Text = view.Recorder.State.ToString();
				switch (view.Recorder.State)
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
				RecordButton.Enabled = view.Recorder.State != RecorderState.Finished;
				PauseLessonMenuItem.Enabled = RecordButton.Enabled;
				RecordTimeLabel.Text = TimeSpanToString(view.Recorder.Duration);
			}
			//OpenLessonMenuItem.Visible = Mode == UsageMode.Record;
		}


		public Form1()
		{
			view = new ViewModel((a) => Invoke(a));
			view.Replay = new Replay();
			view.Replay.OnActionAdded += game_OnActionAdded;
			view.Replay.AddAction(new InitStateAction(19, 19));
			view.Editor = new Editor(view);
			view.Editor.ActiveTool = Tools.Move;
			InitializeComponent();
			this.Paint += Form1_Paint;
			this.FormChanged();
			view.Changed += view_Changed;
		}

		void view_Changed(object sender, ModelChangedEventArgs e)
		{
			FormChanged();
		}

		void Form1_Paint(object sender, PaintEventArgs e)
		{
			FormUpdate();
		}

		void game_OnActionAdded(ActionReference actionReference)
		{
			//game.Save(@"C:\Dokumente und Einstellungen\W\Desktop\GoReplay.gor");
			view.Replay.Save("Current.Replay.gor");
			if (view.ActiveAction.Action == null || view.ActiveAction == new ActionReference(view.Replay, view.Replay.Actions.Count - 2))
			{
				view.ActiveAction = actionReference;
				//view.State = view.ActiveAction.State;
			}
		}

		private Size lastFieldSize;

		private void RenderField()
		{
			ren.BlockSize = (float)Field.Height / (float)(view.State.Height - 1 + 3);
			Field.Width = (int)Math.Round(ren.BlockSize * (view.State.Width - 1 + 3));
			Field.Image = ren.Render(view.State);
			Field.Refresh();
			MoveIndex.Text = "Move " + view.State.MoveIndex;
			PlayerToMove.Text = view.State.PlayerToMove + " to move";
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			if (Field.Size != lastFieldSize)
				FormChanged();
			lastFieldSize = Field.Size;
		}

		private void Field_Click(object sender, EventArgs e)
		{
			var mE = (MouseEventArgs)e;
			PointF pf = ren.ImageToGame(mE.X, mE.Y);
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
			if (!view.State.IsPositionValid(p))
				return;
			List<GameAction> actions = view.Editor.ActiveTool.Click(view.State, actionIndex, p).ToList();
			if (actions.Count == 0)
				return;
			view.Replay.EndTime = view.Time;
			view.Editor.AddActions(actions);
			FormChanged();
		}

		private void UpdateActiveToolMenuItem()
		{
			Tool activeTool = view.Editor.ActiveTool;
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
			view.Editor.ActiveTool = Tools.Move;
		}

		private void putStoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Edit;
		}

		private void scoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Score;
		}

		private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Triangle;
		}

		private void squareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Square;
		}

		private void circleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Circle;
		}

		private void textLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Text;
		}

		private void numberLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Number;
		}

		private void symbolLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			view.Editor.ActiveTool = Tools.Symbol;
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
			if (OpenAudioLessonDialog.ShowDialog() == DialogResult.OK)
			{
				string replay;
				Stream audio;
				AudioLessonFile.Load(OpenAudioLessonDialog.FileName, out replay, out audio);
			}
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
			if (SaveAudioLessonDialog.ShowDialog() == DialogResult.OK)
			{
				string replay = view.Replay.Save();
				view.Recorder.Finish();
				Stream audio = view.Recorder.Data;
				audio.Position = 0;
				AudioLessonFile.Save(SaveAudioLessonDialog.FileName, replay, audio);
			}
		}

		private void NewAudiolessonMenuItem_Click(object sender, EventArgs e)
		{
			Mode = UsageMode.Record;
			view.Media = new Recorder(view);
			FormChanged();
		}

		private void RecordButton_Click(object sender, EventArgs e)
		{
			view.Recorder.Paused = !view.Recorder.Paused;
			FormChanged();
		}

		private void PlayButton_Click(object sender, EventArgs e)
		{
		}
	}
}
