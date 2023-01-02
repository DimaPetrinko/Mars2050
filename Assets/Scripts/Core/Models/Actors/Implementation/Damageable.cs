using System;
using Core.Implementation;
using UnityEngine;

namespace Core.Models.Actors.Implementation
{
	public class Damageable : IDamageable
	{
		public event Action Died;

		public IReactiveProperty<int> Health { get; }
		public IReactiveProperty<int> MaxHealth { get; }

		public Damageable(int maxHealth)
		{
			MaxHealth = new ReactiveProperty<int>(maxHealth, MaxHealthSetter);
			Health = new ReactiveProperty<int>(maxHealth, HealthSetter);
		}

		public void Damage(int damage)
		{
			if (damage > 0) Health.Value -= damage;
		}

		public void Heal(int amount)
		{
			if (amount > 0) Health.Value += amount;
		}

		private void HealthSetter(int value, int currentValue, Action<int> setValue, Action triggerChanged)
		{
			value = Mathf.Clamp(value, 0, MaxHealth.Value);
			setValue(value);
			if (currentValue != value) triggerChanged();
			if (Health.Value == 0) Died?.Invoke();
		}

		private void MaxHealthSetter(int value, int currentValue, Action<int> setValue, Action triggerChanged)
		{
			setValue(value);
			Health.Value = Health.Value;
			if (currentValue != value) triggerChanged();
		}
	}
}