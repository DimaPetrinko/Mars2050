using Core.Models.Enums;

namespace Presentation.Actors
{
	internal interface IBuildingView : IPlaceableView
	{
		Faction Faction { set; }
		ResourceType Type { set; }
		int Health { set; }
		int MaxHealth { set; }
	}
}