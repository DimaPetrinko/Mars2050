using System;
using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actors
{
	public interface IResource : IPlaceable
	{
		event Action IsDiscoveredChanged;
		bool IsDiscovered { get; set; }
		ResourceType Type { get; }
	}
}