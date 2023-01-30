using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Enums;
using NUnit.Framework;

namespace Tests
{
	public class Unit_tests
	{
		private IUnit mUnit;

		#region Mock classes

		#endregion

		[SetUp]
		public void SetUp()
		{
			mUnit = new Unit(Faction.Red, 3);
		}

		[Test]
		public void Faction_IsSet()
		{
			Assert.AreEqual(Faction.Red, mUnit.Faction);
		}
	}
}