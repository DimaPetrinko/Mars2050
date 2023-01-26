using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
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