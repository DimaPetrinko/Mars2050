using System;
using Core.Models.Enums;
using Core.Utils;

namespace Core.Models.GameProcess
{
	public interface IResourceHolder
	{
		event Action<ResourceType, int> ResourceAmountChanged;

		int ResourcesAmount { get; }

		bool HasResource(ResourceType resource, int amount);
		void AddResources(ResourcePackage resourcePackage);
		void UseResources(ResourcePackage resourcePackage);
		int GetResource(ResourceType resourceType);
	}
}