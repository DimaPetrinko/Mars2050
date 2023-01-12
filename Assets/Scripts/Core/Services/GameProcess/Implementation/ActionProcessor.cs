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
		NoResourcesProvided,
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

		public ActionResult Move(
			IPlayer performer,
			ICell from,
			ICell to,
			Dictionary<ResourceType, int> resourcesToUse,
			IMoveConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!from.TryGetActor(out IUnit movable, performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (!config.CanMoveToOccupiedCell && to.HasPlaceable<IMovable>()) return ActionResult.CellIsOccupied;
			if (!config.CanMoveToDamagedBuilding
			    && to.TryGetPlaceable(out IBuilding building)
			    && building.Health.Value < building.MaxHealth.Value)
				return ActionResult.CellHasDamagedBuilding;
			if ((from.Position - to.Position).magnitude > config.MoveRange) return ActionResult.ExceedsRange;
			if (resourcesToUse.Count == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Count == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				from.RemovePlaceable(movable);
				to.AddPlaceable(movable);
			});

			return ActionResult.Success;
		}

		public ActionResult Discover(
			IPlayer performer,
			ICell cell,
			Dictionary<ResourceType, int> resourcesToUse,
			IDiscoverConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!cell.HasActor<IUnit>(performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (!cell.TryGetPlaceable(out IResource resource)) return ActionResult.NoResourceInCell;
			if (resource.IsDiscovered.Value) return ActionResult.ResourceAlreadyDiscovered;
			if (resourcesToUse.Count == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Count == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				resource.IsDiscovered.Value = true;
			});

			return ActionResult.Success;
		}

		public ActionResult Gather(
			IPlayer performer,
			IEnumerable<ICell> cells,
			Dictionary<ResourceType, int> resourcesToUse,
			int roll,
			IGatherConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (resourcesToUse.Count == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Count == 0) return ActionResult.NoResourcesProvided;
			var resources = config.GetResourcesForRoll(roll).AsCollection().ToList();
			var resourcesToGather = cells
				.Where(c => c.HasPlaceable<IResource>() && c.HasActor<IResourceGatherer>(performer.Faction))
				.Select(c => c.GetPlaceable<IResource>())
				.Where(r => r.IsDiscovered.Value)
				.Where(r => resources.Contains(r.Type))
				.Select(r => r.Type)
				.GroupBy(r => r)
				.ToDictionary(g => g.Key, g => g.Count());

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				performer.AddResources(resourcesToGather);
			});

			return ActionResult.Success;
		}

		public ActionResult Build(
			IPlayer performer,
			ICell cell,
			Dictionary<ResourceType, int> resourcesToUse,
			IBuildConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!cell.TryGetPlaceable(out IResource resource)) return ActionResult.NoResourceInCell;
			var buildingConfig = config.BuildingConfigs.GetConfig(resource.Type);
			var cost = config.Resources.Concat(buildingConfig.BuildCost);
			if (!EnoughResources(performer, cost)) return ActionResult.NotEnoughResources;
			if (!cell.HasActor<IUnit>(performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (cell.HasActor<IBuilding>()) return ActionResult.CellIsOccupiedByBuilding;
			if (resourcesToUse.Count == 0) return ActionResult.NoResourcesProvided;
			var resourceProcessor = new ResourceProcessor();
			var (toUse, left) = resourceProcessor.Process(resourcesToUse, config.Resources);
			var (buildCost, _) = resourceProcessor.Process(left, buildingConfig.BuildCost.ToArray());
			if (toUse.Count == 0) return ActionResult.NoResourcesProvided;
			if (buildCost.Count == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				performer.UseResources(buildCost);
				var building = mBuildingFactory.Create(buildingConfig);
				cell.AddPlaceable(building);
			});

			return ActionResult.Success;
		}

		public ActionResult Attack(
			IPlayer performer,
			ICell from,
			ICell to,
			Dictionary<ResourceType, int> resourcesToUse,
			int roll,
			bool repeat,
			IAttackConfig config)
		{
			if (!(repeat && config.Repeatable) && !EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!from.HasActor<IUnit>(performer.Faction)) return ActionResult.NoUnitOfCorrectFactionInCell;
			if (!to.TryGetActor(out IDamageable other) && ((IActor)other).Faction == performer.Faction)
				return ActionResult.NoUnitOfCorrectFactionInCell;
			if ((from.Position - to.Position).magnitude > config.AttackRange) return ActionResult.ExceedsRange;
			if (resourcesToUse.Count == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Count == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				other.Damage(config.GetDamageForRoll(roll));
			});

			return ActionResult.Success;
		}

		public ActionResult Heal(
			IPlayer performer,
			ICell cell,
			Dictionary<ResourceType, int> resourcesToUse,
			bool repeat,
			IHealConfig config)
		{
			if (!(repeat && config.Repeatable) && !EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!cell.TryGetActor(out IDamageable damageable) && ((IActor)damageable).Faction != performer.Faction)
				return ActionResult.NoUnitOfCorrectFactionInCell;
			if (resourcesToUse.Count == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Count == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				damageable.Heal(config.Amount);
			});

			return ActionResult.Success;
		}

		public ActionResult Trade(
			IPlayer performer,
			Dictionary<ResourceType, int> resourcesToSell,
			Dictionary<ResourceType, int> resourcesToBuy,
			bool repeat,
			ITradeConfig config
			)
		{
			if (!(repeat && config.Repeatable) && !EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (resourcesToSell.Count == 0) return ActionResult.NoResourcesProvided;
			if (resourcesToBuy.Count == 0) return ActionResult.NoResourcesProvided;
			var processor = new ResourceProcessor();
			var (toSell, _) = processor.Process(resourcesToSell, config.Resources);
			var (toBuy, _) = processor.Process(resourcesToBuy, new[] { config.PurchaseResource });

			if (toSell.Count == 0) return ActionResult.NoResourcesProvided;
			if (toBuy.Count == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toSell, () =>
			{
				performer.AddResources(toBuy);
			});

			return ActionResult.Success;
		}

		public ActionResult PlayerTrade(
			IPlayer performer,
			IResourceHolder other,
			Dictionary<ResourceType, int> resourcesToSell,
			Dictionary<ResourceType, int> resourcesToBuy,
			bool repeat,
			ITradeConfig config)
		{
			if (!(repeat && config.Repeatable) && !EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!EnoughResources(other, resourcesToBuy)) return ActionResult.NotEnoughResources;
			if (resourcesToSell.Count == 0) return ActionResult.NoResourcesProvided;
			if (resourcesToBuy.Count == 0) return ActionResult.NoResourcesProvided;
			var processor = new ResourceProcessor();
			var (toSell, _) = processor.Process(resourcesToSell, config.Resources);
			var (toBuy, _) = processor.Process(resourcesToBuy, new[] { config.PurchaseResource });

			if (toSell.Count == 0) return ActionResult.NoResourcesProvided;
			if (toBuy.Count == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toSell, () =>
			{
				other.AddResources(toSell);

				performer.AddResources(toBuy);
				other.UseResources(toBuy);
			});

			return ActionResult.Success;
		}

		private void ApplyAction(
			IPlayer performer,
			int oxygen,
			Dictionary<ResourceType, int> resourcesToUse,
			Action applyAction)
		{
			performer.UseOxygen(oxygen);
			performer.UseResources(resourcesToUse);
			applyAction();
		}

		private bool EnoughOxygen(ITurnPerformer performer, IActionConfig config)
		{
			return performer.Oxygen >= config.Oxygen;
		}

		// TODO: may be replaced with resource processor
		private bool EnoughResources(IResourceHolder resourceHolder, IEnumerable<ResourceCostData> resources)
		{
			return resources.All(costData =>
				costData.Relation == ResourceRelation.Same
					? costData.Type.AsCollection().Any(r => resourceHolder.HasResource(r, costData.Amount))
					: resourceHolder.ResourcesCount >= costData.Amount);
		}

		private bool EnoughResources(IResourceHolder resourceHolder, IDictionary<ResourceType, int> resources)
		{
			return resources.All(r => resourceHolder.HasResource(r.Key, r.Value));
		}
	}
}