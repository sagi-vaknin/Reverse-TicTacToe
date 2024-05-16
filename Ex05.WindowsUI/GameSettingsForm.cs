using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.WindowsUI
{
    internal class GameSettingsForm : Form
    {
        private Label m_Players;
        private Label m_Player1;
        private TextBox m_Player1TextBox;
        private CheckBox m_Player2CheckBox;
        private TextBox m_Player2TextBox;
        private Label m_BoardSize;
        private Label m_Rows;
        private NumericUpDown m_RowsNumericUpDown;
        private Label m_Cols;
        private NumericUpDown m_ColsNumericUpDown;
        private Button m_StartButton;

        public GameSettingsForm()
        {
            this.FormClosed += gameSettingsForm_Closed;
            initalizeComponent();
            ShowDialog();
        }
        private void gameSettingsForm_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        internal NumericUpDown RowsNumericUpDown
        {
            get
            {
                return m_RowsNumericUpDown;
            }
        }
        internal CheckBox Player2CheckBox
        {
            get
            {
                return m_Player2CheckBox;
            }
        }
        private String retrieve2ndPlayer()
        {
            String opponent = "";

            if(m_Player2CheckBox.Checked)
            {
                opponent = m_Player2TextBox.Text;
            }
            else
            {
                opponent = "Computer";
            }

            return opponent;
        }
        private void initalizeComponent()
        {
            // create the fields
            m_Players = new Label();
            m_Player1 = new Label();
            m_Player1TextBox = new TextBox();
            m_Player2CheckBox = new CheckBox();
            m_Player2TextBox = new TextBox();
            m_BoardSize = new Label();
            m_Rows = new Label();
            m_RowsNumericUpDown = new NumericUpDown();
            m_Cols = new Label();
            m_ColsNumericUpDown = new NumericUpDown();
            m_StartButton = new Button();

            // form settings
            this.Text = "Game Settings";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 300;
            this.Height = 280;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            // set componenets
            m_Players.Text = "Players:";
            m_Players.Top = 20;
            m_Players.Left = 20;
            this.Controls.Add(m_Players);

            m_Player1.Text = "Player 1:";
            m_Player1.Top = m_Players.Bottom + 5;
            m_Player1.Left = m_Players.Left + 15;
            this.Controls.Add(m_Player1);

            m_Player1TextBox.Top = m_Player1.Top;
            m_Player1TextBox.Left = m_Player1.Right + 4;
            m_Player1TextBox.MaxLength = 10;
            this.Controls.Add(m_Player1TextBox);

            m_Player2CheckBox.Text = "Player 2:";
            m_Player2CheckBox.Top = m_Player1.Bottom + 5;
            m_Player2CheckBox.Left = m_Player1.Left;
            m_Player2TextBox.Enabled = false;
            m_Player2TextBox.Text = "[Computer]";
            m_Player2TextBox.MaxLength = 10;
            this.Controls.Add(m_Player2CheckBox);
            m_Player2CheckBox.CheckedChanged += new EventHandler(opponentCheckBox_Checked);

            m_Player2TextBox.Top = m_Player2CheckBox.Top;
            m_Player2TextBox.Left = m_Player2CheckBox.Right;
            this.Controls.Add(m_Player2TextBox);

            m_BoardSize.Text = "Board Size:";
            m_BoardSize.Left = m_Players.Left;
            m_BoardSize.Top = m_Player2CheckBox.Bottom + 25;
            this.Controls.Add(m_BoardSize);

            m_Rows.Text = "Rows:";
            m_Rows.Width = 50;
            m_Rows.Left = m_Player1.Left;
            m_Rows.Top = m_BoardSize.Bottom + 5;
            this.Controls.Add(m_Rows);

            m_RowsNumericUpDown.Minimum = 4;
            m_RowsNumericUpDown.Maximum = 10;
            m_RowsNumericUpDown.Width = 50;
            m_RowsNumericUpDown.Left = m_Rows.Right;
            m_RowsNumericUpDown.Top = m_Rows.Top - 2;
            this.Controls.Add(m_RowsNumericUpDown);
            m_RowsNumericUpDown.ValueChanged += new EventHandler(rowValue_Changed);

            m_Cols.Text = "Cols:";
            m_Cols.Width = 50;
            m_Cols.Left = m_RowsNumericUpDown.Right + 10;
            m_Cols.Top = m_Rows.Top;
            this.Controls.Add(m_Cols);

            m_ColsNumericUpDown.Minimum = 4;
            m_ColsNumericUpDown.Maximum = 10;
            m_ColsNumericUpDown.Width = 50;
            m_ColsNumericUpDown.Left = m_Cols.Right;
            m_ColsNumericUpDown.Top = m_Cols.Top - 2;
            this.Controls.Add(m_ColsNumericUpDown);
            m_ColsNumericUpDown.ValueChanged += new EventHandler(colValue_Changed);

            m_StartButton.Text = "Start!";
            m_StartButton.Left = m_BoardSize.Left;
            m_StartButton.Top = m_Rows.Bottom + 20;
            m_StartButton.Width = 245;
            this.Controls.Add(m_StartButton);
            m_StartButton.Click += new EventHandler(startButton_Clicked);
        }
        private void opponentCheckBox_Checked(object sender, EventArgs e)
        {
            m_Player2TextBox.Enabled = !m_Player2TextBox.Enabled;
            m_Player2TextBox.Text = m_Player2TextBox.Enabled ? "" : "[Computer]";
        }
        private void rowValue_Changed(object sender, EventArgs e)
        {
            m_ColsNumericUpDown.Value = m_RowsNumericUpDown.Value;
        }
        private void colValue_Changed(object sender, EventArgs e)
        {
            m_RowsNumericUpDown.Value = m_ColsNumericUpDown.Value;
        }
        private void startButton_Clicked(object sender, EventArgs e)
        {
            if(m_Player1TextBox.Text == String.Empty || (m_Player2TextBox.Text == String.Empty && m_Player2CheckBox.Enabled))
            {
                MessageBox.Show("Enter Players Names!", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                this.Hide();
                GameEngine newGame = new GameEngine(this, m_Player1TextBox.Text, retrieve2ndPlayer());
                newGame.CurrentGameForm.ShowDialog();
            }
        }
    }
}
