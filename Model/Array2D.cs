using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class Array2D<T> : IEnumerable<T>
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

	}
}
