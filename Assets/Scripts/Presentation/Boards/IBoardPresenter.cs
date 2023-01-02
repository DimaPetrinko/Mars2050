using Core.Models.Boards;
using UnityEngine;

namespace Presentation.Boards
{
	internal interface IBoardPresenter : IPresenter<IBoard, IBoardView>
	{
		Transform GetCellSpot(Vector2Int position, bool mainSpot = true);
	}
}