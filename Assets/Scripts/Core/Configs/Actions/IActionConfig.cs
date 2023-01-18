using Core.Configs.Actions.Enums;
using Core.Utils;

namespace Core.Configs.Actions
{
	public interface IActionConfig
	{
		ActionType Type { get; }
		int Oxygen { get; }
		ResourceCostData[] Resources { get; }
	}
}