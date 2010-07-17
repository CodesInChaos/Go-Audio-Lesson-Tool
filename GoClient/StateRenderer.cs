using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Model;
using System.Diagnostics;

namespace GoClient
{
	public class StateRenderer : GoRenderer
	{
		public GameState State;
		public Game Game;

		string fontName = "Tahoma";
		private static int[] stars19 = new int[] { 3, 9, 15 };
		private static int[] stars17 = new int[] { 3, 8, 13 };
		private static int[] stars15 = new int[] { 3, 7, 11 };
		private static int[] stars13 = new int[] { 2, 6, 10 };
		private static int[] stars11 = new int[] { 2, 5, 8 };
		private static int[] stars9 = new int[] { 2, 4, 6 };
		private static int[] stars0 = new int[0];

		public float BlockSize { get; set; }
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

		public Point GameToImage(Position p)
		{
			return GameToImage(p.X, p.Y);
		}

		public Point GameToImage(float x, float y)
		{
			//y = State.Height - 1 - y;
			Point p = Point.Empty;
			p.X = (int)Math.Round(BlockSize * (1.5f + x));
			p.Y = (int)Math.Round(BlockSize * (1.5f + y));
			return p;
		}

		public PointF ImageToGame(int x, int y)
		{
			PointF p = PointF.Empty;
			p.X = x / BlockSize - 1.5f;
			p.Y = y / BlockSize - 1.5f;
			//p.Y = State.Height - 1 - p.Y;
			return p;
		}

		private static readonly Brush bgBrush = new SolidBrush(Color.FromArgb(254, 214, 121));

		public void RenderCoordinates()
		{
			Font coordinateFont = new Font(fontName, BlockSize / 2, 0, GraphicsUnit.Pixel);
			Font coordinateFont2 = new Font(fontName, BlockSize / 2, FontStyle.Bold, GraphicsUnit.Pixel);

			for (int x = 0; x < State.Width; x++)
			{
				PointF p = GameToImage(x, -0.5f);
				PointF p2 = GameToImage(x, State.Height - 1 + 0.6f);
				char c = (char)('A' + x);
				if (c >= 'I')//no I displayed
					c++;
				string s = c.ToString();
				Font font = coordinateFont;
				if (active != null && active.Value.X == x)
					font = coordinateFont2;
				DrawString(s, font, Brushes.Black, p, new PointF(0.5f, 1f));
				DrawString(s, font, Brushes.Black, p2, new PointF(0.5f, 0f));
			}

			for (int y = 0; y < State.Width; y++)
			{
				PointF p = GameToImage(-0.6f, y);
				PointF p2 = GameToImage(State.Width - 1 + 1.2f, y);
				string s = (State.Height - y).ToString();
				Font font = coordinateFont;
				if (active != null && active.Value.Y == y)
					font = coordinateFont2;
				DrawString(s, font, Brushes.Black, p, new PointF(1f, 0.5f));
				DrawString(s, font, Brushes.Black, p2, new PointF(1f, 0.5f));
			}
		}

		public Bitmap Render()
		{
			Debug.Assert(Game == null || Game.State == State);
			if (BlockSize <= 0)
				return null;
			GenerateFonts();


			Bitmap bmp = new Bitmap((int)Math.Round(BlockSize * (State.Width - 1 + 3)), (int)Math.Round(BlockSize * (State.Height - 1 + 3)));
			Graphics = Graphics.FromImage(bmp);
			//graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			Graphics.FillRectangle(bgBrush, 0, 0, bmp.Width, bmp.Height);
			RenderCoordinates();
			RenderLines();
			//Draw Stars
			foreach (int sy in Stars(State.Height))
				foreach (int sx in Stars(State.Width))
				{
					PointF p = GameToImage(sx, sy);
					float r = BlockSize * 0.15f;
					Graphics.FillEllipse(Brushes.Black, p.X - r, p.Y - r, 2 * r, 2 * r);
				}
			//Draw stones
			foreach (Position p in State.AllPositions)
			{
				PointF point = GameToImage(p);
				RenderStone(point, BlockSize, State.Stones[p]);
			}
			//Draw Territory
			foreach (Position p in State.AllPositions)
			{
				PointF point = GameToImage(p);
				RenderStone(point, BlockSize / 2, State.Territory[p]);
			}
			//draw variations
			if (Game != null)
				foreach (Position p in State.AllPositions)
				{
					PointF point = GameToImage(p);
					int? alreadyPlayed = Game.Tree.MoveAlreadyPlayed(p);
					if (alreadyPlayed != null)
					{
						bool isCurrentVariation = Game.Tree.IsInCurrentVariation((int)alreadyPlayed);
						RenderVariationMarker(point, BlockSize, isCurrentVariation);
					}

				}
			//draw current move
			if (Game.CurrentMoveIndex != null)
			{
				StoneMoveAction action = Game.Replay.Actions[(int)Game.CurrentMoveIndex] as StoneMoveAction;
				if (action != null)
				{
					PointF point = GameToImage(action.Position);
					RenderCurrentMoveMarker(point, BlockSize);
				}
			}
			//draw labels
			foreach (Position p in State.AllPositions)
			{
				string s = State.Labels[p];
				if (State.Ko == p)
					s = "#KO";
				RenderLabel(p, s);
			}
			return bmp;
		}

		void RenderLabel(Position pos, string s)
		{
			if (String.IsNullOrEmpty(s))
				return;
			PointF point = GameToImage(pos);
			Brush brush;
			switch (State.Stones[pos])
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
						PointF pg = new PointF(point.X - 0.4f * BlockSize, point.Y - 0.4f * BlockSize);
						Graphics.FillEllipse(bgBrush, pg.X, pg.Y, BlockSize * 0.8f, BlockSize * 0.8f);
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
				float cos = (float)Math.Cos(Math.PI / 6);
				float sin = 0.5f;
				float radius = 0.35f * BlockSize;
				PointF p1 = new PointF(point.X, point.Y - radius);
				PointF p2 = new PointF(point.X + cos * radius, point.Y + sin * radius);
				PointF p3 = new PointF(point.X - cos * radius, point.Y + sin * radius);
				Graphics.DrawPolygon(new Pen(brush, 2), new PointF[] { p1, p2, p3 });
			}
			else if ((s == "#SQ") || (s == "#KO"))
			{
				PointF pL = new PointF(point.X - 0.25f * BlockSize, point.Y - 0.25f * BlockSize);
				Graphics.DrawRectangle(new Pen(brush, 2), pL.X, pL.Y, (int)(0.5f * BlockSize), (int)(0.5f * BlockSize));
			}
			else if (s == "#CR")
			{
				PointF pL = new PointF(point.X - 0.3f * BlockSize, point.Y - 0.3f * BlockSize);
				Graphics.DrawEllipse(new Pen(brush, 2), pL.X, pL.Y, (int)(0.6f * BlockSize), (int)(0.6f * BlockSize));
			}
			else DrawString(s, GetFont(s.Length), brush, new PointF(point.X, point.Y), new PointF(0.5f, 0.5f));
		}

		private void RenderLines()
		{
			for (int x = 0; x < State.Width; x++)
			{
				Graphics.DrawLine(Pens.Black, GameToImage(x, 0), GameToImage(x, State.Height - 1));
			}
			for (int y = 0; y < State.Height; y++)
			{
				Graphics.DrawLine(Pens.Black, GameToImage(0, y), GameToImage(State.Width - 1, y));
			}
		}
	}
}
