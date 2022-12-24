namespace Configs.Actions.Interfaces
{
	public interface IGatherConfig : IActionConfig
	{
		ResourceType GetResourcesForRoll(int roll);
	}
}