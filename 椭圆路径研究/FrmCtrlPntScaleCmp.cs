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

namespace 椭圆路径研究
{
    public partial class FrmCtrlPntScaleCmp : Form
    {
        public FrmCtrlPntScaleCmp()
        {
            InitializeComponent();
        }

        private void FrmCtrlPntScaleCmp_Paint(object sender, PaintEventArgs e)
        {
            //圆弧经过缩放后，测试控制点的坐标是否与经过缩放比例计算得到的控制点坐标一致
            //假设圆心在(100,100)，半径为100，起始角度为0，旋转角度为90度
            var g = e.Graphics;
            PointF center = new PointF(100, 100);
            float radius = 100;
            float startAngle = 0; // 起始角度
            float sweepAngle = (float)(Math.PI / 2); // 扫过角度
            var (p0, p1, p2, p3) = ArcBezierPointsHelper.GetArcBezierPoints(center, radius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Blue, p0,p1,p2,p3);
            startAngle += sweepAngle;
            var (p4, p5, p6, p7) = ArcBezierPointsHelper.GetArcBezierPoints(center, radius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Red, p4, p5, p6, p7);
            startAngle += sweepAngle;
            var (p8, p9, p10, p11) = ArcBezierPointsHelper.GetArcBezierPoints(center, radius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Green, p8, p9, p10, p11);
            startAngle += sweepAngle;
            var (p12, p13, p14, p15) = ArcBezierPointsHelper.GetArcBezierPoints(center, radius, startAngle, sweepAngle);
            g.DrawBezier(Pens.Orange, p12, p13, p14, p15);

            //缩放比例为1.5，以左上角(0,0)为缩放中心
            p0= new PointF(p0.X * 1.5f, p0.Y * 1.5f);
            p1 = new PointF(p1.X * 1.5f, p1.Y * 1.5f);
            p2 = new PointF(p2.X * 1.5f, p2.Y * 1.5f);
            p3 = new PointF(p3.X * 1.5f, p3.Y * 1.5f);
            p4 = new PointF(p4.X * 1.5f, p4.Y * 1.5f);
            p5 = new PointF(p5.X * 1.5f, p5.Y * 1.5f);
            p6 = new PointF(p6.X * 1.5f, p6.Y * 1.5f);
            p7 = new PointF(p7.X * 1.5f, p7.Y * 1.5f);
            p8 = new PointF(p8.X * 1.5f, p8.Y * 1.5f);
            p9 = new PointF(p9.X * 1.5f, p9.Y * 1.5f);
            p10 = new PointF(p10.X * 1.5f, p10.Y * 1.5f);
            p11 = new PointF(p11.X * 1.5f, p11.Y * 1.5f);
            p12 = new PointF(p12.X * 1.5f, p12.Y * 1.5f);
            p13 = new PointF(p13.X * 1.5f, p13.Y * 1.5f);
            p14 = new PointF(p14.X * 1.5f, p14.Y * 1.5f);
            p15 = new PointF(p15.X * 1.5f, p15.Y * 1.5f);
            g.DrawBezier(Pens.Blue, p0, p1, p2, p3);
            g.DrawBezier(Pens.Red, p4, p5, p6, p7);
            g.DrawBezier(Pens.Green, p8, p9, p10, p11);
            g.DrawBezier(Pens.Orange, p12, p13, p14, p15);

            //通过公式计算缩放后的控制点坐标
            var centers = new PointF(center.X * 1.5f, center.Y * 1.5f);
            radius *= 1.5f;
            startAngle = 0;
            var (p0s, p1s, p2s, p3s) = ArcBezierPointsHelper.GetArcBezierPoints(centers, radius, startAngle, sweepAngle);
            Debug.WriteLine($"p1:{p1}  p1s:{p1s}");
            Debug.WriteLine($"p2:{p2}  p2s:{p2s}");
            startAngle += sweepAngle;
            var (p4s, p5s, p6s, p7s) = ArcBezierPointsHelper.GetArcBezierPoints(centers, radius, startAngle, sweepAngle);
            Debug.WriteLine($"p5:{p5}  p5s:{p5s}");
            Debug.WriteLine($"p6:{p6}  p6s:{p6s}");
            startAngle += sweepAngle;
            var (p8s, p9s, p10s, p11s) = ArcBezierPointsHelper.GetArcBezierPoints(centers, radius, startAngle, sweepAngle);
            Debug.WriteLine($"p9:{p9}  p9s:{p9s}");
            Debug.WriteLine($"p10:{p10}  p10s:{p10s}");
            startAngle += sweepAngle;
            var (p12s, p13s, p14s, p15s) = ArcBezierPointsHelper.GetArcBezierPoints(centers, radius, startAngle, sweepAngle);
            Debug.WriteLine($"p13:{p13}  p13s:{p13s}");
            Debug.WriteLine($"p14:{p14}  p14s:{p14s}");

            //事实证明，经过缩放后，控制点的坐标与通过公式计算得到的控制点坐标基本一致
        }
    }
}
