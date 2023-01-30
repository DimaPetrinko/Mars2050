using System;
using UnityEngine;

namespace Core.Models.Boards
{
	public interface IPlaceable : IModel
	{
		event Action<IPlaceable> NeighborAdded;
		event Action<IPlaceable> NeighborRemoved;

		IReactiveProperty<Vector2Int> Position { get; }
		void OnNewNeighbor(IPlaceable neighbor);
		void OnNeighborRemoved(IPlaceable neighbor);
	}
}