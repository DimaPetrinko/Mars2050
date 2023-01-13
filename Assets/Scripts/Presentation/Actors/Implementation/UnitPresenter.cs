using System;
using Core.Models.Actors;
using Core.Models.Boards;
using Presentation.Boards;

namespace Presentation.Actors.Implementation
{
	internal class UnitPresenter : IUnitPresenter
	{
		public event Action<IUnitPresenter> Selected;

		private readonly IBoardPresenter mBoardPresenter;

		private ICell mCell;

		public IUnit Model { get; }
		public IUnitView View { get; }

		public bool IsSelected
		{
			set => View.IsSelected = value;
		}

		public UnitPresenter(IUnit model, IUnitView view, IBoardPresenter boardPresenter)
		{
			mBoardPresenter = boardPresenter;
			Model = model;
			View = view;

			Model.CellChanged += OnCellChanged;
			Model.Health.Changed += OnHealthChanged;
			Model.MaxHealth.Changed += OnMaxHealthChanged;

			View.Selected += OnSelected;
		}

		public void Initialize()
		{
			OnHealthChanged(Model.Health.Value);
			OnMaxHealthChanged(Model.MaxHealth.Value);

			View.Faction = Model.Faction;
			OnCellChanged(null);
			IsSelected = false;
		}

		private void OnCellChanged(ICell cell)
		{
			if (mCell != null)
			{
				mCell.PlaceableAdded -= OnPlaceableAddedOrRemoved;
				mCell.PlaceableRemoved -= OnPlaceableAddedOrRemoved;
			}

			mCell = cell;

			if (mCell != null)
			{
				View.HealthBarShown = !mCell.HasPlaceable<IBuilding>();
				UpdateSpot();
				mCell.PlaceableAdded += OnPlaceableAddedOrRemoved;
				mCell.PlaceableRemoved += OnPlaceableAddedOrRemoved;
			}
			else
			{
				View.Cell = null;
			}

			void OnPlaceableAddedOrRemoved(IPlaceable placeable)
			{
				var cellHasBuilding = mCell.HasPlaceable<IBuilding>();
				View.HealthBarShown = !cellHasBuilding;
				if (cellHasBuilding) UpdateSpot();
			}
		}

		private void UpdateSpot()
		{
			var spot = mBoardPresenter.GetCellSpot(mCell, false);
			View.Cell = spot;
		}

		private void OnHealthChanged(int value)
		{
			View.Health = value;
		}

		private void OnMaxHealthChanged(int value)
		{
			View.MaxHealth = value;
		}

		private void OnSelected()
		{
			Selected?.Invoke(this);
		}
	}
}