using Core.Utils;

namespace Core.Configs.Actions
{
	public interface ITradeConfig : IActionConfig
	{
		bool Repeatable { get; }
		ResourceCostData PurchaseResource { get; }
	}
}