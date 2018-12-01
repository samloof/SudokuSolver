using NUnit.Framework;

namespace SudokuSolver
{
    [TestFixture]
    public class ValidationTests
    {
        private SudokuSolver sudokuSolver = null;
        private byte[,] board = null;

        [SetUp]
        public void TestSetup()
        {
            sudokuSolver = new SudokuSolver();

            board = new byte[,] {
                {0, 0, 0,  0, 0, 4,  0, 0, 3},
                {2, 0, 8,  0, 0, 6,  9, 0, 0},
                {0, 4, 0,  0, 0, 0,  0, 2, 5},

                {0, 0, 7,  0, 5, 0,  0, 0, 6},
                {0, 0, 0,  9, 0, 1,  0, 0, 0},
                {8, 0, 0,  0, 3, 0,  7, 0, 0},

                {5, 6, 0,  0, 0, 0,  0, 9, 0},
                {0, 0, 1,  3, 0, 0,  5, 0, 7},
                {7, 0, 0,  2, 0, 0,  0, 0, 0}};
        }

        [Test]
        [Description("Verifies that ValueMoreThanOnceRowCheck returns false when each none zero value appears only once on a row")]
        public void ValueMoreThanOnceRowCheckTest1()
        {
            var res = sudokuSolver.ValueMoreThanOnceRowCheck(board);
            Assert.IsFalse(res, "Expected false since the board doesn't contain same value more than once on a row.");
        }

        [Test]
        [Description("Verifies that ValueMoreThanOnceRowCheck returns true when one none zero value appears more than once on a row")]
        public void ValueMoreThanOnceRowCheckTest2()
        {
            board[8, 8] = 2; // Inserting another 2 on the last row

            var res = sudokuSolver.ValueMoreThanOnceRowCheck(board);
            Assert.IsTrue(res, "Expected true since the board contain same value more than once on a row.");
        }


        [Test]
        [Description("Verifies that ValueMoreThanOnceColCheck returns false when each none zero value appears only once in a column")]
        public void ValueMoreThanOnceColCheckTest1()
        {
            var res = sudokuSolver.ValueMoreThanOnceColCheck(board);
            Assert.IsFalse(res, "Expected false since the board doesn't contain same value more than once in a column.");
        }

        [Test]
        [Description("Verifies that ValueMoreThanOnceColCheck returns true when one none zero value appears more than once in a column")]
        public void ValueMoreThanOnceColCheckTest2()
        {
            board[8, 8] = 5; // Inserting another 5 in the last column

            var res = sudokuSolver.ValueMoreThanOnceColCheck(board);
            Assert.IsTrue(res, "Expected true since the board contain same value more than once in a column.");
        }

        [Test]
        [Description("Verifies that ValueMoreThanOnceBoxCheck returns false when each none zero value appears only once in a box")]
        public void ValueMoreThanOnceBoxCheckTest1()
        {
            var res = sudokuSolver.ValueMoreThanOnceBoxCheck(board);
            Assert.IsFalse(res, "Expected false since the board doesn't contain same value more than once in a box.");
        }

        [Test]
        [Description("Verifies that ValueMoreThanOnceBoxCheck returns true when one none zero value appears more than once in a box")]
        public void ValueMoreThanOnceBoxCheckTest2()
        {
            board[4, 8] = 7; // Inserting another 7 in a box already containing 7

            var res = sudokuSolver.ValueMoreThanOnceBoxCheck(board);
            Assert.IsTrue(res, "Expected true since the board contain same value more than once in a box.");
        }
    }
}
