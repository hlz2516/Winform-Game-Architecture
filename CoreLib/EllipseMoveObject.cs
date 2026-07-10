using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CoreLib
{
    public abstract class EllipseMoveObject : MoveBaseObject
    {
        protected virtual void SetRegionInner(BezierPoints[] points)
        {
            var _rotateCenter = GetRotateCenter(points);
            //计算最大外接圆直径更新工作区大小
            int suitableClientSize = CalculMaxCircumcircleDiameter(points, _rotateCenter);
            this.Width = this.Height = suitableClientSize;
            //此时旋转中心为工作区中心，计算原旋转中心到现旋转中心的坐标偏移，将顶点都加上这个偏移量
            float center = (suitableClientSize / 2);
            var offset = new PointF(center, center).Minus(_rotateCenter);
            List<PointF> bezierPoints = new List<PointF>();
            for (int i = 0; i < points.Length; i++)
            {
                points[i].P0 = PointF.Add(points[i].P0, offset);
                points[i].P1 = PointF.Add(points[i].P1, offset);
                points[i].P2 = PointF.Add(points[i].P2, offset);
                points[i].P3 = PointF.Add(points[i].P3, offset);
                if (i == 0)
                {
                    bezierPoints.AddRange(new PointF[] { points[i].P0 , points[i].P1 , points[i].P2 , points[i].P3 });
                }
                else
                {
                    bezierPoints.AddRange(new PointF[] { points[i].P1, points[i].P2, points[i].P3 });  //数组长度为4+3N
                }
            }
            originVertexes.AddBeziers(bezierPoints.ToArray());
            rotatedVertexes = originVertexes.Clone() as GraphicsPath; //设置初始旋转（0°）后的顶点位置和旋转中心
            rotateCenter = new PointF(center, center);
            this.Region = new Region(originVertexes);

            currPosF = new PointF(this.Left, this.Top);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintRegionInner(e, rotatedVertexes);
        }

        protected virtual PointF GetRotateCenter(BezierPoints[] bezierPoints)
        {
            return new PointF(Width * ScaleFactor / 2, Height * ScaleFactor / 2);
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
                //绘制旋转中心
                g.DrawEllipse(borderPen, rotateCenter.X-2, rotateCenter.Y-2, 4, 4);
            }
        }

        protected virtual int CalculMaxCircumcircleDiameter(BezierPoints[] points, PointF rotateCenter)
        {
            float longestDist = 0f;
            foreach (var point in points)
            {
                float dist1 = rotateCenter.DistanceTo(point.P0);
                float dist2 = rotateCenter.DistanceTo(point.P3);
                float longerDist = Math.Max(dist1, dist2);
                if (longerDist > longestDist)
                {
                    longestDist = longerDist;
                }
            }

            return (int)Math.Ceiling(longestDist) * 2;
        }
    }
}
