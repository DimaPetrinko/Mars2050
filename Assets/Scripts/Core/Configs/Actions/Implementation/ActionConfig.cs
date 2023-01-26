using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	public abstract class ActionConfig : ScriptableObject, IActionConfig
	{
		[Header("Base")]
		[SerializeField] private string m_DisplayName;
		[SerializeField] private ActionRepeatability m_Repeatability;
		[SerializeField] private byte m_Oxygen;
		[SerializeField] private ResourceCostData[] m_Resources;

		public abstract ActionType Type { get; }
		public string DisplayName => m_DisplayName;
		public ActionRepeatability Repeatability => m_Repeatability;
		public byte Oxygen => m_Oxygen;
		public ResourceCostData[] Resources => m_Resources;
	}
}