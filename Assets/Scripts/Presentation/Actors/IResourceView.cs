using System;
using Core.Models.Enums;

namespace Presentation.Actors
{
	internal interface IResourceView : IPlaceableView
	{
		event Action<bool> Discovered;
		bool IsDiscovered { set; }
		ResourceType Type { set; }
	}
}