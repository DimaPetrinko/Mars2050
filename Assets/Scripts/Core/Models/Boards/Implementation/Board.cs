using System.Collections.Generic;
using Core.Utils;
using UnityEngine;

namespace Core.Models.Boards.Implementation
{
	public class Board : IBoard
	{
		private readonly ICell[,] mCells;

		public int Radius { get; }
		public IEnumerable<ICell> Cells => mCells.ToEnumerable();

		public Board(int radius)
		{
			Radius = radius;
			var diameter = Radius * 2 - 1;
			mCells = new ICell[diameter, diameter];
			var offset = Radius - 1;
			for (var x = -offset; x <= offset; x++)
			{
				for (var y = -offset; y <= offset; y++)
				{
					if (!IsValidIndex(x, y)) continue;
					mCells[x + offset, y + offset] = new Cell(x, y);
				}
			}
		}

		public ICell GetCell(Vector2Int position)
		{
			var offset = Radius - 1;
			if (IsValidIndex(position))
				return mCells[position.x + offset, position.y + offset];
			else
			{
				Debug.LogWarning($"Invalid position {position}");
				return null;
			}
		}

		public bool TryGetCell(out ICell cell, Vector2Int position)
		{
			cell = GetCell(position);
			return cell != null;
		}

		public bool InRange(Vector2Int position, int range)
		{
			return InRange(position.x, position.y, range);
		}

		private bool IsValidIndex(Vector2Int position)
		{
			return IsValidIndex(position.x, position.y);
		}
		private bool IsValidIndex(int x, int y)
		{
			return InRange(x, y, Radius);
		}

		private bool InRange(int x, int y, int range)
		{
			return Magnitude(x, y) < range;
		}

		private int Magnitude(int x, int y)
		{
			var z = -x - y;
			return Mathf.Max(Mathf.Abs(x), Mathf.Abs(y), Mathf.Abs(z));
		}
	}
}