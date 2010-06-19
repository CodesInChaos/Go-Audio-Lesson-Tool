using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumiSoft.Media.Wave;
using Xiph.Easy;
using Xiph.Easy.OggVorbis;

namespace CommonGui
{
	public class MyAsyncRecorder : AsyncRecorder
	{
		private long SampleCount;
		public int SamplesPerSecond { get; private set; }
		private WaveIn recorder;
		private AudioEncoderState encoderState;

		protected override void CloseOverride()
		{
			recorder.Dispose();
			encoderState.Finish();
		}

		protected override void PauseOverride(bool value)
		{
			if (!value)
				recorder.Start();
			else
				recorder.Stop();
		}

		protected override void FlushOverride()
		{
			if (!Closed)
				throw new NotSupportedException();
		}

		public override TimeSpan Duration
		{
			get
			{
				lock (RecorderLock)
				{
					return TimeSpan.FromSeconds((double)SampleCount / SamplesPerSecond);
				}
			}
		}

		public MyAsyncRecorder(Stream stream, float quality)
			: base(stream)
		{
			SamplesPerSecond = 44100;
			OggVorbisEncoderSetup encoderSetup = new OggVorbisEncoderSetup();
			encoderSetup.Channels = 1;
			encoderSetup.SampleRate = SamplesPerSecond;
			//encoderSetup.BitRate = VorbisBitrates.AverageBitrate(64 * 1024);
			encoderSetup.BitRate = VorbisBitrates.VariableBitrate(quality);
			encoderSetup.Comments.AddComment("Go Audio Lesson");
			encoderState = encoderSetup.StartEncode(Stream);

			recorder = new WaveIn(WaveIn.Devices[0], encoderSetup.SampleRate, 16, 1, (int)(SamplesPerSecond * 2 * 0.1));
			recorder.BufferFull += recorder_BufferFull;
		}

		void recorder_BufferFull(byte[] buffer)
		{
			lock (RecorderLock)
			{
				int sampleCount = buffer.Length / 2;
				float[,] data = new float[1, sampleCount];
				for (int i = 0; i < sampleCount; i++)
				{
					Int16 intSample = BitConverter.ToInt16(buffer, i * 2);
					float floatSample = intSample / (Int16.MaxValue + 1.0f);
					data[0, i] = floatSample;
				}
				encoderState.Write(data);
				SampleCount += sampleCount;
				OnChanged();
			}
		}
	}
}
