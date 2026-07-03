using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 人物移动
{
    public partial class Form1 : Form
    {
        MoveBody body;

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

            body = new MoveBody();
            body.Name = "body";
            body.Width = body.Height = 100;
            body.Left = 200;
            body.Top = 200;
            body.SetRegion();
            this.Controls.Add(body);
            body.Show();

            KeyInputTrigger.Instance.Register(Keys.Up, body.GetCurrentKeyInput);
            KeyInputTrigger.Instance.Register(Keys.Down, body.GetCurrentKeyInput);
            KeyInputTrigger.Instance.Register(Keys.Left, body.GetCurrentKeyInput);
            KeyInputTrigger.Instance.Register(Keys.Right, body.GetCurrentKeyInput);
        }


        private int seconds = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyInputTrigger.Instance.InputKeys.Enqueue(new KeyWrapper(e,KeyState.KeyPressed));
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            KeyInputTrigger.Instance.InputKeys.Enqueue(new KeyWrapper(e, KeyState.KeyUp));
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
    }
}
