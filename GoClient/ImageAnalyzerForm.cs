using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AudioLessons;
using BoardImageRecognition;
using Chaos.Image;
using CommonGui.ViewModels;
using Model;
using ScreenShots;
using CommonGui.GameRenderer;

namespace GoClient
{
	public partial class ImageAnalyzerForm : Form
	{
		public ImageAnalyzerForm()
		{
			InitializeComponent();
		}

		IntPtr windowHandle = IntPtr.Zero;
		GoVideoRecorder goRecorder;
		Pixels oldPixels = Pixels.Null;

		VideoCapture capturer = new VideoCapture();

		bool found = false;
		BoardParameters bp = new BoardParameters();
		Size size = Size.Empty;
		DateTime RecordingStart;
		Recorder audioRecorder;

		public TimeSpan Duration
		{
			get
			{
				if (audioRecorder != null)
					return audioRecorder.Duration;
				else if (RecordingStart != new DateTime())
					return DateTime.UtcNow - RecordingStart;
				else
					return TimeSpan.Zero;
			}
		}

		private void start_Click(object sender, EventArgs e)
		{
			AudioCheckBox.Enabled = false;
			RecordButton.Enabled = false;
			FinishButton.Enabled = true;
			goRecorder = new GoVideoRecorder();
			RecordingStart = DateTime.UtcNow;
			timeLabel.Text = "Starting...";
			frameCounter = 0;
			capturer.Release(oldPixels);
			oldPixels = Pixels.Null;
			if (AudioCheckBox.Checked)
			{
				audioRecorder = new Recorder(0.3f);
				audioRecorder.Paused = false;
			}
		}

		private string TS(TimeSpan t)
		{
			return ((int)t.TotalMilliseconds).ToString();
			/*int i=(int)Math.Round(t.TotalSeconds*100);
			Decimal d = (Decimal)i / 100;
			return d.ToString();*/
		}

		int frameCounter;
		private void timer1_Tick(object sender, EventArgs e)
		{
			DateTime start0 = DateTime.UtcNow;
			if (this.Handle != ScreenCapture.GetForegroundWindow() && goRecorder == null)
				windowHandle = ScreenCapture.GetForegroundWindow();
			if (windowHandle == IntPtr.Zero)
				return;
			Window window = new Window(windowHandle);
			WindowTitle.Text = window.Title;
			if (windowHandle != ScreenCapture.GetForegroundWindow())//Capturing background windows is buggy
				return;
			Pixels pix = capturer.Capture(windowHandle);
			//allocStats.Text = "Alloc:" + capturer.CacheMissAllocs + "/" + capturer.TotalAllocs;
			//bmp.Save("ScreenShot.bmp");
			if (!Pixels.DataEquals(oldPixels, pix))
			{
				frameCounter++;
				FrameCounterLabel.Text = frameCounter.ToString();
				BoardInfo board = null;
				ImageToBoardInfo imageToBoardInfo = new ImageToBoardInfo();
				if (!(found && pix.Width == size.Width && pix.Height == size.Height))
				{
					found = imageToBoardInfo.GetBoardParameters(pix, out bp);
					size = pix.Size;
				}
				if (found)
					board = imageToBoardInfo.ProcessImage(bp, pix);
				if (board != null)
				{
					if (goRecorder != null)
					{
						goRecorder.Add(Duration, board);
						goRecorder.Replay.Save("Capture.GoVideo");
					}
					GameState gameState = GoVideoToReplay.BoardToGameState(board);
					StateRenderer renderer = new StateRenderer(new GoClient.Drawing.GraphicsSystem());
					renderer.BlockSize = 16;
					renderer.State = gameState;
					Preview.Image = ((GoClient.Drawing.Bitmap)renderer.Render()).InternalBitmap;
				}
			}
			ProcessingTime.Text = TS(DateTime.UtcNow - start0);
			timeLabel.Text = TimeSpan.FromSeconds(Math.Round(Duration.TotalSeconds)).ToString();
			capturer.Release(oldPixels);
			oldPixels = pix;
		}

		private void FinishButton_Click(object sender, EventArgs e)
		{//Fixme: Check Error handling
			bool wasPaused = false;
			if (audioRecorder != null)
			{
				wasPaused = audioRecorder.Paused;
				audioRecorder.Paused = true;
			}
			if (audioRecorder != null)
			{
				using (SaveFileDialog dlg = new SaveFileDialog())
				{
					dlg.DefaultExt = ".GoLesson";
					dlg.Filter = "Go Audio Lesson|*.GoLesson|All Files|*.*";
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						string replay = goRecorder.Replay.Save();
						audioRecorder.Finish();
						Stream audio = audioRecorder.Data;
						audio.Position = 0;
						AudioLessonFile.Save(dlg.FileName, replay, audio);
						FinishButton.Enabled = false;
						goRecorder = null;
						audioRecorder = null;
					}
				}
			}
			else
			{
				using (SaveFileDialog dlg = new SaveFileDialog())
				{
					dlg.DefaultExt = ".GoReplay";
					dlg.Filter = "Go Replay|*.GoReplay|All Files|*.*";
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						goRecorder.Replay.Save(dlg.FileName);
						FinishButton.Enabled = false;
						goRecorder = null;
					}
				}
			}
			if (audioRecorder != null)
				audioRecorder.Paused = wasPaused;
		}
	}
}
