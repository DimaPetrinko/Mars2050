using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Enums;
using NUnit.Framework;

namespace Tests
{
	public class BaseBuilding_Tests
	{
		[Test]
		public void Faction_IsSet()
		{
			IBaseBuilding baseBuilding = new BaseBuilding(Faction.Yellow);

			Assert.AreEqual(Faction.Yellow, baseBuilding.Faction);
		}
	}
}