using CoreLib;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using 人物移动;
using 敌人;

namespace 碰撞检测
{
    public partial class Form1 : Form
    {
        Wall wall;
        Wall wall2;
        MoveBody body;
        TriangleEnemy triangle;
        DrillBitEnemy drillbit;
        List<Control> controls = new List<Control>();

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

            wall = new Wall();
            wall.Name = "wall";
            wall.Left = 400;
            wall.Top = 400;
            wall.SetRegion();
            this.Controls.Add(wall);
            wall.Show();
            controls.Add(wall);

            wall2 = new Wall();
            wall2.Name = "wall2";
            wall2.Left = 300;
            wall2.Top = 300;
            wall2.SetRegion();
            this.Controls.Add(wall2);
            wall2.Show();
            controls.Add(wall2);

            triangle = new TriangleEnemy();
            triangle.Name = "enemy";
            triangle.Left = 0;
            triangle.Top = 100;
            triangle.SetRegion();
            this.Controls.Add(triangle);
            triangle.StartMove();
            triangle.Show();
            controls.Add(triangle);

            drillbit = new DrillBitEnemy();
            drillbit.Name = "drillbit";
            drillbit.Left = 200;
            drillbit.Top = 100;
            drillbit.SetRegion();
            this.Controls.Add(drillbit);
            drillbit.StartMove();
            drillbit.Show();
            controls.Add(drillbit);

            body = new MoveBody();
            body.Name = "body";
            body.Width = body.Height = 100;
            body.Left = 100;
            body.Top = 100;
            body.SetRegion();
            this.Controls.Add(body);
            body.Show();

            KeyInputTrigger.Instance.Register(body.GetCurrentKeyInput, Keys.Up, Keys.Down, Keys.Left, Keys.Right);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyInputTrigger.Instance.InputKey(new KeyWrapper(e,KeyState.KeyPressed));
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

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (CollisionHelper.CheckRegionsCollision(body, controls,out var collisionCtrls))
            {
                button1.Text = $"碰撞了{string.Join(",", collisionCtrls.Select(x => x.Name).ToArray())}";
            }
            else
            {
                button1.Text = "没碰撞";
            }
        }
    }
}
