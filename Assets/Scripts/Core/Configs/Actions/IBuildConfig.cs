using Core.Configs.Buildings;

namespace Core.Configs.Actions
{
	public interface IBuildConfig : IActionConfig
	{
		bool RewardWithTechnology { get; }
		IBuildingConfigs BuildingConfigs { get; }
	}
}