using Core.Configs.Actions;
using Core.Models.Actors;
using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actions.Implementation
{
	public class DiscoverAction : BaseAction, IDiscoverAction
	{
		public ICell Cell { private get; set; }

		public DiscoverAction(
			IDiscoverConfig config
			) : base(config)
		{
			mConditions.Add(CheckTargetCell);
		}

		public ActionResult CheckTargetCell()
		{
			if (Cell == null) return ActionResult.NoCellProvided;
			if (!Cell.HasActor<IResourceDiscoverer>(Performer.Faction)) return ActionResult.NoMovableActorOfCorrectFactionInCell;
			if (!Cell.TryGetPlaceable(out IResource resource)) return ActionResult.NoResourceInCell;
			if (resource.IsDiscovered.Value) return ActionResult.ResourceAlreadyDiscovered;
			return ActionResult.Success;
		}

		protected override void ApplyAction()
		{
			var resource = Cell.GetPlaceable<IResource>();
			resource.IsDiscovered.Value = true;
		}

		protected override void ResetState()
		{
			base.ResetState();
			Cell = null;
		}
	}
}