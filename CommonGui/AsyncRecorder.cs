using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonGui
{
	public abstract class AsyncRecorder : IDisposable
	{
		protected readonly object RecorderLock = new object();

		protected abstract void CloseOverride();
		protected abstract void PauseOverride(bool value);
		protected abstract void FlushOverride();

		private bool mPaused = true;
		public bool Paused
		{
			get
			{
				lock (RecorderLock)
				{
					if (Closed)
						throw new ObjectDisposedException("Recorder");
					return mPaused;
				}
			}
			set
			{
				lock (RecorderLock)
				{
					if (Closed)
						throw new ObjectDisposedException("Recorder");
					if (value != mPaused)
					{
						PauseOverride(value);
						mPaused = value;
					}
				}
			}
		}

		public abstract TimeSpan Duration { get; }
		public Stream Stream { get; private set; }

		public void Flush()
		{
			lock (RecorderLock)
			{
				if (Closed)
					throw new ObjectDisposedException("Recorder");
				FlushOverride();
			}
		}

		public AsyncRecorder(Stream stream)
		{
			Stream = stream;
		}

		void IDisposable.Dispose()
		{
			Close();
		}

		public bool Closed { get; private set; }
		public void Close()
		{
			lock (RecorderLock)
			{
				if (Closed)
					throw new ObjectDisposedException("Recorder");
				Closed = true;
				FlushOverride();
				CloseOverride();
			}
		}

		public event EventHandler ChangedAsync;

		protected void OnChanged()
		{
			if (ChangedAsync != null)
				ChangedAsync.BeginInvoke(this, null, null, null);
		}
	}
}
