using Core.Models.Enums;

namespace Core.Utils
{
	public static class ActionResultExtensions
	{
		public static bool IsSuccess(this ActionResult result)
		{
			return result == ActionResult.Success;
		}
	}
}