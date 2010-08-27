using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumiSoft.Media.Wave;
using Xiph.Easy;
using Xiph.Easy.OggVorbis;
using Xiph.Easy.Speex;
using System.Diagnostics;
using Core;

namespace CommonGui
{
	public class MyAsyncRecorder : AsyncRecorder
	{
		private long SampleCount;
		public int SamplesPerSecond { get; private set; }
		private readonly int FrameSize;
		private WaveIn recorder;
		private AudioEncoderState encoderState;
		private SpeexPreProcessor preprocessor;

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

		public bool DeNoise
		{
			get
			{
				lock (RecorderLock)
				{
					return preprocessor.DeNoise;
				}
			}
			set
			{
				lock (RecorderLock)
				{
					preprocessor.DeNoise = value;
				}
			}
		}

		public bool AutomaticGainControl
		{
			get
			{
				lock (RecorderLock)
				{
					return preprocessor.AutomaticGainControl;
				}
			}
			set
			{
				lock (RecorderLock)
				{
					preprocessor.AutomaticGainControl = value;
				}
			}
		}

		public MyAsyncRecorder(Stream stream, float quality)
			: base(stream)
		{
			SamplesPerSecond = 44100;
			FrameSize = 1024;
			OggVorbisEncoderSetup encoderSetup = new OggVorbisEncoderSetup();
			encoderSetup.Channels = 1;
			encoderSetup.SampleRate = SamplesPerSecond;
			//encoderSetup.BitRate = VorbisBitrates.AverageBitrate(64 * 1024);
			encoderSetup.BitRate = VorbisBitrates.VariableBitrate(quality);
			encoderSetup.Comments.AddComment("Go Audio Lesson");
			encoderState = encoderSetup.StartEncode(Stream);

			preprocessor = new SpeexPreProcessor(SamplesPerSecond, FrameSize);

			recorder = new WaveIn(WaveIn.Devices[0], encoderSetup.SampleRate, 16, 1, FrameSize * 2);
			recorder.BufferFull += recorder_BufferFull;
		}

		void recorder_BufferFull(byte[] buffer)
		{
			lock (RecorderLock)
			{
				try
				{
					int sampleCount = buffer.Length / 2;
					Debug.Assert(sampleCount == FrameSize);
					Int16[] data16 = new Int16[sampleCount];
					for (int i = 0; i < sampleCount; i++)
					{
						data16[i] = BitConverter.ToInt16(buffer, i * 2);
					}
					if (preprocessor.AutomaticGainControl || preprocessor.DeNoise)
					{
						preprocessor.Run(data16, 0);
					}

					float[,] data = new float[1, sampleCount];
					for (int i = 0; i < sampleCount; i++)
					{

						float floatSample = data16[i] / (Int16.MaxValue + 1.0f);
						data[0, i] = floatSample;
					}
					encoderState.Write(data);
					SampleCount += sampleCount;
					OnChanged();
				}
				catch (Exception e)
				{
					Log.Exception("recorder_bufferFull", e);
				}
			}
		}
	}
}
