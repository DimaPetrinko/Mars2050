using System.Linq;
using Core.Models.Actors;
using Core.Models.Enums;
using Presentation.Boards;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class BuildingPresenter : IBuildingPresenter
	{
		private readonly IBoardPresenter mBoardPresenter;

		public IBuilding Model { get; }
		public IBuildingView View { get; }

		public Transform Spot => View.AvailableSpots.FirstOrDefault();

		public BuildingPresenter(IBuilding model, IBuildingView view, IBoardPresenter boardPresenter)
		{
			Model = model;
			View = view;
			mBoardPresenter = boardPresenter;

			Model.Position.Changed += OnPositionChanged;
			Model.Faction.Changed += OnFactionChanged;
			Model.Health.Changed += OnHealthChanged;
			Model.MaxHealth.Changed += OnMaxHealthChanged;
		}

		public void Initialize()
		{
			OnPositionChanged(Model.Position.Value);
			OnMaxHealthChanged(Model.MaxHealth.Value);
			OnHealthChanged(Model.Health.Value);
			OnFactionChanged(Model.Faction.Value);

			View.Type = Model.ResourceType;
		}

		private void OnPositionChanged(Vector2Int position)
		{
			// TODO: building factory triggers an event. game presenter is subbed to it and occupies the resource
			View.Cell = mBoardPresenter.GetCellSpot(position);
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
	}
}