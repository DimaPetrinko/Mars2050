using Presentation.Actors;
using UnityEngine;

namespace Presentation.Boards
{
	internal interface ICellView : IView, IStandingSpotHolder
	{
		Vector2Int Position { set; }
	}
}