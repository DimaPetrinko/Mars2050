using System;
using Core;
using Core.Implementation;
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
			public event Action<IPlaceable> NeighborAdded;
			public event Action<IPlaceable> NeighborRemoved;
			public IReactiveProperty<Vector2Int> Position { get; } = new ReactiveProperty<Vector2Int>();
			public void OnNewNeighbor(IPlaceable neighbor)
			{
				NeighborAdded?.Invoke(neighbor);
			}

			public void OnNeighborRemoved(IPlaceable neighbor)
			{
				NeighborRemoved?.Invoke(neighbor);
			}
		}

		private interface IMockPlaceableActor : IActor, IPlaceable
		{
		}
		private class MockActor : IMockPlaceableActor
		{
			public event Action<IPlaceable> NeighborAdded;
			public event Action<IPlaceable> NeighborRemoved;

			public Faction Faction { get; }
			public IReactiveProperty<Vector2Int> Position { get; } = new ReactiveProperty<Vector2Int>();

			public MockActor(Faction faction = Faction.Blue)
			{
				Faction = faction;
			}

			public void OnNewNeighbor(IPlaceable neighbor)
			{
				NeighborAdded?.Invoke(neighbor);
			}

			public void OnNeighborRemoved(IPlaceable neighbor)
			{
				NeighborRemoved?.Invoke(neighbor);
			}
		}

		private class MockUnit : IUnit
		{

			public Faction Faction { get; }
			public event Action Died;
			public IReactiveProperty<int> Health { get; }
			public IReactiveProperty<int> MaxHealth { get; }
			public void Damage(int damage)
			{
			}

			public void Heal(int amount)
			{
			}

			public void RestoreMaxHealth()
			{
			}

			public event Action<IPlaceable> NeighborAdded;
			public event Action<IPlaceable> NeighborRemoved;
			public IReactiveProperty<Vector2Int> Position { get; } = new ReactiveProperty<Vector2Int>();

			public void OnNewNeighbor(IPlaceable neighbor)
			{
				NeighborAdded?.Invoke(neighbor);
			}

			public void OnNeighborRemoved(IPlaceable neighbor)
			{
				NeighborRemoved?.Invoke(neighbor);
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
			Assert.IsFalse(mCell.HasPlaceable());
		}

		[Test]
		public void HasPlaceable_ReturnsTrue_AfterPlaceableAdded()
		{
			mCell.AddPlaceable(new MockPlaceable());

			Assert.IsTrue(mCell.HasPlaceable());
		}

		[Test]
		public void HasPlaceableWithType_ReturnsTrue_WhenCorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.IsTrue(mCell.HasPlaceable<IMockPlaceableActor>());
		}

		[Test]
		public void HasPlaceableWithType_ReturnsFalse_WhenIncorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.IsFalse(mCell.HasPlaceable<IResource>());
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

			Assert.IsTrue(mCell.TryGetPlaceable(out IPlaceable result));
		}

		[Test]
		public void TryGetPlaceable_ReturnsFalse_WhenNoneAdded()
		{
			Assert.IsFalse(mCell.TryGetPlaceable(out IPlaceable result));
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
			Assert.IsFalse(mCell.HasActor());
		}

		[Test]
		public void HasActor_ReturnsFalse_WhenNonActorAdded()
		{
			mCell.AddPlaceable(new MockPlaceable());

			Assert.IsFalse(mCell.HasActor());
		}

		[Test]
		public void HasActor_ReturnsTrue_AfterPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.IsTrue(mCell.HasActor());
		}

		[Test]
		public void HasActorWithType_ReturnsTrue_WhenCorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.IsTrue(mCell.HasActor<IActor>());
		}

		[Test]
		public void HasActorWithType_ReturnsFalse_WhenIncorrectTypeOfPlaceableAdded()
		{
			mCell.AddPlaceable(new MockActor());

			Assert.IsFalse(mCell.HasActor<IResource>());
		}

		[Test]
		public void HasActorWithType_ReturnsFalse_WhenNonActorProvided()
		{
			mCell.AddPlaceable(new MockPlaceable());

			Assert.IsFalse(mCell.HasActor<IActor>());
		}

		[Test]
		public void HasActorWithFaction_ReturnsTrue_WhenCorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.IsTrue(mCell.HasActor(Faction.Yellow));
		}

		[Test]
		public void HasActorWithFaction_ReturnsFalse_WhenIncorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.IsFalse(mCell.HasActor(Faction.Blue));
		}

		[Test]
		public void HasActorWithFactionAndType_ReturnsTrue_WhenCorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.IsTrue(mCell.HasActor<IActor>(Faction.Yellow));
		}

		[Test]
		public void HasActorWithFactionAndType_ReturnsFalse_WhenIncorrectFactionProvided()
		{
			var mockActor = new MockActor(Faction.Yellow);
			mCell.AddPlaceable(mockActor);

			Assert.IsFalse(mCell.HasActor<IActor>(Faction.Blue));
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
		public void GetActorWithFaction_ReturnsActor_WhenActorOfCorrectFactionAdded()
		{
			var mockActor = new MockActor(Faction.Red);
			mCell.AddPlaceable(mockActor);

			var result = mCell.GetActor<IActor>(Faction.Red);
			Assert.AreEqual(mockActor, result);
		}

		[Test]
		public void GetActorWithFaction_DoesNotReturnActor_WhenActorOfOtherFactionAdded()
		{
			var mockActor = new MockActor(Faction.Red);
			mCell.AddPlaceable(mockActor);

			var result = mCell.GetActor<IActor>(Faction.Blue);
			Assert.IsNull(result);
		}

		[Test]
		public void TryGetActor_ReturnsTrue_WhenActorAdded()
		{
			var mockActor = new MockActor();
			mCell.AddPlaceable(mockActor);

			Assert.IsTrue(mCell.TryGetActor(out IActor result));
		}

		[Test]
		public void TryGetActor_ReturnsFalse_WhenNoneAdded()
		{
			Assert.IsFalse(mCell.TryGetActor(out IActor result));
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
		public void TryGetActorWithFaction_ReturnsTrue_WhenActorOfCorrectFactionAdded()
		{
			var mockActor = new MockActor(Faction.Red);
			mCell.AddPlaceable(mockActor);

			Assert.IsTrue(mCell.TryGetActor(out IActor result, Faction.Red));
		}

		[Test]
		public void TryGetActorWithFaction_ReturnsFalse_WhenActorOfOtherFactionAdded()
		{
			var mockActor = new MockActor(Faction.Red);
			mCell.AddPlaceable(mockActor);

			Assert.IsFalse(mCell.TryGetActor(out IActor result, Faction.Blue));
		}

		[Test]
		public void TryGetActorWithFaction_ResultIsCorrect_WhenActorOfCorrectFactionAdded()
		{
			var mockActor = new MockActor(Faction.Red);
			mCell.AddPlaceable(mockActor);

			mCell.TryGetActor(out IActor result, Faction.Red);
			Assert.AreEqual(mockActor, result);
		}

		[Test]
		public void TryGetActorWithFaction_ResultIsNull_WhenActorOfOtherFactionAdded()
		{
			var mockActor = new MockActor(Faction.Red);
			mCell.AddPlaceable(mockActor);

			mCell.TryGetActor(out IActor result, Faction.Blue);
			Assert.IsNull(result);
		}

		[Test]
		public void RemovePlaceable_MakesHasPlaceableReturnFalse()
		{
			var mockPlaceable = new MockPlaceable();
			mCell.AddPlaceable(mockPlaceable);

			mCell.RemovePlaceable(mockPlaceable);

			Assert.IsFalse(mCell.HasPlaceable());
		}

		[Test]
		public void RemovePlaceable_MakesHasPlaceableReturnTrue_WhenMoreThanOneAdded()
		{
			var mockPlaceable = new MockPlaceable();
			mCell.AddPlaceable(mockPlaceable);
			mCell.AddPlaceable(new MockPlaceable());

			mCell.RemovePlaceable(mockPlaceable);

			Assert.IsTrue(mCell.HasPlaceable());
		}

		[Test]
		public void RemovePlaceable_MakesHasPlaceableReturnTrue_WhenNonAddedPlaceableProvided()
		{
			mCell.AddPlaceable(new MockPlaceable());

			mCell.RemovePlaceable(new MockPlaceable());

			Assert.IsTrue(mCell.HasPlaceable());
		}

		[Test]
		public void AddPlaceable_ChangesPosition_OnAddedPlaceable()
		{
			var position = new Vector2Int(4, 5);
			ICell cell = new Cell(position);
			var placeable = new MockPlaceable();
			cell.AddPlaceable(placeable);

			Assert.AreEqual(cell.Position, placeable.Position.Value);
		}

		[Test]
		public void RemovePlaceable_TriggersCellChangedWithNullCell_OnRemovedPlaceable()
		{
			var position = new Vector2Int(4, 5);
			ICell cell = new Cell(position);
			var placeable = new MockPlaceable();
			cell.AddPlaceable(placeable);
			cell.RemovePlaceable(placeable);

			Assert.AreNotEqual(cell.Position, placeable.Position.Value);
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

		[Test]
		public void OnNewNeighbor_IsTriggered_OnOthersWhenAddingPlaceable()
		{
			var placeable1 = new MockPlaceable();
			var placeable2 = new MockPlaceable();
			var placeable3 = new MockPlaceable();

			var triggered1 = false;
			var triggered2 = false;
			var triggered3 = false;

			mCell.AddPlaceable(placeable1);
			mCell.AddPlaceable(placeable2);
			placeable1.NeighborAdded += _ => triggered1 = true;
			placeable2.NeighborAdded += _ => triggered2 = true;
			placeable3.NeighborAdded += _ => triggered3 = true;
			mCell.AddPlaceable(placeable3);

			Assert.IsTrue(triggered1);
			Assert.IsTrue(triggered2);
			Assert.IsFalse(triggered3);
		}

		[Test]
		public void OnNewNeighbor_IsTriggeredWithCorrectValue_OnOthersWhenAddingPlaceable()
		{
			var placeable1 = new MockPlaceable();
			var placeable2 = new MockPlaceable();
			var placeable3 = new MockPlaceable();

			IPlaceable receivedPlaceable1 = null;
			IPlaceable receivedPlaceable2 = null;
			IPlaceable receivedPlaceable3 = null;

			mCell.AddPlaceable(placeable1);
			mCell.AddPlaceable(placeable2);
			placeable1.NeighborAdded += neighbor => receivedPlaceable1 = neighbor;
			placeable2.NeighborAdded += neighbor => receivedPlaceable2 = neighbor;
			placeable3.NeighborAdded += neighbor => receivedPlaceable3 = neighbor;
			mCell.AddPlaceable(placeable3);

			Assert.AreEqual(placeable3, receivedPlaceable1);
			Assert.AreEqual(placeable3, receivedPlaceable2);
			Assert.IsNull(receivedPlaceable3);
		}

		[Test]
		public void OnNeighborRemoved_IsTriggered_OnOthersWhenRemovingPlaceable()
		{
			var placeable1 = new MockPlaceable();
			var placeable2 = new MockPlaceable();
			var placeable3 = new MockPlaceable();

			var triggered1 = false;
			var triggered2 = false;
			var triggered3 = false;

			mCell.AddPlaceable(placeable1);
			mCell.AddPlaceable(placeable2);
			mCell.AddPlaceable(placeable3);
			placeable1.NeighborRemoved += _ => triggered1 = true;
			placeable2.NeighborRemoved += _ => triggered2 = true;
			placeable3.NeighborRemoved += _ => triggered3 = true;
			mCell.RemovePlaceable(placeable3);

			Assert.IsTrue(triggered1);
			Assert.IsTrue(triggered2);
			Assert.IsFalse(triggered3);
		}

		[Test]
		public void OnNeighborRemoved_IsTriggeredWithCorrectValue_OnOthersWhenRemovingPlaceable()
		{
			var placeable1 = new MockPlaceable();
			var placeable2 = new MockPlaceable();
			var placeable3 = new MockPlaceable();

			IPlaceable receivedPlaceable1 = null;
			IPlaceable receivedPlaceable2 = null;
			IPlaceable receivedPlaceable3 = null;

			mCell.AddPlaceable(placeable1);
			mCell.AddPlaceable(placeable2);
			mCell.AddPlaceable(placeable3);
			placeable1.NeighborRemoved += neighbor => receivedPlaceable1 = neighbor;
			placeable2.NeighborRemoved += neighbor => receivedPlaceable2 = neighbor;
			placeable3.NeighborRemoved += neighbor => receivedPlaceable3 = neighbor;
			mCell.RemovePlaceable(placeable3);

			Assert.AreEqual(placeable3, receivedPlaceable1);
			Assert.AreEqual(placeable3, receivedPlaceable2);
			Assert.IsNull(receivedPlaceable3);
		}
	}
}