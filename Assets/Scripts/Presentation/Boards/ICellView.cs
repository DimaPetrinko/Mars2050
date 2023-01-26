using System;
using Presentation.Actors;
using UnityEngine;

namespace Presentation.Boards
{
	internal interface ICellView : IView, IStandingSpotHolder
	{
		event Action Clicked;

		Vector2Int Position { set; }
	}
}