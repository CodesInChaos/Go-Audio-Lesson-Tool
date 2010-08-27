using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonGui.ViewModels
{
	public enum RecorderState
	{
		Recording,
		Paused,
		Finished
	}

	public class Recorder : Media
	{
		private readonly MemoryStream stream = new MemoryStream();
		private readonly MyAsyncRecorder recorder;
		private bool IsFinished = false;

		public RecorderState State
		{
			get
			{
				if (IsFinished)
					return RecorderState.Finished;
				if (Paused)
					return RecorderState.Paused;
				else
					return RecorderState.Recording;
			}
		}

		public Recorder(float quality)
		{
			recorder = new MyAsyncRecorder(stream, quality);
		}

		public void Finish()
		{
			if (IsFinished)
				return;
			recorder.Close();
			IsFinished = true;
		}

		public Stream Data
		{
			get
			{
				if (!IsFinished)
					throw new InvalidOperationException();
				return stream;
			}
		}

		public override TimeSpan Duration
		{
			get { return recorder.Duration; }
		}

		public override void Dispose()
		{
			recorder.Close();
		}


		private bool mPaused = true;
		public override bool Paused
		{
			get
			{
				return mPaused;
			}
			set
			{
				mPaused = value;
				recorder.Paused = Paused;
			}
		}

		public bool DeNoise { get { return recorder.DeNoise; } set { recorder.DeNoise = value; } }
		public bool AutomaticGainControl { get { return recorder.AutomaticGainControl; } set { recorder.AutomaticGainControl = value; } }
	}
}
