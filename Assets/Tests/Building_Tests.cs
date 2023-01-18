using System;
using Core;
using Core.Implementation;
using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using NUnit.Framework;

namespace Tests
{
	public class Building_Tests
	{

		#region Mock classes

		private class MockResource : IResource
		{
			public event Action<ICell> CellChanged;
			public void ChangeCell(ICell cell)
			{
			}

			public IReactiveProperty<bool> IsOccupied { get; } = new ReactiveProperty<bool>();
			public IReactiveProperty<bool> IsDiscovered { get; }
			public ResourceType Type { get; }
		}

		#endregion

		private IBuilding mBuilding;

		[SetUp]
		public void SetUp()
		{
			mBuilding = new Building(Faction.Yellow, ResourceType.Ore, 4);
		}

		[Test]
		public void Faction_IsSet()
		{
			Assert.AreEqual(Faction.Yellow, mBuilding.Faction.Value);
		}

		[Test]
		public void ResourceType_IsSet()
		{
			Assert.AreEqual(ResourceType.Ore, mBuilding.ResourceType);
		}

		// TODO: should be done in action processor
		// [Test]
		// public void BuildingMaxHealth_IsSetToCombinedValue_WhenMovedToACellWithBuilding()
		// {
		// 	ICell cell = new Cell(0, 0);
		// 	IUnit unit = new Unit(Faction.Yellow, 3);
		//
		// 	cell.AddPlaceable(mBuilding);
		// 	cell.AddPlaceable(unit);
		//
		// 	Assert.AreEqual(6, mBuilding.MaxHealth.Value);
		// }
		//
		// TODO: should be done in action processor
		// [Test]
		// public void BuildingMaxHealth_IsSetToBaseValue_WhenMovedFromACellWithBuilding()
		// {
		// 	ICell cell = new Cell(0, 0);
		// 	IUnit unit = new Unit(Faction.Yellow, 3);
		//
		// 	cell.AddPlaceable(mBuilding);
		// 	cell.AddPlaceable(unit);
		// 	cell.RemovePlaceable(unit);
		//
		// 	Assert.AreEqual(4, mBuilding.MaxHealth.Value);
		// }
	}
}