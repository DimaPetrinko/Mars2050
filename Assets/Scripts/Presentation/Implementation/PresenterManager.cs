using System.Collections.Generic;
using Core.Models;

namespace Presentation.Implementation
{
	internal class PresenterManager : IPresenterManager
	{
		private readonly Dictionary<IModel, IPresenter> mPresenters = new Dictionary<IModel, IPresenter>();

		public void Register(IModel model, IPresenter presenter)
		{
			mPresenters.Add(model, presenter);
		}

		public IPresenter Get(IModel model)
		{
			return mPresenters.TryGetValue(model, out var presenter) ? presenter : null;
		}
	}
}