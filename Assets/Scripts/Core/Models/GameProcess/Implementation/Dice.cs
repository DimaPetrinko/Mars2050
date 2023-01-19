using System;

namespace Core.Models.GameProcess.Implementation
{
	public class Dice : IDice
	{
		public event Action<byte> Rolled;

		private readonly byte mMaxRoll;

		public Dice(byte maxRoll)
		{
			mMaxRoll = maxRoll;
		}

		public void Roll()
		{
			var roll = (byte)UnityEngine.Random.Range(1, mMaxRoll + 1);
			Rolled?.Invoke(roll);
		}
	}
}