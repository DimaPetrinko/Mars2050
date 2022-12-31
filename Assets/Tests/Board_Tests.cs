using System.Linq;
using Core.Models.Boards;
using Core.Models.Boards.Implementation;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	public class Board_Tests
	{
		private IBoard mBoard;

		[SetUp]
		public void SetUp()
		{
			mBoard = new Board(5);
		}

		[Test]
		public void Radius_IsAssigned()
		{
			IBoard board = new Board(5);

			Assert.AreEqual(5, board.Radius);
		}

		[Test]
		public void Cells_Returns7Cell_WhenRadius2()
		{
			IBoard board = new Board(2);

			Assert.AreEqual(7, board.Cells.Count());
		}

		[Test]
		public void Cells_Returns19Cells_WhenRadius3()
		{
			IBoard board = new Board(3);

			Assert.AreEqual(19, board.Cells.Count());
		}

		[Test]
		public void Cells_Returns37Cells_WhenRadius4()
		{
			IBoard board = new Board(4);

			Assert.AreEqual(37, board.Cells.Count());
		}

		[Test]
		public void Cells_ReturnsNonNullCells()
		{
			IBoard board = new Board(4);

			Assert.True(board.Cells.All(c => c != null));
		}

		[Test] public void GetCell_ReturnsCell_WhenPositionInsideRadiusGiven()
		{
			var cell = mBoard.GetCell(new Vector2Int());

			Assert.IsNotNull(cell);
		}

		[Test]
		public void GetCell_ReturnsNull_WhenPositionOutsideRadiusGiven()
		{
			var cell = mBoard.GetCell(new Vector2Int(int.MaxValue, int.MaxValue));

			Assert.IsNull(cell);
		}

		[Test]
		public void GetCell_ReturnsCell_WhenValidPositionGiven()
		{
			var cell = mBoard.GetCell(new Vector2Int(4, 0));

			Assert.IsNotNull(cell);
		}

		[Test]
		public void GetCell_ReturnsNull_WhenInvalidPositionGiven()
		{
			var cell = mBoard.GetCell(new Vector2Int(4, 4));

			Assert.IsNull(cell);
		}

		[Test]
		public void TryGetCell_ReturnsTrue_WhenPositionInsideRadiusGiven()
		{
			Assert.IsTrue(mBoard.TryGetCell(out _, new Vector2Int()));
		}

		[Test]
		public void TryGetCell_ReturnsFalse_WhenPositionOutsideRadiusGiven()
		{
			Assert.IsFalse(mBoard.TryGetCell(out _, new Vector2Int(int.MaxValue, int.MaxValue)));
		}

		[Test]
		public void TryGetCell_ReturnsTrue_WhenValidPositionGiven()
		{
			Assert.IsTrue(mBoard.TryGetCell(out _, new Vector2Int(4, 0)));
		}

		[Test]
		public void TryGetCell_ReturnsFalse_WhenInvalidPositionGiven()
		{
			Assert.IsFalse(mBoard.TryGetCell(out _, new Vector2Int(4, 4)));
		}

		[Test]
		public void TryGetCell_ReturnsCell_WhenPositionInsideRadiusGiven()
		{
			mBoard.TryGetCell(out var cell, new Vector2Int());

			Assert.IsNotNull(cell);
		}

		[Test]
		public void TryGetCell_ReturnsNull_WhenPositionOutsideRadiusGiven()
		{
			mBoard.TryGetCell(out var cell, new Vector2Int(int.MaxValue, int.MaxValue));

			Assert.IsNull(cell);
		}

		[Test]
		public void TryGetCell_ReturnsCell_WhenValidPositionGiven()
		{
			mBoard.TryGetCell(out var cell, new Vector2Int(4, 0));

			Assert.IsNotNull(cell);
		}

		[Test]
		public void TryGetCell_ReturnsNull_WhenInvalidPositionGiven()
		{
			mBoard.TryGetCell(out var cell, new Vector2Int(4, 4));

			Assert.IsNull(cell);
		}

		[Test]
		public void InRange_ReturnsTrue_WhenCorrectPositionGiven()
		{
			Assert.IsTrue(mBoard.InRange(new Vector2Int(4, 0), 6));
		}

		[Test]
		public void InRange_ReturnsFalse_WhenIncorrectPositionGiven()
		{
			Assert.IsFalse(mBoard.InRange(new Vector2Int(4, 0), 2));
		}
	}
}