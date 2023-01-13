using System;
using Core.Models.Boards;

namespace Core.Models.Actors.Implementation
{
	public class Placeable : IPlaceable
	{
		public event Action<ICell> CellChanged;

		public void ChangeCell(ICell cell)
		{
			CellChanged?.Invoke(cell);
		}
	}
}