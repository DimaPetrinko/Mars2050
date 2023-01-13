using System;
using System.Collections.Generic;
using Core.Models.Enums;

namespace Core.Configs.Actions
{
	public enum ResourceRelation
	{
		Same,
		Any
	}

	[Serializable]
	public struct ResourceCostData
	{
		public int Amount;
		public ResourceRelation Relation;
		public ResourceType Type;
	}

	// TODO: Use it everywhere
	public struct ResourcePackage
	{
		public IDictionary<ResourceType, int> Content { get; }

		public ResourcePackage(IDictionary<ResourceType, int> content)
		{
			Content = content;
		}
	}
}