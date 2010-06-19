using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonGui.ViewModels
{
	public class Player : Media
	{
		public readonly Stream Stream;

		public Player(Stream stream)
		{
			Stream = stream;
		}

		protected override void PauseOverride()
		{

		}

		public override TimeSpan Duration
		{
			get { return TimeSpan.Zero; }
		}

		private TimeSpan mPosition;
		public TimeSpan Position
		{
			get
			{
				return mPosition;
			}
		}

		public TimeSpan Seek(TimeSpan position)
		{
			if (position < TimeSpan.Zero)
				position = TimeSpan.Zero;
			if (position > Duration)
				position = Duration;
			mPosition = position;
			return Position;
		}

		public TimeSpan SeekRelative(TimeSpan offset)
		{
			TimeSpan position = Position + offset;
			return Seek(Position);
		}

		public override void Dispose()
		{
		}
	}
}
