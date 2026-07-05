using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreLib
{
    public class BaseObject : Control
    {
        public BaseObject()
        {
            // 开启窗体双缓冲全套
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer,
                true);
            UpdateStyles(); // 立即生效样式

            this.Width = this.Height = 100;
        }
    }
}
