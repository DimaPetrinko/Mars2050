using UnityEngine;

namespace Presentation.Boards
{
	internal interface ICellView : IView
	{
		Transform Transform { get; }
		Vector2Int Position { set; }
	}
}