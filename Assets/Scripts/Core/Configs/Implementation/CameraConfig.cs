using Core.Utils;
using UnityEngine;

namespace Core.Configs.Implementation
{
	[CreateAssetMenu(fileName = nameof(CameraConfig), menuName = "Configs/" + nameof(CameraConfig))]
	public class CameraConfig : ScriptableObject, ICameraConfig
	{
		[SerializeField] private KeyCode m_RightKey;
		[SerializeField] private KeyCode m_LeftKey;
		[SerializeField] private KeyCode m_UpKey;
		[SerializeField] private KeyCode m_DownKey;
		[SerializeField] private float m_MovementSpeed;
		[SerializeField] private float m_ZoomSpeed;
		[SerializeField] private Range<float> m_ZoomLimits;
		[SerializeField] private AnimationCurve m_ZoomTiltCurve;

		public KeyCode RightKey => m_RightKey;
		public KeyCode LeftKey => m_LeftKey;
		public KeyCode UpKey => m_UpKey;
		public KeyCode DownKey => m_DownKey;
		public float MovementSpeed => m_MovementSpeed;
		public float ZoomSpeed => m_ZoomSpeed;
		public Range<float> ZoomLimits => m_ZoomLimits;
		public AnimationCurve ZoomTiltCurve => m_ZoomTiltCurve;
	}
}