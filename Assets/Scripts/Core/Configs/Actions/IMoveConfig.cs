namespace Core.Configs.Actions
{
	public interface IMoveConfig : IActionConfig
	{
		int MoveRange { get; }
		bool CanMoveToOccupiedCell { get; }
		bool CanMoveToDamagedBuilding { get; }
	}
}