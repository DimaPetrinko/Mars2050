using UnityEngine;

namespace Presentation.Actors.Helpers
{
	internal interface IPlaceableView : IView
	{
		Transform Cell { set; }
	}
}