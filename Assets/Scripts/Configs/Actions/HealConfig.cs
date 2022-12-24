using Configs.Actions.Interfaces;
using UnityEngine;

namespace Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(HealConfig), menuName = "Configs/Actions/" + nameof(HealConfig))]
	public class HealConfig : ActionConfig, IHealConfig
	{
		[Header("Heal")]
		[SerializeField] private int m_Amount;

		public override ActionType Type => ActionType.Heal;
		public int Amount => m_Amount;
	}
}