using Core.Configs.Implementation;
using UnityEditor;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class CellView : MonoBehaviour, ICellView
	{
		[SerializeField] private Transform m_MainSpot;
		[SerializeField] private Transform m_SecondarySpot;
		[SerializeField] private GameConfig m_GameConfig;

		public Transform MainSpot => m_MainSpot;
		public Transform SecondarySpot => m_SecondarySpot;

		public Vector2Int Position
		{
			set => transform.localPosition = ToWorldPosition(value);
		}

		private void Start()
		{
			var radius = m_GameConfig.CellRadius;
			transform.localScale = new Vector3(radius, radius, radius);
		}

		private Vector3 ToWorldPosition(Vector2Int position)
		{
			var sqrt = Mathf.Sqrt(3);
			const float f = 3f / 2;

			var x = m_GameConfig.CellRadius * (position.x * sqrt + position.y * sqrt / 2);
			var y = m_GameConfig.CellRadius * position.y * f;

			return new Vector3(x, 0, y);
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Handles.color = Color.green;
			Handles.DrawWireDisc(transform.position, transform.up, m_GameConfig.CellRadius);
		}
#endif
	}
}