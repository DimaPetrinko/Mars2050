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

// TODO: turn each action into a model
// TODO: create action presenter and view
// TODO: action view would contain ui to gather necessary data (resources, players) to perform the action
// TODO: action presenter would communicate with the board to get clicks from cells
namespace Core.Services.GameProcess.Implementation
{
	public class ActionProcessor : IActionProcessor
	{
		private IFactory<IBuilding, Faction, IBuildingConfig> mBuildingFactory;

		// public ActionResult Move(
		// 	IPlayer performer,
		// 	ICell from,
		// 	ICell to,
		// 	ResourcePackage resourcesToUse,
		// 	int combinedHealth,
		// 	IMoveConfig config)
		// {
		// 	if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
		// 	if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
		// 	if (!from.TryGetActor(out IUnit movable, performer.Faction)) return ActionResult.NoMovableActorOfCorrectFactionInCell;
		// 	if (!config.CanMoveToOccupiedCell
		// 	    && to.HasPlaceable<IMovable>()
		// 	    && !to.HasPlaceable<IBaseBuilding>())
		// 		return ActionResult.CellIsOccupied;
		// 	if (!config.CanMoveToDamagedBuilding
		// 	    && to.TryGetPlaceable(out IBuilding building)
		// 	    && building.Health.Value < building.MaxHealth.Value)
		// 		return ActionResult.CellHasDamagedBuilding;
		// 	if ((from.Position - to.Position).magnitude > config.MoveRange) return ActionResult.ExceedsRange;
		// 	if (resourcesToUse.Amount == 0) return ActionResult.NoResourcesProvided;
		// 	var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
		// 	if (toUse.Amount == 0) return ActionResult.NoResourcesProvided;
		//
		// 	ApplyAction(performer, config.Oxygen, toUse, () =>
		// 	{
		// 		from.RemovePlaceable(movable);
		// 		to.AddPlaceable(movable);
		//
		// 		if (from.TryGetPlaceable(out IBuilding fromBuilding)) fromBuilding.RestoreMaxHealth();
		// 		if (to.TryGetPlaceable(out IBuilding toBuilding))
		// 		{
		// 			toBuilding.MaxHealth.Value = combinedHealth;
		// 			movable.MaxHealth.Value = combinedHealth;
		// 		}
		// 		else movable.RestoreMaxHealth();
		// 	});
		//
		// 	return ActionResult.Success;
		// }

		public ActionResult Discover(
			IPlayer performer,
			ICell cell,
			ResourcePackage resourcesToUse,
			IDiscoverConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!cell.HasActor<IUnit>(performer.Faction)) return ActionResult.NoMovableActorOfCorrectFactionInCell;
			if (!cell.TryGetPlaceable(out IResource resource)) return ActionResult.NoResourceInCell;
			if (resource.IsDiscovered.Value) return ActionResult.ResourceAlreadyDiscovered;
			if (resourcesToUse.Amount == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Amount == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				resource.IsDiscovered.Value = true;
			});

			return ActionResult.Success;
		}

		public ActionResult Gather(
			IPlayer performer,
			IEnumerable<ICell> cells,
			ResourcePackage resourcesToUse,
			int roll,
			IGatherConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (resourcesToUse.Amount == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Amount == 0) return ActionResult.NoResourcesProvided;
			var resources = config.GetResourcesForRoll(roll).AsCollection().ToList();
			var resourcesToGather = new ResourcePackage(cells
				.Where(c => c.HasPlaceable<IResource>() && c.HasActor<IResourceGatherer>(performer.Faction))
				.Select(c => c.GetPlaceable<IResource>())
				.Where(r => r.IsDiscovered.Value)
				.Where(r => resources.Contains(r.Type))
				.Select(r => r.Type)
				.GroupBy(r => r)
				.ToDictionary(g => g.Key, g => g.Count()));

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				performer.AddResources(resourcesToGather);
			});

			return ActionResult.Success;
		}

		public ActionResult Build(
			IPlayer performer,
			ICell cell,
			ResourcePackage resourcesToUse,
			IBuildConfig config)
		{
			if (!EnoughOxygen(performer, config)) return ActionResult.NotEnoughOxygen;
			if (!cell.TryGetPlaceable(out IResource resource)) return ActionResult.NoResourceInCell;
			var buildingConfig = config.BuildingConfigs.GetConfig(resource.Type);
			var cost = config.Resources.Concat(buildingConfig.BuildCost);
			if (!EnoughResources(performer, cost)) return ActionResult.NotEnoughResources;
			if (!cell.HasActor<IUnit>(performer.Faction)) return ActionResult.NoMovableActorOfCorrectFactionInCell;
			if (cell.HasActor<IBuilding>()) return ActionResult.CellIsOccupied;
			if (resourcesToUse.Amount == 0) return ActionResult.NoResourcesProvided;
			var resourceProcessor = new ResourceProcessor();
			var (toUse, left) = resourceProcessor.Process(resourcesToUse, config.Resources);
			var (buildCost, _) = resourceProcessor.Process(left, buildingConfig.BuildCost.ToArray());
			if (toUse.Amount == 0) return ActionResult.NoResourcesProvided;
			if (buildCost.Amount == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				performer.UseResources(buildCost);
				var building = mBuildingFactory.Create(performer.Faction, buildingConfig);
				cell.AddPlaceable(building);
			});

			return ActionResult.Success;
		}

		public ActionResult Attack(
			IPlayer performer,
			ICell from,
			ICell to,
			ResourcePackage resourcesToUse,
			int roll,
			bool repeat,
			IAttackConfig config)
		{
			if (!(repeat && config.Repeatability == ActionRepeatability.Repeatable) && !EnoughOxygen(performer, config))
				return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!from.HasActor<IUnit>(performer.Faction)) return ActionResult.NoMovableActorOfCorrectFactionInCell;
			if (!to.TryGetActor(out IDamageable other) && ((IActor)other).Faction == performer.Faction)
				return ActionResult.NoMovableActorOfCorrectFactionInCell;
			if ((from.Position - to.Position).magnitude > config.AttackRange) return ActionResult.ExceedsRange;
			if (resourcesToUse.Amount == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Amount == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				other.Damage(config.GetDamageForRoll(roll));
			});

			return ActionResult.Success;
		}

		public ActionResult Heal(
			IPlayer performer,
			ICell cell,
			ResourcePackage resourcesToUse,
			bool repeat,
			IHealConfig config)
		{
			if (!(repeat && config.Repeatability == ActionRepeatability.Repeatable) && !EnoughOxygen(performer, config))
				return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!cell.TryGetActor(out IDamageable damageable) && ((IActor)damageable).Faction != performer.Faction)
				return ActionResult.NoMovableActorOfCorrectFactionInCell;
			if (resourcesToUse.Amount == 0) return ActionResult.NoResourcesProvided;
			var (toUse, _) = new ResourceProcessor().Process(resourcesToUse, config.Resources);
			if (toUse.Amount == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toUse, () =>
			{
				damageable.Heal(config.Amount);
			});

			return ActionResult.Success;
		}

		public ActionResult Trade(
			IPlayer performer,
			ResourcePackage resourcesToSell,
			ResourcePackage resourcesToBuy,
			bool repeat,
			ITradeConfig config
			)
		{
			if (!(repeat && config.Repeatability == ActionRepeatability.Repeatable) && !EnoughOxygen(performer, config))
				return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (resourcesToSell.Amount == 0) return ActionResult.NoResourcesProvided;
			if (resourcesToBuy.Amount == 0) return ActionResult.NoResourcesProvided;
			var processor = new ResourceProcessor();
			var (toSell, _) = processor.Process(resourcesToSell, config.Resources);
			var (toBuy, _) = processor.Process(resourcesToBuy, new[] { config.PurchaseResource });

			if (toSell.Amount == 0) return ActionResult.NoResourcesProvided;
			if (toBuy.Amount == 0) return ActionResult.NoResourcesProvided;

			ApplyAction(performer, config.Oxygen, toSell, () =>
			{
				performer.AddResources(toBuy);
			});

			return ActionResult.Success;
		}

		public ActionResult PlayerTrade(
			IPlayer performer,
			IResourceHolder other,
			ResourcePackage resourcesToSell,
			ResourcePackage resourcesToBuy,
			bool repeat,
			ITradeConfig config)
		{
			if (!(repeat && config.Repeatability == ActionRepeatability.Repeatable) && !EnoughOxygen(performer, config))
				return ActionResult.NotEnoughOxygen;
			if (!EnoughResources(performer, config.Resources)) return ActionResult.NotEnoughResources;
			if (!EnoughResources(other, resourcesToBuy)) return ActionResult.NotEnoughResources;
			if (resourcesToSell.Amount == 0) return ActionResult.NoResourcesProvided;
			if (resourcesToBuy.Amount == 0) return ActionResult.NoResourcesProvided;
			var processor = new ResourceProcessor();
			var (toSell, _) = processor.Process(resourcesToSell, config.Resources);
			var (toBuy, _) = processor.Process(resourcesToBuy, new[] { config.PurchaseResource });

			if (toSell.Amount == 0) return ActionResult.NoResourcesProvided;
			if (toBuy.Amount == 0) return ActionResult.NoResourcesProvided;

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
			byte oxygen,
			ResourcePackage resourcesToUse,
			Action applyAction)
		{
			performer.Oxygen.Value -= oxygen;
			performer.UseResources(resourcesToUse);
			applyAction();
		}

		private bool EnoughOxygen(ITurnPerformer performer, IActionConfig config)
		{
			return performer.Oxygen.Value >= config.Oxygen;
		}

		// TODO: may be replaced with resource processor
		private bool EnoughResources(IResourceHolder resourceHolder, IEnumerable<ResourceCostData> resources)
		{
			return resources.All(costData =>
				costData.Relation == ResourceRelation.Same
					? costData.Type.AsCollection().Any(r => resourceHolder.HasResource(r, costData.Amount))
					: resourceHolder.ResourcesAmount >= costData.Amount);
		}

		private bool EnoughResources(IResourceHolder resourceHolder, ResourcePackage resources)
		{
			return resources.Content.All(r => resourceHolder.HasResource(r.Key, r.Value));
		}
	}
}