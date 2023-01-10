using UnityEngine;

namespace Presentation.Actors
{
	internal interface IStandingSpotProvider
	{
		Transform Spot { get; }
	}
}