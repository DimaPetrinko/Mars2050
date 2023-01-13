using System;
using Core.Implementation;
using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors.Implementation
{
	public class Resource : IResource
	{
		public IReactiveProperty<bool> IsDiscovered { get; } = new ReactiveProperty<bool>();

		public ResourceType Type { get; }

		public Resource(ResourceType type)
		{
			Type = type;

			mPlaceable = new Placeable();
		}

		#region IPlaceable

		private readonly IPlaceable mPlaceable;

		public event Action<ICell> CellChanged
		{
			add => mPlaceable.CellChanged += value;
			remove => mPlaceable.CellChanged -= value;
		}

		public void ChangeCell(ICell cell)
		{
			mPlaceable.ChangeCell(cell);
		}

		#endregion
	}
}