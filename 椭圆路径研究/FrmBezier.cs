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
    public partial class FrmBezier : Form
    {
        public FrmBezier()
        {
            InitializeComponent();
        }

        private void FrmBezier_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 1. 定义4个贝塞尔控制点
            PointF P0 = new PointF(100, 300);  // 起点
            PointF P1 = new PointF(200, 100);  // 控制柄1
            PointF P2 = new PointF(400, 450);  // 控制柄2
            PointF P3 = new PointF(500, 200);  // 终点

            // ========== 1. 画出4个控制点 + 辅助控制线 ==========
            Pen dashPen = new Pen(Color.Gray) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            g.DrawLine(dashPen, P0, P1);
            g.DrawLine(dashPen, P2, P3);

            // 绘制控制点
            g.FillEllipse(Brushes.Red, P0.X - 5, P0.Y - 5, 10, 10);
            g.FillEllipse(Brushes.Orange, P1.X - 5, P1.Y - 5, 10, 10);
            g.FillEllipse(Brushes.Orange, P2.X - 5, P2.Y - 5, 10, 10);
            g.FillEllipse(Brushes.Red, P3.X - 5, P3.Y - 5, 10, 10);

            // ========== 2. 手动计算贝塞尔所有采样点（纯数学公式） ==========
            List<PointF> bezierPoints = new List<PointF>();
            int sampleCount = 200; // 采样越多越平滑
            for (int i = 0; i <= sampleCount; i++)
            { 
                float t = (float)i / sampleCount;
                float x = CalcBezier(t, P0.X, P1.X, P2.X, P3.X);
                float y = CalcBezier(t, P0.Y, P1.Y, P2.Y, P3.Y);
                bezierPoints.Add(new PointF(x, y));
            }
            // 连线画出手动计算的贝塞尔曲线（蓝色）
            g.DrawLines(Pens.Blue, bezierPoints.ToArray());

            // ========== 3. GDI+原生 GraphicsPath 贝塞尔（绿色对比） ==========
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddBezier(P0, P1, P2, P3);
            g.DrawPath(Pens.Green, path);

            dashPen.Dispose();
            path.Dispose();
        }

        /// <summary>
        /// 三次贝塞尔单轴计算函数
        /// </summary>
        private float CalcBezier(float t, float p0, float p1, float p2, float p3)
        {
            float u = 1 - t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t2 = t * t;
            float t3 = t2 * t;

            return u3 * p0
                 + 3 * u2 * t * p1
                 + 3 * u * t2 * p2
                 + t3 * p3;
        }
    }
}
