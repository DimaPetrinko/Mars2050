using System;
using System.Collections.Generic;
using Core.Models.Enums;
using Core.Utils;
using Presentation.Actors.Helpers.Implementation;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class ResourceView : MonoBehaviour, IResourceView
	{
		public event Action<bool> Discovered;

		[SerializeField] private PlaceableView m_PlaceableView;
		[SerializeField] private StandingSpotHolder m_StandingSpotHolder;
		[SerializeField] private GameObject m_IconsParent;
		[SerializeField] private Pair<ResourceType, GameObject>[] m_Icons;
		[SerializeField] private bool m_IsDiscovered;

		private bool mLastDiscovered;

		public bool IsOccupied
		{
			set => m_IconsParent.SetActive(!value);
		}

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
			set => m_PlaceableView.Cell = value;
		}

		public Transform DefaultStandingSpot => m_StandingSpotHolder.DefaultStandingSpot;

		public IEnumerable<Transform> AvailableSpots => m_StandingSpotHolder.AvailableSpots;

		private void Update() // TODO: for testing purposes
		{
			if (m_IsDiscovered != mLastDiscovered) Discovered?.Invoke(m_IsDiscovered);
			mLastDiscovered = m_IsDiscovered;
		}
	}
}