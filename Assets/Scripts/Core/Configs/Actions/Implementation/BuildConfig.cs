using Core.Configs.Buildings;
using Core.Configs.Buildings.Implementation;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	[CreateAssetMenu(fileName = nameof(BuildConfig), menuName = "Configs/Actions/" + nameof(BuildConfig))]
	public class BuildConfig : ActionConfig, IBuildConfig
	{
		[Header("Build")]
		[SerializeField] private bool m_RewardWithTechnology;
		[SerializeField] private BuildingConfigs m_BuildingConfigs;

		public override ActionType Type => ActionType.Build;
		public bool RewardWithTechnology => m_RewardWithTechnology;
		public IBuildingConfigs BuildingConfigs => m_BuildingConfigs;
	}
}