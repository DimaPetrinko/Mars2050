using UnityEngine;

namespace Presentation.Actors
{
	internal interface IPlaceableView : IView
	{
		Transform Cell { set; }
	}
}