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
using ScreenShotDemo;
using GoClient;
using System.Diagnostics;
using ChaosUtil.Collections;
using System.Threading;

namespace ImageAnalyzer
{
	public partial class ImageAnalyzerForm : Form
	{
		public ImageAnalyzerForm()
		{
			InitializeComponent();
			Thread thread = new Thread(WorkerThreadFunc);
			thread.Priority = ThreadPriority.BelowNormal;
			thread.Start();
		}

		int DistanceSquared(RawColor c1, RawColor c2)
		{
			int dR = c1.R - c2.R;
			int dG = c1.G - c2.G;
			int dB = c1.B - c2.B;
			return dR * dR + dG * dG + dB * dB;
		}

		private readonly RawColor BoardColor = RawColor.FromRgb(220, 180, 90);

		bool IsGray(RawColor c)
		{
			int sum = c.R + c.G + c.B;
			int sqrSum = (c.R * c.R + c.G * c.G + c.B * c.B) * 3;
			int var9 = sqrSum - sum * sum;//Var9=9*Variance
			return var9 < 3000;
		}

		bool IsBoard(RawColor c)
		{
			return DistanceSquared(c, BoardColor) < 400;
		}

		private void CountPixels(RawColor[,] pix, System.Drawing.Rectangle rect, Predicate<RawColor> filter, out float[] cols, out float[] rows)
		{
			int bmpWidth = pix.GetLength(0);
			int bmpHeight = pix.GetLength(1);
			int[] colsI = new int[bmpWidth];
			int[] rowsI = new int[bmpHeight];
			for (int y = rect.Top; y < rect.Bottom; y++)
			{
				for (int x = rect.Left; x < rect.Right; x++)
				{
					RawColor c = pix[x, y];
					if (filter(c))
					{
						colsI[x]++;
						rowsI[y]++;
					}
				}
			}
			cols = new float[bmpWidth];
			rows = new float[bmpHeight];
			for (int x = 0; x < cols.Length; x++)
			{
				if (x < rect.Left || x > rect.Right - 1)
					cols[x] = float.NaN;
				else
					cols[x] = (float)colsI[x] / rect.Height;
			}
			for (int y = 0; y < rows.Length; y++)
			{
				if (y < rect.Top || y > rect.Bottom - 1)
					rows[y] = float.NaN;
				else
					rows[y] = (float)rowsI[y] / rect.Width;
			}
		}

		System.Drawing.Rectangle FindBoard(RawColor[,] pix)
		{
			int bmpWidth = pix.GetLength(0);
			int bmpHeight = pix.GetLength(1);

			float[] boardCols, boardRows;
			CountPixels(pix, new System.Drawing.Rectangle(0, 0, bmpWidth, bmpHeight), IsBoard, out boardCols, out boardRows);
			float max = boardCols.Max();
			if (max < 0.1)
				return System.Drawing.Rectangle.Empty;
			int left = (int)boardCols.FirstIndex(f => f >= 0.8f * max);
			int right = (int)boardCols.LastIndex(f => f >= 0.8f * max) + 1;

			CountPixels(pix, new System.Drawing.Rectangle(left, 0, right - left, bmpHeight), IsBoard, out boardCols, out boardRows);
			max = boardRows.Max();
			int top = (int)boardRows.FirstIndex(f => f >= 0.8f * max);
			int bottom = (int)boardRows.LastIndex(f => f >= 0.8f * max) + 1;

			return System.Drawing.Rectangle.FromLTRB(left, top, right, bottom);
		}

		System.Drawing.Rectangle FindLinedBoard(RawColor[,] pix, System.Drawing.Rectangle boardRect, out float[] grayCols, out float[] grayRows)
		{
			CountPixels(pix, boardRect, IsGray, out grayCols, out grayRows);
			float[] grayCols2 = grayCols;
			float[] grayRows2 = grayRows;
			int left = (int)grayCols.FirstIndex((f, i) => IsLine(grayCols2, i));
			int right = (int)grayCols.LastIndex((f, i) => IsLine(grayCols2, i)) + 1;
			int top = (int)grayRows.FirstIndex((f, i) => IsLine(grayRows2, i));
			int bottom = (int)grayRows.LastIndex((f, i) => IsLine(grayRows2, i)) + 1;
			return System.Drawing.Rectangle.FromLTRB(left, top, right, bottom);
		}

		int[,] CalculateRadii(Func<int, int, int> radiusFunc, int halfSize)
		{
			int size = 2 * halfSize + 1;
			int[,] result = new int[size, size];
			for (int y = -halfSize; y <= +halfSize; y++)
				for (int x = -halfSize; x <= +halfSize; x++)
					result[x + halfSize, y + halfSize] = radiusFunc(x, y);
			return result;
		}

		RawColor[] RadiusColor(RawColor[,] pix, Point center, int[,] radiusData, int maxRadius)
		{
			int[] counts = new int[maxRadius + 1];
			int[] sumR = new int[maxRadius + 1];
			int[] sumG = new int[maxRadius + 1];
			int[] sumB = new int[maxRadius + 1];
			RawColor[] result = new RawColor[maxRadius + 1];
			int halfSize = (radiusData.GetLength(0) - 1) / 2;
			int dx = halfSize - center.X;
			int dy = halfSize - center.Y;
			int left = Math.Max(0, center.X - halfSize);
			int top = Math.Max(0, center.Y - halfSize);
			int right = Math.Min(pix.GetLength(0), center.X + halfSize + 1);
			int bottom = Math.Min(pix.GetLength(1), center.Y + halfSize + 1);
			for (int y = top; y < bottom; y++)
				for (int x = left; x < right; x++)
				{
					RawColor c = pix[x, y];
					int radius = radiusData[x + dx, y + dy];
					counts[radius]++;
					sumR[radius] += c.R;
					sumG[radius] += c.G;
					sumB[radius] += c.B;
				}
			RawColor undefined = RawColor.Transparent;
			for (int i = 0; i < maxRadius + 1; i++)
			{
				if (counts[i] == 0)
					result[i] = undefined;
				else
				{
					result[i] = RawColor.FromRgb(
						(byte)(sumR[i] / counts[i]),
						(byte)(sumG[i] / counts[i]),
						(byte)(sumB[i] / counts[i]));
				}
			}
			return result;
		}

		private bool CalculateFieldPosition(int low, int pixelWidth, float[] lines, out int fieldSize, out double blockSize)
		{
			for (int count = 100; count >= 2; count--)
			{
				double w = (double)(pixelWidth - 1) / (double)(count - 1);
				if (Enumerable.Range(0, count)
					.All(i => IsLine(lines, (int)Math.Round(low + w * i))))
				{
					blockSize = w;
					fieldSize = count;
					return true;
				}
			}
			fieldSize = 0;
			blockSize = 0;
			return false;
		}

		private StoneColor GetStoneColor(RawColor[] circles, double blockSize)
		{
			int black = 0;
			int white = 0;
			for (int r = (int)(blockSize / 4); r < (int)(blockSize / 2); r++)
				if (circles[r] != RawColor.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetUnweightedBrightness() < 0.4)
						black++;
					if (circles[r].GetUnweightedBrightness() > 0.55)
						white++;
				}
			if (white > blockSize / 6.0)
				return StoneColor.White;
			if (black > blockSize / 6.0)
				return StoneColor.Black;
			return StoneColor.None;
		}

		private StoneColor GetSmallStoneColor(RawColor[] circles, double blockSize)
		{
			int black = 0;
			int white = 0;
			for (int r = 2; r < (int)(blockSize / 4); r++)
				if (circles[r] != RawColor.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetUnweightedBrightness() < 0.4)
						black++;
					if (circles[r].GetUnweightedBrightness() > 0.55)
						white++;
				}
			if (white > blockSize / 6.0)
				return StoneColor.White;
			if (black > blockSize / 6.0)
				return StoneColor.Black;
			return StoneColor.None;
		}

		private bool GetCircleMarker(RawColor[] circles, double blockSize, StoneColor stoneColor)
		{
			int blackRing = 0;
			int whiteRing = 0;
			for (int r = 3; r < (int)blockSize / 3; r++)
				if (circles[r] != RawColor.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetUnweightedBrightness() < 0.2)
						blackRing++;
					if (circles[r].GetUnweightedBrightness() > 0.8)
						whiteRing++;
				}
			switch (stoneColor)
			{
				case StoneColor.None:
				case StoneColor.White:
					return blackRing > 0;
				case StoneColor.Black:
					return whiteRing > 0;
				default:
					throw new NotImplementedException();
			}
		}

		private bool GetSquareMarker(RawColor[] squares, double blockSize, StoneColor stoneColor)
		{
			int blackSquare = 0;
			int whiteSquare = 0;
			for (int r = (int)blockSize / 4; r < (int)blockSize / 2; r++)
				if (squares[r] != RawColor.Transparent && IsGray(squares[r]))
				{
					if (squares[r].GetUnweightedBrightness() < 0.2)
						blackSquare++;
					if (squares[r].GetUnweightedBrightness() > 0.8)
						whiteSquare++;
				}
			switch (stoneColor)
			{
				case StoneColor.None:
				case StoneColor.White:
					return blackSquare > 0;
				case StoneColor.Black:
					return whiteSquare > 0;
				default:
					throw new NotImplementedException();
			}
		}

		bool IsLine(float[] lines, int index)
		{
			if (index < 1 || index > lines.Length - 2)
				return false;
			if (lines[index - 1] >= lines[index] || lines[index + 1] > lines[index])
				return false;
			float delta = (lines[index] * 2 - lines[index - 1] - lines[index + 1]) * 0.5f;
			bool result = delta + lines[index] > 0.75;
			return result;
		}

		bool GetBoardParameters(RawColor[,] pix, out BoardParameters bp)
		{
			bp = new BoardParameters();
			bp.BoardRect = System.Drawing.Rectangle.Empty;
			bp.LinedBoardRect = System.Drawing.Rectangle.Empty;
			bp.FieldHeight = 0;
			bp.FieldWidth = 0;
			bp.BlockSize = 0;

			bp.BoardRect = FindBoard(pix);
			if (bp.BoardRect == System.Drawing.Rectangle.Empty)
				return false;
			float[] grayCols, grayRows;
			bp.LinedBoardRect = FindLinedBoard(pix, bp.BoardRect, out grayCols, out grayRows);
			double blockWidth, blockHeight;
			bool foundLines1 = CalculateFieldPosition(bp.LinedBoardRect.Left, bp.LinedBoardRect.Width, grayCols, out bp.FieldWidth, out blockWidth);
			bool foundLines2 = CalculateFieldPosition(bp.LinedBoardRect.Top, bp.LinedBoardRect.Height, grayRows, out bp.FieldHeight, out blockHeight);
			if (Math.Abs(blockHeight - blockWidth) > 1)
				return false;
			bp.BlockSize = (blockHeight + blockWidth) / 2;
			return foundLines1 && foundLines2;
		}

		public struct BoardParameters
		{
			public System.Drawing.Rectangle BoardRect;
			public System.Drawing.Rectangle LinedBoardRect;
			public int FieldWidth;
			public int FieldHeight;
			public double BlockSize;
		}

		public BoardInfo ProcessImage(BoardParameters bp, RawColor[,] pix)
		{
			BoardInfo boardInfo = null;
			boardInfo = new BoardInfo(bp.FieldWidth, bp.FieldHeight);
			int halfSize = (int)(bp.BlockSize / 2) + 1;
			int[,] circleRadiusData = CalculateRadii((dx, dy) => (int)Math.Sqrt(dx * dx + dy * dy), halfSize);
			int[,] squareRadiusData = CalculateRadii((dx, dy) => (int)Math.Max(Math.Abs(dx), Math.Abs(dy)), halfSize);

			for (int y = 0; y < bp.FieldHeight; y++)
				for (int x = 0; x < bp.FieldWidth; x++)
				{
					int px = (int)Math.Round(bp.LinedBoardRect.Left + bp.BlockSize * x);
					int py = (int)Math.Round(bp.LinedBoardRect.Top + bp.BlockSize * y);
					RawColor[] circles = RadiusColor(pix,
						new Point(px, py),
						circleRadiusData,
						2 * halfSize
						);
					RawColor[] squares = RadiusColor(pix,
						new Point(px, py),
						squareRadiusData,
						2 * halfSize
						);

					PointInfo info = new PointInfo();
					info.StoneColor = GetStoneColor(circles, bp.BlockSize);
					info.SmallStoneColor = GetSmallStoneColor(circles, bp.BlockSize);
					if (info.SmallStoneColor == info.StoneColor)//Small stone is part of the real stone
						info.SmallStoneColor = StoneColor.None;
					if (GetCircleMarker(circles, bp.BlockSize, info.StoneColor))
					{
						if (info.SmallStoneColor != StoneColor.Black)
							info.Marker = Marker.Circle;
						else
							info.Marker = Marker.Unknown;//Can't distinguish circle from small stone
					}
					if (GetSquareMarker(squares, bp.BlockSize, info.StoneColor))
						info.Marker = Marker.Square;
					boardInfo.Board[x, y] = info;
				}
			//MirrorBoardInfo(boardInfo);

			return boardInfo;
		}

		private PixelPool pixelPool = new PixelPool(15);
		/*private Bitmap PixelsToBitmap(RawColor[,] pix)
		{
			Bitmap bmp = new Bitmap(pix.GetLength(0), pix.GetLength(1));
		}*/

		private RawColor[,] BitmapToPixels(Bitmap bmp)
		{
			RawColor[,] pix = pixelPool.Alloc(bmp.Width, bmp.Height);
			//RawColor[,] pix = new RawColor[bmp.Width, bmp.Height];
			unsafe
			{
				BitmapData data = null;
				try
				{
					data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

					for (int y = 0; y < data.Height; y++)
					{
						uint* p = (uint*)((byte*)data.Scan0 + y * data.Stride);
						for (int x = 0; x < data.Width; x++)
						{
							Color c = Color.FromArgb((int)*p);
							pix[x, y] = RawColor.FromArgb(*p);
							p++;
						}
					}
				}
				finally
				{
					if (data != null)
						bmp.UnlockBits(data);
				}
			}
			return pix;
		}

		static bool SamePixels(RawColor[,] pix1, RawColor[,] pix2)
		{
			if (pix1.GetLength(0) != pix2.GetLength(0) || pix1.GetLength(1) != pix2.GetLength(1))
				return false;
			for (int y = 0; y < pix1.GetLength(1); y++)
				for (int x = 0; x < pix1.GetLength(0); x++)
					if (pix1[x, y] != pix2[x, y])
						return false;
			return true;
		}


		private void button1_Click(object sender, EventArgs e)
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
			}*/
			Text += " " + (DateTime.UtcNow - start).ToString();
			Preview.Invalidate();
		}

		int windowHandle = 0;
		VideoRecorder recorder;
		private void button2_Click(object sender, EventArgs e)
		{
			int.TryParse(windowHandleBox.Text, out windowHandle);
			windowHandleBox.Text = windowHandle.ToString();
			recorder = new VideoRecorder();
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

		public readonly ThreadSafeQueue<RawColor[,]> Work = new ThreadSafeQueue<RawColor[,]>();
		public readonly ThreadSafeQueue<BoardInfo> FinishedWork = new ThreadSafeQueue<BoardInfo>();
		private volatile bool stopWorker = false;
		private volatile bool workerIdle = false;

		private void WorkerThreadFunc()
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
		}

		RawColor[,] CaptureWindowToPixels(int handle)
		{
			Bitmap bmp = ScreenCapture.CaptureWindow(new IntPtr(windowHandle));
			RawColor[,] pixels = BitmapToPixels(bmp);
			bmp.Dispose();
			return pixels;
		}

		private BoardInfo lastBoard;
		private void timer1_Tick(object sender, EventArgs e)
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
		}

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
			stopWorker = true;
		}
	}
}
