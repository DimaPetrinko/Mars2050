using System;

namespace Utils
{
	[Serializable]
	public struct IntRange
	{
		public int From;
		public int To;

		public readonly bool Contains(int value)
		{
			return From <= value && value <= To;
		}
	}
}