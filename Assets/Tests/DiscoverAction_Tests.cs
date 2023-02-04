using System.Collections.Generic;
using Core.Configs.Actions;
using Core.Models.Actions;
using Core.Models.Actions.Implementation;
using Core.Models.Actors.Implementation;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using Core.Models.GameProcess.Implementation;
using Core.Utils;
using NUnit.Framework;

namespace Tests
{
	public class DiscoverAction_Tests
	{
		#region Mock classes

		class MockDiscoverConfig : IDiscoverConfig
		{
			public ActionType Type { get; set; }
			public string DisplayName { get; set; }
			public ActionRepeatability Repeatability { get; set; }
			public byte Oxygen { get; set; }
			public ResourceCostData[] Resources { get; set; }
		}

		#endregion

		// NoMovableActorOfCorrectFactionInCell;
		// NoResourceInCell;
		// ResourceAlreadyDiscovered;

		[Test]
		public void CheckTargetCell_ReturnsNoMovableActorOfCorrectFactionInCell_WhenNoDiscovererInCell()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockDiscoverConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
			};
			IDiscoverAction discoverAction = new DiscoverAction(config);
			discoverAction.Performer = performer;
			discoverAction.Selected.Value = true;
			var cell = new Cell(0, 0);
			var resource = new Resource(ResourceType.Ore);
			cell.AddPlaceable(resource);
			discoverAction.Cell = cell;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);
			discoverAction.Resources = resources;

			Assert.AreEqual(ActionResult.NoMovableActorOfCorrectFactionInCell, discoverAction.CheckTargetCell());
		}

		[Test]
		public void CheckTargetCell_ReturnsNoMovableActorOfCorrectFactionInCell_WhenDiscovererOfDifferentFaction()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockDiscoverConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
			};
			IDiscoverAction discoverAction = new DiscoverAction(config);
			discoverAction.Performer = performer;
			discoverAction.Selected.Value = true;
			var cell = new Cell(0, 0);
			var unit = new Unit(Faction.Blue, 3);
			var resource = new Resource(ResourceType.Ore);
			cell.AddPlaceable(unit);
			cell.AddPlaceable(resource);
			discoverAction.Cell = cell;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);
			discoverAction.Resources = resources;

			Assert.AreEqual(ActionResult.NoMovableActorOfCorrectFactionInCell, discoverAction.CheckTargetCell());
		}

		[Test]
		public void CheckTargetCell_ReturnsNoResourceInCell_WhenNoResourceInCell()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockDiscoverConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
			};
			IDiscoverAction discoverAction = new DiscoverAction(config);
			discoverAction.Performer = performer;
			discoverAction.Selected.Value = true;
			var cell = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			cell.AddPlaceable(unit);
			discoverAction.Cell = cell;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);
			discoverAction.Resources = resources;

			Assert.AreEqual(ActionResult.NoResourceInCell, discoverAction.CheckTargetCell());
		}

		[Test]
		public void CheckTargetCell_ReturnsResourceAlreadyDiscovered_WhenAlreadyDiscovered()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockDiscoverConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
			};
			IDiscoverAction discoverAction = new DiscoverAction(config);
			discoverAction.Performer = performer;
			discoverAction.Selected.Value = true;
			var cell = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			var resource = new Resource(ResourceType.Ore);
			resource.IsDiscovered.Value = true;
			cell.AddPlaceable(unit);
			cell.AddPlaceable(resource);
			discoverAction.Cell = cell;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);
			discoverAction.Resources = resources;

			Assert.AreEqual(ActionResult.ResourceAlreadyDiscovered, discoverAction.CheckTargetCell());
		}

		[Test]
		public void DiscoverAction_DiscoversResource()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockDiscoverConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
			};
			IDiscoverAction discoverAction = new DiscoverAction(config);
			discoverAction.Performer = performer;
			discoverAction.Selected.Value = true;
			var cell = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			var resource = new Resource(ResourceType.Ore);
			cell.AddPlaceable(unit);
			cell.AddPlaceable(resource);
			discoverAction.Cell = cell;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);
			discoverAction.Resources = resources;

			Assert.AreEqual(ActionResult.Success, discoverAction.Perform(false));
			Assert.IsTrue(resource.IsDiscovered.Value);
		}

		[Test]
		public void Selected_ResetsActionState_WhenSetToFalse()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockDiscoverConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
			};
			IDiscoverAction discoverAction = new DiscoverAction(config);
			discoverAction.Performer = performer;
			discoverAction.Selected.Value = true;
			var cell = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			var resource = new Resource(ResourceType.Ore);
			cell.AddPlaceable(unit);
			cell.AddPlaceable(resource);
			discoverAction.Cell = cell;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);

			discoverAction.Selected.Value = false;

			Assert.AreNotEqual(ActionResult.Success, discoverAction.CheckTargetCell());
		}
	}
}