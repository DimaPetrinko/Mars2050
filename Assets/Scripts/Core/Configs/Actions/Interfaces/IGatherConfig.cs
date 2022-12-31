using Core.Models.Enums;

namespace Core.Configs.Actions.Interfaces
{
	public interface IGatherConfig : IActionConfig
	{
		ResourceType GetResourcesForRoll(int roll);
	}
}