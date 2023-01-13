using System;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Models.GameProcess.Implementation
{
	public class ResourceHolder : IResourceHolder
	{
		public event Action<ResourceType, int> ResourceAmountChanged;

		private readonly ResourcePackage mResources;

		public int ResourcesAmount => mResources.Amount;

		public ResourceHolder()
		{
			mResources = ResourcePackage.Empty();
		}

		public bool HasResource(ResourceType resource, int amount)
		{
			return mResources.Content.TryGetValue(resource, out var possessedAmount) && possessedAmount >= amount;
		}

		public void AddResources(ResourcePackage resourcePackage)
		{
			foreach (var resource in resourcePackage.Content)
			{
				var changed = resource.Value > 0;
				if (mResources.Content.ContainsKey(resource.Key)) mResources.Content[resource.Key] += resource.Value;
				else mResources.Content.Add(resource.Key, resource.Value);

				if (changed) ResourceAmountChanged?.Invoke(resource.Key, mResources.Content[resource.Key]);
			}
		}

		public void UseResources(ResourcePackage resourcePackage)
		{
			foreach (var resource in resourcePackage.Content)
			{
				var changed = false;
				if (mResources.Content.ContainsKey(resource.Key))
				{
					var oldValue = mResources.Content[resource.Key];
					mResources.Content[resource.Key] = Mathf.Max(oldValue - resource.Value, 0);
					changed = oldValue != mResources.Content[resource.Key];
				}
				else mResources.Content.Add(resource.Key, 0);

				if (changed) ResourceAmountChanged?.Invoke(resource.Key, mResources.Content[resource.Key]);
			}
		}

		public int GetResource(ResourceType resourceType)
		{
			return mResources.Content.TryGetValue(resourceType, out var amount) ? amount : 0;
		}
	}
}