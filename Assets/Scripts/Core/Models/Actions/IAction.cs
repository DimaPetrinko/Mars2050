using Core.Models.Enums;
using Core.Utils;

namespace Core.Models.Actions
{
	public interface IAction : IModel
	{
		ActionResult Perform(bool repeat);
		ResourcePackage Resources { set; }
	}
}