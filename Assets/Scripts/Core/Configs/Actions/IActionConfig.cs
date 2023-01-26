using Core.Models.Enums;
using Core.Utils;

namespace Core.Configs.Actions
{
	public interface IActionConfig
	{
		ActionType Type { get; }
		public string DisplayName { get; }
		bool Repeatable { get; }
		byte Oxygen { get; }
		ResourceCostData[] Resources { get; }
	}
}