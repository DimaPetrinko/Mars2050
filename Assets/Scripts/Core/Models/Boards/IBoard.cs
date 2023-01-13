using System.Collections.Generic;
using UnityEngine;

namespace Core.Models.Boards
{
	public interface IBoard : IModel
	{
		int Radius { get; }
		IEnumerable<ICell> Cells { get; }
		ICell GetCell(Vector2Int position);
		bool TryGetCell(out ICell cell, Vector2Int position);
		IEnumerable<ICell> GetCellNeighbors(ICell cell);
	}
}