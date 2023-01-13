using Core.Models;

namespace Presentation
{
	internal interface IPresenter
	{
		void Initialize();
	}

	internal interface IPresenter<out TModel, out TView> : IPresenter where TModel : IModel where TView : IView
	{
		TModel Model { get; }
		TView View { get; }
	}
}