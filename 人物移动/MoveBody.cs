using CoreLib;
using CoreLib.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace 人物移动
{
    public class MoveBody : BaseObject,IRecvKeyInput
    {
        readonly int speed = 5;
        readonly Timer timer;

        volatile bool currLeft = false; //false表示取消这一方向的移动,true表示要移动
        volatile bool currRight = false;
        volatile bool currUp = false;
        volatile bool currDown = false;
        
        public bool CurrentLeft { get => currLeft; }
        public bool CurrentRight { get => currRight; }
        public bool CurrentUp { get => currUp; }
        public bool CurrentDown { get => currDown; }

        public MoveBody()
        {
            timer = new Timer();
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int top = this.Top;
            int left = this.Left;
            if (currUp)
            {
                top -= 1 * speed;
            }
            if (currDown)
            {
                top += 1 * speed;
            }
            if (currLeft)
            {
                left -= 1 * speed;
            }
            if (currRight)
            {
                left += 1 * speed;
            }
            this.Top = top;
            this.Left = left;
        }

        public void GetCurrentKeyInput(KeyWrapper key)
        {
            //根据输入的键来更新移动指示
            if (key.KeyCode == Keys.Up)
            {
                bool nextUp = key.State == KeyState.KeyUp ? false : true;
                if (nextUp != currUp)
                {
                    currUp = nextUp;
                }
            }
            else if (key.KeyCode == Keys.Down)
            {
                bool nextDown = key.State == KeyState.KeyUp ? false : true;
                if (nextDown != currDown)
                {
                    currDown = nextDown;
                }
            }
            else if (key.KeyCode == Keys.Left)
            {
                bool nextLeft = key.State == KeyState.KeyUp ? false : true;
                if (nextLeft != currLeft)
                {
                    currLeft = nextLeft;
                }
            }
            else if (key.KeyCode == Keys.Right)
            {
                bool nextRight = key.State == KeyState.KeyUp ? false : true;
                if (nextRight != currRight)
                {
                    currRight = nextRight;
                }
            }
        }

        public void SetRegion()
        {
            using (GraphicsPath g = new GraphicsPath())
            {
                g.AddEllipse(new Rectangle(0, 0, this.Width, this.Height));
                this.BackColor = Color.Red;
                this.Region = new Region(g);
            }
        }
    }

    public enum Direction 
    { 
        Left, 
        Right, 
        Up,
        Down
    }
}
