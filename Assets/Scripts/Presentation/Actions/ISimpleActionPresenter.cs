using Core.Models.Actions;

namespace Presentation.Actions
{
	internal interface ISimpleActionPresenter<out TAction, out TActionView>
		: IPresenter<TAction, TActionView>
		where TAction : class, IAction
		where TActionView : class, ISimpleActionView
	{
	}
}