using System.Collections.Generic;
using System.Linq;

namespace Core.Utils
{
	public static class CollectionsExtensions
	{
		public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
		{
			var buffer = source.ToList();
			for (var i = 0; i < buffer.Count; i++)
			{
				var j = UnityEngine.Random.Range(i, buffer.Count);
				yield return buffer[j];
				buffer[j] = buffer[i];
			}
		}
	}
}