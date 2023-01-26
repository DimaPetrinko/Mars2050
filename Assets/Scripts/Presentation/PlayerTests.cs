using System;
using System.Linq;
using Core.Configs.Actions.Implementation;
using Core.Configs.Implementation;
using Core.Models.Actions;
using Core.Models.Actions.Implementation;
using Core.Models.Actors.Implementation;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.GameProcess.Implementation;
using Core.Utils;
using Presentation.Actions.Implementation;
using Presentation.Boards;
using Presentation.GameProcess.Implementation;
using Presentation.Implementation;
using UnityEngine;

namespace Presentation
{
	internal interface IGamePresenter
	{
		IAction SelectedAction { get; }
	}

	internal class PlayerTests : MonoBehaviour, IGamePresenter
	{
		private class FakeBoardPresenter : IBoardPresenter
		{
			public event Action<ICell> CellClicked;

			public IBoard Model { get; }
			public IBoardView View { get; }

			public void ClickOnCell(ICell cell)
			{
				CellClicked?.Invoke(cell);
			}

			public void Initialize()
			{
			}

			public Transform GetCellSpot(ICell cell, bool defaultSpot = true)
			{
				return null;
			}

			public Transform GetCellSpot(Vector2Int position, bool defaultSpot = true)
			{
				return null;
			}
		}

		[SerializeField] private GameConfig m_GameConfig;
		[SerializeField] private ActionConfigs m_ActionsConfigs;
		[SerializeField] private PlayerView[] m_PlayerViews;
		[SerializeField] private ActionButtonView[] m_ActionButtonViews;
		[SerializeField] private MoveActionView m_MoveActionView;
		[SerializeField] private DiceView m_DiceView;
		[SerializeField] private Vector2Int m_FromCell;
		[SerializeField] private Vector2Int m_ToCell;

		private readonly FakeBoardPresenter mFakeBoardPresenter = new();

		private IDice mDice;
		private IPlayer[] mPlayers;
		private IAction[] mActions;
		private IPresenterManager mPresenterManager;
		private int mCurrentPlayer;

		public IAction SelectedAction => mActions.FirstOrDefault(a => a.Selected.Value);

		private void Awake()
		{
			mPresenterManager = new PresenterManager();

			mDice = new Dice(m_GameConfig.MaxRoll);
			var dicePresenter = new DicePresenter(mDice, m_DiceView);
			mPresenterManager.Register(mDice, dicePresenter);

			mDice.Rolled += OnDiceRolled;

			foreach (var view in m_PlayerViews) view.gameObject.SetActive(false);

			var factions = Enum
				.GetValues(typeof(Faction))
				.Cast<Faction>()
				.Where(f => f != Faction.Any)
				.ToArray();

			mPlayers = new IPlayer[factions.Length];
			for (var i = 0; i < mPlayers.Length; i++)
			{
				var model = new Player(factions[i]);
				var view = m_PlayerViews[i];
				view.name = $"{factions[i]} player";
				var presenter = new PlayerPresenter(model, view);

				model.HisTurn.Changed += NextPlayer;

				mPlayers[i] = model;
				mPresenterManager.Register(model, presenter);
			}

			var actionTypes = Enum.GetValues(typeof(ActionType))
				.Cast<ActionType>()
				.ToArray();

			IActionFactory actionFactory = new ActionFactory(m_GameConfig, m_ActionsConfigs, mDice);
			mActions = new IAction[actionTypes.Length];
			for (var i = 0; i < mActions.Length; i++)
			{
				var model = actionFactory.Create(ActionType.Move);
				var view = m_ActionButtonViews[i];
				view.name = $"{actionTypes[i]} button";
				if (model == null)
				{
					view.gameObject.SetActive(false);
					continue;
				}

				var presenter = new ActionButtonPresenter(model, m_ActionButtonViews[i], this, m_ActionsConfigs);

				model.Selected.Changed += OnActionSelectedChanged;

				mActions[i] = model;
				mPresenterManager.Register(model, presenter);

				void OnActionSelectedChanged(bool value)
				{
					OnActionSelected(value ? model : null);
				}
			}

			var moveActionPresenter = new MoveActionPresenter(
				(IMoveAction)mActions[0],
				m_MoveActionView,
				mFakeBoardPresenter);
			mPresenterManager.Register((IMoveAction)mActions[0], moveActionPresenter);
		}

		private void Start()
		{
			mPresenterManager.InitializeAll();
			StartPlayerTurn(mCurrentPlayer);
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
			if (Input.GetKeyDown(KeyCode.F))
			{
				var cell = new Cell(m_FromCell);
				cell.AddPlaceable(new Unit(mPlayers[mCurrentPlayer].Faction, m_GameConfig.MaxUnitHealth));
				mFakeBoardPresenter.ClickOnCell(cell);
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				mFakeBoardPresenter.ClickOnCell(new Cell(m_ToCell));
			}
		}

		private void OnActionSelected(IAction value)
		{
			foreach (var action in mActions.Where(a => a != value)) action.Selected.Value = false;
		}

		private void OnDiceRolled(byte roll)
		{
			// TODO: figure out a way to show the change after the animation
			mPlayers[mCurrentPlayer].Oxygen.Value += roll;
			// TODO: for testing
			foreach (var action in mActions) action.Selected.Value = false;
		}

		private void NextPlayer(bool hisTurn)
		{
			if (hisTurn) return;
			mCurrentPlayer = (mCurrentPlayer + 1) % mPlayers.Length;
			StartPlayerTurn(mCurrentPlayer);
		}

		private void StartPlayerTurn(int currentPlayer)
		{
			mDice.Roll();
			mPlayers[currentPlayer].HisTurn.Value = true;
			foreach (var action in mActions) action.Performer = mPlayers[mCurrentPlayer];
		}
	}
}