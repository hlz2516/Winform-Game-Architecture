using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CoreLib
{
    /// <summary>
    /// 实现了标准移动的物体，外部只需要设置基本形状路径即可（只支持多边形）
    /// </summary>
    public abstract class PolygonMoveObject : MoveBaseObject
    {
        protected virtual void SetRegionInner(PointF[] points)
        {
            var _rotateCenter = CalculateRotateCenter(points);
            //计算最大外接圆直径更新工作区大小
            int suitableClientSize = CalculMaxCircumcircleDiameter(points, _rotateCenter);
            this.Width = this.Height = suitableClientSize;
            //此时旋转中心为工作区中心，计算原旋转中心到现旋转中心的坐标偏移，将顶点都加上这个偏移量
            float center = (suitableClientSize / 2);
            var offset = new PointF(center, center).Minus(_rotateCenter);
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = PointF.Add(points[i], offset);
            }
            OriginVertexes.AddPolygon(points);  //建立初始的轮廓点
            OriginVertexes.CloseFigure();
            rotatedVertexes = OriginVertexes.Clone() as GraphicsPath; //设置初始旋转（0°）后的顶点位置和旋转中心
            rotateCenter = new PointF(center, center);
            this.Region = new Region(originVertexes);

            currPosF = new PointF(this.Left, this.Top);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintRegionInner(e, rotatedVertexes);
        }

        protected virtual PointF CalculateRotateCenter(PointF[] points)
        {
            float centerX = 0;
            float centerY = 0;
            foreach (PointF point in points)
            {
                centerX += point.X;
                centerY += point.Y;
            }
            return new PointF(centerX / points.Count(), centerY / points.Count());
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
                g.DrawPath(borderPen, rotatedRegion);
                //绘制旋转中心
                g.DrawEllipse(borderPen, rotateCenter.X-2, rotateCenter.Y-2, 4, 4);
            }
        }

        protected virtual int CalculMaxCircumcircleDiameter(PointF[] points, PointF rotateCenter)
        {
            float longestDist = 0f;
            foreach (var point in points)
            {
                float dist = rotateCenter.DistanceTo(point);
                if (dist > longestDist)
                {
                    longestDist = dist;
                }
            }

            return (int)Math.Ceiling(longestDist) * 2;
        }
    }
}
