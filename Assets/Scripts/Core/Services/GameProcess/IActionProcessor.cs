using System.Collections.Generic;
using Core.Configs.Actions.Interfaces;
using Core.Models.Boards;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Services.GameProcess.Implementation;

namespace Core.Services.GameProcess
{
	public interface IActionProcessor
	{
		ActionResult Move(IPlayer performer, ICell from, ICell to, IMoveConfig config);
		ActionResult Discover(IPlayer performer, ICell cell, IDiscoverConfig config);
		ActionResult Gather(IPlayer performer, IEnumerable<ICell> cells, int roll, IGatherConfig config);
		ActionResult Build(IPlayer performer, ICell cell, IBuildConfig config);
		ActionResult Attack(IPlayer performer, ICell from, ICell to, bool repeat, int roll, IAttackConfig config);
		ActionResult Heal(IPlayer performer, ICell cell, bool repeat, IHealConfig config);
		ActionResult Trade(IPlayer performer, ResourceType from, ResourceType to, ITradeConfig config);
		ActionResult PlayerTrade(IPlayer performer, IResourceHolder other, ResourceType from, ResourceType to,
			ITradeConfig config);
	}
}