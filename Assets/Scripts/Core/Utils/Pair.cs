using System;

namespace Core.Utils
{
	[Serializable]
	public struct Pair<TKey, TValue>
	{
		public TKey Type;
		public TValue Object;
	}
}