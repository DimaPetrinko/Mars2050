using Core.Models.Enums;
using UnityEngine;

namespace Presentation.Actors
{
	internal interface IBaseBuildingView : IPlaceableView
	{
		Faction Faction { set; }

		void UpdateRotation(Vector3 zeroCellPosition);
	}
}