using System;
using Core.Models.Enums;

namespace Presentation.GameProcess
{
	internal interface IPlayerView : IView
	{
		event Action<ActionType> ActionClicked;
		event Action EndTurnClicked;

		bool Active { set; }
		Faction Faction { set; }
		int Oxygen { set; }
		ActionType? SelectedAction { set; }

		void UpdateResource(ResourceType type, int amount);
	}
}