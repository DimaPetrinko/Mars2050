using Core.Models.Actors;
using Core.Models.Boards;

namespace Presentation.Boards.Implementation
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
		}

		public void Initialize()
		{
			View.Type = Model.Type;
			View.Cell = null;
		}

		private void OnCellChanged(ICell cell)
		{
			View.Cell = cell != null ? mBoardPresenter.GetCellObject(cell.Position) : null;
		}

		private void OnIsDiscoveredChanged()
		{
			View.IsDiscovered = Model.IsDiscovered;
		}
	}
}