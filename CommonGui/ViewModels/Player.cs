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
			throw new NotImplementedException();
		}

		public override TimeSpan Duration
		{
			get { throw new NotImplementedException(); }
		}

		public TimeSpan Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public TimeSpan SeekRelative(TimeSpan offset)
		{
			TimeSpan position = Position + offset;
			if (position < TimeSpan.Zero)
				position = TimeSpan.Zero;
			if (position > Duration)
				position = Duration + TimeSpan.FromTicks(1);
			Position = position;
			return position;
		}
	}
}
