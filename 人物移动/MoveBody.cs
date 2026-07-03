using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace 人物移动
{
    public class MoveBody : Control
    {
        readonly int speed = 4;
        readonly Timer timer;

        public NonRepetitiveAdjQueue<KeyWrapper> InputKeys { get; } = new NonRepetitiveAdjQueue<KeyWrapper>(); //不会有相邻重复的key事件参数
        bool currLeft = false; //false表示取消这一方向的移动,true表示要移动
        bool currRight = false;
        bool currUp = false;
        bool currDown = false;
        public MoveBody()
        {
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            while (InputKeys.Count > 0)
            {
                InputKeys.TryDequeue(out var key);
                //拿这一次的key分析在这个方向上是要继续还是停止，如果跟当前指示不一致，那么更新当前指示
                //如果是keydown说明要继续要往这个方向走,如果是keyup，说明这个方向就不走了
                //也有可能这一次没接收到这个方向的指示，那么就按照当前指示走
                if (key.KeyCode == Keys.Up)
                {
                    bool nextUp = false;
                    nextUp = key.State == KeyState.KeyUp ? false : true;
                    if (nextUp != currUp)
                    {
                        currUp = nextUp;
                    }
                }
                else if (key.KeyCode == Keys.Down)
                {
                    bool nextDown = false;
                    nextDown = key.State == KeyState.KeyUp ? false : true;
                    if (nextDown != currDown)
                    {
                        currDown = nextDown;
                    }
                }
                else if (key.KeyCode == Keys.Left)
                {
                    bool nextLeft = false;
                    nextLeft = key.State == KeyState.KeyUp ? false : true;
                    if (nextLeft != currLeft)
                    {
                        currLeft = nextLeft;
                    }
                }
                else if (key.KeyCode == Keys.Right)
                {
                    bool nextRight = false;
                    nextRight = key.State == KeyState.KeyUp ? false : true;
                    if (nextRight != currRight)
                    {
                        currRight = nextRight;
                    }
                }
            }

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
