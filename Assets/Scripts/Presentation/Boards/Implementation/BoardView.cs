using UnityEngine;

namespace Presentation.Boards.Implementation
{
	internal class BoardView : MonoBehaviour, IBoardView
	{
		[SerializeField] private CellView m_CellPrefab;

		public ICellView CreateCell()
		{
			return Instantiate(m_CellPrefab, transform);
		}
	}
}