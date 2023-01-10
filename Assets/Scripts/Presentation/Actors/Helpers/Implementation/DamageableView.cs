using TMPro;
using UnityEngine;

namespace Presentation.Actors.Helpers.Implementation
{
	internal class DamageableView : MonoBehaviour, IDamageableView
	{
		[SerializeField] private GameObject m_HealthBar;
		[SerializeField] private TMP_Text m_Health;
		[SerializeField] private TMP_Text m_MaxHealth;

		public int Health
		{
			set => m_Health.text = value.ToString();
		}

		public int MaxHealth
		{
			set => m_MaxHealth.text = value.ToString();
		}

		public bool HealthBarShown
		{
			set => m_HealthBar.SetActive(value);
		}
	}
}