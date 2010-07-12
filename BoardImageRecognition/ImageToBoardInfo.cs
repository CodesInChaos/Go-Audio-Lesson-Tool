using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Model;
using System.Drawing.Imaging;
using Chaos.Image;

namespace BoardImageRecognition
{
	public struct BoardParameters
	{
		public System.Drawing.Rectangle BoardRect;
		public System.Drawing.Rectangle LinedBoardRect;
		public int FieldWidth;
		public int FieldHeight;
		public double BlockSize;
	}

	public class ImageToBoardInfo
	{
		System.Drawing.Rectangle FindBoard(Pixels pix)
		{
			float[] boardCols, boardRows;
			CountPixels(pix, pix.Rect, IsBoard, out boardCols, out boardRows);
			float max = boardCols.Max();
			if (max < 0.1)
				return System.Drawing.Rectangle.Empty;
			int left = (int)boardCols.FirstIndex(f => f >= 0.8f * max);
			int right = (int)boardCols.LastIndex(f => f >= 0.8f * max) + 1;

			CountPixels(pix, new System.Drawing.Rectangle(left, 0, right - left, pix.Height), IsBoard, out boardCols, out boardRows);
			max = boardRows.Max();
			int top = (int)boardRows.FirstIndex(f => f >= 0.8f * max);
			int bottom = (int)boardRows.LastIndex(f => f >= 0.8f * max) + 1;

			return System.Drawing.Rectangle.FromLTRB(left, top, right, bottom);
		}

		System.Drawing.Rectangle FindLinedBoard(Pixels pix, System.Drawing.Rectangle boardRect, out float[] grayCols, out float[] grayRows)
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

		RawColor[] RadiusColor(Pixels pix, Point center, int[,] radiusData, int maxRadius)
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
			int right = Math.Min(pix.Width, center.X + halfSize + 1);
			int bottom = Math.Min(pix.Height, center.Y + halfSize + 1);
			for (int y = top; y < bottom; y++)
				for (int x = left; x < right; x++)
				{
					RawColor c = pix.Data[x, y];
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
					result[i] = RawColor.FromRGB(
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
			int outerRadius = (int)(blockSize / 2);
			int innerRadius = (int)(blockSize / 4);
			for (int r = innerRadius; r < outerRadius; r++)
				if (circles[r] != RawColor.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetUnweightedBrightness() < 0.45)
						black++;
					if (circles[r].GetUnweightedBrightness() > 0.55)
						white++;
				}
			if (white > (outerRadius - innerRadius) / 2)
				return StoneColor.White;
			if (black > (outerRadius - innerRadius) / 2)
				return StoneColor.Black;
			return StoneColor.None;
		}

		private StoneColor GetSmallStoneColor(RawColor[] circles, double blockSize)
		{
			int black = 0;
			int white = 0;
			int outerRadius = (int)(blockSize / 4);
			int innerRadius = 2;
			for (int r = innerRadius; r < outerRadius; r++)
				if (circles[r] != RawColor.Transparent && IsGray(circles[r]))
				{
					if (circles[r].GetUnweightedBrightness() < 0.4)
						black++;
					if (circles[r].GetUnweightedBrightness() > 0.55)
						white++;
				}
			if (white >= (outerRadius - innerRadius) * 0.7)
				return StoneColor.White;
			if (black >= (outerRadius - innerRadius) * 0.7)
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
					if (circles[r].GetUnweightedBrightness() < 0.35)
						blackRing++;
					if (circles[r].GetUnweightedBrightness() > 0.65)
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

		public bool GetBoardParameters(Pixels pix, out BoardParameters bp)
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

		public BoardInfo ProcessImage(BoardParameters bp, Pixels pix)
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
						if (info.SmallStoneColor != StoneColor.Black)//Can't distinguish circle from small stone
							info.Marker = Marker.Circle;
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




		int DistanceSquared(RawColor c1, RawColor c2)
		{
			int dR = c1.R - c2.R;
			int dG = c1.G - c2.G;
			int dB = c1.B - c2.B;
			return dR * dR + dG * dG + dB * dB;
		}

		private readonly RawColor BoardColor = RawColor.FromRGB(220, 180, 90);

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

		bool IsFadedWhite(RawColor c)
		{
			return DistanceSquared(c, BoardColor) < 400;
		}

		bool IsFadedBlack(RawColor c)
		{
			return DistanceSquared(c, BoardColor) < 400;
		}

		private void CountPixels(Pixels pix, System.Drawing.Rectangle rect, Predicate<RawColor> filter, out float[] cols, out float[] rows)
		{
			int[] colsI = new int[pix.Width];
			int[] rowsI = new int[pix.Height];
			for (int y = rect.Top; y < rect.Bottom; y++)
			{
				for (int x = rect.Left; x < rect.Right; x++)
				{
					RawColor c = pix.Data[x, y];
					if (filter(c))
					{
						colsI[x]++;
						rowsI[y]++;
					}
				}
			}
			cols = new float[pix.Width];
			rows = new float[pix.Height];
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
	}
}
