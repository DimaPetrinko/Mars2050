using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs;
using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using Presentation.Actors.Implementation;
using Presentation.Boards;
using Presentation.Boards.Implementation;
using UnityEngine;

namespace Presentation
{
	internal class BoardTest : MonoBehaviour
	{
		[SerializeField] private BoardView m_BoardPrefab;
		[SerializeField] private ResourceView m_ResourcePrefab;
		[SerializeField] private BaseBuildingView m_BaseBuildingPrefab;
		[SerializeField] private GameConfig m_GameConfig;

		private IBoard mBoard;
		private IBoardPresenter mBoardPresenter;
		private Dictionary<ResourceType, IResource> mResources;
		private IBaseBuilding[] mBaseBuildings;

		private void Awake()
		{
			mBoard = new Board(m_GameConfig.BoardRadius);
			mBoardPresenter = new BoardPresenter(mBoard, m_BoardPrefab);
			mBoardPresenter.Initialize();

			mResources = new Dictionary<ResourceType, IResource>();
			var resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToArray();
			foreach (var type in resourceTypes)
			{
				var resource = new Resource(type);
				var view = Instantiate(m_ResourcePrefab);
				view.name = type.ToString();
				var presenter = new ResourcePresenter(resource, view, mBoardPresenter);
				presenter.Initialize();
				mResources.Add(type, resource);
			}

			var factions = Enum.GetValues(typeof(Faction)).Cast<Faction>().Where(f => f != Faction.Any).ToArray();
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

		private void Start()
		{
			mBoard.GetCell(new Vector2Int(3, 0)).AddPlaceable(mResources[ResourceType.Ore]);
			mBoard.GetCell(new Vector2Int(1, 3)).AddPlaceable(mResources[ResourceType.Electricity]);
			mBoard.GetCell(new Vector2Int(0, 1)).AddPlaceable(mResources[ResourceType.Plants]);
			mBoard.GetCell(new Vector2Int(1, -2)).AddPlaceable(mResources[ResourceType.Water]);

			for (var i = 0; i < mBaseBuildings.Length; i++)
			{
				mBoard.GetCell(m_GameConfig.GetStartingConfiguration(mBaseBuildings.Length, i))
					.AddPlaceable(mBaseBuildings[i]);
			}
		}
	}
}