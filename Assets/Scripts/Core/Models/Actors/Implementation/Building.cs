using System;
using Core.Implementation;
using Core.Models.Boards;
using Core.Models.Enums;
using UnityEngine;

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

		public event Action<IPlaceable> NeighborAdded
		{
			add => mPlaceable.NeighborAdded += value;
			remove => mPlaceable.NeighborAdded -= value;
		}

		public event Action<IPlaceable> NeighborRemoved
		{
			add => mPlaceable.NeighborRemoved += value;
			remove => mPlaceable.NeighborRemoved -= value;
		}

		public IReactiveProperty<Vector2Int> Position => mPlaceable.Position;

		public void OnNewNeighbor(IPlaceable neighbor)
		{
			mPlaceable.OnNewNeighbor(neighbor);
		}

		public void OnNeighborRemoved(IPlaceable neighbor)
		{
			mPlaceable.OnNeighborRemoved(neighbor);
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

		public void RestoreMaxHealth()
		{
			mDamageable.RestoreMaxHealth();
		}

		#endregion
	}
}