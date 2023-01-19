using System;
using System.Collections.Generic;
using Core.Models.Enums;
using Core.Models.Technology;
using Core.Utils;

namespace Core.Models.GameProcess.Implementation
{
	public class Player : IPlayer
	{
		public event Action<ActionType> ActionSelected;
		public event Action ActionCanceled;

		private ActionType? mSelectedAction;

		public Player(Faction faction)
		{
			mResourceHolder = new ResourceHolder();
			mTurnPerformer = new TurnPerformer(faction);
			mTechnologyUser = new TechnologyUser();
		}

		public void SelectAction(ActionType type)
		{
			mSelectedAction = type;
			ActionSelected?.Invoke(mSelectedAction.Value);
		}

		public void CancelAction()
		{
			var hadValue = mSelectedAction.HasValue;
			mSelectedAction = null;
			if (hadValue) ActionCanceled?.Invoke();
		}

		#region IResourceHolder

		private readonly IResourceHolder mResourceHolder;

		public event Action<ResourceType, int> ResourceAmountChanged
		{
			add => mResourceHolder.ResourceAmountChanged += value;
			remove => mResourceHolder.ResourceAmountChanged -= value;
		}

		public int ResourcesAmount => mResourceHolder.ResourcesAmount;

		public bool HasResource(ResourceType resource, int amount)
		{
			return mResourceHolder.HasResource(resource, amount);
		}

		public void AddResources(ResourcePackage resourcePackage)
		{
			mResourceHolder.AddResources(resourcePackage);
		}

		public void UseResources(ResourcePackage resourcePackage)
		{
			mResourceHolder.UseResources(resourcePackage);
		}

		public int GetResource(ResourceType resourceType)
		{
			return mResourceHolder.GetResource(resourceType);
		}

		#endregion

		#region ITurnPerformer

		private readonly ITurnPerformer mTurnPerformer;

		public Faction Faction => mTurnPerformer.Faction;
		public IReactiveProperty<int> Oxygen => mTurnPerformer.Oxygen;
		public IReactiveProperty<bool> HisTurn => mTurnPerformer.HisTurn;

		#endregion

		#region ITechnologyUser

		private readonly ITechnologyUser mTechnologyUser;

		public event Action<ITechnology> TechnologyAdded
		{
			add => mTechnologyUser.TechnologyAdded += value;
			remove => mTechnologyUser.TechnologyAdded -= value;
		}

		public event Action<TechnologyType> TechnologySpent
		{
			add => mTechnologyUser.TechnologySpent += value;
			remove => mTechnologyUser.TechnologySpent -= value;
		}

		public IEnumerable<ITechnology> Technologies => mTechnologyUser.Technologies;

		public void AddTechnology(ITechnology technology)
		{
			mTechnologyUser.AddTechnology(technology);
		}

		public void SpendTechnology(ITechnology technology)
		{
			mTechnologyUser.SpendTechnology(technology);
		}

		#endregion
	}
}