using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * NAME:
 *    Sudoku Solver - a program that solves and prints the solution of Sudoku boards.
 * 
 * VERSION/DATE:
 *    2016-03-28
 *   
 * DESCRIPTION:
 *    A 3D array is first constructed where 2 dimensions represent the board and the 3:rd represents possible values for each cell.
 *    A value for a cell is set on the board if there is only one possible value for that cell. Possible values for all affected cells are then recalculated.
 *    The 3D array is then used to test different values on the board until a solution is found.
 * 
 * EXIT STATUS:
 *    1    Indicates that there are no possible values for cell and that the initial board has value(s) that are incorrect.
 * 
 * AUTHOR:
 *    Written by Sam Lööf.
 *    
 */

namespace SudokuSolver
{
    class Program
    {
        public class SudokuSolver
        {
            const byte MAX = 9;
            byte currentRow = 0;
            byte currentCol = 0;

            public SudokuSolver(byte[,] board) {
                byte[][][] possibleValues = getPossibleValues(board);

                // Check if the solution is already found
                if (noCellsWithValueZero(board)) {
                    writeBoard(board);
                    return;
                }

                byte currentCell = 0;
                bool forward = true;
                while (true) {
                    if (possibleValues[currentCell / MAX][currentCell % MAX] == null) {
                        move(forward, ref currentCell);
                        continue;
                    }

                    if (board[currentCell / MAX, currentCell % MAX] == 0) {
                        setValueOnBoardAndDirection(board, possibleValues, currentCell, ref forward, 0);
                    }
                    else {
                        byte currentIndexPossibleValue;
                        // TODO: Create a index matrix that keeps track of the current index for each cell
                        for (currentIndexPossibleValue = 0; currentIndexPossibleValue < possibleValues[currentCell / MAX][currentCell % MAX].Count(); currentIndexPossibleValue++) {
                            if (board[currentCell / MAX, currentCell % MAX] == possibleValues[currentCell / MAX][currentCell % MAX][currentIndexPossibleValue]) {
                                break;
                            }
                        }

                        if (currentIndexPossibleValue + 1 == possibleValues[currentCell / MAX][currentCell % MAX].Count()) {
                            forward = false;
                            board[currentCell / MAX, currentCell % MAX] = 0;
                        }
                        else {
                            setValueOnBoardAndDirection(board, possibleValues, currentCell, ref forward, (byte)(currentIndexPossibleValue + 1));
                        }
                    }

                    // Check if the solution is found
                    if (currentCell >= (MAX * MAX - 1) && forward) {
                        break;
                    }
                    move(forward, ref currentCell);
                }

                writeBoard(board);
            }

            #region SolveBoard

            /// <summary>
            /// Returns true if there are no cells on the board with value 0, otherwise false is returned.
            /// </summary>
            private bool noCellsWithValueZero(byte[,] board) {
                for (byte row = 0; row < MAX; row++) {
                    for (byte col = 0; col < MAX; col++) {
                        if (board[row, col] == 0) {
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
            private void setValueOnBoardAndDirection(byte[,] board, byte[][][] possibleValues, byte currentCell, ref bool forward, byte currentIndexPossibleValue) {
                for (byte i = currentIndexPossibleValue; i < possibleValues[currentCell / MAX][currentCell % MAX].Count(); i++) {
                    board[currentCell / MAX, currentCell % MAX] = possibleValues[currentCell / MAX][currentCell % MAX][i];

                    // Direction forward if there was no conflict
                    if (!conflictforValue(board, (byte)(currentCell / MAX), (byte)(currentCell % MAX), possibleValues)) {
                        forward = true;
                        return;
                    }
                    // Set board cell value to 0 when going backwards
                    else if (i + 1 == possibleValues[currentCell / MAX][currentCell % MAX].Count()) {
                        forward = false;
                        board[currentCell / MAX, currentCell % MAX] = 0;
                    }
                }
            }

            /// <summary>
            /// Updates currentCell with +1 for forward=true and -1 for forward=false.
            /// </summary>
            private void move(bool forward, ref byte currentCell) {
                if (forward) {
                    currentCell++;
                }
                else {
                    currentCell--;
                }
            }

            /// <summary>
            /// Returns true if the value in cell (rowValueSet, colValueSet) has generated a conflict with another cell on the same row, cell or box.
            /// </summary>
            private bool conflictforValue(byte[,] board, byte rowValueSet, byte colValueSet, byte[][][] possibleValues) {
                byte valueSet = board[rowValueSet, colValueSet];

                for (byte col = 0; col < MAX; col++) {
                    if (possibleValues[rowValueSet][col] == null || col == colValueSet) {
                        continue;
                    }
                    if (board[rowValueSet, col] == valueSet) {
                        return true;
                    }
                }
                for (byte row = 0; row < MAX; row++) {
                    if (possibleValues[row][colValueSet] == null || row == rowValueSet) {
                        continue;
                    }
                    if (board[row, colValueSet] == valueSet) {
                        return true;
                    }
                }

                byte upperLeftCornerRow = 0, upperLeftCornerCol = 0;
                findUpperLeftCornerForBox(rowValueSet, colValueSet, ref upperLeftCornerRow, ref upperLeftCornerCol);

                for (byte row = upperLeftCornerRow; row < upperLeftCornerRow + 3; row++) {
                    for (byte col = upperLeftCornerCol; col < upperLeftCornerCol + 3; col++) {
                        if (possibleValues[row][col] == null || row == rowValueSet || col == colValueSet) {
                            continue;
                        }
                        if (board[row, col] == valueSet) {
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
            private byte[][][] getPossibleValues(byte[,] board) {
                byte[][][] possibleValues = new byte[MAX][][];

                // Important to create the 2D array before setting possible values for each cell (to avoid NullReferenceException)
                for (currentRow = 0; currentRow < MAX; currentRow++) {
                    possibleValues[currentRow] = new byte[MAX][];
                }

                for (currentRow = 0; currentRow < MAX; currentRow++) {
                    for (currentCol = 0; currentCol < MAX; currentCol++) {
                        if (board[currentRow, currentCol] == 0) {
                            setPossibleValuesForCell(board, currentRow, currentCol, possibleValues);
                        }
                        else {
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
            private void setPossibleValuesForCell(byte[,] board, byte row, byte col, byte[][][] possibleValues) {
                possibleValues[row][col] = getPossibleValuesForACell(board, row, col);
                if (possibleValues[row][col].Count() == 0) {
                    Console.WriteLine("No possible values for cell (row, col) = (" + (row + 1) + ", " + (col + 1) + "). The initial board has value(s) that are incorrect.");
                    Environment.Exit(1);
                }
                else if (possibleValues[row][col].Count() == 1) {
                    setValueOnBoardAndUpdatePossibleValues(board, row, col, possibleValues);
                }
            }

            /// <summary>
            /// Sets the value for the cell (rowOnePossibleValue, colOnePossibleValue) in the board that only has one possible value.
            /// Possible values are recalculated for the row, column and box that might be effected by the value set in the board.
            /// </summary>
            private void setValueOnBoardAndUpdatePossibleValues(byte[,] board, byte rowOnePossibleValue, byte colOnePossibleValue, byte[][][] possibleValues) {
                board[rowOnePossibleValue, colOnePossibleValue] = possibleValues[rowOnePossibleValue][colOnePossibleValue][0];
                possibleValues[rowOnePossibleValue][colOnePossibleValue] = null;

                // Recalculate possible values for the row, cell and box
                for (byte col = 0; (rowOnePossibleValue * MAX + col < currentRow * MAX + currentCol) && col < MAX; col++) {
                    if (possibleValues[rowOnePossibleValue][col] == null) {
                        continue;
                    }
                    setPossibleValuesForCell(board, rowOnePossibleValue, col, possibleValues);
                }
                for (byte row = 0; (row * MAX + colOnePossibleValue < currentRow * MAX + currentCol) && row < MAX; row++) {
                    if (possibleValues[row][colOnePossibleValue] == null) {
                        continue;
                    }
                    setPossibleValuesForCell(board, row, colOnePossibleValue, possibleValues);
                }

                byte upperLeftCornerRow = 0, upperLeftCornerCol = 0;
                findUpperLeftCornerForBox(rowOnePossibleValue, colOnePossibleValue, ref upperLeftCornerRow, ref upperLeftCornerCol);

                bool breakLoop = false;
                for (byte row = upperLeftCornerRow; row < (upperLeftCornerRow + 3) && !breakLoop; row++) {
                    for (byte col = upperLeftCornerCol; col < upperLeftCornerCol + 3; col++) {
                        if (row == currentRow && col == currentCol) {
                            breakLoop = true;
                            break;
                        }

                        if (possibleValues[row][col] == null || row == rowOnePossibleValue || col == colOnePossibleValue) {
                            continue;
                        }

                        setPossibleValuesForCell(board, row, col, possibleValues);
		            }
	            }
            }

            /// <summary>
            /// Returns an array of possible values for cell (row, col) based on the board.
            /// </summary>
            private byte[] getPossibleValuesForACell(byte[,] board, byte row, byte col) {
                List<byte> possibleValues = new List<byte>();
                for (byte value = 1; value <= MAX; value++) {
                    if (valueOnRow(board, row, value) || valueInCol(board, col, value) || valueInBox(board, row, col, value)) {
                        continue;
                    }
                    possibleValues.Add(value);
                }

                return possibleValues.ToArray();
            }

            /// <summary>
            /// Returns true if value is found on the specified row on the board.
            /// </summary>>
            private bool valueOnRow(byte[,] board, byte row, byte value) {
                for (byte i = 0; i < MAX; i++) {
		            if (board[row, i] == value) {
			            return true;
                    }
	            }
	            return false;
            }

            /// <summary>
            /// Returns true if value is found on the specified column on the board.
            /// </summary>>
            private bool valueInCol(byte[,] board, byte col, byte value) {
                for (byte i = 0; i < MAX; i++) {
		            if (board[i, col] == value) {
			            return true;
                    }
	            }
	            return false;
            }

            /// <summary>
            /// Returns true if value is found in the box specified by (row, col).
            /// </summary>>
            private bool valueInBox(byte[,] board, byte row, byte col, byte value) {
                byte upperLeftCornerRow = 0, upperLeftCornerCol = 0;

                findUpperLeftCornerForBox(row, col, ref upperLeftCornerRow, ref upperLeftCornerCol);

	            for (byte r = upperLeftCornerRow; r < (upperLeftCornerRow+3); r++) {
		            for (byte c = upperLeftCornerCol; c < (upperLeftCornerCol+3); c++) {
			            if (board[r, c] == value)
				            return true;
		            }
	            }
	            return false;
            }

            /// <summary>
            /// Find the left upper corner (upperLeftCornerRow, upperLeftCornerCol) of the box that contains (row, col).
            /// </summary>
            private void findUpperLeftCornerForBox(byte row, byte col, ref byte upperLeftCornerRow, ref byte upperLeftCornerCol) {
                if (row < 3 && col < 3) { // Box 1
                    upperLeftCornerRow = 0;
                    upperLeftCornerCol = 0;
                }
                else if (row < 3 && col < 6) { // Box 2
                    upperLeftCornerRow = 0;
                    upperLeftCornerCol = 3;
                }
                else if (row < 3) { // Box 3
                    upperLeftCornerRow = 0;
                    upperLeftCornerCol = 6;
                }
                else if (row < 6 && col < 3) { // Box 4
                    upperLeftCornerRow = 3;
                    upperLeftCornerCol = 0;
                }
                else if (row < 6 && col < 6) { // Box 5
                    upperLeftCornerRow = 3;
                    upperLeftCornerCol = 3;
                }
                else if (row < 6) { // Box 6
                    upperLeftCornerRow = 3;
                    upperLeftCornerCol = 6;
                }
                else if (col < 3) { // Box 7
                    upperLeftCornerRow = 6;
                    upperLeftCornerCol = 0;
                }
                else if (col < 6) { // Box 8
                    upperLeftCornerRow = 6;
                    upperLeftCornerCol = 3;
                }
                else { // Box 9
                    upperLeftCornerRow = 6;
                    upperLeftCornerCol = 6;
                }
            }

            /// <summary>
            /// Prints the 3D array with possible values that each cell in the board might contain.
            /// </summary>
            public void writePossibleValues(byte[][][] possibleValues)
            {
                for (byte row = 0; row < MAX; row++) {
                    for (byte col = 0; col < MAX; col++) {
                        Console.Write("(" + (row+1) + ", " + (col+1) + "): ");
                        if (possibleValues[row][col] == null) {
                            Console.Write("Predefined or only one possible value.");
                        }
                        else {
                            for (byte i = 0; i < possibleValues[row][col].Count(); i++) {
                                Console.Write(possibleValues[row][col][i] + " ");
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }

            /// <summary>
            /// Prints the board.
            /// </summary>
            public void writeBoard(byte[,] board) {
                for (byte row = 0; row < MAX; row++) {
                    for (byte col = 0; col < MAX; col++) {
                        Console.Write(board[row, col] + " ");
                        if ((col + 1) % 3 == 0) {
                            Console.Write(" ");
                        }
                    }
                    Console.WriteLine();
                    if ((row + 1) % 3 == 0) {
                        Console.WriteLine();
                    }
                }
            }
            
            #endregion
        }


        static void Main(string[] args)
        {
            // 0 represents a cell with no value set
            byte[,] board;

            Console.WriteLine("Solutions:\n");

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

            new SudokuSolver(board);
            Console.WriteLine();

            // Easy
            board = new byte[,] {
		        {3, 0, 0,  0, 8, 0,  2, 6, 5},
		        {0, 0, 0,  4, 0, 3,  0, 0, 8},
        		{1, 8, 0,  0, 0, 0,  0, 0, 3},
                                     
        		{5, 0, 4,  0, 2, 0,  9, 7, 0},
        		{0, 0, 0,  7, 3, 1,  0, 2, 0},
        		{7, 1, 0,  0, 0, 0,  0, 0, 0},
                                     
        		{0, 2, 3,  9, 0, 6,  4, 0, 0},
		        {0, 0, 0,  0, 4, 0,  0, 8, 0},
		        {0, 0, 6,  0, 5, 2,  3, 0, 0}};

            new SudokuSolver(board);
            Console.WriteLine();

            // Medium
            board = new byte[,] {
		        {6, 0, 0,  8, 0, 0,  0, 3, 1},
		        {0, 1, 0,  3, 0, 0,  0, 0, 9},
        		{0, 8, 7,  0, 1, 0,  0, 0, 0},
                                     
        		{2, 0, 0,  0, 0, 0,  0, 0, 5},
        		{0, 0, 0,  0, 5, 0,  4, 1, 3},
        		{1, 0, 4,  6, 7, 0,  0, 0, 0},
                                     
        		{0, 0, 6,  2, 0, 4,  0, 0, 0},
		        {0, 7, 0,  0, 0, 0,  9, 8, 0},
		        {0, 2, 5,  0, 8, 1,  3, 0, 0}};

            new SudokuSolver(board);
            Console.WriteLine();

            // Hard
            board = new byte[,] {
		        {8, 5, 0,  0, 0, 0,  0, 0, 0},
		        {0, 0, 6,  7, 4, 0,  0, 0, 0},
        		{0, 0, 0,  3, 0, 0,  0, 0, 0},
                                     
        		{0, 0, 2,  0, 0, 0,  6, 0, 4},
        		{0, 0, 0,  0, 0, 5,  0, 0, 2},
        		{0, 8, 4,  0, 0, 0,  0, 0, 5},
                                     
        		{0, 0, 0,  0, 0, 6,  1, 8, 0},
		        {0, 0, 0,  2, 0, 4,  0, 6, 0},
		        {6, 0, 7,  0, 0, 0,  0, 0, 0}};

            new SudokuSolver(board);
            Console.WriteLine();

            // Medium - Bonus
            board = new byte[,] {
		        {7, 0, 0,  0, 0, 0,  0, 5, 0},
		        {5, 0, 0,  0, 0, 4,  1, 0, 6},
        		{2, 6, 0,  0, 8, 0,  0, 9, 0},
                                     
        		{0, 4, 8,  0, 3, 0,  0, 0, 1},
        		{0, 2, 0,  0, 1, 6,  0, 0, 0},
        		{0, 0, 0,  0, 2, 0,  3, 0, 5},
                                     
        		{6, 5, 0,  2, 0, 0,  7, 0, 9},
		        {0, 0, 7,  8, 6, 0,  0, 0, 0},
		        {0, 0, 0,  1, 0, 0,  0, 0, 4}};

            new SudokuSolver(board);
            Console.WriteLine();
        }
    }
}
