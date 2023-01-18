using System;
using Core.Models.Enums;

namespace Core.Utils
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