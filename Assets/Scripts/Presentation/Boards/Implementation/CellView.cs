using System;
using System.Collections.Generic;
using Core.Configs;
using Core.Configs.Implementation;
using Presentation.Actors.Helpers.Implementation;
using UnityEditor;
using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class CellView : MonoBehaviour, ICellView
	{
		public event Action Clicked;

		[SerializeField] private StandingSpotHolder m_StandingSpotHolder;
		[SerializeField] private GameConfig m_GameConfig;
		[SerializeField] private ClickDetector m_ClickDetector;

		public Transform DefaultStandingSpot => m_StandingSpotHolder.DefaultStandingSpot;

		public IEnumerable<Transform> AvailableSpots => m_StandingSpotHolder.AvailableSpots;

		public Vector2Int Position
		{
			set => transform.localPosition = ToWorldPosition(value);
		}

		private IGameConfig Config => m_GameConfig;

		private void Awake()
		{
			m_ClickDetector.Clicked += OnClicked;
		}

		private void Start()
		{
			var radius = Config.CellRadius;
			transform.localScale = new Vector3(radius, radius, radius);
		}

		private Vector3 ToWorldPosition(Vector2Int position)
		{
			var sqrt = Mathf.Sqrt(3);
			const float f = 3f / 2;

			var x = Config.CellRadius * (position.x * sqrt + position.y * sqrt / 2);
			var y = Config.CellRadius * position.y * f;

			return new Vector3(x, 0, y);
		}

		private void OnClicked()
		{
			Clicked?.Invoke();
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Handles.color = Color.green;
			Handles.DrawWireDisc(transform.position, transform.up, Config.CellRadius);
		}
#endif
	}
}