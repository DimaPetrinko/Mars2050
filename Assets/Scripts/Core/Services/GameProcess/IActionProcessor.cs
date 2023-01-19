using System.Collections.Generic;
using Core.Configs.Actions;
using Core.Models.Boards;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Utils;

namespace Core.Services.GameProcess
{
	public interface IActionProcessor
	{
		ActionResult Move(
			IPlayer performer,
			ICell from,
			ICell to,
			ResourcePackage resourcesToUse,
			int combinedHealth,
			IMoveConfig config);

		ActionResult Discover(
			IPlayer performer,
			ICell cell,
			ResourcePackage resourcesToUse,
			IDiscoverConfig config);

		// TODO: for those actions that require roll:
		// extract roll to a separate model
		// when rollable action is requested
		// trigger roll
		// the roll view will show the roll animation
		// then proceed and call the action processor method
		ActionResult Gather(
			IPlayer performer,
			IEnumerable<ICell> cells,
			ResourcePackage resourcesToUse,
			int roll,
			IGatherConfig config);

		ActionResult Build(
			IPlayer performer,
			ICell cell,
			ResourcePackage resourcesToUse,
			IBuildConfig config);

		ActionResult Attack(
			IPlayer performer,
			ICell from,
			ICell to,
			ResourcePackage resourcesToUse,
			int roll,
			bool repeat,
			IAttackConfig config);

		ActionResult Heal(
			IPlayer performer,
			ICell cell,
			ResourcePackage resourcesToUse,
			bool repeat,
			IHealConfig config);

		public ActionResult Trade(
			IPlayer performer,
			ResourcePackage resourcesToSell,
			ResourcePackage resourcesToBuy,
			bool repeat,
			ITradeConfig config
		);

		ActionResult PlayerTrade(
			IPlayer performer,
			IResourceHolder other,
			ResourcePackage resourcesToSell,
			ResourcePackage resourcesToBuy,
			bool repeat,
			ITradeConfig config);
	}
}