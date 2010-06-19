using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonGui.ViewModels
{
	public abstract class Media : IDisposable
	{
		protected abstract void PauseOverride();

		private bool mPaused = true;
		public bool Paused
		{
			get { return mPaused; }
			set
			{
				if (mPaused == value)
					return;
				mPaused = value;
				PauseOverride();
			}
		}

		public abstract TimeSpan Duration { get; }

		public Media()
		{
		}

		public abstract void Dispose();
	}
}
