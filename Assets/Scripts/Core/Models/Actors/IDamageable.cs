using System;

namespace Core.Models.Actors
{
	public interface IDamageable
	{
		event Action Died;

		IReactiveProperty<int> Health { get; }
		IReactiveProperty<int> MaxHealth { get; }
		void Damage(int damage);
		void Heal(int amount);
	}
}