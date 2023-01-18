using Core.Models.Enums;

namespace Core.Models.Technology
{
	public interface ITechnology : IModel
	{
		TechnologyType Type { get; }
		bool Activated { get; }

		void TakeEffect();
	}
}