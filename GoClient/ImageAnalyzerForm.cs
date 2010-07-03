using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChaosUtil;
using System.Drawing.Imaging;
using Model;
using GoClient;
using System.Diagnostics;
using ChaosUtil.Collections;
using System.Threading;
using BoardImageRecognition;
using ScreenShots;
using CommonGui.ViewModels;
using AudioLessons;
using System.IO;

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
		RawColor[,] oldPixels = null;

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
			oldPixels = null;
			frameCounter = 0;
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
			RawColor[,] pix = capturer.Capture(windowHandle);
			//allocStats.Text = "Alloc:" + capturer.CacheMissAllocs + "/" + capturer.TotalAllocs;
			//bmp.Save("ScreenShot.bmp");
			if (oldPixels == null || !ImageToBoardInfo.SamePixels(oldPixels, pix))
			{
				oldPixels=CopyPixels(
				frameCounter++;
				FrameCounterLabel.Text = frameCounter.ToString();
				BoardInfo board = null;
				ImageToBoardInfo imageToBoardInfo = new ImageToBoardInfo();
				if (!(found && pix.GetLength(0) == size.Width && pix.GetLength(1) == size.Height))
				{
					found = imageToBoardInfo.GetBoardParameters(pix, out bp);
					size.Width = pix.GetLength(0);
					size.Height = pix.GetLength(1);
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
					StateRenderer renderer = new StateRenderer();
					renderer.BlockSize = 16;
					renderer.State = gameState;
					Preview.Image = renderer.Render();
				}
			}
			ProcessingTime.Text = TS(DateTime.UtcNow - start0);
			timeLabel.Text = TimeSpan.FromSeconds(Math.Round(Duration.TotalSeconds)).ToString();
			capturer.Release(pix);
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
				SaveFileDialog dlg = new SaveFileDialog();
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
			else
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.DefaultExt = ".GoReplay";
				dlg.Filter = "Go Replay|*.GoReplay|All Files|*.*";
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					goRecorder.Replay.Save(dlg.FileName);
					FinishButton.Enabled = false;
					goRecorder = null;
				}
			}
			if (audioRecorder != null)
				audioRecorder.Paused = wasPaused;
		}
	}
}
