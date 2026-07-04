using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 敌人
{
    public static class PointFExtensions
    {
        public static float DistanceTo(this PointF self, PointF other)
        {
            float dx = other.X - self.X;
            float dy = other.Y - self.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static float DistanceSqTo(this PointF self, PointF other)
        {
            float dx = other.X - self.X;
            float dy = other.Y - self.Y;
            return dx * dx + dy * dy;
        }

        public static SizeF Minus(this PointF self,PointF other)
        {
            return new SizeF(self.X - other.X,self.Y - other.Y);
        }

        public static SizeF ToSizeF(this PointF self)
        {
            return new SizeF(self.X,self.Y);
        }
    }
}
