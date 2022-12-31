namespace Core.Configs.Actions.Interfaces
{
	public interface ITradeConfig : IActionConfig
	{
		bool Repeatable { get; }
		ResourceCostData PurchaseResource { get; }
	}
}