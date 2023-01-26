using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs.Actions;
using Core.Implementation;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Services.GameProcess.Implementation;
using Core.Utils;

namespace Core.Models.Actions.Implementation
{
	public abstract class BaseAction : IAction
	{
		private readonly IActionConfig mConfig;
		protected readonly List<Func<ActionResult>> mConditions;

		private bool mRepeat;
		private bool mCheckAndUseOxygen;
		private ResourcePackage mResourcesToUse;
		private IPlayer mPerformer;

		public ActionType Type => mConfig.Type;
		public IReactiveProperty<bool> Selected { get; }
		public bool Repeatable => mConfig.Repeatable;
		public bool ResourcesRequired => mConfig.Resources.Sum(r => r.Amount) > 0;

		public IPlayer Performer
		{
			protected get { return mPerformer; }
			set
			{
				mPerformer = value;
				Selected.Value = false;
			}
		}

		public ResourcePackage Resources { protected get; set; } = ResourcePackage.Empty();

		protected BaseAction(IActionConfig config)
		{
			Selected = new ReactiveProperty<bool>(false, CustomSelectedSetter);
			mConfig = config;

			mConditions = new List<Func<ActionResult>>()
			{
				CheckPerformerSet,
				CheckIfSelected,
				CheckOxygen,
				CheckPerformerResources
			};
		}

		public ActionResult Perform(bool repeat)
		{
			mRepeat = repeat;
			var actionResult = CheckAllConditions();
			if (!actionResult.IsSuccess()) return actionResult;

			ApplyAction(Performer, mConfig.Oxygen, mCheckAndUseOxygen, mResourcesToUse, ApplyAction);

			return actionResult;
		}

		public ActionResult CheckAllConditions()
		{
			var actionResult = ActionResult.Success;

			foreach (var condition in mConditions)
			{
				actionResult = condition();
				if (!actionResult.IsSuccess()) return actionResult;
			}

			return actionResult;
		}

		public ActionResult CheckPerformerSet()
		{
			if (Performer == null) return ActionResult.PerformerNotSet;
			return ActionResult.Success;
		}

		public ActionResult CheckIfSelected()
		{
			if (!Selected.Value) return ActionResult.NotSelected;
			return ActionResult.Success;
		}

		public ActionResult CheckOxygen()
		{
			mCheckAndUseOxygen = !(mRepeat && mConfig.Repeatable);
			if (mCheckAndUseOxygen && Performer.Oxygen.Value < mConfig.Oxygen) return ActionResult.NotEnoughOxygen;
			return ActionResult.Success;
		}

		public ActionResult CheckPerformerResources()
		{
			if (!mConfig.Resources.All(costData =>
				    costData.Relation == ResourceRelation.Same
					    ? costData.Type.AsCollection().Any(r => Performer.HasResource(r, costData.Amount))
					    : Performer.ResourcesAmount >= costData.Amount))
				return ActionResult.NotEnoughResources;
			return ActionResult.Success;
		}

		public ActionResult CheckProvidedResources()
		{
			if (Resources.Amount == 0 && ResourcesRequired) return ActionResult.NoResourcesProvided;
			(mResourcesToUse, _) = new ResourceProcessor().Process(Resources, mConfig.Resources);
			if (mResourcesToUse.Amount == 0 && ResourcesRequired) return ActionResult.NoResourcesProvided;
			return ActionResult.Success;
		}

		protected abstract void ApplyAction();

		protected virtual void ResetState()
		{
			mRepeat = false;
			mCheckAndUseOxygen = false;
			mResourcesToUse = ResourcePackage.Empty();
		}

		private void ApplyAction(
			IPlayer performer,
			byte oxygen,
			bool useOxygen,
			ResourcePackage resourcesToUse,
			Action applyAction)
		{
			if (useOxygen) performer.Oxygen.Value -= oxygen;
			performer.UseResources(resourcesToUse);
			applyAction();
		}

		private void CustomSelectedSetter(bool value, bool currentValue, Action<bool> setValue, Action triggerChanged)
		{
			if (!value) ResetState();
			setValue(value);
			if (!currentValue.Equals(value)) triggerChanged?.Invoke();
		}

		// TODO: left for compatibility
		protected bool EnoughResources(IResourceHolder resourceHolder, ResourcePackage resources)
		{
			return resources.Content.All(r => resourceHolder.HasResource(r.Key, r.Value));
		}
	}
}