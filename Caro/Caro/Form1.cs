using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {
        SocketManager socket;
        public Form1()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            prcbTime.Step = Cons.STEP_COOL_DOWN;
            prcbTime.Maximum = Cons.TIME_COOL_DOWN;
            prcbTime.Value = 0;

            tmCooldown.Interval = Cons.INTERVAL_COOL_DOWN;

            socket = new SocketManager();

            NewGame();

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
            pnlChessBoard.Controls.Clear();
            PlayTimeLine = new Stack<PlayInfo>();

            Matrix = new List<List<Button>>();
            this.Player = new List<Player>() { 
                new Player ("Player X", Image.FromFile(Application.StartupPath + "\\assets\\x_symbol.png")),
                new Player ("Player O", Image.FromFile(Application.StartupPath + "\\assets\\o_symbol.png"))
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

            PlayTimeLine.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

            ChangePlayer();           

            tmCooldown.Start();
            pnlChessBoard.Enabled = false;
            prcbTime.Value = 0;

            socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", GetChessPoint(btn)));
            undoToolStripMenuItem.Enabled = false;

            Listen();

            if (isEndGame(btn))
            {
                EndGame();
                socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));
            }         
        }

        public void EndGame()
        {
            tmCooldown.Stop();
            pnlChessBoard.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
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
        }

        private void ChangePlayer()
        {
            txbPlayerName.Text = Player[CurrentPlayer].Name;
            pctbMark.Image = Player[CurrentPlayer].Mark;
        }

        public void OtherPlayerMark(Point point)
        {

            Button btn = Matrix[point.Y][point.X];

            if (btn.BackgroundImage != null)
                return;

            Mark(btn);

            PlayTimeLine.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

            ChangePlayer();

            //tmCooldown.Start();
            //prcbTime.Value = 0;
            //socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", new Point(0, 0)));

            if (isEndGame(btn))
            {
                EndGame();
                socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));
            }
        }

        private void tmCooldown_Tick(object sender, EventArgs e)
        {
            prcbTime.PerformStep();

            if (prcbTime.Value >= prcbTime.Maximum)
            {
                EndGame();
                socket.Send(new SocketData((int)SocketCommand.TIME_OUT, "", new Point()));
            }
        }

        private Stack<PlayInfo> playTimeLine;

        public Stack<PlayInfo> PlayTimeLine
        {
            get { return playTimeLine; }
            set { playTimeLine = value; }
        }

        void NewGame()
        {
            prcbTime.Value = 0;
            tmCooldown.Stop();
            undoToolStripMenuItem.Enabled = true;
            btnConnect.Enabled = true;
            DrawChessBoard();
        }

        bool Undo()
        {
            try
            {
                if (PlayTimeLine.Count <= 0)
                {
                    return false;
                }

                bool isUndo1 = UndoAStep();
                bool isUndo2 = UndoAStep();

                PlayInfo oldPlayer = PlayTimeLine.Peek();
                CurrentPlayer = oldPlayer.CurrentPlayer == 1 ? 0 : 1;
                
                return isUndo1 && isUndo2;
            }
            catch
            { 
                return false;
            }
        }

        private bool UndoAStep()
        {
            prcbTime.Value = 0;
            if (PlayTimeLine.Count <= 0)
            {
                prcbTime.Value = 0;
            }
            PlayInfo oldPlayer = PlayTimeLine.Pop();
            Button btn = Matrix[oldPlayer.Point.Y][oldPlayer.Point.X];

            btn.BackgroundImage = null;

            if (PlayTimeLine.Count <= 0)
            {
                CurrentPlayer = 0;
            }
            else
            {
                oldPlayer = PlayTimeLine.Peek();                
            }

            ChangePlayer();

            return true;
        }

        void Exit()
        {
            Application.Exit();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            socket.Send(new SocketData((int)SocketCommand.NEW_GAME, "", new Point()));
            pnlChessBoard.Enabled = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prcbTime.Value = 0;
            Undo();
            socket.Send(new SocketData((int)SocketCommand.UNDO, "", new Point()));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure want to exit?", "Confirm Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
            else
            {
                try
                {
                    socket.Send(new SocketData((int)SocketCommand.QUIT, "", new Point()));
                }
                catch { }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            socket.IP = txbIP.Text;
            try
            {
                btnConnect.Enabled = false;
                
                if (!socket.ConnectServer())
                {
                    socket.isServer = true;
                    pnlChessBoard.Enabled = true;     
                    socket.CreateServer();
                }
                else
                {
                    socket.isServer = false;
                    pnlChessBoard.Enabled = false;            
                    Listen();
                }
            }
            catch { }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);

            if (string.IsNullOrEmpty(txbIP.Text))
            {
                txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }
        void Listen()
        {
                Thread listenThread = new Thread(() =>
                {
                    try
                    {
                        SocketData data = (SocketData)socket.Receive();
                        ProcessData(data);
                    }
                    catch { }
                });
                listenThread.IsBackground = true;
                listenThread.Start(); 
        }

        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.NEW_GAME:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        NewGame();
                        pnlChessBoard.Enabled = false;
                    }));
                    break;
                case (int)SocketCommand.QUIT:
                    tmCooldown.Stop();
                    MessageBox.Show(this, "Opponent has exited!");
                    break;
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        prcbTime.Value = 0;
                        pnlChessBoard.Enabled = true;
                        tmCooldown.Start();
                        OtherPlayerMark(data.Point);
                        undoToolStripMenuItem.Enabled = true;
                    }));         
                    break;
                case (int)SocketCommand.UNDO:
                    Undo();
                    prcbTime.Value = 0;
                    break;
                case (int)SocketCommand.TIME_OUT:
                    MessageBox.Show(this, "Time out!");
                    break;
                case (int)SocketCommand.END_GAME:                   
                    MessageBox.Show(this, "Game over!!");
                    break;
                default:
                    break;
            }
            Listen();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Info = new Form2();        
            Info.Show();
        }
        /*public class ButtonClickEvent : EventArgs
        {
            private Point clickedPoint;

            public Point ClickedPoint
            {
                get { return clickedPoint; }
                set { clickedPoint = value; }
            }

            public ButtonClickEvent(Point point)
            {
                this.ClickedPoint = point;
            }
        }*/
    }
}
