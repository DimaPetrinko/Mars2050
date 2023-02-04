using System;
using Core.Models.Enums;
using Core.Utils;

namespace Presentation.Actions
{
	internal interface ISimpleActionView : IView
	{
		event Action ResourcesChanged;
		event Action Confirmed;
		event Action Closed;

		bool Active { set; }
		ActionResult Result { set; }
		ResourcePackage Resources { get; set; }
		ResourcePackage MaxResources { set; }
		bool CanConfirm { set; }
		bool ResourcesRequired { set; }
	}
}