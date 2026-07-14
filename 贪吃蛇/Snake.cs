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
        List<PointF> bodyLine;
        private GraphicsPath originVertexes;
        public GraphicsPath OriginVertexes => originVertexes;
        public float ScaleFactor { get; set; } = 1;
        private int PenWidth = 20;

        public Snake()
        {
            bodyLine = new List<PointF>();
            originVertexes = new GraphicsPath();
        }

        public override void MoveOneStep()
        {
            //下一步移动逻辑里自带转向，根据当前方向和速度计算下一步头的位置，尾部减少一段长度，形成移动的效果

            //遍历所有points，得到minx,miny,maxx,maxy，计算出当前蛇的包围盒和坐标位置，

            //更新蛇的工作区大小为包围盒大小+PenWidth*2

            //移动控件的坐标位置到包围盒的左上角
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
                bodyLine.Add(new PointF(this.Width * ScaleFactor / 2, i + PenWidth / 2));
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
