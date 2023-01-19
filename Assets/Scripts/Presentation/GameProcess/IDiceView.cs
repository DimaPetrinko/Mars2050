namespace Presentation.GameProcess
{
	internal interface IDiceView : IView
	{
		bool Active { set; }
		byte Roll { set; }
	}
}