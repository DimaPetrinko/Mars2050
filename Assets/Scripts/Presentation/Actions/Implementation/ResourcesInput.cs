using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Presentation.Actions.Implementation
{
	internal class ResourcesInput : MonoBehaviour
	{
		public event Action ResourcesChanged;

		[SerializeField] private ResourceInput[] m_ResourceInputs;

		private Dictionary<ResourceType, ResourceInput> mResourceInputs;
		private ResourcePackage mResources = ResourcePackage.Empty();

		public ResourcePackage Resources
		{
			get => mResources;
			set
			{
				mResources = value;
				foreach (var pair in mResourceInputs)
					pair.Value.Value = mResources.Content.TryGetValue(pair.Key, out var amount) ? amount : 0;
			}
		}

		public void ToggleInputs(bool value)
		{
			foreach (var input in mResourceInputs.Values) input.gameObject.SetActive(value);
		}

		private void Awake()
		{
			mResourceInputs = m_ResourceInputs.ToDictionary(i => i.Type, i => i);
			foreach (var resourceInput in m_ResourceInputs) resourceInput.Changed += OnResourceValueChanged;
		}

		private void OnResourceValueChanged(ResourceType type, int value)
		{
			if (Resources.Content.ContainsKey(type))
				Resources.Content[type] = value;
			else Resources.Content.Add(type, value);
			ResourcesChanged?.Invoke();
		}
	}
}