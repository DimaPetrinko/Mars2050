using System.Collections.Generic;
using Core.Models.Enums;
using Presentation.Actors.Helpers.Implementation;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class BaseBuildingView : MonoBehaviour, IBaseBuildingView
	{
		[SerializeField] private ActorView m_Actor;
		[SerializeField] private PlaceableView m_Placeable;
		[SerializeField] private StandingSpotHolder m_StandingSpotHolder;

		public Faction Faction
		{
			set => m_Actor.Faction = value;
		}

		public Transform Cell
		{
			set => m_Placeable.Cell = value;
		}

		public Transform DefaultStandingSpot => m_StandingSpotHolder.DefaultStandingSpot;

		public IEnumerable<Transform> AvailableSpots => m_StandingSpotHolder.AvailableSpots;

		public void UpdateRotation(Vector3 zeroCellPosition)
		{
			var direction = transform.position - zeroCellPosition;
			var quaternion = Quaternion.LookRotation(direction);
			var euler = quaternion.eulerAngles;
			euler.y = (int)(euler.y / 60f) * 60f + 30;
			quaternion.eulerAngles = euler;
			transform.rotation = quaternion;
		}
	}
}