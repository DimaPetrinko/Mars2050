using System.Collections.Generic;
using Core.Models.GameProcess;

namespace Core.Services.GameProcess
{
	public interface ITurnProcessor
	{
		IEnumerable<ITurnPerformer> TurnPerformers { get; }

		IDice Dice { get; }
	}
}