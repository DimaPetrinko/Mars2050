using System.Linq;
using Core.Models.Actors;
using Core.Models.Boards;
using Presentation.Boards;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class BaseBuildingPresenter : IBaseBuildingPresenter
	{
		private readonly IBoardPresenter mBoardPresenter;

		public IBaseBuilding Model { get; }
		public IBaseBuildingView View { get; }

		public Transform Spot => View.AvailableSpots.FirstOrDefault();

		public BaseBuildingPresenter(IBaseBuilding model, IBaseBuildingView view, IBoardPresenter boardPresenter)
		{
			Model = model;
			View = view;
			mBoardPresenter = boardPresenter;

			Model.CellChanged += OnCellChanged;
		}

		public void Initialize()
		{
			View.Faction = Model.Faction;
			View.Cell = null;
		}

		private void OnCellChanged(ICell cell)
		{
			View.Cell = mBoardPresenter.GetCellSpot(cell);
			View.UpdateRotation(mBoardPresenter.GetCellSpot(Vector2Int.zero).position);
		}
	}
}