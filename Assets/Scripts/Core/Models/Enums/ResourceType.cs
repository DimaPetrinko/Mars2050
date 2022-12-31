using System;

namespace Core.Models.Enums
{
	[Flags]
	public enum ResourceType : short
	{
		Water = 1,
		Plants = 2,
		Ore = 4,
		Electricity = 8
	}
}