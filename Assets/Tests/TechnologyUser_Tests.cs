using System.Linq;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.GameProcess.Implementation;
using Core.Models.Technology;
using NUnit.Framework;

namespace Tests
{
	public class TechnologyUser_Tests
	{
		#region Mock classes

		private class MockTechnology : ITechnology
		{
			public TechnologyType Type { get; }
			public bool Activated { get; }

			public MockTechnology(TechnologyType type)
			{
				Type = type;
			}

			public void TakeEffect()
			{
			}
		}

		#endregion

		private ITechnologyUser mTechnologyUser;

		[SetUp]
		public void SetUp()
		{
			mTechnologyUser = new TechnologyUser();
		}

		[Test]
		public void Technologies_AreEmpty_WhenCreated()
		{
			Assert.AreEqual(0, mTechnologyUser.Technologies.Count());
		}

		[Test]
		public void AddTechnology_AddsTechnology()
		{
			var previousCount = mTechnologyUser.Technologies.Count();

			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			Assert.AreEqual(previousCount + 1, mTechnologyUser.Technologies.Count());
		}

		[Test]
		public void AddTechnology_AddsTechnology_WhenAddedDifferentType()
		{
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			var previousCount = mTechnologyUser.Technologies.Count();
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology2));

			Assert.AreEqual(previousCount + 1, mTechnologyUser.Technologies.Count());
		}

		[Test]
		public void AddTechnology_DoesNothing_WhenAddedSameType()
		{
			var originalTechnology = new MockTechnology(TechnologyType.TestTechnology1);
			mTechnologyUser.AddTechnology(originalTechnology);
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			Assert.AreEqual(1, mTechnologyUser.Technologies.Count());
			Assert.AreEqual(originalTechnology, mTechnologyUser.Technologies.First());
		}

		[Test]
		public void AddTechnology_MakesTechnologiesContainAllAdded()
		{
			var technology1 = new MockTechnology(TechnologyType.TestTechnology1);
			var technology2 = new MockTechnology(TechnologyType.TestTechnology2);
			var technology4 = new MockTechnology(TechnologyType.TestTechnology4);

			mTechnologyUser.AddTechnology(technology1);
			mTechnologyUser.AddTechnology(technology2);
			mTechnologyUser.AddTechnology(technology4);

			var technologies = mTechnologyUser.Technologies.ToList();

			Assert.IsTrue(technologies.Contains(technology1));
			Assert.IsTrue(technologies.Contains(technology2));
			Assert.IsTrue(technologies.Contains(technology4));
		}

		[Test]
		public void SpendTechnology_RemovesTechnology()
		{
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));
			var previousCount = mTechnologyUser.Technologies.Count();

			mTechnologyUser.SpendTechnology(mTechnologyUser.Technologies.First());

			Assert.AreEqual(previousCount - 1, mTechnologyUser.Technologies.Count());
		}

		[Test]
		public void SpendTechnology_DoesNothing_WhenNonAddedProvided()
		{
			var technology = new MockTechnology(TechnologyType.TestTechnology1);
			mTechnologyUser.AddTechnology(technology);
			var previousCount = mTechnologyUser.Technologies.Count();

			mTechnologyUser.SpendTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			Assert.AreEqual(previousCount, mTechnologyUser.Technologies.Count());
			Assert.AreEqual(technology, mTechnologyUser.Technologies.First());
		}

		[Test]
		public void SpendTechnology_MakesTechnologiesContainAllLeft()
		{
			var technology1 = new MockTechnology(TechnologyType.TestTechnology1);
			var technology2 = new MockTechnology(TechnologyType.TestTechnology2);
			var technology4 = new MockTechnology(TechnologyType.TestTechnology4);

			mTechnologyUser.AddTechnology(technology1);
			mTechnologyUser.AddTechnology(technology2);
			mTechnologyUser.AddTechnology(technology4);

			mTechnologyUser.SpendTechnology(technology2);

			var technologies = mTechnologyUser.Technologies.ToList();

			Assert.IsTrue(technologies.Contains(technology1));
			Assert.IsFalse(technologies.Contains(technology2));
			Assert.IsTrue(technologies.Contains(technology4));
		}

		[Test]
		public void TechnologyAdded_Triggers_WhenAdded()
		{
			var triggered = false;

			mTechnologyUser.TechnologyAdded += _ => triggered = true;
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			Assert.IsTrue(triggered);
		}

		[Test]
		public void TechnologyAdded_DoesNotTrigger_WhenAddedSameType()
		{
			var triggered = false;

			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));
			mTechnologyUser.TechnologyAdded += _ => triggered = true;
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			Assert.IsFalse(triggered);
		}

		[Test]
		public void TechnologySpent_Triggers_WhenRemoved()
		{
			var triggered = false;
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			mTechnologyUser.TechnologySpent += _ => triggered = true;
			mTechnologyUser.SpendTechnology(mTechnologyUser.Technologies.First());

			Assert.IsTrue(triggered);
		}

		[Test]
		public void TechnologySpent_DoesNotTrigger_WhenNonAddedProvided()
		{
			var triggered = false;
			mTechnologyUser.AddTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			mTechnologyUser.TechnologySpent += _ => triggered = true;
			mTechnologyUser.SpendTechnology(new MockTechnology(TechnologyType.TestTechnology1));

			Assert.IsFalse(triggered);
		}

		[Test]
		public void TechnologyAdded_TriggersWithCorrectValue_WhenAdded()
		{
			ITechnology receivedValue = null;
			var technology = new MockTechnology(TechnologyType.TestTechnology1);

			mTechnologyUser.TechnologyAdded += t => receivedValue = t;
			mTechnologyUser.AddTechnology(technology);

			Assert.AreEqual(technology, receivedValue);
		}

		[Test]
		public void TechnologySpent_TriggersWithCorrectValue_WhenRemoved()
		{
			var receivedValue = TechnologyType.TestTechnology1;
			var technology = new MockTechnology(TechnologyType.TestTechnology2);

			mTechnologyUser.AddTechnology(technology);
			mTechnologyUser.TechnologySpent += t => receivedValue = t;
			mTechnologyUser.SpendTechnology(technology);

			Assert.AreEqual(technology.Type, receivedValue);
		}
	}
}