using System;

namespace Utils
{
	[Serializable]
	public struct Range
	{
		public int From;
		public int To;

		public readonly bool Contains(int value)
		{
			return From <= value && value <= To;
		}
	}
}