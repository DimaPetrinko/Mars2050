using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs;
using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using Core.Utils;
using Presentation.Actors.Implementation;
using Presentation.Boards;
using Presentation.Boards.Implementation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Presentation
{
	internal class BoardTest : MonoBehaviour
	{
		[SerializeField] private BoardView m_BoardPrefab;
		[SerializeField] private ResourceView m_ResourcePrefab;
		[SerializeField] private BaseBuildingView m_BaseBuildingPrefab;
		[SerializeField] private BuildingView m_BuildingPrefab;
		[SerializeField] private GameConfig m_GameConfig;

		private IBoard mBoard;
		private IBoardPresenter mBoardPresenter;
		private List<IResource> mResources;
		private IBaseBuilding[] mBaseBuildings;
		private Dictionary<ResourceType, IBuilding> mBuildings;

		private void Awake()
		{
			mBoard = new Board(m_GameConfig.BoardRadius);
			mBoardPresenter = new BoardPresenter(mBoard, m_BoardPrefab);
			mBoardPresenter.Initialize();

			CreateResources();
			CreateBases();
			CreateBuildings();
		}

		private void Start()
		{
			for (var i = 0; i < mBaseBuildings.Length; i++)
			{
				mBoard.GetCell(m_GameConfig.GetStartingConfiguration(mBaseBuildings.Length, i))
					.AddPlaceable(mBaseBuildings[i]);
			}

			var cellsPositions = mBoard.Cells
				.Where(c =>
					!c.HasPlaceable<IBaseBuilding>()
					&& mBoard.GetCellNeighbors(c).All(n => !n.HasPlaceable<IBaseBuilding>())
					&& !c.HasPlaceable<IResource>()
					&& mBoard.GetCellNeighbors(c).All(n => !n.HasPlaceable<IResource>()))
				.Select(c => c.Position);
			foreach (var resource in mResources)
			{
				var positions = cellsPositions.ToArray();
				if (positions.Length == 0) continue;
				var randomIndex = Random.Range(0, positions.Length);
				mBoard.GetCell(positions[randomIndex]).AddPlaceable(resource);
				resource.IsDiscovered.Value = true;
			}

			foreach (var pair in mBuildings)
			{
				var cell = mBoard.Cells
					.Where(c =>
						c.TryGetPlaceable(out IResource resource)
						&& resource.Type == pair.Key)
					.OrderBy(c =>
						(c.Position - mBoard.Cells
							.First(bc => bc.HasActor<IBaseBuilding>(pair.Value.Faction.Value))
							.Position)
						.Magnitude())
					.FirstOrDefault();
				cell?.AddPlaceable(pair.Value);
			}
		}

		private void CreateResources()
		{
			mResources = new List<IResource>();
			var resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToArray();
			var resourcesCount = Random.Range(14, 15);
			for (var i = 0; i < resourcesCount; i++)
			{
				var resourceIndex = Random.Range(0, resourceTypes.Length);
				var type = resourceTypes[resourceIndex];

				var resource = new Resource(type);
				var view = Instantiate(m_ResourcePrefab);
				view.name = type.ToString();
				var presenter = new ResourcePresenter(resource, view, mBoardPresenter);
				presenter.Initialize();
				mResources.Add(resource);
			}
		}

		private void CreateBases()
		{
			var factions = Enum.GetValues(typeof(Faction))
				.Cast<Faction>()
				.Where(f => f != Faction.Any)
				.ToArray();
			mBaseBuildings = new IBaseBuilding[factions.Length];
			for (var i = 0; i < factions.Length; i++)
			{
				var baseBuilding = new BaseBuilding(factions[i]);
				var view = Instantiate(m_BaseBuildingPrefab);
				view.name = $"{factions[i]}Base";
				var presenter = new BaseBuildingPresenter(baseBuilding, view, mBoardPresenter);
				presenter.Initialize();
				mBaseBuildings[i] = baseBuilding;
			}
		}

		private void CreateBuildings()
		{
			mBuildings = new Dictionary<ResourceType, IBuilding>();
			CreateBuilding(Faction.Red, ResourceType.Water);
			CreateBuilding(Faction.Green, ResourceType.Electricity);
			CreateBuilding(Faction.Yellow, ResourceType.Ore);
			CreateBuilding(Faction.Blue, ResourceType.Plants);
		}

		private void CreateBuilding(Faction faction, ResourceType type)
		{
			var building = new Building(faction, type, m_GameConfig.MaxBuildingHealth);
			var view = Instantiate(m_BuildingPrefab);
			view.name = $"{type}Building";
			var presenter = new BuildingPresenter(building, view, mBoardPresenter);
			presenter.Initialize();
			mBuildings.Add(type, building);
		}
	}
}