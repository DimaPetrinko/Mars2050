using Core.Models.Enums;
using Presentation.Actors.Helpers;

namespace Presentation.Actors
{
	internal interface IBuildingView : IActorView, IPlaceableView, IDamageableView, IStandingSpotHolder
	{
		ResourceType Type { set; }
	}
}