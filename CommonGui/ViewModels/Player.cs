using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EasyOggVorbis;
using LumiSoft.Media.Wave;
using System.Threading;

namespace CommonGui.ViewModels
{
	public class Player : Media
	{
		public readonly Stream Stream;
		private readonly OggVorbisDecoder Decoder;
		private readonly WaveOut WaveOut;
		private readonly Thread Thread;
		private int Disposed = 0;
		private object PlayerLock = new object();
		private const int samplesPerBlock = 2048;
		private void ThreadFunc()
		{
			while (Thread.VolatileRead(ref Disposed) == 0)
			{
				lock (PlayerLock)
				{
					if (!Paused)

						while (WaveOut.BytesBuffered < samplesPerBlock * 2 * 2)
						{
							float[,] data = new float[Decoder.Channels, samplesPerBlock];
							int count = Decoder.Read(data, 0, data.GetLength(1));
							if (count == 0)
								break;
							byte[] byteData = new byte[count * 2];
							for (int i = 0; i < count; i++)
							{
								Int16 sample = (Int16)(data[0, i] * 0.5f * Int16.MaxValue);
								byteData[i * 2] = (byte)sample;
								byteData[i * 2 + 1] = (byte)(sample >> 8);
							}
							mPosition += Decoder.SampleToTime(count);
							WaveOut.Play(byteData, 0, count * 2);
						}
				}
				Thread.Sleep(16);
			}
		}

		public Player(Stream stream)
		{
			Stream = stream;
			Decoder = new OggVorbisDecoder(stream);
			if (Decoder.Channels != 1)
				throw new InvalidDataException("Only mono sounds supported");
			WaveOut = new WaveOut(WaveOut.Devices[0], Decoder.SamplesPerSecond, 16, Decoder.Channels);
			Thread = new Thread(ThreadFunc);
			Thread.Start();
		}

		public override TimeSpan Duration
		{
			get
			{
				lock (PlayerLock)
				{
					return Decoder.Duration;
				}
			}
		}

		private TimeSpan mPosition;
		public TimeSpan Position
		{
			get
			{
				lock (PlayerLock)
				{
					return mPosition;
				}
			}
			set
			{
				lock (PlayerLock)
				{
					mPosition = value;
				}
			}
		}

		private readonly TimeSpan EndDelta = TimeSpan.FromSeconds(0.0001);
		public TimeSpan Seek(TimeSpan position)
		{
			lock (PlayerLock)
			{
				if (position < TimeSpan.Zero)
					position = TimeSpan.Zero;
				if (position > Duration - EndDelta)
					position = Duration - EndDelta;
				Position = Decoder.Seek(position);
				return Position;
			}
		}

		public TimeSpan SeekRelative(TimeSpan offset)
		{
			TimeSpan position = Position + offset;
			return Seek(Position);
		}

		public override void Dispose()
		{
			lock (PlayerLock)
			{
				Disposed = 1;
				WaveOut.Dispose();
			}
		}

		private bool mPaused = true;
		public override bool Paused
		{
			get
			{
				lock (PlayerLock)
				{
					return mPaused;
				}
			}
			set
			{
				lock (PlayerLock)
				{
					mPaused = value;
				}
			}
		}
	}
}
