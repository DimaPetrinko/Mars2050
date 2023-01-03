using System;
using System.Linq;
using Core.Configs.Actions;
using Core.Configs.Actions.Implementation;
using Core.Configs.Buildings;
using Core.Configs.Buildings.Implementation;
using UnityEngine;

namespace Core.Configs.Implementation
{
	[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Configs/" + nameof(GameConfig))]
	public class GameConfig : ScriptableObject, IGameConfig
	{
		[Serializable]
		public struct StartingConfiguration
		{
			public Vector2Int[] StartingPosition;
		}

		[Serializable]
		public struct WinConfiguration
		{
			public int PlayersCount;
			public int BuildingsToWin;
		}

		[SerializeField] private ActionConfigs m_Actions;
		[SerializeField] private BuildingConfigs m_BuildingConfigs;
		[SerializeField] private CameraConfig m_CameraConfig;
		[SerializeField] private int m_BoardRadius;
		[SerializeField] private float m_CellRadius;
		[SerializeField] private int m_UnitsPerPlayer;
		[SerializeField] private int m_MaxRoll;
		[SerializeField] private int m_MaxResources;
		[SerializeField] private int m_MaxUnitHealth;
		[SerializeField] private int m_MaxBuildingHealth;
		[SerializeField] private int m_MaxBuildingWithUnitHealth;
		[SerializeField] private WinConfiguration[] m_WinConfigurations;
		[SerializeField] private StartingConfiguration[] m_StartingConfigurations;

		public IActionConfigs Actions => m_Actions;
		public IBuildingConfigs BuildingConfigs => m_BuildingConfigs;
		public ICameraConfig CameraConfig => m_CameraConfig;
		public int BoardRadius => m_BoardRadius;
		public float CellRadius => m_CellRadius;
		public int UnitsPerPlayer => m_UnitsPerPlayer;
		public int MaxRoll => m_MaxRoll;
		public int MaxResources => m_MaxResources;
		public int MaxUnitHealth => m_MaxUnitHealth;
		public int MaxBuildingHealth => m_MaxBuildingHealth;
		public int MaxBuildingWithUnitHealth => m_MaxBuildingWithUnitHealth;

		public int GetBuildingsToWin(int playersCount)
		{
			return m_WinConfigurations.FirstOrDefault(c => c.PlayersCount == playersCount).BuildingsToWin;
		}

		public Vector2Int GetStartingConfiguration(int playersCount, int playerIndex)
		{
			return m_StartingConfigurations
				.FirstOrDefault(c => c.StartingPosition.Length == playersCount)
				.StartingPosition[playerIndex] * (m_BoardRadius - 1);
		}
	}
}