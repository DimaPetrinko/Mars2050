using System;
using Core.Models.Boards;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Models.Actors.Implementation
{
	public class BaseBuilding : IBaseBuilding
	{
		public Faction Faction { get; }

		public BaseBuilding(Faction faction)
		{
			Faction = faction;

			mPlaceable = new Placeable();
		}

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
	}
}