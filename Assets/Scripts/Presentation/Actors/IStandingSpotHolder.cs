using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Actors
{
	internal interface IStandingSpotHolder
	{
		Transform DefaultStandingSpot { get; }
		IEnumerable<Transform> AvailableSpots { get; }
	}
}