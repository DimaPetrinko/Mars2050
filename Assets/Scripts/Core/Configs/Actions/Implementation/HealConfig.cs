using Core.Configs.Actions.Enums;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	[CreateAssetMenu(fileName = nameof(HealConfig), menuName = "Configs/Actions/" + nameof(HealConfig))]
	public class HealConfig : ActionConfig, IHealConfig
	{
		[Header("Heal")]
		[SerializeField] private bool m_Repeatable;
		[SerializeField] private int m_Amount;

		public override ActionType Type => ActionType.Heal;
		public bool Repeatable => m_Repeatable;

		public int Amount => m_Amount;
	}
}