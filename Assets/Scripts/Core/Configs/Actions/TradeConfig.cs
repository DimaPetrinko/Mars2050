using Core.Configs.Actions.Enums;
using Core.Configs.Actions.Interfaces;
using UnityEngine;

namespace Core.Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(TradeConfig), menuName = "Configs/Actions/" + nameof(TradeConfig))]
	public class TradeConfig : ActionConfig, ITradeConfig
	{
		[Header("Trade")]
		[SerializeField] private bool m_WithPlayer;
		[SerializeField] private bool m_Repeatable;
		[SerializeField] private ResourceCostData m_PurchaseResource;

		public override ActionType Type => m_WithPlayer ? ActionType.PlayerTrade : ActionType.Trade;
		public bool Repeatable => m_Repeatable;

		public ResourceCostData PurchaseResource => m_PurchaseResource;
	}
}