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

        void DrawChessBoard()
        {
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
                        Location = new Point(firstButton.Location.X + firstButton.Width, firstButton.Location.Y)
                    };
                    pnlChessBoard.Controls.Add(btn);
                    firstButton = btn;
                }
                firstButton.Location = new Point(0, firstButton.Location.Y + Cons.CHESS_HEIGHT);
                firstButton.Width = 0;
                firstButton.Height = 0;
            }
        }
    }
}
