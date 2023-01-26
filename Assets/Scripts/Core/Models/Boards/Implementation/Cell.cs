using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models.Actors;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Models.Boards.Implementation
{
	public class Cell : ICell
	{
		public event Action<IPlaceable> PlaceableAdded;
		public event Action<IPlaceable> PlaceableRemoved;

		private readonly List<IPlaceable> mPlaceables = new();

		public Vector2Int Position { get; }

		public Cell(int x, int y)
		{
			Position = new Vector2Int(x, y);
		}

		public Cell(Vector2Int position)
		{
			Position = position;
		}

		public bool HasPlaceable()
		{
			return mPlaceables.Count > 0;
		}

		public bool HasPlaceable<T>() where T : IPlaceable
		{
			return mPlaceables.Any(p => p is T);
		}

		public bool TryGetPlaceable<T>(out T placeable) where T : IPlaceable
		{
			placeable = GetPlaceable<T>();
			return placeable != null;
		}

		public T GetPlaceable<T>() where T : IPlaceable
		{
			return (T)mPlaceables.FirstOrDefault(p => p is T);
		}

		public IPlaceable GetLastNonUnitPlaceable()
		{
			return mPlaceables.LastOrDefault(p => p is not IUnit);
		}

		public bool HasActor(Faction faction = Faction.Any)
		{
			return mPlaceables.Any(p => p is IActor actor && (faction == Faction.Any || actor.Faction == faction));
		}

		public bool HasActor<T>(Faction faction = Faction.Any)
		{
			return mPlaceables.Any(p => p is T && (faction == Faction.Any || ((IActor)p).Faction == faction));
		}

		public bool TryGetActor<T>(out T placeable, Faction faction = Faction.Any)
		{
			placeable = GetActor<T>(faction);
			return placeable != null;
		}

		public T GetActor<T>(Faction faction = Faction.Any)
		{
			return (T)mPlaceables.FirstOrDefault(p =>
				p is T && (faction == Faction.Any || ((IActor)p).Faction == faction));
		}

		public void AddPlaceable(IPlaceable placeable)
		{
			mPlaceables.Add(placeable);
			placeable.ChangeCell(this);
			PlaceableAdded?.Invoke(placeable);
		}

		public void RemovePlaceable(IPlaceable placeable)
		{
			mPlaceables.Remove(placeable);
			placeable.ChangeCell(null);
			PlaceableRemoved?.Invoke(placeable);
		}
	}
}