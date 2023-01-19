using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.GameProcess.Implementation;
using NUnit.Framework;

namespace Tests
{
	public class Player_Tests
	{
		private IPlayer mPlayer;

		[SetUp]
		public void SetUp()
		{
			mPlayer = new Player(Faction.Red);
		}

		[Test]
		public void ActionSelected_Triggers_WhenSelected()
		{
			var triggered = false;

			mPlayer.ActionSelected += _ => triggered = true;
			mPlayer.SelectAction(ActionType.Move);

			Assert.IsTrue(triggered);
		}

		[Test]
		public void ActionSelected_TriggersWithCorrectValue()
		{
			var receivedType = ActionType.PlayerTrade;

			mPlayer.ActionSelected += type => receivedType = type;
			mPlayer.SelectAction(ActionType.Build);

			Assert.AreEqual(ActionType.Build, receivedType);
		}

		[Test]
		public void ActionCanceled_Triggers_WhenCanceled()
		{
			var triggered = false;

			mPlayer.SelectAction(ActionType.Move);
			mPlayer.ActionCanceled += () => triggered = true;
			mPlayer.CancelAction();

			Assert.IsTrue(triggered);
		}

		[Test]
		public void ActionCanceled_DoesNotTrigger_WhenNotSelected()
		{
			var triggered = false;

			mPlayer.ActionCanceled += () => triggered = true;
			mPlayer.CancelAction();

			Assert.IsFalse(triggered);
		}
	}
}