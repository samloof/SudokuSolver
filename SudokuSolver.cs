using System;
using System.Collections.Generic;
using System.Linq;

/*
 * NAME:
 *    Sudoku Solver - solves Sudoku boards.
 * 
 * VERSION/DATE:
 *    2018-12-01
 *   
 * DESCRIPTION:
 *    A 3D array is first constructed where 2 dimensions represent the board and the 3:rd represents possible values for each cell.
 *    A value for a cell is set on the board if there is only one possible value for that cell. Possible values for all affected cells are then recalculated.
 *    The 3D array is then used to test different values on the board until a solution is found.
 * 
 * EXIT STATUS:
 *    1    Indicates that there are no possible values for cell and that the initial board has value(s) that are incorrect.
 *    2    Initial board incorrect: more than one value in same column, row or box
 * 
 * AUTHOR:
 *    Written by Sam Lööf.
 *    
 * TODO:
 * Check that all cells have possible values
 * Check for the solved board:
 * - no zeros in the solution
 * - number shall not appar several times in row, column or box
 */

namespace SudokuSolver
{
    public class SudokuSolver
    {
        const byte MAX = 9;
        byte currentRow = 0;
        byte currentCol = 0;
        
        public void Solve(byte[,] board)
        {
            CheckInitialBoard(board);

            byte[][][] possibleValues = GetPossibleValues(board);

            // Check if the solution is already found
            if (NoCellsWithValueZero(board))
            {
                return;
            }

            byte currentCell = 0;
            bool forward = true;
            while (!(currentCell > (MAX * MAX - 1) && forward))
            {
                if (possibleValues[currentCell / MAX][currentCell % MAX] == null)
                {
                    Move(forward, ref currentCell);
                    continue;
                }

                if (board[currentCell / MAX, currentCell % MAX] == 0)
                {
                    SetValueOnBoardAndDirection(board, possibleValues, currentCell, ref forward, 0);
                }
                else
                {
                    byte currentIndexPossibleValue;
                    for (currentIndexPossibleValue = 0; currentIndexPossibleValue < possibleValues[currentCell / MAX][currentCell % MAX].Count(); currentIndexPossibleValue++)
                    {
                        if (board[currentCell / MAX, currentCell % MAX] == possibleValues[currentCell / MAX][currentCell % MAX][currentIndexPossibleValue])
                        {
                            break;
                        }
                    }

                    if (currentIndexPossibleValue + 1 == possibleValues[currentCell / MAX][currentCell % MAX].Count())
                    {
                        forward = false;
                        board[currentCell / MAX, currentCell % MAX] = 0;
                    }
                    else
                    {
                        SetValueOnBoardAndDirection(board, possibleValues, currentCell, ref forward, (byte)(currentIndexPossibleValue + 1));
                    }
                }

                Move(forward, ref currentCell);
            }
        }

        #region Verification

        /// <summary>
        /// Initial check if the board’s initial state is incorrect, e.g. if the same number appears twice on the same row
        /// </summary>
        private void CheckInitialBoard(byte[,] board)
        {
            if (ValueMoreThanOnceRowCheck(board))
            {
                CheckInitialBoardError("Initial board incorrect: more than one value on same row.");
            }
            if (ValueMoreThanOnceColCheck(board))
            {
                CheckInitialBoardError("Initial board incorrect: more than one value in same column.");
            }
            if (ValueMoreThanOnceBoxCheck(board))
            {
                CheckInitialBoardError("Initial board incorrect: more than one value in same box.");
            }
        }

        private void CheckInitialBoardError(string msg)
        {
            Console.WriteLine(msg);
            Console.ReadLine();
            Environment.Exit(2);
        }

        /// <summary>
        /// Returns true if a non zero number appears 2 or more times on same row
        /// </summary>
        public bool ValueMoreThanOnceRowCheck(byte[,] board)
        {
            for (byte row = 0; row < MAX; row++)
            {
                for (byte col = 0; col < MAX - 1; col++) // Note that we only go to the next last column
                {
                    if (board[row, col] == 0)
                    {
                        continue;
                    }

                    var searchFor = board[row, col];
                    for (var remainingCol = col + 1; remainingCol < MAX; remainingCol++)
                    {
                        if (board[row, remainingCol] == searchFor)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if a non zero number appears 2 or more times in same column
        /// </summary>
        public bool ValueMoreThanOnceColCheck(byte[,] board)
        {
            for (byte col = 0; col < MAX; col++)
            {
                for (byte row = 0; row < MAX - 1; row++) // Note that we only go to the next last row
                {
                    if (board[row, col] == 0)
                    {
                        continue;
                    }

                    var searchFor = board[row, col];
                    for (var remainingRow = row + 1; remainingRow < MAX; remainingRow++)
                    {
                        if (board[remainingRow, col] == searchFor)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if a non zero number appears 2 or more times in same box
        /// </summary>
        public bool ValueMoreThanOnceBoxCheck(byte[,] board)
        {
            for (byte row = 0; row < MAX; row++)
            {
                for (byte col = 0; col < MAX; col++)
                {
                    if (board[row, col] == 0)
                    {
                        continue;
                    }

                    var searchFor = board[row, col];
                    if (NumberOccurrencesOfValueInBox(board, row, col, searchFor) > 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns number of occurrences of value in the box specified by (row, col)
        /// </summary>>
        private byte NumberOccurrencesOfValueInBox(byte[,] board, byte row, byte col, byte value)
        {
            byte numberOccurrencesOfValueInBox = 0;
            byte upperLeftCornerRow = 0, upperLeftCornerCol = 0;

            FindUpperLeftCornerForBox(row, col, ref upperLeftCornerRow, ref upperLeftCornerCol);

            for (byte r = upperLeftCornerRow; r < (upperLeftCornerRow + 3); r++)
            {
                for (byte c = upperLeftCornerCol; c < (upperLeftCornerCol + 3); c++)
                {
                    if (board[r, c] == value)
                        numberOccurrencesOfValueInBox++;
                }
            }
            return numberOccurrencesOfValueInBox;
        }

        #endregion

        #region SolveBoard

        /// <summary>
        /// Returns true if there are no cells on the board with value 0, otherwise false is returned
        /// </summary>
        private bool NoCellsWithValueZero(byte[,] board)
        {
            for (byte row = 0; row < MAX; row++)
            {
                for (byte col = 0; col < MAX; col++)
                {
                    if (board[row, col] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Tests values for cell (currentCell / MAX, currentCell % MAX) on the board based on possibleValues with start index currentIndexPossibleValue.
        /// Forward is set to true if there is a possible value the cell.
        /// Forward is set to false if there were no possible value for the cell and the board value for the cell is reset to 0.
        /// </summary>
        private void SetValueOnBoardAndDirection(byte[,] board, byte[][][] possibleValues, byte currentCell, ref bool forward, byte currentIndexPossibleValue)
        {
            for (byte i = currentIndexPossibleValue; i < possibleValues[currentCell / MAX][currentCell % MAX].Count(); i++)
            {
                board[currentCell / MAX, currentCell % MAX] = possibleValues[currentCell / MAX][currentCell % MAX][i];

                // Direction forward if there was no conflict
                if (!ConflictforValue(board, (byte)(currentCell / MAX), (byte)(currentCell % MAX), possibleValues))
                {
                    forward = true;
                    return;
                }
                // Set board cell value to 0 when going backwards
                else if (i + 1 == possibleValues[currentCell / MAX][currentCell % MAX].Count())
                {
                    forward = false;
                    board[currentCell / MAX, currentCell % MAX] = 0;
                }
            }
        }

        /// <summary>
        /// Updates currentCell with +1 for forward=true and -1 for forward=false
        /// </summary>
        private void Move(bool forward, ref byte currentCell)
        {
            if (forward)
            {
                currentCell++;
            }
            else
            {
                currentCell--;
            }
        }

        /// <summary>
        /// Returns true if the value in cell (rowValueSet, colValueSet) has generated a conflict with another cell on the same row, cell or box
        /// </summary>
        private bool ConflictforValue(byte[,] board, byte rowValueSet, byte colValueSet, byte[][][] possibleValues)
        {
            byte valueSet = board[rowValueSet, colValueSet];

            for (byte col = 0; col < MAX; col++)
            {
                if (possibleValues[rowValueSet][col] == null || col == colValueSet)
                {
                    continue;
                }
                if (board[rowValueSet, col] == valueSet)
                {
                    return true;
                }
            }
            for (byte row = 0; row < MAX; row++)
            {
                if (possibleValues[row][colValueSet] == null || row == rowValueSet)
                {
                    continue;
                }
                if (board[row, colValueSet] == valueSet)
                {
                    return true;
                }
            }

            byte upperLeftCornerRow = 0, upperLeftCornerCol = 0;
            FindUpperLeftCornerForBox(rowValueSet, colValueSet, ref upperLeftCornerRow, ref upperLeftCornerCol);

            for (byte row = upperLeftCornerRow; row < upperLeftCornerRow + 3; row++)
            {
                for (byte col = upperLeftCornerCol; col < upperLeftCornerCol + 3; col++)
                {
                    if (possibleValues[row][col] == null || row == rowValueSet || col == colValueSet)
                    {
                        continue;
                    }
                    if (board[row, col] == valueSet)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region PossibleValuesEtc

        /// <summary>
        /// Creates a 3D array where 2 dimensions represent the board and the third the possible values for each cell.
        /// null is used for those cells with predefined values and for cells with only one possible value.
        /// Note that this method will update the board when there are only one possible value for a cell.
        /// </summary>
        private byte[][][] GetPossibleValues(byte[,] board)
        {
            byte[][][] possibleValues = new byte[MAX][][];

            // Important to create the 2D array before setting possible values for each cell (to avoid NullReferenceException)
            for (currentRow = 0; currentRow < MAX; currentRow++)
            {
                possibleValues[currentRow] = new byte[MAX][];
            }

            for (currentRow = 0; currentRow < MAX; currentRow++)
            {
                for (currentCol = 0; currentCol < MAX; currentCol++)
                {
                    if (board[currentRow, currentCol] == 0)
                    {
                        SetPossibleValuesForCell(board, currentRow, currentCol, possibleValues);
                    }
                    else
                    {
                        possibleValues[currentRow][currentCol] = null;
                    }
                }
            }

            return possibleValues;
        }

        /// <summary>
        /// Sets the possible values for cell (row, col) in the board.
        /// setValueOnBoardAndUpdatePossibleValues is called if there is only one possible value for the cell.
        /// Note that this method exits the program if there are no possible values for a cell since the board doesn't have a solution if that's the case.
        /// </summary>
        private void SetPossibleValuesForCell(byte[,] board, byte row, byte col, byte[][][] possibleValues)
        {
            possibleValues[row][col] = GetPossibleValuesForACell(board, row, col);
            if (possibleValues[row][col].Count() == 0)
            {
                Console.WriteLine("No possible values for cell (row, col) = (" + (row + 1) + ", " + (col + 1) + "). The initial board has value(s) that are incorrect.");
                Environment.Exit(1);
                Console.ReadLine();
            }
            else if (possibleValues[row][col].Count() == 1)
            {
                SetValueOnBoardAndUpdatePossibleValues(board, row, col, possibleValues);
            }
        }

        /// <summary>
        /// Sets the value for the cell (rowOnePossibleValue, colOnePossibleValue) in the board that only has one possible value.
        /// Possible values are recalculated for the row, column and box that might be effected by the value set in the board.
        /// </summary>
        private void SetValueOnBoardAndUpdatePossibleValues(byte[,] board, byte rowOnePossibleValue, byte colOnePossibleValue, byte[][][] possibleValues)
        {
            board[rowOnePossibleValue, colOnePossibleValue] = possibleValues[rowOnePossibleValue][colOnePossibleValue][0];
            possibleValues[rowOnePossibleValue][colOnePossibleValue] = null;

            // Recalculate possible values for the row, cell and box
            for (byte col = 0; (rowOnePossibleValue * MAX + col < currentRow * MAX + currentCol) && col < MAX; col++)
            {
                if (possibleValues[rowOnePossibleValue][col] == null)
                {
                    continue;
                }
                SetPossibleValuesForCell(board, rowOnePossibleValue, col, possibleValues);
            }
            for (byte row = 0; (row * MAX + colOnePossibleValue < currentRow * MAX + currentCol) && row < MAX; row++)
            {
                if (possibleValues[row][colOnePossibleValue] == null)
                {
                    continue;
                }
                SetPossibleValuesForCell(board, row, colOnePossibleValue, possibleValues);
            }

            byte upperLeftCornerRow = 0, upperLeftCornerCol = 0;
            FindUpperLeftCornerForBox(rowOnePossibleValue, colOnePossibleValue, ref upperLeftCornerRow, ref upperLeftCornerCol);

            bool breakLoop = false;
            for (byte row = upperLeftCornerRow; row < (upperLeftCornerRow + 3) && !breakLoop; row++)
            {
                for (byte col = upperLeftCornerCol; col < upperLeftCornerCol + 3; col++)
                {
                    if (row == currentRow && col == currentCol)
                    {
                        breakLoop = true;
                        break;
                    }

                    if (possibleValues[row][col] == null || row == rowOnePossibleValue || col == colOnePossibleValue)
                    {
                        continue;
                    }

                    SetPossibleValuesForCell(board, row, col, possibleValues);
                }
            }
        }

        /// <summary>
        /// Returns an array of possible values for cell (row, col) based on the board
        /// </summary>
        private byte[] GetPossibleValuesForACell(byte[,] board, byte row, byte col)
        {
            List<byte> possibleValues = new List<byte>();
            for (byte value = 1; value <= MAX; value++)
            {
                if (ValueOnRow(board, row, value) || ValueInCol(board, col, value) || ValueInBox(board, row, col, value))
                {
                    continue;
                }
                possibleValues.Add(value);
            }

            return possibleValues.ToArray();
        }

        /// <summary>
        /// Returns true if value is found on the specified row on the board
        /// </summary>
        private bool ValueOnRow(byte[,] board, byte row, byte value)
        {
            for (byte i = 0; i < MAX; i++)
            {
                if (board[row, i] == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if value is found on the specified column on the board
        /// </summary>
        private bool ValueInCol(byte[,] board, byte col, byte value)
        {
            for (byte i = 0; i < MAX; i++)
            {
                if (board[i, col] == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if value is found in the box specified by (row, col)
        /// </summary>
        private bool ValueInBox(byte[,] board, byte row, byte col, byte value)
        {
            byte upperLeftCornerRow = 0, upperLeftCornerCol = 0;

            FindUpperLeftCornerForBox(row, col, ref upperLeftCornerRow, ref upperLeftCornerCol);

            for (byte r = upperLeftCornerRow; r < (upperLeftCornerRow + 3); r++)
            {
                for (byte c = upperLeftCornerCol; c < (upperLeftCornerCol + 3); c++)
                {
                    if (board[r, c] == value)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Find the left upper corner (upperLeftCornerRow, upperLeftCornerCol) of the box that contains (row, col)
        /// </summary>
        private void FindUpperLeftCornerForBox(byte row, byte col, ref byte upperLeftCornerRow, ref byte upperLeftCornerCol)
        {
            if (row < 3 && col < 3)
            { // Box 1
                upperLeftCornerRow = 0;
                upperLeftCornerCol = 0;
            }
            else if (row < 3 && col < 6)
            { // Box 2
                upperLeftCornerRow = 0;
                upperLeftCornerCol = 3;
            }
            else if (row < 3)
            { // Box 3
                upperLeftCornerRow = 0;
                upperLeftCornerCol = 6;
            }
            else if (row < 6 && col < 3)
            { // Box 4
                upperLeftCornerRow = 3;
                upperLeftCornerCol = 0;
            }
            else if (row < 6 && col < 6)
            { // Box 5
                upperLeftCornerRow = 3;
                upperLeftCornerCol = 3;
            }
            else if (row < 6)
            { // Box 6
                upperLeftCornerRow = 3;
                upperLeftCornerCol = 6;
            }
            else if (col < 3)
            { // Box 7
                upperLeftCornerRow = 6;
                upperLeftCornerCol = 0;
            }
            else if (col < 6)
            { // Box 8
                upperLeftCornerRow = 6;
                upperLeftCornerCol = 3;
            }
            else
            { // Box 9
                upperLeftCornerRow = 6;
                upperLeftCornerCol = 6;
            }
        }

        /// <summary>
        /// Prints the 3D array with possible values that each cell in the board might contain
        /// </summary>
        public void WritePossibleValues(byte[][][] possibleValues)
        {
            for (byte row = 0; row < MAX; row++)
            {
                for (byte col = 0; col < MAX; col++)
                {
                    Console.Write("(" + (row + 1) + ", " + (col + 1) + "): ");
                    if (possibleValues[row][col] == null)
                    {
                        Console.Write("Predefined or only one possible value.");
                    }
                    else
                    {
                        for (byte i = 0; i < possibleValues[row][col].Count(); i++)
                        {
                            Console.Write(possibleValues[row][col][i] + " ");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints the board
        /// </summary>
        public void WriteBoard(byte[,] board)
        {
            for (byte row = 0; row < MAX; row++)
            {
                for (byte col = 0; col < MAX; col++)
                {
                    Console.Write(board[row, col] + " ");
                    if ((col + 1) % 3 == 0)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
                if ((row + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        #endregion
    }
}
