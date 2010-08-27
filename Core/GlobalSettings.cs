using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Core
{
	public class GlobalSettings
	{
		public static string UserDataDir { get { return Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\"; } }
	}
}
