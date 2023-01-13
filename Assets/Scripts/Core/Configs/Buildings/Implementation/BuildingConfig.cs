using System.Collections.Generic;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Configs.Buildings.Implementation
{
	[CreateAssetMenu(fileName = nameof(BuildingConfig), menuName = "Configs/Buildings/" + nameof(BuildingConfig))]
	public class BuildingConfig : ScriptableObject, IBuildingConfig
	{
		[SerializeField] private ResourceType m_Resource;
		[SerializeField] private ResourceCostData[] m_BuildCost;
		[SerializeField] private int m_ProduceAmount;

		public ResourceType Resource => m_Resource;
		public IEnumerable<ResourceCostData> BuildCost => m_BuildCost;
		public int ProduceAmount => m_ProduceAmount;
	}
}