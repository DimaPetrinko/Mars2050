using Core.Models.Enums;
using Presentation.Actors.Helpers.Implementation;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	public class UnitView : MonoBehaviour, IUnitView
	{
		[SerializeField] private ActorView m_Actor;
		[SerializeField] private DamageableView m_Damageable;
		[SerializeField] private PlaceableView m_Placeable;

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
	}
}