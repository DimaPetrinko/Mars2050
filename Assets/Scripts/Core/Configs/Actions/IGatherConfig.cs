using Core.Models.Enums;

namespace Core.Configs.Actions
{
	public interface IGatherConfig : IActionConfig
	{
		ResourceType GetResourcesForRoll(int roll);
	}
}