using System;
using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors.Implementation
{
	public class BaseBuilding : IBaseBuilding
	{
		public event Action<ICell> CellChanged;

		public Faction Faction { get; }

		public BaseBuilding(Faction faction)
		{
			Faction = faction;
		}

		public void ChangeCell(ICell cell)
		{
			CellChanged?.Invoke(cell);
		}
	}
}