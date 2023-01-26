namespace Core.Models.Enums
{
	public enum ActionResult
	{
		PerformerNotSet,
		NotSelected,
		NotEnoughOxygen,
		NotEnoughResources,
		NoResourcesProvided,
		NoMovableInCell,
		NoMovableActorOfCorrectFactionInCell,
		CellIsOccupied,
		CellHasDamagedBuilding,
		ExceedsRange,
		NoResourceInCell,
		ResourceAlreadyDiscovered,
		SameCell,
		NoCellProvided,
		Success
	}
}