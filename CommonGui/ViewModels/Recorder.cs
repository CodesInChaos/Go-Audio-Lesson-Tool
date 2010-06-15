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

	public class Recorder : MediaModel
	{
		private readonly MemoryStream stream = new MemoryStream();
		private readonly MyAsyncRecorder recorder;
		private bool Finished = false;

		public RecorderState State
		{
			get
			{
				if (Finished)
					return RecorderState.Finished;
				if (Paused)
					return RecorderState.Paused;
				else
					return RecorderState.Recording;
			}
		}

		public Recorder(ViewModel model)
			: base(model)
		{
			recorder = new MyAsyncRecorder(stream);
			recorder.ChangedAsync += (o, e) => Model.OnChangedAsync();
		}

		public void Finish()
		{
			recorder.Close();
			Finished = true;
		}

		public Stream Data
		{
			get
			{
				if (!Finished)
					throw new InvalidOperationException();
				return stream;
			}
		}

		protected override void PauseOverride()
		{
			recorder.Paused = Paused;
		}

		public override TimeSpan Duration
		{
			get { return recorder.Duration; }
		}
	}
}
