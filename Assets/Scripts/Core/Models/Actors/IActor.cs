using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors
{
	public interface IActor : IPlaceable
	{
		Faction Faction { get; }
	}
}