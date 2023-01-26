using System;
using System.Linq;
using Core.Models.Boards;
using Presentation.Actors;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class CellPresenter : ICellPresenter
	{
		public event Action<ICell> Clicked;

		private readonly IPresenterManager mPresenterManager;

		public ICell Model { get; }
		public ICellView View { get; }

		public Transform Spot { get; private set; }

		public CellPresenter(ICell model, ICellView view, IPresenterManager presenterManager)
		{
			Model = model;
			View = view;
			mPresenterManager = presenterManager;

			Model.PlaceableAdded += OnPlaceableAddedOrRemoved;
			Model.PlaceableRemoved += OnPlaceableAddedOrRemoved;
			View.Clicked += OnClicked;
		}

		public void Initialize()
		{
			View.Position = Model.Position;
			Spot = View.AvailableSpots.FirstOrDefault();
		}

		private void OnPlaceableAddedOrRemoved(IPlaceable placeable)
		{
			// TODO: won't assignSpot if last is not placeable
			var presenter = mPresenterManager.Get(Model.GetLastNonUnitPlaceable());
			if (presenter is not IStandingSpotProvider spotProvider) return;
			Spot = spotProvider.Spot;
		}

		private void OnClicked()
		{
			Clicked?.Invoke(Model);
		}
	}
}