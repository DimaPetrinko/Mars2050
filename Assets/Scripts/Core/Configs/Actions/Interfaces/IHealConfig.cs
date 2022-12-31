namespace Core.Configs.Actions.Interfaces
{
	public interface IHealConfig : IActionConfig
	{
		bool Repeatable { get; }
		int Amount { get; }
	}
}