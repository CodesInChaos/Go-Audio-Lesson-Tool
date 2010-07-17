using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Ionic.Zip;
using System.IO;

namespace GoClient
{
	public class BoardSkin : IDisposable
	{
		public Bitmap Board;
		public readonly Bitmap BlackStone;
		public readonly Bitmap WhiteStone;
		public readonly Bitmap StoneShadow;
		public PointF ShadowOffset;
		public Color LineColor = Color.FromArgb(160, Color.Black);
		public Color CoordinateColor = Color.Black;

		private static Bitmap LoadBitmap(ZipEntry zipEntry)
		{
			MemoryStream stream = new MemoryStream();
			zipEntry.Extract(stream);
			stream.Position = 0;
			Bitmap result = new Bitmap(stream);
			stream.Dispose();
			return result;
		}

		private BoardSkin(string filename)
		{
			ZipFile zip = ZipFile.Read(filename);
			Board = LoadBitmap(zip["Board.png"]);
		}

		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
