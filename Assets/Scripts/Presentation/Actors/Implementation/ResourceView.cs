using System;
using Core.Models.Enums;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class ResourceView : MonoBehaviour, IResourceView
	{
		public event Action<bool> Discovered;

		[SerializeField] private GameObject m_IconsParent;
		[SerializeField] private Pair<ResourceType>[] m_Icons;
		[SerializeField] private bool m_IsDiscovered;

		private bool mLastDiscovered;

		public bool IsDiscovered
		{
			set => m_IconsParent.SetActive(value);
		}

		public ResourceType Type
		{
			set
			{
				foreach (var icon in m_Icons) icon.Object.SetActive(icon.Type == value);
			}
		}

		public Transform Cell
		{
			set
			{
				transform.SetParent(value);
				if (value != null)
				{
					gameObject.SetActive(true);
					transform.localPosition = Vector3.zero;
				}
				else gameObject.SetActive(false);
			}
		}

		private void Update()
		{
			if (m_IsDiscovered != mLastDiscovered) Discovered?.Invoke(m_IsDiscovered);
			mLastDiscovered = m_IsDiscovered;
		}
	}
}