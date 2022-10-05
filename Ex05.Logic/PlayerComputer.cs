using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.Logic
{
     public class PlayerComputer
    {
        private int m_Score = 0;
        private int[] m_ArrayOfComputerMoves = new int[0];
        private readonly int r_ColumnsOfResultMatrix = 0;

        public int Score
        {
            get
            {
                return m_Score;
            }
            internal set
            {
                m_Score = value;
            }
        }

        // Creating an Array equivalent to size of matrix (cols*rows), and feeling it with number of indexes. 
        // Indexes of this array - this is potential moves of the PC  
        public PlayerComputer(int i_Rows, int i_Cols)
        {
            m_ArrayOfComputerMoves = new int[i_Rows * i_Cols];

            for(int i = 0; i < m_ArrayOfComputerMoves.Length; i++)
            {
                m_ArrayOfComputerMoves[i] = i;
            }

            r_ColumnsOfResultMatrix =i_Cols;
        }

        // After succeed move of player or PC, Need to update an array of potential moves of the PC
        public void UpdateArrayOfComputerMoves( int[] i_PrevSucceedMoves)
        {
            int indexForDeleteFirst = r_ColumnsOfResultMatrix * i_PrevSucceedMoves[0] + i_PrevSucceedMoves[1];
            int indexForDeleteSecond = r_ColumnsOfResultMatrix * i_PrevSucceedMoves[2] + i_PrevSucceedMoves[3];

            DeleteFromArrayOfComputerMove(indexForDeleteFirst);
            DeleteFromArrayOfComputerMove(indexForDeleteSecond);
        }

        // Deleting used index 
        public void DeleteFromArrayOfComputerMove(int i_IndexForDelete)
        {
            int numIdx = Array.IndexOf(m_ArrayOfComputerMoves, i_IndexForDelete);
            List<int> tmp = new List<int>(m_ArrayOfComputerMoves);
            tmp.RemoveAt(numIdx);
            m_ArrayOfComputerMoves = tmp.ToArray();
        }

        // Logic of PC moves: 
        // Random choose index from Array of moves -> random choose second index (if index is same, repeat step 2) ->
        // -> transformation indexes ti coordinates of matrix
        public int[] ComputerMakeMove ()
        {
            Random rand = new Random();
            int[] computerMovesCoordinates = new int[4];
            int firstChooseInd = rand.Next(0, m_ArrayOfComputerMoves.Length - 1);
            int secondChooseInd = 0;

            if(m_ArrayOfComputerMoves.Length == 2)
            {
                firstChooseInd = 0;
                secondChooseInd = 1;
            }
            while (m_ArrayOfComputerMoves.Length > 2)
            {
                secondChooseInd = rand.Next(0, m_ArrayOfComputerMoves.Length - 1);
                if (secondChooseInd != firstChooseInd) { break; }
            }

            // Transformation to coordinate
            computerMovesCoordinates[0] = m_ArrayOfComputerMoves[firstChooseInd] / r_ColumnsOfResultMatrix;
            computerMovesCoordinates[1] = m_ArrayOfComputerMoves[firstChooseInd] % r_ColumnsOfResultMatrix;
            computerMovesCoordinates[2] = m_ArrayOfComputerMoves[secondChooseInd] / r_ColumnsOfResultMatrix;
            computerMovesCoordinates[3] = m_ArrayOfComputerMoves[secondChooseInd]% r_ColumnsOfResultMatrix;

            return computerMovesCoordinates;
        }
    }
}
