using Core.Models.Enums;

namespace Core.Models.GameProcess
{
	public interface ITurnPerformer
	{
		Faction Faction { get; }
		IReactiveProperty<int> Oxygen { get; }
		IReactiveProperty<bool> HisTurn { get; }
	}
}