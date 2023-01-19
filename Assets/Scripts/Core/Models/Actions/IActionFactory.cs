using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Utils;

namespace Core.Models.Actions
{
	public interface IActionFactory : IFactory<IAction, IPlayer, ActionType>
	{
	}
}