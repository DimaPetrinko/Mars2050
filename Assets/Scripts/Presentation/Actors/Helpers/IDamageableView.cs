namespace Presentation.Actors.Helpers
{
	internal interface IDamageableView
	{
		int Health { set; }
		int MaxHealth { set; }
		bool HealthBarShown { set; }
	}
}