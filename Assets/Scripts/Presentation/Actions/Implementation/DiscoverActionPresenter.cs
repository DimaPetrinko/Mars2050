using Core.Models.Actions;
using Core.Models.Boards;
using Core.Utils;
using Presentation.Boards;

namespace Presentation.Actions.Implementation
{
	internal class DiscoverActionPresenter
		: SimpleActionPresenter<IDiscoverAction, IDiscoverActionView>,
			IDiscoverActionPresenter
	{
		private readonly IBoardPresenter mBoardPresenter;

		public DiscoverActionPresenter(
			IDiscoverAction model,
			IDiscoverActionView view,
			IBoardPresenter boardPresenter
		) : base(model, view)
		{
			boardPresenter.CellClicked += OnCellClicked;
		}

		private void OnCellClicked(ICell cell)
		{
			Model.Cell = cell;
			var result = Model.CheckTargetCell();
			View.Result = result;
			View.CanConfirm = result.IsSuccess();
		}
	}
}