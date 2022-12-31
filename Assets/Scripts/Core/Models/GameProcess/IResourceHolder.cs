using System.Collections.Generic;
using Core.Models.Enums;

namespace Core.Models.GameProcess
{
	public interface IResourceHolder
	{
		IEnumerable<ResourceType> Resources { get; }
		int ResourcesCount { get; }

		bool HasResource(ResourceType resource, int amount);
		void AddResources(ResourceType[] resources);
		void UseResources(ResourceType[] resources);
	}
}