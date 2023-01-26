using System;

namespace Core
{
	public interface IReactiveProperty<T>
	{
		event Action<T> Changed;
		event Action<T> Updated;
		T Value { get; set; }
	}
}