using Core.Models.Actors;

namespace Presentation.Actors
{
	internal interface IBuildingPresenter : IPresenter<IBuilding, IBuildingView>, IStandingSpotProvider
	{
	}
}