using System;
using Core.Configs;
using Core.Configs.Implementation;
using UnityEngine;

namespace Presentation.Implementation
{
	internal class CameraView : MonoBehaviour, ICameraView
	{
		public event Action<Vector2> PositionDelta;
		public event Action<float> ZoomDelta;

		[SerializeField] private CameraConfig m_Config;
		[SerializeField] private Transform m_Pivot;
		[SerializeField] private Transform m_Camera;

		public Vector2 Position
		{
			set => m_Pivot.position = new Vector3(value.x, 0, value.y);
		}

		public float Zoom
		{
			set
			{
				m_Camera.localPosition = new Vector3(0, 0, -value);
				var rotation = Config.ZoomTiltCurve.Evaluate(value);
				m_Pivot.rotation = Quaternion.Euler(rotation, 0, 0);
			}
		}

		private ICameraConfig Config => m_Config;

		private void Update()
		{
			var positionDelta = Vector2.zero;
			if (Input.GetKey(Config.RightKey)) positionDelta.x += Config.MovementSpeed * Time.deltaTime;
			if (Input.GetKey(Config.LeftKey)) positionDelta.x -= Config.MovementSpeed * Time.deltaTime;
			if (Input.GetKey(Config.UpKey)) positionDelta.y += Config.MovementSpeed * Time.deltaTime;
			if (Input.GetKey(Config.DownKey)) positionDelta.y -= Config.MovementSpeed * Time.deltaTime;
			PositionDelta?.Invoke(positionDelta);

			var zoomDelta = -Input.mouseScrollDelta.y * Config.ZoomSpeed * Time.deltaTime;
			ZoomDelta?.Invoke(zoomDelta);
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (m_Pivot != null && m_Camera != null) Gizmos.DrawLine(m_Pivot.position, m_Camera.position);
		}
#endif
	}
}