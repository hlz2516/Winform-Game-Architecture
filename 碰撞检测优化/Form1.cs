using CoreLib;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace 碰撞检测优化
{
    public partial class Form1 : Form
    {
        MainCharacter character;
        Wall wall1;
        Wall wall2;
        Wall wall3;
        Wall wall4;
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

            character = new MainCharacter();
            character.Name = "circle";
            character.Left = 0;
            character.Top = 0;
            character.SetRegion();
            MapContainer.Controls.Add(character);
            character.Show();

            wall1 = new Wall();
            wall1.Name = "wall1";
            wall1.Left = 100;
            wall1.Top = 100;
            wall1.SetRegion();
            this.Controls.Add(wall1);
            wall1.Show();
            MapContainer.Controls.Add(wall1);

            wall2 = new Wall();
            wall2.Name = "wall2";
            wall2.Left = 400;
            wall2.Top = 100;
            wall2.SetRegion();
            this.Controls.Add(wall2);
            wall2.Show();
            MapContainer.Controls.Add(wall2);

            wall3 = new Wall();
            wall3.Name = "wall3";
            wall3.Left = 100;
            wall3.Top = 400;
            wall3.SetRegion();
            this.Controls.Add(wall3);
            wall3.Show();
            MapContainer.Controls.Add(wall3);

            wall4 = new Wall();
            wall4.Name = "wall4";
            wall4.Left = 400;
            wall4.Top = 400;
            wall4.SetRegion();
            this.Controls.Add(wall4);
            wall4.Show();
            MapContainer.Controls.Add(wall4);

            KeyInputTrigger.Instance.Register(character.GetCurrentKeyInput, Keys.Up, Keys.Down, Keys.Left, Keys.Right);
            MapManager.SetMapContainer(MapContainer, 5, 5);
            MapManager.AddObjectToMap(wall1);
            MapManager.AddObjectToMap(wall2);
            MapManager.AddObjectToMap(wall3);
            MapManager.AddObjectToMap(wall4);
            MapManager.AddObjectToMap(character);

            character.NotifyContainerRefreshDetectedRegion += Character_NotifyContainerRefreshDetectedRegion;
            character.StartMove();
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

        private static List<Point> detectedPos = new List<Point>();
        private void Character_NotifyContainerRefreshDetectedRegion(List<Point> points)
        {
            detectedPos.Clear();
            detectedPos.AddRange(points);
            MapContainer.Invalidate();
        }
        
        private void MapContainer_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            foreach (var item in detectedPos)
            {
                g.FillRectangle(Brushes.Yellow, new Rectangle(item.X, item.Y, 150, 150));
            }

            g.Dispose();
        }
    }
}
