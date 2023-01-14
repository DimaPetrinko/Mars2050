using System;
using System.Collections.Generic;
using Core.Models.Enums;
using Core.Models.Technology;

namespace Core.Models.GameProcess
{
	public interface ITechnologyUser
	{
		event Action<ITechnology> TechnologyAdded;
		event Action<TechnologyType> TechnologySpent;

		IEnumerable<ITechnology> Technologies { get; }

		void AddTechnology(ITechnology technology);
		void SpendTechnology(ITechnology technology);
	}
}