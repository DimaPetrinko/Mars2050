using System.Collections.Generic;
using System.Linq;

namespace Core.Utils
{
	public static class ArrayExtensions
	{
		public static IEnumerable<T> ToEnumerable<T>(this T[,] target)
		{
			return target.Cast<T>().Where(e => e != null);
		}
	}
}