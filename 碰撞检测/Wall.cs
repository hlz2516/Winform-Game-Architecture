using CoreLib;
using CoreLib.Interfaces;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace 碰撞检测
{
    public class Wall : BaseObject,IRegion
    {
        private GraphicsPath originVertexes;
        public GraphicsPath OriginVertexes => originVertexes;

        public float ScaleInfactor { get; set; } = 1;

        public Wall()
        {
            originVertexes = new GraphicsPath();
        }

        public void SetRegion()
        {
            originVertexes.AddRectangle(new System.Drawing.RectangleF(0, 0, this.Width* ScaleInfactor, this.Height* ScaleInfactor));
            this.Region = new Region(originVertexes);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Region == null)
                return;
            var g = e.Graphics;
            using (var pen = new Pen(Color.Blue, 2))
            {
                g.DrawPath(pen, originVertexes);
            } 
        }
    }
}
