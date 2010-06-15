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
		public abstract long Length { get; }
		public abstract void Seek(long samplePos);
		public void Seek(TimeSpan time)
		{
			Seek((long)(time.TotalSeconds * SamplesPerSecond));
		}
		public TimeSpan Duration { get { return TimeSpan.FromSeconds((double)Length / SamplesPerSecond); } }
		public abstract long SamplePosition { get; }
		public TimeSpan Position { get { return TimeSpan.FromSeconds((double)SamplePosition / SamplesPerSecond); } }
		public abstract int Read(float[,] data, int offset, int size);

		public AudioSource(int channels, int samplesPerSecond)
		{
			Channels = channels;
			SamplesPerSecond = samplesPerSecond;
		}
	}
}
