using Core.Models.GameProcess;
using Core.Models.GameProcess.Implementation;
using NUnit.Framework;

namespace Tests
{
	public class Dice_Tests
	{
		private IDice mDice;

		[SetUp]
		public void SetUp()
		{
			mDice = new Dice(6);
		}

		[Test]
		public void Rolled_Triggered_WhenRollCalled()
		{
			var triggered = false;

			mDice.Rolled += _ => triggered = true;
			mDice.Roll();

			Assert.IsTrue(triggered);
		}

		[Test]
		public void Rolled_TriggeredWithCorrectValue_WhenRollCalled()
		{
			var receivedValue = 255;
			mDice.Rolled += roll => receivedValue = roll;

			for (var i = 0; i < 1000; i++)
			{
				mDice.Roll();
				Assert.GreaterOrEqual(receivedValue, 1);
				Assert.LessOrEqual(receivedValue, 6);
			}
		}
	}
}