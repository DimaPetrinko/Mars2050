using System;
using System.Linq;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.Technology;

namespace Presentation.GameProcess.Implementation
{
	internal class PlayerPresenter : IPlayerPresenter
	{
		private ActionType? mSelectedAction;

		public IPlayer Model { get; }
		public IPlayerView View { get; }

		public PlayerPresenter(IPlayer model, IPlayerView view)
		{
			Model = model;
			View = view;

			Model.HisTurn.Changed += OnHisTurnChanged;
			Model.Oxygen.Changed += OnOxygenChanged;
			Model.ResourceAmountChanged += OnResourceAmountChanged;
			Model.TechnologyAdded += OnTechnologyAdded;
			Model.TechnologySpent += OnTechnologySpent;

			View.ActionClicked += OnActionClicked;
			View.EndTurnClicked += OnEndTurnClicked;
		}

		public void Initialize()
		{
			View.Faction = Model.Faction;
			OnHisTurnChanged(Model.HisTurn.Value);
			OnOxygenChanged(Model.Oxygen.Value);
			foreach (var type in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
				OnResourceAmountChanged(type, Model.GetResource(type));
		}

		private void OnHisTurnChanged(bool value)
		{
			View.Active = value;
			if (!value) CancelAction();
		}

		private void OnOxygenChanged(int value)
		{
			View.Oxygen = value;
		}

		private void OnResourceAmountChanged(ResourceType type, int amount)
		{
			View.UpdateResource(type, amount);
		}

		private void OnTechnologyAdded(ITechnology value)
		{
			// TODO: create technology presenter and save it to list
			throw new System.NotImplementedException();
		}

		private void OnTechnologySpent(TechnologyType value)
		{
			// TODO: destroy presenter and view and remove from list
			throw new System.NotImplementedException();
		}

		private void OnActionClicked(ActionType type)
		{
			if (mSelectedAction.HasValue)
			{
				if (mSelectedAction == type)
				{
					CancelAction();
					return;
				}
			}
			else SelectAction(type);
		}


		private void OnEndTurnClicked()
		{
			Model.HisTurn.Value = false;
		}

		private void SelectAction(ActionType type)
		{
			mSelectedAction = type;
			View.SelectedAction = mSelectedAction.Value;
			Model.SelectAction(mSelectedAction.Value);
		}

		private void CancelAction()
		{
			mSelectedAction = null;
			View.SelectedAction = null;
			Model.CancelAction();
		}
	}
}