using System;

namespace Presentation.Actions
{
	internal interface IActionButtonView : IView
	{
		event Action Clicked;

		string Name { set; }
		bool Selected { set; }
		bool Active { set; }
	}
}