using Core.Models.Actors;
using Core.Models.Boards;
using Presentation.Boards;

namespace Presentation.Actors.Implementation
{
	internal class ResourcePresenter : IResourcePresenter
	{
		private readonly IBoardPresenter mBoardPresenter;

		public IResource Model { get; }
		public IResourceView View { get; }

		public ResourcePresenter(IResource model, IResourceView view, IBoardPresenter boardPresenter)
		{
			Model = model;
			View = view;
			mBoardPresenter = boardPresenter;

			Model.CellChanged += OnCellChanged;
			Model.IsDiscoveredChanged += OnIsDiscoveredChanged;
			View.Discovered += OnDiscovered;
		}

		public void Initialize()
		{
			View.IsDiscovered = Model.IsDiscovered;
			View.Type = Model.Type;
			View.Cell = null;
		}

		private void OnCellChanged(ICell cell)
		{
			View.Cell = cell != null ? mBoardPresenter.GetCellObject(cell.Position) : null;
		}

		private void OnDiscovered(bool value)
		{
			Model.IsDiscovered = value;
		}

		private void OnIsDiscoveredChanged()
		{
			View.IsDiscovered = Model.IsDiscovered;
		}
	}
}