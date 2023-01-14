using System;
using System.Collections.Generic;
using Core.Models.Enums;
using Core.Models.Technology;

namespace Core.Models.GameProcess.Implementation
{
	public class TechnologyUser : ITechnologyUser
	{
		public event Action<ITechnology> TechnologyAdded;
		public event Action<TechnologyType> TechnologySpent;

		private readonly Dictionary<TechnologyType, ITechnology> mTechnologies = new();

		public IEnumerable<ITechnology> Technologies => mTechnologies.Values;

		public void AddTechnology(ITechnology technology)
		{
			if (mTechnologies.ContainsKey(technology.Type)) return;
			mTechnologies.Add(technology.Type, technology);
			TechnologyAdded?.Invoke(technology);
		}

		public void SpendTechnology(ITechnology technology)
		{
			if (!mTechnologies.ContainsKey(technology.Type) || !mTechnologies.ContainsValue(technology)) return;
			mTechnologies.Remove(technology.Type);
			TechnologySpent?.Invoke(technology.Type);
		}
	}
}