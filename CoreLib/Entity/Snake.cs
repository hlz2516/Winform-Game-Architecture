using CoreLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CoreLib.Entity
{
    public class Snake : MoveBaseObject
    {
        public const int StepPixels = 8;

        private int PenWidth = 20;
        private LinkedList<PointF> bodyLine = new LinkedList<PointF>();
        /// <summary>
        /// 初始蛇整体朝向
        /// </summary>
        public override float RotateAngle { get => rotateAngle; set => rotateAngle = value; }
        /// <summary>
        /// 头移动方向
        /// </summary>
        public override float MoveDirection { get => moveDirection; set => moveDirection = value; }

        public Snake()
        {

        }

        public override void MoveOneStep()
        {
            //下一步移动逻辑里自带转向，根据当前方向和单步位移步数（默认8）计算下一步头的位置，如果下一步位置会跟墙碰撞，则不移动
            //如果确定移动，尾部减少一段长度，形成移动的效果
            PointF nextPosF = new PointF();
            for (int i = 1; i <= StepPixels; i++)
            {
                var offset = TrigonometricFunctions.GetPointByAngle(0, 0, i, MoveDirection);
                nextPosF = PointF.Add(currPosF, offset.ToSizeF());
                var realPos = new Point((int)Math.Round(nextPosF.X), (int)Math.Round(nextPosF.Y));
                bodyLine.AddFirst(realPos);
                bodyLine.RemoveLast();
            }
            
            //遍历所有points，得到minx,miny,maxx,maxy，计算出当前蛇的包围盒和坐标位置，
            float minX = bodyLine.Min(p => p.X);
            float minY = bodyLine.Min(p => p.Y);
            float maxX = bodyLine.Max(p => p.X);
            float maxY = bodyLine.Max(p => p.Y);
            Rectangle box = new Rectangle((int)minX - PenWidth / 2, (int)(minY - PenWidth / 2),
                (int)(maxX - minX + PenWidth), (int)(maxY - minY + PenWidth));
            //先把包围盒整体移动到（0，0），再把控件的Location进行反向偏移
            Point _offset = new Point((int)box.X, (int)box.Y);
            var node = bodyLine.First;
            while (node != null)
            {
                PointF oldPt = node.Value;
                node.Value = new PointF(oldPt.X - box.X, oldPt.Y - box.Y);
                node = node.Next;
            }

            currPosF = bodyLine.First.Value;
            this.Left = this.Left + _offset.X;
            this.Top = this.Top + _offset.Y;
            this.Width = box.Width;
            this.Height = box.Height;
            //使用widen方法把包围盒的区域扩大到蛇的宽度，作为控件的Region
            rotatedVertexes.Reset();
            rotatedVertexes.AddLines(bodyLine.ToArray());
            Pen widenPen = new Pen(Color.Transparent, PenWidth);
            widenPen.StartCap = LineCap.Round;
            widenPen.EndCap = LineCap.Round;
            widenPen.LineJoin = LineJoin.Round;
            rotatedVertexes.Widen(widenPen);
            this.Region = new Region(rotatedVertexes);
            this.Invalidate(this.Region);
        }

        public override void Rotate()
        {
            rotateCenter = CalculateRotateCenter(originVertexes.PathPoints);
            Matrix mat = new Matrix();
            mat.RotateAt(RotateAngle, rotateCenter);
            rotatedVertexes = originVertexes.Clone() as GraphicsPath;
            rotatedVertexes.Transform(mat);
            //重建bodyLine，使用rotatedVertexes的PathPoints作为新的bodyLine
            bodyLine.Clear();
            foreach (var pt in rotatedVertexes.PathPoints)
            {
                bodyLine.AddLast(pt);
            }
            //遍历所有points，得到minx,miny,maxx,maxy，计算出当前蛇的包围盒和坐标位置，
            float minX = bodyLine.Min(p => p.X);
            float minY = bodyLine.Min(p => p.Y);
            float maxX = bodyLine.Max(p => p.X);
            float maxY = bodyLine.Max(p => p.Y);
            Rectangle box = new Rectangle((int)minX - PenWidth / 2, (int)(minY - PenWidth / 2),
                (int)(maxX - minX + PenWidth), (int)(maxY - minY + PenWidth));
            //先把包围盒整体移动到（0，0），再把控件的Location进行反向偏移
            Point _offset = new Point((int)box.X, (int)box.Y);
            var node = bodyLine.First;
            while (node != null)
            {
                PointF oldPt = node.Value;
                node.Value = new PointF(oldPt.X - box.X, oldPt.Y - box.Y);
                node = node.Next;
            }

            currPosF = bodyLine.First.Value;
            this.Left = this.Left + _offset.X;
            this.Top = this.Top + _offset.Y;
            this.Width = box.Width;
            this.Height = box.Height;
            //使用widen方法把包围盒的区域扩大到蛇的宽度，作为控件的Region
            rotatedVertexes.Reset();
            rotatedVertexes.AddLines(bodyLine.ToArray());
            Pen widenPen = new Pen(Color.Transparent, PenWidth);
            widenPen.StartCap = LineCap.Round;
            widenPen.EndCap = LineCap.Round;
            widenPen.LineJoin = LineJoin.Round;
            rotatedVertexes.Widen(widenPen);
            this.Region = new Region(rotatedVertexes);
            this.Invalidate(this.Region);
        }

        public override void SetRegion()
        {
            //初始一条向下的直线
            int length = (int)(this.Height * ScaleFactor) - PenWidth;
            for (int i = 0; i < length; i++)
            {
                bodyLine.AddFirst(new PointF(PenWidth / 2, i + PenWidth / 2));
            }
            originVertexes.AddLines(bodyLine.ToArray());
            this.Width = PenWidth;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen widenPen = new Pen(Color.Red, PenWidth);
            widenPen.StartCap = LineCap.Round;
            widenPen.EndCap = LineCap.Round;
            widenPen.LineJoin = LineJoin.Round;
            rotatedVertexes.Widen(widenPen);
            e.Graphics.DrawPath(widenPen, rotatedVertexes);
            //在最后一个点上画一个圆，表示蛇头
            PointF headPoint = bodyLine.First.Value;
            PointF secPoint = bodyLine.First.Next.Value;
            Pen pen = new Pen(Color.Blue, 4);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;
            e.Graphics.DrawLine(pen, headPoint, secPoint);
            pen.Dispose();
            widenPen.Dispose();
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

        }
    }
}
