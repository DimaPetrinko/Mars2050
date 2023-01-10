using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors
{
	public interface IResource : IPlaceable
	{
		IReactiveProperty<bool> IsDiscovered { get; }
		ResourceType Type { get; }
	}
}