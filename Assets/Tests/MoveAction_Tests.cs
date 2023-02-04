using System;
using System.Collections.Generic;
using Core;
using Core.Configs.Actions;
using Core.Implementation;
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
using UnityEngine;

namespace Tests
{
	public class MoveAction_Tests
	{
		#region Mock classes

		private class MockMovable : IMovable
		{
			public event Action<IPlaceable> NeighborAdded;
			public event Action<IPlaceable> NeighborRemoved;
			public IReactiveProperty<Vector2Int> Position { get; } = new ReactiveProperty<Vector2Int>();
			public void OnNewNeighbor(IPlaceable neighbor)
			{
			}

			public void OnNeighborRemoved(IPlaceable neighbor)
			{
			}
		}

		private class MockMovableActor : IMovable, IActor
		{
			public MockMovableActor(Faction faction)
			{
				Faction = faction;
			}

			public Faction Faction { get; }
			public event Action<IPlaceable> NeighborAdded;
			public event Action<IPlaceable> NeighborRemoved;
			public IReactiveProperty<Vector2Int> Position { get; } = new ReactiveProperty<Vector2Int>();
			public void OnNewNeighbor(IPlaceable neighbor)
			{
			}

			public void OnNeighborRemoved(IPlaceable neighbor)
			{
			}
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
		public void MoveAction_ReturnsCellIsOccupied_WhenCannotMoveToOccupiedCellAndCellIsOccupiedByBaseBuildingOfOtherFaction()
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
		public void MoveAction_ReturnsCellIsOccupied_WhenCannotMoveToOccupiedCellAndCellIsOccupiedByBuildingOfOtherFaction()
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
		public void MoveAction_RestoresHealth_WhenDamageableMovedFromCellWithBuilding()
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
			moveAction.Perform(false);

			moveAction.From = to;
			moveAction.To = new Cell(2, 0);

			Assert.AreEqual(ActionResult.Success, moveAction.Perform(false));
			Assert.AreEqual(3, unit.Health.Value);
			Assert.AreEqual(4, building.Health.Value);
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

			Assert.AreNotEqual(ActionResult.Success, moveAction.CheckStartCell());
			Assert.AreNotEqual(ActionResult.Success, moveAction.CheckDestinationCell());
		}
	}
}