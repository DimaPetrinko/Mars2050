using Core.Models.Enums;
using UnityEngine;

namespace Presentation.Boards
{
	internal interface IResourceView : IView
	{
		bool IsDiscovered { set; }
		ResourceType Type { set; }
		Transform Cell { set; }
	}
}