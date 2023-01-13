using UnityEngine;

namespace Presentation.Actors.Helpers.Implementation
{
	internal class PlaceableView : MonoBehaviour, IPlaceableView
	{
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
	}
}