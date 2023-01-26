using Core.Models.Enums;
using Core.Utils;

namespace Core.Configs.Actions
{
	public interface IActionConfig
	{
		ActionType Type { get; }
		public string DisplayName { get; }
		ActionRepeatability Repeatability { get; }
		byte Oxygen { get; }
		ResourceCostData[] Resources { get; }
	}
}