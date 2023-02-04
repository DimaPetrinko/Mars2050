using Core.Models.Actions;
using Core.Models.Boards;
using Core.Models.Enums;
using Core.Utils;
using Presentation.Boards;

namespace Presentation.Actions.Implementation
{
	internal class MoveActionPresenter
		: SimpleActionPresenter<IMoveAction, IMoveActionView>,
			IMoveActionPresenter
	{
		private enum CellSelectionState
		{
			Start,
			Destination,
			Both
		}

		private readonly IBoardPresenter mBoardPresenter;

		private CellSelectionState mCellSelectionState;

		public MoveActionPresenter(
			IMoveAction model,
			IMoveActionView view,
			IBoardPresenter boardPresenter
		) : base(model, view)
		{
			mBoardPresenter = boardPresenter;
			mBoardPresenter.CellClicked += OnCellClicked;
		}

		private void OnCellClicked(ICell cell)
		{
			switch (mCellSelectionState)
			{
				case CellSelectionState.Start:
				{
					Model.From = cell;
					var result = Model.CheckStartCell();
					View.Result = result;
					if (result.IsSuccess())
					{
						mCellSelectionState++;
					}

					break;
				}
				case CellSelectionState.Destination:
				{
					Model.To = cell;
					var destinationCheckResult = Model.CheckDestinationCell();

					if (!destinationCheckResult.IsSuccess())
					{
						View.Result = destinationCheckResult;
						break;
					}

					var rangeCheckResult = Model.CheckMoveRange();
					if (!rangeCheckResult.IsSuccess())
					{
						View.Result = rangeCheckResult;
						break;
					}

					View.Result = ActionResult.Success;
					mCellSelectionState++;
					break;
				}
				case CellSelectionState.Both:
				{
					mCellSelectionState = CellSelectionState.Start;
					OnCellClicked(cell);
					break;
				}
			}

			View.CanConfirm = mCellSelectionState == CellSelectionState.Both;
		}

		protected override void ResetData()
		{
			base.ResetData();
			mCellSelectionState = CellSelectionState.Start;
		}
	}
}