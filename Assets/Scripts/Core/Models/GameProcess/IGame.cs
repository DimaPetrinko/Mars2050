using System.Collections.Generic;
using Core.Services.GameProcess;

namespace Core.Models.GameProcess
{
	public interface IGame : IModel
	{
		IEnumerable<IPlayer> Players { get; }
		ITurnProcessor TurnProcessor { get; }
		IActionProcessor ActionProcessor { get; }

		void InitializeBoard();
		void SpawnResources();
		void InitializePlayers();
		void StartTurns();
		void EndGame();
	}
}