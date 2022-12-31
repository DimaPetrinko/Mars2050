using System;
using Core.Models.Enums;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	[Serializable]
	internal struct IconData
	{
		public GameObject Object;
		public ResourceType Type;
	}

	internal class ResourceView : MonoBehaviour, IResourceView
	{
		[SerializeField] private GameObject m_IconsParent;
		[SerializeField] private IconData[] m_Icons;

		public bool IsDiscovered
		{
			set
			{
				m_IconsParent.SetActive(false);
			}
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
	}
}