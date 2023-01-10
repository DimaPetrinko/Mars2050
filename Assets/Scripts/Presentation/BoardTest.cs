using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs.Implementation;
using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Utils;
using Presentation.Actors;
using Presentation.Actors.Implementation;
using Presentation.Boards;
using Presentation.Boards.Implementation;
using Presentation.Implementation;
using UnityEngine;
using Camera = Core.Models.GameProcess.Implementation.Camera;
using Random = UnityEngine.Random;

namespace Presentation
{
	internal class BoardTest : MonoBehaviour
	{
		[SerializeField] private CameraView m_CameraView;
		[SerializeField] private BoardView m_BoardPrefab;
		[SerializeField] private ResourceView m_ResourcePrefab;
		[SerializeField] private BaseBuildingView m_BaseBuildingPrefab;
		[SerializeField] private BuildingView m_BuildingPrefab;
		[SerializeField] private UnitView m_UnitView;
		[SerializeField] private GameConfig m_GameConfig;

		private IPresenterManager mPresenterProvider;
		private IBoard mBoard;
		private ICamera mCamera;
		private IBoardPresenter mBoardPresenter;
		private ICameraPresenter mCameraPresenter;
		private List<IResourcePresenter> mResources;
		private IBaseBuilding[] mBaseBuildings;
		private Dictionary<IUnitPresenter, Faction> mUnits;
		private Dictionary<ResourceType, IBuilding> mBuildings;

		private void Awake()
		{
			mPresenterProvider = new PresenterManager();
			mBoard = new Board(m_GameConfig.BoardRadius);
			var boardView = Instantiate(m_BoardPrefab);
			mBoardPresenter = new BoardPresenter(mBoard, boardView, mPresenterProvider);
			mBoardPresenter.Initialize();

			mCamera = new Camera(
				mBoard.Radius * m_GameConfig.CellRadius * 3 / 2,
				m_GameConfig.CameraConfig.ZoomLimits);
			mCameraPresenter = new CameraPresenter(mCamera, m_CameraView);
			mCameraPresenter.Initialize();

			CreateResources();
			CreateBases();
			CreateUnits();
			CreateBuildings();
		}

		private void Start()
		{
			for (var i = 0; i < mBaseBuildings.Length; i++)
			{
				mBoard.GetCell(m_GameConfig.GetStartingConfiguration(mBaseBuildings.Length, i))
					.AddPlaceable(mBaseBuildings[i]);
			}

			var basePositions = mBoard.Cells
				.Where(c => c.HasPlaceable<IBaseBuilding>())
				.Select(c => new { c.GetPlaceable<IBaseBuilding>().Faction, c.Position })
				.ToDictionary(p => p.Faction, p => p.Position);
			foreach (var pair in mUnits)
			{
				mBoard.GetCell(basePositions[pair.Value]).AddPlaceable(pair.Key.Model);
			}

			var eligiblePositions = mBoard.Cells
				.Where(c =>
					!c.HasPlaceable<IBaseBuilding>()
					&& mBoard.GetCellNeighbors(c).All(n => !n.HasPlaceable<IBaseBuilding>())
					&& !c.HasPlaceable<IResource>()
					&& mBoard.GetCellNeighbors(c).All(n => !n.HasPlaceable<IResource>()))
				.Select(c => c.Position);
			foreach (var resource in mResources)
			{
				var positions = eligiblePositions.ToArray();
				if (positions.Length == 0) continue;
				var randomIndex = Random.Range(0, positions.Length);
				mBoard.GetCell(positions[randomIndex]).AddPlaceable(resource.Model);
				resource.Model.IsDiscovered.Value = true;
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
				if (cell == null) continue;
				cell.AddPlaceable(pair.Value);
				var resource = cell.GetPlaceable<IResource>();
				var resourcePresenter = mResources.FirstOrDefault(p => p.Model == resource);
				if (resourcePresenter == null) continue;
				resourcePresenter.IsOccupied = true;
			}

			var faction = Faction.Red;
			var unit = mUnits
				.Where(p => p.Value == faction)
				.Select(p => p.Key)
				.FirstOrDefault().Model;

			var building = mBuildings.Values.FirstOrDefault(b => b.Faction.Value == faction);
			var buildingCell = mBoard.Cells.FirstOrDefault(
				c => c.TryGetActor(out IBuilding found, faction) && found == building);
			var baseCell = mBoard.Cells.FirstOrDefault(c => c.HasActor<IBaseBuilding>(faction));

			baseCell.RemovePlaceable(unit);
			buildingCell.AddPlaceable(unit);
		}

		private void CreateResources()
		{
			mResources = new List<IResourcePresenter>();
			var resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToArray();
			var counts = new Dictionary<ResourceType, int>
			{
				{ResourceType.Ore, 0},
				{ResourceType.Electricity, 0},
				{ResourceType.Plants, 0},
				{ResourceType.Water, 0}
			};
			var maxCounts = counts.Keys.ToDictionary(r => r,
				r => Mathf.CeilToInt(m_GameConfig.GetResourceRatio(r) * m_GameConfig.MaxResources));

			for (var i = 0; i < m_GameConfig.MaxResources; i++)
			{
				ResourceType type;
				var tries = 0;
				const int maxTries = 10;
				do
				{
					var resourceIndex = Random.Range(0, resourceTypes.Length);
					type = resourceTypes[resourceIndex];
					tries++;
				} while (counts[type] >= maxCounts[type] && tries < maxTries);

				var resource = new Resource(type);
				var view = Instantiate(m_ResourcePrefab);
				view.name = type.ToString();
				var presenter = new ResourcePresenter(resource, view, mBoardPresenter);
				presenter.Initialize();
				mPresenterProvider.Register(resource, presenter);
				mResources.Add(presenter);
				counts[type]++;
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
				mPresenterProvider.Register(baseBuilding, presenter);
				mBaseBuildings[i] = baseBuilding;
			}
		}

		private void CreateUnits()
		{
			mUnits = new Dictionary<IUnitPresenter, Faction>();

			var factions = Enum.GetValues(typeof(Faction))
				.Cast<Faction>()
				.Where(f => f != Faction.Any)
				.ToArray();
			foreach (var faction in factions)
			{
				for (var i = 0; i < m_GameConfig.UnitsPerPlayer; i++)
				{
					var unit = new Unit(faction, m_GameConfig.MaxUnitHealth, m_GameConfig.MaxBuildingWithUnitHealth);
					var view = Instantiate(m_UnitView);
					var presenter = new UnitPresenter(unit, view, mBoardPresenter);
					presenter.Initialize();
					mPresenterProvider.Register(unit, presenter);
					mUnits.Add(presenter, faction);
					presenter.Selected += OnUnitSelected;
				}
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
			mPresenterProvider.Register(building, presenter);
			mBuildings.Add(type, building);
		}

		private void OnUnitSelected(IUnitPresenter unit)
		{
			foreach (var u in mUnits.Keys)
			{
				u.IsSelected = unit == u;
			}
		}
	}
}