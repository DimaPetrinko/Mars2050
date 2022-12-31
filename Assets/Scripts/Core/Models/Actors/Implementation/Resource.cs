using System;
using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors.Implementation
{
	public class Resource : IResource
	{
		public event Action<ICell> CellChanged;
		public event Action IsDiscoveredChanged;

		private bool mIsDiscovered;

		public bool IsDiscovered
		{
			get => mIsDiscovered;
			set
			{
				var changed = mIsDiscovered != value;
				mIsDiscovered = value;
				if (changed) IsDiscoveredChanged?.Invoke();
			}
		}

		public ResourceType Type { get; }

		public Resource(ResourceType type)
		{
			Type = type;
		}

		public void ChangeCell(ICell cell)
		{
			CellChanged?.Invoke(cell);
		}
	}
}