using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Enums;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	public class Placeable_Tests
	{
		private class MockCell : ICell
		{
			public Vector2Int Position { get; }
			public bool HasPlaceable()
			{
				throw new System.NotImplementedException();
			}

			public bool HasPlaceable<T>() where T : IPlaceable
			{
				throw new System.NotImplementedException();
			}

			public bool TryGetPlaceable<T>(out T placeable) where T : IPlaceable
			{
				throw new System.NotImplementedException();
			}

			public T GetPlaceable<T>() where T : IPlaceable
			{
				throw new System.NotImplementedException();
			}

			public bool HasActor(Faction faction = Faction.Any)
			{
				throw new System.NotImplementedException();
			}

			public bool HasActor<T>(Faction faction = Faction.Any)
			{
				throw new System.NotImplementedException();
			}

			public bool TryGetActor<T>(out T placeable, Faction faction = Faction.Any)
			{
				throw new System.NotImplementedException();
			}

			public T GetActor<T>(Faction faction = Faction.Any)
			{
				throw new System.NotImplementedException();
			}

			public void AddPlaceable(IPlaceable placeable)
			{
			}

			public void RemovePlaceable(IPlaceable placeable)
			{
			}
		}

		private IPlaceable mPlaceable;

		[SetUp]
		public void SetUp()
		{
			mPlaceable = new Placeable();
		}

		[Test]
		public void CellChanged_Triggers()
		{
			var triggered = false;

			mPlaceable.CellChanged += _ => triggered = true;
			mPlaceable.ChangeCell(new MockCell());

			Assert.IsTrue(triggered);
		}

		[Test]
		public void AddPlaceable_TriggersCellChangedWithCorrectCell_OnAddedPlaceable()
		{
			var cell = new MockCell();
			ICell triggeredCell = null;
			mPlaceable.CellChanged += c => triggeredCell = c;
			mPlaceable.ChangeCell(cell);

			Assert.AreEqual(cell, triggeredCell);
		}

		[Test]
		public void RemovePlaceable_TriggersCellChangedWithNullCell_OnRemovedPlaceable()
		{
			ICell triggeredCell = null;
			mPlaceable.CellChanged += c => triggeredCell = c;
			mPlaceable.ChangeCell(null);

			Assert.IsNull(triggeredCell);
		}
	}
}