using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Model;

namespace GoClient
{
	public class StateRenderer
	{
		string fontName = "Tahoma";


		private static int[] stars19 = new int[] { 3, 9, 15 };
		private static int[] stars17 = new int[] { 3, 8, 13 };
		private static int[] stars15 = new int[] { 3, 7, 11 };
		private static int[] stars13 = new int[] { 2, 6, 10 };
		private static int[] stars11 = new int[] { 2, 5, 8 };
		private static int[] stars9 = new int[] { 2, 4, 6 };
		private static int[] stars0 = new int[0];

		public float BlockSize { get; set; }
		public BoardSetup BoardSetup { get; set; }
		public Position? active;
		Font[] fonts = new Font[5];
		public Font GetFont(int length)
		{
			if (length > fonts.Length)
				length = fonts.Length;
			return fonts[length - 1];
		}
		public int[] Stars(int size)
		{
			switch (size)
			{
				case 19:
					return stars19;
				case 17:
					return stars17;
				case 15:
					return stars15;
				case 13:
					return stars13;
				case 11:
					return stars11;
				case 9:
					return stars9;
				default:
					return stars0;
			}
		}

		private void GenerateFonts()
		{
			for (int i = 0; i < fonts.Length; i++)
			{
				fonts[i] = new Font(fontName, BlockSize / (i + 2) * 1.5f, FontStyle.Bold, GraphicsUnit.Pixel);
			}
		}

		public Point GameToImage(float x, float y)
		{
			y = BoardSetup.Height - 1 - y;
			Point p = Point.Empty;
			p.X = (int)(BlockSize * (1.5f + x));
			p.Y = (int)(BlockSize * (1.5f + y));
			return p;
		}

		public PointF ImageToGame(int x, int y)
		{
			PointF p = PointF.Empty;
			p.X = x / BlockSize - 1.5f;
			p.Y = y / BlockSize - 1.5f;
			p.Y = BoardSetup.Height - 1 - p.Y;
			return p;
		}

		private static readonly Brush bgBrush = new SolidBrush(Color.FromArgb(254, 214, 121));

		public static void DrawString(Graphics graphics, string s, Font font, Brush brush, PointF point, PointF align)
		{
			SizeF size = graphics.MeasureString(s, font);
			PointF newPoint = new PointF(point.X - size.Width * align.X, point.Y - size.Height * align.Y);
			graphics.DrawString(s, font, brush, newPoint);
		}

		public Bitmap Render(GameState state)
		{
			if (BlockSize <= 0)
				return null;
			GenerateFonts();


			Bitmap bmp = new Bitmap((int)Math.Round(BlockSize * (state.Width - 1 + 3)), (int)Math.Round(BlockSize * (state.Height - 1 + 3)));
			Graphics graphics = Graphics.FromImage(bmp);
			//graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			graphics.FillRectangle(bgBrush, 0, 0, bmp.Width, bmp.Height);

			Font coordinateFont = new Font(fontName, BlockSize / 2, 0, GraphicsUnit.Pixel);
			Font coordinateFont2 = new Font(fontName, BlockSize / 2, FontStyle.Bold, GraphicsUnit.Pixel);

			for (int x = 0; x < state.Width; x++)
			{
				PointF p = GameToImage(x, -0.6f);
				PointF p2 = GameToImage(x, state.Height - 1 + 0.5f);
				string s = ((char)('A' + x)).ToString();
				Font font = coordinateFont;
				if (active != null && active.Value.X == x)
					font = coordinateFont2;
				DrawString(graphics, s, font, Brushes.Black, p, new PointF(0.5f, 0f));
				DrawString(graphics, s, font, Brushes.Black, p2, new PointF(0.5f, 1f));
			}

			for (int y = 0; y < state.Width; y++)
			{
				PointF p = GameToImage(-0.6f, y);
				PointF p2 = GameToImage(state.Width - 1 + 1.2f, y);
				string s = (y + 1).ToString();
				Font font = coordinateFont;
				if (active != null && active.Value.Y == y)
					font = coordinateFont2;
				DrawString(graphics, s, font, Brushes.Black, p, new PointF(1f, 0.5f));
				DrawString(graphics, s, font, Brushes.Black, p2, new PointF(1f, 0.5f));
			}

			for (int x = 0; x < state.Width; x++)
			{
				graphics.DrawLine(Pens.Black, GameToImage(x, 0), GameToImage(x, state.Height - 1));
			}
			for (int y = 0; y < state.Height; y++)
			{
				graphics.DrawLine(Pens.Black, GameToImage(0, y), GameToImage(state.Width - 1, y));
			}
			//Draw Stars
			foreach (int sy in Stars(state.Height))
				foreach (int sx in Stars(state.Width))
				{
					Point p = GameToImage(sx, sy);
					float r = BlockSize * 0.15f;
					graphics.FillEllipse(Brushes.Black, p.X - r, p.Y - r, 2 * r, 2 * r);
				}
			//Draw stones
			for (int y = 0; y < state.Height; y++)
				for (int x = 0; x < state.Width; x++)
				{
					Point p = GameToImage(x - 0.5f, y + 0.5f);
					switch (state.Stones[x, y])
					{
						case StoneColor.None:
							{
								break;
							}
						case StoneColor.Black:
							{
								graphics.FillEllipse(Brushes.Black, p.X, p.Y, BlockSize, BlockSize);
								break;
							}
						case StoneColor.White:
							{
								graphics.FillEllipse(Brushes.White, p.X, p.Y, BlockSize, BlockSize);
								graphics.DrawEllipse(Pens.Black, p.X, p.Y, BlockSize, BlockSize);
								break;
							}

						default:
							{
								throw new NotImplementedException();
							}
					}
				}
			//draw labels
			for (int y = 0; y < state.Height; y++)
				for (int x = 0; x < state.Width; x++)
				{
					string s = state.Labels[x, y];
					if (state.Ko == new Position(x, y))
						s = "#KO";
					if (String.IsNullOrEmpty(s))
						continue;
					Point p = GameToImage(x, y);
					Brush brush;
					switch (state.Stones[x, y])
					{
						case StoneColor.Black:
							{
								brush = Brushes.White;
								break;
							}
						case StoneColor.White:
							{
								brush = Brushes.Black;
								break;
							}
						case StoneColor.None:
							{
								Point pg = GameToImage(x - 0.4f, y + 0.4f);
								graphics.FillEllipse(bgBrush, pg.X, pg.Y, (int)(BlockSize * 0.8), (int)(BlockSize * 0.8));
								brush = Brushes.Black;
								break;
							}
						default:
							{
								throw new NotImplementedException();
							}
					}
					if (s == "#TR")
					{
						Point p1 = GameToImage(x, y + 0.3f);
						Point p2 = GameToImage(x - 0.3f, y - 0.3f);
						Point p3 = GameToImage(x + 0.3f, y - 0.3f);
						graphics.DrawPolygon(new Pen(brush, 2), new Point[] { p1, p2, p3 });
					}
					else if ((s == "#SQ") || (s == "#KO"))
					{
						Point pL = GameToImage(x - 0.3f, y + 0.3f);
						graphics.DrawRectangle(new Pen(brush, 2), pL.X, pL.Y, (int)(0.6f * BlockSize), (int)(0.6f * BlockSize));
					}
					else if (s == "#CI")
					{
						Point pL = GameToImage(x - 0.3f, y + 0.3f);
						graphics.DrawEllipse(new Pen(brush, 2), pL.X, pL.Y, (int)(0.6f * BlockSize), (int)(0.6f * BlockSize));
					}
					else DrawString(graphics, s, GetFont(s.Length), brush, new PointF(p.X, p.Y), new PointF(0.5f, 0.5f));
				}
			return bmp;
		}
	}
}
