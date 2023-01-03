using Core.Utils;
using UnityEngine;

namespace Core.Configs
{
	public interface ICameraConfig
	{
		KeyCode RightKey { get; }
		KeyCode LeftKey { get; }
		KeyCode UpKey { get; }
		KeyCode DownKey { get; }
		float MovementSpeed { get; }
		float ZoomSpeed { get; }
		Range<float> ZoomLimits { get; }
		AnimationCurve ZoomTiltCurve { get; }
	}
}