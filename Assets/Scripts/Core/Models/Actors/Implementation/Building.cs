using System;
using Core.Implementation;
using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors.Implementation
{
	public class Building : IBuilding
	{
		public IReactiveProperty<Faction> Faction { get; }
		Faction IActor.Faction => Faction.Value;

		public ResourceType ResourceType { get; }

		public Building(Faction faction, ResourceType resourceType, int maxHealth)
		{
			Faction = new ReactiveProperty<Faction>(faction);
			ResourceType = resourceType;

			mPlaceable = new Placeable();
			mDamageable = new Damageable(maxHealth);
		}

		#region IPlaceable

		private readonly IPlaceable mPlaceable;

		public event Action<ICell> CellChanged
		{
			add => mPlaceable.CellChanged += value;
			remove => mPlaceable.CellChanged -= value;
		}

		public void ChangeCell(ICell cell)
		{
			mPlaceable.ChangeCell(cell);
			if (cell.TryGetPlaceable(out IResource resource))
			{
				resource.IsOccupied.Value = true;
			}
		}

		#endregion

		#region IDamageable

		private readonly IDamageable mDamageable;

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

		#endregion
	}
}