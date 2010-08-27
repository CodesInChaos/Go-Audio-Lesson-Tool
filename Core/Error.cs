using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static class Log
	{
		public static Action<string> _Log;

		public static void Message(string s)
		{
			_Log(s);
		}

		public static void Exception(string s, Exception e)
		{
			Message(s + " Exception " + e.GetType().Name + " " + e.Message);
		}
	}
}
