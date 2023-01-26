using Core.Implementation;
using NUnit.Framework;

namespace Tests
{
	public class ReactiveProperty_Tests
	{
		[Test]
		public void Value_IsSetToDefault()
		{
			var property = new ReactiveProperty<int>();

			Assert.AreEqual(0, property.Value);
		}

		[Test]
		public void Value_IsSet_WhenDefaultProvided()
		{
			var property = new ReactiveProperty<int>(2);

			Assert.AreEqual(2, property.Value);
		}

		[Test]
		public void Value_IsChanged()
		{
			var property = new ReactiveProperty<int>(2);

			property.Value = 5;

			Assert.AreNotEqual(2, property.Value);
		}

		[Test]
		public void Changed_IsTriggered_WhenChangingToDifferentValue()
		{
			var triggered = false;
			var property = new ReactiveProperty<int>();

			property.Changed += _ => triggered = true;
			property.Value = 5;

			Assert.IsTrue(triggered);
		}

		[Test]
		public void Changed_IsNotTriggered_WhenChangingToSameValue()
		{
			var triggered = false;
			var property = new ReactiveProperty<int>(4);

			property.Changed += _ => triggered = true;
			property.Value = 4;

			Assert.IsFalse(triggered);
		}

		[Test]
		public void Changed_IsTriggeredWithCorrectValue()
		{
			var changedValue = 0;
			var property = new ReactiveProperty<int>();

			property.Changed += v => changedValue = v;
			property.Value = 4;

			Assert.AreEqual(4, changedValue);
		}

		[Test]
		public void Updated_IsTriggered_EveryTime()
		{
			var triggered = false;
			var property = new ReactiveProperty<int>();

			property.Value = 5;
			property.Updated += _ => triggered = true;
			property.Value = 5;

			Assert.IsTrue(triggered);
		}

		[Test]
		public void Updated_IsTriggeredWithCorrectValue()
		{
			var changedValue = 0;
			var property = new ReactiveProperty<int>();

			property.Value = 4;
			property.Updated += v => changedValue = v;
			property.Value = 4;

			Assert.AreEqual(4, changedValue);

		}

		[Test]
		public void CustomSetter_IsCalled_WhenProvided()
		{
			var called = false;
			var property = new ReactiveProperty<int>(0,
				(_, _, _, _) => called = true);

			property.Value = 3;

			Assert.IsTrue(called);
		}

		[Test]
		public void CustomSetter_DoesNotCallDefaultSetter_WhenProvided()
		{
			var called = false;
			var property = new ReactiveProperty<int>(0,
				(_, _, _, _) => {});

			property.Changed += _ => called = true;
			property.Value = 3;

			Assert.IsFalse(called);
		}
	}
}