using Core.Models.Actors;

namespace Presentation.Actors
{
	internal interface IResourcePresenter : IPresenter<IResource, IResourceView>, IStandingSpotProvider
	{
		bool IsOccupied { set; }
	}
}