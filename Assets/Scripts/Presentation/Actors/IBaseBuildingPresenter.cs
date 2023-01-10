using Core.Models.Actors;

namespace Presentation.Actors
{
	internal interface IBaseBuildingPresenter : IPresenter<IBaseBuilding, IBaseBuildingView>, IStandingSpotProvider
	{

	}
}