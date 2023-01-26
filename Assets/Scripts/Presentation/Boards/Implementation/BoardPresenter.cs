using System;
using System.Linq;
using Core.Models.Boards;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class BoardPresenter : IBoardPresenter
	{
		public event Action<ICell> CellClicked;

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
				ICellPresenter presenter = new CellPresenter(cells[i], view, mPresenterManager);
				presenter.Initialize();
				presenter.Clicked += OnCellClicked;
				mPresenterManager.Register(cells[i], presenter);
				mCells[i] = presenter;
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

		private void OnCellClicked(ICell cell)
		{
			CellClicked?.Invoke(cell);
		}
	}
}