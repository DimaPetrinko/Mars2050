namespace Presentation.Boards
{
	internal interface IBoardView : IView
	{
		ICellView CreateCell();
	}
}