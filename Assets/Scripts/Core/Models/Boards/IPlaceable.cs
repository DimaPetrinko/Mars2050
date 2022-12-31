using System;

namespace Core.Models.Boards
{
	public interface IPlaceable : IModel
	{
		event Action<ICell> CellChanged;

		void ChangeCell(ICell cell);
	}
}