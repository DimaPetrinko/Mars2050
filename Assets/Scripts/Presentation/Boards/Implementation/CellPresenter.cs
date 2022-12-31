using Core.Models.Boards;

namespace Presentation.Boards.Implementation
{
	internal class CellPresenter : ICellPresenter
	{
		public CellPresenter(ICell model, ICellView view)
		{
			Model = model;
			View = view;
		}

		public ICell Model { get; }
		public ICellView View { get; }

		public void Initialize()
		{
			View.Position = Model.Position;
		}
	}
}