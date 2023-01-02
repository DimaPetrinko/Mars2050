using System.Collections.Generic;
using System.Linq;
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
					var position = new Vector2Int(x, y);
					if (!IsValidIndex(position)) continue;
					mCells[x + offset, y + offset] = new Cell(position);
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

		public IEnumerable<ICell> GetCellNeighbors(ICell cell)
		{
			return Cells.Where(c => c != cell && (c.Position - cell.Position).InRange(1));
		}

		private bool IsValidIndex(Vector2Int position)
		{
			return position.InRange(Radius - 1);
		}
	}
}