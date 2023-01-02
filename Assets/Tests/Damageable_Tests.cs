using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using NUnit.Framework;

namespace Tests
{
	public class Damageable_Tests
	{
		private IDamageable mDamageable;

		[SetUp]
		public void SetUp()
		{
			mDamageable = new Damageable(3);
		}

		[Test]
		public void MaxHealth_IsSet()
		{
			Assert.AreEqual(3, mDamageable.MaxHealth.Value);
		}

		[Test]
		public void Health_IsTheSameAsMaxHealth()
		{
			Assert.AreEqual(3, mDamageable.Health.Value);
		}

		[Test]
		public void Health_IsNoMoreThanMaxHealth()
		{
			mDamageable.Health.Value = 60;

			Assert.AreEqual(mDamageable.MaxHealth.Value, mDamageable.Health.Value);
		}

		[Test]
		public void Health_IsNoLessThanZero()
		{
			mDamageable.Health.Value = -60;

			Assert.AreEqual(0, mDamageable.Health.Value);
		}

		[Test]
		public void Health_IsClamped_WhenMaxHealthChanged()
		{
			mDamageable.MaxHealth.Value = 6;
			mDamageable.Health.Value = 5;

			mDamageable.MaxHealth.Value = 3;

			Assert.AreEqual(mDamageable.MaxHealth.Value, mDamageable.Health.Value);
		}

		[Test]
		public void Damage_SubtractsHealth()
		{
			mDamageable.Damage(2);

			Assert.AreEqual(1, mDamageable.Health.Value);
		}

		[Test]
		public void Damage_DoesNotSubtractHealth_WhenNegativeValue()
		{
			mDamageable.Health.Value = 2;
			mDamageable.Damage(-2);

			Assert.AreEqual(2, mDamageable.Health.Value);
		}

		[Test]
		public void Heal_AddsHealth()
		{
			mDamageable.Health.Value = 1;

			mDamageable.Heal(1);

			Assert.AreEqual(2, mDamageable.Health.Value);
		}

		[Test]
		public void Heal_DoesNotAddHealth_WhenNegativeValue()
		{
			mDamageable.Health.Value = 1;

			mDamageable.Heal(-1);

			Assert.AreEqual(1, mDamageable.Health.Value);
		}

		[Test]
		public void Died_Triggers_WhenHealthReaches0()
		{
			var triggered = false;

			mDamageable.Died += () => triggered = true;
			mDamageable.Health.Value = 0;

			Assert.IsTrue(triggered);
		}

		[Test]
		public void Died_Triggers_WhenDamagedForMoreThanHealth()
		{
			var triggered = false;

			mDamageable.Died += () => triggered = true;
			mDamageable.Damage(60);

			Assert.IsTrue(triggered);
		}

		[Test]
		public void Died_DoesNotTrigger_WhenHealthIsNot0()
		{
			var triggered = false;

			mDamageable.Died += () => triggered = true;
			mDamageable.Health.Value = 2;

			Assert.IsFalse(triggered);
		}

		[Test]
		public void HealthChanged_Triggers_WhenHealthChanges()
		{
			var triggered = false;

			mDamageable.Health.Changed += _ => triggered = true;
			mDamageable.Health.Value = 1;

			Assert.IsTrue(triggered);
		}

		[Test]
		public void HealthChanged_DoesNotTrigger_WhenHealthStaysSame()
		{
			var triggered = false;

			mDamageable.Health.Changed += _ => triggered = true;
			mDamageable.Health.Value = 3;

			Assert.IsFalse(triggered);
		}

		[Test]
		public void MaxHealthChanged_Triggers_WhenMaxHealthChanges()
		{
			var triggered = false;

			mDamageable.MaxHealth.Changed += _ => triggered = true;
			mDamageable.MaxHealth.Value = 1;

			Assert.IsTrue(triggered);
		}

		[Test]
		public void MaxHealthChanged_DoesNotTrigger_WhenMaxHealthStaysSame()
		{
			var triggered = false;

			mDamageable.MaxHealth.Changed += _ => triggered = true;
			mDamageable.MaxHealth.Value = 3;

			Assert.IsFalse(triggered);
		}
	}
}