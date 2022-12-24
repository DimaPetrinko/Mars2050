using System.Linq;
using Configs.Actions.Interfaces;
using UnityEngine;

namespace Configs.Actions
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