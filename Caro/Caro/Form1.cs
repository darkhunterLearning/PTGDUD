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
       
        void DrawChessBoard()
        {
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
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button(){
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(firstButton.Location.X + firstButton.Width, firstButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch
                    };
                    btn.Click += btn_Click;
                    pnlChessBoard.Controls.Add(btn);
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
    }
}
