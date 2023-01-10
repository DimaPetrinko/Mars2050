using System;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Models.Boards
{
	public interface ICell : IModel
	{
		event Action<IPlaceable> PlaceableAdded;
		event Action<IPlaceable> PlaceableRemoved;

		Vector2Int Position { get; }

		bool HasPlaceable();
		bool HasPlaceable<T>() where T : IPlaceable;
		bool TryGetPlaceable<T>(out T placeable) where T : IPlaceable;
		T GetPlaceable<T>() where T : IPlaceable;
		IPlaceable GetLastNonUnitPlaceable();
		bool HasActor(Faction faction = Faction.Any);
		bool HasActor<T>(Faction faction = Faction.Any);
		bool TryGetActor<T>(out T placeable, Faction faction = Faction.Any);
		T GetActor<T>(Faction faction = Faction.Any);
		void AddPlaceable(IPlaceable placeable);
		void RemovePlaceable(IPlaceable placeable);
	}
}