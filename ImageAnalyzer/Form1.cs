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

namespace ImageAnalyzer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			deltaJoin = new DeltaJoin(game);
		}

		int DistanceSquared(Color c1, Color c2)
		{
			int dR = c1.R - c2.R;
			int dG = c1.G - c2.G;
			int dB = c1.B - c2.B;
			return dR * dR + dG * dG + dB * dB;
		}

		private readonly Color BoardColor = Color.FromArgb(220, 180, 90);

		bool IsGray(Color c)
		{
			int sum = c.R + c.G + c.B;
			int sqrSum = (c.R * c.R + c.G * c.G + c.B * c.B) * 3;
			int var9 = sqrSum - sum * sum;//Var9=9*Variance
			return var9 < 3000;
		}

		bool IsBoard(Color c)
		{
			return DistanceSquared(c, BoardColor) < 400;
		}

		private void CountPixels(Color[,] pix, System.Drawing.Rectangle rect, Predicate<Color> filter, out float[] cols, out float[] rows)
		{
			int bmpWidth = pix.GetLength(0);
			int bmpHeight = pix.GetLength(1);
			int[] colsI = new int[bmpWidth];
			int[] rowsI = new int[bmpHeight];
			for (int y = rect.Top; y < rect.Bottom; y++)
			{
				for (int x = rect.Left; x < rect.Right; x++)
				{
					Color c = pix[x, y];
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

		System.Drawing.Rectangle FindBoard(Color[,] pix)
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

		System.Drawing.Rectangle FindLinedBoard(Color[,] pix, System.Drawing.Rectangle boardRect, out float[] grayCols, out float[] grayRows)
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

		Color[] RadiusColor(Color[,] pix, Point center, System.Drawing.Rectangle rect, Func<int, int, int> radiusFunc, int maxRadius)
		{
			int[] counts = new int[maxRadius + 1];
			int[] sumR = new int[maxRadius + 1];
			int[] sumG = new int[maxRadius + 1];
			int[] sumB = new int[maxRadius + 1];
			Color[] result = new Color[maxRadius + 1];
			for (int y = rect.Top; y < rect.Bottom; y++)
				for (int x = rect.Left; x < rect.Right; x++)
				{
					Color c = pix[x, y];
					int radius = radiusFunc(x - center.X, y - center.Y);
					counts[radius]++;
					sumR[radius] += c.R;
					sumG[radius] += c.G;
					sumB[radius] += c.B;
				}
			Color undefined = Color.Transparent;
			for (int i = 0; i < maxRadius + 1; i++)
			{
				if (counts[i] == 0)
					result[i] = undefined;
				else
				{
					result[i] = Color.FromArgb(
						sumR[i] / counts[i],
						sumG[i] / counts[i],
						sumB[i] / counts[i]);
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

		private StoneColor GetStoneColor(Color[] circles, double blockSize)
		{
			int black = 0;
			int white = 0;
			for (int r = (int)(blockSize / 4); r < (int)(blockSize / 2); r++)
				if (circles[r] != Color.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetBrightness() < 0.4)
						black++;
					if (circles[r].GetBrightness() > 0.55)
						white++;
				}
			if (white > blockSize / 6.0)
				return StoneColor.White;
			if (black > blockSize / 6.0)
				return StoneColor.Black;
			return StoneColor.None;
		}

		private StoneColor GetSmallStoneColor(Color[] circles, double blockSize)
		{
			int black = 0;
			int white = 0;
			for (int r = 2; r < (int)(blockSize / 4); r++)
				if (circles[r] != Color.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetBrightness() < 0.4)
						black++;
					if (circles[r].GetBrightness() > 0.55)
						white++;
				}
			if (white > blockSize / 6.0)
				return StoneColor.White;
			if (black > blockSize / 6.0)
				return StoneColor.Black;
			return StoneColor.None;
		}

		private bool GetCircleMarker(Color[] circles, double blockSize, StoneColor stoneColor)
		{
			int blackRing = 0;
			int whiteRing = 0;
			for (int r = 3; r < (int)blockSize / 3; r++)
				if (circles[r] != Color.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetBrightness() < 0.2)
						blackRing++;
					if (circles[r].GetBrightness() > 0.8)
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

		private bool GetSquareMarker(Color[] squares, double blockSize, StoneColor stoneColor)
		{
			int blackSquare = 0;
			int whiteSquare = 0;
			for (int r = (int)blockSize / 4; r < (int)blockSize / 2; r++)
				if (squares[r] != Color.Transparent && IsGray(squares[r]))
				{
					if (squares[r].GetBrightness() < 0.2)
						blackSquare++;
					if (squares[r].GetBrightness() > 0.8)
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

		bool GetBoardParameters(Color[,] pix, out System.Drawing.Rectangle boardRect, out System.Drawing.Rectangle linedBoardRect, out int fieldWidth, out int fieldHeight, out double blockWidth, out double blockHeight)
		{
			boardRect = System.Drawing.Rectangle.Empty;
			linedBoardRect = System.Drawing.Rectangle.Empty;
			fieldHeight = 0;
			fieldWidth = 0;
			blockHeight = 0;
			blockWidth = 0;

			boardRect = FindBoard(pix);
			if (boardRect == System.Drawing.Rectangle.Empty)
				return false;
			float[] grayCols, grayRows;
			linedBoardRect = FindLinedBoard(pix, boardRect, out grayCols, out grayRows);

			bool foundLines1 = CalculateFieldPosition(linedBoardRect.Left, linedBoardRect.Width, grayCols, out fieldWidth, out blockWidth);
			bool foundLines2 = CalculateFieldPosition(linedBoardRect.Top, linedBoardRect.Height, grayRows, out fieldHeight, out blockHeight);
			return foundLines1 && foundLines2;
		}

		public BoardInfo ProcessImage(Color[,] pix)
		{
			//Get Params
			int fieldWidth, fieldHeight;
			double blockHeight, blockWidth;
			System.Drawing.Rectangle boardRect;
			System.Drawing.Rectangle linedBoardRect;
			bool foundBoard = GetBoardParameters(pix, out boardRect, out linedBoardRect, out fieldWidth, out fieldHeight, out blockWidth, out blockHeight);
			double blockSize = Math.Min(blockWidth, blockHeight);
			BoardInfo boardInfo = null;
			//graphics.DrawRectangle(Pens.Red, boardRect);
			if (blockWidth - blockHeight > 2)
				foundBoard = false;
			//Analyze Contents
			if (foundBoard)
			{
				boardInfo = new BoardInfo(fieldWidth, fieldHeight);
				int scanSize = (int)blockSize + 1;
				for (int y = 0; y < fieldHeight; y++)
					for (int x = 0; x < fieldWidth; x++)
					{
						int px = (int)Math.Round(linedBoardRect.Left + blockWidth * x);
						int py = (int)Math.Round(linedBoardRect.Top + blockHeight * y);
						Color[] circles = RadiusColor(pix,
							new Point(px, py),
							new System.Drawing.Rectangle(px - scanSize, py - scanSize, scanSize * 2, scanSize * 2),
							(dx, dy) => (int)Math.Sqrt(dx * dx + dy * dy),
							2 * scanSize
							);
						Color[] squares = RadiusColor(pix,
							new Point(px, py),
							new System.Drawing.Rectangle(px - scanSize, py - scanSize, scanSize * 2, scanSize * 2),
							(dx, dy) => (int)Math.Max(Math.Abs(dx), Math.Abs(dy)),
							2 * scanSize
							);

						PointInfo info = new PointInfo();
						info.StoneColor = GetStoneColor(circles, blockSize);
						info.SmallStoneColor = GetSmallStoneColor(circles, blockSize);
						if (info.SmallStoneColor == info.StoneColor)//Small stone is part of the real stone
							info.SmallStoneColor = StoneColor.None;
						if (GetCircleMarker(circles, blockSize, info.StoneColor))
						{
							if (info.SmallStoneColor != StoneColor.Black)
								info.Marker = Marker.Circle;
							else
								info.Marker = Marker.Unknown;//Can't distinguish circle from small stone
						}
						if (GetSquareMarker(squares, blockSize, info.StoneColor))
							info.Marker = Marker.Square;
						boardInfo.Board[x, y] = info;
					}
				MirrorBoardInfo(boardInfo);
			}
			return boardInfo;
		}

		private static Color[,] BitmapToPixels(Bitmap bmp)
		{
			Color[,] pix = new Color[bmp.Width, bmp.Height];
			unsafe
			{
				BitmapData data = null;
				try
				{
					data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

					for (int y = 0; y < data.Height; y++)
					{
						int* p = (int*)((byte*)data.Scan0 + y * data.Stride);
						for (int x = 0; x < data.Width; x++)
						{
							pix[x, y] = Color.FromArgb(*p);
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


		private void button1_Click(object sender, EventArgs e)
		{
			//Copy
			DateTime start0 = DateTime.UtcNow;
			Bitmap bmp = new Bitmap(OriginalPB.Image);
			var graphics = Graphics.FromImage(OriginalPB.Image);
			Color[,] pix = BitmapToPixels(bmp);

			Text = (DateTime.UtcNow - start0).ToString();
			DateTime start = DateTime.UtcNow;
			BoardInfo info = ProcessImage(pix);

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
			OriginalPB.Invalidate();
		}

		int windowHandle = 0;
		Game game = new Game(new Replay());
		DeltaJoin deltaJoin;
		private void button2_Click(object sender, EventArgs e)
		{
			int.TryParse(windowHandleBox.Text, out windowHandle);
			windowHandleBox.Text = windowHandle.ToString();
		}

		void MirrorBoardInfo(BoardInfo info)
		{
			for (int y = 0; y < info.Height / 2; y++)
				for (int x = 0; x < info.Width; x++)
				{
					int mirrorY = info.Height - 1 - y;
					PointInfo p = info.Board[x, y];
					info.Board[x, y] = info.Board[x, mirrorY];
					info.Board[x, mirrorY] = p;
				}
		}

		private string TS(TimeSpan t)
		{
			return ((int)t.TotalMilliseconds).ToString();
			/*int i=(int)Math.Round(t.TotalSeconds*100);
			Decimal d = (Decimal)i / 100;
			return d.ToString();*/
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			DateTime start0 = DateTime.UtcNow;
			if (this.Handle != ScreenCapture.GetForegroundWindow())
				windowHandleBox.Text = ScreenCapture.GetForegroundWindow().ToString();
			if (windowHandle == 0)
				return;
			if (windowHandle != ScreenCapture.GetForegroundWindow().ToInt32())//Capturing background windows is buggy
				return;
			Bitmap bmp = ScreenCapture.CaptureWindow(new IntPtr(windowHandle));
			//bmp.Save("ScreenShot.bmp");
			Color[,] pixels = BitmapToPixels(bmp);
			DateTime start1 = DateTime.UtcNow;
			BoardInfo board = ProcessImage(pixels);
			DateTime start2 = DateTime.UtcNow;
			if (board == null)
				return;
			deltaJoin.Add(null, board);
			StateRenderer renderer = new StateRenderer();
			renderer.BlockSize = 16;
			renderer.BoardSetup = game.State;
			game.Seek(game.Replay.Actions.Count - 1);
			OriginalPB.Image = renderer.Render(game.State);
			game.Replay.Save("Capture.GoReplay");
			Text = "T" + TS(DateTime.UtcNow - start0) + " P" + TS(start2 - start1) + " D" + TS(DateTime.UtcNow - start2);
		}
	}
}
