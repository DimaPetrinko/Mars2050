using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.GameProcess.Implementation;
using NUnit.Framework;

namespace Tests
{
	public class TurnPerformer_Test
	{
		private ITurnPerformer mTurnPerformer;

		[SetUp]
		public void SetUp()
		{
			mTurnPerformer = new TurnPerformer(Faction.Red, 6);
		}

		[Test]
		public void Faction_IsSet()
		{
			Assert.AreEqual(Faction.Red, mTurnPerformer.Faction);
		}

		[Test]
		public void Oxygen_IsSetTo0()
		{
			Assert.AreEqual(0, mTurnPerformer.Oxygen.Value);
		}

		[Test]
		public void Oxygen_DoesNotGoBeyond0()
		{
			mTurnPerformer.Oxygen.Value = -3;

			Assert.AreEqual(0, mTurnPerformer.Oxygen.Value);
		}

		[Test]
		public void Roll_ReturnsValueBetween0AndMaxRollBothInclusive()
		{
			for (var i = 0; i < 1000; i++)
			{
				var roll = mTurnPerformer.Roll();
				Assert.GreaterOrEqual(roll, 0);
				Assert.LessOrEqual(roll, 6);
			}
		}
	}
}