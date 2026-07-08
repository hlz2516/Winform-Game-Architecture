using CoreLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreLib
{
    /// <summary>
    /// 实现了标准移动的物体，外部只需要设置基本形状路径即可（只支持多边形）
    /// </summary>
    public abstract class PolygonMoveObject : BaseObject,IMove
    {
        PointF rotateCenter; //一旦确定后不再改变
        public float ScaleFactor { get; set; } = 1;
        private float rotateAngle;
        public float RotateAngle
        {
            get => rotateAngle;
            set
            {
                if (rotateAngle != value)
                {
                    moveDirection = rotateAngle = value;
                }
            }
        }
        public float MoveSpeed { get; set; } = 4;
        private float moveDirection;
        public float MoveDirection
        {
            get => moveDirection;
            set
            {
                if (moveDirection != value)
                {
                    rotateAngle = moveDirection = value;
                }
            }
        }
        private GraphicsPath rotatedVertexes;
        public GraphicsPath RotatedVertexes => rotatedVertexes;
        private GraphicsPath originVertexes;
        public GraphicsPath OriginVertexes => originVertexes;
        private PointF currPosF;
        public PointF CurrPosF => currPosF;

        public PolygonMoveObject()
        {
            originVertexes = new GraphicsPath();
            rotatedVertexes = new GraphicsPath();
        }

        public virtual void MoveOneStep()
        {
            //计算这一次移动需要的偏移量，基于方向，速度（如果方向不变，直接调用上一次的偏移量）
            var offset = TrigonometricFunctions.GetPointByAngle(0, 0, MoveSpeed, moveDirection);
            //在上一次的理论位置加上这个偏移量，得到下一次的理论位置nextPosF，四舍五入得到整数坐标作为下一次实际位置更新，内存里更新currPosF = nextPosF
            var nextPosF = PointF.Add(currPosF, offset.ToSizeF());
            var realPos = new Point((int)Math.Round(nextPosF.X), (int)Math.Round(nextPosF.Y));
            this.Left = realPos.X;
            this.Top = realPos.Y;
            this.Invalidate(this.Region);
            currPosF = nextPosF;
        }
        
        public virtual void Rotate()
        {
            Matrix mat = new Matrix();
            mat.RotateAt(RotateAngle, rotateCenter);
            rotatedVertexes = originVertexes.Clone() as GraphicsPath;
            rotatedVertexes.Transform(mat);
            this.Region = new Region(rotatedVertexes);
            this.Invalidate(this.Region);
        }

        /// <summary>
        /// 实现时必须调用SetRegionInner输入形状路径，以实现Region设置，旋转和移动
        /// </summary>
        public abstract void SetRegion();
        //{
        //    PointF[] points = new PointF[3];
        //    points[0] = new PointF(0, 0);
        //    points[1] = new PointF(this.Width * ScaleFactor, 0);
        //    points[2] = new PointF(this.Width * ScaleFactor / 2, this.Width * ScaleFactor / 2);
        //   SetRegionInner(points);
        //}

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
            originVertexes.AddPolygon(points);  //建立初始的轮廓点
            originVertexes.CloseFigure();
            rotatedVertexes = originVertexes.Clone() as GraphicsPath; //设置初始旋转（0°）后的顶点位置和旋转中心
            rotateCenter = new PointF(center, center);
            this.Region = new Region(originVertexes);

            currPosF = new PointF(this.Left, this.Top);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintRotatedRegion(e, rotatedVertexes);
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

        protected virtual void PaintRotatedRegion(PaintEventArgs e, GraphicsPath rotatedRegion)
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
                g.DrawEllipse(borderPen, rotateCenter.X, rotateCenter.Y, 4, 4);
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
