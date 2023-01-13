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
			Assert.IsFalse(mResource.IsDiscovered.Value);
		}

		[Test]
		public void ResourceType_IsSet()
		{
			Assert.AreEqual(ResourceType.Ore, mResource.Type);
		}
	}
}