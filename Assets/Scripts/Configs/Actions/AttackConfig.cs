using System;
using System.Linq;
using Configs.Actions.Interfaces;
using UnityEngine;
using Utils;

namespace Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(AttackConfig), menuName = "Configs/Actions/" + nameof(AttackConfig))]
	public class AttackConfig : ActionConfig, IAttackConfig
	{
		[Serializable]
		public struct RollDefinition
		{
			public Range Range;
			public int Damage;
		}

		[Header("Attack")]
		[SerializeField] private bool m_Repeatable;
		[SerializeField] private RollDefinition[] m_RollDefinition;

		public override ActionType Type => ActionType.Attack;
		public bool Repeatable => m_Repeatable;

		public int GetDamageForRoll(int roll)
		{
			return m_RollDefinition.FirstOrDefault(d => d.Range.Contains(roll)).Damage;
		}
	}
}