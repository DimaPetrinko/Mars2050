using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models.Enums;

namespace Core.Utils
{
	public static class ResourcesExtensions
	{
		public static IEnumerable<ResourceType> AsCollection(this ResourceType flags)
		{
			return ((ResourceType[])Enum.GetValues(typeof(ResourceType)))
				.Where(mask => ((int)flags & (int)mask) > 0);
		}
	}
}