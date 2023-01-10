using System;
using Core.Models.Enums;
using Presentation.Actors.Helpers;

namespace Presentation.Actors
{
	internal interface IResourceView : IPlaceableView, IStandingSpotHolder
	{
		event Action<bool> Discovered;
		bool IsOccupied { set; }
		bool IsDiscovered { set; }
		ResourceType Type { set; }
	}
}