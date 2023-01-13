using System;
using UnityEngine;

namespace Presentation.Actors.Helpers.Implementation
{
	[RequireComponent(typeof(Collider))]
	public class ClickDetector : MonoBehaviour
	{
		public event Action Clicked;

		private bool mClicked;

		private void OnMouseDown()
		{
			if (mClicked) return;
			Clicked?.Invoke();
		}

		private void OnMouseUp()
		{
			mClicked = false;
		}
	}
}