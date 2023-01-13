using System.Linq;
using Core.Models.Boards;
using Presentation.Actors;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class CellPresenter : ICellPresenter
	{
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
		}

		public void Initialize()
		{
			View.Position = Model.Position;
			Spot = View.AvailableSpots.FirstOrDefault();
		}

		private void OnPlaceableAddedOrRemoved(IPlaceable placeable)
		{
			var presenter = mPresenterManager.Get(Model.GetLastNonUnitPlaceable());
			if (presenter is not IStandingSpotProvider spotProvider) return;
			Spot = spotProvider.Spot;
		}
	}
}