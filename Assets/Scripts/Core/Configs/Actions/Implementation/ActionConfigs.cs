using System.Collections.Generic;
using System.Linq;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	[CreateAssetMenu(fileName = nameof(ActionConfigs), menuName = "Configs/Actions/" + nameof(ActionConfigs))]
	public class ActionConfigs : ScriptableObject, IActionConfigs
	{
		[SerializeField] private ActionConfig[] m_Actions;

		private Dictionary<ActionType, IActionConfig> mActionConfigs;

		private void Awake()
		{
			mActionConfigs = m_Actions.ToDictionary(a => a.Type, a => (IActionConfig)a);
		}

		public TConfig GetConfig<TConfig>(ActionType actionType) where TConfig : IActionConfig
		{
			return (TConfig)GetConfig(actionType);
		}

		public IActionConfig GetConfig(ActionType actionType)
		{
			return mActionConfigs.TryGetValue(actionType, out var config) ? config : null;
		}
	}
}