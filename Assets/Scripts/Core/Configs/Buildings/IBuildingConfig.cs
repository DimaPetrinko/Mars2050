using System.Collections.Generic;
using Core.Models.Enums;
using Core.Utils;

namespace Core.Configs.Buildings
{
	public interface IBuildingConfig
	{
		ResourceType Resource { get; }
		IEnumerable<ResourceCostData> BuildCost { get; }
		int ProduceAmount { get; }
	}
}