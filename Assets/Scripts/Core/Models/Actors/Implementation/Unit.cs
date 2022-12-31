using System;
using Core.Models.Boards;
using Core.Models.Enums;
using TMPro;
using UnityEngine;

namespace Core.Models.Actors.Implementation
{
	public readonly struct UnitViewModel
	{
		public readonly Vector3 Position;
		public readonly int Health;
		public readonly int MaxHealth;
		public readonly bool ShowHealth;
	}
	public class UnitView : MonoBehaviour
	{
		[SerializeField] private TMP_Text m_Health;

		public void UpdateWithModel(UnitViewModel model)
		{
			transform.position = model.Position;
			m_Health.text = $"{model.Health}/{model.MaxHealth}";
			m_Health.gameObject.SetActive(model.ShowHealth);
		}
	}

	public class Unit : IUnit
	{
		public event Action<ICell> CellChanged;
		public event Action Died;

		private int mHealth;

		public Faction Faction { get; }

		public int Health
		{
			get => mHealth;
			set => mHealth = Mathf.Clamp(value, 0, MaxHealth);
		}

		public int MaxHealth { get; set; }

		public void Damage(int damage)
		{
			Health -= damage;
			if (Health == 0)
				Died?.Invoke();
		}

		public void Heal(int amount)
		{
			Health += amount;
		}

		public void ChangeCell(ICell cell)
		{
			CellChanged?.Invoke(cell);
		}
	}
}