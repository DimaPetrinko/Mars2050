using Core.Models.Actors;
using Core.Models.Boards;
using Core.Models.Enums;
using Presentation.Boards;

namespace Presentation.Actors.Implementation
{
	internal class BuildingPresenter : IBuildingPresenter
	{
		private readonly IBoardPresenter mBoardPresenter;

		public IBuilding Model { get; }
		public IBuildingView View { get; }

		public BuildingPresenter(IBuilding model, IBuildingView view, IBoardPresenter boardPresenter)
		{
			Model = model;
			View = view;
			mBoardPresenter = boardPresenter;

			Model.CellChanged += OnCellChanged;
			Model.Faction.Changed += OnFactionChanged;
			Model.Health.Changed += OnHealthChanged;
			Model.MaxHealth.Changed += OnMaxHealthChanged;
		}

		private void OnCellChanged(ICell cell)
		{
			View.Cell = mBoardPresenter.GetCellSpot(cell.Position);
		}

		private void OnFactionChanged(Faction value)
		{
			View.Faction = value;
		}

		private void OnHealthChanged(int value)
		{
			View.Health = value;
		}

		private void OnMaxHealthChanged(int value)
		{
			View.MaxHealth = value;
		}

		public void Initialize()
		{
			OnMaxHealthChanged(Model.MaxHealth.Value);
			OnHealthChanged(Model.Health.Value);
			OnFactionChanged(Model.Faction.Value);
			View.Cell = null;
			View.Type = Model.ResourceType;
		}
	}
}