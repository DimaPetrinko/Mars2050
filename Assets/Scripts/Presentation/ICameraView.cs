using System;
using UnityEngine;

namespace Presentation
{
	internal interface ICameraView : IView
	{
		event Action<Vector2> PositionDelta;
		event Action<float> ZoomDelta;

		Vector2 Position { set; }
		float Zoom { set; }
	}
}