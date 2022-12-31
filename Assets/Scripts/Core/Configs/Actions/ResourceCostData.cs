using System;
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

	public readonly struct ResourceData
	{
		private readonly ResourceType mType;
		private readonly int mAmount;

		public ResourceData(ResourceType type, int amount)
		{
			mType = type;
			mAmount = amount;
		}

		public ResourceType[] AsArray()
		{
			var resources = new ResourceType[mAmount];
			for (var i = 0; i < resources.Length; i++) resources[i] = mType;
			return resources;
		}
	}
}