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
    public abstract class MoveBaseObject : BaseObject, IMove
    {
        public static float DefaultMoveSpeed { get; } = 4;

        protected PointF rotateCenter;
        public PointF RotateCenter { get => rotateCenter; }
        public float ScaleFactor { get; set; } = 1;
        protected float rotateAngle;
        public virtual float RotateAngle
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
        public float MoveSpeed { get; set; } = DefaultMoveSpeed;
        protected float moveDirection;
        public virtual float MoveDirection
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
        protected GraphicsPath rotatedVertexes = new GraphicsPath();
        public GraphicsPath RotatedVertexes => rotatedVertexes;
        protected GraphicsPath originVertexes = new GraphicsPath();
        public GraphicsPath OriginVertexes => originVertexes;
        protected PointF currPosF;
        public PointF CurrPosF => currPosF;
        public bool MoveEnabled { get; set; } = true;
        public virtual void MoveOneStep()
        {
            if (!MoveEnabled)
                return;
            
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintRegionInner(e, rotatedVertexes);
        }

        public abstract void SetRegion();
        protected abstract void PaintRegionInner(PaintEventArgs e, GraphicsPath rotatedRegion);
    }
}
