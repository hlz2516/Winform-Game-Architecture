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
        EllipseMoveObject ellipseMoveObject;
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

            ellipseMoveObject = new EllipseMoveObject();
            ellipseMoveObject.Name = "elli";
            ellipseMoveObject.Left = 200;
            ellipseMoveObject.Top = 200;
            ellipseMoveObject.SetRegion();
            this.Controls.Add(ellipseMoveObject);
            ellipseMoveObject.Show();
        }

        int times = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (times < 30)
            {
                ellipseMoveObject.MoveOneStep();
            }
            else if (times >= 30 && times < 120)
            {
                ellipseMoveObject.Rotate();
                ellipseMoveObject.RotateAngle += 2;
            }
            else if (times >= 120 && times < 150)
            {
                ellipseMoveObject.MoveOneStep();
                if (times == 149)
                {
                    times = 0;
                    return;
                }
            }

            times++;
        }
    }
}
