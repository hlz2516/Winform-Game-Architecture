using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 椭圆路径研究
{
    public partial class FrmDrawEllipse : Form
    {
        public FrmDrawEllipse()
        {
            InitializeComponent();
        }

        private void FrmDrawEllipse_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            //假设圆心在(200,200)，X半径为100，Y半径为50，起始角度为0，旋转角度为90度
            PointF center = new PointF(200, 200);
            float xRadius = 100;
            float yRadius = 50;
            float startAngle = 0; // 起始角度
            float sweepAngle = (float)(Math.PI / 2); // 扫过角度
            var (p0, p1, p2, p3) = ArcBezierPointsHelper.GetEllipseArcBezierPoints(center, xRadius,yRadius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Blue, p0, p1, p2, p3);
            startAngle += sweepAngle;
            var (p4, p5, p6, p7) = ArcBezierPointsHelper.GetEllipseArcBezierPoints(center, xRadius, yRadius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Red, p4, p5, p6, p7);
            startAngle += sweepAngle;
            var (p8, p9, p10, p11) = ArcBezierPointsHelper.GetEllipseArcBezierPoints(center, xRadius, yRadius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Green, p8, p9, p10, p11);
            startAngle += sweepAngle;
            var (p12, p13, p14, p15) = ArcBezierPointsHelper.GetEllipseArcBezierPoints(center, xRadius, yRadius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Orange, p12, p13, p14, p15);
            g.Dispose();
        }
    }
}
