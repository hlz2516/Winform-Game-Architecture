using CoreLib;
using CoreLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 贪吃蛇
{
    public class Snake : MoveBaseObject
    {
        private int PenWidth = 20;
        LinkedList<PointF> bodyLine = new LinkedList<PointF>();
        public Snake()
        {

        }

        public override void MoveOneStep()
        {
            //下一步移动逻辑里自带转向，根据当前方向和速度计算下一步头的位置，如果下一步位置会跟墙碰撞，则不移动
            //如果确定移动，尾部减少一段长度，形成移动的效果
            for (int i = 1; i <= MoveSpeed; i++)
            {
                var offset = TrigonometricFunctions.GetPointByAngle(0, 0, i, MoveDirection);
                var nextPosF = PointF.Add(currPosF, offset.ToSizeF());
                var realPos = new Point((int)Math.Round(nextPosF.X), (int)Math.Round(nextPosF.Y));
                bodyLine.AddFirst(realPos);
                bodyLine.RemoveLast();
                currPosF = nextPosF;
            }
            //遍历所有points，得到minx,miny,maxx,maxy，计算出当前蛇的包围盒和坐标位置，
            float minX = bodyLine.Min(p => p.X);
            float minY = bodyLine.Min(p => p.Y);
            float maxX = bodyLine.Max(p => p.X);
            float maxY = bodyLine.Max(p => p.Y);
            Rectangle box = new Rectangle((int)minX - PenWidth / 2, (int)(minY - PenWidth / 2),
                (int)(maxX - minX + PenWidth), (int)(maxY - minY + PenWidth));
            //先把包围盒的位置移动到（0，0），再把控件的Location进行反向偏移
            Point _offset = new Point((int)box.X, (int)box.Y);
            box.Offset(-box.X, -box.Y);
            this.Left = this.Left + _offset.X;
            this.Top = this.Top + _offset.Y;
            this.Width = box.Width;
            this.Height = box.Height;
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
                bodyLine.AddFirst(new PointF(this.Width * ScaleFactor / 2, i + PenWidth / 2));
            }
            originVertexes.AddLines(bodyLine.ToArray());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintRegionInner(e, rotatedVertexes);
        }

        protected override void PaintRegionInner(PaintEventArgs e, GraphicsPath rotatedRegion)
        {

        }
    }
}
