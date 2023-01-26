using System;
using Core.Models.Boards;
using Presentation.Actors;

namespace Presentation.Boards
{
	internal interface ICellPresenter : IPresenter<ICell, ICellView>, IStandingSpotProvider
	{
		event Action<ICell> Clicked;
	}
}