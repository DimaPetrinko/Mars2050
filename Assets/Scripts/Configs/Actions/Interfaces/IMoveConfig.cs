namespace Configs.Actions.Interfaces
{
	public interface IMoveConfig : IActionConfig
	{
		int MoveRange { get; }
		bool CanMoveToOccupiedCell { get; }
		bool CanMoveToDamagedBuilding { get; }
	}
}