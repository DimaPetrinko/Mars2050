using Core.Models.Enums;

namespace Core.Models.Actors
{
	public interface IBuilding : IActor, IDamageable, IResourceGatherer
	{
		new IReactiveProperty<Faction> Faction { get; }
		ResourceType ResourceType { get; }
	}
}