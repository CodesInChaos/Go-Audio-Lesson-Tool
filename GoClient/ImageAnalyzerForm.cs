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

namespace GoClient
{
	public partial class ImageAnalyzerForm : Form
	{
		public ImageAnalyzerForm()
		{
			InitializeComponent();
			//Thread thread = new Thread(WorkerThreadFunc);
			//thread.Priority = ThreadPriority.BelowNormal;
			//thread.Start();
		}

		/*private void button1_Click(object sender, EventArgs e)
		{
			//Copy
			DateTime start0 = DateTime.UtcNow;
			Bitmap bmp = new Bitmap(Preview.Image);
			var graphics = Graphics.FromImage(Preview.Image);
			RawColor[,] pix = BitmapToPixels(bmp);

			Text = (DateTime.UtcNow - start0).ToString();
			DateTime start = DateTime.UtcNow;
			//Get Params
			BoardParameters bp;
			bool foundBoard = GetBoardParameters(pix, out bp);
			BoardInfo board;
			if (foundBoard)
				board = ProcessImage(bp, pix);

			/*
			float[] grayCols, grayRows;
			CountPixels(pix, linedBoardRect, IsGray, out grayCols, out grayRows);
			List<int> cols = new List<int>();
			for (int x = 1; x < bmp.Width - 1; x++)
			{
				if (grayCols[x] > 0.7 && grayCols[x] > grayCols[x - 1] && grayCols[x] >= grayCols[x + 1])
				{
					cols.Add(x);
					graphics.DrawLine(Pens.Red, new Point(x, 0), new Point(x, 10));
				}
			}

			List<int> rows = new List<int>();
			for (int y = 1; y < bmp.Height - 1; y++)
			{
				if (grayRows[y] > 0.7 && grayRows[y] > grayRows[y - 1] && grayRows[y] > grayRows[y + 1])
				{
					cols.Add(y);
					graphics.DrawLine(Pens.Red, new Point(0, y), new Point(10, y));
				}
			}
			Text += " " + (DateTime.UtcNow - start).ToString();
			Preview.Invalidate();
		}*/

		int windowHandle = 0;
		GoVideoRecorder recorder;
		private void button2_Click(object sender, EventArgs e)
		{
			int.TryParse(windowHandleBox.Text, out windowHandle);
			windowHandleBox.Text = windowHandle.ToString();
			recorder = new GoVideoRecorder();
			RecordingStart = DateTime.UtcNow;
		}

		/*void MirrorBoardInfo(BoardInfo info)
		{
			for (int y = 0; y < info.Height / 2; y++)
				for (int x = 0; x < info.Width; x++)
				{
					int mirrorY = info.Height - 1 - y;
					PointInfo p = info.Board[x, y];
					info.Board[x, y] = info.Board[x, mirrorY];
					info.Board[x, mirrorY] = p;
				}
		}*/

		private string TS(TimeSpan t)
		{
			return ((int)t.TotalMilliseconds).ToString();
			/*int i=(int)Math.Round(t.TotalSeconds*100);
			Decimal d = (Decimal)i / 100;
			return d.ToString();*/
		}

		RawColor[,] oldPixels = null;

		//public readonly ThreadSafeQueue<RawColor[,]> Work = new ThreadSafeQueue<RawColor[,]>();
		//public readonly ThreadSafeQueue<BoardInfo> FinishedWork = new ThreadSafeQueue<BoardInfo>();
		//private volatile bool stopWorker = false;
		//private volatile bool workerIdle = false;

		/*private void WorkerThreadFunc()
		{
			Size size = Size.Empty;
			BoardParameters bp = new BoardParameters();
			bool found = false;
			while (!stopWorker)
			{
				RawColor[,] pix;
				while (Work.TryDequeue(out pix))
				{
					BoardInfo board = null;
					DateTime start = DateTime.UtcNow;

					if (!(found && pix.GetLength(0) == size.Width && pix.GetLength(1) == size.Height))
					{
						found = GetBoardParameters(pix, out bp);
						size.Width = pix.GetLength(0);
						size.Height = pix.GetLength(1);
					}
					if (found)
						board = ProcessImage(bp, pix);
					board.ProcessDuration = DateTime.UtcNow - start;
					if (board != null)
						FinishedWork.Enqueue(board);
					pixelPool.Release(pix);
				}
				workerIdle = true;
				Thread.Sleep(100);
				workerIdle = false;
			}
		}*/
		VideoCapture capturer = new VideoCapture();

		bool found = false;
		BoardParameters bp = new BoardParameters();
		Size size = Size.Empty;
		DateTime RecordingStart;

		private void timer1_Tick(object sender, EventArgs e)
		{
			DateTime start0 = DateTime.UtcNow;
			if (this.Handle != ScreenCapture.GetForegroundWindow())
				windowHandleBox.Text = ScreenCapture.GetForegroundWindow().ToString();
			if (windowHandle == 0)
				return;
			if (windowHandle != ScreenCapture.GetForegroundWindow().ToInt32())//Capturing background windows is buggy
				return;
			RawColor[,] pix = capturer.Capture(windowHandle);
			allocStats.Text = "Alloc:" + capturer.CacheMissAllocs + "/" + capturer.TotalAllocs;
			//bmp.Save("ScreenShot.bmp");
			if (oldPixels == null || !ImageToBoardInfo.SamePixels(oldPixels, pix))
			{
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
					recorder.Add(DateTime.UtcNow - RecordingStart, board);
					recorder.Replay.Save("Capture.GoVideo");
					GameState gameState = GoVideoToReplay.BoardToGameState(board);
					StateRenderer renderer = new StateRenderer();
					renderer.BlockSize = 16;
					renderer.BoardSetup = gameState;
					Preview.Image = renderer.Render(gameState);
				}
			}
			Text = TS(DateTime.UtcNow - start0);
			capturer.Release(pix);
		}

		/*private void timer1_Tick(object sender, EventArgs e)
		{
			BoardInfo board;
			while (FinishedWork.TryDequeue(out board))
			{
				recorder.Add(board);
				recorder.video.SaveAsList("Capture.GoVideo");
				GameState gameState = VideoToReplay.BoardToGameState(board);
				StateRenderer renderer = new StateRenderer();
				renderer.BlockSize = 16;
				renderer.BoardSetup = gameState;
				Preview.Image = renderer.Render(gameState);
				lastBoard = board;
			}
			Text = "Queue:" + (workerIdle ? "Idle" : Work.Count.ToString());
			if (lastBoard != null)
			{
				Text += " T=" + TS(lastBoard.ProcessDuration);
			}

			DateTime start0 = DateTime.UtcNow;
			if (this.Handle != ScreenCapture.GetForegroundWindow())
				windowHandleBox.Text = ScreenCapture.GetForegroundWindow().ToString();
			if (windowHandle == 0)
				return;
			if (windowHandle != ScreenCapture.GetForegroundWindow().ToInt32())//Capturing background windows is buggy
				return;
			RawColor[,] pixels = CaptureWindowToPixels(windowHandle);
			allocStats.Text = "Alloc:" + pixelPool.CacheMissCount + "/" + pixelPool.AllocCount;
			//bmp.Save("ScreenShot.bmp");
			if (oldPixels == null || !SamePixels(oldPixels, pixels))
			{
				CopyPixels(pixels, ref oldPixels);
				Work.Enqueue(pixels);
			}
			else
				pixelPool.Release(pixels);
		}*/

		private void CopyPixels(RawColor[,] src, ref RawColor[,] dest)
		{
			if (dest == null || dest.GetLength(0) != src.GetLength(0) || dest.GetLength(1) != src.GetLength(1))
				dest = new RawColor[src.GetLength(0), src.GetLength(1)];
			RawColor[,] localDest = dest;

			for (int y = 0; y < src.GetLength(1); y++)
				for (int x = 0; x < src.GetLength(0); x++)
					localDest[x, y] = src[x, y];
		}


		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			//stopWorker = true;
		}
	}
}
