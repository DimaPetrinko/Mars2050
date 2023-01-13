using Core.Configs.Actions.Enums;

namespace Core.Configs.Actions
{
	public interface IActionConfigs
	{
		TConfig GetConfig<TConfig>(ActionType actionType) where TConfig : IActionConfig;
	}
}