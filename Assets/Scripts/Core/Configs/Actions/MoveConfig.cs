using Core.Configs.Actions.Enums;
using Core.Configs.Actions.Interfaces;
using UnityEngine;

namespace Core.Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(MoveConfig), menuName = "Configs/Actions/" + nameof(MoveConfig))]
	public class MoveConfig : ActionConfig, IMoveConfig
	{
		[Header("Move")]
		[SerializeField] private int m_MoveRange;
		[SerializeField] private bool m_CanMoveToOccupiedCell;
		[SerializeField] private bool m_CanMoveToDamagedBuilding;

		public override ActionType Type => ActionType.Move;
		public int MoveRange => m_MoveRange;
		public bool CanMoveToOccupiedCell => m_CanMoveToOccupiedCell;
		public bool CanMoveToDamagedBuilding => m_CanMoveToDamagedBuilding;
	}
}