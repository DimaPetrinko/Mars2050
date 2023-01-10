using System.Linq;
using Core.Models.Actors;
using Core.Models.Boards;
using Presentation.Boards;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class ResourcePresenter : IResourcePresenter
	{
		private readonly IBoardPresenter mBoardPresenter;

		public IResource Model { get; }
		public IResourceView View { get; }

		public Transform Spot => View.AvailableSpots.FirstOrDefault();

		public bool IsOccupied
		{
			set => View.IsOccupied = value;
		}

		public ResourcePresenter(IResource model, IResourceView view, IBoardPresenter boardPresenter)
		{
			Model = model;
			View = view;
			mBoardPresenter = boardPresenter;

			Model.CellChanged += OnCellChanged;
			Model.IsDiscovered.Changed += OnIsDiscoveredChanged;
			View.Discovered += OnDiscovered;
		}

		public void Initialize()
		{
			OnIsDiscoveredChanged(Model.IsDiscovered.Value);

			View.Type = Model.Type;
			View.Cell = null;
		}

		private void OnCellChanged(ICell cell)
		{
			View.Cell = cell != null ? mBoardPresenter.GetCellSpot(cell) : null;
		}

		private void OnDiscovered(bool value)
		{
			Model.IsDiscovered.Value = value;
		}

		private void OnIsDiscoveredChanged(bool value)
		{
			View.IsDiscovered = value;
		}
	}
}