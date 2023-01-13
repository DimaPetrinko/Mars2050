using System;
using Core;
using Core.Models.Actors;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	public class Cell_Tests
	{
		#region Mock classes

		private class MockPlaceable : IPlaceable
		{
			public event Action<ICell> CellChanged;

			public void ChangeCell(ICell cell)
			{
				CellChanged?.Invoke(cell);
			}
		}

		private class MockActor : IActor
		{
			public event Action<ICell> CellChanged;

			public Faction Faction { get; }

			public MockActor(Faction faction = Faction.Blue)
			{
				Faction = faction;
			}

			public void ChangeCell(ICell cell)
			{
				CellChanged?.Invoke(cell);
			}
		}

		private class MockUnit : IUnit
		{
			public event Action<ICell> CellChanged;
			public void ChangeCell(ICell cell)
			{
			}

			public Faction Faction { get; }
			public event Action Died;
			public IReactiveProperty<int> Health { get; }
			public IReactiveProperty<int> MaxHealth { get; }
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
				throw new NotImplementedException();
			}
		}

		#endregion

		private ICell mCell;

		[SetUp]
		public void SetUp()
		{
			mCell = new Cell(new Vector2Int());
		}

		[Test]
		public void Position_IsAssigned()
		{
			var position = new Vector2Int(4, 5);

			ICell cell = new Cell(position);

			Assert.AreEqual(position, cell.Position);
		}

		[Test]
		public void HasPlaceable_ReturnsFalse_WhenNoneAdded()
		{
			Assert.False(mCell.HasPlaceable());
		}

		[Test]
		public void HasPlaceable_ReturnsTrue_AfterPlaceableAdded()
		{
			mCell.AddPlaceable(new MockPlaceable());

			Assert.True(mCell.HasPlaceable());
		}

		[Test]
		public void HasPlaceableWithType_ReturnsTrue_WhenCorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.True(mCell.HasPlaceable<IActor>());
		}

		[Test]
		public void HasPlaceableWithType_ReturnsFalse_WhenIncorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.False(mCell.HasPlaceable<IResource>());
		}

		[Test]
		public void GetPlaceable_ReturnsCorrectPlaceable()
		{
			var mockPlaceable = new MockPlaceable();
			mCell.AddPlaceable(mockPlaceable);

			var result = mCell.GetPlaceable<IPlaceable>();
			Assert.AreEqual(mockPlaceable, result);
		}

		[Test]
		public void GetPlaceable_DoesNotReturnPlaceable_WhenNoneAdded()
		{
			var result = mCell.GetPlaceable<IPlaceable>();
			Assert.IsNull(result);
		}

		[Test]
		public void TryGetPlaceable_ReturnsTrue_WhenPlaceableAdded()
		{
			var mockPlaceable = new MockPlaceable();
			mCell.AddPlaceable(mockPlaceable);

			Assert.True(mCell.TryGetPlaceable(out IPlaceable result));
		}

		[Test]
		public void TryGetPlaceable_ReturnsFalse_WhenNoneAdded()
		{
			Assert.False(mCell.TryGetPlaceable(out IPlaceable result));
		}

		[Test]
		public void TryGetPlaceable_ResultIsCorrect_WhenPlaceableAdded()
		{
			var mockPlaceable = new MockPlaceable();
			mCell.AddPlaceable(mockPlaceable);

			mCell.TryGetPlaceable(out IPlaceable result);
			Assert.AreEqual(mockPlaceable, result);
		}

		[Test]
		public void TryGetPlaceable_ResultIsNull_WhenNoneAdded()
		{
			mCell.TryGetPlaceable(out IPlaceable result);
			Assert.IsNull(result);
		}

		[Test]
		public void GetLastNonUnitPlaceable_ReturnsLastAdded()
		{
			var lastAdded = new MockPlaceable();
			mCell.AddPlaceable(new MockPlaceable());
			mCell.AddPlaceable(lastAdded);

			Assert.AreEqual(lastAdded, mCell.GetLastNonUnitPlaceable());
		}

		[Test]
		public void GetLastNonUnitPlaceable_ReturnsLastAdded_WhichIsNotUnit()
		{
			var lastAdded = new MockPlaceable();
			mCell.AddPlaceable(new MockPlaceable());
			mCell.AddPlaceable(lastAdded);
			mCell.AddPlaceable(new MockUnit());

			Assert.AreEqual(lastAdded, mCell.GetLastNonUnitPlaceable());
		}

		[Test]
		public void GetLastNonUnitPlaceable_ReturnsNull_WhenNoneAdded()
		{
			Assert.AreEqual(null, mCell.GetLastNonUnitPlaceable());
		}

		[Test]
		public void GetLastNonUnitPlaceable_ReturnsNull_WhenUnitAdded()
		{
			mCell.AddPlaceable(new MockUnit());

			Assert.AreEqual(null, mCell.GetLastNonUnitPlaceable());
		}

		[Test]
		public void HasActor_ReturnsFalse_WhenNoneAdded()
		{
			Assert.False(mCell.HasActor());
		}

		[Test]
		public void HasActor_ReturnsFalse_WhenNonActorAdded()
		{
			mCell.AddPlaceable(new MockPlaceable());

			Assert.False(mCell.HasActor());
		}

		[Test]
		public void HasActor_ReturnsTrue_AfterPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.True(mCell.HasActor());
		}

		[Test]
		public void HasActorWithType_ReturnsTrue_WhenCorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.True(mCell.HasActor<IActor>());
		}

		[Test]
		public void HasActorWithType_ReturnsFalse_WhenIncorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.False(mCell.HasActor<IResource>());
		}

		[Test]
		public void HasActorWithType_ReturnsFalse_WhenNonActorProvided()
		{
			mCell.AddPlaceable(new MockPlaceable());

			Assert.False(mCell.HasActor<IActor>());
		}

		[Test]
		public void HasActorWithFaction_ReturnsTrue_WhenCorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.True(mCell.HasActor(Faction.Yellow));
		}

		[Test]
		public void HasActorWithFaction_ReturnsFalse_WhenIncorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.False(mCell.HasActor(Faction.Blue));
		}

		[Test]
		public void HasActorWithFactionAndType_ReturnsTrue_WhenCorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.True(mCell.HasActor<IActor>(Faction.Yellow));
		}

		[Test]
		public void HasActorWithFactionAndType_ReturnsFalse_WhenIncorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.False(mCell.HasActor<IActor>(Faction.Blue));
		}

		[Test]
		public void GetActor_ReturnsCorrectActor()
		{
			var mockActor = new MockActor();
			mCell.AddPlaceable(mockActor);

			var result = mCell.GetActor<IActor>();
			Assert.AreEqual(mockActor, result);
		}

		[Test]
		public void GetActor_DoesNotReturnActor_WhenNoneAdded()
		{
			var result = mCell.GetActor<IActor>();
			Assert.IsNull(result);
		}

		[Test]
		public void TryGetActor_ReturnsTrue_WhenActorAdded()
		{
			var mockActor = new MockActor();
			mCell.AddPlaceable(mockActor);

			Assert.True(mCell.TryGetActor(out IActor result));
		}

		[Test]
		public void TryGetActor_ReturnsFalse_WhenNoneAdded()
		{
			Assert.False(mCell.TryGetActor(out IActor result));
		}

		[Test]
		public void TryGetActor_ResultIsCorrect_WhenActorAdded()
		{
			var mockActor = new MockActor();
			mCell.AddPlaceable(mockActor);

			mCell.TryGetActor(out IActor result);
			Assert.AreEqual(mockActor, result);
		}

		[Test]
		public void TryGetActor_ResultIsNull_WhenNoneAdded()
		{
			mCell.TryGetActor(out IActor result);
			Assert.IsNull(result);
		}

		[Test]
		public void RemovePlaceable_MakesHasPlaceableReturnFalse()
		{
			var mockPlaceable = new MockPlaceable();
			mCell.AddPlaceable(mockPlaceable);

			mCell.RemovePlaceable(mockPlaceable);

			Assert.False(mCell.HasPlaceable());
		}

		[Test]
		public void RemovePlaceable_MakesHasPlaceableReturnTrue_WhenMoreThanOneAdded()
		{
			var mockPlaceable = new MockPlaceable();
			mCell.AddPlaceable(mockPlaceable);
			mCell.AddPlaceable(new MockPlaceable());

			mCell.RemovePlaceable(mockPlaceable);

			Assert.True(mCell.HasPlaceable());
		}

		[Test]
		public void RemovePlaceable_MakesHasPlaceableReturnTrue_WhenNonAddedPlaceableProvided()
		{
			mCell.AddPlaceable(new MockPlaceable());

			mCell.RemovePlaceable(new MockPlaceable());

			Assert.True(mCell.HasPlaceable());
		}

		[Test]
		public void AddPlaceable_TriggersCellChanged_OnAddedPlaceable()
		{
			var triggered = false;

			var placeable = new MockPlaceable();
			placeable.CellChanged += _ => triggered = true;
			mCell.AddPlaceable(placeable);

			Assert.IsTrue(triggered);
		}

		[Test]
		public void RemovePlaceable_TriggersCellChanged_OnRemovedPlaceable()
		{
			var triggered = false;

			var placeable = new MockPlaceable();
			placeable.CellChanged += _ => triggered = true;
			mCell.RemovePlaceable(placeable);

			Assert.IsTrue(triggered);
		}

		[Test]
		public void AddPlaceable_TriggersCellChangedWithCorrectCell_OnAddedPlaceable()
		{
			ICell cell = null;
			var placeable = new MockPlaceable();
			placeable.CellChanged += changedCell => cell = changedCell;
			mCell.AddPlaceable(placeable);

			Assert.AreEqual(mCell, cell);
		}

		[Test]
		public void RemovePlaceable_TriggersCellChangedWithNullCell_OnRemovedPlaceable()
		{
			var cell = mCell;

			var placeable = new MockPlaceable();
			placeable.CellChanged += changedCell => cell = changedCell;
			mCell.RemovePlaceable(placeable);

			Assert.IsNull(cell);
		}

		[Test]
		public void PlaceableAdded_Triggers_WhenAddingPlaceable()
		{
			var triggered = false;

			mCell.PlaceableAdded += _ => triggered = true;
			mCell.AddPlaceable(new MockPlaceable());

			Assert.IsTrue(triggered);
		}

		[Test]
		public void PlaceableAdded_TriggersWithCorrectValue_WhenAddingPlaceable()
		{
			var placeable = new MockPlaceable();
			IPlaceable addedPlaceable = null;

			mCell.PlaceableAdded += added => addedPlaceable = added;
			mCell.AddPlaceable(placeable);

			Assert.AreEqual(placeable, addedPlaceable);
		}

		[Test]
		public void PlaceableRemoved_Triggers_WhenRemovingPlaceable()
		{
			var triggered = false;
			var placeable = new MockPlaceable();

			mCell.AddPlaceable(placeable);
			mCell.PlaceableRemoved += _ => triggered = true;
			mCell.RemovePlaceable(placeable);

			Assert.IsTrue(triggered);
		}

		[Test]
		public void PlaceableRemoved_TriggersWithCorrectValue_WhenRemovingPlaceable()
		{
			var placeable = new MockPlaceable();
			IPlaceable removedPlaceable = null;

			mCell.AddPlaceable(placeable);
			mCell.PlaceableRemoved += removed => removedPlaceable = removed;
			mCell.RemovePlaceable(placeable);

			Assert.AreEqual(placeable, removedPlaceable);
		}
	}
}