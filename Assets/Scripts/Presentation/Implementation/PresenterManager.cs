using System.Collections.Generic;
using System.Linq;
using Core.Models;

namespace Presentation.Implementation
{
	internal class PresenterManager : IPresenterManager
	{
		private readonly List<KeyValuePair<IModel, IPresenter>> mPresenters = new();

		public void Register(IModel model, IPresenter presenter)
		{
			mPresenters.Add(new KeyValuePair<IModel, IPresenter>(model, presenter));
		}

		public IPresenter Get(IModel model)
		{
			return mPresenters.FirstOrDefault(p => p.Key == model).Value;
		}

		public void InitializeAll()
		{
			foreach (var presenter in mPresenters)
			{
				presenter.Value.Initialize();
			}
		}
	}
}