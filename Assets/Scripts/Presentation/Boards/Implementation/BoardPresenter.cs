using System.Linq;
using Core.Models.Boards;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class BoardPresenter : IBoardPresenter
	{
		private readonly ICellPresenter[] mCells;

		public IBoard Model { get; }
		public IBoardView View { get; }

		public BoardPresenter(IBoard model, BoardView boardPrefab)
		{
			Model = model;
			View = Object.Instantiate(boardPrefab);

			mCells = new ICellPresenter[Model.Cells.Count()];
		}

		public void Initialize()
		{
			var cells = Model.Cells.ToArray();
			for (var i = 0; i < mCells.Length; i++)
			{
				var view = View.CreateCell();
				mCells[i] = new CellPresenter(cells[i], view);
				mCells[i].Initialize();
			}
		}

		public Transform GetCellSpot(Vector2Int position, bool mainSpot = true)
		{
			var cell = Model.GetCell(position);
			if (cell == null) return null;

			var view = mCells.FirstOrDefault(c => c.Model == cell)?.View;
			var spot = mainSpot ? view?.MainSpot : view?.SecondarySpot;
			return spot;
		}
	}
}