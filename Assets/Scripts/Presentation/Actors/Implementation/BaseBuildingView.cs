using Core.Models.Enums;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	internal class BaseBuildingView : MonoBehaviour, IBaseBuildingView
	{
		[SerializeField] private Pair<Faction>[] m_BuildingVariants;

		public Faction Faction
		{
			set
			{
				foreach (var variant in m_BuildingVariants) variant.Object.SetActive(variant.Type == value);
			}
		}

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