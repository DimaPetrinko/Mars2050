using System;
using System.Linq;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.GameProcess.Implementation;
using Core.Utils;
using Presentation.GameProcess.Implementation;
using Presentation.Implementation;
using UnityEngine;

namespace Presentation
{
	internal class PlayerTests : MonoBehaviour
	{
		[SerializeField] private PlayerView m_PlayerViewPrefab;
		[SerializeField] private RectTransform m_PlayersContainer;

		private IPlayer[] mPlayers;
		private IPresenterManager mPresenterManager;
		private int mCurrentPlayer;

		private void Awake()
		{
			mPresenterManager = new PresenterManager();

			var factions = Enum
				.GetValues(typeof(Faction))
				.Cast<Faction>()
				.Where(f => f != Faction.Any)
				.ToArray();

			mPlayers = new IPlayer[factions.Length];
			for (var i = 0; i < mPlayers.Length; i++)
			{
				var model = new Player(factions[i]);
				var view = Instantiate(m_PlayerViewPrefab, m_PlayersContainer);
				view.name = $"{factions[i]} player";
				var presenter = new PlayerPresenter(model, view);
				presenter.Initialize();

				model.HisTurn.Changed += NextPlayer;

				mPlayers[i] = model;
				mPresenterManager.Register(model, presenter);
			}
		}

		private void Start()
		{
			mPlayers[mCurrentPlayer].HisTurn.Value = true;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				mPlayers[mCurrentPlayer].Oxygen.Value++;
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				mPlayers[mCurrentPlayer].Oxygen.Value--;
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				mPlayers[mCurrentPlayer].AddResources(new ResourcePackage(ResourceType.Ore, 1));
			}
			if (Input.GetKeyDown(KeyCode.W))
			{
				mPlayers[mCurrentPlayer].AddResources(new ResourcePackage(ResourceType.Water, 1));
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				mPlayers[mCurrentPlayer].AddResources(new ResourcePackage(ResourceType.Plants, 1));
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				mPlayers[mCurrentPlayer].AddResources(new ResourcePackage(ResourceType.Electricity, 1));
			}
		}

		private void NextPlayer(bool value)
		{
			if (value) return;
			mCurrentPlayer = (mCurrentPlayer + 1) % mPlayers.Length;
			mPlayers[mCurrentPlayer].HisTurn.Value = true;
		}
	}
}