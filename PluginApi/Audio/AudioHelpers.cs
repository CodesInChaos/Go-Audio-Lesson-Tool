using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginApi.Audio
{
	internal static class AudioHelpers
	{
		public static long TimeToSample(TimeSpan time, int samplesPerSecond)
		{
			return (long)Math.Round(time.TotalSeconds * samplesPerSecond);
		}

		public static TimeSpan TimeSpanFromSecondsExact(double seconds)
		{
			//TimeSpan.FromSeconds rounds to milliseconds, wtf MS
			return TimeSpan.FromTicks((long)Math.Round(seconds * TimeSpan.TicksPerSecond));
		}

		public static long SampleToTime(long sample, int samplesPerSecond)
		{
			return TimeSpanFromSecondsExact((double)sample / (double)samplesPerSecond);
		}
	}
}
