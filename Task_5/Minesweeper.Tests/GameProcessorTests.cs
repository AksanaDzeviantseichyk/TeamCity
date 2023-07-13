using Minesweeper.Core;
using Minesweeper.Core.Enums;
using NUnit.Framework;
using System.Data.Common;

namespace Minesweeper.Tests
{
    [TestFixture]
    public class GameProcessorTests
    {
        private bool[,] boolField;

        [SetUp]
        public void SetUp()
        {
            boolField = new bool[,]
        {
            { false, false, false, false },
            { false, false, false, false },
            { false, false, false, false },
            { false, false, false, false }
        };
        }
        //Positive scenarios
        [TestCase(1, 1, new int[] { 0, 1, 3 }, new int[] { 0, 3, 1 })]
        public void Open_CellWithoutMine_Success(int row, int col, int[] mineRow, int[] mineCol)
        {
            // Precondition
            for(int i = 0; i < mineRow.Length; i++)
                boolField[mineRow[i], mineCol[i]] = true;
            var gameProcessor = new GameProcessor(boolField);

            // Action
            var gameState = gameProcessor.Open(row, col);

            // Assert
            Assert.AreEqual(GameState.Active, gameState, "Game state is not Active");

        }
        [TestCase(0, 0)]
        [Test]
        public void Open_CellWithMine_Lose(int row, int col)
        {
            // Precondition
            boolField[row, col] = true;
            var gameProcessor = new GameProcessor(boolField);

            // Action
            var gameState = gameProcessor.Open(row, col);

            // Assert
            Assert.AreEqual(GameState.Lose, gameState, "Game state is not Lose");
        }

        [TestCase(0, new int[] {}, new int[] {})]
        [TestCase(3, new int[] { 0, 1, 3 }, new int[] { 0, 3, 1 })]
        [TestCase(1, new int[] { 0, 1, 1, 2, 3 }, new int[] { 0, 0, 3, 2, 1 })]
        public void Open_AllFieldsWithoutMine_Win(int mineCount, int[] mineRow, int[] mineCol)
        {
            // Precondition
            for (int i = 0; i < mineCount; i++)
                boolField[mineRow[i], mineCol[i]] = true;

            // Action
            var gameState = OpenAllFieldsWithoutMine();

            // Assert
            Assert.AreEqual(GameState.Win, gameState, "Game state is not Win");

        }
        private GameState OpenAllFieldsWithoutMine()
        {
            var gameProcessor = new GameProcessor(boolField);
            GameState gameState = GameState.Active;
            for (int i = 0; i < boolField.GetLength(0); i++)
            {
                for (int j = 0; j < boolField.GetLength(1); j++)
                {
                    if (!(gameState == GameState.Active))
                        return gameState;
                    if (!boolField[i, j] && gameState == GameState.Active)
                        gameState = gameProcessor.Open(j, i);
                }

            }
            return gameState;
        }

        [TestCase(1, 1, 3, 2)]
        [Test]
        public void Open_SameFieldTwoTimes_SameStateActive(int rowOpen, int colOpen, int rowMine, int colMine)
        {
            // Precondition
            boolField[rowMine, colMine] = true;
            var gameProcessor = new GameProcessor(boolField);

            // Action
            gameProcessor.Open(rowOpen, colOpen);
            var gameState = gameProcessor.Open(rowOpen, colOpen);

            // Assert
            Assert.AreEqual(GameState.Active, gameState, "Game state is not Active");

        }
        [TestCase(2, 2, new int[] { 0 }, new int[] { 0 }, ExpectedResult = PointState.Neighbors0)]
        [TestCase(1, 1, new int[] {0, 0, 0}, new int[] {0, 1, 2}, ExpectedResult = PointState.Neighbors3)]
        [TestCase(1, 1, new int[] { 0, 0, 0, 2, 2, 2 }, new int[] { 0, 1, 2, 0, 1, 2 }, ExpectedResult = PointState.Neighbors6)]
        [TestCase(1, 1, new int[] { 0, 0, 0, 1, 1, 2, 2, 2 }, new int[] { 0, 1, 2, 0, 2, 0, 1, 2 }, ExpectedResult = PointState.Neighbors8)]
        // [Test]
        public PointState GetCurrentField_CountNumberOfMineNeighbors_NumberOfMineNeighborsFrom0To8(int row, int col, int[] mineRow, int[] mineCol)
        {
            // Precondition
            for (int i = 0; i < mineRow.Length; i++)
                boolField[mineRow[i], mineCol[i]] = true;
            var gameProcessor = new GameProcessor(boolField);

            // Action
            gameProcessor.Open(row, col);
            var currentField = gameProcessor.GetCurrentField();

            // Assert
            return currentField[row, col];
            
        }

        [Test]
        public void GetCurrentField_Initial_ReturnsClosedFields()
        {
            // Precondition
            var gameProcessor = new GameProcessor(boolField);

            // Action
            var currentField = gameProcessor.GetCurrentField();

            // Assert
            for(int i = 0; i < currentField.GetLength(0); i++) 
                for(int j = 0; j < currentField.GetLength(1); j++)
                    Assert.AreEqual(PointState.Close, currentField[i, j], "Point state is not Close");
            
        }

        [TestCase(true, 0, 0)]
        [Test]
        public void GetCurrentField_AfterOpenCellWithMine_ReturnsMinePointStatus(bool mine, int row, int col)
        {

            // Precondition
            boolField[row, col] = mine;
            var gameProcessor = new GameProcessor(boolField);
            
            // Action
            gameProcessor.Open(row, col);
            var currentField = gameProcessor.GetCurrentField();

            // Assert
            Assert.AreEqual(PointState.Mine, currentField[row, col], "Point state is not Mine");
            
        }

        //Negative scenarios
        [Test]
        public void Open_InvalidCell_ShouldThrowIndexOutOfRangeException()
        {
            //Precondition
            var gameProcessor = new GameProcessor(boolField);

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => gameProcessor.Open(10, 10), "Throw is not IndexOutOfRangeException");
        }

        [Test]
        public void Open_GameFinished_ShouldThrowInvalidOperationException()
        {
            //Precondition
            var gameProcessor = new GameProcessor(boolField);

            //Action
            gameProcessor.Open(2, 2);

            // Assert
            Assert.Throws<InvalidOperationException>(() => gameProcessor.Open(0, 0), "Throw is not InvalidOperationException");
        }

    }
}