using Core.Configs;
using Core.Configs.Actions;
using Core.Models.Enums;
using Core.Models.GameProcess;

namespace Core.Models.Actions.Implementation
{
	public class ActionFactory : IActionFactory
	{
		private readonly IGameConfig mGameConfig;
		private readonly IActionConfigs mActionConfigs;
		private readonly IDice mDice;

		public ActionFactory(
			IGameConfig gameConfig,
			IActionConfigs actionConfigs,
			IDice dice)
		{
			mGameConfig = gameConfig;
			mActionConfigs = actionConfigs;
			mDice = dice;
		}

		public IAction Create(ActionType type)
		{
			var config = mActionConfigs.GetConfig(type);
			return type switch
			{
				ActionType.Move => new MoveAction((IMoveConfig)config, mGameConfig.MaxBuildingWithUnitHealth),
				// ActionType.Discover => new DiscoverAction(performer, config),
				// ActionType.Gather => new GatherAction(performer, config, mDice),
				// ActionType.Build => new BuildAction(performer, config),
				// ActionType.Attack => new AttackAction(performer, config, mDice),
				// ActionType.Heal => new HealAction(performer, config),
				// ActionType.Trade => new TradeAction(performer, config),
				// ActionType.PlayerTrade => new PlayerTradeAction(performer, config),
				_ => null
			};
		}
	}
}