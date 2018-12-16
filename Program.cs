using System;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuSolver sudokuSolver = new SudokuSolver();

            // 0 represents a cell with no value set
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

            try
            {
                sudokuSolver.Solve(board);
                Console.WriteLine("Solution:\n");
                sudokuSolver.WriteBoard(board);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
