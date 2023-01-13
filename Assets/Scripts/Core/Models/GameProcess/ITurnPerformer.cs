using Core.Models.Enums;

namespace Core.Models.GameProcess
{
	public interface ITurnPerformer
	{
		Faction Faction { get; }
		int Oxygen { get; set; }

		void UseOxygen(int oxygen);
		int Roll();
		void EndTurn();
	}
}