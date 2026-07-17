using CoreLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 贪吃蛇
{
    public partial class Form1 : Form
    {
        SnakeCharacter snake;
        public Form1()
        {
            InitializeComponent();
            // 开启窗体双缓冲全套
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer,
                true);
            UpdateStyles(); // 立即生效样式

            snake = new SnakeCharacter();
            snake.Left = 200;
            snake.Top = 200;
            snake.SetRegion();
            this.Controls.Add(snake);
            //设置初始朝向
            snake.MoveDirection = snake.RotateAngle = 270;
            snake.Rotate();
            snake.StartMove();

            KeyInputTrigger.Instance.Register(snake.GetCurrentKeyInput, Keys.Up, Keys.Down, Keys.Left, Keys.Right);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyInputTrigger.Instance.InputKey(new KeyWrapper(e, KeyState.KeyPressed));
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            KeyInputTrigger.Instance.InputKey(new KeyWrapper(e, KeyState.KeyUp));
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    // 手动触发按键事件
                    OnKeyDown(new KeyEventArgs(keyData));
                    return true; // 返回true：告知系统按键已处理，不再执行焦点跳转
            }
            return base.ProcessDialogKey(keyData);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
