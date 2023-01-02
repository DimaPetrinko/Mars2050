using UnityEngine;

namespace Presentation.Boards
{
	internal interface ICellView : IView
	{
		Transform MainSpot { get; }
		Transform SecondarySpot { get; }
		Vector2Int Position { set; }
	}
}