using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs;
using Core.Configs.Actions.Enums;
using Core.Configs.Implementation;
using Core.Models.Enums;
using Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.GameProcess.Implementation
{
	internal class PlayerView : MonoBehaviour, IPlayerView
	{
		public event Action<ActionType> ActionClicked;
		public event Action EndTurnClicked;

		[SerializeField] private GameConfig m_GameConfig;
		[SerializeField] private Button m_EndTurnButton;
		[SerializeField] private Image[] m_FactionBorderParts;
		[SerializeField] private TMP_Text m_Oxygen;
		[SerializeField] private Pair<ResourceType, TMP_Text>[] m_Resources;
		[SerializeField] private ActionButton[] m_ActionButtons;

		private Dictionary<ResourceType, TMP_Text> mResources;
		private ActionType? mSelectedAction;
		private int mOxygen;

		public bool Active
		{
			set => gameObject.SetActive(value);
		}

		public Faction Faction
		{
			set
			{
				var color = Config.GetUIColorForFaction(value);
				foreach (var part in m_FactionBorderParts) part.color = color;
			}
		}

		public int Oxygen
		{
			set
			{
				m_Oxygen.text = value.ToString();
				mOxygen = value;
				UpdateActionButtons();
			}
		}

		public ActionType? SelectedAction
		{
			set
			{
				mSelectedAction = value;
				UpdateActionButtons();
			}
		}

		private void UpdateActionButtons()
		{
			foreach (var button in m_ActionButtons)
			{
				var correctType = button.Type == mSelectedAction;
				button.ToggleActive((!mSelectedAction.HasValue || correctType) && mOxygen >= button.RequiredOxygen);
				button.ToggleSelected(mSelectedAction.HasValue && correctType);
			}
		}

		private IGameConfig Config => m_GameConfig;

		public void UpdateResource(ResourceType type, int amount)
		{
			mResources[type].text = amount.ToString();
		}

		private void Awake()
		{
			mResources = m_Resources
				.ToDictionary(p => p.Type, p => p.Object);
			m_EndTurnButton.onClick.AddListener(EndTurnButtonClicked);

			foreach (var button in m_ActionButtons) button.Clicked += OnActionClicked;
		}

		private void OnActionClicked(ActionType type)
		{
			ActionClicked?.Invoke(type);
		}

		private void EndTurnButtonClicked()
		{
			EndTurnClicked?.Invoke();
		}
	}
}