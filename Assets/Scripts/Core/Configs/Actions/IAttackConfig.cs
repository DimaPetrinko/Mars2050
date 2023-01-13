namespace Core.Configs.Actions
{
	public interface IAttackConfig : IActionConfig
	{
		bool Repeatable { get; }
		double AttackRange { get; }

		int GetDamageForRoll(int roll);
	}
}