using System.Linq;
using Core.Models.Boards;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class BoardPresenter : IBoardPresenter
	{
		private readonly ICellPresenter[] mCells;
		private readonly IPresenterManager mPresenterManager;

		public IBoard Model { get; }
		public IBoardView View { get; }

		public BoardPresenter(IBoard model, IBoardView view, IPresenterManager presenterManager)
		{
			Model = model;
			View = view;
			mPresenterManager = presenterManager;

			mCells = new ICellPresenter[Model.Cells.Count()];
		}

		public void Initialize()
		{
			var cells = Model.Cells.ToArray();
			for (var i = 0; i < mCells.Length; i++)
			{
				var view = View.CreateCell();
				mCells[i] = new CellPresenter(cells[i], view, mPresenterManager);
				mCells[i].Initialize();
				mPresenterManager.Register(cells[i], mCells[i]);
			}
		}

		public Transform GetCellSpot(ICell cell, bool defaultSpot = true)
		{
			if (cell == null) return null;
			var cellPresenter = GetCellPresenter(cell);
			if (cellPresenter == null) return null;
			return defaultSpot ? cellPresenter.View.DefaultStandingSpot : cellPresenter.Spot;
		}

		public Transform GetCellSpot(Vector2Int position, bool defaultSpot = true)
		{
			return GetCellSpot(Model.GetCell(position), defaultSpot);
		}

		private ICellPresenter GetCellPresenter(ICell cell)
		{
			return mCells.FirstOrDefault(c => c.Model == cell);
		}
	}
}