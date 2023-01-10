using System.Collections.Generic;
using Core.Models.Enums;
using Core.Utils;
using Presentation.Actors.Helpers.Implementation;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class BuildingView : MonoBehaviour, IBuildingView
	{
		[SerializeField] private ActorView m_Actor;
		[SerializeField] private DamageableView m_Damageable;
		[SerializeField] private PlaceableView m_Placeable;
		[SerializeField] private StandingSpotHolder m_StandingSpotHolder;
		[SerializeField] private Pair<ResourceType, GameObject>[] m_BuildingVariants;

		public ResourceType Type
		{
			set
			{
				foreach (var variant in m_BuildingVariants) variant.Object.SetActive(variant.Type == value);
			}
		}

		public Faction Faction
		{
			set => m_Actor.Faction = value;
		}

		public int Health
		{
			set => m_Damageable.Health = value;
		}

		public int MaxHealth
		{
			set => m_Damageable.MaxHealth = value;
		}

		public bool HealthBarShown
		{
			set => m_Damageable.HealthBarShown = value;
		}

		public Transform Cell
		{
			set => m_Placeable.Cell = value;
		}

		public Transform DefaultStandingSpot => m_StandingSpotHolder.DefaultStandingSpot;

		public IEnumerable<Transform> AvailableSpots => m_StandingSpotHolder.AvailableSpots;
	}
}