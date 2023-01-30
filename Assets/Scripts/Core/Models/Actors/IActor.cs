using Core.Models.Enums;

namespace Core.Models.Actors
{
	public interface IActor
	{
		Faction Faction { get; }
	}
}