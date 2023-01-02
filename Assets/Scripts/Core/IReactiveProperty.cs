using System;

namespace Core
{
	public interface IReactiveProperty<T>
	{
		event Action<T> Changed;
		T Value { get; set; }
	}
}