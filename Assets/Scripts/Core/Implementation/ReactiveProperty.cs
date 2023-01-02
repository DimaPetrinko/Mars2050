using System;

namespace Core.Implementation
{
	public class ReactiveProperty<T> : IReactiveProperty<T>
	{
		public delegate void ReactiveSetter(T value, T currentValue, Action<T> setValue, Action triggerChanged);

		public event Action<T> Changed;

		private T mValue;
		private readonly ReactiveSetter mSetter;

		public T Value
		{
			get => mValue;
			set
			{
				mSetter(value, mValue, v => mValue = v, () => Changed?.Invoke(mValue));
			}
		}

		public ReactiveProperty(T defaultValue = default, ReactiveSetter customSetter = null)
		{
			mValue = defaultValue;
			mSetter = customSetter ?? DefaultSetter;
		}

		private void DefaultSetter(T value, T currentValue, Action<T> setValue, Action triggerChanged)
		{
			mValue = value;
			if (!currentValue.Equals(value)) triggerChanged?.Invoke();
		}
	}
}