using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Enums;
using NUnit.Framework;

namespace Tests
{
	public class Building_Tests
	{
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
	}
}