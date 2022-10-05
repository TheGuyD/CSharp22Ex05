using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05.Logic;

namespace Ex05.WindowsFormUI
{
    
        /*
         * please import refferance Ex05.Logic before starting the project , thank you 
         * */
    public class WelcomeForm : Form
    {
        private const string k_FormTitle = "Remember Game - Welcome Form";
        private const string k_FirstPlayerName = "Player 1 Name:";
        private const string k_SecondPlayerName = "Player 2 Name:";
        private int m_IndexOfMatrixSize = 0;
        private readonly string[] r_MatrixSizes = new string[] { "4x4", "4x5", "4x6", "5x4", "5x6", "6x4", "6x5", "6x6" };
        private Label m_LabelFirstPlayerName = null;
        private Label m_LabelSecondPlayerName = null;
        private TextBox m_TextBoxFirstPlayerName = null;
        private TextBox m_TextBoxSecondPlayerName = null;
        private Button m_ButtonChoosePlayer = null;
        private Button m_ButtonChooseMatrix = null;
        private Button m_ButtonStart = null;
        private GameLogic m_GameLogic = null;

        public WelcomeForm()
        {
            drawWelcomeFormAndTitleBase();

            createFirstPlayerNameLabel();

            createFirstPlayerNameTextBox();

            createSecondPlayerNameLabel();

            createSecondPlayerNameTextBox();

            createAgainstAPlayerButton();

            createChooseMatrixSizeButton();

            createStartButton();

            this.FormClosed += welcomeFormFormClosing_Click;

            this.ShowDialog();
        }

        private void welcomeFormFormClosing_Click(object sender, FormClosedEventArgs e)
        {
            buttonStart_Click((sender as WelcomeForm), e);
        }

        private void createStartButton()
        {
            m_ButtonStart = new Button();
            m_ButtonStart.Text = "Start!";
            m_ButtonStart.Location = new Point(250, 117);
            m_ButtonStart.BackColor = Color.Pink;
            m_ButtonStart.Click += buttonStart_Click;
            
            this.Controls.Add(m_ButtonStart);
        }

        private void createChooseMatrixSizeButton()
        {
            m_ButtonChooseMatrix = new Button();
            m_ButtonChooseMatrix.Text = r_MatrixSizes[m_IndexOfMatrixSize];
            m_ButtonChooseMatrix.Location = new Point(20, 70);
            m_ButtonChooseMatrix.Size = new Size(70, 70);
            m_ButtonChooseMatrix.BackColor = Color.Pink;
            m_ButtonChooseMatrix.Click += buttonChooseMatrix_Click;

            this.Controls.Add(m_ButtonChooseMatrix);
        }

        private void createAgainstAPlayerButton()
        {
            m_ButtonChoosePlayer = new Button();
            m_ButtonChoosePlayer.Text = "Against a Friend";
            m_ButtonChoosePlayer.Size = new Size(110, 20);
            m_ButtonChoosePlayer.Location = new Point(220, 30);
            m_ButtonChoosePlayer.Click += buttonChoosePlayerDisable_Click;

            this.Controls.Add(m_ButtonChoosePlayer);
        }

        private void createSecondPlayerNameTextBox()
        {
            m_TextBoxSecondPlayerName = new TextBox();
            m_TextBoxSecondPlayerName.Location = new Point(110, 32);

            this.Controls.Add(m_TextBoxSecondPlayerName);
        }

        private void createSecondPlayerNameLabel()
        {
            m_LabelSecondPlayerName = new Label();
            m_LabelSecondPlayerName.Text = k_SecondPlayerName;
            m_LabelSecondPlayerName.Location = new Point(10, 32);

            this.Controls.Add(m_LabelSecondPlayerName);
        }

        private void createFirstPlayerNameTextBox()
        {
            m_TextBoxFirstPlayerName = new TextBox();
            m_TextBoxFirstPlayerName.Location = new Point(110, 10);

            this.Controls.Add(m_TextBoxFirstPlayerName);
        }
        
        private void createFirstPlayerNameLabel()
        {
            m_LabelFirstPlayerName = new Label();
            m_LabelFirstPlayerName.Text = k_FirstPlayerName;
            m_LabelFirstPlayerName.Location = new Point(10, 9);

            this.Controls.Add(m_LabelFirstPlayerName);
        }

        private void drawWelcomeFormAndTitleBase()
        {
            this.ClientSize = new Size(350, 160);
            this.Text = k_FormTitle;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.FormClosed -= welcomeFormFormClosing_Click;

            int rows = int.Parse(m_ButtonChooseMatrix.Text[0].ToString());
            int cols = int.Parse(m_ButtonChooseMatrix.Text[2].ToString());

            if (m_LabelSecondPlayerName.Enabled)
            {
                string playerOneName = m_TextBoxFirstPlayerName.Text == string.Empty  ? "Player 1" : m_TextBoxFirstPlayerName.Text;
                string playerTwoName = m_TextBoxSecondPlayerName.Text == string.Empty ? "Player 2" : m_TextBoxSecondPlayerName.Text;
                m_GameLogic = new GameLogic(playerOneName, playerTwoName, rows, cols);
            }
            else
            {
                string playerOneName = m_TextBoxFirstPlayerName.Text == string.Empty  ? "Player 1" : m_TextBoxFirstPlayerName.Text;
                m_GameLogic = new GameLogic( playerOneName, rows, cols);
            }

            this.Dispose();

            new GameForm(m_GameLogic).ShowDialog();
        }

        private void buttonChooseMatrix_Click(object sender, EventArgs e)
        {
            m_ButtonChooseMatrix.Text = r_MatrixSizes[++m_IndexOfMatrixSize];

            if (m_IndexOfMatrixSize == r_MatrixSizes.Length - 1)
            {
                m_IndexOfMatrixSize = -1;
            }
        }

        private void buttonChoosePlayerDisable_Click(object sender, EventArgs e)
        {
            disablePlayerTwoTextBox();
            disableSecondPlayerNameLabel();
            changeTextAgainstAPlayerToAgainstComputer();

            m_ButtonChoosePlayer.Click -= buttonChoosePlayerDisable_Click;
            m_ButtonChoosePlayer.Click += buttonChoosePlayerEnable_Click;
        }

        private void buttonChoosePlayerEnable_Click(object sender, EventArgs e)
        {
            enablePlayerTwoTextBox();
            exposePlayerTwoLabel();
            changeTextAgainstComputerToAgainstPlayer();

            m_ButtonChoosePlayer.Click -= buttonChoosePlayerEnable_Click;
            m_ButtonChoosePlayer.Click += buttonChoosePlayerDisable_Click;
        }
        
        private void disablePlayerTwoTextBox()
        {
            m_TextBoxSecondPlayerName.Text = "--------Computer--------";
            m_TextBoxSecondPlayerName.Enabled = false;
        }

        private void disableSecondPlayerNameLabel()
        {
            m_LabelSecondPlayerName.Enabled = false;
        }

        private void changeTextAgainstAPlayerToAgainstComputer()
        {
            m_ButtonChoosePlayer.Text = "Against Computer";
        }

        private void enablePlayerTwoTextBox()
        {
            m_TextBoxSecondPlayerName.Text = " ";
            m_TextBoxSecondPlayerName.Enabled = true;
        }

        private void exposePlayerTwoLabel()
        {
            m_LabelSecondPlayerName.Enabled = true;
        }

        private void changeTextAgainstComputerToAgainstPlayer()
        {
            m_ButtonChoosePlayer.Text = "Against a Friend";
        }
    }
}
