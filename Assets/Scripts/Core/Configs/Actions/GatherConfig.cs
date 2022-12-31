using System;
using System.Linq;
using Core.Configs.Actions.Enums;
using Core.Configs.Actions.Interfaces;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(GatherConfig), menuName = "Configs/Actions/" + nameof(GatherConfig))]
	public class GatherConfig : ActionConfig, IGatherConfig
	{
		[Serializable]
		public struct RollDefinition
		{
			public IntRange Range;
			public ResourceType Resources;
		}

		[Header("Gather")]
		[SerializeField] private RollDefinition[] m_RollDefinition;

		public override ActionType Type => ActionType.Gather;

		public ResourceType GetResourcesForRoll(int roll)
		{
			return m_RollDefinition.FirstOrDefault(d => d.Range.Contains(roll)).Resources;
		}
	}
}