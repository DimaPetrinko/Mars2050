using Core.Models.Enums;
using Core.Utils;

namespace Core.Models.Actions
{
	public interface IActionFactory : IFactory<IAction, ActionType>
	{
	}
}