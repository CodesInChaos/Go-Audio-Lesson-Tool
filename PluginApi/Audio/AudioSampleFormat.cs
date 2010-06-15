using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginApi.Audio
{
	public class AudioSampleFormat
	{
		public int SamplesPerSecond { get; private set; }
		public int Channels { get; private set; }
		public int BytesPerSample { get; private set; }

		public int BytesPerSecond { get { return BytesPerSample * BlockSize; } }
		public int BitsPerSample { get { return BytesPerSample * 8; } }
		public int BlockSize { get { return Channels * BytesPerSample; } }

		public AudioSampleFormat(int samplesPerSecond, int channels, int bytesPerSample)
		{
			SamplesPerSecond = samplesPerSecond;
			Channels = channels;
			BytesPerSample = bytesPerSample;
		}

		public Int64 TimeToByte(TimeSpan time)
		{
			return (Int64)(time.TotalSeconds * BytesPerSecond);
		}
	}
}
