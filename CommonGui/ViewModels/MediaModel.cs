using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonGui.ViewModels
{
	public abstract class MediaModel : ModelPart<ViewModel>
	{
		protected abstract void PauseOverride();

		private bool mPaused = true;
		public bool Paused
		{
			get { return mPaused; }
			set
			{
				if (mPaused == value)
					return;
				mPaused = value;
				PauseOverride();
			}
		}

		public abstract TimeSpan Duration { get; }

		public MediaModel(ViewModel model)
			: base(model)
		{
		}
	}
}
