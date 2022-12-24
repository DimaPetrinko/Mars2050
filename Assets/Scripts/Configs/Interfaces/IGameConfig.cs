using Configs.Actions.Interfaces;
using Configs.Buildings.Interfaces;
using UnityEngine;

namespace Configs.Interfaces
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
		Vector3Int GetStartingConfiguration(int playersCount, int playerIndex);
	}
}