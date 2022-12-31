using Core.Configs.Buildings.Interfaces;

namespace Core.Configs.Actions.Interfaces
{
	public interface IBuildConfig : IActionConfig
	{
		bool RewardWithTechnology { get; }
		IBuildingConfigs BuildingConfigs { get; }
	}
}