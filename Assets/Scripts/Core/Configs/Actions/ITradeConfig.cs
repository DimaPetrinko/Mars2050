using Core.Utils;

namespace Core.Configs.Actions
{
	public interface ITradeConfig : IActionConfig
	{
		ResourceCostData PurchaseResource { get; }
	}
}