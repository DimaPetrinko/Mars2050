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
		public void ChangeCell(ICell cell)
		{
			throw new NotImplementedException();
		}

		public Faction Faction { get; }
		public event Action Died;
		public IReactiveProperty<int> Health { get; }
		public IReactiveProperty<int> MaxHealth { get; }
		public void Damage(int damage)
		{
			throw new NotImplementedException();
		}

		public void Heal(int amount)
		{
			throw new NotImplementedException();
		}
	}
}