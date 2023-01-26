using System.Collections.Generic;
using System.Linq;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	[CreateAssetMenu(fileName = nameof(ActionConfigs), menuName = "Configs/Actions/" + nameof(ActionConfigs))]
	public class ActionConfigs : ScriptableObject, IActionConfigs
	{
		[SerializeField] private ActionConfig[] m_Actions;
		[SerializeField] private Pair<ActionResult, string>[] m_ActionResultTexts;

		private Dictionary<ActionType, IActionConfig> mActionConfigs;
		private Dictionary<ActionResult, string> mActionResultTexts;

		private Dictionary<ActionType, IActionConfig> ActionConfigsDictionary =>
			mActionConfigs ??= m_Actions.ToDictionary(a => a.Type, a => (IActionConfig)a);

		private Dictionary<ActionResult, string> ActionResultTexts =>
			mActionResultTexts ??= m_ActionResultTexts.ToDictionary(a => a.Type, a => a.Object);

		public TConfig GetConfig<TConfig>(ActionType actionType) where TConfig : IActionConfig
		{
			return (TConfig)GetConfig(actionType);
		}

		public IActionConfig GetConfig(ActionType actionType)
		{
			return ActionConfigsDictionary.TryGetValue(actionType, out var config) ? config : null;
		}

		public string GetResultText(ActionResult result)
		{
			return ActionResultTexts.TryGetValue(result, out var text) ? text : "";
		}
	}
}