using System;

namespace SudokuSolver.Exceptions
{
    public class UnsolvableBoardException : Exception
    {
        private static string generalMsg = "Unsolvable board. The initial board has value(s) that are incorrect. ";

        public UnsolvableBoardException() : base(generalMsg)
        {

        }

        public UnsolvableBoardException(string msg) : base(generalMsg + msg)
        {

        }
    }
}
