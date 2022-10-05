using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.Logic
{
    public class GameLogic
    {
        private readonly Player[] r_Players = new Player[2];
        private string m_WhoWin = null;
        private readonly PlayerComputer r_PlayerComputer = null;
        private readonly Matrix r_WhiteMatrix = new Matrix(0, 0);
        private eWhoPlay m_WhoPlayNow = eWhoPlay.PlayerOne;
        private readonly bool r_IsPlayerVsPlayer = false;
        private readonly int[] r_Choose = new int[4];
        private int m_ChooseCounter = 0;
        private int m_CurrentLetterRowCell = 0;
        private int m_CurrentLetterColCell = 0;
        private char m_LetterFromResultMatrix = '\0';
 
        public int[] Choose
        {
            get
            {
                return r_Choose;
            }
            set
            {
                r_Choose[m_ChooseCounter++] = m_CurrentLetterRowCell = value[0];
                r_Choose[m_ChooseCounter++] = m_CurrentLetterColCell = value[1];
            }
        }

        /// player Vs player 
        public Player PlayerWhoPlayNow
        {
            get
            {
                return r_Players[(int)m_WhoPlayNow];
            }
        }

        public Player[] Players
        {
            get
            {
                return r_Players;
            }
        }

        public string WhoWin
        {
            get
            {
                return m_WhoWin;
            }
        }

        public Matrix GetWhiteMatrix
        {
            get
            {
                return r_WhiteMatrix;
            }
        }

        public PlayerComputer PlayerComputer
        {
            get
            {
                return r_PlayerComputer;
            }
        }

        public char LetterFromResultMatrix
        {
            get
            {
                return m_LetterFromResultMatrix;
            }
        }

        //Two players game constructor
        public GameLogic(string i_PlayerOneName, string i_PlayerTwoName, int i_Rows, int i_Cols)
        {
            r_Players[0] = new Player(i_PlayerOneName);
            r_Players[1] = new Player(i_PlayerTwoName);
            r_WhiteMatrix = new Matrix(i_Rows, i_Cols);
            r_IsPlayerVsPlayer = true;
        }

        //Player vs. Computer constructor
        public GameLogic(string i_PlayerOneName, int i_Rows, int i_Cols) 
        {
            r_Players[0] = new Player(i_PlayerOneName);
            r_PlayerComputer = new PlayerComputer(i_Rows, i_Cols);
            r_WhiteMatrix = new Matrix(i_Rows, i_Cols);
        }
 
        public eWhoPlay WhoPlayNow
        {
            get
            {
                return m_WhoPlayNow;
            }
        }

        private void updatePlayerScore()
        {
            r_Players[(int)m_WhoPlayNow].Score++;
        }
 
        private void updatePlayerComputerScore()
        {
            r_PlayerComputer.Score++;
        }
 
        public bool IsWhiteMatrixFull()
        {
            bool isWhitMatrixFull = false;

            if (r_IsPlayerVsPlayer)
            {
                isWhitMatrixFull = (Players[0].Score + Players[1].Score) == ((r_WhiteMatrix.WhiteMatrixChars.Length) / 2);
            }

            else
            {
                isWhitMatrixFull = (r_PlayerComputer.Score + Players[0].Score) == ((r_WhiteMatrix.WhiteMatrixChars.Length) / 2);
            }
 
 
            return isWhitMatrixFull;
        }
 
        private void whoWinMethod()
        {
            if (r_IsPlayerVsPlayer)
            {
                //player vs player
                whoWinPlayerVsPlayer();
            }
            else
            {
                //player vs Computer
                whoWinPlayerVsComputer();
            }
 
        }
 
        private void whoWinPlayerVsPlayer()
        {
            if (r_Players[0].Score == r_Players[1].Score)
            {
                m_WhoWin = null;
            }

            else
            {
                m_WhoWin = (r_Players[0].Score > r_Players[1].Score) ? r_Players[0].Name : r_Players[1].Name;
            }
        }
 
        private void whoWinPlayerVsComputer()
        {
            if (r_PlayerComputer.Score == r_Players[0].Score)
            {
                m_WhoWin = null;
            }

            else
            {
                m_WhoWin = (r_PlayerComputer.Score > r_Players[0].Score) ? "PC" : r_Players[0].Name;
            }
        }
 
        public void Round()
        {
            bool isItRoundTow = m_ChooseCounter == 4;

            m_LetterFromResultMatrix = GetWhiteMatrix.CopyLetterFromResultMatrix(m_CurrentLetterRowCell, m_CurrentLetterColCell);

            GetWhiteMatrix.PasteLetterToWhiteMatrix(m_LetterFromResultMatrix, m_CurrentLetterRowCell, m_CurrentLetterColCell);
            
            if (isItRoundTow)
            {
                bool isTowLettersEqual =  m_LetterFromResultMatrix == r_WhiteMatrix.WhiteMatrixChars[r_Choose[0], r_Choose[1]];
                  
                if (isTowLettersEqual)
                {
                    if(!r_IsPlayerVsPlayer)
                    {
                        r_PlayerComputer.UpdateArrayOfComputerMoves(r_Choose);
                    }

                    bool isPlayer = m_WhoPlayNow == eWhoPlay.PlayerOne || WhoPlayNow == eWhoPlay.PlayerTwo;
                    
                    if (isPlayer)
                    {
                        updatePlayerScore();
                    }

                    else
                    {
                        updatePlayerComputerScore();
                    }
                }

                else
                {
                    r_WhiteMatrix.DeleteCellsWhiteMatrix(r_Choose);
                    swapWhoPlay();
                }
                
                m_ChooseCounter = 0;
                
                if (IsWhiteMatrixFull())
                {
                    whoWinMethod();
                }
            }
        }
 
        private void swapWhoPlay()
        {
            if (r_IsPlayerVsPlayer)
            {
                swapPlayersVsPlayer();
            }

            else
            {
                swapPlayerVsAi();
            }
        }
 
        private void swapPlayersVsPlayer()
        {
            bool isPlayerOnePlayNow = m_WhoPlayNow == eWhoPlay.PlayerOne;

            m_WhoPlayNow = isPlayerOnePlayNow ? eWhoPlay.PlayerTwo : eWhoPlay.PlayerOne;
        }
 
        private void swapPlayerVsAi()
        {
            m_WhoPlayNow = m_WhoPlayNow == eWhoPlay.PlayerOne ? eWhoPlay.Computer : eWhoPlay.PlayerOne;
        }
    }
    
}
