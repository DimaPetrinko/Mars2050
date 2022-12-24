using Configs.Actions.Interfaces;
using UnityEngine;

namespace Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(TradeConfig), menuName = "Configs/Actions/" + nameof(TradeConfig))]
	public class TradeConfig : ActionConfig, ITradeConfig
	{
		[Header("Trade")]
		[SerializeField] private bool m_WithPlayer;
		[SerializeField] private ResourceCostData m_PurchaseResource;

		public override ActionType Type => m_WithPlayer ? ActionType.PlayerTrade : ActionType.Trade;
		public ResourceCostData PurchaseResource => m_PurchaseResource;
	}
}