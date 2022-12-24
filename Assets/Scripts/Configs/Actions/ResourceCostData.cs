using System;

namespace Configs.Actions
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
}