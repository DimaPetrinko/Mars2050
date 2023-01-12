using System.Collections.Generic;
using Core.Configs.Actions;
using Core.Models.Boards;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Services.GameProcess.Implementation;

namespace Core.Services.GameProcess
{
	public interface IActionProcessor
	{
		ActionResult Move(
			IPlayer performer,
			ICell from,
			ICell to,
			Dictionary<ResourceType, int> resourcesToUse,
			IMoveConfig config);

		ActionResult Discover(
			IPlayer performer,
			ICell cell,
			Dictionary<ResourceType, int> resourcesToUse,
			IDiscoverConfig config);

		ActionResult Gather(
			IPlayer performer,
			IEnumerable<ICell> cells,
			Dictionary<ResourceType, int> resourcesToUse,
			int roll,
			IGatherConfig config);

		ActionResult Build(
			IPlayer performer,
			ICell cell,
			Dictionary<ResourceType, int> resourcesToUse,
			IBuildConfig config);

		ActionResult Attack(
			IPlayer performer,
			ICell from,
			ICell to,
			Dictionary<ResourceType, int> resourcesToUse,
			int roll,
			bool repeat,
			IAttackConfig config);

		ActionResult Heal(
			IPlayer performer,
			ICell cell,
			Dictionary<ResourceType, int> resourcesToUse,
			bool repeat,
			IHealConfig config);

		public ActionResult Trade(
			IPlayer performer,
			Dictionary<ResourceType, int> resourcesToSell,
			Dictionary<ResourceType, int> resourcesToBuy,
			bool repeat,
			ITradeConfig config
		);

		ActionResult PlayerTrade(
			IPlayer performer,
			IResourceHolder other,
			Dictionary<ResourceType, int> resourcesToSell,
			Dictionary<ResourceType, int> resourcesToBuy,
			bool repeat,
			ITradeConfig config);
	}
}