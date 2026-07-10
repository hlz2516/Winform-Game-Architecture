using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace 敌人
{
    public partial class Form1 : Form
    {
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

            var body = new TriangleEnemy();
            body.Name = "enemy";
            body.Left = 200;
            body.Top = 200;
            body.SetRegion();
            this.Controls.Add(body);
            body.StartMove();
            body.Show();

            var drillbit = new DrillBitEnemy();
            drillbit.Name = "drillbit";
            drillbit.Left = 600;
            drillbit.Top = 200;
            drillbit.SetRegion();
            this.Controls.Add(drillbit);
            drillbit.StartMove();
            drillbit.Show();
        }
    }
}
