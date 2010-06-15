using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace CommonGui.ViewModels
{
	public class ViewModel : BaseModel
	{
		public readonly Game Game;
		private TimeSpan mTime;
		public TimeSpan Time { get { return mTime; } set { mTime = value; OnChanged(); } }
		public Editor Editor { get; set; }
		public MediaModel Media { get; set; }
		public Player Player { get { return Media as Player; } }
		public Recorder Recorder { get { return Media as Recorder; } }

		public ViewModel(Action<Action> invoke)
			: base(invoke)
		{
		}
	}
}
