using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models.Actors;
using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class BoardTest : MonoBehaviour
	{
		[SerializeField] private BoardView m_BoardPrefab;
		[SerializeField] private ResourceView m_ResourcePrefab;
		[SerializeField] private int m_BoardRadius;

		private IBoard mBoard;
		private IBoardPresenter mBoardPresenter;
		private Dictionary<ResourceType, IResource> mResources;

		private void Awake()
		{
			mBoard = new Board(m_BoardRadius);
			mBoardPresenter = new BoardPresenter(mBoard, m_BoardPrefab);
			mBoardPresenter.Initialize();

			mResources = new Dictionary<ResourceType, IResource>();
			var resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToArray();
			m_ResourcePrefab.gameObject.SetActive(true);
			foreach (var type in resourceTypes)
			{
				var resource = new Resource(type);
				var view = Instantiate(m_ResourcePrefab);
				view.name = type.ToString();
				var presenter = new ResourcePresenter(resource, view, mBoardPresenter);
				presenter.Initialize();
				mResources.Add(type, resource);
			}
			m_ResourcePrefab.gameObject.SetActive(false);
		}

		private void Start()
		{
			mBoard.GetCell(new Vector2Int(3, 0)).AddPlaceable(mResources[ResourceType.Ore]);
			mBoard.GetCell(new Vector2Int(1, 3)).AddPlaceable(mResources[ResourceType.Electricity]);
			mBoard.GetCell(new Vector2Int(0, 1)).AddPlaceable(mResources[ResourceType.Plants]);
			mBoard.GetCell(new Vector2Int(4, -4)).AddPlaceable(mResources[ResourceType.Water]);
		}
	}
}