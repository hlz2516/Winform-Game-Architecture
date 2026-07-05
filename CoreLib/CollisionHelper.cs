using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CoreLib
{
    public class CollisionHelper
    {
        /// <summary>按控件真实Region精确碰撞检测（支持异形裁剪）</summary>
        public static bool CheckRegionCollision(Control c1, Control c2)
        {
            if (c1 == null || c2 == null || c1 == c2) return false;

            // 无自定义Region则回退到Bounds矩形
            Region r1 = c1.Region ?? new Region(c1.Bounds);
            Region r2 = c2.Region ?? new Region(c2.Bounds);

            // 复制区域避免修改原控件Region
            using (Region reg1 = r1.Clone())
            using (Region reg2 = r2.Clone())
            {
                // 偏移Region到统一屏幕坐标
                Matrix m1 = new Matrix();
                m1.Translate(c1.Left, c1.Top);
                reg1.Transform(m1);

                Matrix m2 = new Matrix();
                m2.Translate(c2.Left, c2.Top);
                reg2.Transform(m2);

                // 判断两个区域是否存在交集
                reg1.Intersect(reg2);
                return !reg1.IsEmpty(Graphics.FromHwnd(c1.Handle));
            }
        }

        public static bool CheckRegionsCollision(Control c1,IEnumerable<Control> ctrls,out List<Control> collisionCtrls)
        {
            collisionCtrls = null;

            if (c1 is null || ctrls is null)
            {
                return false;
            }

            if (ctrls.Count() == 0 || ctrls.Any(x=>x is null || x.Region is null))
            {
                return false;
            }

            collisionCtrls = new List<Control>();
            foreach (Control ctrl in ctrls)
            {
                Region reg1 = c1.Region.Clone();
                Matrix m1 = new Matrix();
                m1.Translate(c1.Left, c1.Top);
                reg1.Transform(m1);

                var tmpReg = ctrl.Region.Clone();
                Matrix m2 = new Matrix();
                m2.Translate(ctrl.Left, ctrl.Top);
                tmpReg.Transform(m2);
                reg1.Intersect(tmpReg);
                if (!reg1.IsEmpty(Graphics.FromHwnd(c1.Handle)))
                {
                    collisionCtrls.Add(ctrl);
                }
            }

            return collisionCtrls.Count > 0;
        }
    }
}
