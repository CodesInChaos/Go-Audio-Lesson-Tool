using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BoardImageRecognition
{
	public class Window
	{
		internal class NativeMethods
		{
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern int GetWindowTextLength(HandleRef hWnd);
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);
		}

		private readonly IntPtr mHandle;

		public IntPtr Handle { get { return mHandle; } }

		public string Title
		{
			get
			{
				int capacity = NativeMethods.GetWindowTextLength(new HandleRef(this, Handle)) * 2;//No idea why *2
				StringBuilder lpString = new StringBuilder(capacity);
				NativeMethods.GetWindowText(new HandleRef(this, Handle), lpString, lpString.Capacity);
				return lpString.ToString();
			}
		}

		public Window(IntPtr handle)
		{
			mHandle = handle;
		}
	}
}
