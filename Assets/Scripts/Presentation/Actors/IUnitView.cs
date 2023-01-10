using System;
using Presentation.Actors.Helpers;

namespace Presentation.Actors
{
	internal interface IUnitView : IActorView, IPlaceableView, IDamageableView
	{
		event Action Selected;

		bool IsSelected { set; }
	}
}