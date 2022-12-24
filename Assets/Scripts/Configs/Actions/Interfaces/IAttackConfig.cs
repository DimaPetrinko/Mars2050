namespace Configs.Actions.Interfaces
{
	public interface IAttackConfig : IActionConfig
	{
		bool Repeatable { get; }
		int GetDamageForRoll(int roll);
	}
}