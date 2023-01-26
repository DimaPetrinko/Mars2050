namespace Core.Configs.Actions
{
	public interface IAttackConfig : IActionConfig
	{
		double AttackRange { get; }

		int GetDamageForRoll(int roll);
	}
}