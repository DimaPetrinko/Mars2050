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
	public class Unit_tests
	{
		private IUnit mUnit;

		#region Mock classes

		private class MockBuilding : IBuilding
		{
			private Faction mFaction;
			private IReactiveProperty<Faction> mFaction1;
			public event Action<ICell> CellChanged;
			public void ChangeCell(ICell cell)
			{
			}

			Faction IActor.Faction => mFaction;

			public ResourceType ResourceType { get; }
			public event Action Died;
			public IReactiveProperty<int> Health { get; } = new ReactiveProperty<int>();
			public IReactiveProperty<int> MaxHealth { get; } = new ReactiveProperty<int>();
			public void Damage(int damage)
			{
				throw new NotImplementedException();
			}

			public void Heal(int amount)
			{
				throw new NotImplementedException();
			}

			public void RestoreMaxHealth()
			{
			}

			IReactiveProperty<Faction> IBuilding.Faction => mFaction1;
		}

		#endregion

		[SetUp]
		public void SetUp()
		{
			mUnit = new Unit(Faction.Red, 3, 6);
		}

		[Test]
		public void Faction_IsSet()
		{
			Assert.AreEqual(Faction.Red, mUnit.Faction);
		}

		// TODO: should be done in action processor
		[Test]
		public void MaxHealth_IsSetToCombinedValue_WhenMovedToACellWithBuilding()
		{
			ICell cell = new Cell(0, 0);
			IBuilding building = new MockBuilding();

			cell.AddPlaceable(building);
			cell.AddPlaceable(mUnit);

			Assert.AreEqual(6, mUnit.MaxHealth.Value);
		}

		// TODO: should be done in action processor
		[Test]
		public void MaxHealth_IsSetToBaseValue_WhenMovedFromACellWithBuilding()
		{
			ICell cell = new Cell(0, 0);
			IBuilding building = new MockBuilding();

			cell.AddPlaceable(building);
			cell.AddPlaceable(mUnit);
			cell.RemovePlaceable(mUnit);

			Assert.AreEqual(3, mUnit.MaxHealth.Value);
		}
	}
}