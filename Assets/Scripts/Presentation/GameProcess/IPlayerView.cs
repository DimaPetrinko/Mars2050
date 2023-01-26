using System;
using Core.Models.Enums;

namespace Presentation.GameProcess
{
	internal interface IPlayerView : IView
	{
		event Action EndTurnClicked;

		bool Active { set; }
		Faction Faction { set; }
		short Oxygen { set; }

		void UpdateResource(ResourceType type, int amount);
	}
}