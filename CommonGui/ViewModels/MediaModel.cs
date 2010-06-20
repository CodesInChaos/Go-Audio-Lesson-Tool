using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonGui.ViewModels
{
	public abstract class Media : IDisposable
	{
		public abstract bool Paused { get; set; }
		public abstract TimeSpan Duration { get; }

		public Media()
		{
		}

		public abstract void Dispose();
	}
}
