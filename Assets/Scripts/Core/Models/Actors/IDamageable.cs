using System;

namespace Core.Models.Actors
{
	public interface IDamageable
	{
		event Action Died;

		int Health { get; set; }
		int MaxHealth { get; set; }
		void Damage(int damage);
		void Heal(int amount);
	}
}