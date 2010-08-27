using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GoClient
{
	class MyLog
	{
		readonly string Filename;

		public MyLog(string filename)
		{
			Filename = filename;
			File.Delete(filename);
		}

		public void Log(string s)
		{
			File.AppendAllText(Filename, s);
		}
	}
}
