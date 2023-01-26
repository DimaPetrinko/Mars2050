using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs;
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
		public event Action EndTurnClicked;

		[SerializeField] private GameConfig m_GameConfig;
		[SerializeField] private Button m_EndTurnButton;
		[SerializeField] private Image[] m_FactionBorderParts;
		[SerializeField] private TMP_Text m_Oxygen;
		[SerializeField] private Pair<ResourceType, TMP_Text>[] m_Resources;

		private Dictionary<ResourceType, TMP_Text> mResources;

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

		public short Oxygen
		{
			set => m_Oxygen.text = value.ToString();
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
		}

		private void EndTurnButtonClicked()
		{
			EndTurnClicked?.Invoke();
		}
	}
}