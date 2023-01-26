using UnityEngine;

namespace Core.Utils
{
	public static class Vector2IntExtensions
	{
		public static bool InRange(this Vector2Int position, int range)
		{
			return position.Magnitude() <= range;
		}

		public static int Magnitude(this Vector2Int vector)
		{
			var z = -vector.x - vector.y;
			return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(z));
		}
	}
}