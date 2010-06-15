using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ChaosUtil;

namespace CommonGui.ViewModels
{
	public class ModelChangedEventArgs : EventArgs { };


	public abstract class BaseModel
	{
		private long mVersionCounter;

		public void OnChangedAsync()
		{
			Invoke(() => OnChanged());
		}

		public long OnChanged()
		{
			long result;
			mVersionCounter++;
			result = mVersionCounter;
			if (Changed != null)
				Changed(this, null);
			return result;
		}

		public long VersionCounter
		{
			get
			{
				return mVersionCounter;
			}
		}
		public event EventHandler<ModelChangedEventArgs> Changed;
		private readonly Action<Action> Invoke;

		public BaseModel(Action<Action> invoke)
		{
			Invoke = invoke;
		}
	}
}
