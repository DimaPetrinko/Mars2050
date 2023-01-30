using System.Linq;
using Core.Models.Actors;
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

			Model.Position.Changed += OnPositionChanged;
			Model.IsDiscovered.Changed += OnIsDiscoveredChanged;
			View.Discovered += OnDiscovered;
		}

		public void Initialize()
		{
			OnPositionChanged(Model.Position.Value);
			OnIsDiscoveredChanged(Model.IsDiscovered.Value);

			View.Type = Model.Type;
		}

		private void OnPositionChanged(Vector2Int position)
		{
			View.Cell = mBoardPresenter.GetCellSpot(position);
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