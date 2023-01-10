using System;
using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors.Implementation
{
	public class Unit : IUnit
	{
		private readonly int mCombinedHealth;
		private IBuilding mOccupiedBuilding;

		public Faction Faction { get; }

		public Unit(Faction faction, int maxHealth, int combinedHealth)
		{
			Faction = faction;
			mCombinedHealth = combinedHealth;

			mPlaceable = new Placeable();
			mDamageable = new Damageable(maxHealth);
		}

		#region IPlaceable

		private readonly Placeable mPlaceable;

		public event Action<ICell> CellChanged
		{
			add => mPlaceable.CellChanged += value;
			remove => mPlaceable.CellChanged -= value;
		}

		public void ChangeCell(ICell cell)
		{
			mPlaceable.ChangeCell(cell);

			// TODO: should be done in action processor
			if (cell == null)
			{
				RestoreMaxHealth();
				mOccupiedBuilding?.RestoreMaxHealth();
				return;
			}

			if (!cell.TryGetActor(out IBuilding building, Faction)) return;
			mOccupiedBuilding = building;
			MaxHealth.Value = mCombinedHealth;
			building.MaxHealth.Value = mCombinedHealth;
		}

		#endregion

		#region IDamageable

		private readonly Damageable mDamageable;

		public event Action Died
		{
			add => mDamageable.Died += value;
			remove => mDamageable.Died -= value;
		}

		public IReactiveProperty<int> Health => mDamageable.Health;
		public IReactiveProperty<int> MaxHealth => mDamageable.MaxHealth;

		public void Damage(int damage)
		{
			mDamageable.Damage(damage);
		}

		public void Heal(int amount)
		{
			mDamageable.Heal(amount);
		}

		public void RestoreMaxHealth()
		{
			mDamageable.RestoreMaxHealth();
		}

		#endregion
	}
}