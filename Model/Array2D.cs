using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class Array2D<T> : IEnumerable<T>, ICloneable
	{
		T[,] arr;
		public T this[int x, int y]
		{
			get
			{
				return arr[x, y];
			}
			set
			{
				arr[x, y] = value;
			}
		}

		public T this[Position p]
		{
			get
			{
				return this[p.X, p.Y];
			}
			set
			{
				this[p.X, p.Y] = value;
			}
		}

		public int Width { get { return arr.GetUpperBound(0) + 1; } }
		public int Height { get { return arr.GetUpperBound(1) + 1; } }

		public Array2D(int width, int height)
		{
			arr = new T[width, height];
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return arr.Cast<T>().GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return arr.GetEnumerator();
		}

		#endregion

		public Array2D<T> Clone()
		{
			Array2D<T> result = new Array2D<T>(Width, Height);
			for (int y = 0; y < Height; y++)
				for (int x = 0; x < Width; x++)
					result[x, y] = this[x, y];
			return result;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
