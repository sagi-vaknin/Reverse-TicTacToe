using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Ex05.GameLogic;
using System.Threading;

namespace Ex05.WindowsUI
{
    internal class GameForm : Form
    {
        private Label m_Player1;
        private Label m_Player2;
        private String m_Player1Name;
        private String m_Player2Name;
        private readonly int r_GameBoardSize;
        private BoardButton[,] m_Board;
        public GameForm(String i_Player1Name, String i_Player2Name, int i_GameBoardSize)
        {
            FormClosed += gameForm_Closed;
            m_Player1Name = i_Player1Name;
            m_Player2Name = i_Player2Name;
            r_GameBoardSize = i_GameBoardSize;
            m_Board = new BoardButton[r_GameBoardSize, r_GameBoardSize];
            initializeComponent();
        }
        internal BoardButton[,] FormBoard
        {
            get
            {
                return m_Board;
            }
        }
        private void gameForm_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void initializeComponent()
        {
            this.Text = "TicTacToeMisere";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Width = r_GameBoardSize * 60 + 15;
            this.Height = r_GameBoardSize * 60 + 80;
            this.StartPosition = FormStartPosition.CenterScreen;
            initiliazeScoreLabels();
            this.createGameBoard();
        }
        private void initiliazeScoreLabels()
        {
            m_Player1 = new Label();
            m_Player2 = new Label();

            m_Player1.Text = $"{m_Player1Name}: 0";
            m_Player2.Text = $"{m_Player2Name}: 0";
            BoldLabelCurrentPlayer(ePlayerTurn.XTurn);
            m_Player1.AutoSize = true;
            m_Player2.AutoSize = true;
            m_Player1.Location = new Point(this.ClientSize.Width / 4 - (this.m_Player1.Width / 4), this.ClientSize.Height - 25);
            m_Player2.Left = m_Player1.Right + 10;
            m_Player2.Top = m_Player1.Top;

            this.Controls.Add(m_Player1);
            this.Controls.Add(m_Player2);
        }
        private void createGameBoard()
        {
            for(int i = 0; i < r_GameBoardSize; i++)
            {
                for(int j = 0; j < r_GameBoardSize; j++)
                {
                    m_Board[i, j] = new BoardButton(i, j);

                    m_Board[i, j].Width = 50;
                    m_Board[i, j].Height = 50;
                    if(i == 0)
                    {
                        m_Board[i, j].Top = 5;
                    }
                    else
                    {
                        m_Board[i, j].Top = m_Board[i - 1, j].Bottom + 10;
                    }

                    if(j == 0)
                    {
                        m_Board[i, j].Left = 5;
                    }
                    else
                    {
                        m_Board[i, j].Left = m_Board[i, j - 1].Right + 10;
                    }

                    this.Controls.Add(m_Board[i, j]);
                }
            }
        }
        internal void UpdateScoreLabels(int i_Player1Score, int i_Player2Score)
        {
            m_Player1.Text = $"{m_Player1Name}: {i_Player1Score}";
            m_Player2.Text = $"{m_Player2Name}: {i_Player2Score}";
        }
        internal void BoldLabelCurrentPlayer(ePlayerTurn i_CurrentTurn)
        {
            if(i_CurrentTurn == ePlayerTurn.XTurn)
            {
                m_Player1.Font = new Font(m_Player1.Font, FontStyle.Bold);
                m_Player2.Font = new Font(m_Player2.Font, FontStyle.Regular);
            }
            else
            {
                m_Player2.Font = new Font(m_Player2.Font, FontStyle.Bold);
                m_Player1.Font = new Font(m_Player1.Font, FontStyle.Regular);
            }
        }
    }
}
