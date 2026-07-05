using CoreLib;
using CoreLib.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace 敌人
{
    public class TriangleEnemy : BaseObject,IRotate,IMove
    {
        PointF rotateCenter; //一旦确定后不再改变
        Timer rotateTimer = new Timer();
        int times = 0;
        public float ScaleInfactor { get; set; } = 1;
        private float rotateAngle;
        public float RotateAngle
        {
            get => rotateAngle;
            set
            {
                if (rotateAngle != value)
                {
                    moveDirection =rotateAngle = value;
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

        public TriangleEnemy()
        {
            originVertexes = new GraphicsPath();
            rotatedVertexes = new GraphicsPath();
            rotateTimer.Interval = 20;
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
                Matrix mat = new Matrix();
                mat.RotateAt(RotateAngle, rotateCenter);
                rotatedVertexes = originVertexes.Clone() as GraphicsPath;
                rotatedVertexes.Transform(mat);
                this.Region = new Region(rotatedVertexes);
                this.Invalidate(this.Region);
                RotateAngle += 2;
            }
            else if(times >= 120 && times < 150)
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

        public void MoveOneStep()
        {
            //计算这一次移动需要的偏移量，基于方向，速度（如果方向不变，直接调用上一次的偏移量）
            var offset = TrigonometricFunctions.GetPointByAngle(0,0,MoveSpeed, moveDirection);
            //在上一次的理论位置加上这个偏移量，得到下一次的理论位置nextPosF，四舍五入得到整数坐标作为下一次实际位置更新，内存里更新currPosF = nextPosF
            var nextPosF = PointF.Add(currPosF,offset.ToSizeF());
            var realPos = new Point((int)Math.Round(nextPosF.X), (int)Math.Round(nextPosF.Y));
            this.Left = realPos.X;
            this.Top = realPos.Y;
            this.Invalidate(this.Region);
            currPosF = nextPosF;
        }

        public void StartRotate() => rotateTimer.Start();

        public void SetRegion()
        {
            PointF[] points = new PointF[3];
            points[0] = new PointF(0, 0);
            points[1] = new PointF(this.Width * ScaleInfactor, 0);
            points[2] = new PointF(this.Width * ScaleInfactor / 2, this.Width * ScaleInfactor / 2);
            var _rotateCenter = CalculateRotateCenter(points);
            //计算最大外接圆直径更新工作区大小
            int suitableClientSize = CalculMaxCircumcircleDiameter(points, _rotateCenter);
            this.Width = this.Height = suitableClientSize;
            //此时旋转中心为工作区中心，计算原旋转中心到现旋转中心的坐标偏移，将顶点都加上这个偏移量
            float center = (suitableClientSize / 2);
            var offset = new PointF(center,center).Minus(_rotateCenter);
            for (int i = 0;i < 3;i++)
            {
                points[i] = PointF.Add(points[i], offset);
            }
            originVertexes.AddPolygon(points);  //建立初始的轮廓点
            originVertexes.CloseFigure();
            rotatedVertexes = originVertexes.Clone() as GraphicsPath; //设置初始旋转（0°）后的顶点位置和旋转中心
            rotateCenter = new PointF(center,center);
            this.Region = new Region(originVertexes);

            currPosF = new PointF(this.Left,this.Top);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintRotatedRegion(e, rotatedVertexes);
        }

        public PointF CalculateRotateCenter(PointF[] points)
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

        public void PaintRotatedRegion(PaintEventArgs e, GraphicsPath rotatedRegion)
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

        public int CalculMaxCircumcircleDiameter(PointF[] points, PointF rotateCenter)
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
