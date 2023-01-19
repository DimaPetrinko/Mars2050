using System;

namespace Core.Models.GameProcess
{
	public interface IDice : IModel
	{
		event Action<byte> Rolled;

		void Roll();
	}
}