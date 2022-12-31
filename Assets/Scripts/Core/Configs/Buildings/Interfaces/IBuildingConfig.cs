using System.Collections.Generic;
using Core.Configs.Actions;
using Core.Models.Enums;

namespace Core.Configs.Buildings.Interfaces
{
	public interface IBuildingConfig
	{
		ResourceType Resource { get; }
		IEnumerable<ResourceCostData> BuildCost { get; }
		int ProduceAmount { get; }
	}
}