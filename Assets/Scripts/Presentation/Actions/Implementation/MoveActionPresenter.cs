using Core.Models.Actions;
using Core.Models.Boards;
using Core.Models.Enums;
using Core.Utils;
using Presentation.Boards;

namespace Presentation.Actions.Implementation
{
	internal class MoveActionPresenter : IMoveActionPresenter
	{
		private enum CellSelectionState
		{
			Start,
			Destination,
			Both
		}

		private readonly IBoardPresenter mBoardPresenter;

		private CellSelectionState mCellSelectionState;
		private bool mRepeat;

		public IMoveAction Model { get; }
		public IMoveActionView View { get; }

		public MoveActionPresenter(IMoveAction model, IMoveActionView view, IBoardPresenter boardPresenter)
		{
			Model = model;
			View = view;

			Model.Selected.Changed += OnSelectedChanged;
			View.Confirmed += OnActionConfirmed;
			View.Closed += OnViewClosed;
			View.ResourcesChanged += OnResourcesChanged;

			mBoardPresenter = boardPresenter;
			mBoardPresenter.CellClicked += OnCellClicked;
		}

		public void Initialize()
		{
			OnSelectedChanged(Model.Selected.Value);
			View.ResourcesRequired = Model.ResourcesRequired;
		}

		private void ResetData()
		{
			View.CanConfirm = false;
			View.Result = ActionResult.Success;
			mCellSelectionState = CellSelectionState.Start;
			mRepeat = false;
			View.Resources = ResourcePackage.Empty();
		}

		private void OnSelectedChanged(bool value)
		{
			View.Active = value;
			ResetData();
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
						Model.From = cell;
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

		private void OnResourcesChanged()
		{
			Model.Resources = View.Resources;
		}

		private void OnActionConfirmed()
		{
			var result = Model.Perform(mRepeat);

			if (!result.IsSuccess()) View.Result = result;
			else
			{
				mRepeat = true;
				if (!Model.Repeatable) OnViewClosed();
			}
		}

		private void OnViewClosed()
		{
			Model.Selected.Value = false;
		}
	}
}