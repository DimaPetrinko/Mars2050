using Core.Models.Boards;
using Core.Models.Enums;

namespace Core.Models.Actions
{
	public interface IDiscoverAction : IAction
	{
		ICell Cell { set; }

		ActionResult CheckTargetCell();
	}
}