using Core.Configs.Actions;
using Core.Configs.Buildings;
using Core.Models.Enums;
using UnityEngine;

namespace Core.Configs
{
	public interface IGameConfig
	{
		IActionConfigs Actions { get; }
		IBuildingConfigs BuildingConfigs { get; }
		ICameraConfig CameraConfig { get; }
		int BoardRadius { get; }
		float CellRadius { get; }
		int UnitsPerPlayer { get; }
		int MaxRoll { get; }
		int MaxResources { get; }
		int MaxUnitHealth { get; }
		int MaxBuildingHealth { get; }
		int MaxBuildingWithUnitHealth { get; }

		int GetBuildingsToWin(int playersCount);
		Vector2Int GetStartingConfiguration(int playersCount, int playerIndex);
		float GetResourceRatio(ResourceType resourceType);
		Color GetUIColorForFaction(Faction faction);
	}
}