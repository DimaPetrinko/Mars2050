using Core.Models;

namespace Presentation
{
	internal interface IPresenterManager
	{
		void Register(IModel model, IPresenter presenter);
		IPresenter Get(IModel model);
	}
}