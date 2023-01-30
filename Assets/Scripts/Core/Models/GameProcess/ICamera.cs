using Core.Utils;
using UnityEngine;

namespace Core.Models.GameProcess
{
	public interface ICamera : IModel
	{
		IReactiveProperty<Vector2> Position { get; }
		IReactiveProperty<float> Zoom { get; }
		Range<float> ZoomLimits { get; }
	}
}