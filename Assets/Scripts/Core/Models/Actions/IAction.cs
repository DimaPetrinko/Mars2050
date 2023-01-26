using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Utils;

namespace Core.Models.Actions
{
	public interface IAction : IModel
	{
		ActionType Type { get; }
		IReactiveProperty<bool> Selected { get; }
		ActionRepeatability Repeatability { get; }
		bool ResourcesRequired { get; }
		IPlayer Performer { get; set; }
		ResourcePackage Resources { set; }

		ActionResult Perform(bool repeat);
		ActionResult CheckAllConditions();
		ActionResult CheckPerformerSet();
		ActionResult CheckIfSelected();
		ActionResult CheckOxygen();
		ActionResult CheckPerformerResources();
		ActionResult CheckProvidedResources();
	}
}