using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sgf
{
	public struct SgfProperty
	{
		public readonly string Name;
		public readonly SgfValue[] Values;

		public SgfProperty(string name, SgfValue value)
		{
			Name = name;
			Values = new SgfValue[] { value };
		}

		public SgfProperty(string name, IEnumerable<SgfValue> values)
		{
			Name = name;
			Values = values.ToArray();
		}
	}
}
