using Core.Models.Boards;
using UnityEngine;

namespace Presentation.Boards
{
	internal interface IBoardPresenter : IPresenter<IBoard, IBoardView>
	{
		Transform GetCellObject(Vector2Int position);
	}
}