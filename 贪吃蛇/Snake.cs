using CoreLib;
using CoreLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace 贪吃蛇
{
    public class Snake : MoveBaseObject
    {
        public const int StepPixels = 8;

        private int PenWidth = 20;
        private LinkedList<PointF> bodyLine = new LinkedList<PointF>();
        private Timer moveTimer = new Timer();

        public Snake()
        {
            moveTimer.Interval = 100;
            moveTimer.Tick += MoveAction;
        }

        public void StartMove() => moveTimer.Start();

        int time = 0;
        private void MoveAction(object sender, EventArgs e)
        {
            switch (time)
            {
                case 1:MoveDirection = 90; break;
                case 10:MoveDirection = 180; break;
                case 20:MoveDirection = 270;break;
                case 30 :MoveDirection = 0;break;
                default:
                    break;
            }
            MoveOneStep();
            time++;
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
            originVertexes.Reset();
            originVertexes.AddLines(bodyLine.ToArray());
            Pen widenPen = new Pen(Color.Transparent, PenWidth);
            widenPen.StartCap = LineCap.Round;
            widenPen.EndCap = LineCap.Round;
            widenPen.LineJoin = LineJoin.Round;
            originVertexes.Widen(widenPen);
            this.Region = new Region(originVertexes);
            this.Invalidate(this.Region);
        }

        public override void Rotate()
        {
            
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
            //currPosF意为头在region里的相对位置
            currPosF = bodyLine.First.Value;
            ////遍历所有points，得到minx,miny,maxx,maxy，计算出当前蛇的包围盒和坐标位置，
            //float minX = bodyLine.Min(p => p.X);
            //float minY = bodyLine.Min(p => p.Y);
            //float maxX = bodyLine.Max(p => p.X);
            //float maxY = bodyLine.Max(p => p.Y);
            //Rectangle box = new Rectangle((int)minX - PenWidth / 2, (int)(minY - PenWidth / 2),
            //    (int)(maxX - minX + PenWidth), (int)(maxY - minY + PenWidth));
            ////先把包围盒的位置移动到（0，0），再把控件的Location进行反向偏移
            //Point _offset = new Point((int)box.X, (int)box.Y);
            //box.Offset(-box.X, -box.Y);
            //this.Left = this.Left + _offset.X;
            //this.Top = this.Top + _offset.Y;
            this.Width = PenWidth;
            //使用widen方法把包围盒的区域扩大到蛇的宽度，作为控件的Region
            originVertexes.Reset();
            originVertexes.AddLines(bodyLine.ToArray());
            Pen widenPen = new Pen(Color.Transparent, PenWidth);
            widenPen.StartCap = LineCap.Round;
            widenPen.EndCap = LineCap.Round;
            widenPen.LineJoin = LineJoin.Round;
            originVertexes.Widen(widenPen);
            this.Region = new Region(originVertexes);
            this.Invalidate(this.Region);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen widenPen = new Pen(Color.Red, PenWidth);
            widenPen.StartCap = LineCap.Round;
            widenPen.EndCap = LineCap.Round;
            widenPen.LineJoin = LineJoin.Round;
            originVertexes.Widen(widenPen);
            e.Graphics.DrawPath(widenPen, originVertexes);
            widenPen.Dispose();
        }

        protected override void PaintRegionInner(PaintEventArgs e, GraphicsPath rotatedRegion)
        {

        }
    }
}
