namespace Core.Configs.Actions
{
	public interface IHealConfig : IActionConfig
	{
		bool Repeatable { get; }
		int Amount { get; }
	}
}