using System;
using System.Collections.Generic;
using Core.Configs.Actions;
using Core.Models.Actions;
using Core.Models.Actions.Implementation;
using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using Core.Models.GameProcess.Implementation;
using Core.Utils;
using NUnit.Framework;

namespace Tests
{
	public class MoveAction_Tests
	{
		#region Mock classes

		private class MockMovable : IMovable
		{
			public event Action<ICell> CellChanged;
			public void ChangeCell(ICell cell)
			{
			}
		}

		private class MockMovableActor : IMovable, IActor
		{
			public MockMovableActor(Faction faction)
			{
				Faction = faction;
			}
			public event Action<ICell> CellChanged;
			public void ChangeCell(ICell cell)
			{
			}

			public Faction Faction { get; }
		}

		private class MockMoveConfig : IMoveConfig
		{
			public ActionType Type { get; set; }
			public string DisplayName { get; set; }
			public ActionRepeatability Repeatability { get; set; }
			public byte Oxygen { get; set; }
			public ResourceCostData[] Resources { get; set; }
			public int MoveRange { get; set; }
			public bool CanMoveToOccupiedCell { get; set; }
			public bool CanMoveToDamagedBuilding { get; set; }
		}

		#endregion

		[Test]
		public void MoveAction_ReturnsPerformerNotSet()
		{
			IMoveAction moveAction = new MoveAction(new MockMoveConfig { Oxygen = 5 }, 6);
			moveAction.Performer = null;

			Assert.AreEqual(ActionResult.PerformerNotSet, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNotSelected()
		{
			IMoveAction moveAction = new MoveAction(new MockMoveConfig { Oxygen = 5 }, 6);
			moveAction.Selected.Value = false;
			moveAction.Performer = new Player(Faction.Red);

			Assert.AreEqual(ActionResult.NotSelected, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNotEnoughOxygen()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 2;
			IMoveAction moveAction = new MoveAction(new MockMoveConfig { Oxygen = 5 }, 6);
			moveAction.Performer = performer;
			moveAction.Selected.Value = true;

			Assert.AreEqual(ActionResult.NotEnoughOxygen, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNotEnoughResources()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 5,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore | ResourceType.Plants
					}
				}
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;

			Assert.AreEqual(ActionResult.NotEnoughResources, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNotEnoughResources_WhenResourcesDontComplyWithConfig()
		{
			var performer = new Player(Faction.Red);
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				}
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = performer;
			moveAction.Selected.Value = true;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);

			Assert.AreEqual(ActionResult.NotEnoughResources, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNoMovableInCell()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ }
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			moveAction.From = new Cell(0, 0);

			Assert.AreEqual(ActionResult.NoMovableInCell, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNoMovableActorOfCorrectFactionInCell_WhenUnitOfOtherFaction()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ }
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Yellow));
			moveAction.From = from;

			Assert.AreEqual(ActionResult.NoMovableActorOfCorrectFactionInCell, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsCellIsOccupied_WhenCannotMoveToOccupiedCellAndCellIsOccupied()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = false
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(0, 0);
			to.AddPlaceable(new MockMovableActor(Faction.Yellow));
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.CellIsOccupied, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsCellIsOccupied_WhenCannotMoveToOccupiedCellAndCellIsOccupiedBySameFaction()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = false
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(0, 0);
			to.AddPlaceable(new MockMovableActor(Faction.Red));
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.CellIsOccupied, moveAction.Perform(false));
		}

		[Test]
		public void
			MoveAction_ReturnsCellIsOccupied_WhenCannotMoveToOccupiedCellAndCellIsOccupiedByBaseBuildingOfOtherFaction()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = false
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(0, 0);
			to.AddPlaceable(new BaseBuilding(Faction.Yellow));
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.CellIsOccupied, moveAction.Perform(false));
		}

		[Test]
		public void
			MoveAction_ReturnsCellIsOccupied_WhenCannotMoveToOccupiedCellAndCellIsOccupiedByBuildingOfOtherFaction()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = false
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(0, 0);
			to.AddPlaceable(new Building(Faction.Yellow, ResourceType.Ore, 4));
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.CellIsOccupied, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsCellHasDamagedBuilding_WhenCannotMoveToDamagedBuildingAndCellHasDamagedBuilding()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = false
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(0, 0);
			var building = new Building(Faction.Yellow, ResourceType.Ore, 4);
			building.Health.Value -= 2;
			to.AddPlaceable(building);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.CellHasDamagedBuilding, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsExceedsRange_WhenCellsAreFarApart()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(2, 0);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.ExceedsRange, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsSameCell_WhenCellsHaveSamePosition()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(2, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(2, 0);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.SameCell, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsSameCell_WhenCellsAreSame()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var cell = new Cell(2, 0);
			cell.AddPlaceable(new MockMovableActor(Faction.Red));
			moveAction.From = cell;
			moveAction.To = cell;

			Assert.AreEqual(ActionResult.SameCell, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNoResourcesProvided_WhenNoResourcesProvided()
		{
			var performer = new Player(Faction.Red);
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 4,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = performer;
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(1, 0);
			moveAction.From = from;
			moveAction.To = to;
			performer.AddResources(new ResourcePackage(ResourceType.Ore, 10));

			Assert.AreEqual(ActionResult.NoResourcesProvided, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_ReturnsNoResourcesProvided_WhenResourcesDontComplyWithConfig()
		{
			var performer = new Player(Faction.Red);
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				},
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = performer;
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			from.AddPlaceable(new MockMovableActor(Faction.Red));
			var to = new Cell(1, 0);
			moveAction.From = from;
			moveAction.To = to;
			performer.AddResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 5 },
				{ ResourceType.Electricity, 5 },
			}));
			moveAction.Resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 },
				{ ResourceType.Electricity, 5 },
			});

			Assert.AreEqual(ActionResult.NoResourcesProvided, moveAction.Perform(false));
		}

		[Test]
		public void MoveAction_MovesUnit()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			var unit = new MockMovableActor(Faction.Red);
			from.AddPlaceable(unit);
			var to = new Cell(1, 0);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.Success, moveAction.Perform(false));
			Assert.IsFalse(from.TryGetPlaceable(out IMovable _));
			Assert.IsTrue(to.TryGetPlaceable(out IMovable movedUnit));
			Assert.AreEqual(unit, movedUnit);
		}

		[Test]
		public void MoveAction_DoesNotCombineHealth_WhenNotDamageableMovedToCellWithBuilding()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1,
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			var unit = new MockMovableActor(Faction.Red);
			from.AddPlaceable(unit);
			var to = new Cell(1, 0);
			var building = new Building(Faction.Red, ResourceType.Ore, 4);
			to.AddPlaceable(building);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.Success, moveAction.Perform(false));
			Assert.AreEqual(4, building.Health.Value);
		}

		[Test]
		public void MoveAction_CombinesHealth_WhenDamageableMovedToCellWithBuilding()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1,
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = new Player(Faction.Red);
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			from.AddPlaceable(unit);
			var to = new Cell(1, 0);
			var building = new Building(Faction.Red, ResourceType.Ore, 4);
			to.AddPlaceable(building);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.Success, moveAction.Perform(false));
			Assert.AreEqual(6, unit.Health.Value);
			Assert.AreEqual(6, building.Health.Value);
		}

		[Test]
		public void MoveAction_UsesOxygen_WhenRepeatAndActionNotRepeatable()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockMoveConfig
			{
				Repeatability = ActionRepeatability.None,
				Oxygen = 1,
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1,
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = performer;
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			from.AddPlaceable(unit);
			var to = new Cell(1, 0);
			var building = new Building(Faction.Red, ResourceType.Ore, 4);
			to.AddPlaceable(building);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.Success, moveAction.Perform(true));
			Assert.AreEqual(4, performer.Oxygen.Value);
		}

		[Test]
		public void MoveAction_DoesNotUseOxygen_WhenRepeatAndActionRepeatable()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockMoveConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
					{ },
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1,
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = performer;
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			from.AddPlaceable(unit);
			var to = new Cell(1, 0);
			var building = new Building(Faction.Red, ResourceType.Ore, 4);
			to.AddPlaceable(building);
			moveAction.From = from;
			moveAction.To = to;

			Assert.AreEqual(ActionResult.Success, moveAction.Perform(true));
			Assert.AreEqual(5, performer.Oxygen.Value);
		}

		[Test]
		public void Selected_ResetsActionState_WhenSetToFalse()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockMoveConfig
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
				CanMoveToOccupiedCell = true,
				CanMoveToDamagedBuilding = true,
				MoveRange = 1,
			};
			IMoveAction moveAction = new MoveAction(config, 6);
			moveAction.Performer = performer;
			moveAction.Selected.Value = true;
			var from = new Cell(0, 0);
			var unit = new Unit(Faction.Red, 3);
			from.AddPlaceable(unit);
			var to = new Cell(1, 0);
			var building = new Building(Faction.Red, ResourceType.Ore, 4);
			to.AddPlaceable(building);
			moveAction.From = from;
			moveAction.To = to;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);

			moveAction.Selected.Value = false;

			Assert.AreNotEqual(ActionResult.Success, moveAction.CheckProvidedResources());
			Assert.AreNotEqual(ActionResult.Success, moveAction.CheckStartCell());
			Assert.AreNotEqual(ActionResult.Success, moveAction.CheckDestinationCell());
		}

		[Test]
		public void ResourcesRequired_ReturnsTrue_WhenResourcesInConfigPresent()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				}
			};

			IMoveAction moveAction = new MoveAction(config, 6);

			Assert.IsTrue(moveAction.ResourcesRequired);
		}

		[Test]
		public void ResourcesRequired_ReturnsFalse_WhenNoResourcesInConfig()
		{
			var config = new MockMoveConfig
			{
				Resources = new ResourceCostData[]
				{ }
			};

			IMoveAction moveAction = new MoveAction(config, 6);

			Assert.IsFalse(moveAction.ResourcesRequired);
		}
	}
}