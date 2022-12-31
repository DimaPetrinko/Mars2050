using System.Linq;
using Core.Configs.Buildings.Interfaces;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Configs.Buildings
{
	[CreateAssetMenu(fileName = nameof(BuildingConfigs), menuName = "Configs/Buildings/" + nameof(BuildingConfigs))]
	public class BuildingConfigs : ScriptableObject, IBuildingConfigs
	{
		[SerializeField] private BuildingConfig[] m_Buildings;

		public IBuildingConfig GetConfig(ResourceType resource)
		{
			return m_Buildings.FirstOrDefault(c => c.Resource == resource);
		}
	}
}