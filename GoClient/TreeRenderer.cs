using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil;
using System.Drawing;
using Model;
using ChaosUtil.Mathematics;

namespace GoClient
{
	public class TreeRenderer
	{
		public Point Scroll;
		public Graphics Graphics;
		public int BlockSize;
		public System.Drawing.Rectangle ClipRect;
		public GraphicalGameTree Tree { get { return Game.Tree; } }
		public Game Game;
		static Font font = new Font("Tahoma", 10, GraphicsUnit.Pixel);

		public PointF ToGraphic(Vector2f position)
		{
			float x = Scroll.X + (position.X + 0.5f) * BlockSize;
			float y = Scroll.Y + (position.Y + 0.5f) * BlockSize;
			return new PointF(x, y);
		}

		public void RenderStone(Vector2f position, StoneColor color, float diameter)
		{
			PointF topLeft = ToGraphic(position - new Vector2f(0.5f, 0.5f) * diameter);
			PointF bottomRight = ToGraphic(position + new Vector2f(0.5f, 0.5f) * diameter);
			Brush brush;
			switch (color)
			{
				case StoneColor.Black:
					brush = Brushes.Black;
					break;
				case StoneColor.White:
					brush = Brushes.White;
					break;
				default:
					brush = Brushes.Transparent;
					break;
			}
			Graphics.FillEllipse(brush, System.Drawing.RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y));
		}

		public void RenderStone(Vector2i position, StoneColor color)
		{
			RenderStone(new Vector2f(position.X, position.Y), color, 0.8f);
		}

		public void Render()
		{
			if (ClipRect.Left != 0)
				1.ToString();
			int left = 0;// (ClipRect.Left - Scroll.X) / BlockSize - 1;
			int top = 0;// (ClipRect.Top - Scroll.Y) / BlockSize - 1;
			int right = (ClipRect.Right - Scroll.X) / BlockSize + 2;
			int bottom = (ClipRect.Bottom - Scroll.Y) / BlockSize + 2;
			Graphics.FillRectangle(Brushes.SandyBrown, ClipRect);
			for (int y = top; y <= bottom; y++)
				for (int x = left; x <= right; x++)
				{
					Vector2i position = new Vector2i(x, y);
					int? node = Tree.NodeAtPosition(position);

					//Render Selection
					if (node == Tree.SelectedNode)
					{
						RenderSelection(position);
					}
					//Render Connections
					Directions connections = Tree.ConnectionsAtPosition(position);
					//Left
					if (connections == (Directions.Left))
						Graphics.DrawLine(Pens.Black, ToGraphic(new Vector2f(x, y)), ToGraphic(new Vector2f(x - 0.5f, y)));
					//Left-Down
					if ((connections & (Directions.Left | Directions.Down)) == (Directions.Left | Directions.Down))
						Graphics.DrawLine(Pens.Black, ToGraphic(new Vector2f(x, y)), ToGraphic(new Vector2f(x, y + 0.5f)));
					//Left-Right
					if ((connections & (Directions.Left | Directions.Right)) == (Directions.Left | Directions.Right))
						Graphics.DrawLine(Pens.Black, ToGraphic(new Vector2f(x + 0.5f, y)), ToGraphic(new Vector2f(x - 0.5f, y)));
					//Up-Down
					if ((connections & (Directions.Up | Directions.Down)) == (Directions.Up | Directions.Down))
						Graphics.DrawLine(Pens.Black, ToGraphic(new Vector2f(x, y + 0.5f)), ToGraphic(new Vector2f(x, y - 0.5f)));
					//Up-Right
					if ((connections & (Directions.Up | Directions.Right)) == (Directions.Up | Directions.Right))
						Graphics.DrawLine(Pens.Black, ToGraphic(new Vector2f(x, y - 0.5f)), ToGraphic(new Vector2f(x + 0.5f, y)));
					//Render Node
					if (node != null)
					{
						int firstActionIndex = Tree.Node((int)node).FirstActionIndex;
						GameAction firstAction = Tree.Replay.Actions[firstActionIndex];

						if (firstAction is MoveAction)
							RenderMove(position, firstActionIndex);
						else if (firstAction is InitStateAction)
							RenderBoardAction(position, firstActionIndex);
						else RenderSetupNode(position);
					}
				}
		}

		private System.Drawing.RectangleF Square(Vector2i position, float size)
		{
			float left = position.X - size;
			float top = position.Y - size;
			float right = position.X + size;
			float bottom = position.Y + size;
			PointF topLeft = ToGraphic(new Vector2f(left, top));
			PointF bottomRight = ToGraphic(new Vector2f(right, bottom));
			System.Drawing.RectangleF rect = System.Drawing.RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
			return rect;
		}

		private void RenderSelection(Vector2i position)
		{
			Graphics.FillRectangle(Brushes.RoyalBlue, Square(position, 0.5f));
		}

		private void RenderSetupNode(Vector2i position)
		{
			Graphics.FillEllipse(Brushes.Gray, Square(position, 0.4f));
		}

		private void RenderBoardAction(Vector2i position, int actionIndex)
		{
			Graphics.FillRectangle(Brushes.Brown, Square(position, 0.4f));
		}

		private void RenderMove(Vector2i position, int actionIndex)
		{
			MoveAction action = (MoveAction)Tree.Replay.Actions[actionIndex];
			RenderStone(position, action.Color);
			Brush brush = Brushes.Black;
			if (action.Color == StoneColor.Black)
				brush = Brushes.White;
			int moveNumber = Game.Replay.MoveNumber(actionIndex);
			PointF pixelPosition = ToGraphic(new Vector2f(position.X, position.Y));
			StateRenderer.DrawString(Graphics, moveNumber.ToString(), font, brush, pixelPosition, new PointF(0.5f, 0.5f));
		}
	}
}
