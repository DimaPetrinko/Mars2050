using System;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Actions.Implementation
{
	internal abstract class SimpleActionView : MonoBehaviour, ISimpleActionView
	{
		public event Action ResourcesChanged;
		public event Action Confirmed;
		public event Action Closed;

		[SerializeField] private Button m_ConfirmButton;
		[SerializeField] private Button m_CloseButton;
		[SerializeField] private ResourcesInput m_ResourcesInput;
		[SerializeField] private ActionErrorPopup m_ErrorPopup;

		public bool Active
		{
			set => gameObject.SetActive(value);
		}

		public ActionResult Result
		{
			set
			{
				if (value != ActionResult.Success) m_ErrorPopup.Show(value);
				else m_ErrorPopup.Hide();
			}
		}

		public ResourcePackage Resources
		{
			get => m_ResourcesInput.Resources;
			set => m_ResourcesInput.Resources = value;
		}

		public ResourcePackage MaxResources
		{
			set => m_ResourcesInput.MaxResource = value;
		}

		public bool CanConfirm
		{
			set => m_ConfirmButton.interactable = value;
		}

		public bool ResourcesRequired
		{
			set => m_ResourcesInput.ToggleInputs(value);
		}

		private void Awake()
		{
			m_ConfirmButton.onClick.AddListener(OnConfirmClicked);
			m_CloseButton.onClick.AddListener(OnCloseClicked);
			m_ResourcesInput.ResourcesChanged += OnResourcesChanged;
		}

		private void OnConfirmClicked()
		{
			Confirmed?.Invoke();
		}

		private void OnCloseClicked()
		{
			Closed?.Invoke();
		}

		private void OnResourcesChanged()
		{
			ResourcesChanged?.Invoke();
		}
	}
}