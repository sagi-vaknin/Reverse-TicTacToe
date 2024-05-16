using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex05.GameLogic;

namespace Ex05.WindowsUI
{
    public class ScoreTable
    {
        private int m_Player1Score = 0;
        private int m_Player2Score = 0;
        public int Player1Score
        {
            get
            {
                return m_Player1Score;
            }
        }
        public int Player2Score
        {
            get
            {
                return m_Player2Score;
            }
        }
        internal void UpdateScoreBoard(GameEngine i_Game)
        {
            if(i_Game.GameState == eGameStates.XWon)
            {
                m_Player1Score++;
            }
            else if(i_Game.GameState == eGameStates.OWon)
            {
                m_Player2Score++;
            }
        }
    }
}
