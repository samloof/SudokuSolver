using NUnit.Framework;

namespace SudokuSolver
{
    [TestFixture]
    public class FinalBoardTests
    {
        private SudokuSolver sudokuSolver = null;
        private byte[,] board = null;

        [SetUp]
        public void TestSetup()
        {
            sudokuSolver = new SudokuSolver();

            board = new byte[,] {
                {2, 4, 3,  5, 6, 7,  9, 8, 1},
                {6, 1, 9,  2, 8, 4,  3, 7, 5},
                {5, 8, 7,  3, 9, 1,  6, 4, 2},

                {9, 3, 1,  4, 7, 6,  2, 5, 8},
                {8, 2, 6,  9, 1, 5,  4, 3, 7},
                {7, 5, 4,  8, 3, 2,  1, 9, 6},

                {1, 6, 5,  7, 4, 3,  8, 2, 9},
                {3, 7, 8,  1, 2, 9,  5, 6, 4},
                {4, 9, 2,  6, 5, 8,  7, 1, 3}};
        }

        [Test]
        [Description("Verifies that IsSolution returns false when board still contains 0")]
        public void IsSolution()
        {
            board[8, 8] = 0; // 0 in last cell

            var res = sudokuSolver.IsRowsSolution(board);
            Assert.IsFalse(res, "Expected false since the board still contains 0.");
        }

        [Test]
        [Description("Verifies that IsRowsSolution returns true when 1-9 appears on each row of the board")]
        public void IsRowsSolutionTest1()
        {
            var res = sudokuSolver.IsRowsSolution(board);
            Assert.IsTrue(res, "Expected true since the board contains 1-9 on each row.");
        }

        [Test]
        [Description("Verifies that IsRowsSolution returns false when 1-9 doesn't appear on each row of the board")]
        public void IsRowsSolutionTest2()
        {
            board[6, 0] = 9; // Inserting another 9 on row 7

            var res = sudokuSolver.IsRowsSolution(board);
            Assert.IsFalse(res, "Expected false since the board doesn't contains 1-9 on each row.");
        }

        [Test]
        [Description("Verifies that IsColumnsSolution returns true when 1-9 appears in each column of the board")]
        public void IsColumnsSolutionTest1()
        {
            var res = sudokuSolver.IsColumnsSolution(board);
            Assert.IsTrue(res, "Expected true since the board contains 1-9 in each column.");
        }

        [Test]
        [Description("Verifies that IsColumnsSolution returns false when 1-9 doesn't appear in each column of the board")]
        public void IsColumnsSolutionTest2()
        {
            board[1, 1] = 6; // Inserting another 6 in column 2

            var res = sudokuSolver.IsRowsSolution(board);
            Assert.IsFalse(res, "Expected false since the board doesn't contains 1-9 in each column.");
        }

        [Test]
        [Description("Verifies that IsBoxSolution returns true when 1-9 appears in each box of the board")]
        public void IsBoxSolutionTest1()
        {
            var res = sudokuSolver.IsBoxSolution(board);
            Assert.IsTrue(res, "Expected true since the board contains 1-9 in each box.");
        }

        [Test]
        [Description("Verifies that IsBoxSolution returns false when 1-9 doesn't appear in each box of the board")]
        public void IsBoxSolutionTest2()
        {
            board[2, 7] = 1; // Inserting another 1 in box 3

            var res = sudokuSolver.IsBoxSolution(board);
            Assert.IsFalse(res, "Expected false since the board doesn't contains 1-9 in each box.");
        }
    }
}
