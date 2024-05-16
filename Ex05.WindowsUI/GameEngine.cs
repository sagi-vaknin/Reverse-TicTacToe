using Ex05.GameLogic;
using Ex05.WindowsUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex05.WindowsUI
{
    internal class GameEngine
    {
        private LogicBoard m_CurrentBoard;
        private GameForm m_CurrentGameForm;
        private readonly int r_BoardSize;
        private readonly bool r_PlayVsComputer;
        private ePlayerTurn m_TurnOf;
        private eGameStates m_GameState;
        private ScoreTable m_ScoreBoard;
        private readonly String r_Player1Name;
        private readonly String r_Player2Name;

        public GameEngine(GameSettingsForm i_GameSettings, String i_Player1Name, String i_Player2Name)
        {
            r_BoardSize = (int)i_GameSettings.RowsNumericUpDown.Value;
            m_CurrentBoard = new LogicBoard(r_BoardSize);
            r_PlayVsComputer = !(i_GameSettings.Player2CheckBox.Checked);
            m_TurnOf = ePlayerTurn.XTurn;
            m_GameState = eGameStates.InAction;
            m_ScoreBoard = new ScoreTable();
            r_Player1Name = i_Player1Name;
            r_Player2Name = i_Player2Name;
            m_CurrentGameForm = new GameForm(r_Player1Name, r_Player2Name, r_BoardSize);

            for(int i = 0; i < r_BoardSize; i++) 
            {
                for(int j = 0; j < r_BoardSize; j++)
                {
                    CurrentGameForm.FormBoard[i, j].Click += new EventHandler(cell_Clicked);
                }
            }

            CurrentBoard.ClearBoard();
            CurrentBoard.GameEnded += new LogicBoard.GameEndedEventHandler(game_Ended);
            CurrentBoard.BoardChanged += new LogicBoard.BoardUpdatedEventHandler(cell_Updated);
        }
        internal LogicBoard CurrentBoard
        {
            get
            {
                return m_CurrentBoard;
            }

            set
            {
                m_CurrentBoard = value;
            }
        }
        internal eGameStates GameState
        {
            get
            {
                return m_GameState;
            }

            set
            {
                m_GameState = value;
            }
        }
        internal ePlayerTurn TurnOf
        {
            get
            {
                return m_TurnOf;
            }

            set
            {
                m_TurnOf = value;
            }
        }
        internal bool PlayVsComputer
        {
            get
            {
                return r_PlayVsComputer;
            }
        }
        internal ScoreTable ScoreBoard
        {
            get
            {
                return m_ScoreBoard;
            }
        }
        internal GameForm CurrentGameForm
        {
            get
            {
                return m_CurrentGameForm;
            }
        }
        private void cell_Clicked(object sender, EventArgs e)
        {
            BoardButton pressedButton = sender as BoardButton;
            updateUserMove(pressedButton);

            if(this.PlayVsComputer && this.TurnOf == ePlayerTurn.OTurn && this.GameState == eGameStates.InAction)
            {
                this.CurrentBoard.GenerateComputerMove();
                this.TurnOf = ePlayerTurn.XTurn;
                CurrentGameForm.BoldLabelCurrentPlayer(this.TurnOf);

                if(this.GameState != eGameStates.InAction)
                {
                    doWhenGameEnded();
                }
            }
        }
        private void updateUserMove(BoardButton i_PressedButton)
        {
            if(this.TurnOf == ePlayerTurn.XTurn)
            {
                this.CurrentBoard.UpdateCell(i_PressedButton.ButtonRow, i_PressedButton.ButtonCol, eBoardSigns.X);
                this.TurnOf = ePlayerTurn.OTurn;
            }
            else
            {
                this.CurrentBoard.UpdateCell(i_PressedButton.ButtonRow, i_PressedButton.ButtonCol, eBoardSigns.O);
                this.TurnOf = ePlayerTurn.XTurn;
            }

            CurrentGameForm.BoldLabelCurrentPlayer(this.TurnOf);
            if(this.GameState != eGameStates.InAction)
            {
                doWhenGameEnded();
            }
        }
        private String retrieveMessageBoxText()
        {
            String output = "";

            if(this.GameState == eGameStates.XWon)
            {
                output = string.Format(@"The winner is {0}!
Would you like to play another round?", r_Player1Name);
            }
            else if(this.GameState == eGameStates.OWon)
            {
                output = string.Format(@"the winner is {0}!,
Would you like to play another round?", r_Player2Name);
            }
            else if(this.GameState == eGameStates.Draw)
            {
                output = string.Format(@"Tie!
Would you like to play another round?");
            }

            return output;
        }
        private String retrieveMessageBoxTitle()
        {
            String title = "";

            if((this.GameState == eGameStates.XWon) || (this.GameState == eGameStates.OWon))
            {
                title = "A Win!";
            }
            else if(this.GameState == eGameStates.Draw)
            {
                title = "A Tie!";
            }

            return title;
        }
        private void doWhenGameEnded()
        {
            String messageBoxText = retrieveMessageBoxText();
            String messageBoxTitle = retrieveMessageBoxTitle();

            if(MessageBox.Show(messageBoxText, messageBoxTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.prepareAnotherRoundTable();
            }
            else
            {
                this.CurrentGameForm.Close();
            }
        }
        private void prepareAnotherRoundTable()
        {
            for(int i = 0; i < r_BoardSize; i++)
            {
                for(int j = 0; j < r_BoardSize; j++)
                {
                    this.CurrentGameForm.FormBoard[i, j].Text = "";
                    this.CurrentGameForm.FormBoard[i, j].Enabled = true;
                }
            }

            this.CurrentBoard.ClearBoard();
            this.TurnOf = ePlayerTurn.XTurn;
            CurrentGameForm.UpdateScoreLabels(ScoreBoard.Player1Score, ScoreBoard.Player2Score);
            CurrentGameForm.BoldLabelCurrentPlayer(this.TurnOf);
            this.GameState = eGameStates.InAction;
        }
        private void cell_Updated(int i_Row, int i_Col)
        {
            CurrentGameForm.FormBoard[i_Row, i_Col].Enabled = false;
            CurrentGameForm.FormBoard[i_Row, i_Col].Text = (this.TurnOf == ePlayerTurn.XTurn) ? "X" : "O";
        }
        private void game_Ended(eGameStates i_EndGameState)
        {
            m_GameState = i_EndGameState;
            m_ScoreBoard.UpdateScoreBoard(this);
        }
    }
}
