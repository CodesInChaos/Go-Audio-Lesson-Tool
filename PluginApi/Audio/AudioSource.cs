using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PluginApi.Audio
{
	public abstract class AudioSource
	{
		public int Channels { get; private set; }
		public int SamplesPerSecond { get; private set; }

		public long Length { get; protected set; }
		public long SamplePosition { get; protected set; }

		public TimeSpan Duration { get { return TimeSpan.FromSeconds((double)Length / SamplesPerSecond); } }
		public TimeSpan Position { get { return TimeSpan.FromSeconds((double)SamplePosition / SamplesPerSecond); } }


		public void Seek(TimeSpan time)
		{
			Seek((long)(time.TotalSeconds * SamplesPerSecond));
		}

		public void Seek(long samplePos)
		{
			SeekOverride(samplePos);
		}

		public int Read(float[][] data, int offset, int size)
		{
			return ReadOverride(data, offset, size);
		}


		public AudioSource(int channels, int samplesPerSecond)
		{
			Channels = channels;
			SamplesPerSecond = samplesPerSecond;
		}

		protected abstract int ReadOverride(float[][] data, int offset, int size);
		protected abstract void SeekOverride(long samplePos);

	}
}
