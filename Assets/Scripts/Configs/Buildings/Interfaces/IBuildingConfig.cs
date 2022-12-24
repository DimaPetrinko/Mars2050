using System.Collections.Generic;
using Configs.Actions;

namespace Configs.Buildings.Interfaces
{
	public interface IBuildingConfig
	{
		ResourceType Resource { get; }
		IEnumerable<ResourceCostData> BuildCost { get; }
		int ProduceAmount { get; }
	}
}