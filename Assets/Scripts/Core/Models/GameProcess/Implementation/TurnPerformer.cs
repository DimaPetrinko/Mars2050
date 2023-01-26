using System;
using Core.Implementation;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Models.GameProcess.Implementation
{
	public class TurnPerformer : ITurnPerformer
	{
		// TODO: must not be able to end turn if the total amount of resources is greater than the value in config
		public Faction Faction { get; }
		public IReactiveProperty<short> Oxygen { get; }
		public IReactiveProperty<bool> HisTurn { get; }

		public TurnPerformer(Faction faction)
		{
			Oxygen = new ReactiveProperty<short>(0, OxygenSetter);
			HisTurn = new ReactiveProperty<bool>();
			Faction = faction;
		}

		private void OxygenSetter(short value, short currentValue, Action<short> setValue, Action triggerChanged)
		{
			value = (short)Mathf.Max(value, 0);
			setValue(value);
			if (currentValue != value) triggerChanged();
		}
	}
}