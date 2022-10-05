using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ex05.Logic;

namespace Ex05.WindowsFormUI
{
    
        /*
         * please import refferance Ex05.Logic before starting the project , thank you 
         * */
    public class FinishForm : Form
    {
       
        GameLogic m_GameLogic = null;

        public FinishForm(string i_WhoWin, GameLogic i_GameLogic)
        {
            m_GameLogic = i_GameLogic;

            string message = m_GameLogic.WhoWin == null ? "It's a draw" : string.Format("the winner is {0}", m_GameLogic.WhoWin);
            
            DialogResult result = MessageBox.Show(string.Format("{0}, click Retry to restart", message),
                                                  "End Game?", MessageBoxButtons.RetryCancel);

            if (result == DialogResult.Retry)
            {
                buttonRetry_Click();
            }
            else
            {
                this.Dispose();
            }
        }

        private void buttonRetry_Click()
        {
            if (m_GameLogic.PlayerComputer == null)
            {
                new GameForm(new GameLogic(m_GameLogic.Players[0].Name,
                                           m_GameLogic.Players[1].Name,
                                           m_GameLogic.GetWhiteMatrix.Rows,
                                           m_GameLogic.GetWhiteMatrix.Cols)).ShowDialog();
            }
            else
            {
                new GameForm(new GameLogic(m_GameLogic.Players[0].Name, 
                                           m_GameLogic.GetWhiteMatrix.Rows, 
                                           m_GameLogic.GetWhiteMatrix.Cols)).ShowDialog();
            }
        }
    }
}
