using System;

namespace Core.Utils
{
	[Serializable]
	public struct Range<T> where T: IComparable
	{
		public T From;
		public T To;

		public readonly bool Contains(T value)
		{
			return value.CompareTo(From) + To.CompareTo(value) == 2;
		}
	}
}