using System;
using Core.Configs.Actions;
using Core.Models.Actors;
using Core.Models.Boards;
using Core.Models.Enums;
using Core.Utils;

namespace Core.Models.Actions.Implementation
{
	public class MoveAction : BaseAction, IMoveAction
	{
		private readonly IMoveConfig mConfig;
		private readonly int mCombinedHealth;

		public ICell From { private get; set; }
		public ICell To { private get; set; }

		public MoveAction(
			IMoveConfig config,
			int combinedHealth
			) : base(config)
		{
			mConfig = config;
			mCombinedHealth = combinedHealth;

			mConditions.AddRange(new Func<ActionResult>[]
			{
				CheckStartCell,
				CheckDestinationCell,
				CheckMoveRange,
			});
		}

		public ActionResult CheckStartCell()
		{
			if (From == null) return ActionResult.NoCellProvided;
			if (!From.TryGetPlaceable(out IMovable movable))
				return ActionResult.NoMovableInCell;
			if (movable is IActor actor && actor.Faction != Performer.Faction)
				return ActionResult.NoMovableActorOfCorrectFactionInCell;
			return ActionResult.Success;
		}

		public ActionResult CheckDestinationCell()
		{
			if (To == null) return ActionResult.NoCellProvided;
			if (!mConfig.CanMoveToOccupiedCell
			    && !To.HasActor<IBaseBuilding>(Performer.Faction)
			    && !To.HasActor<IBuilding>(Performer.Faction)
			    && (To.HasPlaceable<IBaseBuilding>() || To.HasPlaceable<IBuilding>() ||
			        To.HasPlaceable<IMovable>()))
				return ActionResult.CellIsOccupied;
			if (!mConfig.CanMoveToDamagedBuilding
			    && To.TryGetPlaceable(out IBuilding building)
			    && building.Health.Value < building.MaxHealth.Value)
				return ActionResult.CellHasDamagedBuilding;
			return ActionResult.Success;
		}

		public ActionResult CheckMoveRange()
		{
			var magnitude = (From.Position - To.Position).Magnitude();
			if (magnitude == 0) return ActionResult.SameCell;
			if (magnitude > mConfig.MoveRange) return ActionResult.ExceedsRange;
			return ActionResult.Success;
		}

		protected override void ApplyAction()
		{
			var movable = From.GetPlaceable<IMovable>();
			From.RemovePlaceable(movable);
			To.AddPlaceable(movable);

			if (movable is IDamageable damageable)
			{
				if (From.TryGetPlaceable(out IBuilding fromBuilding)) fromBuilding.RestoreMaxHealth();
				if (To.TryGetPlaceable(out IBuilding toBuilding))
				{
					toBuilding.MaxHealth.Value = mCombinedHealth;
					damageable.MaxHealth.Value = mCombinedHealth;
				}
				else damageable.RestoreMaxHealth();
			}
		}

		protected override void ResetState()
		{
			base.ResetState();
			From = null;
			To = null;
		}
	}
}