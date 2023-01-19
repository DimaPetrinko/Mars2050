using Core.Models.Boards;

namespace Core.Models.Actions
{
	public interface IMoveAction : IAction
	{
		ICell From { set; }
		ICell To { set; }
	}
}