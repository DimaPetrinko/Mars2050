using System;
using Core.Models.Enums;
using TMPro;
using UnityEngine;

namespace Presentation.Actions.Implementation
{
	internal class ResourceInput : MonoBehaviour
	{
		public event Action<ResourceType, int> Changed;

		[SerializeField] private ResourceType m_Type;
		[SerializeField] private TMP_Text m_TypeName;
		[SerializeField] private TMP_InputField m_InputField;

		private int mMaxValue;

		public ResourceType Type => m_Type;

		public int Value
		{
			get
			{
				if (int.TryParse(m_InputField.text, out var count)) count = Mathf.Clamp(count, 0, mMaxValue);
				Value = count;
				return count;
			}
			set => m_InputField.SetTextWithoutNotify(value.ToString());
		}

		public int MaxValue
		{
			set => mMaxValue = value;
		}

		private void Awake()
		{
			m_InputField.onValueChanged.AddListener(OnValueChanged);
		}

		private void Start()
		{
			m_TypeName.text = m_Type.ToString();
		}

		private void OnValueChanged(string value)
		{
			Changed?.Invoke(m_Type, Value);
		}
	}
}