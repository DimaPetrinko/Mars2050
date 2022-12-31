using Core.Configs.Actions.Enums;

namespace Core.Configs.Actions.Interfaces
{
	public interface IActionConfig
	{
		ActionType Type { get; }
		int Oxygen { get; }
		ResourceCostData[] Resources { get; }
	}
}