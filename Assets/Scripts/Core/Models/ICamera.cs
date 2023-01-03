using Core.Utils;
using UnityEngine;

namespace Core.Models
{
	public interface ICamera : IModel
	{
		IReactiveProperty<Vector2> Position { get; }
		IReactiveProperty<float> Zoom { get; }
		float MoveRadius { get; }
		Range<float> ZoomLimits { get; }
	}
}