using Core.Models.Enums;

namespace Core.Configs.Buildings
{
	public interface IBuildingConfigs
	{
		IBuildingConfig GetConfig(ResourceType resource);
	}
}