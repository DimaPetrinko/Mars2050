using System.Linq;
using Core.Configs.Actions.Enums;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	[CreateAssetMenu(fileName = nameof(ActionConfigs), menuName = "Configs/Actions/" + nameof(ActionConfigs))]
	public class ActionConfigs : ScriptableObject, IActionConfigs
	{
		[SerializeField] private ActionConfig[] m_Actions;

		public TConfig GetConfig<TConfig>(ActionType actionType) where TConfig : IActionConfig
		{
			return (TConfig)(IActionConfig)m_Actions.FirstOrDefault(c => c.Type == actionType);
		}
	}
}