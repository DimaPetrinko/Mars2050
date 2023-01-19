using System;
using System.Linq;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	[CreateAssetMenu(fileName = nameof(AttackConfig), menuName = "Configs/Actions/" + nameof(AttackConfig))]
	public class AttackConfig : ActionConfig, IAttackConfig
	{
		[Serializable]
		public struct RollDefinition
		{
			public Range<int> Range;
			public int Damage;
		}

		[Header("Attack")]
		[SerializeField] private bool m_Repeatable;
		[SerializeField] private double m_AttackRange;
		[SerializeField] private RollDefinition[] m_RollDefinition;

		public override ActionType Type => ActionType.Attack;
		public bool Repeatable => m_Repeatable;
		public double AttackRange => m_AttackRange;

		public int GetDamageForRoll(int roll)
		{
			return m_RollDefinition.FirstOrDefault(d => d.Range.Contains(roll)).Damage;
		}
	}
}