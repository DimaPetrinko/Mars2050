using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Actions.Implementation
{
	[RequireComponent(typeof(Button), typeof(CanvasGroup))]
	internal class ActionButtonView : MonoBehaviour, IActionButtonView
	{
		public event Action Clicked;

		[SerializeField] private TMP_Text m_NameText;
		[SerializeField] private GameObject m_SelectedContent;
		[SerializeField] private GameObject m_DeselectedContent;
		[SerializeField][Range(0, 1)] private float m_DisabledAlpha;

		private Button mButton;
		private CanvasGroup mCanvasGroup;

		public string Name
		{
			set => m_NameText.text = value;
		}

		public bool Selected
		{
			set
			{
				m_SelectedContent.SetActive(value);
				m_DeselectedContent.SetActive(!value);
			}
		}

		public bool Active
		{
			set
			{
				mButton.interactable = value;
				mCanvasGroup.alpha = value ? 1 : m_DisabledAlpha;
			}
		}

		private void Awake()
		{
			mCanvasGroup = GetComponent<CanvasGroup>();
			mButton = GetComponent<Button>();
			mButton.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			Clicked?.Invoke();
		}
	}
}