using System;
using Core.Models.Enums;

namespace Core.Models.GameProcess
{
	public interface IPlayer : IResourceHolder, ITurnPerformer, ITechnologyUser, IModel
	{
		event Action<ActionType> ActionSelected;
		event Action ActionCanceled;

		void SelectAction(ActionType type);
		void CancelAction();
	}
}