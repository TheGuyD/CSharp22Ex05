using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Ex05.Logic;
using System.Threading;

namespace Ex05.WindowsFormUI
{
        /*
         * please import refferance Ex05.Logic before starting the project , thank you 
         * */
    public class GameForm : Form
    {
        private readonly List<List<Button>> r_MatrixButton = new List<List<Button>>();
        private readonly Label r_LabelFirstPlayer = null;
        private readonly Label r_LabelSecondPlayer = null;
        private readonly Label r_LabelCurrentPlayer = null;
        private readonly GameLogic r_GameLogic = null;
        private int m_NumberOfChosenButtons = 0;
        private readonly Button[] r_ButtonChoosed = new Button[2];
        private int[] m_ComputerMove = null;

        public GameForm(GameLogic i_GameLogic)
        {
            r_GameLogic = i_GameLogic;

            initiateGameFormBaseAndTitle(i_GameLogic);

            initiateEmptyMatrixOfButtons();

            r_LabelFirstPlayer = playerLabel(i_GameLogic.Players[0].Name, this.Bottom - 140, Color.Green);

            if (r_GameLogic.PlayerComputer == null)
            {
                r_LabelSecondPlayer = playerLabel(i_GameLogic.Players[1].Name, this.Bottom - 105, Color.HotPink);
            }
            else
            {
                r_LabelSecondPlayer = playerLabel("Computer", this.Bottom - 105, Color.HotPink);
            }

            r_LabelCurrentPlayer = initiateCurrentPlayer(i_GameLogic.Players[0].Name, r_LabelFirstPlayer.BackColor);

            this.Controls.Add(r_LabelFirstPlayer);
            this.Controls.Add(r_LabelSecondPlayer);
            this.Controls.Add(r_LabelCurrentPlayer);
        }

        private void initiateGameFormBaseAndTitle(GameLogic i_GameLogic)
        {
            this.ClientSize = new Size(i_GameLogic.GetWhiteMatrix.Cols * 100,
                                                        i_GameLogic.GetWhiteMatrix.Rows * 100);
            this.Text = "Game Board";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private Label initiateCurrentPlayer(string i_FirstPlayerName, Color i_BackColor)
        {
            Label currantPlayer = new Label();
            currantPlayer.Text = string.Format("Current: {0}", i_FirstPlayerName);
            currantPlayer.Location = new Point(10, this.Bottom - 65);
            currantPlayer.ForeColor = Color.White;
            currantPlayer.BackColor = i_BackColor;
            currantPlayer.Size = new Size(currantPlayer.Text.Length + 100, 20);

            return currantPlayer;
        }

        private void initiateEmptyMatrixOfButtons()
        {
            for (int i = 0; i < r_GameLogic.GetWhiteMatrix.Rows; i++)
            {
                r_MatrixButton.Add(new List<Button>());

                for (int j = 0; j < r_GameLogic.GetWhiteMatrix.Cols; j++)
                {
                    Button card = new Button();
                    card.Location = new Point(30 + j * 100, 30 + i * 70);
                    card.Size = new Size(50, 50);

                    if (r_GameLogic.PlayerComputer == null)
                    {
                        card.Click += buttonChoseInPlayerVsPlayerMode_Click;
                    }
                    else
                    {
                        card.Click += buttonOfBoardInPlayerVsComputerMode_Click;
                    }

                    r_MatrixButton[i].Add(card);
                    this.Controls.Add(r_MatrixButton[i][j]);
                }
            }
        }

        //button clicked for player vs Computer
        private void buttonOfBoardInPlayerVsComputerMode_Click(object i_Sender, EventArgs i_)
        {
            bool isTheButtonAlreadyClicked = (i_Sender as Button).Text != "";

            if (isTheButtonAlreadyClicked)
            {
                return;
            }

            r_ButtonChoosed[m_NumberOfChosenButtons] = (i_Sender as Button);
            ++m_NumberOfChosenButtons;

            updateChoosedButtonTextAndColor(i_Sender);

            if (m_NumberOfChosenButtons == 2)
            {
                m_NumberOfChosenButtons = 0;
                bool isCellInWhiteMatrixWasDeleted = r_GameLogic.GetWhiteMatrix.WhiteMatrixChars
                                                         [r_GameLogic.Choose[0], r_GameLogic.Choose[1]] == '\0';

                if (isCellInWhiteMatrixWasDeleted)
                {
                    //Thread Sleep was used for create possibility to see changes in WinForm
                    System.Threading.Thread.Sleep(2000);
                    deleteTextFromButtonMatrix();
                    restoreButtonColor();

                    if (r_GameLogic.WhoPlayNow == eWhoPlay.Computer)
                    {
                        changeCurrentPlayerLabel("Computer");
                        r_LabelCurrentPlayer.Update();
                        computerPlayerMakeMove();
                    }
                    else
                    {
                        changeCurrentPlayerLabel(r_GameLogic.PlayerWhoPlayNow.Name);
                    }
                }
                else
                {
                    if (r_GameLogic.WhoPlayNow == eWhoPlay.PlayerOne)
                    {
                        changeFirstPlayerLabelPairs(r_GameLogic.PlayerWhoPlayNow.Name);

                        if (r_GameLogic.IsWhiteMatrixFull())
                        {
                            disposeGameFormAndCreateFinishForm();
                        }
                    }
                    else
                    {
                        changeSecondPlayerLabelPairs("Computer");
                        computerPlayerMakeMove();
                    }
                }
            }
        }

        private void changeFirstPlayerLabelPairs(string i_FirstPlayerName)
        {
            r_LabelFirstPlayer.Text = string.Format("{0} : {1} Pairs", i_FirstPlayerName, r_GameLogic.PlayerWhoPlayNow.Score);
        }

        private void changeSecondPlayerLabelPairs(string i_SecondPlayerName)
        {
            if (i_SecondPlayerName == "Computer")
            {
                r_LabelSecondPlayer.Text = string.Format("{0} : {1} Pairs", i_SecondPlayerName, r_GameLogic.PlayerComputer.Score);
            }
            else
            {
                r_LabelSecondPlayer.Text = string.Format("{0} : {1} Pairs", i_SecondPlayerName, r_GameLogic.PlayerWhoPlayNow.Score);
            }
        }

        private void changeCurrentPlayerLabel(string i_CurrentPlayerName)
        {
            r_LabelCurrentPlayer.Text = string.Format("Current : {0}", i_CurrentPlayerName);
            r_LabelCurrentPlayer.BackColor = r_GameLogic.WhoPlayNow == eWhoPlay.PlayerOne ?
                                                  r_LabelFirstPlayer.BackColor : r_LabelSecondPlayer.BackColor;
        }

        private void updateChoosedButtonTextAndColor(object i_Sender)
        {
            //normalize the column and row to fit the List<List<Button>> indexes. 
            int col = ((i_Sender as Button).Location.X - 30) / 100;
            int row = ((i_Sender as Button).Location.Y - 30) / 70;

            //update the game logic with the new choosed cell - it's important to the game manage
            r_GameLogic.Choose = new int[] { row, col };

            //change the choosed button Color in UI
            Color backColor = r_GameLogic.WhoPlayNow == eWhoPlay.PlayerOne ? r_LabelFirstPlayer.BackColor : r_LabelSecondPlayer.BackColor;
            (i_Sender as Button).BackColor = backColor;

            //Make a Logical Round
            r_GameLogic.Round();

            //The logical Round update a Letter From Result Matrix - the Letter is the new Button choosed text
            (i_Sender as Button).Text = r_GameLogic.LetterFromResultMatrix.ToString();

            //update the choosed button with the player button color and the Letter From Result Matrix
            (i_Sender as Button).Update();
        }

        private void restoreButtonColor()
        {
            r_ButtonChoosed[0].BackColor = Color.Empty;
            r_ButtonChoosed[0].Update();
            r_ButtonChoosed[1].BackColor = Color.Empty;
            r_ButtonChoosed[1].Update();
        }

        private void computerPlayerMakeMove()
        {
            //if the inner logical matrix is full the game is over.
            if (r_GameLogic.IsWhiteMatrixFull())
            {
                return;
            }

            //else computer will make move
            m_ComputerMove = r_GameLogic.PlayerComputer.ComputerMakeMove();
            changeEventHandlersToComputerClickEventHandlers();
            Thread.Sleep(1500);
            r_MatrixButton[m_ComputerMove[0]][m_ComputerMove[1]].PerformClick();
            r_MatrixButton[m_ComputerMove[2]][m_ComputerMove[3]].PerformClick();
            
        }

        private void changeEventHandlersToComputerClickEventHandlers()
        {
            r_MatrixButton[m_ComputerMove[0]][m_ComputerMove[1]].Click -= buttonOfBoardInPlayerVsComputerMode_Click;
            r_MatrixButton[m_ComputerMove[0]][m_ComputerMove[1]].Click += computerChooseButton_Click;
            r_MatrixButton[m_ComputerMove[2]][m_ComputerMove[3]].Click -= buttonOfBoardInPlayerVsComputerMode_Click;
            r_MatrixButton[m_ComputerMove[2]][m_ComputerMove[3]].Click += computerChooseButton_Click;
        }

        //event handler for Computer clicks
        private void computerChooseButton_Click(object sender, EventArgs e)
        {
            r_ButtonChoosed[m_NumberOfChosenButtons] = (sender as Button);
            ++m_NumberOfChosenButtons;

            updateChoosedButtonTextAndColor(sender);

            bool isSecondButtonPressed = m_NumberOfChosenButtons == 2;

            if (isSecondButtonPressed)
            {
                m_NumberOfChosenButtons = 0;
                bool isCellInWhiteMatrixWasDeleted = r_GameLogic.GetWhiteMatrix.WhiteMatrixChars[r_GameLogic.Choose[0],
                                                         r_GameLogic.Choose[1]] == '\0';

                //if the Computer buttons choosed was unmatched letters 
                if (isCellInWhiteMatrixWasDeleted)
                {
                    Thread.Sleep(1500);
                    deleteTextFromButtonMatrix();
                    restoreButtonColor();
                    changeCurrentPlayerLabel(r_GameLogic.PlayerWhoPlayNow.Name);
                }
                else
                {
                    //if the logical matrix is full the gme is over.
                    if (r_GameLogic.IsWhiteMatrixFull())
                    {
                        disposeGameFormAndCreateFinishForm();
                    }
                    else
                    {
                        changeSecondPlayerLabelPairs("Computer");
                        r_LabelSecondPlayer.Update();
                        computerPlayerMakeMove();
                    }
                }
            }

            (sender as Button).Click -= computerChooseButton_Click;
            (sender as Button).Click += buttonOfBoardInPlayerVsComputerMode_Click;
        }

        private void buttonChoseInPlayerVsPlayerMode_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text != "")
            {
                return;
            }

            r_ButtonChoosed[m_NumberOfChosenButtons] = (sender as Button);
            ++m_NumberOfChosenButtons;
            
            updateChoosedButtonTextAndColor(sender);

            if (m_NumberOfChosenButtons == 2)
            {
                m_NumberOfChosenButtons = 0;
                bool isCellInWhiteMatrixWasDeleted = r_GameLogic.GetWhiteMatrix.WhiteMatrixChars
                                                         [r_GameLogic.Choose[0], r_GameLogic.Choose[1]] == '\0';

                if (isCellInWhiteMatrixWasDeleted)
                {
                    Thread.Sleep(2000);
                    deleteTextFromButtonMatrix();
                    restoreButtonColor();
                    changeCurrentPlayerLabel(r_GameLogic.PlayerWhoPlayNow.Name);
                }
                else
                {
                    if (r_GameLogic.WhoPlayNow == eWhoPlay.PlayerOne)
                    {
                        changeFirstPlayerLabelPairs(r_GameLogic.PlayerWhoPlayNow.Name);
                    }
                    else
                    {
                        changeSecondPlayerLabelPairs(r_GameLogic.PlayerWhoPlayNow.Name);
                    }
                }
            }

            if (r_GameLogic.IsWhiteMatrixFull())
            {
                
                disposeGameFormAndCreateFinishForm();
            }
        }

        private void disposeGameFormAndCreateFinishForm()
        {
            Thread.Sleep(1500);
            this.Dispose();
            new FinishForm(r_GameLogic.WhoWin, r_GameLogic);
        }

        private void deleteTextFromButtonMatrix()
        {
            r_ButtonChoosed[0].Text = string.Empty;
            r_ButtonChoosed[1].Text = string.Empty;
        }

        private Label playerLabel(string i_PlayerName, int i_YCordinate, Color i_Color)
        {
            Label playerLabel = new Label();
            int playerScore = 0;
            playerLabel.BackColor = i_Color;
            playerLabel.Text = String.Format("{0} : {1} Pairs", i_PlayerName, playerScore);
            playerLabel.ForeColor = Color.White;
            playerLabel.Location = new Point(10, i_YCordinate);
            playerLabel.Size = new Size(i_PlayerName.Length + 100, 20);

            return playerLabel;
        }
    }
}
