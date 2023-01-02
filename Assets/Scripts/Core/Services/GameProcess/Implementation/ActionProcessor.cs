using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs.Actions;
using Core.Configs.Buildings;
using Core.Models.Actors;
using Core.Models.Boards;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Utils;

namespace Core.Services.GameProcess.Implementation
{
	public interface IFactory<out T, in TParam1>
	{
		T Create(TParam1 param1);
	}

	public interface IActionResult
	{
	}
	public enum ActionResult
	{
		Success,
		NotEnoughOxygen,
		NotEnoughResources,
		NoUnitOfCorrectFactionInCell,
		CellIsOccupied,
		CellHasDamagedBuilding,
		ExceedsRange,
		NoResourceInCell,
		ResourceAlreadyDiscovered,
		CellIsOccupiedByBuilding,
	}

	public class ActionProcessor : IActionProcessor
	{
		private IFactory<IBuilding, IBuildingConfig> mBuildingFactory;

		public ActionResult Move(IPlayer performer, ICell from, ICell to, IMoveConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!from.TryGetActor(out IUnit movable, performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (!config.CanMoveToOccupiedCell && to.HasPlaceable<IMovable>()) return ActionResult.CellIsOccupied;
			if (!config.CanMoveToDamagedBuilding && to.TryGetPlaceable(out IBuilding building) &&
			    building.Health.Value < building.MaxHealth.Value)
				return ActionResult.CellHasDamagedBuilding;
			if ((from.Position - to.Position).magnitude > config.MoveRange) return ActionResult.ExceedsRange;

			return ApplyAction(performer, config, () =>
			{
				from.RemovePlaceable(movable);
				to.AddPlaceable(movable);
			});
		}

		public ActionResult Discover(IPlayer performer, ICell cell, IDiscoverConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!cell.HasActor<IUnit>(performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (!cell.TryGetPlaceable(out IResource resource)) return ActionResult.NoResourceInCell;
			if (resource.IsDiscovered.Value) return ActionResult.ResourceAlreadyDiscovered;

			return ApplyAction(performer, config, () =>
			{
				resource.IsDiscovered.Value = true;
			});
		}

		public ActionResult Gather(IPlayer performer, IEnumerable<ICell> cells, int roll, IGatherConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			var resources = config.GetResourcesForRoll(roll).AsCollection().ToList();
			var resourcesToGather = cells
				.Where(c => c.HasPlaceable<IResource>() && c.HasActor<IResourceGatherer>(performer.Faction))
				.Select(c => c.GetPlaceable<IResource>())
				.Where(r => r.IsDiscovered.Value)
				.Where(r => resources.Contains(r.Type))
				.Select(r => r.Type)
				.ToArray();

			return ApplyAction(performer, config, () =>
			{
				performer.AddResources(resourcesToGather);
			});
		}

		public ActionResult Build(IPlayer performer, ICell cell, IBuildConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!cell.TryGetPlaceable(out IResource resource)) return ActionResult.NoResourceInCell;
			if (!cell.HasActor<IUnit>(performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (cell.HasActor<IBuilding>()) return ActionResult.CellIsOccupiedByBuilding;

			var buildingConfig = config.BuildingConfigs.GetConfig(resource.Type);
			if (!EnoughResources(performer, buildingConfig.BuildCost)) return ActionResult.NotEnoughResources;

			return ApplyAction(performer, config, () =>
			{
				// performer.UseResources(buildingConfig.BuildCost);
				var building = mBuildingFactory.Create(buildingConfig);
				cell.AddPlaceable(building);
			});
		}

		public ActionResult Attack(IPlayer performer, ICell from, ICell to, bool repeat, int roll, IAttackConfig config)
		{
			throw new NotImplementedException();
		}

		public ActionResult Attack(IPlayer performer, ICell from, ICell to, bool repeat, int roll,
			ResourceType resourceToUse, IAttackConfig config)
		{
			if (!(repeat && config.Repeatable) && !EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!from.HasActor<IUnit>(performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (!to.TryGetActor(out IDamageable other) && ((IActor)other).Faction == performer.Faction)
				return ActionResult.NoUnitOfCorrectFactionInCell;
			if ((from.Position - to.Position).magnitude > config.AttackRange) return ActionResult.ExceedsRange;

			return ApplyAction(performer, config, () =>
			{
				other.Damage(config.GetDamageForRoll(roll));
			});
		}

		public ActionResult Heal(IPlayer performer, ICell cell, bool repeat, IHealConfig config)
		{
			if (!(repeat && config.Repeatable) && !EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!cell.TryGetActor(out IDamageable damageable) && ((IActor)damageable).Faction != performer.Faction)
				return ActionResult.NoUnitOfCorrectFactionInCell;

			return ApplyAction(performer, config, () =>
			{
				damageable.Heal(config.Amount);
			});
		}

		public ActionResult Trade(IPlayer performer, ResourceType from, ResourceType to, ITradeConfig config)
		{
			return ApplyAction(performer, config, () =>
			{
				var resourceData = new ResourceData(to, config.PurchaseResource.Amount);
				performer.AddResources(resourceData.AsArray());
			});
		}

		public ActionResult PlayerTrade(IPlayer performer, IResourceHolder other, ResourceType from, ResourceType to,
			ITradeConfig config)
		{
			throw new NotImplementedException();
		}

		public ActionResult PlayerTrade(IPlayer performer, IResourceHolder other, ResourceData from, ResourceData to,
			ITradeConfig config)
		{
			return ApplyAction(performer, config, () =>
			{
				performer.UseResources(from.AsArray());
				other.AddResources(from.AsArray());

				performer.AddResources(to.AsArray());
				other.UseResources(to.AsArray());
			});
		}

		private ActionResult ApplyAction(IPlayer performer, IActionConfig config,
			// ResourceType resourcesToUse,
			Action applyAction)
		{
			performer.UseOxygen(config.Oxygen);
			// TODO: figure out how to combine resourcesToUse with the required resources from config

			// config.Resources.Select(d => new ResourceData(resourcesToUse, ))
			// performer.UseResources(config.Resources);
			applyAction();
			return ActionResult.Success;
		}

		private bool EnoughOxygen(ITurnPerformer performer, IActionConfig config)
		{
			return performer.Oxygen >= config.Oxygen;
		}

		private bool EnoughResources(IResourceHolder resourceHolder, IEnumerable<ResourceCostData> resources)
		{
			return resources.All(costData =>
				costData.Relation == ResourceRelation.Same
					? costData.Type.AsCollection().Any(r => resourceHolder.HasResource(r, costData.Amount))
					: resourceHolder.ResourcesCount >= costData.Amount);
		}
	}
}