using Core.Configs.Actions;
using Core.Models.Actions;
using Core.Utils;

namespace Presentation.Actions.Implementation
{
	internal class ActionButtonPresenter : IActionButtonPresenter
	{
		private readonly IGamePresenter mGamePresenter;
		private readonly IActionConfigs mActionConfigs;

		public IAction Model { get; }
		public IActionButtonView View { get; }

		public ActionButtonPresenter(
			IAction model,
			IActionButtonView view,
			IGamePresenter gamePresenter,
			IActionConfigs actionConfigs)
		{
			Model = model;
			View = view;

			mGamePresenter = gamePresenter;
			mActionConfigs = actionConfigs;

			Model.Selected.Updated += OnSelectedUpdated;
			View.Clicked += OnViewClicked;
		}

		public void Initialize()
		{
			View.Name = mActionConfigs.GetConfig(Model.Type).DisplayName;
			OnSelectedUpdated(Model.Selected.Value);
		}

		private void OnSelectedUpdated(bool value)
		{
			View.Selected = value;
			View.Active = (mGamePresenter.SelectedAction == null || mGamePresenter.SelectedAction == Model)
			              && Model.CheckPerformerSet().IsSuccess()
			              && Model.CheckOxygen().IsSuccess()
			              && Model.CheckPerformerResources().IsSuccess();
		}

		private void OnViewClicked()
		{
			Model.Selected.Value = !Model.Selected.Value;
		}
	}
}