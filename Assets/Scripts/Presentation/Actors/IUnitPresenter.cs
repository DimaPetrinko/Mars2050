using System;
using Core.Models.Actors;

namespace Presentation.Actors
{
	internal interface IUnitPresenter : IPresenter<IUnit, IUnitView>
	{
		event Action<IUnitPresenter> Selected;

		bool IsSelected { set; }

	}
}