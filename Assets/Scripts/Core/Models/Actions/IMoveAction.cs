using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actions
{
	public interface IMoveAction : IAction
	{
		ICell From { set; }
		ICell To { set; }

		ActionResult CheckStartCell();
		ActionResult CheckDestinationCell();
		ActionResult CheckMoveRange();
	}
}