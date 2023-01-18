using System.Collections.Generic;
using System.Linq;
using Core.Models.Enums;

namespace Core.Utils
{
	public struct ResourcePackage
	{
		public IDictionary<ResourceType, int> Content { get; }

		public int Amount => Content.Sum(r => r.Value);

		public static ResourcePackage Copy(ResourcePackage other)
		{
			return new ResourcePackage(other.Content.ToDictionary(
				p => p.Key,
				p => p.Value));
		}

		public static ResourcePackage Empty()
		{
			return new ResourcePackage(new Dictionary<ResourceType, int>());
		}

		public ResourcePackage(IDictionary<ResourceType, int> resources)
		{
			Content = resources;
		}

		public ResourcePackage(ResourceType type, int amount)
		{
			Content = new Dictionary<ResourceType, int> { { type, amount } };
		}

		public void Clear()
		{
			Content.Clear();
		}
	}
}