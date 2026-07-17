using CoreLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 贪吃蛇
{
    public class TextObject : BaseObject
    {
        public string Text { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawString(Text, new Font("Arial", 12), Brushes.Black, new PointF(0, 0));
        }
    }
}
