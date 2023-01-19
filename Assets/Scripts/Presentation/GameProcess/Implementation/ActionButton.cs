using System;
using Core.Configs.Actions.Implementation;
using Core.Models.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.GameProcess.Implementation
{
	[RequireComponent(typeof(Button), typeof(CanvasGroup))]
	internal class ActionButton : MonoBehaviour
	{
		public event Action<ActionType> Clicked;

		[SerializeField] private ActionConfig m_ActionConfig;
		[SerializeField] private GameObject m_SelectedContent;
		[SerializeField] private GameObject m_DeselectedContent;
		[SerializeField][Range(0, 1)] private float m_DisabledAlpha;

		private Button mButton;
		private CanvasGroup mCanvasGroup;

		public ActionType Type => m_ActionConfig.Type;
		public int RequiredOxygen => m_ActionConfig.Oxygen;

		public void ToggleSelected(bool value)
		{
			m_SelectedContent.SetActive(value);
			m_DeselectedContent.SetActive(!value);
		}

		public void ToggleActive(bool value)
		{
			mButton.interactable = value;
			mCanvasGroup.alpha = value ? 1 : m_DisabledAlpha;
		}

		private void Awake()
		{
			mCanvasGroup = GetComponent<CanvasGroup>();
			mButton = GetComponent<Button>();
			mButton.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			Clicked?.Invoke(Type);
		}
	}
}