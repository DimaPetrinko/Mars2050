using Core.Models.Enums;
using TMPro;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class BuildingView : MonoBehaviour, IBuildingView
	{
		[SerializeField] private Pair<Faction, GameObject>[] m_FactionObjects;
		[SerializeField] private Pair<ResourceType, GameObject>[] m_BuildingVariants;
		[SerializeField] private TMP_Text m_Health;
		[SerializeField] private TMP_Text m_MaxHealth;

		public Transform Cell
		{
			set
			{
				transform.SetParent(value);
				if (value == null)
				{
					gameObject.SetActive(false);
					return;
				}

				gameObject.SetActive(true);
				transform.localPosition = Vector3.zero;
			}
		}

		public Faction Faction
		{
			set
			{
				foreach (var variant in m_FactionObjects) variant.Object.SetActive(variant.Type == value);
			}
		}

		public ResourceType Type
		{
			set
			{
				foreach (var variant in m_BuildingVariants) variant.Object.SetActive(variant.Type == value);
			}
		}

		public int Health
		{
			set => m_Health.text = value.ToString();
		}

		public int MaxHealth
		{
			set => m_MaxHealth.text = value.ToString();
		}
	}
}