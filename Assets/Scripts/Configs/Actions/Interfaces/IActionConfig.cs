namespace Configs.Actions.Interfaces
{
	public interface IActionConfig
	{
		ActionType Type { get; }
		int Oxygen { get; }
		ResourceCostData[] Resources { get; }
	}
}