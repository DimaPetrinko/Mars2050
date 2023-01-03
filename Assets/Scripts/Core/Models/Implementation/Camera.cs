using System;
using Core.Implementation;
using Core.Utils;
using UnityEngine;

namespace Core.Models.Implementation
{
	public class Camera : ICamera
	{
		public Camera(float moveRadius, Range<float> zoomLimits)
		{
			MoveRadius = moveRadius;
			ZoomLimits = zoomLimits;

			Position = new ReactiveProperty<Vector2>(Vector2.zero, PositionSetter);
			Zoom = new ReactiveProperty<float>(ZoomLimits.To, ZoomSetter);
		}

		public IReactiveProperty<Vector2> Position { get; }
		public IReactiveProperty<float> Zoom { get; }
		public float MoveRadius { get; }
		public Range<float> ZoomLimits { get; }

		private void PositionSetter(Vector2 value, Vector2 currentValue, Action<Vector2> setValue, Action triggerChanged)
		{
			value = Vector2.ClampMagnitude(value, MoveRadius);
			setValue(value);
			if (value != currentValue) triggerChanged();
		}

		private void ZoomSetter(float value, float currentValue, Action<float> setValue, Action triggerChanged)
		{
			value = Mathf.Clamp(value, ZoomLimits.From, ZoomLimits.To);
			setValue(value);
			if (Mathf.Abs(value - currentValue) > Mathf.Epsilon) triggerChanged();
		}
	}
}