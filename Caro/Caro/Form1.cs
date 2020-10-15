using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            prcbTime.Step = Cons.STEP_COOL_DOWN;
            prcbTime.Maximum = Cons.TIME_COOL_DOWN;
            prcbTime.Value = 0;

            tmCooldown.Interval = Cons.INTERVAL_COOL_DOWN;

            DrawChessBoard();

        }
        private List<Player> player;

        public List<Player> Player
        {
            get { return player; }
            set { player = value; }
        }

        private int currentPlayer;

        public int CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }

        private List<List<Button>> matrix;

        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }

        void DrawChessBoard()
        {
            pnlChessBoard.Enabled = true;
            Matrix = new List<List<Button>>();
            this.Player = new List<Player>() { 
                new Player ("Seimei", Image.FromFile(Application.StartupPath + "\\assets\\x_symbol.png")),
                new Player ("Dark Seimei", Image.FromFile(Application.StartupPath + "\\assets\\o_symbol.png"))
            };
            
            CurrentPlayer = 0;
            ChangePlayer();
            Button firstButton = new Button()
            {
                Width = 0, Location = new Point(0, 0)
            };
            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button(){
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(firstButton.Location.X + firstButton.Width, firstButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString(),
                    };
                    btn.Click += btn_Click;
                    pnlChessBoard.Controls.Add(btn);
                    Matrix[i].Add(btn);
                    firstButton = btn;
                }
                firstButton.Location = new Point(0, firstButton.Location.Y + Cons.CHESS_HEIGHT);
                firstButton.Width = 0;
                firstButton.Height = 0;
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null)
                return;
            Mark(btn);
            ChangePlayer();

            tmCooldown.Start();
            prcbTime.Value = 0;

            if (isEndGame(btn))
            {
                EndGame();
            }

         
        }

        public void EndGame()
        {
            tmCooldown.Stop();
            pnlChessBoard.Enabled = false;
            MessageBox.Show("End!");
        }

        private bool isEndGame(Button btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndMainDiagonal(btn) || isEndSecondaryDiagonal(btn);
        }

        private Point GetChessPoint(Button btn)
        {
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);
            Point point = new Point(horizontal, vertical);
            return point;
        }

        private bool isEndHorizontal(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countLeft = 0;
            for(int i = point.X; i >= 0; i--){
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }

            int countRight = 0;
            for (int i = point.X + 1; i <Cons.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }

            return countLeft + countRight == 5;
        }

        private bool isEndVertical(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countUp = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countUp++;
                }
                else
                    break;
            }

            int countDown = 0;
            for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countDown++;
                }
                else
                    break;
            }

            return countUp + countDown == 5;
        }

        private bool isEndMainDiagonal(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countUp = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countUp++;
                }
                else
                    break;
            }

            int countDown = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X + i >= Cons.CHESS_BOARD_WIDTH)
                    break;
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countDown++;
                }
                else
                    break;
            }

            return countUp + countDown == 5;
        }

        private bool isEndSecondaryDiagonal(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countUp = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countUp++;
                }
                else
                    break;
            }

            int countDown = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X - i < 0)
                    break;

                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countDown++;
                }
                else
                    break;
            }

            return countUp + countDown == 5;
            
        }
        private void Mark(Button btn){
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }

        private void ChangePlayer()
        {
            txbPlayerName.Text = Player[CurrentPlayer].Name;
            pctbMark.Image = Player[CurrentPlayer].Mark;
        }


        private void tmCooldown_Tick(object sender, EventArgs e)
        {
            prcbTime.PerformStep();

            if (prcbTime.Value >= prcbTime.Maximum)
            {
                EndGame();               
            }
        }
    }
}
