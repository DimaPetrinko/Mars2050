using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using UnityEngine;

namespace Presentation.Actors.Helpers.Implementation
{
	public class StandingSpotHolder : MonoBehaviour, IStandingSpotHolder
	{
		[SerializeField] private Transform[] m_StandingSpots;

		public Transform DefaultStandingSpot => m_StandingSpots.FirstOrDefault();

		public IEnumerable<Transform> AvailableSpots => m_StandingSpots.Where(s => s.childCount == 0).Shuffle();
	}
}