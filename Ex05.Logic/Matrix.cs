using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.Logic
{
    public class Matrix
    {
        private readonly int r_Rows = 0;
        private readonly int r_Columns = 0;
        private readonly char[,] r_WhiteMatrixChars = new char[0, 0];
        private readonly char[,] r_ResultMatrixChars = new char[0, 0];
       
        public Matrix(int i_Rows, int i_Columns)
        {
            r_Rows = i_Rows;
            r_Columns = i_Columns;

            r_WhiteMatrixChars = new char[i_Rows, i_Columns];
            r_ResultMatrixChars = new char[i_Rows, i_Columns];
            buildResultMatrix();
        }

        public int Rows { 
            get 
                {
                return r_Rows;
                }
        }
        public int Cols { 
            get 
                {
                return r_Columns;
                }
        }
        public char[,] WhiteMatrixChars
        {
            get
            {
                return r_WhiteMatrixChars;
            }
        }

        //Method to shuffle elements of result matrix. Receive an array with elements of result matrix, shuffling them and
        //return by reference shuffled array 
        private void shuffle<T>(ref T[] i_Arr)
        {
            Random rand = new Random();

            for (int i = i_Arr.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                (i_Arr[j], i_Arr[i]) = (i_Arr[i], i_Arr[j]);
            }
        }

        //Method for building aResult Matrix
        private void buildResultMatrix()
        {
            char[] tempArrChar = new char[r_Rows * r_Columns];

            //Create 1-D array and filling it with a pair of Big letters(A x 2, B x 2..... etc)   
            for (int i = 0; i < tempArrChar.Length / 2; i++)
            {
                tempArrChar[i] = Convert.ToChar('A' + i);
                tempArrChar[tempArrChar.Length - 1 - i] = tempArrChar[i];
            }

            // Method to shuffle elements in 1-D array 
            shuffle(ref tempArrChar);

            //auxiliary variable for transferring a 1-D array to a 2-D
            int counter = 0;

            //Filling Result Matrix with elements of shuffled 1-D array 
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Columns; j++)
                {
                    r_ResultMatrixChars[i, j] = tempArrChar[counter];
                    counter++;
                }
            }
        }

        // Validation for input: if it isn`t out of range
        public void MatrixBorderValidation(string i_UserInput)
        {
            bool isInMatrixBorer = (i_UserInput[0] - '0' <= r_Rows) && (i_UserInput[1] - 'A' < r_Columns);

            if(isInMatrixBorer)
            {
                return;
            }

            char c = (char)(r_Columns + 'A' - 1);
            throw new Exception($"Users` input should be rows 1-{r_Rows} and columns A-{c} is invalid");
        }

        // Validation for input: if choosed cell hasn't been choose earlier
        public void MatrixCellAvailable(string i_UserInput)
        {
            bool isCellEmpty = WhiteMatrixChars[i_UserInput[0] - '1', i_UserInput[1] - 'A'] == '\0';
            if (!isCellEmpty)
            {
                throw new Exception("Cell has already been chosen");
            }
        }

        // Method for copping Cell from Result Matrix
        public char CopyLetterFromResultMatrix(int i_RowOfCell, int i_ColumnOfCell)
        {

            char valueOfCell = r_ResultMatrixChars[i_RowOfCell, i_ColumnOfCell];
            return valueOfCell;
        }

        //Method for paste copied cell to White Matrix
        public void PasteLetterToWhiteMatrix(char i_CharToPaste, int i_RowOfCell, int i_ColumnOfCell)
        {
            r_WhiteMatrixChars[i_RowOfCell, i_ColumnOfCell] = i_CharToPaste;
        }

        //Method for deleting not paired cells from White Matrix
        public void DeleteCellsWhiteMatrix(params int[] i_Coordinates)
        {
            r_WhiteMatrixChars[i_Coordinates[0], i_Coordinates[1]] = '\0';
            r_WhiteMatrixChars[i_Coordinates[2], i_Coordinates[3]] = '\0';
        }
    }
}
