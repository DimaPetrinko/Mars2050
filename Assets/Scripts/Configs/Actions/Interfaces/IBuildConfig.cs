using Configs.Buildings;
using Configs.Buildings.Interfaces;

namespace Configs.Actions.Interfaces
{
	public interface IBuildConfig
	{
		bool RewardWithTechnology { get; }
		IBuildingConfigs BuildingConfigs { get; }
	}
}