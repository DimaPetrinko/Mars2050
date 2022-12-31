using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Enums;
using NUnit.Framework;

namespace Tests
{
	public class Resource_Tests
	{
		private IResource mResource;

		[SetUp]
		public void SetUp()
		{
			mResource = new Resource(ResourceType.Ore);
		}

		[Test]
		public void IsDiscovered_ReturnsFalse_ByDefault()
		{
			Assert.IsFalse(mResource.IsDiscovered);
		}

		[Test]
		public void ResourceType_IsSet()
		{
			Assert.AreEqual(ResourceType.Ore, mResource.Type);
		}

		[Test]
		public void IsDiscoveredChanged_Triggers_WhenIsDiscoveredSetToOpposite()
		{
			var triggered = false;

			mResource.IsDiscoveredChanged += () => triggered = true;
			mResource.IsDiscovered = true;

			Assert.IsTrue(triggered);
		}

		[Test]
		public void IsDiscoveredChanged_DoesNotTrigger_WhenIsDiscoveredSetToSame()
		{
			var triggered = false;

			mResource.IsDiscovered = true;
			mResource.IsDiscoveredChanged += () => triggered = true;
			mResource.IsDiscovered = true;

			Assert.IsFalse(triggered);
		}

		[Test]
		public void IsDiscovered_HasNewValue_WhenIsDiscoveredChangedTriggered()
		{
			mResource.IsDiscovered = false;
			var result = false;

			mResource.IsDiscoveredChanged += () => result = mResource.IsDiscovered;
			mResource.IsDiscovered = true;

			Assert.AreEqual(true, result);
		}
	}
}