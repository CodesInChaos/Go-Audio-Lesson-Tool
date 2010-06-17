using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sgf
{
	public struct SgfValue
	{
		public readonly string RawValue;
		public double AsReal()
		{
			return double.Parse(RawValue);
		}

		public string AsSimpleText()
		{
			return RawValue;
		}

		public string AsFormattedText()
		{
			return RawValue;
		}

		public int AsInt()
		{
			return int.Parse(RawValue);
		}

		public SgfValue[] AsCompound()
		{
			throw new NotImplementedException();
		}

		private SgfValue(string rawValue)
		{
			RawValue = rawValue;
		}
	}
}
