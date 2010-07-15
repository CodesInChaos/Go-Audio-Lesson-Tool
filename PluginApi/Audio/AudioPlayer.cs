using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginApi.Audio
{
	public abstract class AudioPlayer
	{
		private float mVolume;

		public AudioSource AudioSource { get; set; }

		protected abstract void PauseOverride();
		protected abstract void PlayOverride();
		protected abstract void SeekOverride();
		protected abstract void VolumeChangedOverride();
		public TimeSpan Position { get { return AudioHelpers.SampleToTime(SamplePosition, AudioSource.SamplesPerSecond); } }
		public long SamplePosition { get { return AudioSource.SamplePosition; } }

		public bool IsPlaying { get; private set; }
		public bool IsPaused { get; set; }
		public float Volume { get { return mVolume; } set { mVolume = value; VolumeChangedOverride(); } }
		public event EventHandler Finished;
		protected void OnFinished()
		{
			if (Finished != null)
				Finished(this, new EventArgs());
		}

		public void Play()
		{
			PlayOverride();
			Playing = true;
		}

		public void Pause()
		{
			PauseOverride();
			Playing = false;
		}

		public void Stop()
		{
			Pause();
			Seek(0);
		}

		public void Seek(long samplePosition)
		{
			AudioSource.Seek(samplePosition);
			SeekOverride();
		}

		public void Seek(TimeSpan time)
		{
			AudioSource.Seek(time);
			SeekOverride();
		}
	}
}
