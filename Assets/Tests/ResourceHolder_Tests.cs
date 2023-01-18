using System.Collections.Generic;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.GameProcess.Implementation;
using Core.Utils;
using NUnit.Framework;

namespace Tests
{
	public class ResourceHolder_Tests
	{
		private IResourceHolder mResourceHolder;

		[SetUp]
		public void SetUp()
		{
			mResourceHolder = new ResourceHolder();
		}

		[Test]
		public void ResourcesAmount_Returns0_WhenCreated()
		{
			Assert.AreEqual(0, mResourceHolder.ResourcesAmount);
		}

		[Test]
		public void AddResources_ChangesAmount()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 10));

			Assert.AreEqual(10, mResourceHolder.ResourcesAmount);
		}

		[Test]
		public void UseResources_ChangesAmount()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Ore, 10));
			mResourceHolder.UseResources(new ResourcePackage(ResourceType.Ore, 3));

			Assert.AreEqual(7, mResourceHolder.ResourcesAmount);
		}

		[Test]
		public void UseResources_DoesNotChangeAmountBelow0()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Ore, 7));
			mResourceHolder.UseResources(new ResourcePackage(ResourceType.Ore, 10));

			Assert.AreEqual(0, mResourceHolder.ResourcesAmount);
		}

		[Test]
		public void HasResources_ReturnsTrue_WhenAddedRequestedResource()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Water, 10));

			Assert.IsTrue(mResourceHolder.HasResource(ResourceType.Water, 1));
		}

		[Test]
		public void HasResources_ReturnsTrue_WhenAddedRequestedAmount()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Plants, 10));

			Assert.IsTrue(mResourceHolder.HasResource(ResourceType.Plants, 10));
		}

		[Test]
		public void HasResources_ReturnsTrue_WhenAddedGreaterAmount()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 10));

			Assert.IsTrue(mResourceHolder.HasResource(ResourceType.Electricity, 4));
		}

		[Test]
		public void HasResources_ReturnsFalse_WhenAddedOtherResource()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Ore, 10));

			Assert.IsFalse(mResourceHolder.HasResource(ResourceType.Water, 1));
		}

		[Test]
		public void HasResources_ReturnsFalse_WhenAddedSmallerAmount()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Plants, 4));

			Assert.IsFalse(mResourceHolder.HasResource(ResourceType.Plants, 10));
		}

		[Test]
		public void GetResource_Returns0_WhenNoSuchAdded()
		{
			Assert.AreEqual(0, mResourceHolder.GetResource(ResourceType.Ore));
		}

		[Test]
		public void GetResource_ReturnsCorrectValue_WhenAdded()
		{
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 2));

			Assert.AreEqual(2, mResourceHolder.GetResource(ResourceType.Electricity));
		}

		[Test]
		public void GetResource_ReturnsCorrectValue_WhenMultipleAdded()
		{
			mResourceHolder.AddResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Plants, 5 },
				{ ResourceType.Water, 2 }
			}));

			Assert.AreEqual(5, mResourceHolder.GetResource(ResourceType.Plants));
		}

		[Test]
		public void ResourceAmountChanged_Triggers_WhenAdded()
		{
			var triggered = false;

			mResourceHolder.ResourceAmountChanged += (_, _) => triggered = true;
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 2));

			Assert.IsTrue(triggered);
		}

		[Test]
		public void ResourceAmountChanged_TriggersCorrectTimes_WhenAddedMultiple()
		{
			var triggered = 0;

			mResourceHolder.ResourceAmountChanged += (_, _) => triggered++;
			mResourceHolder.AddResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Electricity, 2 },
				{ ResourceType.Water, 2 }
			}));

			Assert.AreEqual(2, triggered);
		}

		[Test]
		public void ResourceAmountChanged_Triggers_WhenUsed()
		{
			var triggered = false;

			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 5));
			mResourceHolder.ResourceAmountChanged += (_, _) => triggered = true;
			mResourceHolder.UseResources(new ResourcePackage(ResourceType.Electricity, 2));

			Assert.IsTrue(triggered);
		}

		[Test]
		public void ResourceAmountChanged_TriggersCorrectTimes_WhenUsedMultiple()
		{
			var triggered = 0;

			mResourceHolder.AddResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Electricity, 10 },
				{ ResourceType.Water, 10 }
			}));
			mResourceHolder.ResourceAmountChanged += (_, _) => triggered++;
			mResourceHolder.UseResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Electricity, 2 },
				{ ResourceType.Water, 2 }
			}));

			Assert.AreEqual(2, triggered);
		}

		[Test]
		public void ResourceAmountChanged_TriggersCorrectTimes_WhenUsedNonAddedType()
		{
			var triggered = 0;

			mResourceHolder.AddResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Water, 10 }
			}));
			mResourceHolder.ResourceAmountChanged += (_, _) => triggered++;
			mResourceHolder.UseResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Electricity, 2 },
				{ ResourceType.Water, 2 }
			}));

			Assert.AreEqual(1, triggered);
		}

		[Test]
		public void ResourceAmountChanged_DoesNotTrigger_When0Added()
		{
			var triggered = false;

			mResourceHolder.ResourceAmountChanged += (_, _) => triggered = true;
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 0));

			Assert.IsFalse(triggered);
		}

		[Test]
		public void ResourceAmountChanged_DoesNotTrigger_When0AddedAndSomeUsed()
		{
			var triggered = false;

			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 0));
			mResourceHolder.ResourceAmountChanged += (_, _) => triggered = true;
			mResourceHolder.UseResources(new ResourcePackage(ResourceType.Electricity, 2));

			Assert.IsFalse(triggered);
		}

		[Test]
		public void ResourceAmountChanged_TriggersWithCorrectTypeAndAmount_WhenAdded()
		{
			var receivedType = ResourceType.Ore;
			var receivedAmount = 0;
			mResourceHolder.ResourceAmountChanged += (type, amount) =>
			{
				receivedType = type;
				receivedAmount = amount;
			};
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Electricity, 2));

			Assert.AreEqual(ResourceType.Electricity, receivedType);
			Assert.AreEqual(2, receivedAmount);
		}

		[Test]
		public void ResourceAmountChanged_TriggersWithCorrectTypeAndAmount_WhenRemoved()
		{
			var receivedType = ResourceType.Ore;
			var receivedAmount = 0;
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Plants, 10));
			mResourceHolder.ResourceAmountChanged += (type, amount) =>
			{
				receivedType = type;
				receivedAmount = amount;
			};
			mResourceHolder.UseResources(new ResourcePackage(ResourceType.Plants, 2));

			Assert.AreEqual(ResourceType.Plants, receivedType);
			Assert.AreEqual(8, receivedAmount);
		}

		[Test]
		public void ResourceAmountChanged_TriggersWith0Amount_WhenRemovedBeyond0()
		{
			var receivedAmount = 99;
			mResourceHolder.AddResources(new ResourcePackage(ResourceType.Plants, 2));
			mResourceHolder.ResourceAmountChanged += (_, amount) =>
			{
				receivedAmount = amount;
			};
			mResourceHolder.UseResources(new ResourcePackage(ResourceType.Plants, 10));

			Assert.AreEqual(0, receivedAmount);
		}
	}
}