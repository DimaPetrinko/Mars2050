using System;
using Core.Models.Boards;
using UnityEngine;

namespace Presentation.Boards
{
	internal interface IBoardPresenter : IPresenter<IBoard, IBoardView>
	{
		event Action<ICell> CellClicked;

		Transform GetCellSpot(Vector2Int position, bool defaultSpot = true);
		bool CellHasBuilding(Vector2Int position);
	}
}