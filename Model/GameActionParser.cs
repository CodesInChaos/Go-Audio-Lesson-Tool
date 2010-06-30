using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosUtil.TreeDocuments;
using System.IO;

namespace Model
{
	public static class GameActionParser
	{
		public static GameAction Parse(TreeDoc doc)
		{
			switch (doc.Name)
			{
				case "M":
					if ((string)doc.Element("", 0) == "P")
						return new PassMoveAction(StoneColorHelper.Parse(doc.Element("", 1)));
					else
						return new StoneMoveAction(
							(Position)doc.Element("", 0),
							StoneColorHelper.Parse(doc.Element("", 1))
							);
				case "S":
					return new SetStoneAction(
							Positions.Parse(doc.Element("", 0)),
							StoneColorHelper.Parse(doc.Element("", 1))
							);
				case "Territory":
					return new TerritoryAction(
							Positions.Parse(doc.Element("", 0)),
							StoneColorHelper.Parse(doc.Element("", 1))
							);
				case "L":
					return new LabelAction(
							Positions.Parse(doc.Element("", 0)),
							(string)doc.Element("", 1)
							);
				case "T":
					return new ReplayTimeAction(TimeSpan.FromSeconds((double)doc.Element("")));
				case "Select":
					return new SelectStateAction((int)doc.Element(""));
				case "Board":
					return new CreateBoardAction((int)doc.Element("", 0), (int)doc.Element("", 1));
				default:
					throw new InvalidDataException("Unknown Action " + doc.Name);
			}
		}
	}
}
