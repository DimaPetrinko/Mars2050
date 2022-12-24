using Configs.Actions.Interfaces;
using UnityEngine;

namespace Configs.Actions
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