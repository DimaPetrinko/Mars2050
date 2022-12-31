using System;
using UnityEngine;

namespace Presentation.Actors.Implementation
{
	[Serializable]
	internal struct Pair<T>
	{
		public T Type;
		public GameObject Object;
	}
}