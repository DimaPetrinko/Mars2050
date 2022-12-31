using UnityEditor;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class CellView : MonoBehaviour, ICellView
	{
		[SerializeField] private float m_OuterRadius;

		public Transform Transform => transform;

		public Vector2Int Position
		{
			set => transform.localPosition = ToWorldPosition(value);
		}

		private void Start()
		{
			transform.localScale = new Vector3(m_OuterRadius, m_OuterRadius, m_OuterRadius);
		}

		private Vector3 ToWorldPosition(Vector2Int position)
		{
			var sqrt = Mathf.Sqrt(3);
			const float f = 3f / 2;

			var x = m_OuterRadius * (position.x * sqrt + position.y * sqrt / 2);
			var y = m_OuterRadius * position.y * f;

			return new Vector3(x, 0, y);
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Handles.color = Color.green;
			Handles.DrawWireDisc(transform.position, transform.up, m_OuterRadius);
		}
#endif
	}
}