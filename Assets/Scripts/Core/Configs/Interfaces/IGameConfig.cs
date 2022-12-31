using Core.Configs.Actions.Interfaces;
using Core.Configs.Buildings.Interfaces;
using UnityEngine;

namespace Core.Configs.Interfaces
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