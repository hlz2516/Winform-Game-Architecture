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

namespace 椭圆路径研究
{
    public partial class FrmEncapsulationTest : Form
    {
        CircleEnemy circle;
        public FrmEncapsulationTest()
        {
            InitializeComponent();
            InitializeComponent();
            // 开启窗体双缓冲全套
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer,
                true);
            UpdateStyles(); // 立即生效样式

            circle = new CircleEnemy();
            circle.Name = "elli";
            circle.Left = 10;
            circle.Top = 100;
            circle.SetRegion();
            circle.StartMove();
            this.Controls.Add(circle);
            circle.Show();
        }
    }
}
