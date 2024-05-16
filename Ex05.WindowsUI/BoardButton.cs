using Ex05.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex05.WindowsUI
{
    internal class BoardButton : Button
    {
        private int m_ButtonRow;
        private int m_ButtonCol;

        public BoardButton(int i_Row, int i_Col) 
        :base()
        {
            m_ButtonRow = i_Row;
            m_ButtonCol = i_Col;
        }    
        internal int ButtonRow
        {
            get 
            {
                return m_ButtonRow; 
            }

            set 
            { 
                m_ButtonRow = value; 
            }
        }
        internal int ButtonCol
        {
            get
            {
                return m_ButtonCol;
            }

            set
            {
                m_ButtonCol = value;
            }
        }
    }
}
