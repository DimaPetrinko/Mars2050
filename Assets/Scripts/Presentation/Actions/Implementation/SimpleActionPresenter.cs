using Core.Models.Actions;
using Core.Models.Enums;
using Core.Utils;

namespace Presentation.Actions.Implementation
{
	internal abstract class SimpleActionPresenter<TAction, TActionView>
		: ISimpleActionPresenter<TAction, TActionView>
		where TAction : class, IAction
		where TActionView : class, ISimpleActionView
	{
		private bool mRepeat;

		public TAction Model { get; }
		public TActionView View { get; }

		protected SimpleActionPresenter(TAction model, TActionView view)
		{
			Model = model;
			View = view;

			Model.Selected.Changed += OnSelectedChanged;
			View.Confirmed += OnActionConfirmed;
			View.Closed += OnViewClosed;
			View.ResourcesChanged += OnResourcesChanged;
		}

		public void Initialize()
		{
			OnSelectedChanged(Model.Selected.Value);
			View.ResourcesRequired = Model.ResourcesRequired;
		}

		protected virtual void ResetData()
		{
			View.CanConfirm = false;
			View.Result = ActionResult.Success;
			mRepeat = false;
			View.Resources = ResourcePackage.Empty();
			View.MaxResources = Model.Performer?.Resources ?? ResourcePackage.Empty();
		}

		private void OnSelectedChanged(bool value)
		{
			View.Active = value;
			ResetData();
		}

		private void OnResourcesChanged()
		{
			Model.Resources = View.Resources;
		}

		private void OnActionConfirmed()
		{
			var result = Model.Perform(mRepeat);

			if (!result.IsSuccess()) View.Result = result;
			else
			{
				mRepeat = true;
				switch (Model.Repeatability)
				{
					case ActionRepeatability.None:
						OnViewClosed();
						break;
					case ActionRepeatability.Continuable:
						if (Model.CheckOxygen().IsSuccess() && Model.CheckPerformerResources().IsSuccess()) ResetData();
						else OnViewClosed();
						break;
				}
			}
		}

		private void OnViewClosed()
		{
			Model.Selected.Value = false;
		}
	}
}