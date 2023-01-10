using Presentation.Actors.Helpers;
using UnityEngine;

namespace Presentation.Actors
{
	internal interface IBaseBuildingView : IActorView, IPlaceableView, IStandingSpotHolder
	{
		void UpdateRotation(Vector3 zeroCellPosition);
	}
}