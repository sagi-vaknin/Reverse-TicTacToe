using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ex05.GameLogic
{
    public class LogicBoard
    {
        public delegate void BoardUpdatedEventHandler(int i_Row, int i_Col);
        public delegate void GameEndedEventHandler(eGameStates i_EndGameState);

        private eBoardSigns[,] m_Board;
        private readonly int r_BoardSize;
        private static Random s_Random;
        public event BoardUpdatedEventHandler BoardChanged;
        public event GameEndedEventHandler GameEnded;

        public LogicBoard(int i_BoardSize)
        {
            m_Board = new eBoardSigns[i_BoardSize, i_BoardSize];
            r_BoardSize = i_BoardSize;
            s_Random = null;
        }
        internal eBoardSigns[,] Board
        {
            get
            {
                return m_Board;
            }
        }
        internal int BoardSize
        {
            get
            {
                return r_BoardSize;
            }
        }
        public void ClearBoard()
        {
            for(int i = 0; i < r_BoardSize; i++)
            {
                for(int j = 0; j < r_BoardSize; j++)
                {
                    m_Board[i, j] = eBoardSigns.NoSign;
                }
            }
        }
        public void UpdateCell(int i_Row, int i_Col, eBoardSigns i_BoardSignToInsert)
        {
            m_Board[i_Row, i_Col] = i_BoardSignToInsert;
            checkForADraw();
            checkForAWin();
            OnCellUpdate(i_Row, i_Col);
        }
        public void GenerateComputerMove()
        {
            if(s_Random == null)
            {
                s_Random = new Random();
            }

            List<int[]> emptySlots = findEmptySlots();
            int randomIndex = s_Random.Next(0, emptySlots.Count);
            int[] computerMove = emptySlots[randomIndex];

            UpdateCell(computerMove[0], computerMove[1], eBoardSigns.O);
        }
        private List<int[]> findEmptySlots()
        {
            List<int[]> slots = new List<int[]>();

            for(int i = 0; i < BoardSize; i++)
            {
                for(int j = 0; j < BoardSize; j++)
                {
                    if(Board[i, j] == eBoardSigns.NoSign)
                    {
                        int[] emptySlot = { i, j };

                        slots.Add(emptySlot);
                    }
                }
            }

            return slots;
        }
        private void checkForADraw()
        {
            bool gameEndedWithDraw = true;

            for(int i = 0; i < BoardSize; i++)
            {
                for(int j = 0; j < BoardSize; j++)
                {
                    if(Board[i, j] == eBoardSigns.NoSign)
                    {
                        gameEndedWithDraw = false;
                        break;
                    }
                }

                if(!gameEndedWithDraw)
                {
                    break;
                }
            }

            if(gameEndedWithDraw)
            {
                OnGameEnd(eGameStates.Draw);
            }
        }
        private void checkForHorizontalWin()
        {
            int xApperances = 0, oApperances = 0;
            eGameStates GameWinner;

            //checking if horizontal "losing" move
            for(int i = 0; i < BoardSize; i++)
            {
                for(int j = 0; j < BoardSize; j++)
                {
                    if((Board[i, j] == eBoardSigns.X))
                    {
                        xApperances++;
                    }
                    else if(Board[i, j] == eBoardSigns.O)
                    {
                        oApperances++;
                    }
                    else
                    {
                        continue;
                    }
                }

                if((xApperances == BoardSize) || (oApperances == BoardSize))
                {
                    GameWinner = (xApperances == BoardSize) ? eGameStates.OWon : eGameStates.XWon;
                    OnGameEnd(GameWinner);
                    break;
                }
                else
                {
                    xApperances = 0;
                    oApperances = 0;
                }
            }
        }
        private void checkForVerticalWin()
        {
            int xApperances = 0, oApperances = 0;
            eGameStates GameWinner;

            for(int i = 0; i < BoardSize; i++)
            {
                for(int j = 0; j < BoardSize; j++)
                {
                    if((Board[j, i] == eBoardSigns.X))
                    {
                        xApperances++;
                    }
                    else if((Board[j, i]) == eBoardSigns.O)
                    {
                        oApperances++;
                    }
                    else
                    {
                        continue;
                    }
                }

                if((xApperances == BoardSize) || (oApperances == BoardSize))
                {
                    GameWinner = (xApperances == BoardSize) ? eGameStates.OWon : eGameStates.XWon;
                    OnGameEnd(GameWinner);
                    break;
                }
                else
                {
                    xApperances = 0;
                    oApperances = 0;
                }
            }
        }
        private void checkForMainDiagonalWin()
        {
            int xApperances = 0, oApperances = 0;
            eGameStates GameWinner;

            for(int i = 0; i < BoardSize; i++)
            {
                if(Board[i, i] == eBoardSigns.X)
                {
                    xApperances++;
                }
                else if(Board[i, i] == eBoardSigns.O)
                {
                    oApperances++;
                }
                else
                {
                    continue;
                }
            }

            if((xApperances == BoardSize) || (oApperances == BoardSize))
            {
                GameWinner = (xApperances == BoardSize) ? eGameStates.OWon : eGameStates.XWon;
                OnGameEnd(GameWinner);
            }
        }
        private void checkForSecondDiagonalWin()
        {
            int xApperances = 0, oApperances = 0;
            eGameStates GameWinner;

            for(int i = 0; i < BoardSize; i++)
            {
                if(Board[i, BoardSize - i - 1] == eBoardSigns.X)
                {
                    xApperances++;
                }
                else if(Board[i, BoardSize - i - 1] == eBoardSigns.O)
                {
                    oApperances++;
                }
                else
                {
                    continue;
                }
            }

            if((xApperances == BoardSize) || (oApperances == BoardSize))
            {
                GameWinner = (xApperances == BoardSize) ? eGameStates.OWon : eGameStates.XWon;
                OnGameEnd(GameWinner);
            }
        }
        private void checkForAWin()
        {
            checkForHorizontalWin();
            checkForVerticalWin();
            checkForMainDiagonalWin();
            checkForSecondDiagonalWin();
        }
        protected virtual void OnCellUpdate(int i_Row, int i_Col)
        {
            if(BoardChanged != null)
            {
                BoardChanged.Invoke(i_Row, i_Col);
            }
        }
        protected virtual void OnGameEnd(eGameStates i_EndGameState)
        {
            if(GameEnded != null)
            {
                GameEnded.Invoke(i_EndGameState);
            }
        }
    }
}
