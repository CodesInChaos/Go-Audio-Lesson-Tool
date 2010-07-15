using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginApi.Audio
{
	public class AudioDataReadyEventArgs : EventArgs
	{
		public float[][] Buffer { get; private set; }
		public int Offset { get; private set; }
		public int Size { get; private set; }

		public AudioDataReadyEventArgs(float[][] buffer, int offset, int size)
		{
			Buffer = buffer;
			Offset = offset;
			Size = size;
			int length = buffer[0].Length;
			for (int channel = 0; channel < buffer.Length; channel++)
				if (buffer[channel].Length != length)
					throw new ArgumentException("buffer");
			if ((UInt32)offset >= buffer.Length)
				throw new ArgumentException("Offset out of range");
			if ((UInt32)(offset + size) > buffer.Length)
				throw new ArgumentException("Size out of range");
		}

		public AudioDataReadyEventArgs(float[][] buffer)
			: this(buffer, 0, buffer[0].Length)
		{

		}
	}

	public abstract class AudioRecorder
	{
		public AudioSampleFormat SampleFormat { get; private set; }

		private bool mRecording;

		public bool Recording
		{
			get { return mRecording; }
			set
			{
				if (mRecording == value)
					return;
				mRecording = value;
				if (value)
					StartOverride();
				else
					StopOverride();
			}
		}

		public bool Paused { get; set; }
		public int SamplesReady { get; }
		public int BufferSize { get; }

		public void Read(float[][] data, int offset, int size)
		{
			if (size > SamplesReady)
				throw new InvalidOperationException("Not enough samples ready");
		}

		public void Start()
		{
			Recording = true;
		}

		public void Stop()
		{
			Recording = false;
		}

		protected abstract void StartOverride();
		protected abstract void StopOverride();

		public event EventHandler<AudioDataReadyEventArgs> DataReady;
		protected void OnDataReady(float[,] buffer, int offset, int size)
		{
			if (DataReady != null)
				DataReady(this, new AudioDataReadyEventArgs(buffer, offset, size));
		}
		protected void OnDataReady(float[,] buffer)
		{
			if (DataReady != null)
				DataReady(this, new AudioDataReadyEventArgs(buffer));
		}

		public AudioRecorder(AudioSampleFormat sampleFormat)
		{
			SampleFormat = sampleFormat;
		}
	}
}

