using Core.Models.Actors;
using Core.Models.Boards;
using Presentation.Boards;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class UnitPresenter : IUnitPresenter
	{
		private readonly IBoardPresenter mBoardPresenter;
		private Vector2Int mPosition;

		public IUnit Model { get; }
		public IUnitView View { get; }

		public UnitPresenter(IUnit model, IUnitView view, IBoardPresenter boardPresenter)
		{
			mBoardPresenter = boardPresenter;
			Model = model;
			View = view;

			Model.Position.Changed += OnPositionChanged;
			Model.NeighborAdded += OnNeighborAddedOrRemoved;
			Model.NeighborRemoved += OnNeighborAddedOrRemoved;
			Model.Health.Changed += OnHealthChanged;
			Model.MaxHealth.Changed += OnMaxHealthChanged;
		}

		public void Initialize()
		{
			OnHealthChanged(Model.Health.Value);
			OnMaxHealthChanged(Model.MaxHealth.Value);

			View.Faction = Model.Faction;
			OnPositionChanged(Model.Position.Value);
		}

		private void OnPositionChanged(Vector2Int position)
		{
			mPosition = position;
			View.HealthBarShown = !mBoardPresenter.CellHasBuilding(mPosition);
			View.Cell = mBoardPresenter.GetCellSpot(mPosition, false);
		}

		private void OnNeighborAddedOrRemoved(IPlaceable neighbor)
		{
			var cellHasBuilding = neighbor is IBuilding;
			View.HealthBarShown = !cellHasBuilding;
			if (cellHasBuilding) View.Cell = mBoardPresenter.GetCellSpot(mPosition, false);
		}

		private void OnHealthChanged(int value)
		{
			View.Health = value;
		}

		private void OnMaxHealthChanged(int value)
		{
			View.MaxHealth = value;
		}
	}
}