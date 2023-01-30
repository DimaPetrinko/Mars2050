using System;
using Core.Implementation;
using Core.Models.Boards;
using UnityEngine;

namespace Core.Models.Actors.Implementation
{
	public class Placeable : IPlaceable
	{
		public event Action<IPlaceable> NeighborAdded;
		public event Action<IPlaceable> NeighborRemoved;

		public IReactiveProperty<Vector2Int> Position { get; }

		public Placeable()
		{
			Position = new ReactiveProperty<Vector2Int>();
		}

		public void OnNewNeighbor(IPlaceable neighbor)
		{
			NeighborAdded?.Invoke(neighbor);
		}

		public void OnNeighborRemoved(IPlaceable neighbor)
		{
			NeighborRemoved?.Invoke(neighbor);
		}
	}
}