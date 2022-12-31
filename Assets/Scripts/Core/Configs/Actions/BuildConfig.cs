using Core.Configs.Actions.Enums;
using Core.Configs.Actions.Interfaces;
using Core.Configs.Buildings;
using Core.Configs.Buildings.Interfaces;
using UnityEngine;

namespace Core.Configs.Actions
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