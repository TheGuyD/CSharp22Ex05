using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.Logic
{
     public class Player
    {
        private string m_Name = string.Empty;
        private int m_Score = 0;
       
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

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

        public Player(string i_Name)
        {
            m_Name = i_Name;
        }

    }
}
