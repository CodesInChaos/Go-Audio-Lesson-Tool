using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core;

namespace GoClient
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			Log._Log = new MyLog(GlobalSettings.UserDataDir + "GoClient.log").Log;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ParentForm());
		}
	}
}
