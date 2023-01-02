using Core.Configs.Actions.Enums;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	public abstract class ActionConfig : ScriptableObject, IActionConfig
	{
		[Header("Base")]
		[SerializeField] private int m_Oxygen;
		[SerializeField] private ResourceCostData[] m_Resources;

		public abstract ActionType Type { get; }
		public int Oxygen => m_Oxygen;
		public ResourceCostData[] Resources => m_Resources;
	}
}