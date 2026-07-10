using CoreLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 椭圆路径研究
{
    public class CircleEnemy : EllipseMoveObject
    {
        Timer rotateTimer = new Timer();
        int times = 0;

        public CircleEnemy()
        {
            rotateTimer.Interval = 30;
            rotateTimer.Tick += Move_Logic;
        }

        private void Move_Logic(object sender, EventArgs e)
        {
            if (times < 30)
            {
                MoveOneStep();
            }
            else if (times >= 30 && times < 120)
            {
                Rotate();
                RotateAngle += 2;
            }
            else if (times >= 120 && times < 150)
            {
                MoveOneStep();
                if (times == 149)
                {
                    times = 0;
                    return;
                }
            }

            times++;
        }

        public void StartMove() => rotateTimer.Start();

        /// <summary>
        /// 实现时必须调用SetRegionInner输入贝塞尔曲线列表，以实现Region设置，旋转和移动
        /// </summary>
        public override void SetRegion()
        {
            BezierPoints[] ellipse = new BezierPoints[4];
            PointF center = new PointF(Width * ScaleFactor / 2, Height * ScaleFactor / 2);
            float xRadius = Width * ScaleFactor / 2;
            float yRadius = Height * ScaleFactor / 2;
            float startAngle = 0; // 起始角度
            float sweepAngle = (float)(Math.PI / 2); // 扫过角度
            for (int i = 0; i < 4; i++)
            {
                ellipse[i] = CurveHelper.GetEllipseArcBezierPoints(center, xRadius, yRadius, startAngle, sweepAngle);
                startAngle += sweepAngle;
            }
            SetRegionInner(ellipse);
        }

        protected override void PaintRegionInner(PaintEventArgs e, GraphicsPath rotatedRegion)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // 如果没有设置Region，直接退出
            if (this.Region == null)
                return;
            g.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
            using (Pen borderPen = new Pen(Color.Red, 2)) // 边框画笔：红色、宽度2
            {
                // 绘制Region外边框
                g.DrawBeziers(borderPen, rotatedRegion.PathPoints);
                //根据旋转角度绘制当前朝向上的眼睛，用于指示当前朝向
                var offset = TrigonometricFunctions.GetPointByAngle(0, 0, Width / 4,RotateAngle);
                var eyePos = PointF.Add(rotateCenter, offset.ToSizeF());
                g.DrawEllipse(borderPen, eyePos.X - 2, eyePos.Y - 2, 4, 4);
                //绘制旋转中心
                //g.DrawEllipse(borderPen, RotateCenter.X - 2, RotateCenter.Y - 2, 4, 4);
            }
        }
    }
}
