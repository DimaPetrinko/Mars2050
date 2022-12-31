using Core.Models.Enums;

namespace Core.Configs.Buildings.Interfaces
{
	public interface IBuildingConfigs
	{
		IBuildingConfig GetConfig(ResourceType resource);
	}
}