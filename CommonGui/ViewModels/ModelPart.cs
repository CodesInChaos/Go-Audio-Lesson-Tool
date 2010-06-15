using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CommonGui.ViewModels
{
	public class ModelPart
	{
		private readonly object Lock = new object();
		public BaseModel Model { get; private set; }
		public ModelPart(BaseModel model)
		{
			Model = model;
		}

		public long OnChanged()
		{
			lock (Lock)
			{
				VersionCounter = Model.OnChanged();
				return VersionCounter;
			}
		}

		public long VersionCounter { get; private set; }
	}

	public class ModelPart<TModel> : ModelPart
		where TModel : BaseModel
	{
		public new TModel Model { get { return (TModel)base.Model; } }

		public ModelPart(TModel model)
			: base(model)
		{
		}
	}

}
