using Core.Configs.Actions;
using Core.Configs.Buildings;
using UnityEngine;

namespace Core.Configs
{
	public interface IGameConfig
	{
		IActionConfigs Actions { get; }
		IBuildingConfigs BuildingConfigs { get; }
		int BoardRadius { get; }
		int UnitsPerPlayer { get; }
		int MaxRoll { get; }
		int MaxResources { get; }
		int MaxUnitHealth { get; }
		int MaxBuildingHealth { get; }
		int MaxBuildingWithUnitHealth { get; }

		int GetBuildingsToWin(int playersCount);
		Vector2Int GetStartingConfiguration(int playersCount, int playerIndex);
	}
}