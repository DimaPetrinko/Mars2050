using System;
using System.Linq;
using Configs.Actions.Interfaces;
using UnityEngine;
using Utils;

namespace Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(GatherConfig), menuName = "Configs/Actions/" + nameof(GatherConfig))]
	public class GatherConfig : ActionConfig, IGatherConfig
	{
		[Serializable]
		public struct RollDefinition
		{
			public Range Range;
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