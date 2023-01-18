using System;
using Core.Implementation;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Models.GameProcess.Implementation
{
	public class TurnPerformer : ITurnPerformer
	{
		public Faction Faction { get; }
		public IReactiveProperty<int> Oxygen { get; }
		public IReactiveProperty<bool> HisTurn { get; }

		public TurnPerformer(Faction faction)
		{
			Oxygen = new ReactiveProperty<int>(0, OxygenSetter);
			HisTurn = new ReactiveProperty<bool>();
			Faction = faction;
		}

		// TODO: move to turn processor
		// public int Roll()
		// {
		// 	return UnityEngine.Random.Range(0, mMaxRoll + 1);
		// }

		private void OxygenSetter(int value, int currentValue, Action<int> setValue, Action triggerChanged)
		{
			value = Mathf.Max(value, 0);
			setValue(value);
			if (currentValue != value) triggerChanged();
		}
	}
}