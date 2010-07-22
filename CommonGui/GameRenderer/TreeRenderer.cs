using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util;
using Model;
using Chaos.Util.Mathematics;
using CommonGui.Drawing;
using Chaos.Image;

namespace CommonGui.GameRenderer
{
	public class TreeRenderer : GoRenderer
	{
		public Vector2i Scroll;
		public int BlockSize;
		public RectangleI ClipRect;
		public GraphicalGameTree Tree { get { return Game.Tree; } }
		public Game Game;
		static Font font = new Font("Tahoma", 10);

		public Vector2f ToGraphic(Vector2f position)
		{
			float x = Scroll.X + (position.X + 0.5f) * BlockSize;
			float y = Scroll.Y + (position.Y + 0.5f) * BlockSize;
			return new Vector2f(x, y);
		}

		private RawColor FadedBlack = RawColor.FromARGB(140, 0, 0, 0);
		private RawColor FadedWhite = RawColor.FromARGB(140, 255, 255, 255);

		public void RenderStone(Vector2f position, StoneColor color, float diameter, bool faded)
		{
			RawColor rawColor;
			switch (color)
			{
				case StoneColor.Black:
					rawColor = RawColor.Black;
					break;
				case StoneColor.White:
					rawColor = RawColor.White;
					break;
				default:
					rawColor = RawColor.Transparent;
					break;
			}
			if (faded)
				rawColor = RawColor.FromARGB(140, rawColor);
			Graphics.FillCircle(rawColor, ToGraphic(position), 0.5f * diameter*BlockSize);
		}

		public void RenderStone(Vector2i position, StoneColor color, bool faded)
		{
			RenderStone(new Vector2f(position.X, position.Y), color, 0.8f, faded);
		}

		public void Render()
		{
			int left = 0;// (ClipRect.Left - Scroll.X) / BlockSize - 1;
			int top = 0;// (ClipRect.Top - Scroll.Y) / BlockSize - 1;
			int right = (ClipRect.Right - Scroll.X) / BlockSize + 2;
			int bottom = (ClipRect.Bottom - Scroll.Y) / BlockSize + 2;
			Graphics.FillRectangle(BackgroundColor, ClipRect);
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
						Graphics.DrawLine(new Pen(RawColor.Black), ToGraphic(new Vector2f(x, y)), ToGraphic(new Vector2f(x - 0.5f, y)));
					//Left-Down
					if ((connections & (Directions.Left | Directions.Down)) == (Directions.Left | Directions.Down))
						Graphics.DrawLine(new Pen(RawColor.Black), ToGraphic(new Vector2f(x, y)), ToGraphic(new Vector2f(x, y + 0.5f)));
					//Left-Right
					if ((connections & (Directions.Left | Directions.Right)) == (Directions.Left | Directions.Right))
						Graphics.DrawLine(new Pen(RawColor.Black), ToGraphic(new Vector2f(x + 0.5f, y)), ToGraphic(new Vector2f(x - 0.5f, y)));
					//Up-Down
					if ((connections & (Directions.Up | Directions.Down)) == (Directions.Up | Directions.Down))
						Graphics.DrawLine(new Pen(RawColor.Black), ToGraphic(new Vector2f(x, y + 0.5f)), ToGraphic(new Vector2f(x, y - 0.5f)));
					//Up-Right
					if ((connections & (Directions.Up | Directions.Right)) == (Directions.Up | Directions.Right))
						Graphics.DrawLine(new Pen(RawColor.Black), ToGraphic(new Vector2f(x, y - 0.5f)), ToGraphic(new Vector2f(x + 0.5f, y)));
					//Render Node
					if (node != null)
					{
						int firstActionIndex = Tree.Node((int)node).FirstActionIndex;
						GameAction firstAction = Tree.Replay.Actions[firstActionIndex];

						if (firstAction is MoveAction)
							RenderMove(position, firstActionIndex);
						else if (firstAction is CreateBoardAction)
							RenderBoardAction(position, firstActionIndex);
						else RenderSetupNode(position);
					}
				}
		}

		private RectangleF Square(Vector2i position, float size)
		{
			float left = position.X - size;
			float top = position.Y - size;
			float right = position.X + size;
			float bottom = position.Y + size;
			Vector2f topLeft = ToGraphic(new Vector2f(left, top));
			Vector2f bottomRight = ToGraphic(new Vector2f(right, bottom));
			RectangleF rect = RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
			return rect;
		}

		RawColor SelectionColor = RawColor.FromRGB(0x4169E1);//RoyalBlue

		private void RenderSelection(Vector2i position)
		{
			Graphics.FillRectangle(SelectionColor, Square(position, 0.5f));
		}

		private void RenderSetupNode(Vector2i position)
		{
			Graphics.FillCircle(RawColor.Gray, ToGraphic(position), 0.4f * BlockSize);
		}

		private void RenderBoardAction(Vector2i position, int actionIndex)
		{
			Graphics.FillRectangle(RawColor.FromRGB(0xC57B10), Square(position, 0.4f));
			CreateBoardAction action = (CreateBoardAction)Tree.Replay.Actions[actionIndex];
			Vector2f pixelPosition = ToGraphic(new Vector2f(position.X, position.Y));
			if (action.Width == action.Height)
				DrawString(action.Width.ToString(), font, RawColor.Black, pixelPosition, new Vector2f(0.5f, 0.5f));
		}

		private void RenderMove(Vector2i position, int actionIndex)
		{
			MoveAction action = (MoveAction)Tree.Replay.Actions[actionIndex];
			RenderStone(position, action.Color, !Tree.IsInCurrentVariation(actionIndex));
			RawColor color;
			if (action.Color == StoneColor.Black)
				color = RawColor.White;
			else
				color = RawColor.Black;
			int moveNumber = Game.Replay.MoveNumber(actionIndex);
			Vector2f pixelPosition = ToGraphic(new Vector2f(position.X, position.Y));
			DrawString(moveNumber.ToString(), font, color, pixelPosition, new Vector2f(0.5f, 0.5f));
		}

		public TreeRenderer(GraphicsSystem graphicsSystem)
			: base(graphicsSystem)
		{

		}
	}
}
