﻿using NUnit.Framework;
using System.Linq;

namespace SudokuSolver
{
    [TestFixture]
    public class SolveBoardTests
    {
        private SudokuSolver sudokuSolver = null;

        [SetUp]
        public void TestSetup()
        {
            sudokuSolver = new SudokuSolver();
        }

        [Test]
        public void BoardTest1()
        {
            var board = new byte[,] {
                {0, 0, 0,  0, 0, 4,  0, 0, 3},
                {2, 0, 8,  0, 0, 6,  9, 0, 0},
                {0, 4, 0,  0, 0, 0,  0, 2, 5},

                {0, 0, 7,  0, 5, 0,  0, 0, 6},
                {0, 0, 0,  9, 0, 1,  0, 0, 0},
                {8, 0, 0,  0, 3, 0,  7, 0, 0},

                {5, 6, 0,  0, 0, 0,  0, 9, 0},
                {0, 0, 1,  3, 0, 0,  5, 0, 7},
                {7, 0, 0,  2, 0, 0,  0, 0, 0}};

            var solution = new byte[,] {
                {9, 7, 5,  8, 2, 4,  1, 6, 3},
                {2, 3, 8,  5, 1, 6,  9, 7, 4},
                {1, 4, 6,  7, 9, 3,  8, 2, 5},

                {3, 9, 7,  4, 5, 8,  2, 1, 6},
                {6, 5, 2,  9, 7, 1,  3, 4, 8},
                {8, 1, 4,  6, 3, 2,  7, 5, 9},

                {5, 6, 3,  1, 8, 7,  4, 9, 2},
                {4, 2, 1,  3, 6, 9,  5, 8, 7},
                {7, 8, 9,  2, 4, 5,  6, 3, 1}};

            sudokuSolver.Solve(board);

            // Using Linq Cast to flatten the 2D arrays into linear arrays and can then use SequenceEqual
            bool equal = board.Cast<byte>().SequenceEqual(solution.Cast<byte>());
            Assert.True(equal, "Expecting bord to get solved as in the solution board.");
        }

        [Test]
        public void BoardTest2()
        {
            // Easy
            var board = new byte[,] {
                {3, 0, 0,  0, 8, 0,  2, 6, 5},
                {0, 0, 0,  4, 0, 3,  0, 0, 8},
                {1, 8, 0,  0, 0, 0,  0, 0, 3},

                {5, 0, 4,  0, 2, 0,  9, 7, 0},
                {0, 0, 0,  7, 3, 1,  0, 2, 0},
                {7, 1, 0,  0, 0, 0,  0, 0, 0},

                {0, 2, 3,  9, 0, 6,  4, 0, 0},
                {0, 0, 0,  0, 4, 0,  0, 8, 0},
                {0, 0, 6,  0, 5, 2,  3, 0, 0}};

            var solution = new byte[,] {
                {3, 4, 7,  1, 8, 9,  2, 6, 5},
                {2, 6, 5,  4, 7, 3,  1, 9, 8},
                {1, 8, 9,  2, 6, 5,  7, 4, 3},

                {5, 3, 4,  6, 2, 8,  9, 7, 1},
                {6, 9, 8,  7, 3, 1,  5, 2, 4},
                {7, 1, 2,  5, 9, 4,  8, 3, 6},

                {8, 2, 3,  9, 1, 6,  4, 5, 7},
                {9, 5, 1,  3, 4, 7,  6, 8, 2},
                {4, 7, 6,  8, 5, 2,  3, 1, 9}};

            sudokuSolver.Solve(board);

            bool equal = board.Cast<byte>().SequenceEqual(solution.Cast<byte>());
            Assert.True(equal, "Expecting bord to get solved as in the solution board.");
        }

        [Test]
        public void BoardTest3()
        {
            // Medium
            var board = new byte[,] {
                {6, 0, 0,  8, 0, 0,  0, 3, 1},
                {0, 1, 0,  3, 0, 0,  0, 0, 9},
                {0, 8, 7,  0, 1, 0,  0, 0, 0},

                {2, 0, 0,  0, 0, 0,  0, 0, 5},
                {0, 0, 0,  0, 5, 0,  4, 1, 3},
                {1, 0, 4,  6, 7, 0,  0, 0, 0},

                {0, 0, 6,  2, 0, 4,  0, 0, 0},
                {0, 7, 0,  0, 0, 0,  9, 8, 0},
                {0, 2, 5,  0, 8, 1,  3, 0, 0}};

            var solution = new byte[,] {
                {6, 4, 9,  8, 2, 5,  7, 3, 1},
                {5, 1, 2,  3, 6, 7,  8, 4, 9},
                {3, 8, 7,  4, 1, 9,  5, 2, 6},

                {2, 9, 3,  1, 4, 8,  6, 7, 5},
                {7, 6, 8,  9, 5, 2,  4, 1, 3},
                {1, 5, 4,  6, 7, 3,  2, 9, 8},

                {8, 3, 6,  2, 9, 4,  1, 5, 7},
                {4, 7, 1,  5, 3, 6,  9, 8, 2},
                {9, 2, 5,  7, 8, 1,  3, 6, 4}};

            sudokuSolver.Solve(board);

            bool equal = board.Cast<byte>().SequenceEqual(solution.Cast<byte>());
            Assert.True(equal, "Expecting bord to get solved as in the solution board.");
        }

        [Test]
        public void BoardTest4()
        {
            // Hard
            var board = new byte[,] {
                {8, 5, 0,  0, 0, 0,  0, 0, 0},
                {0, 0, 6,  7, 4, 0,  0, 0, 0},
                {0, 0, 0,  3, 0, 0,  0, 0, 0},

                {0, 0, 2,  0, 0, 0,  6, 0, 4},
                {0, 0, 0,  0, 0, 5,  0, 0, 2},
                {0, 8, 4,  0, 0, 0,  0, 0, 5},

                {0, 0, 0,  0, 0, 6,  1, 8, 0},
                {0, 0, 0,  2, 0, 4,  0, 6, 0},
                {6, 0, 7,  0, 0, 0,  0, 0, 0}};

            var solution = new byte[,] {
                {8, 5, 3,  6, 2, 9,  7, 4, 1},
                {9, 2, 6,  7, 4, 1,  3, 5, 8},
                {4, 7, 1,  3, 5, 8,  2, 9, 6},

                {5, 3, 2,  8, 9, 7,  6, 1, 4},
                {1, 6, 9,  4, 3, 5,  8, 7, 2},
                {7, 8, 4,  1, 6, 2,  9, 3, 5},

                {2, 4, 5,  9, 7, 6,  1, 8, 3},
                {3, 9, 8,  2, 1, 4,  5, 6, 7},
                {6, 1, 7,  5, 8, 3,  4, 2, 9}};

            sudokuSolver.Solve(board);

            bool equal = board.Cast<byte>().SequenceEqual(solution.Cast<byte>());
            Assert.True(equal, "Expecting bord to get solved as in the solution board.");
        }

        [Test]
        public void BoardTest5()
        {
            // Medium - Bonus
            var board = new byte[,] {
                {7, 0, 0,  0, 0, 0,  0, 5, 0},
                {5, 0, 0,  0, 0, 4,  1, 0, 6},
                {2, 6, 0,  0, 8, 0,  0, 9, 0},

                {0, 4, 8,  0, 3, 0,  0, 0, 1},
                {0, 2, 0,  0, 1, 6,  0, 0, 0},
                {0, 0, 0,  0, 2, 0,  3, 0, 5},

                {6, 5, 0,  2, 0, 0,  7, 0, 9},
                {0, 0, 7,  8, 6, 0,  0, 0, 0},
                {0, 0, 0,  1, 0, 0,  0, 0, 4}};

            var solution = new byte[,] {
                {7, 1, 4,  6, 9, 2,  8, 5, 3},
                {5, 8, 9,  3, 7, 4,  1, 2, 6},
                {2, 6, 3,  5, 8, 1,  4, 9, 7},

                {9, 4, 8,  7, 3, 5,  2, 6, 1},
                {3, 2, 5,  4, 1, 6,  9, 7, 8},
                {1, 7, 6,  9, 2, 8,  3, 4, 5},

                {6, 5, 1,  2, 4, 3,  7, 8, 9},
                {4, 3, 7,  8, 6, 9,  5, 1, 2},
                {8, 9, 2,  1, 5, 7,  6, 3, 4}};

            sudokuSolver.Solve(board);

            bool equal = board.Cast<byte>().SequenceEqual(solution.Cast<byte>());
            Assert.True(equal, "Expecting bord to get solved as in the solution board.");
        }

        [Test]
        public void BoardTest6()
        {
            var board = new byte[,] {
                {2, 0, 0,  5, 0, 7,  9, 0, 0},
                {0, 0, 0,  2, 0, 0,  0, 0, 5},
                {0, 8, 0,  0, 0, 0,  6, 0, 2},

                {0, 0, 0,  4, 0, 0,  2, 0, 8},
                {0, 2, 0,  0, 1, 0,  0, 3, 0},
                {7, 0, 4,  0, 0, 2,  0, 0, 0},

                {1, 0, 5,  0, 0, 0,  0, 2, 0},
                {3, 0, 0,  0, 0, 9,  0, 0, 0},
                {0, 0, 2,  6, 0, 8,  0, 0, 3}};

            var solution = new byte[,] {
                {2, 4, 3,  5, 6, 7,  9, 8, 1},
                {6, 1, 9,  2, 8, 4,  3, 7, 5},
                {5, 8, 7,  3, 9, 1,  6, 4, 2},

                {9, 3, 1,  4, 7, 6,  2, 5, 8},
                {8, 2, 6,  9, 1, 5,  4, 3, 7},
                {7, 5, 4,  8, 3, 2,  1, 9, 6},

                {1, 6, 5,  7, 4, 3,  8, 2, 9},
                {3, 7, 8,  1, 2, 9,  5, 6, 4},
                {4, 9, 2,  6, 5, 8,  7, 1, 3}};

            sudokuSolver.Solve(board);

            bool equal = board.Cast<byte>().SequenceEqual(solution.Cast<byte>());
            Assert.True(equal, "Expecting bord to get solved as in the solution board.");
        }
    }
}
