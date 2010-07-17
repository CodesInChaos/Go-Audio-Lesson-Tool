using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GoClient
{
	public static class Config
	{
		public static string UserDataDir { get { return Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\"; } }
	}
}
