using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Presentation.Actors.Helpers.Implementation
{
	internal class ActorView : MonoBehaviour, IActorView
	{
		[SerializeField] private Pair<Faction, GameObject>[] m_FactionObjects;

		public Faction Faction
		{
			set
			{
				foreach (var variant in m_FactionObjects) variant.Object.SetActive(variant.Type == value);
			}
		}
	}
}