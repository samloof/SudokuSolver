using System;

namespace SudokuSolver.Exceptions
{
    public class InitialBoardIncorrectException : Exception
    {
        private static string generalMsg = "Initial board incorrect. ";

        public InitialBoardIncorrectException() : base(generalMsg)
        {

        }

        public InitialBoardIncorrectException(string msg) : base(generalMsg + msg)
        {

        }
    }
}
