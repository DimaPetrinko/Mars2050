using System.Linq;
using Core.Models.Actors;
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

			Model.Position.Changed += OnPositionChanged;
		}

		public void Initialize()
		{
			OnPositionChanged(Model.Position.Value);
			View.Faction = Model.Faction;
		}

		private void OnPositionChanged(Vector2Int position)
		{
			View.Cell = mBoardPresenter.GetCellSpot(position);
			View.UpdateRotation(mBoardPresenter.GetCellSpot(Vector2Int.zero).position);
		}
	}
}