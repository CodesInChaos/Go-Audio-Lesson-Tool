using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Diagnostics;
using CommonGui.Drawing;
using Chaos.Util.Mathematics;
using Chaos.Image;

namespace CommonGui.GameRenderer
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
				fonts[i] = new Font(fontName, BlockSize / (i + 2) * 1.5f, FontStyle.Bold);
			}
		}

		public Vector2i GameToImage(Position p)
		{
			return GameToImage(p.X, p.Y);
		}

		public Vector2i GameToImage(float x, float y)
		{
			//y = State.Height - 1 - y;
			int x2 = (int)Math.Round(BlockSize * (1.5f + x));
			int y2 = (int)Math.Round(BlockSize * (1.5f + y));
			return new Vector2i(x2, y2);
		}

		public Vector2f ImageToGame(int x, int y)
		{
			float x2 = x / BlockSize - 1.5f;
			float y2 = y / BlockSize - 1.5f;
			//p.Y = State.Height - 1 - p.Y;
			return new Vector2f(x2, y2);
		}


		public void RenderCoordinates()
		{
			Font coordinateFont = new Font(fontName, BlockSize / 2, 0);
			Font coordinateFont2 = new Font(fontName, BlockSize / 2, FontStyle.Bold);

			for (int x = 0; x < State.Width; x++)
			{
				Vector2f p = GameToImage(x, -0.5f);
				Vector2f p2 = GameToImage(x, State.Height - 1 + 0.6f);
				char c = (char)('A' + x);
				if (c >= 'I')//no I displayed
					c++;
				string s = c.ToString();
				Font font = coordinateFont;
				if (active != null && active.Value.X == x)
					font = coordinateFont2;
				DrawString(s, font, RawColor.Black, p, new Vector2f(0.5f, 1f));
				DrawString(s, font, RawColor.Black, p2, new Vector2f(0.5f, 0f));
			}

			for (int y = 0; y < State.Width; y++)
			{
				Vector2f p = GameToImage(-0.6f, y);
				Vector2f p2 = GameToImage(State.Width - 1 + 1.2f, y);
				string s = (State.Height - y).ToString();
				Font font = coordinateFont;
				if (active != null && active.Value.Y == y)
					font = coordinateFont2;
				DrawString(s, font, RawColor.Black, p, new Vector2f(1f, 0.5f));
				DrawString(s, font, RawColor.Black, p2, new Vector2f(1f, 0.5f));
			}
		}

		public Bitmap Render()
		{
			Debug.Assert(Game == null || Game.State == State);
			if (BlockSize <= 0)
				return null;
			GenerateFonts();


			Bitmap bmp = GraphicsSystem.CreateBitmap((int)Math.Round(BlockSize * (State.Width - 1 + 3)), (int)Math.Round(BlockSize * (State.Height - 1 + 3)));
			Graphics = bmp.CreateGraphics();

			Graphics.FillRectangle(BackgroundColor, RectangleF.FromLTWH(0, 0, bmp.Width, bmp.Height));
			RenderCoordinates();
			RenderLines();
			//Draw Stars
			foreach (int sy in Stars(State.Height))
				foreach (int sx in Stars(State.Width))
				{
					Vector2f p = GameToImage(sx, sy);
					float r = BlockSize * 0.15f;
					Graphics.FillCircle(RawColor.Black, p, r);
				}
			//Draw stones
			foreach (Position p in State.AllPositions)
			{
				Vector2f point = GameToImage(p);
				RenderStone(point, BlockSize, State.Stones[p]);
			}
			//Draw Territory
			foreach (Position p in State.AllPositions)
			{
				Vector2f point = GameToImage(p);
				RenderStone(point, BlockSize / 2, State.Territory[p]);
			}
			//draw variations
			if (Game != null)
				foreach (Position p in State.AllPositions)
				{
					Vector2f point = GameToImage(p);
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
					Vector2f point = GameToImage(action.Position);
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
			Vector2f point = GameToImage(pos);
			RawColor color;
			switch (State.Stones[pos])
			{
				case StoneColor.Black:
					{
						color = RawColor.White;
						break;
					}
				case StoneColor.White:
					{
						color = RawColor.Black;
						break;
					}
				case StoneColor.None:
					{
						Graphics.FillCircle(BackgroundColor, point, 0.4f * BlockSize);
						color = RawColor.Black;
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
				Vector2f p1 = new Vector2f(point.X, point.Y - radius);
				Vector2f p2 = new Vector2f(point.X + cos * radius, point.Y + sin * radius);
				Vector2f p3 = new Vector2f(point.X - cos * radius, point.Y + sin * radius);
				Graphics.DrawPolygon(new Pen(color, 2), new Vector2f[] { p1, p2, p3 });
			}
			else if ((s == "#SQ") || (s == "#KO"))
			{
				Vector2f pL = new Vector2f(point.X - 0.25f * BlockSize, point.Y - 0.25f * BlockSize);
				Graphics.DrawRectangle(new Pen(color, 2), RectangleF.FromLTWH(pL.X, pL.Y, (int)(0.5f * BlockSize), (int)(0.5f * BlockSize)));
			}
			else if (s == "#CR")
			{
				Vector2f pL = new Vector2f(point.X - 0.3f * BlockSize, point.Y - 0.3f * BlockSize);
				Graphics.DrawCircle(new Pen(color, 2), point, 0.3f * BlockSize);
			}
			else DrawString(s, GetFont(s.Length), color, new Vector2f(point.X, point.Y), new Vector2f(0.5f, 0.5f));
		}

		private void RenderLines()
		{
			for (int x = 0; x < State.Width; x++)
			{
				Graphics.DrawLine(new Pen(RawColor.Black), GameToImage(x, 0), GameToImage(x, State.Height - 1));
			}
			for (int y = 0; y < State.Height; y++)
			{
				Graphics.DrawLine(new Pen(RawColor.Black), GameToImage(0, y), GameToImage(State.Width - 1, y));
			}
		}

		public StateRenderer(GraphicsSystem graphicsSystem)
			:base(graphicsSystem)
		{

		}
	}
}
