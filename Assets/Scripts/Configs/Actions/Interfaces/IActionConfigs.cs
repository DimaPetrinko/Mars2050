namespace Configs.Actions.Interfaces
{
	public interface IActionConfigs
	{
		TConfig GetConfig<TConfig>(ActionType actionType) where TConfig : IActionConfig;
	}
}