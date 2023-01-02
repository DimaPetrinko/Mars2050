using System;

namespace Presentation.Actors.Implementation
{
	[Serializable]
	internal struct Pair<TKey, TValue>
	{
		public TKey Type;
		public TValue Object;
	}
}